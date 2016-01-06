using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestSharp;
using System.ServiceModel.Web;
using System.Net;
using ScmWcfService.Config;

namespace ScmWcfService.Provider
{
    public class ApiClient
    {
        public string token { get; set; }
        public ApiClient() { }
        public ApiClient(string token) { this.token = token; }

        public IRestRequest GenRequest(string url,Method method=Method.GET, DataFormat format=DataFormat.Json) {
            var req = new RestRequest(url, method);
            req.RequestFormat = format;
            return req;
        }

        public T Execute<T>(IRestRequest request) where T : new()
        {
            var response = genClient().Execute<T>(request);
            return response.Data;
        }


        public IRestResponse Execute(IRestRequest request)
        {
            var response = genClient().Execute(request);
            return responseHandler(response);
        }

        private RestClient genClient()
        {
            var client = new RestClient();
            client.BaseUrl = ApiConfig.BaseUri;
            if (this.token != null)
            {
                client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(this.token, "Bearer");
            }
            return client;
        }

        private IRestResponse responseHandler(IRestResponse res)
        {
            if (res.StatusCode != HttpStatusCode.OK && res.StatusCode != HttpStatusCode.Created)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = res.StatusCode;
                WebOperationContext.Current.OutgoingResponse.StatusDescription = res.StatusDescription;
                throw new WebFaultException<string>(res.StatusDescription, res.StatusCode);
            }
            return res;
        }
    }

    public class Parameter { 
      
    }
}