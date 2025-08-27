using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autodesk.Revit.DB;

namespace Forta.Core.Plantillas.Generales.Cotas.DimensionStyles
{
    public static class DimStyleCleanup
    {
        // === Agrega este helper si aún no existe en la clase ===
        private static string Normalize(string s)
        {
            if (s == null) return string.Empty;
            s = s.Replace('\u200B', ' ')  // zero-width space
                 .Replace('\u200E', ' ')  // LRM
                 .Replace('\u00A0', ' '); // NBSP
            while (s.Contains("  ")) s = s.Replace("  ", " ");
            return s.Trim();
        }

        // === Pega este método EXACTO ===
        public static int DepurarManteniendoFI(Document doc, IEnumerable<string> _nombresFiNoUsados)
        {
            if (doc == null) throw new ArgumentNullException(nameof(doc));

            // --- Detectar si un nombre "parece FI" (contiene la marca FI como token o al inicio)
            bool EsFI(string name)
            {
                var n = Normalize(name).ToUpperInvariant();
                if (string.IsNullOrEmpty(n)) return false;

                // 1) Arranque con "FI" (cubre "FI2mm..." y "FI - 2mm ...")
                if (n.StartsWith("FI")) return true;

                // 2) " FI " como palabra o con separadores comunes (evita falsos positivos como "PERFIL")
                return n.Contains(" FI ")
                    || n.Contains(" FI-")
                    || n.Contains("-FI ")
                    || n.Contains(" FI(")
                    || n.Contains("(FI")
                    || n.EndsWith(" FI");
            }

            // --- Todos los tipos de cota
            var allTypes = new FilteredElementCollector(doc)
                .OfClass(typeof(DimensionType))
                .WhereElementIsElementType()
                .Cast<DimensionType>()
                .ToList();

            // Si no hay NI UN solo tipo con "FI", no borramos nada (freno de seguridad)
            if (!allTypes.Any(dt => EsFI(dt.Name)))
            {
                Debug.WriteLine("[DepurarCotas] Abortado: no se detectan tipos con 'FI' en el nombre.");
                return 0;
            }

            int eliminadas = 0;

            // --- FASE 1: borrar INSTANCIAS cuyo tipo NO contenga "FI"
            var instanciasNoFI = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Dimensions)
                .WhereElementIsNotElementType()
                .OfClass(typeof(Dimension))
                .Cast<Dimension>()
                .Where(d =>
                {
                    var dt = doc.GetElement(d.GetTypeId()) as DimensionType;
                    return dt == null || !EsFI(dt.Name);
                })
                .Select(d => d.Id)
                .ToList();

            if (instanciasNoFI.Count > 0)
            {
                using (var t = new Transaction(doc, "Depurar cotas (instancias) sin 'FI'"))
                {
                    t.Start();
                    eliminadas += doc.Delete(instanciasNoFI)?.Count ?? 0;
                    t.Commit();
                }
            }

            // --- FASE 2: borrar TIPOS cuyo nombre NO contenga "FI" (si ya no tienen dependencias)
            var tiposNoFI = allTypes
                .Where(dt => !EsFI(dt.Name))
                .ToList();

            if (tiposNoFI.Count > 0)
            {
                using (var t = new Transaction(doc, "Depurar estilos de cota sin 'FI'"))
                {
                    t.Start();
                    foreach (var dt in tiposNoFI)
                    {
                        try
                        {
                            // Si aún hay instancias colgando de este tipo, saltar
                            var deps = dt.GetDependentElements(new ElementClassFilter(typeof(Dimension)));
                            if (deps != null && deps.Count > 0) continue;

                            var deleted = doc.Delete(dt.Id);
                            if (deleted != null && deleted.Count > 0) eliminadas++;
                        }
                        catch
                        {
                            // Saltar tipos que Revit no permita borrar
                        }
                    }
                    t.Commit();
                }
            }

            Debug.WriteLine($"[DepurarCotas] Eliminados (instancias + tipos) sin 'FI': {eliminadas}");
            return eliminadas;
        }

    }
}