using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace ADOAdmin.AzureDevOps.Clients
{
    class RestAPIClient
    {

        public string AzureDevOpsGet(string devOpsPat, string apiUrl)
        {
            string apiResponse = ApiCall("GET", devOpsPat, apiUrl, null);
            return apiResponse;
        }
        public string AzureDevOpsPost(string devOpsPat, string apiUrl, string jsonData)
        {
            string apiResponse = ApiCall("POST", devOpsPat, apiUrl, jsonData);
            return apiResponse;
        }
        public string AzureDevOpsPatch(string devOpsPat, string apiUrl, string jsonData)
        {
            string apiResponse = ApiCall("PATCH", devOpsPat, apiUrl, jsonData);
            return apiResponse;
        }

        private string ApiCall(string apiActivity, string devOpsPat, string apiUrl, string jsonData)
        {
            string _responseBody = "";
            string _statusCode = "";

            switch (apiActivity)
            {
                //Get ADO API's
                case "GET":
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
                        ASCIIEncoding.ASCII.GetBytes(
                        string.Format("{0}:{1}", "", devOpsPat))));
                        using (HttpResponseMessage response = client.GetAsync(apiUrl).Result)
                        {
                            _responseBody = response.Content.ReadAsStringAsync().Result;
                            //response.EnsureSuccessStatusCode();
                            _statusCode = response.StatusCode.ToString();
                            if (response.IsSuccessStatusCode && _statusCode != "NonAuthoritativeInformation")
                                return _responseBody;
                            else
                                throw new HttpRequestException("API throwed error with status code:" + _statusCode + " and Response body: " + _responseBody);
                        }
                    }
                case "PATCH":
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json-patch+json"));
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
                                    ASCIIEncoding.ASCII.GetBytes(
                                    string.Format("{0}:{1}", "", devOpsPat))));

                            var method = new HttpMethod("PATCH");
                            var request = new HttpRequestMessage(method, apiUrl)
                            {
                                Content = new StringContent("[" + jsonData + "]", Encoding.UTF8, "application/json-patch+json")

                            };
                            using (HttpResponseMessage response = client.SendAsync(request).Result)
                            {
                                _responseBody = response.Content.ReadAsStringAsync().Result;
                                _statusCode = response.StatusCode.ToString();
                                if (response.IsSuccessStatusCode && _statusCode != "NonAuthoritativeInformation")
                                    return _responseBody;
                                else
                                    throw new HttpRequestException("API throwed error with status code:" + _statusCode + " and Response body: " + _responseBody);
                            }
                        }
                    }
                case "POST":
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
                                ASCIIEncoding.ASCII.GetBytes(
                                string.Format("{0}:{1}", "", devOpsPat))));

                        var method = new HttpMethod("POST");
                        var request = new HttpRequestMessage(method, apiUrl)
                        {
                            Content = new StringContent(jsonData, Encoding.UTF8, "application/json")
                        };
                        using (HttpResponseMessage response = client.SendAsync(request).Result)
                        {
                            _responseBody = response.Content.ReadAsStringAsync().Result;
                            _statusCode = response.StatusCode.ToString();
                            if (response.IsSuccessStatusCode && _statusCode != "NonAuthoritativeInformation")
                                return _responseBody;
                            else
                                throw new HttpRequestException("API throwed error with status code:" + _statusCode + " and Response body: " + _responseBody);
                        }
                    }

            }
            return "Error in API call";
        }
    }
}
