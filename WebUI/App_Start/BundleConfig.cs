using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Inv.WebUI.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
           




            bundles.Add(new StyleBundle("~/Bundles/AppStyle2")
                .Include( 
                 "~/Scripts/IgGrid/infragistics.css",
                 "~/Scripts/jsgrid/jsgrid.min.css",
                 "~/Scripts/jsgrid/jsgrid-theme.min.css",
                 "~/Scripts/jsgrid/jsgrid-theme.css",
                 "~/Content/DataTables/css/jquery.dataTables.min.css"));

            bundles.Add(new ScriptBundle("~/Bundles/AppScript2")
              .Include( 
                "~/js/my_js.js",
                "~/NewStyle/js/bootstrap.min.js",
                "~/NewStyle/js/jquery.meanmenu.js",
                "~/NewStyle/js/jquery.mCustomScrollbar.concat.min.js",
                "~/NewStyle/js/jquery.sticky.js",
                "~/NewStyle/js/jquery.scrollUp.min.js",
                "~/NewStyle/js/counterup/jquery.counterup.min.js",
                "~/NewStyle/js/counterup/waypoints.min.js",
                //"~/NewStyle/js/dropzone.js",
                "~/NewStyle/js/multiple-email/multiple-email-active.js",
                "~/NewStyle/js/summernote.min.js",
                "~/NewStyle/js/summernote-active.js"));

            bundles.Add(new ScriptBundle("~/Bundles/AppScript3")
              .Include( 
                "~/ClientApp/Entities.js",  
                "~/ClientApp/MessageBox.js" ));


           
        }
    }
}