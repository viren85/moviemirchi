
namespace MvcWebRole1
{
    using System.Web.Optimization;

    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            // JS
            bundles.Add(new ScriptBundle("~/bundles/script/jquery").Include(
                 "~/content/jquery-2.1.3.min.js",
                 "~/content/jquery.cookie.js"));

            bundles.Add(new ScriptBundle("~/bundles/script/bootstrap").Include(
                 "~/content/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/script/prettyphoto").Include(
                 "~/content/jquery.prettyphoto.js"));

            bundles.Add(new ScriptBundle("~/bundles/script/moviecore").Include(
                 "~/content/movie*"));

            bundles.Add(new ScriptBundle("~/bundles/script/controls").IncludeDirectory(
                 "~/content/controls", "*.js", false));

            bundles.Add(new ScriptBundle("~/bundles/script/pages").IncludeDirectory(
                 "~/content/pages", "*.js", false));

            // CSS
            bundles.Add(new StyleBundle("~/bundles/style/prettyphoto").Include(
                "~/content/prettyphoto.css"));

            bundles.Add(new StyleBundle("~/bundles/style/jqueryui").IncludeDirectory(
                "~/content/themes/base", "*.css", false));

            bundles.Add(new StyleBundle("~/bundles/style/custom").Include(
                 "~/content/custom.css"));

            bundles.Add(new StyleBundle("~/bundles/style/controls").IncludeDirectory(
                 "~/content/styles", "*.css", false));

            ////BundleTable.EnableOptimizations = true;
        }
    }
}