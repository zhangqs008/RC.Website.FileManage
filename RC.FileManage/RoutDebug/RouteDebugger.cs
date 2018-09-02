using System.Web.Routing;

namespace RC.FileManage.RoutDebug
{
    /// <summary>
    ///     路由调试辅助类
    /// </summary>
    public static class RouteDebugger
    {
        /// <summary>
        ///     重写路由表(作为测试)
        /// </summary>
        /// <param name="routes"></param>
        public static void RewriteRoutesForTesting(RouteCollection routes)
        {
            using (routes.GetReadLock())
            {
                bool flag = false;
                foreach (RouteBase routeBase in routes)
                {
                    var route = routeBase as Route;
                    if (route != null)
                    {
                        route.RouteHandler = new DebugRouteHandler();
                    }
                    if (route == DebugRoute.Singleton)
                    {
                        flag = true;
                    }
                }
                if (flag)
                {
                    return;
                }
                (routes).Add(DebugRoute.Singleton);
            }
        }
    }
}