using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Forta.IESP.Commands
{
    public class PlantillaIESP : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            TaskDialog.Show("IESP", "Command para plantilla de IESP");
            return Result.Succeeded;
        }
    }
}
