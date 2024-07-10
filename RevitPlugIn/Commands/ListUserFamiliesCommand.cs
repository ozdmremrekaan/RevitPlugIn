using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using RevitPlugIn.Forms;

namespace RevitPlugIn.Commands
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class ListUserFamiliesCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            using (UserFamilyForm form = new UserFamilyForm(doc))
            {
                form.ShowDialog();
            }

            return Result.Succeeded;
        }

        public static string GetPath()
        {
            return typeof(ListUserFamiliesCommand).Namespace + "." + nameof(ListUserFamiliesCommand);
        }
    }
}
