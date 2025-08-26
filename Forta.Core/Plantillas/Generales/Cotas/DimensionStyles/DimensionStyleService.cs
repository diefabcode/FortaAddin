using System;
using System.Diagnostics;
using System.Linq;
using Autodesk.Revit.DB;

namespace Forta.Core.Plantillas.Generales.Cotas.DimensionStyles
{
    public static class DimensionStyleService
    {
        // -------- helpers por nombre (ES/EN) compatibles Revit 2023 --------
        private static Parameter FindParamByName(Element e, params string[] names)
        {
            // Recorre TODOS los parámetros del elemento (tipo y compartidos)
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
            try { var p = FindParamByName(e, names); if (p != null && !p.IsReadOnly) p.Set(val); }
            catch (Exception ex) { Debug.WriteLine($"{string.Join("/", names)}: {ex.Message}"); }
        }
        private static void SetStr(Element e, string[] names, string val)
        {
            try { var p = FindParamByName(e, names); if (p != null && !p.IsReadOnly) p.Set(val); }
            catch (Exception ex) { Debug.WriteLine($"{string.Join("/", names)}: {ex.Message}"); }
        }
        private static void SetDInternal(Element e, string[] names, double ival)
        {
            try { var p = FindParamByName(e, names); if (p != null && !p.IsReadOnly) p.Set(ival); }
            catch (Exception ex) { Debug.WriteLine($"{string.Join("/", names)}: {ex.Message}"); }
        }
        private static void SetDMm(Element e, string[] names, double mm)
        {
            SetDInternal(e, names, UnitUtils.ConvertToInternalUnits(mm, UnitTypeId.Millimeters));
        }
        private static void SetColor(Element e, string[] names, Color c)
        {
            try { var p = FindParamByName(e, names); if (p != null && !p.IsReadOnly) { int rgb = (c.Red << 16) | (c.Green << 8) | c.Blue; p.Set(rgb); } }
            catch (Exception ex) { Debug.WriteLine($"{string.Join("/", names)}: {ex.Message}"); }
        }

        // --- Flecha prioritaria ES/EN para la "Marca" de cota ---
        static readonly string[] PreferredArrowNames = new[]
        {
    "Arrow Filled 15 Degree",
    "Arrow 15 Degree Filled",
    "Flecha 15 grados rellena",
    "Flecha 15° rellena",
    "Flecha rellena 15 grados",
    "Flecha 15 grados rellenada"
};

        static void SetDimensionTickMark(Document doc, DimensionType dimType)
        {
            var pTick = FindParamByName(dimType, "Marca", "Tick Mark");
            if (pTick == null || pTick.IsReadOnly) return;

            // buscamos una flecha que coincida con los nombres preferidos
            var chosen = FindArrowhead(doc, PreferredArrowNames);
            if (chosen != null) pTick.Set(chosen.Id);
        }

        private static Element FindArrowhead(Document doc, string[] preferred)
        {
            // Recolecta TODOS los tipos y filtra por categorías que suenen a flechas (ES/EN),
            // sin depender de BuiltInCategory ni ArrowheadType (robusto a versiones/idiomas).
            var types = new FilteredElementCollector(doc)
                .WhereElementIsElementType()
                .Cast<ElementType>()
                .Where(et =>
                {
                    var cn = et.Category?.Name ?? "";
                    return cn.IndexOf("arrow", StringComparison.OrdinalIgnoreCase) >= 0 ||
                           cn.IndexOf("arrowhead", StringComparison.OrdinalIgnoreCase) >= 0 ||
                           cn.IndexOf("arrow head", StringComparison.OrdinalIgnoreCase) >= 0 ||
                           cn.IndexOf("flecha", StringComparison.OrdinalIgnoreCase) >= 0;
                })
                .ToList();

            foreach (var name in preferred)
            {
                var t = types.FirstOrDefault(x =>
                    x.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0);
                if (t != null) return t;
            }
            return null;
        }

