using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;
using System.Text;

namespace RevitPlugin.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class CurvesTotalLength : IExternalCommand
    {
        public class DetailLineFilter : ISelectionFilter
        {
            public bool AllowElement(Element element)
            {
                return element.Category.Id.IntegerValue.Equals(
                  (int)BuiltInCategory.OST_Lines);
            }
            public bool AllowReference(Reference r, XYZ p)
            {
                return false;
            }
        }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Get application and document objects
            UIApplication uiApp = commandData.Application;

            try
            {
                // Implement Selection Filter to select curves
                IList<Element> pickedRef = new List<Element>();
                Selection selection = uiApp.ActiveUIDocument.Selection;
                DetailLineFilter selFilter = new DetailLineFilter();
                pickedRef = selection.PickElementsByRectangle(selFilter, "Select lines");

                // Measure their total length
                List<double> lengthList = new List<double>();
                foreach (Element element in pickedRef)
                {
                    DetailLine line = element as DetailLine;
                    if (line != null)
                    {
                        lengthList.Add(line.GeometryCurve.Length);
                    }
                }

                // Return a message window that displays total length to user
                string lengthFeet = Math.Round(lengthList.Sum(), 2).ToString() + " ft";
                string lengthMeters = Math.Round(lengthList.Sum() * 0.3048, 2).ToString() + " m";
                string lengthMilimeters = Math.Round(lengthList.Sum() * 304.8, 2).ToString() + " mm";
                string lengthInch = Math.Round(lengthList.Sum() * 12, 2).ToString() + " inch";

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Total Length is:");
                sb.AppendLine(lengthFeet);
                sb.AppendLine(lengthInch);
                sb.AppendLine(lengthMeters);
                sb.AppendLine(lengthMilimeters);

                TaskDialog.Show("Line Length", sb.ToString());

                // Assuming that everything went right return Result.Succeeded
                return Result.Succeeded;
            }
            // This is where we "catch" potential errors and define how to deal with them
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                // If user decided to cancel the operation return Result.Canceled
                return Result.Cancelled;
            }
            catch (Exception exception)
            {
                // If something went wrong return Result.Failed
                message = exception.Message;
                return Result.Failed;
            }
        }
    }
}