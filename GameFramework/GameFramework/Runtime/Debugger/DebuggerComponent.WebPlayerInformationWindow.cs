//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public partial class DebuggerComponent
    {
        private sealed class WebPlayerInformationWindow : ScrollableDebuggerWindowBase
        {
            protected override void OnDrawScrollableWindow()
            {
                GUILayout.Label("<b>Web Player Information</b>");
                GUILayout.BeginVertical("box");
                {
                    DrawItem("Absolute URL:", Application.absoluteURL);
                    DrawItem("Streamed Bytes:", Application.streamedBytes.ToString());
#if !UNITY_5_5_OR_NEWER
                    DrawItem("Web Security Enabled:", Application.webSecurityEnabled.ToString());
                    DrawItem("Web Security Host URL:", Application.webSecurityHostUrl.ToString());
#endif
                }
                GUILayout.EndVertical();
            }
        }
    }
}
