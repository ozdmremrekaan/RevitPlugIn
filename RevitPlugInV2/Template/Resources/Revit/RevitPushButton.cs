using Autodesk.Revit.UI;
using RevitPlugInV2.Template;



namespace RevitPlugInV2.Resources.Revit
{
    public static class RevitPushButton
    {
        // creates push button
        public static PushButton Create(RevitPushButtonDataModel data)
        {
            var btnDataName = Guid.NewGuid().ToString();
            var btnData = new PushButtonData(btnDataName, data.Label, CoreAssembly.GetAssemblyLocation(), data.CommandNamespacePath)
            {
                LargeImage = ResourceImage.GetIcon(data.IconImageName),
                ToolTipImage = ResourceImage.GetIcon(data.ToolTipImageName)
            };

            return data.Panel.AddItem(btnData) as PushButton;
            
        }
    }
}
