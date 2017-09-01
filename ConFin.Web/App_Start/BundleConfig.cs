using System.Web.Optimization;

namespace ConFin.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // SignalR bundle
            bundles.Add(new ScriptBundle("~/bundles/SignalR").Include(
                        "~/Scripts/jquery.signalR-{version}.js"));
        }
    }
}