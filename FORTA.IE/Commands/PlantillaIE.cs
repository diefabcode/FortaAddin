using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Forta.IE.Commands
{
    public class PlantillaIE : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            TaskDialog.Show("IE","Command para plantilla eléctrica");
            return Result.Succeeded;
        }
    }
}
