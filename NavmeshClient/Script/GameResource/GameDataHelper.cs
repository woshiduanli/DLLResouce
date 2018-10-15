using UnityEngine;
using System.Collections.Generic;

static class GameDataHelper
{
    #region º”‘ÿreference◊ ‘¥

    public static void ReloadDataFromFile(string strPath, int iEditionType, bool bCrypto)
    {
        Global.mapr_mgr.ReloadDataFromFile(strPath, iEditionType, bCrypto);
    }

    #endregion
}

