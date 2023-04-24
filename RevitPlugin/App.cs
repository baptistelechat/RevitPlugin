
using System.Reflection;

using Autodesk.Revit.UI;
using System.Windows.Media.Imaging;

namespace RevitPlugin
{
    class App : IExternalApplication
    {
        // define a method that will create our tab and button
        static void AddRibbonPanel(RibbonPanel panel)
        {
            // Get dll assembly path
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            // create push button for CurveTotalLength
            PushButtonData pushButtonData = new PushButtonData(
                "cmdCurveTotalLength",
                "Total" + Environment.NewLine + "  Length  ",
                thisAssemblyPath,
                "RevitPlugin.Commands.CurvesTotalLength");

            PushButton pushButton = panel.AddItem(pushButtonData) as PushButton;
            pushButton.ToolTip = "Select Multiple Lines to Obtain Total Length";
            pushButton.Image =
                new BitmapImage(new Uri("pack://application:,,,/RevitPlugin;component/Resources/CurvesTotalLength_32x32.png"));
            pushButton.LargeImage =
                new BitmapImage(new Uri("pack://application:,,,/RevitPlugin;component/Resources/CurvesTotalLength_32x32.png"));
            pushButton.ToolTipImage =
                new BitmapImage(new Uri("pack://application:,,,/RevitPlugin;component/Resources/Cat.gif"));
        }

        public Result OnStartup(UIControlledApplication application)
        {
            // Call our method that will load up our toolbar

            // Create a custom ribbon tab
            string tabName = "RevitPlugin";
            application.CreateRibbonTab(tabName);

            // Add a new ribbon panel
            RibbonPanel ribbonPanelTools = application.CreateRibbonPanel(tabName, "Tools");

            AddRibbonPanel(ribbonPanelTools);

            return Result.Succeeded;
        }
        public Result OnShutdown(UIControlledApplication application)
        {
            // Do nothing
            return Result.Succeeded;
        }
    }
}