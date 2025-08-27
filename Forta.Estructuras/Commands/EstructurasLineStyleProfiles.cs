using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace Forta.Estructuras.Commands
{
    public static class EstructurasLineStyleProfiles
    {
        public class LineStyleConfig
        {
            public string Name { get; set; }
            public int Weight { get; set; }
            public Color Color { get; set; }
            public string PatternName { get; set; }
        }

        public static LineStyleConfig DiscontinuaN1 =>
            new LineStyleConfig 
            { 
                Name = "#1 Discontinua", 
                Weight = 1, 
                Color = new Color(0, 0, 0), 
                PatternName = "FI Linea Discontinua" 
            };

        public static LineStyleConfig SolidaN1 =>
            new LineStyleConfig 
            { 
                Name = "#1 Solida", 
                Weight = 1, 
                Color = new Color(0, 0, 0), 
                PatternName = "Solid" 
            };

        public static LineStyleConfig SolidaRojaN1 =>
            new LineStyleConfig 
            { 
                Name = "#1 Solida Roja", 
                Weight = 1, 
                Color = new Color(255, 0, 0), 
                PatternName = "Solid" 
            };

        public static LineStyleConfig DiscontinuaN2 =>
            new LineStyleConfig 
            { 
                Name = "#2 Discontinua", 
                Weight = 2, 
                Color = new Color(0, 0, 0), 
                PatternName = "FI Linea Discontinua" 
            };

        public static LineStyleConfig SolidaN2 =>
            new LineStyleConfig 
            { 
                Name = "#2 Solida", 
                Weight = 2, 
                Color = new Color(0, 0, 0), 
                PatternName = "Solid" 
            };

        public static LineStyleConfig SolidaRojaN2 =>
            new LineStyleConfig 
            { 
                Name = "#2 Solida Roja", 
                Weight = 2, 
                Color = new Color(255, 0, 0), 
                PatternName = "Solid" 
            };

        public static LineStyleConfig DiscontinuaN3 =>
            new LineStyleConfig 
            { 
                Name = "#3 Discontinua", 
                Weight = 3, 
                Color = new Color(0, 0, 0), 
                PatternName = "FI Linea Discontinua" 
            };

        public static LineStyleConfig SolidaN3 =>
            new LineStyleConfig 
            { 
                Name = "#3 Solida", 
                Weight = 3, 
                Color = new Color(0, 0, 0), 
                PatternName = "Solid" 
            };

        public static LineStyleConfig SolidaRojaN3 =>
            new LineStyleConfig 
            { 
                Name = "#3 Solida Roja", 
                Weight = 3, 
                Color = new Color(255, 0, 0), 
                PatternName = "Solid" 
            };

        public static IEnumerable<LineStyleConfig> All()
        {
            yield return DiscontinuaN1;
            yield return SolidaN1;
            yield return SolidaRojaN1;
            yield return DiscontinuaN2;
            yield return SolidaN2;
            yield return SolidaRojaN2;
            yield return DiscontinuaN3;
            yield return SolidaN3;
            yield return SolidaRojaN3;
        }
    }
}