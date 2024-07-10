using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitPlugIn.Interface
{
    class App : IExternalApplication
    {
        public static UIControlledApplication UiApp { get; private set; }

        public Result OnStartup(UIControlledApplication a)
        {
            UiApp = a;

            var ui = new SetupInterface();
            ui.Initialize(a);

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }
    }

}
