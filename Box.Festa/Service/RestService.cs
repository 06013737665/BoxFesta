using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Box.Festa.Service
{
    public class RestService<T> 
    {
        HttpClient client;

        public RestService()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            //  NativeMessageHandler clientHandler = new NativeMessageHandler(false, true);
            client = new HttpClient(clientHandler);

            client.DefaultRequestHeaders.Add("cache-control", "no-cache");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        public async Task<T> RefreshDataAsync(string restUrl, object[] parametros, bool pagseguro = false)
        {
            var uri = new Uri(string.Format(restUrl, parametros));

            try
            {
                if (pagseguro)
                {
                    client = new HttpClient();

                    client.DefaultRequestHeaders.Add("cache-control", "no-cache");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                }
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {

                    var content = await response.Content.ReadAsStringAsync();
                    content = content.Replace("\"{\\\"", "{\"").Replace("}\\\"\"", "}").Replace(",\\", ",").Replace("\\", "").Replace("}\"", "}").Replace("\n","").Replace("\\\"", "");
                   
                    return JsonConvert.DeserializeObject<T>(content);
                }
            }
            catch (JsonSerializationException ex)
            {
                Console.WriteLine(@"ERROR {0}", ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"ERROR {0}", ex.Message);
            }

            return default(T);
        }

        public async Task<XmlDocument> RefreshDataXmlAsync(string restUrl, object[] parametros)
        {
            var uri = new Uri(string.Format(restUrl, parametros));

            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {

                    var content = await response.Content.ReadAsStringAsync();
                    content = content.Replace("\"{\\\"", "{\"").Replace("}\\\"\"", "}").Replace(",\\", ",").Replace("\\", "").Replace("}\"", "}").Replace("\n", "").Replace("\\\"", "");
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(content);
                    return doc;
                   
                  
                }
            }
            catch (JsonSerializationException ex)
            {
                Console.WriteLine(@"ERROR {0}", ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"ERROR {0}", ex.Message);
            }

            return new XmlDocument();
        }

        public static async Task<String> AccessTheWebAsync(String url, string content, string method, string user = "", string password = "", string domain = "")
        {
            HttpClientHandler httpCH = null;

            if (!string.IsNullOrEmpty(user))
            {
                httpCH = new HttpClientHandler();
                httpCH.Credentials = new NetworkCredential(user, password, domain);
            }

            var urlContents = "";
            try
            {
                HttpClient client;
                Task<string> getStringTask;
                Task<HttpResponseMessage> postStringTask;

                if (httpCH == null)
                    client = new HttpClient();
                else
                    client = new HttpClient(httpCH);

                HttpContent contentPost = new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded");


                if (method.ToLower().Equals("get"))
                {
                    //client.Timeout = TimeSpan.FromMilliseconds(10000);
                    getStringTask = client.GetStringAsync(url);
                    getStringTask.Start();

                    getStringTask.Wait(10000);
                    if (getStringTask.IsCompleted)
                    {
                        urlContents = getStringTask.Result;
                    }
                    else
                    {
                        throw new Exception("Timeout Pagseguro");
                    }
                }
                else
                {
                    postStringTask = client.PostAsync(url, contentPost);
                    urlContents = await postStringTask.Result.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

                log.Debug("-------------------------------task exeption "+ex.ToString());
                throw ex;
            }
            return urlContents;
        }

        public static String AtualizarStatus(String url, string content, string method, string user = "", string password = "", string domain = "")
        {
           

            var urlContents = "";
            try
            {
                HttpClient client;
                Task<string> getStringTask;
                Task<HttpResponseMessage> postStringTask;

                
                    client = new HttpClient();

                HttpContent contentPost = new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded");


                if (method.ToLower().Equals("get"))
                {
                    //client.Timeout = TimeSpan.FromMilliseconds(10000);
                    getStringTask = client.GetStringAsync(url);
                  //  getStringTask.Start();

                    getStringTask.Wait(10000);
                    if (getStringTask.IsCompleted)
                    {
                        urlContents = getStringTask.Result;
                    }
                    else
                    {
                        throw new Exception("Timeout Pagseguro");
                    }
                }
               
            }
            catch (Exception ex)
            {
                log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

                log.Debug("-------------------------------task exeption " + ex.ToString());
                throw ex;
            }
            return urlContents;
        }

        public async Task<T> RefreshDataPostAsync(string restUrl, object parametro)
        {
            var uri = new Uri(string.Format(restUrl));

            try
            {
                var jsonRequest = JsonConvert.SerializeObject(parametro);
                var contentParam = new StringContent(jsonRequest, Encoding.UTF8, "text/json");
               // var contentParam = new StringContent(jsonRequest, Encoding.UTF8, "text/xml");
                var response = await client.PostAsync(uri, contentParam);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(content);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"ERROR {0}", ex.Message);
            }

            return default(T);
        }

    }

}
