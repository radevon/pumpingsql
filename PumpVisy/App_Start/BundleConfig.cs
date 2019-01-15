using System.Web;
using System.Web.Optimization;

namespace PumpVisy
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/js/jquery-{version}.js")
                        .Include("~/js/bootstrap.js")
                        .Include("~/js/programm.js")
                        .Include("~/js/moment-with-locales.min.js")
                        .Include("~/js/moment-timezone-with-data.min.js"));

            

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
               "~/js/jqueryval/jquery.validate*"));

            

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/js/bootstrap.js"));
                      //"~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                      "~/css/bootstrap.css",
                      "~/css/main.css"));

           
        }
    }
}
