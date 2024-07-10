using Autodesk.Revit.UI;
using RevitPlugInV2.Commands;
using RevitPlugInV2.Resources.Revit;
using RevitPlugInV2.Utils;

namespace RevitPlugInV2.Interface
{
    public class SetupInterface
    {
        public static PushButton LoginLogoutButton { get; private set; }
        public static PushButton LogoutButton { get; private set; }
        public static PushButton LoadFamiliesButton { get; private set; }
        public static PushButton UploadFamilyButton { get; private set; }
        public static PushButton ListUserFamiliesButton { get; private set; }

        public void Initialize(UIControlledApplication app)
        {
            string tabName = "RevitPlugInV2";
            app.CreateRibbonTab(tabName);

            var loginPanel = app.CreateRibbonPanel(tabName, "Login Commands");
            CreateLoginButtons(loginPanel);

            var familyPanel = app.CreateRibbonPanel(tabName, "Family Panel");
            CreateFamilyButtons(familyPanel);

            CheckButtonVisibility();
        }

        private void CreateLoginButtons(RibbonPanel loginPanel)
        {
            LoginLogoutButton = CreateButton(new RevitPushButtonDataModel
            {
                Label = "Login",
                Panel = loginPanel,
                Tooltip = "This is for authentication",
                CommandNamespacePath = RevitCommand.GetPath(),
                IconImageName = "Icon_Add.png",
                ToolTipImageName = "Icon_Edit.png"
            });

            LogoutButton = CreateButton(new RevitPushButtonDataModel
            {
                Label = "Logout",
                Panel = loginPanel,
                Tooltip = "Click to logout",
                CommandNamespacePath = LogOutCommand.GetPath(),
                IconImageName = "Icon_Delete.png",
                ToolTipImageName = "Icon_Delete.png"
            });
        }

        private void CreateFamilyButtons(RibbonPanel familyPanel)
        {
            LoadFamiliesButton = CreateButton(new RevitPushButtonDataModel
            {
                Label = "Load Families",
                Panel = familyPanel,
                Tooltip = "Load families into project",
                CommandNamespacePath = LoadFamiliesCommand.GetPath(),
                IconImageName = "Button_List.png",
                ToolTipImageName = "Button_List.png"
            });

            UploadFamilyButton = CreateButton(new RevitPushButtonDataModel
            {
                Label = "Upload Family",
                Panel = familyPanel,
                Tooltip = "Upload selected family to cloud",
                CommandNamespacePath = UploadFamilyCommand.GetPath(),
                IconImageName = "Icon_Add.png",
                ToolTipImageName = "Icon_Add.png"
            });

            ListUserFamiliesButton = CreateButton(new RevitPushButtonDataModel
            {
                Label = "List User Families",
                Panel = familyPanel,
                Tooltip = "List families for user",
                CommandNamespacePath = ListUserFamiliesCommand.GetPath(),
                IconImageName = "Button_List.png",
                ToolTipImageName = "Button_List.png"
            });
        }

        private PushButton CreateButton(RevitPushButtonDataModel dataModel)
        {
            return RevitPushButton.Create(dataModel);
        }

        public static void CheckButtonVisibility()
        {
            bool isUserLoggedIn = UserUtils.IsUserLoggedIn();
            LoginLogoutButton.Visible = !isUserLoggedIn;
            LogoutButton.Visible = isUserLoggedIn;

            if (isUserLoggedIn)
            {
                bool isAdmin = UserUtils.LoggedInUser.Role == UserRole.ADMIN;
                LoadFamiliesButton.Visible = isAdmin;
                UploadFamilyButton.Visible = isAdmin;
                ListUserFamiliesButton.Visible = !isAdmin;
            }
            else
            {
                LoadFamiliesButton.Visible = false;
                UploadFamilyButton.Visible = false;
                ListUserFamiliesButton.Visible = false;
            }
        }
    }
}
