using System;
using System.Diagnostics;
using System.Linq;
using Autodesk.Revit.DB;

namespace Forta.Core.Plantillas.Generales.Textos.TextStyles
{
    public static class TextStylesService
    {
        /// Crea o actualiza un TextNoteType con todas las propiedades.
        /// sizeMm, leaderOffsetMm, tabSizeMm -> mm
        public static ElementId CreateOrUpdate(
            Document doc,
            string name,
            string font,
            double sizeMm,
            Color color,
            int lineWeight,
            bool bold,
            bool italic,
            bool underline,
            double widthFactor,
            double leaderOffsetMm,
            double tabSizeMm,
            string arrowType // "flecha" | "punto" | "diagonal"
        )
        {
            // 1) Buscar existente o duplicar base
            var collector = new FilteredElementCollector(doc).OfClass(typeof(TextNoteType));
            TextNoteType tnt = collector.Cast<TextNoteType>().FirstOrDefault(x => x.Name == name);

            if (tnt == null)
            {
                var baseType = collector.Cast<TextNoteType>().FirstOrDefault();
                if (baseType == null)
                    throw new InvalidOperationException("No se encontró un TextNoteType base para duplicar.");

                tnt = baseType.Duplicate(name) as TextNoteType;
            }

            // 2) Setear propiedades
            SetGraphics(tnt, color, lineWeight, leaderOffsetMm);
            SetText(tnt, font, sizeMm, tabSizeMm, bold, italic, underline, widthFactor);
            SetArrowheadByType(tnt, arrowType);

            return tnt.Id;
        }

        private static void SetGraphics(TextNoteType tnt, Color color, int lineWeight, double leaderOffsetMm)
        {
            // Color
            var colorParam = tnt.get_Parameter(BuiltInParameter.LINE_COLOR);
            if (colorParam != null && !colorParam.IsReadOnly)
            {
                int colorRGB = (color.Red << 16) | (color.Green << 8) | color.Blue;
                colorParam.Set(colorRGB);
            }

            // Grosor
            var penParam = tnt.get_Parameter(BuiltInParameter.LINE_PEN);
            if (penParam != null && !penParam.IsReadOnly)
                penParam.Set(lineWeight);

            // Fondo transparente
            var bgParam = tnt.get_Parameter(BuiltInParameter.TEXT_BACKGROUND);
            if (bgParam != null && !bgParam.IsReadOnly)
                bgParam.Set(0);

            // Ocultar borde
            var borderParam = tnt.get_Parameter(BuiltInParameter.TEXT_BOX_VISIBILITY);
            if (borderParam != null && !borderParam.IsReadOnly)
                borderParam.Set(0);

            // Desfase directriz (mm -> internos)
            var leaderParam = tnt.get_Parameter(BuiltInParameter.LEADER_OFFSET_SHEET);
            if (leaderParam != null && !leaderParam.IsReadOnly)
            {
                double val = UnitUtils.ConvertToInternalUnits(leaderOffsetMm, UnitTypeId.Millimeters);
                leaderParam.Set(val);
            }
        }

