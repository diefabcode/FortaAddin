using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace Forta.Core.Plantillas.Generales.Lineas.LineStyles
{
    public static class LineStylesService
    {
        public static void RemoveCustom(Document doc)
        {
            var linesCat = doc.Settings.Categories.get_Item(BuiltInCategory.OST_Lines);
            var toDelete = new List<ElementId>();
            foreach (Category sc in linesCat.SubCategories)
                if (!sc.Name.StartsWith("<")) toDelete.Add(sc.Id);
            if (toDelete.Count > 0) doc.Delete(toDelete);
        }

        public static void Ensure(Document doc, IEnumerable<string> names)
        {
            var linesCat = doc.Settings.Categories.get_Item(BuiltInCategory.OST_Lines);
            foreach (var name in names)
                if (!Exists(linesCat, name))
                    doc.Settings.Categories.NewSubcategory(linesCat, name);
        }

        public static void SetProps(Document doc, string styleName, int weight, Color color, string patternNameOrSolid)
        {
            var linesCat = doc.Settings.Categories.get_Item(BuiltInCategory.OST_Lines);
            foreach (Category sc in linesCat.SubCategories)
            {
                if (sc.Name != styleName) continue;
                sc.SetLineWeight(weight, GraphicsStyleType.Projection);
                sc.LineColor = color;
                if (patternNameOrSolid != "Solid")
                {
                    var col = new FilteredElementCollector(doc).OfClass(typeof(LinePatternElement));
                    foreach (LinePatternElement p in col)
                        if (p.Name == patternNameOrSolid)
                        { sc.SetLinePatternId(p.Id, GraphicsStyleType.Projection); break; }
                }
                break;
            }
        }

        private static bool Exists(Category linesCat, string name)
        {
            foreach (Category sc in linesCat.SubCategories) if (sc.Name == name) return true;
            return false;
        }
    }
}
