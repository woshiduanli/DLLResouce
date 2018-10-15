using UnityEngine;
using System.Collections.Generic;
public class ShadersList : MonoBehaviour
{
    public Shader[] list;
    private Dictionary<string, Shader> shaderDic = new Dictionary<string, Shader>();

    void Awake()
    {
        if (list == null || list.Length == 0)
            return;
        for (int i = 0; i < list.Length; i++)
        {
            Shader s = list[i];
            if (!s)
                continue;
            shaderDic[s.name] = s;
        }
    }

    public Shader GetShader(string sname)
    {
        if(shaderDic.ContainsKey(sname))
            return shaderDic[sname];
        return null;
    }
}