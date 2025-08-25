using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace Forta.Core.Plantillas.Generales.Lineas.ObjectStyles
{
    public static class ObjectStylesService
    {
        public static void SetModelWeights(Document doc, int proj, int cut)
        {
            var cats = doc.Settings.Categories;
            foreach (Category c in cats)
            {
                if (c.CategoryType != CategoryType.Model || !c.CanAddSubcategory)
                    continue;

                try
                {
                    // Proyección
                    if (c.GetLineWeight(GraphicsStyleType.Projection) != proj)
                        c.SetLineWeight(proj, GraphicsStyleType.Projection);

                    // Corte (solo si es válido)
                    if (c.GetLineWeight(GraphicsStyleType.Cut) != cut)
                        c.SetLineWeight(cut, GraphicsStyleType.Cut);
                }
                catch
                {
                    // Algunas categorías no soportan corte → las ignoramos
                }
            }
        }

        public static void SetAnnotationWeights(Document doc, int proj)
        {
            var cats = doc.Settings.Categories;
            foreach (Category c in cats)
                if (c.CategoryType == CategoryType.Annotation &&
                    c.GetLineWeight(GraphicsStyleType.Projection) != proj)
                    c.SetLineWeight(proj, GraphicsStyleType.Projection);
        }

        // Mapa nombreCategoría -> nombrePatrón (puedes enviar nombres en ES/EN)
        public static void SetAnnotationPatterns(Document doc, IDictionary<string, string> categoryToPatternName)
        {
            var patterns = new Dictionary<string, ElementId>();
            var col = new FilteredElementCollector(doc).OfClass(typeof(LinePatternElement));
            foreach (LinePatternElement p in col) patterns[p.Name] = p.Id;

            var cats = doc.Settings.Categories;
            foreach (Category c in cats)
            {
                if (c.CategoryType != CategoryType.Annotation) continue;
                if (!categoryToPatternName.TryGetValue(c.Name, out var patternName)) continue;
                if (patterns.TryGetValue(patternName, out var pid))
                    c.SetLinePatternId(pid, GraphicsStyleType.Projection);
            }
        }
    }
}
