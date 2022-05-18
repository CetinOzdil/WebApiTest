using System;
using System.Collections.Generic;
using System.Text;
using WebHoster.Interface;

namespace WebHoster.Class
{
    internal class StartupInjectionConfiguration : IStartupInjectionConfiguration
    {
        public IStartupInjection AuthInjection { get; set; }
        public List<IStartupInjection> ApplicationInjection { get; set; } = new List<IStartupInjection>();
    }
}
