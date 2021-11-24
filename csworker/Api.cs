using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using csprjclib.Models;
using System.Net.Http.Headers;
using csprjclib.ApiModels;
using System.Text;
using System.Net;

namespace csworker
{
    public class hClient : HttpClient
    {
        string basePath = "http://localhost:5001/Agent/";

        public hClient()
        {
            BaseAddress = new Uri(basePath);
            DefaultRequestHeaders.Accept.Clear();
            DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }

    public class Api : IDisposable
    {
        public void Dispose()
        {
            try
            {
                _client.Dispose();
            }
            catch (Exception)
            {

            }
        }

        hClient _client = new hClient();

        private HttpResponseMessage apiGet(string add)
        {
            HttpResponseMessage resp = new HttpResponseMessage();

            try
            {
                return _client.GetAsync(add).Result;
            }
            catch (Exception ex)
            {
                resp.StatusCode = HttpStatusCode.BadGateway;
                resp.ReasonPhrase = ex.Message;
            }

            return resp;
        }

        private HttpResponseMessage apiPost(string add, object obj)
        {
            HttpContent sCon = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            //sCon.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage resp = new HttpResponseMessage();
            try
            {
                resp = _client.PostAsync(add, sCon).Result;
            }
            catch (Exception ex)
            {
                resp.StatusCode = HttpStatusCode.BadGateway;
                resp.ReasonPhrase = ex.Message;
            }
            return resp;
        }

        public List<MonitoredFile> GetMonitors()
        {
            var resp = apiGet("GetActiveMonitors");

            var monitoredFiles = new List<MonitoredFile>();

            if (resp.IsSuccessStatusCode)
            {
                monitoredFiles = JsonConvert.DeserializeObject<List<MonitoredFile>>(resp.Content.ReadAsStringAsync().Result);
            }
            
            return monitoredFiles;
        }

        public int HashCheck(apiMonitoredFileHash model)
        {
            var resp = apiPost("HashCheck", model);
            if (resp.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<int>(resp.Content.ReadAsStringAsync().Result);
            return 99; //resp error
        }

        public int UploadFile(apiMonitoredFileContent model)
        {
            var resp = apiPost("UploadFile", model);
            if (resp.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<int>(resp.Content.ReadAsStringAsync().Result);
            return 99; //resp error
        }
    }
}
