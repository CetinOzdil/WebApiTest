using System;
using System.Collections.Generic;
using System.Text;

namespace WebHoster.Interface
{
    internal interface IStartupInjectionConfiguration
    {
        public IStartupInjection AuthInjection { get; set; }
        public List<IStartupInjection> ApplicationInjection { get; set; }

    }
}
