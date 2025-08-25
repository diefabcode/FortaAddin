// Forta.Core/Plantillas/Generales/Lineas/LinePatterns/LinePatternsService.cs
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace Forta.Core.Plantillas.Generales.Lineas.LinePatterns
{
    public static class LinePatternsService
    {
        public static ElementId CreateOrUpdate(
            Document doc,
            string name,
            IList<(LinePatternSegmentType type, double length)> segments)
        {
            var existing = new FilteredElementCollector(doc)
                .OfClass(typeof(LinePatternElement))
                .Cast<LinePatternElement>()
                .FirstOrDefault(x => x.GetLinePattern().Name == name);

            var lp = new LinePattern(name);
            var segs = segments.Select(s => new LinePatternSegment(s.type, s.length)).ToList();
            lp.SetSegments(segs);

            if (existing != null)
            {
                existing.SetLinePattern(lp);
                return existing.Id;
            }

            // ⬇️ AQUÍ el fix: devuelve el Id del elemento creado
            var created = LinePatternElement.Create(doc, lp);
            return created.Id;
        }
    }
}

