using System.Collections.Generic;
using System.Linq;
using Frapid.AssetBundling;
using Frapid.Configuration;
using Frapid.Framework.Extensions;
using Frapid.Framework.StaticContent;
using Serilog;

namespace Frapid.Web
{
    public static class AssetConfig
    {
        private static bool IsDevelopment()
        {
            string value = ConfigurationManager.GetConfigurationValue("ParameterConfigFileLocation", "IsDevelopment");
            return value.Or("").ToUpperInvariant().StartsWith("T");
        }

        public static void Register()
        {
            Log.Verbose("Registering default assets.");

            bool dev = IsDevelopment();

            Log.Information(dev
                ? "Development mode was detected. Compression will not be performed."
                : "Production mode was detected. We will try to compress assets.");

            RegisterAsset(AssetType.JavaScript, dev, "frapid-core.js", GetFrapidCoreScript());
            RegisterAsset(AssetType.JavaScript, dev, "libraries.js", GetLibrariesScript());
            RegisterAsset(AssetType.JavaScript, dev, "backend.js", GetDashboardScript());
            RegisterAsset(AssetType.JavaScript, dev, "scrudfactory-view.js", GetScrudFactoryView());
            RegisterAsset(AssetType.JavaScript, dev, "scrudfactory-form.js", GetScrudFactoryForm());

            RegisterAsset(AssetType.StyleSheet, dev, "master-page.css", GetMasterPageStyle());
            RegisterAsset(AssetType.StyleSheet, dev, "assets/css/master-page-rtl.css", GetMasterPageRtlStyle());
            RegisterAsset(AssetType.StyleSheet, dev, "report.css", GetReportStyle());
            RegisterAsset(AssetType.StyleSheet, dev, "report-rtl.css", GetReportRtlStyle());

            //You can register your own assets on each of your modules by implementing IRegisterableAssets.
            //The above assets can also be overriden by your own registerable assets.
            PerformDiscovery();
        }

        private static void PerformDiscovery()
        {
            Log.Verbose("Discovering registrable assets.");
            var type = typeof (IRegisterableAssets);
            var members = type.GetTypeMembers<IRegisterableAssets>();

            foreach (var registerable in members)
            {
                if (registerable == null)
                {
                    continue;
                }

                Log.Information("Registering asset group \"{AssetGroupName}\" of App \"{AppName}\"", registerable.AssetGroupName, registerable.AppName);

                foreach (var asset in registerable.Assets)
                {
                    Log.Information("Registering asset bundle \"{BundleName}\" from group \"{AssetGroupName}\" of App \"{AppName}\"", asset.BundleName, registerable.AssetGroupName, registerable.AppName);
                    RegisterAsset(asset);
                }
            }
        }

        private static void RegisterAsset(AssetType type, bool isDevelopment, string name, string[] files)
        {
            Log.Information("Registering asset \"{name}\" having {count} files.", name, files.Length);

            var asset = new Asset
            {
                Type = type,
                IsDevelopment = isDevelopment,
                BundleName = name,
                Files = files,
                CacheDurationInMinutes = 60*24*7
            };

            RegisterAsset(asset);
        }

        private static void RegisterAsset(Asset asset)
        {
            var bundle = new Registration(asset);
            bundle.Resiter();
        }

        #region Scripts

