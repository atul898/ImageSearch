
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    namespace ImageSearch.Service
    {
        internal class WebBackendClient : HttpClient
        {
            //Reference
            //https://developers.google.com/image-search/v1/jsondevguide#json_args

            private static object syncRoot = new Object();
            private const string endpointRoot = @"http://ajax.googleapis.com/ajax/services/search/images?v=1.0&rsz=8";

            public WebBackendClient()
            {
                MaxResponseContentBufferSize = 1024 * 1024;
            }

            public T Download<T>(string relativePath)
            {
                string response = GetStringAsync(endpointRoot + relativePath).Result;
                return JsonConvert.DeserializeObject<T>(response);
            }

            public async Task<T> DownloadAsync<T>(string relativePath)
            {
                string response = null;
                var t = Task.Factory.StartNew(async delegate
                {
                    response = await GetStringAsync(endpointRoot + relativePath);
                    return JsonConvert.DeserializeObject<T>(response);
                });
                return await t.Result;
            }

            //public async Task<T> DownloadAsync<T>(string relativePath)
            //{
            //    string response = await GetStringAsync(endpointRoot + relativePath);
            //    return JsonConvert.DeserializeObject<T>(response);
            //}
        }
    }
