using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RevitPlugInV2.Forms;

namespace RevitPlugInV2.Commands
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class UploadFamilyCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            using (Transaction transaction = new Transaction(doc, "Upload Family"))
            {
                transaction.Start();

                // Close the transaction before opening the form
                transaction.Commit();
            }

            // Open the form after closing the transaction
            using (FamilyUploadForm form = new FamilyUploadForm(doc))
            {
                form.ShowDialog();
            }

            return Result.Succeeded;
        }

        public static string GetPath()
        {
            return typeof(UploadFamilyCommand).Namespace + "." + nameof(UploadFamilyCommand);
        }
    }
}
