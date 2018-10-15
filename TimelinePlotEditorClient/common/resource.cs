using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using System.Reflection;
using UnityEngine;

public class ResourceObject
{
    // table_name对应的Type（不用每次去GetType）
    public static Dictionary<string, Type> type_dic = new Dictionary<string, Type>();

    private Dictionary<string, string> object_;
    private string table_type;

    public ResourceObject(string table_name)
    {
        object_ = new Dictionary<string, string>();
        table_type = table_name + "Reference";

        if (!type_dic.ContainsKey(table_type))
        {
            Type type = Type.GetType(table_type);
            type_dic.Add(table_type, type);
        }
    }

    public void Clear()
    {
        object_.Clear();
    }

    public void AddProperty(string col_name, string col_value)
    {
        object_.Add(col_name.ToLower(), col_value);
    }

    public bool ContainsKey(string col_name)
    {
        return object_.ContainsKey(col_name.ToLower());
    }

    public string GetPropertyValue(string col_name)
    {
        col_name = col_name.ToLower();
        if (!object_.ContainsKey(col_name))
            return "";

        return object_[col_name];
    }

    public int GetIntPropertyValue(string col_name)
    {
        col_name = col_name.ToLower();
        if (!object_.ContainsKey(col_name))
            return 0;

        int resule = 0;
        int.TryParse(object_[col_name], out resule);
        return resule;
    }

    public bool WriteToStream(StringBuilder sb)
    {
        Type obj_type = type_dic[table_type];
        foreach (FieldInfo field_info in obj_type.GetFields())
        {
            string col_name = field_info.Name.ToLower();

            if (!object_.ContainsKey(col_name))
                continue;

            string value = object_[col_name];
            switch (field_info.FieldType.Name.ToLower())
            {
                case "int32":
                case "int16":
                case "byte":
                case "single":
                case "boolean":
                    value = ResourceUtil.ParseString2Mem(value);
                    int value_int = Convert.ToInt32(value);
                    if (value_int == 0)
                        continue;
                    value = value_int.ToString();
                    break;
                default:
                    if (string.IsNullOrEmpty(value))
                        continue;
                    break;
            }

            sb.Append(string.Format("{0}\t{1}\t", field_info.Name, ResourceUtil.ParseString2File(value)));
        }

        sb.Append("\r\n");
        return true;
    }

    //add by wenzy
    public bool WriteToStreamForLua(StringBuilder sb)
    {
        Type obj_type = type_dic[table_type];
        sb.Append("{");
        foreach (FieldInfo field_info in obj_type.GetFields())
        {
            string col_name = field_info.Name.ToLower();

            if (!object_.ContainsKey(col_name))
                continue;

            string value = object_[col_name];
            int value_int = 0;
            switch (field_info.FieldType.Name.ToLower())
            {
                case "int32":
                case "int16":
                case "byte":
                case "single":  // is float
                    value = ResourceUtil.ParseString2Mem(value);
                    value_int = Convert.ToInt32(value);
                    value = value_int.ToString();
                    sb.Append(string.Format("{0}={1},\t", field_info.Name, ResourceUtil.ParseString2File(value)));
                    break;
                case "boolean":
                    value = ResourceUtil.ParseString2Mem(value);
                    value_int = Convert.ToInt32(value);
                    if (value_int == 0)
                    {
                        value = "false";
                    }
                    else
                    {
                        value = "true";
                    }
                    sb.Append(string.Format("{0}={1},\t", field_info.Name, ResourceUtil.ParseString2File(value)));
                    break;
                default:
                    if (string.IsNullOrEmpty(value))
                    {
                        value = "";
                    }

                    string tmpValueStr = ResourceUtil.ParseString2File(value);
                    // 下面两个地方，由于简繁体转换工具的缺陷，导致无法识别字串常量
                    // 所以把它改成等效的表示法
                    tmpValueStr = tmpValueStr.Replace("\\", @"\\"); // "\\\\");
                    tmpValueStr = tmpValueStr.Replace("\"", "\x5c\x22"); //     "\\\"");
                    value = string.Format("\"{0}\"", tmpValueStr);
                    sb.Append(string.Format("{0}={1},\t", field_info.Name, value));

                    break;
            }
        }
        sb.Append("},");
        sb.Append("\r\n");
        return true;
    }

}

