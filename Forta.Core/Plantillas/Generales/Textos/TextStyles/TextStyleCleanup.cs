using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Forta.Core.Plantillas.Generales.Textos.TextStyles
{
    public static class TextStyleCleanup
    {
        private static string Normalize(string s)
        {
            if (s == null) return string.Empty;
            s = s.Replace('\u200B', ' ')  // zero-width space
                 .Replace('\u200E', ' ')  // LRM
                 .Replace('\u00A0', ' '); // NBSP
            while (s.Contains("  ")) s = s.Replace("  ", " ");
            return s.Trim();
        }

        public static int DepurarManteniendoFI(Document doc, IEnumerable<string> nombresFiEsperados)
        {
            if (doc == null) throw new ArgumentNullException(nameof(doc));

            // Detectar si un nombre es específicamente de tipo FI (MUY ESPECÍFICO)
            bool EsFI(string name)
            {
                var n = Normalize(name).ToUpperInvariant();
                if (string.IsNullOrEmpty(n)) return false;

                Debug.WriteLine($"Evaluando si '{name}' es FI. Normalizado: '{n}'");

                // SOLO detectar patrones muy específicos de FI
                // 1) Arranque exacto con "FI " (nuestro patrón principal)
                if (n.StartsWith("FI "))
                {
                    Debug.WriteLine($"  ✓ Detectado como FI (patrón FI )");
                    return true;
                }

                // 2) Solo "FI" (nombre exacto)
                if (n == "FI")
                {
                    Debug.WriteLine($"  ✓ Detectado como FI (nombre exacto)");
                    return true;
                }

                Debug.WriteLine($"  ✗ NO detectado como FI");
                return false;
            }

            // Obtener todos los tipos de texto
            var allTypes = new FilteredElementCollector(doc)
                .OfClass(typeof(TextNoteType))
                .WhereElementIsElementType()
                .Cast<TextNoteType>()
                .ToList();

            Debug.WriteLine($"Total tipos de texto encontrados: {allTypes.Count}");

            // Verificación de seguridad: si no hay NINGÚN tipo con "FI", abortar
            var tiposFI = allTypes.Where(tt => EsFI(tt.Name)).ToList();
            Debug.WriteLine($"Tipos FI detectados: {tiposFI.Count}");
            
            if (tiposFI.Count == 0)
            {
                Debug.WriteLine("[DepurarTextos] Abortado: no se detectan tipos con 'FI' en el nombre.");
                return 0;
            }

            int eliminadas = 0;

            // FASE 1: Eliminar INSTANCIAS cuyo tipo NO contenga "FI"
            try
            {
                var instanciasNoFI = new FilteredElementCollector(doc)
                    .OfCategory(BuiltInCategory.OST_TextNotes)
                    .WhereElementIsNotElementType()
                    .OfClass(typeof(TextNote))
                    .Cast<TextNote>()
                    .Where(t =>
                    {
                        var tt = doc.GetElement(t.GetTypeId()) as TextNoteType;
                        bool esNoFI = tt == null || !EsFI(tt.Name);
                        if (esNoFI && tt != null)
                        {
                            Debug.WriteLine($"  Instancia a eliminar con tipo: '{tt.Name}'");
                        }
                        return esNoFI;
                    })
                    .Select(t => t.Id)
                    .ToList();

                Debug.WriteLine($"Instancias de texto NO-FI a eliminar: {instanciasNoFI.Count}");

                if (instanciasNoFI.Count > 0)
                {
                    using (var t = new Transaction(doc, "Depurar textos (instancias) sin 'FI'"))
                    {
                        t.Start();
                        var deletedIds = doc.Delete(instanciasNoFI);
                        eliminadas += deletedIds != null ? deletedIds.Count : 0;
                        t.Commit();
                        Debug.WriteLine($"Instancias de texto eliminadas: {(deletedIds != null ? deletedIds.Count : 0)}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error en FASE 1 (textos): {ex.Message}");
            }

            // FASE 2: Eliminar TIPOS cuyo nombre NO contenga "FI" 
            // (solo si ya no tienen dependencias)
            try
            {
                // Refrescar la lista después de eliminar instancias
                var allTypesUpdated = new FilteredElementCollector(doc)
                    .OfClass(typeof(TextNoteType))
                    .WhereElementIsElementType()
                    .Cast<TextNoteType>()
                    .ToList();

                var tiposNoFI = allTypesUpdated
                    .Where(tt => !EsFI(tt.Name))
                    .Select(tt => new { Id = tt.Id, Name = tt.Name })
                    .ToList();

                Debug.WriteLine($"Tipos de texto NO-FI a evaluar para eliminación: {tiposNoFI.Count}");

                if (tiposNoFI.Count > 0)
                {
                    using (var t = new Transaction(doc, "Depurar estilos de texto sin 'FI'"))
                    {
                        t.Start();
                        int tiposEliminados = 0;
                        foreach (var tipoInfo in tiposNoFI)
                        {
                            try
                            {
                                Debug.WriteLine($"Evaluando tipo de texto para eliminación: '{tipoInfo.Name}'");

                                // Intentar eliminar directamente usando el ID
                                var deleted = doc.Delete(tipoInfo.Id);
                                if (deleted != null && deleted.Count > 0)
                                {
                                    Debug.WriteLine($"  ✓ Eliminado tipo de texto: '{tipoInfo.Name}'");
                                    tiposEliminados++;
                                    eliminadas++;
                                }
                                else
                                {
                                    Debug.WriteLine($"  ⚠️ No se eliminó '{tipoInfo.Name}' - Delete retornó null o 0 elementos");
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"  ✗ No se pudo eliminar '{tipoInfo.Name}': {ex.Message}");
                                // Saltar tipos que Revit no permita borrar (como tipos del sistema)
                            }
                        }
                        Debug.WriteLine($"Se eliminaron {tiposEliminados} tipos de texto de {tiposNoFI.Count} intentados");
                        t.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error en FASE 2 (textos): {ex.Message}");
            }

            Debug.WriteLine($"[DepurarTextos] Total eliminados (instancias + tipos) sin 'FI': {eliminadas}");
            return eliminadas;
        }

        // Método adicional para debug: listar todos los tipos de texto
        public static void ListAllTextTypes(Document doc)
        {
            var allTypes = new FilteredElementCollector(doc)
                .OfClass(typeof(TextNoteType))
                .WhereElementIsElementType()
                .Cast<TextNoteType>()
                .ToList();

            Debug.WriteLine("=== TODOS LOS TIPOS DE TEXTO ===");
            foreach (var tt in allTypes)
            {
                Debug.WriteLine($"  '{tt.Name}' (ID: {tt.Id})");
            }
        }
    }
}
