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
        // Normaliza el nombre: quita espacios raros/ocultos y espacios múltiples
        private static string Normalize(string s)
        {
            if (s == null) return string.Empty;
            s = s.Replace('\u200B', ' ')  // zero-width space
                 .Replace('\u200E', ' ')  // LRM
                 .Replace('\u00A0', ' '); // NBSP
            while (s.Contains("  ")) s = s.Replace("  ", " ");
            return s.Trim();
        }

        /// <summary>
        /// Elimina instancias y tipos de cota cuyo NOMBRE de DimensionType
        /// NO esté en la lista blanca "nombresFi".
        /// </summary>
        public static int DepurarManteniendoFI(Document doc, IEnumerable<string> nombresFi)
        {
            if (doc == null) throw new ArgumentNullException(nameof(doc));
            var whitelist = new HashSet<string>(
                (nombresFi ?? Enumerable.Empty<string>()).Select(Normalize),
                StringComparer.OrdinalIgnoreCase
            );

            int eliminadasInstancias = 0;
            int eliminadosTipos = 0;

            // ---------- FASE 1: borrar INSTANCIAS cuyo tipo NO esté en whitelist
            var instanciasNoFI = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Dimensions)
                .WhereElementIsNotElementType()
                .OfClass(typeof(Dimension))
                .Cast<Dimension>()
                .Where(d =>
                {
                    var dt = doc.GetElement(d.GetTypeId()) as DimensionType;
                    var name = Normalize(dt?.Name);
                    return !whitelist.Contains(name);
                })
                .Select(d => d.Id)
                .ToList();

            if (instanciasNoFI.Count > 0)
            {
                using (var t = new Transaction(doc, "Depurar cotas (instancias) no FI"))
                {
                    t.Start();
                    var deleted = doc.Delete(instanciasNoFI);
                    eliminadasInstancias = deleted?.Count ?? 0;
                    t.Commit();
                }
            }

            // ---------- FASE 2: borrar TIPOS cuyo nombre NO esté en whitelist
            var tiposNoFI = new FilteredElementCollector(doc)
                .OfClass(typeof(DimensionType))
                .WhereElementIsElementType()
                .Cast<DimensionType>()
                .Where(dt => !whitelist.Contains(Normalize(dt.Name)))
                .ToList();

            using (var t = new Transaction(doc, "Depurar estilos de cota no FI"))
            {
                t.Start();
                foreach (var dt in tiposNoFI)
                {
                    try
                    {
                        // Si aún hay dependencias, saltar
                        var deps = dt.GetDependentElements(new ElementClassFilter(typeof(Dimension)));
                        if (deps != null && deps.Count > 0) continue;

                        var deleted = doc.Delete(dt.Id);
                        if (deleted != null && deleted.Count > 0) eliminadosTipos++;
                    }
                    catch { /* saltar tipos que Revit no deje borrar */ }
                }
                t.Commit();
            }

            Debug.WriteLine($"[DepurarCotas] Instancias: {eliminadasInstancias}, Tipos: {eliminadosTipos}");
            return eliminadasInstancias + eliminadosTipos;
        }
    }
}
