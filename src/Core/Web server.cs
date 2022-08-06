using MelonLoader;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NEP.Scoreworks.Core
{
    public class Web_server
    {
            // creates json so i can send the score data over to flask
            public string deaths(int kills)
            {
                var data = new[] {

                new {total = kills}
            };

                var json = JArray.FromObject(data)[0].ToString();
                return json;
            }

            // sends data o the webserver 
            public async Task sendkillsAsync(string data, string endpoint)
            {
                try
                {
                    MelonLogger.Msg("trying to send data to server");

                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://127.0.0.1:5000/" + endpoint);
                    httpWebRequest.ContentType = "application/json; charset=utf-8";
                    httpWebRequest.Method = "POST";

                  

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {

                        streamWriter.WriteAsync(data);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    using (var response = httpWebRequest.GetResponse() as HttpWebResponse)
                    {
                        if (httpWebRequest.HaveResponse && response != null)
                        {
                            using (var reader = new StreamReader(response.GetResponseStream()))
                            {

                                MelonLogger.Msg(reader.ReadToEnd());
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    MelonLogger.Msg(e.Message);
                }
            }

        }
    }
}
