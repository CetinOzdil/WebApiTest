using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebHoster.Interface
{
    public interface IStartupInjection
    {
        public void InjectConfig(IApplicationBuilder app);
        public void InjectConfigureServices(IServiceCollection services);
    }
}