        private static string[] GetScrudFactoryView()
        {
            return new[]
            {
                "~/Scripts/Modules/ScrudFactory/view/block-begin.js",
                "~/Scripts/Modules/ScrudFactory/view/initialization.js",
                "~/Scripts/Modules/ScrudFactory/shared/conversion.js",
                "~/Scripts/Modules/ScrudFactory/shared/field-factory.js",
                "~/Scripts/Modules/ScrudFactory/shared/scrud-class.js",
                "~/Scripts/Modules/ScrudFactory/shared/localization.js",
                "~/Scripts/Modules/ScrudFactory/shared/view.js",
                "~/Scripts/Modules/ScrudFactory/view/add-button.js",
                "~/Scripts/Modules/ScrudFactory/view/ajax-load-page-count.js",
                "~/Scripts/Modules/ScrudFactory/view/buttons-action.js",
                "~/Scripts/Modules/ScrudFactory/view/buttons-custom.js",
                "~/Scripts/Modules/ScrudFactory/view/buttons.js",
                "~/Scripts/Modules/ScrudFactory/view/check.js",
                "~/Scripts/Modules/ScrudFactory/view/export.js",
                "~/Scripts/Modules/ScrudFactory/view/filters-ajax.js",
                "~/Scripts/Modules/ScrudFactory/view/filters.js",
                "~/Scripts/Modules/ScrudFactory/view/gridview-actions-delete.js",
                "~/Scripts/Modules/ScrudFactory/view/gridview-actions-edit-ajax.js",
                "~/Scripts/Modules/ScrudFactory/view/gridview-actions-edit.js",
                "~/Scripts/Modules/ScrudFactory/view/gridview-actions-get-key.js",
                "~/Scripts/Modules/ScrudFactory/view/gridview-actions.js",
                "~/Scripts/Modules/ScrudFactory/view/gridview-ajax.js",
                "~/Scripts/Modules/ScrudFactory/view/gridview.js",
                "~/Scripts/Modules/ScrudFactory/view/import-export.js",
                "~/Scripts/Modules/ScrudFactory/view/import.js",
                "~/Scripts/Modules/ScrudFactory/view/flag.js",
                "~/Scripts/Modules/ScrudFactory/view/kanban-cards-ajax.js",
                "~/Scripts/Modules/ScrudFactory/view/kanban-cards.js",
                "~/Scripts/Modules/ScrudFactory/view/kanban-ratable.js",
                "~/Scripts/Modules/ScrudFactory/view/kanban-segment-ajax.js",
                "~/Scripts/Modules/ScrudFactory/view/kanban-segment.js",
                "~/Scripts/Modules/ScrudFactory/view/kanban-sortable.js",
                "~/Scripts/Modules/ScrudFactory/view/shared/localization.js",
                "~/Scripts/Modules/ScrudFactory/view/meta-definition.js",
                "~/Scripts/Modules/ScrudFactory/view/paging.js",
                "~/Scripts/Modules/ScrudFactory/view/procedure-annotation.js",
                "~/Scripts/Modules/ScrudFactory/view/query-string.js",
                "~/Scripts/Modules/ScrudFactory/view/select.js",
                "~/Scripts/Modules/ScrudFactory/view/verification-ajax.js",
                "~/Scripts/Modules/ScrudFactory/view/verification.js",
                "~/Scripts/Modules/ScrudFactory/view/view-components.js",
                "~/Scripts/Modules/ScrudFactory/view/view-shared.js",
                "~/Scripts/Modules/ScrudFactory/view/view-success.js",
                "~/Scripts/Modules/ScrudFactory/view/events-on-doc-ready.js",
                "~/Scripts/Modules/ScrudFactory/view/events-on-viewready.js",
                "~/Scripts/Modules/ScrudFactory/view/block-end.js"
            };
        }

