/*
 * Created on 2023
 *
 * Copyright (c) 2023 dotmobstudio
 * Support : dotmobstudio@gmail.com
 */
using System;
using UnityEngine;

namespace dotmob
{
	public static class ConvertFormat
	{
		private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public static string ConvertNumberToCurrency(int number)
		{
			return $"{number:#,0}";
		}

		public static string ConvertTimToMS(int sec)
		{
			string str = $"{sec / 60:D2}";
			string str2 = $"{sec % 60:D2}";
			return str + ":" + str2;
		}

		public static string ConvertTimeToHMS(int sec)
		{
			string text = $"{sec / 3600:D2}";
			string text2 = $"{sec % 3600 / 60:D2}";
			string text3 = $"{sec % 60:D2}";
			return text + ":" + text2 + ":" + text3;
		}

		public static long ConvertToTimestamp(DateTime value)
		{
			return (long)(value - Epoch).TotalSeconds;
		}

		public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
		{
			return Epoch.AddSeconds(unixTimeStamp).ToLocalTime();
		}
		public static DateTime TimeStampToDataTime(long timeStamp)
		{
            DateTime utcTime = DateTimeOffset.FromUnixTimeMilliseconds(timeStamp).UtcDateTime;
            TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, localTimeZone);
            return localTime;
        }
		public static TimeSpan GetRemainTime(string eventStartTime, string eventEndTime)
		{
			DateTime result = default(DateTime);
			DateTime result2 = default(DateTime);
			if (!DateTime.TryParse(eventStartTime, out result) || !DateTime.TryParse(eventEndTime, out result2))
			{
				return default(TimeSpan);
			}
			return result2 - DateTime.UtcNow;
		}

		public static Color ConvertColor(int r, int g, int b)
		{
			return new Color((float)r / 255f, (float)g / 255f, (float)b / 255f);
		}

		public static Color ConvertColor(int r, int g, int b, int a)
		{
			return new Color((float)r / 255f, (float)g / 255f, (float)b / 255f, (float)a / 255f);
		}
	}
}
