using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;

namespace OpenYS.NetCore.Example
{
    class Program
    {
        static IConfiguration Configuration;

        static async Task Main(string[] args)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                //.AddJsonFile("appsettings.json")
                .AddUserSecrets("c4349a92-4716-43ee-a589-d4d40882912f")
                .Build();

            ServiceLocator.ServiceProvider = new ServiceCollection()
                                        .AddOpenYS(option =>
                                                {
                                                    option.AppKey = Configuration["OpenYS:AppKey"];
                                                    option.Secret = Configuration["OpenYS:Secret"];
                                                    option.BaseUrl = Configuration["OpenYS:BaseUrl"];
                                                })
                                        .BuildServiceProvider();


            var openYsApi1 = ServiceLocator.ServiceProvider.GetService<OpenYSApi>();
            var openYsApi2 = ServiceLocator.ServiceProvider.GetService<OpenYSApi>();

            //var accessToken = openYsApi1.AccessTokenManager.GetAccessToken(openYsApi1.AppKey);
            //if (accessToken == null)
            //{
            //    Console.WriteLine("accesstoken is null");
            //}
            //try
            //{
            //    accessToken = await openYsApi1.GetAccessTokenAsync();

            //    Console.WriteLine(accessToken.Token);

            //    accessToken = openYsApi1.AccessTokenManager.GetAccessToken(openYsApi1.AppKey);
            //    if (accessToken == null)
            //    {
            //        Console.WriteLine("accesstoken is null");
            //    }
            //    else
            //    {
            //        Console.WriteLine("from token manager");
            //        Console.WriteLine(accessToken.Token);
            //    }
            //}
            //catch (OpenYSException ex)
            //{
            //    Console.WriteLine(ex.Code);
            //    Console.WriteLine(ex.Message);
            //}

            //// 添加设备
            //try
            //{
            //    await openYsApi1.AddDeviceAsync("XXXXXXXXX", "XXXXXX");
            //}
            //catch (OpenYSException ex)
            //{
            //    Console.WriteLine(ex.Code);
            //    Console.WriteLine(ex.Message);
            //}

            // 删除设备
            //try
            //{
            //    await openYsApi1.DeleteDeviceAsync("XXXXXXXXX");
            //}
            //catch (OpenYSException ex)
            //{
            //    Console.WriteLine(ex.Code);
            //    Console.WriteLine(ex.Message);
            //}


            // 获取摄像头列表
            try
            {
                var pagedList = await openYsApi1.GetCameraListAsync();
                foreach (var item in pagedList.Items)
                {
                    Console.WriteLine($"{item.DeviceSerial} {item.ChannelNo} {item.ChannelName} {item.Status}");
                }
            }
            catch (OpenYSException ex)
            {
                Console.WriteLine(ex.Code);
                Console.WriteLine(ex.Message);
            }



        }
    }
}
