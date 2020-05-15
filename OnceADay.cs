using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Dilbert.Function
{
	public static class OnceADay
	{
		private static ILogger _log;

		[FunctionName("OnceADay")]
		public static void RunTimer([TimerTrigger("%DailySchedule%")] TimerInfo myTimer, ILogger log)
		{
			_log = log;
			PostDilbert();
		}

		// [FunctionName("HttpTrigger")]
		// public static void RunHttp([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req, ILogger log)
		// {
		// 	_log = log;
		// 	PostDilbert();
		// }

		private static void PostDilbert()
		{
			_log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

			string urlWithAccessToken = GetEnvironmentVariable("WebhookUrl");
			string icon = GetIcon();
			string channel = GetChannel();
			string username = GetEnvironmentVariable("BotName");

			SlackClient client = new SlackClient(urlWithAccessToken);

			var payload = new Payload()
			{
				Icon = icon,
				Channel = channel,
				Text = $"https://dilbert.com/strip/{DateTime.Now.ToString("yyyy-MM-dd")}",
				Username = username
			};

			client.PostMessage(payload);
		}

		private static string GetEnvironmentVariable(string name)
		{
			string variable = System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
			_log.LogInformation($"{name}: {variable}");
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
