using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Api.Filters
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment _env;
        ILogger<HttpGlobalExceptionFilter> _logger;

        public HttpGlobalExceptionFilter(IHostingEnvironment env,ILogger<HttpGlobalExceptionFilter> logger)
        {
            _env = env;
            _logger = logger;
        }
        

        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            var json = new JsonErrorResponse();
            if (context.Exception.GetType() == typeof(UserOperationException))
            {
                json.Message = context.Exception.Message;
                context.Result = new BadRequestObjectResult(json);
            }
            else {
                json.Message = "发生内部未知错误";
                if (_env.IsDevelopment())
                {
                    json.DeveloperMessage = context.Exception.StackTrace;
                }
                context.Result = new InternalServerErrorObjectResult(json);
            }
            _logger.LogError(context.Exception,context.Exception.Message);
            context.ExceptionHandled = true;
        }
    }
    public class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult(object error):base(error)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
