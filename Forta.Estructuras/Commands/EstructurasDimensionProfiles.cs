using Autodesk.Revit.DB;
using Forta.Core.Plantillas.Generales.Cotas.DimensionStyles;

namespace Forta.Estructuras.Commands
{
    public static class EstructurasDimensionProfiles
    {
        public static (string name, DimStyleOptions opt) General2mm()
        {
            var opt = new DimStyleOptions
            {
                Text = new DimTextOptions
                {
                    Font = "Arial",
                    SizeMm = 2.0,              // ✅ CORRECTO
                    WidthFactor = 1.0,
                    Bold = 0,
                    Italic = 0,
                    Underline = 0,
                    Background = 0,           // 0 = Transparente
                    OffsetFromDimLineMm = 0.7938,
                    Orientation = 0           // Arriba, luego izquierda
                },
                Graphics = new DimGraphicsOptions
                {
                    DimLineWeight = 1,
                    TickLineWeight = 1,
                    DimLineExtensionMm = 0.0,
                    DimLineExtensionFlippedMm = 2.3813,
                    WitnessGapMm = 1.5875,
                    WitnessExtensionMm = 2.3813,
                    Color = new Color(0, 0, 0),
                    TickPreferred = new[] { "Arrow Filled 15 Degree", "Flecha 15" },
                    InsideTickPreferred = new[] { "Diagonal 1/16", "Diagonal 1/8", "Diagonal 3 mm", "Diagonal 3/32", "Diagonal" }
                },
                Units = new DimUnitsOptions
                {
                    Spec = SpecTypeId.Length,
                    Unit = UnitTypeId.Meters,
                    Accuracy = 0.01,   // 2 decimales
                    UseAlternate = false
                },
                Equality = new DimEqualityOptions
                {
                    EqText = "EQ",
                    EqFormula = 1,   // Longitud total
                    EqDisplay = 2    // Marca y línea
                }
            };
            return ("FI - 2mm SDH", opt);
        }

        // Ejemplo de otro estilo (cambias lo que difiera)
        public static (string name, DimStyleOptions opt) General3mmRoja()
        {
            var (name, o) = General2mm();
            o.Text.SizeMm = 3.0;
            o.Graphics.DimLineWeight = 2;
            o.Graphics.Color = new Color(255, 0, 0);
            return ("FI – Cota General 3mm Roja", o);
        }
    }
}
