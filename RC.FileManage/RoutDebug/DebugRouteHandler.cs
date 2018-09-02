// Type: RouteDebug.DebugRouteHandler
// Assembly: RouteDebug, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: C:\Users\zhangqs008\Desktop\RouteDebug.dll

using System.Web;
using System.Web.Routing;

namespace RC.FileManage.RoutDebug
{
    public class DebugRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new DebugHttpHandler
            {
                RequestContext = requestContext
            };
        }
    }
}