        private static string[] GetScrudFactoryForm()
        {
            return new[]
            {
                "~/Scripts/Modules/ScrudFactory/form/block-begin.js",
                "~/Scripts/Modules/ScrudFactory/form/initialization.js",
                "~/Scripts/Modules/ScrudFactory/shared/conversion.js",
                "~/Scripts/Modules/ScrudFactory/shared/field-factory.js",
                "~/Scripts/Modules/ScrudFactory/shared/scrud-class.js",
                "~/Scripts/Modules/ScrudFactory/shared/localization.js",
                "~/Scripts/Modules/ScrudFactory/shared/view.js",
                "~/Scripts/Modules/ScrudFactory/form/title.js",
                "~/Scripts/Modules/ScrudFactory/form/url.js",
                "~/Scripts/Modules/ScrudFactory/form/form-cancel.js",
                "~/Scripts/Modules/ScrudFactory/form/form-custom-fields.js",
                "~/Scripts/Modules/ScrudFactory/form/form-edit.js",
                "~/Scripts/Modules/ScrudFactory/form/form-navigation.js",
                "~/Scripts/Modules/ScrudFactory/form/form-actions.js",
                "~/Scripts/Modules/ScrudFactory/form/form-layout-field-group.js",
                "~/Scripts/Modules/ScrudFactory/form/form-layout.js",
                "~/Scripts/Modules/ScrudFactory/form/form-load-default-values.js",
                "~/Scripts/Modules/ScrudFactory/form/form-load-dropdowns.js",
                "~/Scripts/Modules/ScrudFactory/form/form-reset.js",
                "~/Scripts/Modules/ScrudFactory/form/form-save.js",
                "~/Scripts/Modules/ScrudFactory/form/form-validate.js",
                "~/Scripts/Modules/ScrudFactory/form/form-create.js",
                "~/Scripts/Modules/ScrudFactory/form/ajax-request-initialize.js",
                "~/Scripts/Modules/ScrudFactory/form/event-on-doc-keydown.js",
                "~/Scripts/Modules/ScrudFactory/form/event-on-doc-ready.js",
                "~/Scripts/Modules/ScrudFactory/form/visibility.js",
                "~/Scripts/Modules/ScrudFactory/form/trigger-events.js",
                "~/Scripts/Modules/ScrudFactory/form/event-on-ajax-stop.js",
                "~/Scripts/Modules/ScrudFactory/form/block-end.js"
            };
        }

        private static string[] GetFrapidCoreScript()
        {
            return new[]
            {
                "~/Scripts/frapid/core/dom/checkable.js",
                "~/Scripts/frapid/core/dom/document.js",
                "~/Scripts/frapid/core/dom/frame.js",
                "~/Scripts/frapid/core/dom/events.js",
                "~/Scripts/frapid/core/dom/uploader.js",
                "~/Scripts/frapid/core/dom/popunder.js",
                "~/Scripts/frapid/core/dom/select.js",
                "~/Scripts/frapid/core/grid/cell.js",
                "~/Scripts/frapid/core/grid/print-grid.js",
                "~/Scripts/frapid/core/libraries/chartjs.js",
                "~/Scripts/frapid/core/libraries/jquery-notify.js",
                "~/Scripts/frapid/core/libraries/semantic-ui.js",
                "~/Scripts/frapid/core/prototype/string.js",
                "~/Scripts/frapid/core/array.js",
                "~/Scripts/frapid/core/conversion.js",
                "~/Scripts/frapid/core/date-expressions.js",
                "~/Scripts/frapid/core/enums.js",
                "~/Scripts/frapid/core/browser.js",
                "~/Scripts/frapid/core/json.js",
                "~/Scripts/frapid/core/localization.js",
                "~/Scripts/frapid/core/location.js",
                "~/Scripts/frapid/core/frapid-ajax.js",
                "~/Scripts/frapid/core/validation.js",
                "~/Scripts/frapid/core/window.js",
                "~/Scripts/frapid/core/dom/checkable.js",
                "~/Scripts/frapid/core/dom/document.js",
                "~/Scripts/frapid/core/dom/events.js",
                "~/Scripts/frapid/core/dom/popunder.js",
                "~/Scripts/frapid/core/dom/select.js",
                "~/Scripts/frapid/core/grid/cell.js",
                "~/Scripts/frapid/core/grid/print-grid.js",
                "~/Scripts/frapid/core/libraries/chartjs.js",
                "~/Scripts/frapid/core/libraries/jquery-notify.js",
                "~/Scripts/frapid/core/libraries/semantic-ui.js",
                "~/Scripts/frapid/core/prototype/string.js"
            };
        }

