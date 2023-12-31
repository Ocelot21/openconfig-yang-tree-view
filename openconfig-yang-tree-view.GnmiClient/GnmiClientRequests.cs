using Gnmi;
using Grpc.Core;
using Grpc.Net.Client;
using System.Net.Http;
using System;
using Newtonsoft.Json.Linq;

namespace openconfig_yang_tree_view.GnmiClient
{
    public static class GnmiClientRequests
    {
        public static string GetRequest(string ip, string port, string username, string password, bool isHttps, string path)
        {
            try
            {
                string address = isHttps ? $"https://{ip}:{port}" : $"http://{ip}:{port}";

                var httpClientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };

                var httpClient = new HttpClient(httpClientHandler);
                httpClient.BaseAddress = new Uri(address);

                var channelOptions = new GrpcChannelOptions
                {
                    HttpClient = httpClient,
                    DisposeHttpClient = true
                };


                GrpcChannel channel = GrpcChannel.ForAddress(address, channelOptions);

                var _client = new gNMI.gNMIClient(channel);

                var request = new CapabilityRequest();

                var metaData = new Metadata();

                metaData.Add("username", username);
                metaData.Add("password", password);

                var getRequest = new GetRequest();

                var gnmiPath = new Gnmi.Path();

                var nodeList = path.Split('/', StringSplitOptions.RemoveEmptyEntries);

                foreach (var nodeName in nodeList)
                {
                    gnmiPath.Elem.Add(new PathElem { Name = nodeName });
                }

                getRequest.Path.Add(gnmiPath);

                var getResponse = _client.Get(getRequest, metaData);

                string formattedJson = JValue.Parse(getResponse.Notification.ToString()).ToString(Newtonsoft.Json.Formatting.Indented);

                return JsonHelper.DecodeJsonVals(formattedJson.ToString());
            }
            
            catch (Exception ex)
            {
                return $"An error occured. Message:\n{ex.Message}\nSource:\n{ex.Source}";
            }
        }
    }
}
