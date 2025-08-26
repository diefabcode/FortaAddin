using System;
using System.Diagnostics;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Forta.Core.Plantillas.Generales.Cotas.DimensionStyles
{
    public static class DimensionStyleService
    {
        // -------- helpers por nombre (ES/EN) compatibles Revit 2023 --------
        private static Parameter FindParamByName(Element e, params string[] names)
        {
            foreach (Parameter p in e.Parameters)
            {
                var n = p.Definition?.Name ?? "";
                foreach (var key in names)
                {
                    if (!string.IsNullOrWhiteSpace(key) &&
                        n.IndexOf(key, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return p;
                    }
                }
            }
            return null;
        }

        private static void SetInt(Element e, string[] names, int val)
        {
            try
            {
                var p = FindParamByName(e, names);
                if (p != null && !p.IsReadOnly)
                    p.Set(val);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SetInt {string.Join("/", names)}: {ex.Message}");
            }
        }

        private static void SetStr(Element e, string[] names, string val)
        {
            try
            {
                var p = FindParamByName(e, names);
                if (p != null && !p.IsReadOnly)
                    p.Set(val);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SetStr {string.Join("/", names)}: {ex.Message}");
            }
        }

        private static void SetDInternal(Element e, string[] names, double ival)
        {
            try
            {
                var p = FindParamByName(e, names);
                if (p != null && !p.IsReadOnly)
                    p.Set(ival);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SetDInternal {string.Join("/", names)}: {ex.Message}");
            }
        }

        private static void SetDMm(Element e, string[] names, double mm)
        {
            SetDInternal(e, names, UnitUtils.ConvertToInternalUnits(mm, UnitTypeId.Millimeters));
        }

        private static void SetColor(Element e, string[] names, Color c)
        {
            try
            {
                var p = FindParamByName(e, names);
                if (p != null && !p.IsReadOnly)
                {
                    int rgb = (c.Red << 16) | (c.Green << 8) | c.Blue;
                    p.Set(rgb);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SetColor {string.Join("/", names)}: {ex.Message}");
            }
        }

        static void SetYesNo(ElementType t, string[] names, bool value)
        {
            // Busca por nombre localizado (ES/EN) y escribe 1/0
            foreach (var n in names)
            {
                var p = t.LookupParameter(n);
                if (p != null && !p.IsReadOnly && p.StorageType == StorageType.Integer)
                {
                    p.Set(value ? 1 : 0);
                    return;
                }
            }
        }


        // VERSIÓN CORREGIDA SIN BuiltInCategory.OST_DimensionArrowheads

        static void SetDimensionTickMark(Document doc, DimensionType dimType)
        {
            try
            {
                Debug.WriteLine($"=== Configurando Tick Mark para: {dimType.Name} ===");

                // Usar solo BuiltInParameters que SÍ existen en Revit 2023
                BuiltInParameter[] tickParams = {
            BuiltInParameter.DIM_LEADER_ARROWHEAD,
            BuiltInParameter.LEADER_ARROWHEAD
        };

                Parameter tickParam = null;
                foreach (var bip in tickParams)
                {
                    try
                    {
                        var p = dimType.get_Parameter(bip);
                        if (p != null && !p.IsReadOnly)
                        {
                            tickParam = p;
                            Debug.WriteLine($"✅ Parámetro encontrado: {bip} -> {p.Definition.Name}");
                            Debug.WriteLine($"Valor actual: {p.AsElementId()}");
                            break;
                        }
                        else if (p != null)
                        {
                            Debug.WriteLine($"⚠️ Parámetro encontrado pero ReadOnly: {bip} -> {p.Definition.Name}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error probando {bip}: {ex.Message}");
                    }
                }

                // Si no funciona con BuiltInParameter, usar búsqueda por nombre
                if (tickParam == null)
                {
                    Debug.WriteLine("Probando búsqueda por nombre de parámetro...");

                    foreach (Parameter p in dimType.Parameters)
                    {
                        var paramName = p.Definition?.Name ?? "";

                        if (paramName.Equals("Marca", StringComparison.OrdinalIgnoreCase) ||
                            paramName.Equals("Tick Mark", StringComparison.OrdinalIgnoreCase) ||
                            paramName.Contains("Arrow"))
                        {
                            Debug.WriteLine($"Parámetro encontrado por nombre: {paramName}");
                            Debug.WriteLine($"ReadOnly: {p.IsReadOnly}, StorageType: {p.StorageType}");

                            if (!p.IsReadOnly && p.StorageType == StorageType.ElementId)
                            {
                                tickParam = p;
                                break;
                            }
                        }
                    }
                }

                if (tickParam == null)
                {
                    Debug.WriteLine("❌ No se encontró parámetro de tick mark accesible");

                    // Debug: Listar TODOS los parámetros ElementId del DimensionType
                    Debug.WriteLine("--- TODOS LOS PARÁMETROS ElementId ---");
                    foreach (Parameter p in dimType.Parameters)
                    {
                        if (p.StorageType == StorageType.ElementId)
                        {
                            Debug.WriteLine($"  - {p.Definition?.Name} = {p.AsElementId()} (ReadOnly: {p.IsReadOnly})");
                        }
                    }
                    return;
                }

                // Buscar la flecha específicamente
                var allTypes = new FilteredElementCollector(doc)
                    .WhereElementIsElementType()
                    .Cast<ElementType>()
                    .ToList();

                // Nombres con PRIORIDAD a 15 grados
                string[] arrowNames = {
            "Flecha 15 grados rellenada",  // PRIMERA PRIORIDAD
            "Arrow Filled 15 Degree",      // SEGUNDA PRIORIDAD
            "Flecha 20 grados rellenada",
            "Arrow Filled 20 Degree",
            "Flecha 30 grados rellenada",  // Última opción
            "Arrow Filled 30 Degree"
        };

                Element chosenArrow = null;
                foreach (var arrowName in arrowNames)
                {
                    chosenArrow = allTypes.FirstOrDefault(et =>
                        et.Name.Equals(arrowName, StringComparison.OrdinalIgnoreCase));

                    if (chosenArrow != null)
                    {
                        Debug.WriteLine($"✅ Flecha encontrada: {chosenArrow.Name} (ID: {chosenArrow.Id})");
                        break;
                    }
                }

                // Si no encuentra las específicas, buscar cualquier flecha
                if (chosenArrow == null)
                {
                    Debug.WriteLine("Buscando cualquier flecha disponible...");

                    var arrows = allTypes.Where(et =>
                        (et.Name.ToLower().Contains("flecha") || et.Name.ToLower().Contains("arrow")) &&
                        (et.Name.ToLower().Contains("rellen") || et.Name.ToLower().Contains("filled")))
                        .ToList();

                    Debug.WriteLine($"Flechas rellenas encontradas: {arrows.Count}");
                    foreach (var arrow in arrows)
                    {
                        Debug.WriteLine($"  - {arrow.Name}");
                    }

                    chosenArrow = arrows.FirstOrDefault();
                }

                if (chosenArrow != null)
                {
                    Debug.WriteLine($"Aplicando flecha: {chosenArrow.Name}");
                    Debug.WriteLine($"Valor antes: {tickParam.AsElementId()}");

                    tickParam.Set(chosenArrow.Id);

                    Debug.WriteLine($"Valor después: {tickParam.AsElementId()}");
                    Debug.WriteLine("✅ Tick mark aplicado exitosamente");
                }
                else
                {
                    Debug.WriteLine("❌ No se encontró ninguna flecha para aplicar");

                    // Lista todas las opciones disponibles
                    var allArrows = allTypes.Where(et =>
                        et.Name.ToLower().Contains("flecha") ||
                        et.Name.ToLower().Contains("arrow") ||
                        et.Name.ToLower().Contains("diagonal"))
                        .OrderBy(et => et.Name);

                    Debug.WriteLine("--- TODAS LAS OPCIONES DISPONIBLES ---");
                    foreach (var arrow in allArrows)
                    {
                        Debug.WriteLine($"  - {arrow.Name}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ ERROR en SetDimensionTickMark: {ex.Message}");
                Debug.WriteLine($"StackTrace: {ex.StackTrace}");
            }
        }



        private static Element FindBestArrowhead(Document doc)
        {
            // Nombres preferidos en orden de prioridad - SOLO FLECHAS, NO DIAGONALES
            string[] preferredNames = new[]
            {
                // Español - Flechas rellenas
                "Flecha 15 grados rellena",
                "Flecha 15° rellena",
                "Flecha rellena 15 grados",
                "Flecha 15 grados rellenada",
                "Flecha 15 rellenada",
                "Flecha rellena",
                // Inglés - Flechas rellenas
                "Arrow Filled 15 Degree",
                "Arrow 15 Degree Filled",
                "Arrow Filled 15°",
                "15 Degree Arrow Filled",
                "Arrow Filled",
                "Filled Arrow",
                // Fallbacks - Solo flechas, NO diagonales
                "Arrow",
                "Flecha"
            };

            try
            {
                Debug.WriteLine("Buscando arrowheads en todos los ElementTypes...");

                // Buscar en todos los ElementTypes que puedan ser arrowheads
                var allTypes = new FilteredElementCollector(doc)
                    .WhereElementIsElementType()
                    .Cast<ElementType>()
                    .ToList();

                Debug.WriteLine($"Total ElementTypes en documento: {allTypes.Count}");

                // Filtrar por nombres que contengan palabras clave de arrowheads - SOLO FLECHAS
                var candidateArrowheads = allTypes.Where(et =>
                {
                    var name = et.Name.ToLower();
                    return (name.Contains("arrow") || name.Contains("flecha")) &&
                           (name.Contains("filled") || name.Contains("rellen") ||
                            name.Contains("15") || name.Contains("degree") || name.Contains("grado"));
                }).ToList();

                Debug.WriteLine($"Candidatos encontrados: {candidateArrowheads.Count}");
                foreach (var candidate in candidateArrowheads)
                {
                    Debug.WriteLine($"  - {candidate.Name} (Category: {candidate.Category?.Name ?? "null"})");
                }

                // Buscar por nombres preferidos
                foreach (var preferredName in preferredNames)
                {
                    var match = candidateArrowheads.FirstOrDefault(et =>
                        et.Name.IndexOf(preferredName, StringComparison.OrdinalIgnoreCase) >= 0);

                    if (match != null)
                    {
                        Debug.WriteLine($"Encontrado arrowhead preferido: {match.Name}");
                        return match;
                    }
                }

                // Fallback 1: Cualquier arrow filled
                var filledArrow = candidateArrowheads.FirstOrDefault(et =>
                    (et.Name.ToLower().Contains("filled") || et.Name.ToLower().Contains("rellen")) &&
                    (et.Name.ToLower().Contains("arrow") || et.Name.ToLower().Contains("flecha")) &&
                    !et.Name.ToLower().Contains("diagonal")); // EXCLUIR diagonales

                if (filledArrow != null)
                {
                    Debug.WriteLine($"Usando fallback filled arrow: {filledArrow.Name}");
                    return filledArrow;
                }

                // Fallback 2: Cualquier arrow (sin diagonal)
                var anyArrow = candidateArrowheads.FirstOrDefault(et =>
                    (et.Name.ToLower().Contains("arrow") || et.Name.ToLower().Contains("flecha")) &&
                    !et.Name.ToLower().Contains("diagonal")); // EXCLUIR diagonales

                if (anyArrow != null)
                {
                    Debug.WriteLine($"Usando fallback cualquier arrow: {anyArrow.Name}");
                    return anyArrow;
                }

                Debug.WriteLine("No se encontró ningún arrowhead adecuado (solo flechas)");
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error en FindBestArrowhead: {ex.Message}");
                return null;
            }
        }

        // Método de debug para listar todos los arrowheads disponibles

        // ----------------- API principal parametrizable -----------------
        public static ElementId CreateOrUpdate(Document doc, string typeName, DimStyleOptions opt)
        {
            Debug.WriteLine($"=== Creando/Actualizando DimensionType: {typeName} ===");

            try
            {
                // 1) buscar/duplicar
                var dimTypes = new FilteredElementCollector(doc).OfClass(typeof(DimensionType)).Cast<DimensionType>().ToList();
                Debug.WriteLine($"DimensionTypes encontrados: {dimTypes.Count}");

                var dimType = dimTypes.FirstOrDefault(x => x.Name == typeName);
                if (dimType == null)
                {
                    var baseType = dimTypes.FirstOrDefault();
                    if (baseType == null)
                        throw new InvalidOperationException("No se encontró DimensionType base para duplicar.");

                    Debug.WriteLine($"Duplicando desde: {baseType.Name}");
                    dimType = baseType.Duplicate(typeName) as DimensionType;
                }

                Debug.WriteLine($"DimensionType obtenido: {dimType.Name} (ID: {dimType.Id})");

                // 2) Unidades principales
                try
                {
                    var fo = new FormatOptions(opt.Units.Spec, opt.Units.Unit) { UseDefault = false, Accuracy = opt.Units.Accuracy };
                    dimType.SetUnitsFormatOptions(fo);
                    Debug.WriteLine("Unidades configuradas correctamente");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error configurando unidades principales: " + ex.Message);
                }

                // Alternativas ON/OFF (por nombre)
                SetInt(dimType, new[] { "Unidades alternativas", "Alternate Units" }, opt.Units.UseAlternate ? 1 : 0);

                // 3) Texto
                SetDMm(dimType, new[] { "Tamaño de texto", "Text Size" }, opt.Text.SizeMm);
                SetStr(dimType, new[] { "Tipo de letra", "Text Font" }, opt.Text.Font);
                SetInt(dimType, new[] { "Fondo de texto", "Text Background" }, opt.Text.Background);
                SetDInternal(dimType, new[] { "Factor de anchura", "Width Factor" }, opt.Text.WidthFactor);
                SetDMm(dimType, new[] { "Desfase de texto", "Text Offset From Dimension Line" }, opt.Text.OffsetFromDimLineMm);
                SetYesNo(dimType, new[] { "Negrita", "Bold" }, opt.Text.Bold != 0);
                SetYesNo(dimType, new[] { "Cursiva", "Italic" }, opt.Text.Italic != 0);
                SetYesNo(dimType, new[] { "Subrayado", "Underline" }, opt.Text.Underline != 0);
                SetInt(dimType, new[] { "Convención de lectura", "Text Orientation" }, opt.Text.Orientation);

                // 4) Gráficos
                SetInt(dimType, new[] { "Grosor de línea", "Dimension Line Weight" }, opt.Graphics.DimLineWeight);
                SetInt(dimType, new[] { "Grosor de línea de marca", "Tick Mark Line Weight" }, opt.Graphics.TickLineWeight);

                // Aplicar el tick mark - AQUÍ ES DONDE ESTAVA EL PROBLEMA
                Debug.WriteLine("Configurando tick mark...");
                SetDimensionTickMark(doc, dimType);

                SetDMm(dimType, new[] { "Extensión de línea de cota", "Dimension Line Extension" }, opt.Graphics.DimLineExtensionMm);
                SetDMm(dimType, new[] { "Extensión de línea de cota volteada", "Dimension Line Extension (Flipped)" }, opt.Graphics.DimLineExtensionFlippedMm);
                SetDMm(dimType, new[] { "Separación entre línea de referencia y elemento", "Witness Line Gap" }, opt.Graphics.WitnessGapMm);
                SetDMm(dimType, new[] { "Extensión de línea de referencia", "Witness Line Extension" }, opt.Graphics.WitnessExtensionMm);
                SetColor(dimType, new[] { "Color", "Color" }, opt.Graphics.Color);

                // 5) Igualdad
                SetStr(dimType, new[] { "Texto de igualdad", "Equality Text" }, opt.Equality.EqText);
                SetInt(dimType, new[] { "Fórmula de igualdad", "Equality Formula" }, opt.Equality.EqFormula);
                SetInt(dimType, new[] { "Visualización de referencia de igualdad", "Equality Display" }, opt.Equality.EqDisplay);

                Debug.WriteLine($"=== DimensionType {typeName} configurado correctamente ===");
                return dimType.Id;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR GENERAL en CreateOrUpdate: {ex.Message}");
                Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                throw;
            }
        }
    }
}