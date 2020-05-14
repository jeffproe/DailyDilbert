using Newtonsoft.Json;

namespace Dilbert
{
	public class Payload
	{
		public Payload()
		{
			UnfurlLinks = true;
			UnfurlMedia = true;
		}

		[JsonProperty("channel")]
		public string Channel { get; set; }

		[JsonProperty("username")]
		public string Username { get; set; }

		[JsonProperty("text")]
		public string Text { get; set; }

		[JsonProperty("icon_emoji")]
		public string Icon { get; set; }

		[JsonProperty("unfurl_media")]
		public bool UnfurlMedia { get; set; }

		[JsonProperty("unfurl_links")]
		public bool UnfurlLinks { get; set; }
	}
}