public class ResourceUtilWrite : ResourceUtil
{

    public ResourceUtilWrite(string path, string table_name) : base(path, table_name, false) { ReaderAll(); }
    public ResourceUtilWrite(string path, string table_name, int editor_version) : base(path, table_name, false) { ReaderAll(); }

    public bool AddData(object obj)
    {
        Type obj_type = obj.GetType();
        ResourceObject obj_res = new ResourceObject(table_name);

        foreach (FieldInfo field_info in obj_type.GetFields())
        {
            object obj_value = field_info.GetValue(obj);
            if (obj_value == null)
                continue;

            switch (field_info.FieldType.Name)
            {
                case "Boolean":
                    obj_value = ((Boolean)obj_value) == true ? 1 : 0;
                    break;
            }

            obj_res.AddProperty(field_info.Name, obj_value.ToString());
        }

        return Save(obj_res.GetIntPropertyValue("id"), obj_res);
    }

    public bool Save(int id, ResourceObject res_obj)
    {
        if (objs_.ContainsKey(id))
            return false;

        objs_.Add(id, res_obj);
        return true;
    }

    // 条件删除
    public bool DeleteData(string col_name, int col_value)
    {
        col_name = col_name.ToLower();
        int[] keys = new int[objs_.Keys.Count];
        objs_.Keys.CopyTo(keys, 0);

        foreach (int one_key in keys)
        {
            ResourceObject res_obj = objs_[one_key];
            if (res_obj.GetIntPropertyValue(col_name) != col_value)
                continue;

            objs_.Remove(res_obj.GetIntPropertyValue("id"));
        }

        return true;
    }

    public void Delete(int id)
    {
        objs_.Remove(id);
    }

    public bool Update(int id, int id_old, ResourceObject res_obj)
    {
        if (id != id_old)
        {
            objs_.Remove(id_old);
            return Save(id, res_obj);
        }

        if (!objs_.ContainsKey(id))
            return false;

        objs_[id] = res_obj;
        return true;
    }

