using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace Forta.Core.Plantillas.Generales.Lineas.EliminarAnteriores
{
    public static class LinePatternsCleanup
    {
        public static void DeleteCustom(Document doc)
        {
            var col = new FilteredElementCollector(doc).OfClass(typeof(LinePatternElement));
            var ids = new List<ElementId>();

            foreach (LinePatternElement p in col)
                if (p.Name != "Solid" && p.Name != "<Invisible lines>")
                    ids.Add(p.Id);

            if (ids.Count > 0) doc.Delete(ids);
        }
    }
}