        // ----------------- API principal parametrizable -----------------
        public static ElementId CreateOrUpdate(Document doc, string typeName, DimStyleOptions opt)
        {
            // 1) buscar/duplicar
            var dimTypes = new FilteredElementCollector(doc).OfClass(typeof(DimensionType)).Cast<DimensionType>().ToList();
            var dimType = dimTypes.FirstOrDefault(x => x.Name == typeName)
                       ?? (dimTypes.FirstOrDefault()?.Duplicate(typeName) as DimensionType)
                       ?? throw new InvalidOperationException("No se encontró DimensionType base para duplicar.");

            // 2) Unidades principales
            try
            {
                var fo = new FormatOptions(opt.Units.Spec, opt.Units.Unit) { UseDefault = false, Accuracy = opt.Units.Accuracy };
                dimType.SetUnitsFormatOptions(fo);
            }
            catch (Exception ex) { Debug.WriteLine("Units (principal): " + ex.Message); }
            // Alternativas ON/OFF (por nombre)
            SetInt(dimType, new[] { "Unidades alternativas", "Alternate Units" }, opt.Units.UseAlternate ? 1 : 0);
            if (opt.Units.UseAlternate)
            {
                try
                {
                    var foAlt = new FormatOptions(opt.Units.Spec, opt.Units.AltUnit) { UseDefault = false, Accuracy = opt.Units.AltAccuracy };
                    // algunos builds exponen un parámetro string/json; si no, se ignora
                    var p = FindParamByName(dimType, "Formato de unidades alternativas", "Alternate Units Format");
                    if (p != null && !p.IsReadOnly) ; // no hay API directa estándar en 2023 → omitimos con seguridad
                }
                catch (Exception ex) { Debug.WriteLine("Units (alt): " + ex.Message); }
            }

            // 3) Texto
            SetDMm(dimType, new[] { "Tamaño de texto", "Text Size" }, opt.Text.SizeMm);
            SetStr(dimType, new[] { "Tipo de letra", "Text Font" }, opt.Text.Font);
            SetInt(dimType, new[] { "Fondo de texto", "Text Background" }, opt.Text.Background);
            SetDInternal(dimType, new[] { "Factor de anchura", "Width Factor" }, opt.Text.WidthFactor);
            SetDMm(dimType, new[] { "Desfase de texto", "Text Offset From Dimension Line" }, opt.Text.OffsetFromDimLineMm);
            SetInt(dimType, new[] { "Negrita", "Bold" }, opt.Text.Bold);
            SetInt(dimType, new[] { "Cursiva", "Italic" }, opt.Text.Italic);
            SetInt(dimType, new[] { "Subrayado", "Underline" }, opt.Text.Underline);
            SetInt(dimType, new[] { "Convención de lectura", "Text Orientation" }, opt.Text.Orientation);

            // 4) Gráficos
            SetInt(dimType, new[] { "Grosor de línea", "Dimension Line Weight" }, opt.Graphics.DimLineWeight);
            SetInt(dimType, new[] { "Grosor de línea de marca", "Tick Mark Line Weight" }, opt.Graphics.TickLineWeight);
            SetDimensionTickMark(doc, dimType);
            SetDMm(dimType, new[] { "Extensión de línea de cota", "Dimension Line Extension" }, opt.Graphics.DimLineExtensionMm);
            SetDMm(dimType, new[] { "Extensión de línea de cota volteada", "Dimension Line Extension (Flipped)" }, opt.Graphics.DimLineExtensionFlippedMm);
            SetDMm(dimType, new[] { "Separación entre línea de referencia y elemento", "Witness Line Gap" }, opt.Graphics.WitnessGapMm);
            SetDMm(dimType, new[] { "Extensión de línea de referencia", "Witness Line Extension" }, opt.Graphics.WitnessExtensionMm);
            SetColor(dimType, new[] { "Color", "Color" }, opt.Graphics.Color);

            // Patrón: sólido (si existe el parámetro en ese build)
            try
            {
                var p = FindParamByName(dimType, "Patrón de eje", "Centerline Pattern");
                if (p != null && !p.IsReadOnly)
                {
                    var lp = new FilteredElementCollector(doc).OfClass(typeof(LinePatternElement)).Cast<LinePatternElement>();
                    var solid = lp.FirstOrDefault(x => x.Name.Equals("Solid", StringComparison.OrdinalIgnoreCase) || x.Name.Equals("Sólido", StringComparison.OrdinalIgnoreCase));
                    if (solid != null) p.Set(solid.Id);
                }
            }
            catch (Exception ex) { Debug.WriteLine("Pattern: " + ex.Message); }
        

            // 5) Igualdad
            SetStr(dimType, new[] { "Texto de igualdad", "Equality Text" }, opt.Equality.EqText);
            SetInt(dimType, new[] { "Fórmula de igualdad", "Equality Formula" }, opt.Equality.EqFormula);
            SetInt(dimType, new[] { "Visualización de referencia de igualdad", "Equality Display" }, opt.Equality.EqDisplay);

            return dimType.Id;
        }
    }
}

