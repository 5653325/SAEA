﻿/****************************************************************************
*Copyright (c) 2018 yswenli All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：WENLI-PC
*公司名称：yswenli
*命名空间：SAEA.MVC
*文件名： SAEAMvcApplication
*版本号： v5.0.0.1
*唯一标识：85030224-1d7f-4fc0-8e65-f4b6144c6a46
*当前的用户域：WENLI-PC
*创建人： yswenli
*电子邮箱：wenguoli_520@qq.com
*创建时间：2018/4/10 13:59:33
*描述：
*
*=====================================================================
*修改标记
*修改时间：2018/4/10 13:59:33
*修改人： yswenli
*版本号： v5.0.0.1
*描述：
*
*****************************************************************************/
using SAEA.Http;
using System;

namespace SAEA.MVC
{
    /// <summary>
    /// SAEA.MVC应用程序
    /// </summary>
    public partial class SAEAMvcApplication
    {
        WebHost webHost;

        internal AreaCollection AreaCollection { get; private set; } = new AreaCollection();

        /// <summary>
        /// 是否处于运行中
        /// </summary>
        public bool Running { get; set; } = false;

        /// <summary>
        /// 构建mvc容器
        /// </summary>
        /// <param name="mvcConfig"></param>
        public SAEAMvcApplication(SAEAMvcApplicationConfig mvcConfig) : this(mvcConfig.Root, mvcConfig.Port, mvcConfig.IsStaticsCached, mvcConfig.IsZiped, mvcConfig.BufferSize, mvcConfig.Count, mvcConfig.IsDebug)
        {
            webHost.WebConfig.HomePage = mvcConfig.DefaultPage;
        }

        /// <summary>
        /// 构建mvc容器
        /// </summary>
        /// <param name="root">根目录</param>
        /// <param name="port">监听端口</param>
        /// <param name="isStaticsCached">是否启用静态缓存</param>
        /// <param name="isZiped">是压启用内容压缩</param>
        /// <param name="bufferSize">http处理数据缓存大小</param>
        /// <param name="count">http连接数上限</param>
        /// <param name="isDebug">调试模式</param>
        public SAEAMvcApplication(string root = "wwwroot", int port = 39654, bool isStaticsCached = true, bool isZiped = false, int bufferSize = 1024 * 10, int count = 10000, bool isDebug = false, string controllerNameSpace="")
        {
            try
            {
                if (string.IsNullOrEmpty(controllerNameSpace))
                    AreaCollection.RegistAll();
                else
                    AreaCollection.RegistAll(controllerNameSpace);
            }
            catch (Exception ex)
            {
                throw new Exception("当前代码无任何Controller或者不符合MVC 命名规范！ err:" + ex.Message);
            }

            webHost = new WebHost(typeof(HttpContext), root, port, isStaticsCached, isZiped, bufferSize, count, 120 * 1000, isDebug);


            webHost.RouteParam = AreaCollection.RouteTable;
        }

        /// <summary>
        /// 设置默认路由地址
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        public void SetDefault(string controllerName, string actionName)
        {
            webHost.WebConfig.DefaultRoute = new SAEA.Common.NameValueItem() { Name = controllerName, Value = actionName };
        }

        /// <summary>
        /// 设置默认首页
        /// </summary>
        /// <param name="defaultPage"></param>
        public void SetDefault(string defaultPage)
        {
            webHost.WebConfig.HomePage = "/" + defaultPage;
        }

        /// <summary>
        /// 设置禁止访问列表
        /// </summary>
        /// <param name="list"></param>
        public void SetForbiddenAccessList(params string[] list)
        {
            webHost.WebConfig.SetForbiddenAccessList(list);
        }

        /// <summary>
        /// 启动MVC服务
        /// </summary>
        public void Start()
        {
            try
            {
                webHost.Start();
                this.Running = true;
            }
            catch (Exception ex)
            {
                throw new Exception("当前端口已被其他程序占用，请更换端口再做尝试！ err:" + ex.Message);
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            try
            {
                webHost.Stop();
                this.Running = false;
            }
            catch (Exception ex)
            {
                throw new Exception("关闭SAEA.MVCServer失败 err:" + ex.Message);
            }
        }
    }
}
