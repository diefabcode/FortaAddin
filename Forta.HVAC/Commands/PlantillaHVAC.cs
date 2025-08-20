using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Forta.HVAC.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class PlantillaHVAC : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData,ref string message,ElementSet elements)
        {
            TaskDialog.Show("HVAC", "Stub de Plantilla HVAC listo.");
            return Result.Succeeded;
        }
    }
}
