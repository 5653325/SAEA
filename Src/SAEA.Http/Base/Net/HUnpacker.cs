﻿/****************************************************************************
*Copyright (c) 2018 yswenli All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：WENLI-PC
*公司名称：yswenli
*命名空间：SAEA.Http.Base.Net
*文件名： HCoder
*版本号： v5.0.0.1
*唯一标识：1c78283d-e311-4d8d-b781-395253c9454c
*当前的用户域：WENLI-PC
*创建人： yswenli
*电子邮箱：wenguoli_520@qq.com
*创建时间：2018/4/8 17:19:30
*描述：
*
*=====================================================================
*修改标记
*修改时间：2018/4/8 17:19:30
*修改人： yswenli
*版本号： v5.0.0.1
*描述：
*
*****************************************************************************/
using SAEA.Common;
using SAEA.Sockets.Interface;
using System;
using System.Collections.Generic;

namespace SAEA.Http.Base.Net
{
    class HUnpacker : IUnpacker
    {
        List<byte> _cache = new List<byte>();

        public void Unpack(byte[] data, Action<ISocketProtocal> unpackCallback, Action<DateTime> onHeart = null, Action<byte[]> onFile = null)
        {

        }

        public void GetRequest(string id, byte[] data, Action<HttpMessage> onUnpackage)
        {
            _cache.AddRange(data);

            var buffer = _cache.ToArray();

            if (RequestDataReader.Analysis(buffer, out HttpMessage httpMessage))
            {
                httpMessage.ID = id;

                //post需要处理body
                if (httpMessage.Method == ConstHelper.POST)
                {
                    var contentLen = httpMessage.ContentLength;
                    var positon = httpMessage.Position;
                    var totlalLen = contentLen + positon;
                    if (buffer.Length == totlalLen)
                    {
                        RequestDataReader.AnalysisBody(buffer, httpMessage);
                        onUnpackage.Invoke(httpMessage);
                        Array.Clear(buffer, 0, buffer.Length);
                        _cache.Clear();
                    }
                }
                else
                {
                    onUnpackage.Invoke(httpMessage);
                    Array.Clear(buffer, 0, buffer.Length);
                    _cache.Clear();
                }
            }
        }


        public void Clear()
        {
            _cache.Clear();
        }
    }
}
