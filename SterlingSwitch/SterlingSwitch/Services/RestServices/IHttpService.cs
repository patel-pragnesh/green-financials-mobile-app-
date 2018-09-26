using SterlingSwitch.Services.RestServices;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SterlingSwitch.Services.RestServices
{
    public interface IHttpService
    {
        Task<HttpResponseMessage> Send<T>(HttpMethod httpMethod, T model, string bearerToken, string baseUrl, string referenceUrl,string pageOrViewModel, bool isSensitive = false);
        Task<HttpResponseMessage> Get(string bearerToken, string baseUrl, string referenceUrl, string page);
        Task<HttpResponseMessage> Post<T>(T model, string bearerToken, string baseUrl, string referenceUrl, string page, bool isSensitive = false);
        Task<HttpResponseMessage> PostIBS<T>(T model, string bearerToken, string baseUrl, string referenceUrl, string page, bool isSensitive = false);

    }
}
