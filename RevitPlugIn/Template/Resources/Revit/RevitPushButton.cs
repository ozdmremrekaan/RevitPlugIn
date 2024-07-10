using Autodesk.Revit.UI;
using RevitPlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RevitPlugIn.Template;

namespace RevitPlugIn.Resources.Revit
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
