namespace Saltyeyes.Cephalon {
    using Saltyeyes.Cephalon.Properties;
    using System;
    using System.Collections.Generic;

    public static class Utils {

        private static readonly List<String> Vowels = new List<String>() { "a", "e", "i", "o", "u"};

        public static string RandomPick(string listText) {
            var list = listText.Split('|');
            var random = new Random();
            return list[random.Next(list.Length)];
        }

        public static string RandomSSML(string listText) {
            var list = Resources.ResourceManager.GetString(listText).Split('|');
            var random = new Random();
            return list[random.Next(list.Length)];
        }

        public static string Article(string text) {
            return Vowels.Contains(text.Substring(0, 1).ToLower()) ? "an" : "a";
        }

        public static string TimeRemaining(DateTimeOffset dateTime) {
            TimeSpan timeRemaining = (dateTime - DateTimeOffset.Now);
            List<String> timeParts = new List<String>();

            if (timeRemaining.Hours >= 1) {
                timeParts.Add(string.Format(Resources.ResourceManager.GetString("TimePartHour"), timeRemaining.Hours, timeRemaining.Hours == 1 ? "" : "s"));
            }
            if (timeRemaining.Minutes >= 1) {
                timeParts.Add(string.Format(Resources.ResourceManager.GetString("TimePartMinute"), timeRemaining.Minutes, timeRemaining.Minutes == 1 ? "" : "s"));
            }
            if (timeRemaining.Seconds >= 1) {
                timeParts.Add(string.Format(Resources.ResourceManager.GetString("TimePartSecond"), timeRemaining.Seconds, timeRemaining.Seconds == 1 ? "" : "s"));
            }

            List<String> timePartResources = new List<string>() {
                "TimeReadableOnePart",
                "TimeReadableTwoParts",
                "TimeReadableThreeParts"
            };

            String timePartStringName = timePartResources[timeParts.Count - 1];
            String timePartString = Resources.ResourceManager.GetString(timePartStringName);

            return string.Format(timePartString, timeParts.ToArray());
        }
    }
}