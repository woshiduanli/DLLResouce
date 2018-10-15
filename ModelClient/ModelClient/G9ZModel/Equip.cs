using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Model
{
    /// <summary>
    /// 装备定义，原ItemResInfo
    /// </summary>
    public class Equip : ScriptableObject 
    {
        /// <summary>
        /// 装备名称
        /// </summary>
        public string EquipName = string.Empty;
        /// <summary>
        /// 模型路径
        /// </summary>
        [AssetToFilePath("模型",".model")]
        public string ModelPath = string.Empty;

        /// <summary>
        /// 高模型路径
        /// </summary>
        [AssetToFilePath("高模型", ".model")]
        public string HighModelPath = string.Empty;

        [CommonAttribute("低模蒙皮信息")]
        public string[] Bones;

        [CommonAttribute("高模蒙皮信息")]
        public string[] HighBones;

        [CommonAttribute("蒙皮根骨骼")]
        public string RootBone = "Bip01 Pelvis";

        [CommonAttribute("模型父节点")]
        public string ParentBone = "Bip01";

        [IntPopup("占位", new string[] { "身体", "备用", "右手武器", "头", "脸", "翅膀" }, new int[] { 0, 1, 2, 3, 4, 5, })]
        public int Mask = 0;

        public EffectConfig[] Effects = new EffectConfig[0];
    }
}