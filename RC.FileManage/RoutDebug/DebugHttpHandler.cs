using System.Linq;
using System.Web;
using System.Web.Routing;

namespace RC.FileManage.RoutDebug
{
    public class DebugHttpHandler : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            const string format =
                "<html>\r\n<head>\r\n    <title>Route Tester</title>\r\n    <style>\r\n        body, td, th {{font-family: verdana; font-size: small;}}\r\n        .message {{font-size: .9em;}}\r\n        caption {{font-weight: bold;}}\r\n        tr.header {{background-color: #ffc;}}\r\n        label {{font-weight: bold; font-size: 1.1em;}}\r\n        .false {{color: #c00;}}\r\n        .true {{color: #0c0;}}\r\n    </style>\r\n</head>\r\n<body>\r\n<h1>Route Tester</h1>\r\n<div id=\"main\">\r\n    <p class=\"message\">\r\n        Type in a url in the address bar to see which defined routes match it. \r\n        A {{*catchall}} route is added to the list of routes automatically in \r\n        case none of your routes match.\r\n    </p>\r\n    <p><label>Route</label>: {1}</p>\r\n    <div style=\"float: left;\">\r\n        <table border=\"1\" cellpadding=\"3\" cellspacing=\"0\" width=\"300\">\r\n            <caption>Route Data</caption>\r\n            <tr class=\"header\"><th>Key</th><th>Value</th></tr>\r\n            {0}\r\n        </table>\r\n    </div>\r\n    <div style=\"float: left; margin-left: 10px;\">\r\n        <table border=\"1\" cellpadding=\"3\" cellspacing=\"0\" width=\"300\">\r\n            <caption>Data Tokens</caption>\r\n            <tr class=\"header\"><th>Key</th><th>Value</th></tr>\r\n            {4}\r\n        </table>\r\n    </div>\r\n    <hr style=\"clear: both;\" />\r\n    <table border=\"1\" cellpadding=\"3\" cellspacing=\"0\">\r\n        <caption>All Routes</caption>\r\n        <tr class=\"header\">\r\n            <th>Matches Current Request</th>\r\n            <th>Url</th>\r\n            <th>Defaults</th>\r\n            <th>Constraints</th>\r\n            <th>DataTokens</th>\r\n        </tr>\r\n        {2}\r\n    </table>\r\n    <hr />\r\n    <strong>AppRelativeCurrentExecutionFilePath</strong>: {3}\r\n</div>\r\n</body>\r\n</html>";
            string routeDatas = string.Empty;
            RouteData routeData = RequestContext.RouteData;
            RouteValueDictionary values = routeData.Values;
            RouteBase route = routeData.Route;
            string s = string.Empty;
            using (RouteTable.Routes.GetReadLock())
            {
                foreach (RouteBase routeBase in RouteTable.Routes)
                {
                    string matched = string.Format("<span class=\"{0}\">{0}</span>",
                                                   (routeBase.GetRouteData(RequestContext.HttpContext) != null
                                                        ? "true"
                                                        : "false"));
                    string url = "n/a";
                    string defaults = "n/a";
                    string constraints = "n/a";
                    string dataTokens = "n/a";
                    var temp = routeBase as Route;
                    if (temp != null)
                    {
                        url = temp.Url;
                        defaults = FormatRouteValueDictionary(temp.Defaults);
                        constraints = FormatRouteValueDictionary(temp.Constraints);
                        dataTokens = FormatRouteValueDictionary(temp.DataTokens);
                    }
                    s = s +
                        string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{3}</td></tr>",
                                      (object) matched, (object) url, (object) defaults, (object) constraints,
                                      (object) dataTokens);
                }
            }
            string matchResult = "n/a";
            string tokens = "";
            if (!(route is DebugRoute))
            {
                foreach (string index in values.Keys)
                    routeDatas = routeDatas +
                                 string.Format("\t<tr><td>{0}</td><td>{1}&nbsp;</td></tr>", index, values[index]);
                foreach (string index in routeData.DataTokens.Keys)
                    tokens = tokens +
                             string.Format("\t<tr><td>{0}</td><td>{1}&nbsp;</td></tr>", index,
                                           routeData.DataTokens[index]);
                var route2 = route as Route;
                if (route2 != null)
                    matchResult = route2.Url;
            }
            else
            {
                matchResult = "<strong class=\"false\">NO MATCH!</strong>";
            }
            context.Response.Write(string.Format(format, (object) routeDatas, (object) matchResult, (object) s,
                                                 (object) context.Request.AppRelativeCurrentExecutionFilePath,
                                                 (object) tokens));
        }

        private static string FormatRouteValueDictionary(RouteValueDictionary values)
        {
            if (values == null)
                return "(null)";
            string str = values.Keys.Aggregate(string.Empty,
                                               (current, index) =>
                                               current + string.Format("{0} = {1}, ", index, values[index]));
            if (str.EndsWith(", "))
                str = str.Substring(0, str.Length - 2);
            return str;
        }
    }
}