    public bool SaveToFile(string path, int global_version)
    {

        StringBuilder sb = new StringBuilder();
        sb.Append(string.Format("version\t{0}\r\n", global_version));

        int[] ids = new int[objs_.Keys.Count];
        objs_.Keys.CopyTo(ids, 0);
        Array.Sort(ids);
        foreach (int one_id in ids)
        {
            ResourceObject obj = objs_[one_id];
            if (!obj.WriteToStream(sb))
                return false;
        }
        Debug.Log(GetResouceFileName(path, table_name));
        FileStream file_stream = new FileStream(GetResouceFileName(path, table_name), FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
  
        using (StreamWriter stream_write = new StreamWriter(file_stream, Encoding.UTF8))
        {
            Debug.Log("1111111111111111");
            stream_write.Write(sb.ToString());
        }

        return true;
    }

    // add by wenzy
    public bool SaveToLuaFile(string path, int global_version)
    {
        StringBuilder sb = new StringBuilder();
        //sb.Append( string.Format( "-- version\t{0}\r\n", global_version ) );
        sb.Append(string.Format("{0}=", table_name));
        sb.Append("{\r\n");

        int[] ids = new int[objs_.Keys.Count];
        objs_.Keys.CopyTo(ids, 0);
        Array.Sort(ids);
        foreach (int one_id in ids)
        {
            sb.Append(string.Format("[{0}]=", one_id.ToString()));
            ResourceObject obj = objs_[one_id];
            if (!obj.WriteToStreamForLua(sb))
                return false;
        }

        sb.Append("}");

        string resFilename = string.Format("{0}/{1}.lua", path, table_name);

        FileStream file_stream = new FileStream(resFilename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
        using (StreamWriter stream_write = new StreamWriter(file_stream, Encoding.UTF8))
        {
            stream_write.Write(sb.ToString());
        }

        return true;
    }
}

/// <summary>
/// 把所有数据读到内存中
/// </summary>
public class ResourceUtilReader2Mem : ResourceUtil
{
    //private int edition_type;
    public ResourceUtilReader2Mem(string path, string table_name)
        : base(path, table_name, false)
    {
        //this.edition_type = edition_type;
        ReaderAll();
    }

    public string GetValueByCol(int id, string col_name)
    {
        if (!objs_.ContainsKey(id))
            return "";

        return objs_[id].GetPropertyValue(col_name);
    }

    public int GetIntValueByCol(int id, string col_name)
    {
        string value_str = GetValueByCol(id, col_name);
        int value_int = 0;
        int.TryParse(value_str, out value_int);
        return value_int;
    }
}


/// <summary>
/// 该类一行一行读取文本
/// </summary>
public class ResourceUtilReader : ResourceUtil
{

    protected ResourceObject current_object;
    private int edition_type;

    private Dictionary<int, int> check_id;
    private int error_id = 0;

    public ResourceUtilReader(byte[] content, string table_name, int edition_type, bool crypto)
        : base(content, table_name, crypto)
    {
        this.current_object = new ResourceObject(table_name);
        this.edition_type = edition_type;
        check_id = new Dictionary<int, int>();
        error_id = 0;
    }

    public ResourceUtilReader(string path, string table_name, int edition_type, bool crypto)
        : base(path, table_name, crypto)
    {
        this.current_object = new ResourceObject(table_name);
        this.edition_type = edition_type;
        check_id = new Dictionary<int, int>();
        error_id = 0;
    }

    public int GetErrorId()
    {
        return error_id;
    }

    private bool ChekcId()
    {
        int id = GetIntValueByCol("id");
        if (check_id.ContainsKey(id))
        {
            error_id = id;
            return false;
        }

        check_id.Add(id, 1);
        return true;
    }

    // 采用了一行一行的读取方式
    public bool GetNextLine()
    {
        if (!base.GetNextLine(current_object))
            return false;

        return true;
        if (!ChekcId())
            return false;

        int current_editor = GetIntValueByCol("EditionType");
        if (EditionType.ALL == this.edition_type)
            return true;

        if (current_editor != EditionType.COMMON && current_editor != this.edition_type)
            return GetNextLine();

        return true;
    }

    public string GetValueByCol(string col_name)
    {
        return GetValueByCol(current_object, col_name);
    }

    public int GetIntValueByCol(string col_name)
    {
        return GetIntValueByCol(current_object, col_name);
    }
}

public class ResourceUtil : IDisposable
{
    protected string table_name;
    protected StreamReader stream_reader;
    protected int version = 0;
    protected Dictionary<int, ResourceObject> objs_ = new Dictionary<int, ResourceObject>();
    protected DataCrypto dc = new DataCrypto();
    protected bool crypto = false;

    public ResourceUtil(byte[] content, string table_name, bool crypto)
    {
        this.crypto = crypto;
        this.table_name = table_name;
        stream_reader = new StreamReader(new MemoryStream(content), Encoding.UTF8);
        Init();
    }

    public ResourceUtil(string path, string table_name, bool crypto)
    {
        this.crypto = crypto;
        this.table_name = table_name;
        string filepath=Path.GetFullPath(GetResouceFileName(path, table_name));
        string[] files = Directory.GetFiles(path, "*.txt");
        bool needcreateFile=true;
         foreach (string file in files)
         {
             if (Path.GetFullPath(file) == filepath)
             {
                 needcreateFile = false;
                 break;
             }
         }
        FileStream file_stream;
        if (needcreateFile)
        {
            file_stream = File.Create(filepath);
        }
        else
        {
            file_stream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        }
        
        stream_reader = new StreamReader(file_stream, Encoding.UTF8);
        Init();
    }

    private void Init()
    {
        // 读取第一行版本信息
        string version_line = stream_reader.ReadLine();
        if (crypto)
            version_line = dc.Decrypto(version_line);

        string name = null, value = null;
        if (ParsePorperty(version_line, ref name, ref value))
        {
            if (string.Compare(name, "version", true) == 0)
                int.TryParse(value, out version);
        }
    }

    public void Dispose()
    {
        this.stream_reader.Close();
    }

    public bool CheckVersion(int global_version)
    {
        return global_version < version ? false : true;
    }

    public int GetVersion()
    {
        return version;
    }

    private string GetNextLine()
    {
        string line = null;
        while (string.IsNullOrEmpty(line))
        {
            if (stream_reader.EndOfStream)
                return null;

            line = stream_reader.ReadLine();
        }
        return line;
    }

    protected bool GetNextLine(ResourceObject current_object)
    {

        if (stream_reader.EndOfStream)
            return false;

        string line = GetNextLine();
        if (line == null)
            return false;

        if (crypto)
            line = dc.Decrypto(line);

        current_object.Clear();

        int index = 0;
        string[] property = line.Split('\t');
        while (index < (property.Length - 1))
        {
            string name = property[index++].ToLower();
            if (string.IsNullOrEmpty(name))
                break;

            string value = property[index++];
            if (current_object.ContainsKey(name))
                throw (new Exception(string.Format("数据有错，有相同属性: name={0}, line={1}", name, line)));

            current_object.AddProperty(name, value);
        }

        return true;
    }

    protected string GetValueByCol(ResourceObject current_object, string col_name)
    {
        col_name = col_name.ToLower();
        if (!current_object.ContainsKey(col_name))
            return "";

        return ParseString2Mem(current_object.GetPropertyValue(col_name));
    }

    protected int GetIntValueByCol(ResourceObject current_object, string col_name)
    {
        string value = GetValueByCol(current_object, col_name);
        if (string.IsNullOrEmpty(value))
            return 0;

        int result;
        int.TryParse(value, out result);
        return result;
    }

    // 解释一行中的一个属性数据（返回名字，和值）
    protected bool ParsePorperty(string value_str, ref string name, ref string value)
    {
        if (string.IsNullOrEmpty(value_str))
            return false;

        string[] one_line = value_str.Split('\t');
        if (one_line.Length != 2)
            return false;

        name = one_line[0].ToLower();
        value = ParseString2Mem(one_line[1]);
        return true;
    }

    // 将数据全部读入内存
    public void ReaderAll()
    {
        ResourceObject current_object = new ResourceObject(table_name);
        bool br = GetNextLine(current_object);
        while (br)
        {
            int id = GetIntValueByCol(current_object, "id");
            objs_.Add(id, current_object);
            current_object = new ResourceObject(table_name);
            br = GetNextLine(current_object);
        }
    }

    public static string ParseString2Mem(string line)
    {
        line = line.Replace("#\\r", "\r");
        line = line.Replace("#\\n", "\n");
        line = line.Replace("#\\t", "\t");
        return line;
    }

    public static string ParseString2File(string line)
    {

        line = line.Replace("\r", "#\\r");
        line = line.Replace("\n", "#\\n");
        line = line.Replace("\t", "#\\t");

        // 去掉字符串最后的转意符
        string[] keys = new string[] { "#\\r", "#\\n", "#\\t" };
        for (int i = 0; i < keys.Length; i++)
        {
            if (line.Length - keys[i].Length <= 0)
                continue;

            if (line.LastIndexOf(keys[i]) != line.Length - keys[i].Length)
                continue;

            line = line.Substring(0, line.Length - keys[i].Length);
        }

        return line;
    }

    protected string GetResouceFileName(string file_path, string table_name)
    {
        //加密和明文的数据文件后续名统一为.txt，陈俊，2013/01/21
        //if ( this.crypto )
        //    return string.Format( "{0}/{1}.dat", file_path, table_name );
        //else
        //    return string.Format( "{0}/{1}.txt", file_path, table_name );
        return string.Format("{0}/{1}.txt", file_path, table_name);
    }
}
