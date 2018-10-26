using System;
using System.Web;
using System.Web.Optimization;
using ItemDoc.Framework.Environment;

namespace ItemDoc.Web
{
  /// <summary>
  /// 
  /// </summary>
  public class BundleConfig
  {


    private static readonly bool EnableOptimizations = Config.AppSettings<bool>("BundleTable.EnableOptimizations", false);

    private static readonly string EnableMin = Config.AppSettings<string>("BundleTable.EnableMin", "");
    private static readonly string version = Config.AppSettings<string>("BundleTable.Version", "221");

    // 有关捆绑的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkId=301862
    public static void RegisterBundles(BundleCollection bundles)
    {

      //bundles.Clear();
      //bundles.ResetAll();

      BundleTable.EnableOptimizations = EnableOptimizations;
      bundles.UseCdn = false;   //enable CDN support


      var jqueryCdnPath = "http://apps.bdimg.com/libs/jquery/1.11.1/jquery.js";

      bundles.Add(new ScriptBundle("~/assets/common-js", jqueryCdnPath).Include( 
        "~/assets/layer/layer.js",
        "~/assets/common/common.js"
        ));
      //"~/assets/layer/layer.js",
      //"~/assets/lib/jquery-1.10.2.js",
      //"~/assets/bootstrap/js/bootstrap.min.js",

      bundles.Add(new StyleBundle("~/assets/common-css").Include(
        "~/assets/bootstrap/css/bootstrap.min.css",
        "~/assets/layer/theme/default/layer.css",
        "~/assets/common/common.css"
      ));
      bundles.Add(new ScriptBundle("~/assets/jquery.validate").Include(
        "~/assets/jquery.validate/jquery.validate.js",
        "~/assets/jquery.validate/jquery.validate.unobtrusive.js"
      ));


      bundles.Add(new ScriptBundle("~/assets/formvalidation").Include(
      "~/assets/formvalidation/dist/js/formValidation.js",
      "~/assets/formvalidation/dist/js/framework/bootstrap.js",
      "~/assets/formvalidation/dist/js/language/zh_CN.js"
      ));

      bundles.Add(new StyleBundle("~/assets/formvalidation").Include(
        "~/assets/formvalidation/dist/css/formValidation.css"
        ));

      bundles.Add(new ScriptBundle("~/assets/bootstrap-treeview").Include(
          "~/assets/bootstrap-treeview/js/bootstrap-treeview.js"
           ));

      bundles.Add(new StyleBundle("~/assets/bootstrap-treeview").Include(
        "~/assets/bootstrap-treeview/css/bootstrap-treeview.css"
        ));




      //bundles.Add(new StyleBundle("~/sopcss").Include(
      //  //<!-- Bootstrap v3.3.7 -->
      //  "~/scripts/bootstrap/css/bootstrap.min.css",
      //  "~/scripts/bootstrap/css/bootstrap-theme.min.css",
      //  //< !--Font Awesome 4.6.3-- >
      //  "~/scripts/plugins/font-awesome/css/font-awesome.min.css",
      //  "~/scripts/sopcom/common.css"
      //));
      //bundles.Add(new ScriptBundle("~/sopscript").Include(
      //  //< !--jQuery -->
      //  "~/scripts/jquery/jquery-1.11.1.min.js",
      //  //<!-- Bootstrap v3.3.5 -->
      //  "~/scripts/bootstrap/js/bootstrap.min.js",
      //  "~/scripts/sopcom/common.js"
      //));




      //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
      //            "~/Scripts/jquery-{version}.js"));

      //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
      //            "~/Scripts/jquery.validate*"));

      //// 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
      //// 生产准备就绪，请使用 https://modernizr.com 上的生成工具仅选择所需的测试。
      //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
      //            "~/Scripts/modernizr-*"));

      //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
      //          "~/Scripts/bootstrap.js",
      //          "~/Scripts/respond.js"));

      //bundles.Add(new StyleBundle("~/Content/css").Include(
      //          "~/Content/bootstrap.css",
      //          "~/Content/site.css"));
    }
  }
}
