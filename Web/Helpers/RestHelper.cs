using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using RestSharp;

namespace Web.Helpers
{
    public class RestHelper
    {
        public static string HttpGet(string host, string urlParams)
        {
            var client = new RestClient(host);

            var request = new RestRequest(urlParams, Method.GET);

            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = response.Content;

                return content;
            }
            else
            {
                Logger.Log.Error($"RestHelper errror. Status code: {response.StatusCode}. Message: {response.ErrorMessage}");
                return string.Empty;
            }
        }
    }
}