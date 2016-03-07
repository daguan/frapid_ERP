using System.Text.RegularExpressions;

namespace Frapid.WebsiteBuilder.Extensions
{
    public static class StringExtension
    {
        public static string ToAlias(this string input, int limit = 100)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            // remove entities
            input = Regex.Replace(input, @"&\w+;", "");
            // remove anything that is not letters, numbers, dash, or space
            input = Regex.Replace(input, @"[^A-Za-z0-9\-\s]", "");
            // remove any leading or trailing spaces left over
            input = input.Trim();
            // replace spaces with single dash
            input = Regex.Replace(input, @"\s+", "-");
            // if we end up with multiple dashes, collapse to single dash            
            input = Regex.Replace(input, @"\-{2,}", "-");
            // make it all lower case
            input = input.ToLower();
            // if it's too long, clip it
            if (input.Length > limit)
            {
                input = input.Substring(0, limit);
            }
            // remove trailing dash, if there is one
            if (input.EndsWith("-"))
            {
                input = input.Substring(0, input.Length - 1);
            }
            return input;
        }
    }
}