using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenYS.NetCore
{
    public class OpenYSApi
    {
        private readonly HttpClient _httpClient;
        private readonly AccessTokenManager _accessTokenManager;
        //private readonly OpenYSOption _openYSOption;

        public OpenYSApi(HttpClient httpClient, IOptions<OpenYSOption> openYSOptionAccessor, AccessTokenManager accessTokenManager)
        {
            _httpClient = httpClient;
            _accessTokenManager = accessTokenManager;
            //_openYSOption = openYSOptionAccessor.Value;

            AppKey = openYSOptionAccessor.Value.AppKey;
            Secret = openYSOptionAccessor.Value.Secret;
            BaseUrl = openYSOptionAccessor.Value.BaseUrl;

            Identifier = Guid.NewGuid().ToString();
        }

        public string Identifier { get; private set; }
        public string AppKey { get; private set; }
        public string Secret { get; private set; }

        public string BaseUrl { get; private set; }

        public AccessTokenManager AccessTokenManager => _accessTokenManager;

        public void ResetConfig(string appKey, string secret)
        {
            AppKey = appKey;
            Secret = secret;
        }

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <returns><see cref="AccessToken"/></returns>
        /// <exception cref="OpenYSException">返回码不是200报异常</exception>
        public async Task<AccessToken> GetAccessTokenAsync()
        {
            var requestUri = $"{BaseUrl}/lapp/token/get";

            var dic = new Dictionary<string, string>
            {
                {"appKey",AppKey },
                {"appSecret",Secret }
            };

            var content = new FormUrlEncodedContent(dic);
            try
            {
                var responseMessage = await _httpClient.PostAsync(requestUri, content);
                var json = await responseMessage.Content.ReadAsStringAsync();


                var parse = JToken.Parse(json);
                var code = parse["code"].ToObject<int>();
                if (code != 200)
                {
                    var msg = parse["msg"].ToString();
                    throw new OpenYSException(code, msg);
                }

                var data = parse["data"];

                var token = data["accessToken"].ToString();
                var expireTime = data["expireTime"].ToObject<long>();

                var accessToken = new AccessToken(token, expireTime);

                _accessTokenManager.AddOrUpdateAccessToken(AppKey, accessToken);

                return accessToken;
            }
            catch (OpenYSException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
                throw new OpenYSException("未知错误");
            }
        }


        /// <summary>
        /// 添加设备
        /// </summary>
        /// <param name="deviceSerial">设备序列号，存在英文字母的设备序列号，字母需为大写</param>
        /// <param name="validateCode">设备验证码，设备机身上的六位大写字母</param>
        /// <exception cref="OpenYSException">返回码不是200报异常</exception>
        public async Task AddDeviceAsync(string deviceSerial, string validateCode)
        {
            if (string.IsNullOrEmpty(deviceSerial)) throw new OpenYSException("设备序列号不能为空");
            if (string.IsNullOrEmpty(validateCode)) throw new OpenYSException("设备验证码不能为空");

            var accessToken = await CheckAccessTokenAsync();

            var requestUri = $"{BaseUrl}/lapp/device/add";

            var dic = new Dictionary<string, string>
            {
                {"accessToken",accessToken.Token },
                {"deviceSerial",deviceSerial },
                {"validateCode",validateCode }
            };

            var content = new FormUrlEncodedContent(dic);
            try
            {
                var responseMessage = await _httpClient.PostAsync(requestUri, content);
                var json = await responseMessage.Content.ReadAsStringAsync();

                var parse = JToken.Parse(json);
                var code = parse["code"].ToObject<int>();
                if (code != 200)
                {
                    var msg = parse["msg"].ToString();
                    throw new OpenYSException(code, msg);
                }
            }
            catch (OpenYSException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
                throw new OpenYSException("未知错误");
            }
        }


        /// <summary>
        /// 删除设备
        /// </summary>
        /// <param name="deviceSerial">设备序列号，存在英文字母的设备序列号，字母需为大写</param>
        /// <exception cref="OpenYSException">返回码不是200报异常</exception>
        public async Task DeleteDeviceAsync(string deviceSerial)
        {
            if (string.IsNullOrEmpty(deviceSerial)) throw new OpenYSException("设备序列号不能为空");

            var accessToken = await CheckAccessTokenAsync();

            var requestUri = $"{BaseUrl}/lapp/device/delete";

            var dic = new Dictionary<string, string>
            {
                {"accessToken",accessToken.Token },
                {"deviceSerial",deviceSerial }
            };

            var content = new FormUrlEncodedContent(dic);
            try
            {
                var responseMessage = await _httpClient.PostAsync(requestUri, content);
                var json = await responseMessage.Content.ReadAsStringAsync();

                var parse = JToken.Parse(json);
                var code = parse["code"].ToObject<int>();
                if (code != 200)
                {
                    var msg = parse["msg"].ToString();
                    throw new OpenYSException(code, msg);
                }
            }
            catch (OpenYSException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
                throw new OpenYSException("未知错误");
            }
        }


        /// <summary>
        /// 获取摄像头列表
        /// </summary>
        /// <param name="pageStart">分页起始页，从0开始</param>
        /// <param name="pageSize">分页大小，默认为10，最大为50</param>
        /// <returns><see cref="PagedList{T}"/></returns>
        /// <exception cref="OpenYSException">返回码不是200报异常</exception>
        public async Task<PagedList<MCamera>> GetCameraListAsync(int pageStart = 0, int pageSize = 10)
        {
            pageSize = pageSize > 50 ? 50 : pageSize;

            var accessToken = await CheckAccessTokenAsync();

            var requestUri = $"{BaseUrl}/lapp/camera/list";

            var dic = new Dictionary<string, string>
            {
                {"accessToken",accessToken.Token },
                {"pageStart",pageStart.ToString() },
                {"pageSize",pageSize.ToString() }
            };

            var content = new FormUrlEncodedContent(dic);
            try
            {
                var responseMessage = await _httpClient.PostAsync(requestUri, content);
                var json = await responseMessage.Content.ReadAsStringAsync();

                var parse = JToken.Parse(json);
                var code = parse["code"].ToObject<int>();
                if (code != 200)
                {
                    var msg = parse["msg"].ToString();
                    throw new OpenYSException(code, msg);
                }

                var pagedList = new PagedList<MCamera>();

                var page = parse["page"];
                pagedList.Total = page["total"].ToObject<int>();
                pagedList.Page = page["page"].ToObject<int>();
                pagedList.Size = page["size"].ToObject<int>();

                var data = parse["data"] as JArray;

                foreach (var item in data)
                {
                    pagedList.Items.Add(new MCamera
                    {
                        DeviceSerial = item["deviceSerial"].ToString(),
                        ChannelNo = item["channelNo"].ToObject<int>(),
                        ChannelName = item["channelName"].ToString(),
                        Status = item["status"].ToObject<int>(),
                        PicUrl = item["picUrl"].ToString(),
                        IsEncrypt = item["isEncrypt"].ToObject<int>(),
                        VideoLevel = item["videoLevel"].ToObject<int>(),
                        Permission = item["permission"].ToObject<int>()
                    });
                }

                return pagedList;
            }
            catch (OpenYSException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
                throw new OpenYSException("未知错误");
            }

        }




        /// <summary>
        /// 检查AccessToken，如果没有或过期则调用接口获取
        /// </summary>
        /// <returns><see cref="AccessToken"/></returns>
        /// <exception cref="OpenYSException">重新获取时可能报异常</exception>
        private async Task<AccessToken> CheckAccessTokenAsync()
        {
            var accessToken = _accessTokenManager.GetAccessToken(AppKey);
            if (accessToken == null || accessToken.Expired || accessToken.NearlyExpired)
            {
                accessToken = await GetAccessTokenAsync();
            }
            return accessToken;
        }
    }
}
