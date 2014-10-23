
namespace MvcWebRole1
{
    using System.Web.Optimization;

    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            // JS
            {
                //// TODO: Investigate if we should bundle jquery and bootstrap or simply continue to use the .min.js
                //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                //     "~/content/jquery-{version}.js"));

                //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                //     "~/content/bootstrap*"));

                bundles.Add(new ScriptBundle("~/bundles/script/prettyphoto").Include(
                     "~/content/jquery.prettyphoto.js"));

                bundles.Add(new ScriptBundle("~/bundles/script/moviecore").Include(
                     "~/content/movie*"));

                bundles.Add(new ScriptBundle("~/bundles/script/controls").IncludeDirectory(
                     "~/content/controls", "*.js", false));
            }

            // CSS
            {
                //// TODO: Investigate if we should bundle bootstrap or simply continue to use the .min.css
                //bundles.Add(new StyleBundle("~/bundles/bootstrap").Include(
                //     "~/content/bootstrap*"));

                bundles.Add(new StyleBundle("~/bundles/style/prettyphoto").Include(
                    "~/content/prettyphoto.css"));

                bundles.Add(new StyleBundle("~/bundles/style/jqueryui").IncludeDirectory(
                    "~/content/themes/base", "*.css", false));

                bundles.Add(new StyleBundle("~/bundles/style/custom").Include(
                     "~/content/custom.css"));

                bundles.Add(new StyleBundle("~/bundles/style/controls").IncludeDirectory(
                     "~/content/styles", "*.css", false));
            }
 
            ////BundleTable.EnableOptimizations = true;
        }
    }
}