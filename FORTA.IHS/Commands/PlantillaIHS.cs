using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Forta.IHS.Commands
{
    [Transaction(TransactionMode.Manual)]
    public  class PlantillaIHS : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            TaskDialog.Show("IHS", "Command para plantilla de IHS prueba");
            return Result.Succeeded;    
        }
    }
}
