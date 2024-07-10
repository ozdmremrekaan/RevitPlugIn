using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitPlugInV2.Interface;
using RevitPlugInV2.Utils;

namespace RevitPlugInV2.Commands
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class LogOutCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UserUtils.Logout();

            Autodesk.Revit.UI.TaskDialog.Show("Logout", "You have been logged out successfully.");
            SetupInterface.CheckButtonVisibility();

            return Result.Succeeded;
        }
        public static string GetPath()
        {
            return typeof(LogOutCommand).Namespace + "." + nameof(LogOutCommand);
        }

    }

}