        private static void SetText(TextNoteType tnt, string font, double sizeMm, double tabSizeMm,
                                    bool bold, bool italic, bool underline, double widthFactor)
        {
            // Tamaño
            var sizeParam = tnt.get_Parameter(BuiltInParameter.TEXT_SIZE);
            if (sizeParam != null && !sizeParam.IsReadOnly)
            {
                double val = UnitUtils.ConvertToInternalUnits(sizeMm, UnitTypeId.Millimeters);
                sizeParam.Set(val);
            }

            // Tabulación
            var tabParam = tnt.get_Parameter(BuiltInParameter.TEXT_TAB_SIZE);
            if (tabParam != null && !tabParam.IsReadOnly)
            {
                double val = UnitUtils.ConvertToInternalUnits(tabSizeMm, UnitTypeId.Millimeters);
                tabParam.Set(val);
            }

            // Fuente
            var fontParam = tnt.get_Parameter(BuiltInParameter.TEXT_FONT);
            if (fontParam != null && !fontParam.IsReadOnly)
                fontParam.Set(font);

            // Bold / Italic / Underline
            var boldParam = tnt.get_Parameter(BuiltInParameter.TEXT_STYLE_BOLD);
            if (boldParam != null && !boldParam.IsReadOnly)
                boldParam.Set(bold ? 1 : 0);

            var italicParam = tnt.get_Parameter(BuiltInParameter.TEXT_STYLE_ITALIC);
            if (italicParam != null && !italicParam.IsReadOnly)
                italicParam.Set(italic ? 1 : 0);

            var underlineParam = tnt.get_Parameter(BuiltInParameter.TEXT_STYLE_UNDERLINE);
            if (underlineParam != null && !underlineParam.IsReadOnly)
                underlineParam.Set(underline ? 1 : 0);

            // Width factor
            var widthParam = tnt.get_Parameter(BuiltInParameter.TEXT_WIDTH_SCALE);
            if (widthParam != null && !widthParam.IsReadOnly)
                widthParam.Set(widthFactor);
        }

        // Selección de punta de flecha por tipo (similar a tu lógica original)
        private static void SetArrowheadByType(TextNoteType tnt, string tipo)
        {
            try
            {
                var doc = tnt.Document;
                var elementTypes = new FilteredElementCollector(doc)
                    .WhereElementIsElementType()
                    .Cast<ElementType>()
                    .Where(et => et.Category == null) // arrowheads son ElementTypes sin Category
                    .ToList();

                var arrowheads = elementTypes.Where(et =>
                {
                    string n = et.Name.ToLower();
                    return n.Contains("grado") || n.Contains("degree") ||
                           n.Contains("flecha") || n.Contains("arrow") ||
                           n.Contains("punto") || n.Contains("dot") ||
                           n.Contains("diagonal") || n.Contains("sin") || n.Contains("none");
                }).ToList();

                Element chosen = null;
                string k = (tipo ?? "").ToLower();

                if (k == "flecha")
                {
                    chosen = arrowheads.FirstOrDefault(a =>
                        a.Name.ToLower().Contains("flecha") &&
                        a.Name.Contains("15") &&
                        a.Name.ToLower().Contains("rellen"));
                    if (chosen == null)
                        chosen = arrowheads.FirstOrDefault(a =>
                            a.Name.ToLower().Contains("arrow") &&
                            a.Name.ToLower().Contains("filled") &&
                            a.Name.Contains("15"));
                }
                else if (k == "punto")
                {
                    var puntos = arrowheads.Where(a =>
                        (a.Name.ToLower().Contains("punto") && a.Name.ToLower().Contains("rellen")) ||
                        (a.Name.ToLower().Contains("dot") && a.Name.ToLower().Contains("filled"))
                    ).ToList();

                    string[] sizes = { "1,5", "1.5", "1/16", "3", "3 mm", "1/8" };
                    foreach (var s in sizes)
                    {
                        chosen = puntos.FirstOrDefault(a => a.Name.Contains(s));
                        if (chosen != null) break;
                    }
                    if (chosen == null) chosen = puntos.FirstOrDefault();
                }
                else if (k == "diagonal")
                {
                    var diags = arrowheads.Where(a => a.Name.ToLower().Contains("diagonal")).ToList();
                    string[] sizes = { "1/8", "1/4", "3", "3 mm", "5", "5 mm" };
                    foreach (var s in sizes)
                    {
                        chosen = diags.FirstOrDefault(a => a.Name.Contains(s));
                        if (chosen != null) break;
                    }
                    if (chosen == null) chosen = diags.FirstOrDefault();
                }

                if (chosen != null)
                {
                    var leaderArrowParam = tnt.get_Parameter(BuiltInParameter.LEADER_ARROWHEAD);
                    if (leaderArrowParam != null && !leaderArrowParam.IsReadOnly)
                        leaderArrowParam.Set(chosen.Id);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Arrowhead: {ex.Message}");
            }
        }
    }
}
