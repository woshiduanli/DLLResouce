using UnityEngine;
namespace Model
{
    [System.Serializable()]
    public class EffectConfig 
    {

        public bool Active = false;

        /// <summary>
        /// 特效资源文件路径
        /// </summary>
        [AssetToFilePath( "特效资源", ".go")]
        public string EffectAssetPath = string.Empty;

        [AssetToFilePath("低配特效资源", ".go")]
        public string LowEffectAssetPath = string.Empty;

        /// <summary>
        /// 绑定的骨骼名称（无需带路径）
        /// </summary>
        public string BindBone = string.Empty;

        /// <summary>
        /// 位置（Local）
        /// </summary>
        public Vector3 Position = Vector3.zero;

        /// <summary>
        /// 旋转（Local）
        /// </summary>
        public Vector3 Rotation = Vector3.zero;


        /// <summary>
        /// 缩放（Local）
        /// </summary>
        public Vector3 Scale = Vector3.one;
    }
}