#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

#endregion

namespace Forta.Estructuras.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class PlantillaEstructural : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData,ref string message,ElementSet elements)
        {
            TaskDialog.Show("ESTRUCTURAS", "Command para plantilla de Estructuras");
            return Result.Succeeded;
        }
    }
}
