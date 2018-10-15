using System;
using System.Collections.Generic;
using System.Reflection;

public partial class PropDefine {
    public virtual void LoadReference( ResourceUtilReader resource_reader ) {}
    public virtual bool LoadConfig() { return true; }
}






// 地图信息配置表
public partial class MapReference : PropDefine {
    [Editor( "ID,40" )]
    public short ID;
    [Editor( "名字,100" )]
    public string Name;
    [Editor( "地图文件名,100" )]
    public string FileName;
    [Editor( "宽,50" )]
    public int Width;
    [Editor( "高,50" )]
    public int Height;
}


[System.AttributeUsage( System.AttributeTargets.Field )]
public partial class EditorAttribute : System.Attribute {

    public enum ExportType {
        All = 0,        // 缺省的，服务器和客户端都需要的字段
        ServerOnly,
        ClientOnly
    }

    public string Comment;      // 注释
    public string Type;         // 类型
    public ExportType Export;   // 导出

    public EditorAttribute(string comment)
        : this( comment, string.Empty, ExportType.All ) { }

    public EditorAttribute(string comment, ExportType export)
        : this( comment, string.Empty, export ) { }

    public EditorAttribute(string comment, string type)
        : this( comment, type, ExportType.All ) { }

    public EditorAttribute(string comment, string type, ExportType exportType) {
        this.Comment = comment;
        this.Type = type;
        this.Export = exportType;
    }

}
