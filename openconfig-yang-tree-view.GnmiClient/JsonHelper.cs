using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openconfig_yang_tree_view.GnmiClient
{
    public static class JsonHelper
    {
        public static string DecodeJsonVals(string inputJson)
        {
            JArray jsonArray = JArray.Parse(inputJson);

            foreach (JObject obj in jsonArray)
            {
                JArray updateArray = (JArray)obj["update"];

                foreach (JObject updateObj in updateArray)
                {
                    JObject valObj = (JObject)updateObj["val"];
                    string jsonValBase64 = (string)valObj["jsonVal"];
                    if (jsonValBase64 == null)
                    {
                        continue;
                    }
                    byte[] bytes = Convert.FromBase64String(jsonValBase64);
                    string decodedJson = Encoding.UTF8.GetString(bytes);

                    if (IsValidJson(decodedJson))
                    {
                        valObj["jsonVal"] = JObject.Parse(decodedJson);
                    }
                    else
                    {
                        if (decodedJson.Contains("\""))
                        {
                            var stringArray = decodedJson.Split("\"");
                            decodedJson = stringArray[1];
                        }
                        valObj["jsonVal"] = decodedJson;
                    }
                }
            }

            return jsonArray.ToString();
        }

        public static bool IsValidJson(string input)
        {
            try
            {
                JObject.Parse(input);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
