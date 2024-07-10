using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitPlugInV2.Resources.Revit
{
    public class RevitPushButtonDataModel
    {
        public string Label { get; set; }
        public RibbonPanel Panel { get; set; }
        public string CommandNamespacePath { get; set; }
        public string Tooltip { get; set; }
        public string IconImageName { get; set; }
        public string ToolTipImageName { get; set; }

        public RevitPushButtonDataModel()
        {
            
        }
    }
}
