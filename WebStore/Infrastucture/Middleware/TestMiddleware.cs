using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Infrastucture.Middleware
{
    public class TestMiddleware
    {
        private readonly RequestDelegate _Next;
        public TestMiddleware(RequestDelegate Next) => _Next = Next;
        public async Task InvokeAsync(HttpContext context)
        {
            await _Next(context);
        }
    }
}
