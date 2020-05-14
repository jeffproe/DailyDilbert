using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace Dilbert.Function
{
	public static class OnceADay
	{
		private static TraceWriter _log;

		[FunctionName("OnceADay")]
		public static void Run([TimerTrigger("%ScheduleAppSetting%")]TimerInfo myTimer, TraceWriter log)
		{
			_log = log;
			log.Info($"C# Timer trigger function executed at: {DateTime.Now}");

			string urlWithAccessToken = GetEnvironmentVariable("WebhookUrl");
			string icon = GetIcon();
			string channel = GetChannel();
			string username = GetEnvironmentVariable("BotName");

			SlackClient client = new SlackClient(urlWithAccessToken);

			var payload = new Payload()
			{
				Icon = icon,
				Channel = channel,
				Text = $"http://dilbert.com/strip/{DateTime.Now.ToString("yyyy-MM-dd")}",
				Username = username
			};

			client.PostMessage(payload);
		}

		private static string GetEnvironmentVariable(string name)
		{
			string variable = System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
			_log.Info($"{name}: {variable}");
			return variable;
		}

		private static string GetIcon()
		{
			string icon = GetEnvironmentVariable("Icon");
			if (!icon.StartsWith(":"))
			{
				icon = $":{icon}";
			}
			if (!icon.EndsWith(":"))
			{
				icon = $"{icon}:";
			}
			return icon.ToLower();
		}

		private static string GetChannel()
		{
			string channel = GetEnvironmentVariable("Channel");
			if (!channel.StartsWith("#"))
			{
				channel = $"#{channel}";
			}
			return channel.ToLower(); ;
		}
	}
}
