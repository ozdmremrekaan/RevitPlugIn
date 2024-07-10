using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using RevitPlugIn.Forms;
using RevitPlugIn.Utils;

namespace RevitPlugIn.Commands
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class LoadFamiliesCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            AdminFamilyForm adminFamilyForm = new AdminFamilyForm(doc);
            adminFamilyForm.ShowDialog();
            return Result.Succeeded;
        }

   
        public static string GetPath()
        {
            return typeof(LoadFamiliesCommand).Namespace + "." + nameof(LoadFamiliesCommand);
        }
    }
}
