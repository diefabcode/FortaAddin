using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace Forta.Core.Plantillas.Generales.Cotas.DimensionStyles
{
    public static class DimStyleCleanup
    {
        /// <summary>
        /// Elimina TODAS las cotas (instancias) cuyo DimensionType.Name
        /// NO empieza con "FI". Devuelve cuántas cotas se solicitaron eliminar.
        /// </summary>
        public static int DepurarCotasNoFI(Document doc)
        {
            if (doc == null) throw new ArgumentNullException(nameof(doc));

            var idsAEliminar = new List<ElementId>();

            var dims = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Dimensions)
                .WhereElementIsNotElementType()
                .OfClass(typeof(Dimension))
                .Cast<Dimension>();

            foreach (var d in dims)
            {
                var typeId = d.GetTypeId();
                if (typeId == ElementId.InvalidElementId) continue;

                var dt = doc.GetElement(typeId) as DimensionType;
                if (dt == null) continue;

                var name = dt.Name ?? string.Empty;
                if (!name.StartsWith("FI", StringComparison.OrdinalIgnoreCase))
                    idsAEliminar.Add(d.Id);
            }

            if (idsAEliminar.Count == 0)
            {
                Debug.WriteLine("[DepurarCotas] No hay cotas para eliminar.");
                return 0;
            }

            using (var t = new Transaction(doc, "Depurar cotas no FI"))
            {
                t.Start();
                var deleted = doc.Delete(idsAEliminar); // también borra dependientes si aplica
                t.Commit();

                Debug.WriteLine($"[DepurarCotas] Eliminadas {idsAEliminar.Count} cotas no FI. " +
                                $"Elementos afectados totales: {deleted?.Count ?? 0}.");
            }

            return idsAEliminar.Count;
        }
    }
}
