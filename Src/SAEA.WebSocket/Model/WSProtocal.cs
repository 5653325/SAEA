﻿/****************************************************************************
*Copyright (c) 2018 yswenli All Rights Reserved.
*CLR版本： 2.1.4
*机器名称：WENLI-PC
*公司名称：wenli
*命名空间：SAEA.WebSocket
*文件名： Class1
*版本号： v5.0.0.1
*唯一标识：ef84e44b-6fa2-432e-90a2-003ebd059303
*当前的用户域：WENLI-PC
*创建人： yswenli
*电子邮箱：wenguoli_520@qq.com
*创建时间：2018/3/1 15:54:21
*描述：
*
*=====================================================================
*修改标记
*修改时间：2018/3/1 15:54:21
*修改人： yswenli
*版本号： v5.0.0.1
*描述：
*
*****************************************************************************/

using SAEA.Common;
using SAEA.Sockets.Interface;
using SAEA.WebSocket.Type;
using System.IO;

namespace SAEA.WebSocket.Model
{
    public class WSProtocal : ISocketProtocal
    {
        int _mask = RandomHelper.GetInt(1);

        public long BodyLength { get; set; }
        public byte[] Content { get; set; }
        public byte Type { get; set; }

        public WSProtocal(byte type, byte[] content)
        {
            this.Type = type;
            if (content != null)
                this.BodyLength = content.Length;
            else
                this.BodyLength = 0;
            this.Content = content;
        }

        public WSProtocal(WSProtocalType type, byte[] content)
        {
            this.Type = (byte)type;
            if (content != null)
                this.BodyLength = content.Length;
            else
                this.BodyLength = 0;
            this.Content = content;
        }


        /// <summary>
        /// 将当前实体转换成websocket所需的结构
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            int _payloadLength;

            byte[] _extPayloadLength;

            ulong len = (ulong)this.BodyLength;

            if (len < 126)
            {
                _payloadLength = (byte)len;
                _extPayloadLength = new byte[0];
            }
            else if (len < 0x010000)
            {
                _payloadLength = (byte)126;
                _extPayloadLength = ((ushort)len).InternalToByteArray(EndianOrder.Big);
            }
            else
            {
                _payloadLength = (byte)127;
                _extPayloadLength = len.InternalToByteArray(EndianOrder.Big);
            }

            using (var buff = new MemoryStream())
            {
                var header = (int)0x1;
                header = (header << 1) + (byte)0x0;
                header = (header << 1) + (byte)0x0;
                header = (header << 1) + (byte)0x0;
                header = (header << 4) + this.Type;
                header = (header << 1) + (byte)0x1;
                header = (header << 7) + (byte)_payloadLength;
                buff.Write(((ushort)header).InternalToByteArray(EndianOrder.Big), 0, 2);

                //mask
                var maskBytes = _mask.ToBytes();
                buff.Write(maskBytes, 0, 4);


                if (_payloadLength > 125)
                    buff.Write(_extPayloadLength, 0, _payloadLength == 126 ? 2 : 8);


                if (_payloadLength > 0)
                {
                    buff.Write(this.Content, 0, (int)this.BodyLength);
                }

                buff.Flush();
                return buff.ToArray();
            }
        }
    }
}
