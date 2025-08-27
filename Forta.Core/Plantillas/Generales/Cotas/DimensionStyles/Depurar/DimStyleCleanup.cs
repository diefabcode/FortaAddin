using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Forta.Core.Plantillas.Generales.Cotas.DimensionStyles
{
    public static class DimStyleCleanup
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
                // 1) Arranque exacto con "FI - " (nuestro patrón principal)
                if (n.StartsWith("FI - "))
                {
                    Debug.WriteLine($"  ✓ Detectado como FI (patrón FI - )");
                    return true;
                }

                // 2) Solo "FI" (nombre exacto)
                if (n == "FI")
                {
                    Debug.WriteLine($"  ✓ Detectado como FI (nombre exacto)");
                    return true;
                }

                // ELIMINADAS las reglas amplias que causaban falsos positivos:
                // - " FI " (podría coincidir con "Elevación FI algo")  
                // - " FI" al final (podría coincidir con "algo FI")

                Debug.WriteLine($"  ✗ NO detectado como FI");
                return false;
            }

            // Obtener solo tipos de cota ALINEADA y LINEAL (no tocar elevación, etc.)
            // IMPORTANTE: Excluir familias genéricas
            var allTypes = new FilteredElementCollector(doc)
                .OfClass(typeof(DimensionType))
                .WhereElementIsElementType()
                .Cast<DimensionType>()
                .Where(dt => {
                    var styleName = dt.StyleType.ToString();
                    var typeName = dt.Name;
                    
                    // Solo permitir Linear y Aligned (las cotas normales)
                    bool isCorrectStyle = styleName == "Linear" || styleName == "Aligned";
                    
                    // EXCLUIR familias genéricas que no son tipos específicos
                    bool isNotGenericFamily = !typeName.Equals("Estilo de cota lineal", StringComparison.OrdinalIgnoreCase);
                    
                    return isCorrectStyle && isNotGenericFamily;
                })
                .ToList();

            Debug.WriteLine($"Total tipos de cota encontrados: {allTypes.Count}");

            // Verificación de seguridad: si no hay NINGÚN tipo con "FI", abortar
            var tiposFI = allTypes.Where(dt => EsFI(dt.Name)).ToList();
            Debug.WriteLine($"Tipos FI detectados: {tiposFI.Count}");
            
            if (tiposFI.Count == 0)
            {
                Debug.WriteLine("[DepurarCotas] Abortado: no se detectan tipos con 'FI' en el nombre.");
                return 0;
            }

            int eliminadas = 0;

            // FASE 1: Eliminar INSTANCIAS cuyo tipo NO contenga "FI"
            try
            {
                var instanciasNoFI = new FilteredElementCollector(doc)
                    .OfCategory(BuiltInCategory.OST_Dimensions)
                    .WhereElementIsNotElementType()
                    .OfClass(typeof(Dimension))
                    .Cast<Dimension>()
                    .Where(d =>
                    {
                        var dt = doc.GetElement(d.GetTypeId()) as DimensionType;
                        bool esNoFI = dt == null || !EsFI(dt.Name);
                        if (esNoFI && dt != null)
                        {
                            Debug.WriteLine($"  Instancia a eliminar con tipo: '{dt.Name}'");
                        }
                        return esNoFI;
                    })
                    .Select(d => d.Id)
                    .ToList();

                Debug.WriteLine($"Instancias NO-FI a eliminar: {instanciasNoFI.Count}");

                if (instanciasNoFI.Count > 0)
                {
                    using (var t = new Transaction(doc, "Depurar cotas (instancias) sin 'FI'"))
                    {
                        t.Start();
                        var deletedIds = doc.Delete(instanciasNoFI);
                        eliminadas += deletedIds != null ? deletedIds.Count : 0;
                        t.Commit();
                        Debug.WriteLine($"Instancias eliminadas: {(deletedIds != null ? deletedIds.Count : 0)}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error en FASE 1: {ex.Message}");
            }

            // FASE 2: Eliminar TIPOS cuyo nombre NO contenga "FI" 
            // (solo si ya no tienen dependencias)
            try
            {
                // Refrescar la lista después de eliminar instancias (solo Linear y Aligned)
                // IMPORTANTE: Excluir familias genéricas como "Estilo de cota lineal"
                var allTypesUpdated = new FilteredElementCollector(doc)
                    .OfClass(typeof(DimensionType))
                    .WhereElementIsElementType()
                    .Cast<DimensionType>()
                    .Where(dt => {
                        var styleName = dt.StyleType.ToString();
                        var typeName = dt.Name;
                        
                        // Solo Linear y Aligned
                        bool isCorrectStyle = styleName == "Linear" || styleName == "Aligned";
                        
                        // EXCLUIR familias genéricas que no son tipos específicos
                        bool isNotGenericFamily = !typeName.Equals("Estilo de cota lineal", StringComparison.OrdinalIgnoreCase);
                        
                        return isCorrectStyle && isNotGenericFamily;
                    })
                    .ToList();

                var tiposNoFI = allTypesUpdated
                    .Where(dt => !EsFI(dt.Name))
                    .Select(dt => new { Id = dt.Id, Name = dt.Name })
                    .ToList();

                Debug.WriteLine($"Tipos NO-FI a evaluar para eliminación: {tiposNoFI.Count}");

                if (tiposNoFI.Count > 0)
                {
                    using (var t = new Transaction(doc, "Depurar estilos de cota sin 'FI'"))
                    {
                        t.Start();
                        int tiposEliminados = 0;
                        foreach (var tipoInfo in tiposNoFI)
                        {
                            try
                            {
                                Debug.WriteLine($"Evaluando tipo para eliminación: '{tipoInfo.Name}'");

                                // Intentar eliminar directamente usando el ID
                                var deleted = doc.Delete(tipoInfo.Id);
                                if (deleted != null && deleted.Count > 0)
                                {
                                    Debug.WriteLine($"  ✓ Eliminado tipo: '{tipoInfo.Name}'");
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
                        Debug.WriteLine($"Se eliminaron {tiposEliminados} tipos de {tiposNoFI.Count} intentados");
                        t.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error en FASE 2: {ex.Message}");
            }

            Debug.WriteLine($"[DepurarCotas] Total eliminados (instancias + tipos) sin 'FI': {eliminadas}");
            return eliminadas;
        }

        // Método adicional para debug: listar todos los tipos de cota
        public static void ListAllDimensionTypes(Document doc)
        {
            var allTypes = new FilteredElementCollector(doc)
                .OfClass(typeof(DimensionType))
                .WhereElementIsElementType()
                .Cast<DimensionType>()
                .ToList();

            Debug.WriteLine("=== TODOS LOS TIPOS DE COTA ===");
            foreach (var dt in allTypes)
            {
                Debug.WriteLine($"  '{dt.Name}' (ID: {dt.Id})");
            }
        }
    }
}