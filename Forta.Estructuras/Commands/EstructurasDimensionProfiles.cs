using Autodesk.Revit.DB;
using Forta.Core.Plantillas.Generales.Cotas.DimensionStyles;

namespace Forta.Estructuras.Commands
{
    public static class EstructurasDimensionProfiles
    {
        public static (string name, DimStyleOptions opt) FI2mmSDH()
        {
            var opt = new DimStyleOptions
            {
                Text = new DimTextOptions
                {
                    Font = "Arial",
                    SizeMm = 2.0,
                    WidthFactor = 1.0,
                    Bold = 1, // 1 = ACTIVADO
                    Italic = 1,
                    Underline = 1,
                    Background = 1,           // 1 = Transparente
                    OffsetFromDimLineMm = 0.7938,
                    Orientation = 0           // Horizontal
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
                    // PRIORIDAD a 15 grados
                    TickPreferred = new[] {
                        "Flecha 15 grados rellenada",  // PRIMERA PRIORIDAD
                        "Arrow Filled 15 Degree",      // SEGUNDA PRIORIDAD
                        "Flecha 20 grados rellenada",
                        "Arrow Filled 20 Degree",
                        "Flecha 30 grados rellenada",  // Última opción
                        "Arrow Filled 30 Degree",
                        "Arrow Filled",
                        "Filled Arrow"
                    },
                    InsideTickPreferred = new[] {
                        "Diagonal 1/16",
                        "Diagonal 1/8",
                        "Diagonal 3 mm",
                        "Diagonal 3/32",
                        "Diagonal"
                    }
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

        public static (string name, DimStyleOptions opt) FI2mmSDV()
        {
            var (name, o) = FI2mmSDH();
            o.Text.SizeMm = 2.0;
            o.Graphics.DimLineWeight = 2;
            o.Graphics.TickLineWeight = 2;
            o.Graphics.Color = new Color(0, 0, 0);
            o.Text.Orientation = 1;
            o.Units.Unit = UnitTypeId.Centimeters;
            return ("FI - 2mm SDV", o);
        }

        // Perfil adicional para debug/testing
        public static (string name, DimStyleOptions opt) DebugStyle()
        {
            var (name, o) = FI2mmSDH();
            o.Text.SizeMm = 2.5;
            o.Graphics.DimLineWeight = 1;
            o.Graphics.Color = new Color(0, 128, 255); // Azul
            return ("FI - Debug Style", o);
        }
    }
}