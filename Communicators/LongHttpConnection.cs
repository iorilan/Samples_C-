  public class SampleAPIHelper
    {
        private readonly string url = "";
        private readonly string apiKey = "";
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SampleAPIHelper(string url, string apiKey)
        {
            this.url = url;
            this.apiKey = apiKey;
        }

        public Data[] ListenForUpdate()
        {
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest("/your/api/path", Method.GET);
                request.RequestFormat = DataFormat.Json;
                request.AddHeader("Authorization", "Bearer" + apiKey);
                request.AddBody(new
                {
                    ...//your body
                });
                System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true; // may need to remove

                IRestResponse restResponse = client.Execute(request);
                if (restResponse != null && restResponse.IsSuccessful)
                {
                    SampleResponse response = JsonConvert.DeserializeObject<SampleResponse>(restResponse.Content);
                    if (response != null && response.DataList != null && response.DataList.Length > 0)
                        return response.DataList;
                }
                else
                {
                    if (response == null)
                        log.Error("response is null");
                    else
                        log.Error("response is unsuccessful");
                }
            }
            catch (TimeoutException)
            {
                log.Error("API call time out");
            }
            catch (Exception ex)
            {
                log.Error("Exception ", ex);
            }

            return null;
        }
    }
	
	
	
	....
	usage
	....
	
	 Task.Run(() =>
                {
                    while (true)
                    {
                        log.Debug("Inside endless loop - " + DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss"));
                        Data[] dataList = SampleAPIHelper.ListenForUpdate();
                        if (dataList != null)
                            Task.Run(() => DoSomething(dataList));
                    }
                });