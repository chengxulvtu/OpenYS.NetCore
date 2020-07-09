
using OpenYS.NetCore;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class OpenYSExtensions
    {

        public static IServiceCollection AddOpenYS(this IServiceCollection services, Action<OpenYSOption> action)
        {
            services.AddOptions();
            services.Configure(action);
            services.AddHttpClient<OpenYSApi>();
            services.AddSingleton<AccessTokenManager>();

            //services.AddTransient(sp =>
            //{
            //    var option = new OpenYSOption();
            //    action?.Invoke(option);
            //    var api = new OpenYSApi(sp.GetService<>());
            //    api.Config(option.AppKey, option.Secret);
            //    return api;
            //});

            return services;
        }
    }


    public class OpenYSOption
    {
        public string BaseUrl { get; set; }

        public string AppKey { get; set; }

        public string Secret { get; set; }
    }
}
