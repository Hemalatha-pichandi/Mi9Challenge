using JSONExamples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Script.Serialization;

namespace WCFJSON
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "JsonTest" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select JsonTest.svc or JsonTest.svc.cs at the Solution Explorer and start debugging.
    public class JsonTest : IJsonTest
    {       

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "GetJsonTestData")]
        public Payload GetJsonData()
        {
            Payload payLoad = new Payload()
            {
                drm = false,
                episodeCount = 0
            };
            return payLoad;
        }

        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
        UriTemplate = "PostJsonTestData")]
        public string PostJsonData(string JSONData)
        {
            try
            {                
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var jsonObject = serializer.Deserialize<dynamic>(JSONData);
                var payLoads = jsonObject["payload"];
                
                List<object> results = new List<object>();
                
                foreach (var item in payLoads)
                    if (ConditionCheck(item))
                        GenerateResult(item, results);

                Dictionary<string, List<object>> resultData = new Dictionary<string, List<object>>();
                resultData.Add("response", results);

                return serializer.Serialize(resultData);
            }
            catch (Exception e)
            {
                throw new WebFaultException<string>("Could not decode request: JSON parsing failed", System.Net.HttpStatusCode.BadRequest);                
            }   
        }

        protected bool ConditionCheck(Dictionary<string, object> item)
        {
            if (!item.Keys.Contains("drm") || !item.Keys.Contains("episodeCount"))
                return false;
            if ((bool)item["drm"] == false || (int)item["episodeCount"] <= 0)
                return false;
            return true;
        }

        protected bool GenerateResult(Dictionary<string, object> payLoadData, List<object> ResultList)
        {
            Dictionary<string, object> payLoad = new Dictionary<string, object>();

            if (!payLoadData.Keys.Contains("image"))
                payLoad.Add("image", "");
            else if (!(payLoadData["image"] as Dictionary<string, object>).Keys.Contains("showImage"))
                payLoad.Add("image", "");
            else
                payLoad.Add("image", (payLoadData["image"] as Dictionary<string, object>)["showImage"]);

            if (!payLoadData.Keys.Contains("slug"))
                payLoad.Add("slug", "");
            else
                payLoad.Add("slug", payLoadData["slug"]);

            if (!payLoadData.Keys.Contains("title"))
                payLoad.Add("title", "");
            else
                payLoad.Add("title", payLoadData["title"]);
            
            ResultList.Add(payLoad);
            return true;
        }

    }
}