        private static string[] GetLibrariesScript()
        {
            string[] libraries =
            {
                "~/Scripts/jquery-2.2.3.js",
                "~/Scripts/jquery.address.js",
                "~/Scripts/jquery-ui.js",
                "~/Scripts/jquery-timePicker/jquery.timepicker.js",
                "~/Scripts/shortcut.js",
                "~/Scripts/jqueryNumber/jquery.number.js",
                "~/Scripts/date.js",
                "~/Scripts/chartjs/Chart.min.js",
                "~/Scripts/chartjs/legend.js",
                "~/Scripts/notify-combined.min.js",
                "~/Scripts/semantic-ui/semantic.js",
                "~/Scripts/jquery.signalR.js",
                "~/Scripts/vakata-jstree/dist/jstree.min.js",
                "~/Scripts/momentjs/moment-with-locales.js",
                "~/Scripts/underscore/underscore-min.js",
                "~/Scripts/angular/angular.min.js",
                "~/Scripts/linq.js/linq.js"
            };

            libraries = libraries.Concat(GetFrapidCoreScript()).ToArray();

            return libraries;
        }

        private static string[] GetDashboardScript()
        {
            var script = new List<string>
            {
                "~/Scripts/jquery-1.9.1.js",
                "~/Scripts/angular/angular.min.js",
                "~/Scripts/jquery.address.js",
                "~/Scripts/jquery-ui.js",
                "~/Scripts/jquery-timepicker/jquery.timepicker.js",
                "~/Scripts/shortcut.js",
                "~/Scripts/papaparse/papaparse.min.js",
                "~/Scripts/jqueryNumber/jquery.number.js",
                "~/Scripts/date.js",
                "~/Scripts/chartjs/Chart.min.js",
                "~/Scripts/chartjs/legend.js",
                "~/Scripts/notify-combined.min.js",
                "~/Scripts/semantic-ui/semantic.js",
                "~/Scripts/jquery.signalR.js",
                "~/Scripts/vakata-jstree/dist/jstree.min.js",
                "~/Scripts/momentjs/moment-with-locales.js",
                "~/Scripts/underscore/underscore-min.js",
                "~/Scripts/sprintf/sprintf.min.js",
                "~/Scripts/linq.js/linq.js"
            };

            script.AddRange(GetFrapidCoreScript());
            return script.ToArray();
        }

        #endregion

        #region Stylesheets

        private static string[] GetMasterPageStyle()
        {
            return new[]
            {
                "~/Scripts/colorbox/colorbox.css",
                "~/Scripts/jquery-ui/css/custom-theme/jquery-ui.css",
                "~/Scripts/jquery-timepicker/jquery.timepicker.css",
                "~/Scripts/vakata-jstree/dist/themes/default/style.css",
                "~/Scripts/semantic-ui/semantic.css",
                "~/Stylesheets/Backend/frapid.css"
            };
        }


        private static string[] GetMasterPageRtlStyle()
        {
            return new[]
            {
                "~/Stylesheets/rtl.css",
                "~/Scripts/colorbox/colorbox.css",
                "~/Scripts/jquery-ui/css/custom-theme/jquery-ui.css",
                "~/Scripts/jquery-timepicker/jquery.timepicker.css",
                "~/Scripts/vakata-jstree/dist/themes/default/style.css",
                "~/Scripts/semantic-ui/semantic.rtl.css",
                "~/Stylesheets/Backend/frapid.css"
            };
        }

        private static string[] GetReportStyle()
        {
            return new[]
            {
                "~/Scripts/semantic-ui/components/icon.css",
                "~/Stylesheets/report.css"
            };
        }

        private static string[] GetReportRtlStyle()
        {
            return new[]
            {
                "~/Scripts/semantic-ui/components/icon.rtl.css",
                "~/Stylesheets/report.rtl.css"
            };
        }

        #endregion
    }
}