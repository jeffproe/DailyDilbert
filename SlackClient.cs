using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using Newtonsoft.Json;

// This is from https://gist.github.com/jogleasonjr/7121367

namespace Dilbert
{
	public class SlackClient
	{
		private readonly Uri _uri;
		private readonly Encoding _encoding = new UTF8Encoding();

		public SlackClient(string urlWithAccessToken)
		{
			_uri = new Uri(urlWithAccessToken);
		}

		//Post a message using simple strings
		public void PostMessage(string text, string username = null, string channel = null, string icon = null)
		{
			Payload payload = new Payload()
			{
				Channel = channel,
				Username = username,
				Text = text,
				Icon = icon
			};

			PostMessage(payload);
		}

		//Post a message using a Payload object
		public void PostMessage(Payload payload)
		{
			string payloadJson = JsonConvert.SerializeObject(payload);

			using (WebClient client = new WebClient())
			{
				NameValueCollection data = new NameValueCollection();
				data["payload"] = payloadJson;

				var response = client.UploadValues(_uri, "POST", data);

				//The response text is usually "ok"
				string responseText = _encoding.GetString(response);
			}
		}
	}
}