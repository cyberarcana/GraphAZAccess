using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GraphAZLib
{
    public static class LogAnalyticsHelper
    {
		//// Update customerId to your Log Analytics workspace ID
		//static string customerId = "X";

		//// For sharedKey, use either the primary or the secondary Connected Sources client authentication key   
		//static string sharedKey = "X";

		//// You can use an optional field to specify the timestamp from the data. If the time field is not specified, Azure Monitor assumes the time is the message ingestion time
		//static string TimeStampField = "";

		public static void PostNeo4JDb(string aLogAnalyticsWSID, string aSharedKey, string aJsonString, string aLogType)
		{
			// Create a hash for the API signature
			var datestring = DateTime.UtcNow.ToString("r");
			var jsonBytes = Encoding.UTF8.GetBytes(aJsonString);

			string stringToHash = "POST\n" + jsonBytes.Length + "\napplication/json\n" + "x-ms-date:" + datestring + "\n/api/logs";
			string hashedString = BuildSignature(stringToHash, aSharedKey);
			string signature = "SharedKey " + aLogAnalyticsWSID + ":" + hashedString;

			PostData(aLogAnalyticsWSID, signature, datestring, aJsonString, aLogType);
		}

		// Build the API signature
		private static string BuildSignature(string message, string secret)
		{
			var encoding = new System.Text.ASCIIEncoding();
			byte[] keyByte = Convert.FromBase64String(secret);
			byte[] messageBytes = encoding.GetBytes(message);
			using (var hmacsha256 = new HMACSHA256(keyByte))
			{
				byte[] hash = hmacsha256.ComputeHash(messageBytes);
				return Convert.ToBase64String(hash);
			}
		}

		// Send a request to the POST API endpoint
		private static void PostData(string aLogAnalyticsWSID, string signature, string date, string json, string aLogType)
		{
			try
			{
				string url = "https://" + aLogAnalyticsWSID + ".ods.opinsights.azure.com/api/logs?api-version=2016-04-01";

				System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
				client.DefaultRequestHeaders.Add("Accept", "application/json");
				client.DefaultRequestHeaders.Add("Log-Type", aLogType);
				client.DefaultRequestHeaders.Add("Authorization", signature);
				client.DefaultRequestHeaders.Add("x-ms-date", date);
				client.DefaultRequestHeaders.Add("time-generated-field", "");

				System.Net.Http.HttpContent httpContent = new StringContent(json, Encoding.UTF8);
				httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
				Task<System.Net.Http.HttpResponseMessage> response = client.PostAsync(new Uri(url), httpContent);

				System.Net.Http.HttpContent responseContent = response.Result.Content;				
				
				string result = responseContent.ReadAsStringAsync().Result;
				Console.WriteLine($"Log Analytics API Return Result for {aLogType} was: {result}. Status code was: {response.Result.StatusCode}");
			}
			catch (Exception excep)
			{
				Console.WriteLine("API Post Exception: " + excep.Message);
			}
		}
	}
}
