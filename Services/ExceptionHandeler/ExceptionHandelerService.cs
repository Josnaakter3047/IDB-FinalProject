
using DataModels.Other.View;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.ExceptionHandeler
{
    public class ExceptionHandelerService
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IServiceProvider _serviceProvider;

        public ExceptionHandelerService(RequestDelegate next, ILogger<ExceptionHandelerService> logger, IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _next = next;
            _serviceProvider = serviceProvider;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                //var _userManger = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                //var userIdentity =_httpContextAccessor?.HttpContext?.User?.Identity as ClaimsIdentity;
                //if (userIdentity.IsAuthenticated)
                //{
                //    var userEmail = userIdentity.FindFirst("userEmail").Value;
                //    var user = await _userManger.FindByEmailAsync(userEmail);
                //    if (user != null)
                //    {
                //        await httpContext.Response.WriteAsync(new ErrorDetailsViewModel()
                //        {
                //            statusCode = 401,
                //            message = "Account Suspended"
                //        }.ToString());
                //    }
                //}

                //var data = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;

            if (exception.Message.ToLower().Contains("error: 40"))
                await context.Response.WriteAsync(new ErrorDetailsViewModel()
                {
                    statusCode = context.Response.StatusCode,
                    message = "Could not establish connection with database"
                }.ToString());

            else if (exception.Message.ToLower().Contains("error: 19"))
                await context.Response.WriteAsync(new ErrorDetailsViewModel()
                {
                    statusCode = context.Response.StatusCode,
                    message = "Unstable Internet Connection"
                }.ToString());
            else if (exception.Message.ToLower().Contains("an error occurred while updating the entries"))
                await context.Response.WriteAsync(new ErrorDetailsViewModel()
                {
                    statusCode = context.Response.StatusCode,
                    message = "Invalid data detected"
                }.ToString());
            else if (exception.Message.ToLower().Contains("connection timeout expired"))
                await context.Response.WriteAsync(new ErrorDetailsViewModel()
                {
                    statusCode = context.Response.StatusCode,
                    message = "Connection Timeout Expired For SQL Server"
                }.ToString());
            else
                await context.Response.WriteAsync(new ErrorDetailsViewModel()
                {
                    statusCode = context.Response.StatusCode,
                    message = "Unknown Error occured"
                }.ToString());
        }
    }
}
