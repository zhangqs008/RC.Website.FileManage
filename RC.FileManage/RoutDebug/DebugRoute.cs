// Type: RouteDebug.DebugRoute
// Assembly: RouteDebug, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: C:\Users\zhangqs008\Desktop\RouteDebug.dll

using System.Web.Routing;

namespace RC.FileManage.RoutDebug
{
    public class DebugRoute : Route
    {
        private static readonly DebugRoute singleton = new DebugRoute();

        static DebugRoute()
        {
        }

        private DebugRoute()
            : base("{*catchall}", new DebugRouteHandler())
        {
        }

        public static DebugRoute Singleton
        {
            get { return singleton; }
        }
    }
}