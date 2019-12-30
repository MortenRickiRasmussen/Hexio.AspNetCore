using System;
using System.Linq;

namespace Hexio.AspNetCore.Logging.Enrichers
{
    public static class RequestBodyFilter
    {
        private static readonly string[] Blacklist = new[] { "password", "pwd", "secret", "cpr", "cprnumber" };

        public static string Get(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "No body";
            }

            if (Blacklist.Any(x => text.IndexOf(x, StringComparison.InvariantCultureIgnoreCase) != -1))
            {
                return "Sensitive information removed";
            }

            if (text.Length > 100_000)
            {
                return "BODY TRUNCATED TO LENGTH 100_000" + Environment.NewLine +
                       text.Substring(0, 100_000);
            }

            return text;
        }
    }
}