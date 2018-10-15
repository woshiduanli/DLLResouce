//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

using GameFramework;
using GameFramework.Network;
using LitJson;

namespace UnityGameFramework.Runtime
{
    public class EuPacket : Packet
    {
        // Not use this id
        public override int Id
        {
            get
            {
                return 0;
            }
        }

        public string Tag;
        public object[] Args;
    }


    /// <summary>
    /// 默认网络辅助器。
    /// </summary>
    public class DefaultNetworkHelper : NetworkHelperBase
    {
        /// <summary>
        /// 发送心跳协议包。
        /// </summary>
        /// <param name="networkChannel">网络频道。</param>
        /// <returns>是否发送心跳协议包成功。</returns>
        public override bool SendHeartBeat(INetworkChannel networkChannel)
        {
            return false;
        }

        /// <summary>
        /// 序列化协议包。
        /// </summary>
        /// <typeparam name="T">协议包类型。</typeparam>
        /// <param name="networkChannel">网络频道。</param>
        /// <param name="destination">要序列化的目标流。</param>
        /// <param name="packet">要序列化的协议包。</param>
        public override void Serialize<T>(INetworkChannel networkChannel, Stream destination, T packet)
        {
            EuPacket ep = packet as EuPacket;

            if (ep != null)
            {
                // Avoid create a new array
                string args = JsonMapper.ToJson(ep.Args);
                string json;
                if (ep.Args.Length == 0)
                    json = string.Concat("[\"", ep.Tag, "\"]");
                else
                    json = string.Concat("[\"", ep.Tag, "\",", args.Substring(1));
                byte[] bytes = Encoding.UTF8.GetBytes(json);
                destination.Write(bytes, 0, bytes.Length);
            }
        }

        /// <summary>
        /// 反序列化协议包。
        /// </summary>
        /// <param name="networkChannel">网络频道。</param>
        /// <param name="source">要反序列化的来源流。</param>
        /// <param name="customErrorData">用户自定义错误数据。</param>
        /// <returns>反序列化后的协议包。</returns>
        public override Packet Deserialize(INetworkChannel networkChannel, Stream source, out object customErrorData)
        {
            EuPacket p = new EuPacket();

            StreamReader sr = new StreamReader(source);
            JsonData data = JsonMapper.ToObject(sr);
            p.Tag = data[0].ToString();
            p.Args = data.Cast<object>().Skip(1).ToArray();

            customErrorData = null;
            return p;
        }
    }
}
