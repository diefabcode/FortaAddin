using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;

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
            var p = FindParamByName(t, names);
            if (p == null) return;
            if (p.IsReadOnly) return;
            if (p.StorageType != StorageType.Integer) return;

            p.Set(value ? 1 : 0);
        }

        static void SetDimensionTickMark(Document doc, DimensionType dimType)
        {
            try
            {
                // BuiltInParameters que existen en Revit 2023
                BuiltInParameter[] tickParams =
                {
            BuiltInParameter.DIM_LEADER_ARROWHEAD,
            BuiltInParameter.LEADER_ARROWHEAD
        };

                Parameter tickParam = null;

                // 1) Intentar por BuiltInParameter
                foreach (var bip in tickParams)
                {
                    try
                    {
                        var p = dimType.get_Parameter(bip);
                        if (p != null && !p.IsReadOnly)
                        {
                            tickParam = p;
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error probando {bip}: {ex.Message}");
                    }
                }

                // 2) Fallback por nombre (ES/EN) si no se encontró
                if (tickParam == null)
                {
                    foreach (Parameter p in dimType.Parameters)
                    {
                        var paramName = p.Definition?.Name ?? string.Empty;

                        bool esTick =
                            paramName.Equals("Marca", StringComparison.OrdinalIgnoreCase) ||
                            paramName.Equals("Tick Mark", StringComparison.OrdinalIgnoreCase) ||
                            paramName.IndexOf("Arrow", StringComparison.OrdinalIgnoreCase) >= 0;

                        if (esTick && !p.IsReadOnly && p.StorageType == StorageType.ElementId)
                        {
                            tickParam = p;
                            break;
                        }
                    }
                }

                // Si no hay parámetro editable, salir
                if (tickParam == null) return;

                // 3) Elegir la flecha priorizando 15°
                var allTypes = new FilteredElementCollector(doc)
                    .WhereElementIsElementType()
                    .Cast<ElementType>()
                    .ToList();

                string[] arrowNames =
                {
            "Flecha 15 grados rellenada",
            "Arrow Filled 15 Degree",
            "Flecha 20 grados rellenada",
            "Arrow Filled 20 Degree",
            "Flecha 30 grados rellenada",
            "Arrow Filled 30 Degree"
        };

                Element chosenArrow = null;

                // 3a) Búsqueda exacta por nombres preferidos
                foreach (var arrowName in arrowNames)
                {
                    chosenArrow = allTypes.FirstOrDefault(et =>
                        et.Name.Equals(arrowName, StringComparison.OrdinalIgnoreCase));
                    if (chosenArrow != null) break;
                }

                // 3b) Si no hay exacta, tomar cualquier “flecha” rellena
                if (chosenArrow == null)
                {
                    chosenArrow = allTypes.FirstOrDefault(et =>
                        (et.Name.IndexOf("flecha", StringComparison.OrdinalIgnoreCase) >= 0 ||
                         et.Name.IndexOf("arrow", StringComparison.OrdinalIgnoreCase) >= 0) &&
                        (et.Name.IndexOf("rellen", StringComparison.OrdinalIgnoreCase) >= 0 ||
                         et.Name.IndexOf("filled", StringComparison.OrdinalIgnoreCase) >= 0));
                }

                // 4) Aplicar si se encontró algo
                if (chosenArrow != null)
                {
                    tickParam.Set(chosenArrow.Id);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ ERROR en SetDimensionTickMark: {ex.Message}");
                Debug.WriteLine($"StackTrace: {ex.StackTrace}");
            }
        }

        public static ElementId CreateOrUpdate(Document doc, string typeName, DimStyleOptions opt)
        {
            try
            {
                // 1) buscar/duplicar
                var dimTypes = new FilteredElementCollector(doc)
                    .OfClass(typeof(DimensionType))
                    .Cast<DimensionType>()
                    .ToList();

                var dimType = dimTypes.FirstOrDefault(x => x.Name == typeName);
                if (dimType == null)
                {
                    var baseType = dimTypes.FirstOrDefault();
                    if (baseType == null)
                        throw new InvalidOperationException("No se encontró DimensionType base para duplicar.");

                    dimType = baseType.Duplicate(typeName) as DimensionType;
                }

                // 2) Unidades principales
                try
                {
                    var fo = new FormatOptions(opt.Units.Spec, opt.Units.Unit)
                    {
                        UseDefault = false,
                        Accuracy = opt.Units.Accuracy
                    };
                    dimType.SetUnitsFormatOptions(fo);
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
                SetYesNo(dimType, new[] { "Negrita", "Bold" }, opt.Text.Bold != 0);
                SetYesNo(dimType, new[] { "Cursiva", "Italic" }, opt.Text.Italic != 0);
                SetYesNo(dimType, new[] { "Subrayado", "Underline" }, opt.Text.Underline != 0);
                SetDMm(dimType, new[] { "Desfase de texto", "Text Offset From Dimension Line" }, opt.Text.OffsetFromDimLineMm);
                SetInt(dimType, new[] { "Convención de lectura", "Text Orientation" }, opt.Text.Orientation);

                // 4) Gráfica
                SetInt(dimType, new[] { "Grosor de línea", "Dimension Line Weight" }, opt.Graphics.DimLineWeight);
                SetInt(dimType, new[] { "Grosor de línea de marca", "Tick Mark Line Weight" }, opt.Graphics.TickLineWeight);

                // Tick mark
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

                doc.Regenerate();
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