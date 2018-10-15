﻿//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Network
{
    internal partial class NetworkManager
    {
        private partial class NetworkChannel
        {
            private sealed class ReceiveState
            {
                public const int HeaderLength = 2;

                private readonly int m_BufferSize;
                private readonly byte[] m_Buffer;
                private int m_Length;
                private int m_ReceivedLength;

                public ReceiveState(int bufferSize)
                {
                    m_BufferSize = bufferSize;
                    m_Buffer = new byte[bufferSize];
                    Reset();
                }

                public int BufferSize
                {
                    get
                    {
                        return m_BufferSize;
                    }
                }

                public int Length
                {
                    get
                    {
                        return m_Length;
                    }
                    set
                    {
                        m_Length = value;
                    }
                }

                public int ReceivedLength
                {
                    get
                    {
                        return m_ReceivedLength;
                    }
                    set
                    {
                        m_ReceivedLength = value;
                    }
                }

                public void Reset()
                {
                    Length = HeaderLength;
                    ReceivedLength = 0;
                }

                public byte[] GetBuffer()
                {
                    return m_Buffer;
                }
            }
        }
    }
}
