using System.Collections;
using System.IO;
using UnityEngine;


/// <summary>
///     场景异步加载控制逻辑
///     在进入游戏场景前会对在该场景出现的角色做动画预加载处理
/// </summary>
public class AsyncLevelLoader : MonoBehaviour
{
    public static string LoadedLevelName = string.Empty;
}