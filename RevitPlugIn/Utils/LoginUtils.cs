using Autodesk.Revit.UI;
using RevitPlugin;
using System.Windows.Forms;
using RevitPlugIn.Forms;
using RevitPlugIn.Utils;

public class LoginUtils
{
    public static bool UsagePermissionControl(string softwareName, out string errorMessage)
    {
        errorMessage = "";
        if (UserUtils.LoggedInUser == null)
        {
            var result = LoginUtils.RequestLogin();
            errorMessage = "Please login to use the tools.";
            if (!result) return false;
        }

        return true;
    }

    public static bool RequestLogin()
    {

        LoginForm form = new LoginForm();
        form.ShowDialog();
        return form.DialogResult == DialogResult.OK;
    }

}
