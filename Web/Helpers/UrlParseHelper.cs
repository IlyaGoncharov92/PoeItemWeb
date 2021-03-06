﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Web.Helpers
{
    public class UrlParseHelper
    {
        public static string UrlParse(string url)
        {
            var poeTradeHost = ConfigurationManager.AppSettings["PoeTradeHost"];

            var indexStart = url.IndexOf(poeTradeHost, StringComparison.Ordinal);

            if (indexStart == -1)
                return string.Empty;

            var indexEnd = indexStart + poeTradeHost.Length;

            var urlParams = url.Substring(indexEnd);

            return urlParams;
        }

        public static string UrlWikiParse(string url)
        {
            var wikiHost = ConfigurationManager.AppSettings["PoeWikiHost"];

            var indexStart = url.IndexOf(wikiHost, StringComparison.Ordinal);

            if (indexStart == -1)
                return string.Empty;

            var indexEnd = indexStart + wikiHost.Length;

            var urlParams = url.Substring(indexEnd);

            return urlParams;
        }
    }
}