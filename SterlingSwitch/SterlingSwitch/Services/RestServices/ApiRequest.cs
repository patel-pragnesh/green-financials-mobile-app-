using Microsoft.AppCenter.Analytics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SterlingSwitch.Helper;
using SterlingSwitch.Services.Abstractions.Entities;
using SterlingSwitch.Services.Constants;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SterlingSwitch.Services.RestServices
{
    public class ApiRequest : IHttpService
    {


        public async Task<HttpResponseMessage> Get(string bearerToken, string baseUrl, string referenceUrl, string pageOrViewModel)
        {
            bearerToken = GlobalStaticFields.Token;
            using (var client = new HttpClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                client.BaseAddress = new Uri(baseUrl);
                if (!string.IsNullOrEmpty(bearerToken))
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {bearerToken}");

                client.DefaultRequestHeaders.Add("AppId", URLConstants.AppId);
                client.DefaultRequestHeaders.Add("SwitchID", GlobalStaticFields.Customer?.Email);
                client.DefaultRequestHeaders.Add("ChannelID", "1");
                //hash using hashmac256
                string hash = "";
                //get unique ID
                string uniqueid = GlobalStaticFields.GetUniqueID;
                hash = SHA.ComputeHMACSHA256(referenceUrl, GlobalStaticFields.Customer?.Email, uniqueid);
                //now add it to the header
                client.DefaultRequestHeaders.Add("X-CRC", hash);
                var request = await client.GetAsync(referenceUrl);
                Analytics.TrackEvent($"Get Request {referenceUrl}");
                LogResponse(request, baseUrl, referenceUrl, "", pageOrViewModel);
                return request;
            }
        }

        public async Task<HttpResponseMessage> GetWithSwitchId(string switchId, string bearerToken, string baseUrl, string referenceUrl, string pageOrViewModel)
        {
            using (var client = new HttpClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                client.BaseAddress = new Uri(baseUrl);
                if (!string.IsNullOrEmpty(bearerToken))
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {bearerToken}");

                client.DefaultRequestHeaders.Add("AppId", URLConstants.AppId);
                client.DefaultRequestHeaders.Add("SwitchID", switchId);

                var request = await client.GetAsync(referenceUrl);
                Analytics.TrackEvent($"Get Request {referenceUrl}");
                LogResponse(request, baseUrl, referenceUrl, "", pageOrViewModel);
                return request;
            }
        }

        public async Task<HttpResponseMessage> Send<T>(HttpMethod httpMethod, T model, string bearerToken, string baseUrl, string referenceUrl, string pageOrViewModel, bool isSensitive = false)
        {
            StringContent stringContent = default(StringContent);
            string jobj = "";
            using (var client = new HttpClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                client.BaseAddress = new Uri(baseUrl);
                if (!string.IsNullOrEmpty(bearerToken))
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "sk_test_00ecc6cd0604db4d2c81f6db4ceca19d581f602c");

                client.DefaultRequestHeaders.Add("AppId", URLConstants.AppId);
                client.DefaultRequestHeaders.Add("SwitchID", GlobalStaticFields.Customer?.Email);
                if (model != null)
                {
                    jobj = JsonConvert.SerializeObject(model);
                    stringContent = new StringContent(jobj, Encoding.UTF8, "application/json");
                }
                HttpRequestMessage requestMessage = new HttpRequestMessage();
                requestMessage.Method = httpMethod;
                requestMessage.Content = stringContent;
                Analytics.TrackEvent($"Get Request {referenceUrl}");
                var request = await client.SendAsync(requestMessage);


                LogResponse(request, baseUrl, referenceUrl, jobj, pageOrViewModel, isSensitive);


                return request;
            }
        }

        public async Task<HttpResponseMessage> Post<T>(T model, string bearerToken, string baseUrl, string referenceUrl, string pageOrViewModel, bool isSensitive = false)
        {
            StringContent stringContent = default(StringContent);
            string jobj = ""; bearerToken = GlobalStaticFields.Token;
            string hash = "";
            using (var client = new HttpClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                client.BaseAddress = new Uri(baseUrl);
                if (!string.IsNullOrEmpty(bearerToken))
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

                client.DefaultRequestHeaders.Add("AppId", URLConstants.AppId);

                client.DefaultRequestHeaders.Add("SwitchID", GlobalStaticFields.Customer?.Email);


                if (model != null)
                {
                    jobj = JsonConvert.SerializeObject(model);
                    stringContent = new StringContent(jobj, Encoding.UTF8, "application/json");
                }
                //hash using hashmac256
                string uniqueid = GlobalStaticFields.GetUniqueID;
                hash = SHA.ComputeHMACSHA256(jobj, GlobalStaticFields.Customer.Email, uniqueid);
                //now add it to the header
                client.DefaultRequestHeaders.Add("X-CRC", hash);
                Analytics.TrackEvent($"Get Request {referenceUrl}");
                var request = await client.PostAsync(referenceUrl, stringContent);

                LogResponse(request, baseUrl, referenceUrl, jobj, pageOrViewModel, isSensitive);


                return request;
            }
        }

        public async Task<HttpResponseMessage> Login(LoginInfo model, string baseUrl, string referenceUrl, string pageOrViewModel, bool isSensitive = false)
        {
            using (var client = new HttpClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                var key = Security.GetKey(16);//uniqued key generated dynamically that will be used for symetric encryption
                var iv = Security.GetKeyByte(8);//initialization vector that will be used for symmetric encrypt.iv and key go together for symmetric encryption
                var encodedKey = Encoding.UTF8.GetBytes(key);//convert to byte array
                //Assymetrically encode the key for header transmission
                var xcrc = Utilities.EncryptData(encodedKey, GlobalStaticFields.Exponent(), GlobalStaticFields.SHaredPublicKey());//this is the assymmetric encryption, encrypting the symmetric key. 
                //The server will need the private assymetric key to decrypt this in order to get the symetric  key 

                //var tripleDes_iv = Security.GetKeyByte(8);
                //var iv = Convert.ToBase64String(tripleDes_iv);
                //var key = await Utilities.GetUniqueKey();

                client.BaseAddress = new Uri(string.Concat(baseUrl, referenceUrl));
                client.DefaultRequestHeaders.Add("AppId", URLConstants.AppId);
                client.DefaultRequestHeaders.Add("X-KEY", xcrc);
                client.DefaultRequestHeaders.Add("X-IV", Convert.ToBase64String(iv));
                client.DefaultRequestHeaders.Add("X-PID", GlobalStaticFields.DeviceIMEI());

                using (var request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress))
                {
                    var encryptedPass = Security.TripleDESEncrypt(Encoding.UTF8.GetBytes(model.Password), Encoding.UTF8.GetBytes(key), iv);//symmetrically encrypting the password using key and IV
                    // var encryptedPass = Convert.ToBase64String(Security.TripleDESEncrypt(Encoding.UTF8.GetBytes(model.Password), Encoding.UTF8.GetBytes(key), tripleDes_iv));
                    request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
                        {
                            {"grant_type", "password"},
                            {"username", model.UserID},
                            {"password", Convert.ToBase64String(encryptedPass)},
                        });

                    var response = await client.SendAsync(request);
                    LogResponse(response, baseUrl, referenceUrl, "Sensitive", pageOrViewModel, isSensitive);
                    return response;

                }


            }
        }

        public async Task<HttpResponseMessage> OnBoarding(SwitchUser model, string bearerToken, string baseUrl, string referenceUrl, string pageOrViewModel, bool isSensitive = false)
        {

            using (var client = new HttpClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                client.BaseAddress = new Uri(baseUrl);
                if (!string.IsNullOrEmpty(bearerToken))
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);
                client.DefaultRequestHeaders.Add("AppId", URLConstants.AppId);
                client.DefaultRequestHeaders.Add("SwitchID", GlobalStaticFields.Customer?.Email);
                client.DefaultRequestHeaders.Add("ChannelID", "1");

                var key = Security.GetKey(16);
                var iv = Security.GetKeyByte(8);
                var encodedKey = Encoding.UTF8.GetBytes(key);
                var xcrc = Utilities.EncryptData(encodedKey, GlobalStaticFields.Exponent(), GlobalStaticFields.SHaredPublicKey());
                client.DefaultRequestHeaders.Add("X-KEY", xcrc);
                client.DefaultRequestHeaders.Add("X-IV", Convert.ToBase64String(iv));
                var jObj = string.Empty;
                if (model != null)
                {
                    jObj = JsonConvert.SerializeObject(model);
                }
                var encryptedData = Security.TripleDESEncrypt(Encoding.UTF8.GetBytes(jObj), Encoding.UTF8.GetBytes(key), iv);
                dynamic acct = new JObject();
                acct.Data = Convert.ToBase64String(encryptedData);
                var jdata = JsonConvert.SerializeObject(acct);
                StringContent content = new StringContent(jdata, Encoding.UTF8, "application/json");
                var request = await client.PostAsync(referenceUrl, content);
                LogResponse(request, baseUrl, referenceUrl, jdata, pageOrViewModel, isSensitive);
                return request;
            }

        }
        public async Task<HttpResponseMessage> Post2<T>(T model, string baseUrl, string referenceUrl, string pageOrViewModel, bool isSensitive = false)
        {


            StringContent stringContent = default(StringContent);
            string jobj = ""; string bearerToken = GlobalStaticFields.Token;
            using (var client = new HttpClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                client.BaseAddress = new Uri(baseUrl);
                if (!string.IsNullOrEmpty(bearerToken))
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

                client.DefaultRequestHeaders.Add("AppId", URLConstants.AppId);
                client.DefaultRequestHeaders.Add("SwitchID", GlobalStaticFields.Customer?.Email);
                if (model != null)
                {
                    jobj = JsonConvert.SerializeObject(model);
                    stringContent = new StringContent(jobj, Encoding.UTF8, "application/json");
                }
                //hash using hashmac256
                string hash = "";
                string uniqueid = GlobalStaticFields.GetUniqueID;
                hash = SHA.ComputeHMACSHA256(jobj, GlobalStaticFields.Customer.Email, uniqueid);
                //now add it to the header
                client.DefaultRequestHeaders.Add("CRC", hash);
                Analytics.TrackEvent($"Get Request {referenceUrl}");
                var request = await client.PostAsync(referenceUrl, stringContent);

                LogResponse(request, baseUrl, referenceUrl, jobj, pageOrViewModel, isSensitive);
                return request;
            }
        }

        public async Task<HttpResponseMessage> PostIBS<T>(T model, string bearerToken, string baseUrl, string referenceUrl, string pageOrViewModel, bool isSensitive = false)
        {
            StringContent stringContent = default(StringContent);
            string jobj = "";
            using (var client = new HttpClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                client.BaseAddress = new Uri(baseUrl);
                if (!string.IsNullOrEmpty(bearerToken))
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

                client.DefaultRequestHeaders.Add("AppId", URLConstants.AppId);
                client.DefaultRequestHeaders.Add("SwitchID", GlobalStaticFields.Customer?.Email);
                var xml = Utilities.XmlSerializer<T>(model);

                var ibs = new IBSRequestContent
                {
                    Appid = URLConstants.AppId,
                    XML = xml
                };

                if (ibs != null)
                {
                    jobj = JsonConvert.SerializeObject(ibs);
                    stringContent = new StringContent(jobj, Encoding.UTF8, "application/json");
                }
                //hash using hashmac256
                string hash = "";
                string uniqueid = GlobalStaticFields.GetUniqueID;
                hash = SHA.ComputeHMACSHA256(jobj, GlobalStaticFields.Customer.Email, uniqueid);
                //now add it to the header
                client.DefaultRequestHeaders.Add("CRC", hash);
                var request = await client.PostAsync(referenceUrl, stringContent);
                LogResponse(request, baseUrl, referenceUrl, jobj, pageOrViewModel, isSensitive);

                return request;
            }
        }

        public async Task<HttpResponseMessage> Log<T>(T model, string bearerToken, string baseUrl, string referenceUrl)
        {
            StringContent stringContent = default(StringContent);
            string jobj = "";
            using (var client = new HttpClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                client.BaseAddress = new Uri(baseUrl);
                if (!string.IsNullOrEmpty(bearerToken))
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

                client.DefaultRequestHeaders.Add("AppId", URLConstants.AppId);
                if (model != null)
                {
                    jobj = JsonConvert.SerializeObject(model);
                    stringContent = new StringContent(jobj, Encoding.UTF8, "application/json");
                }
                //hash using hashmac256
                string hash = "";
                string uniqueid = GlobalStaticFields.GetUniqueID;
                hash = SHA.ComputeHMACSHA256(jobj, GlobalStaticFields.Customer.Email, uniqueid);
                //now add it to the header
                client.DefaultRequestHeaders.Add("CRC", hash);
                Analytics.TrackEvent($"Get Request {referenceUrl}");
                var request = await client.PostAsync(referenceUrl, stringContent);
                return request;
            }
        }

        public async Task<HttpResponseMessage> Post<T>(T model, string baseUrl, string referenceUrl, string key, byte[] iv, string pageOrViewModel, bool isSensitive = false)
        {

            using (var client = new HttpClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                StringContent content;

                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Add("AppId", URLConstants.AppId);
                client.DefaultRequestHeaders.Add("SwitchID", GlobalStaticFields.Customer?.Email);
                client.DefaultRequestHeaders.Add("ChannelID", "1");


                var encodedKey = Encoding.UTF8.GetBytes(key);
                var xkey = Utilities.EncryptData(encodedKey, GlobalStaticFields.Exponent(), GlobalStaticFields.SHaredPublicKey());

               
                var xiv = Convert.ToBase64String(iv);
                var imei = GlobalStaticFields.DeviceIMEI();
                client.DefaultRequestHeaders.Add("X-KEY", xkey);
                client.DefaultRequestHeaders.Add("X-IV", xiv);
                
                client.DefaultRequestHeaders.Add("X-PID", imei);

                var jObj = string.Empty;
                if (model != null)
                {
                    jObj = JsonConvert.SerializeObject(model);
                }
                if (isSensitive)
                {
                    var encryptedData = Security.TripleDESEncrypt(Encoding.UTF8.GetBytes(jObj), encodedKey, iv);
                    dynamic acct = new JObject();
                    acct.Data = encryptedData;
                    var jdata = JsonConvert.SerializeObject(acct);
                    jObj = jdata;
                    content = new StringContent(jdata, Encoding.UTF8, "application/json");
                }
                else
                {
                    content = new StringContent(jObj, Encoding.UTF8, "application/json");
                }

                var xcrc = Security.HmacSHA512(jObj, encodedKey);
                client.DefaultRequestHeaders.Add("X-CRC", xcrc);




                var request = await client.PostAsync(referenceUrl, content);
                LogResponse(request, baseUrl, referenceUrl, jObj, pageOrViewModel, isSensitive);
                return request;
            }

        }


        private async void LogResponse(HttpResponseMessage httpResponseMessage, string baseUrl, string referenceUrl, string jsonrequest, string page, bool isSensitive = false)
        {
            var content = await httpResponseMessage.Content.ReadAsStringAsync();
            if (isSensitive)
            {
                await BusinessLogic.Log("", "", baseUrl + referenceUrl, "SENSITIVE", content, page);
            }
            else
            {
                await BusinessLogic.Log("", "", baseUrl + referenceUrl, jsonrequest, content, page);
            }
        }

        private string Sha512(string content)
        {

            using (var sha = new HMACSHA512(GetKey()))
            {

                return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(content)));
            }
        }

        private byte[] GetKey()
        {
            string key = "olamide";
            return Encoding.UTF8.GetBytes(key);
        }
    }
}
