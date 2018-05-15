﻿using ApplicationCore.SharedKernel;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.Application;

namespace Web.Auth
{
    public class JwtRefreshFilter : IAsyncResultFilter
    {
        readonly JwtHandler _jwtHandler;
        readonly IAppLogger<JwtRefreshFilter> _appLogger;
        readonly Jwt _jwt;

        public JwtRefreshFilter(JwtHandler jwtHandler, IAppLogger<JwtRefreshFilter> appLogger, IOptions<Jwt> options)
        {
            _jwtHandler = jwtHandler;
            _appLogger = appLogger;
            _jwt = options.Value;
        }
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var user = context.HttpContext.User;
            if (_jwt.AutoRefresh) RefreshJwtToken(user, context);
            await next();
        }

        void RefreshJwtToken(ClaimsPrincipal user, ResultExecutingContext context)
        {
            if (!user.Identity.IsAuthenticated) return;
            string refresh = user.FindFirstValue(JwtHandler.JWT_REFRESH_DATE);
            if (DateTime.UtcNow.ToBinary() < long.Parse(refresh)) return;
            string token = _jwtHandler.CreateToken(user.Claims.Where(w => w.Type != JwtHandler.JWT_REFRESH_DATE).ToArray());
            context.HttpContext.Response.Headers.Add(_jwt.HeaderName, token);
            _appLogger.Info($"成功刷新Jwt令牌为 {token}");
        }
    }
}
