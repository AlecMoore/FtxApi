namespace FtxApi
{
    public class Client
    {
        public string ApiKey { get; }

        public string ApiSecret { get; }

        public string SubAccount { get; }

        public Client()
        {
            ApiKey = "WKlrgG3yaO5wwy1f-L0hwJk8LlFtYENCLA9cptee";
            ApiSecret = "";
            SubAccount = "BTC";
        }

        public Client(string apiKey, string apiSecret, string subAccount)
        {
            ApiKey = apiKey;
            ApiSecret = apiSecret;
            SubAccount = SubAccount;
        }

    }
}