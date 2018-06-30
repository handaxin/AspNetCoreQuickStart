﻿using Microsoft.IdentityModel.Tokens;
using System;

namespace MyCompany.MyProject.Web.Application
{
    public class AppSettings
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public ConnectionStrings ConnectionStrings { get; set; }

        /// <summary>
        /// 定义本程序的日志事件Id
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// cookie设置
        /// </summary>
        public Cookie Cookie { get; set; }

    }

    public class ConnectionStrings
    {
        /// <summary>
        /// 默认连接
        /// </summary>
        public string Default { get; set; }

        /// <summary>
        /// 默认查询连接(读写分离)
        /// </summary>
        public string DefaultQuery { get; set; }
    }

    public class Cookie
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public TimeSpan ExpireTimeSpan { get; set; }
    }
}