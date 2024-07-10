using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitPlugInV2.Forms;
using RevitPlugInV2.Interface;
using RevitPlugInV2.Utils;

namespace RevitPlugInV2
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class RevitCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            while (UserUtils.LoggedInUser == null)
            {
                using (LoginForm loginForm = new LoginForm())
                {
                    if (loginForm.ShowDialog() != DialogResult.OK)
                    {
                        Autodesk.Revit.UI.TaskDialog.Show("Login Required", "You must log in to use this feature.");
                        return Result.Cancelled;
                    }
                }
            }

            // Kullanıcı başarıyla giriş yaptıysa, işlemleri gerçekleştir
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            var userInfo = UserUtils.LoggedInUser;
            if (userInfo == null)
            {
                Autodesk.Revit.UI.TaskDialog.Show("Error", "User information not found. Please log in again.");
                return Result.Failed;
            }

            // Giriş yapıldığında butonların görünürlüğünü kontrol et ve güncelle

            SetupInterface.CheckButtonVisibility();
            UserRole userRole = userInfo.Role;

            if (userRole == UserRole.ADMIN)
            {
                
            }
            else if (userRole == UserRole.USER)
            {
                
            }
            else
            {
                Autodesk.Revit.UI.TaskDialog.Show("Access Denied", "You do not have the required permissions to use this feature.");
                return Result.Cancelled;
            }

            // İşlem başarıyla tamamlandı
            using (Transaction transaction = new Transaction(doc, "MyTransactionName"))
            {
                try
                {
                    transaction.Start();
                    transaction.Commit();
                    return Result.Succeeded;
                }
                catch (Exception ex)
                {
                    Autodesk.Revit.UI.TaskDialog.Show("Error", "An error occurred: " + ex.Message);
                    transaction.RollBack();
                    return Result.Failed;
                }
            }
        }

        public static string GetPath()
        {
            return typeof(RevitCommand).Namespace + "." + nameof(RevitCommand);
        }
    }
}
