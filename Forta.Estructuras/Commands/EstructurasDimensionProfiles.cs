using Autodesk.Revit.DB;
using Forta.Core.Plantillas.Generales.Cotas.DimensionStyles;

namespace Forta.Estructuras.Commands
{
    public static class EstructurasDimensionProfiles
    {

        //ESTILO DE COTA 2MM SIN DECIMALES HORIZONTAL UNIDADES MILIMETROS
        public static (string name, DimStyleOptions opt) FI2mmSDHMM()
        {
            var opt = new DimStyleOptions
            {
                Text = new DimTextOptions
                {
                    Font = "Arial",
                    SizeMm = 2.0,
                    WidthFactor = 1.0,
                    Bold = 0, // 1 = ACTIVADO
                    Italic = 0,
                    Underline = 0,
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
                    Unit = UnitTypeId.Millimeters,
                    Accuracy = 1.0,   // 0 decimales
                    UseAlternate = false
                },
                Equality = new DimEqualityOptions
                {
                    EqText = "EQ",
                    EqFormula = 1,   // Longitud total
                    EqDisplay = 2    // Marca y línea
                }
            };
            return ("FI - 2mm SDH(mm)", opt);
        }

        //ESTILO DE COTA 2MM CON DECIMALES HORIZONTAL UNIDADES MILIMETROS
        public static (string name, DimStyleOptions opt) FI2mmCDHMM()
        {
            var (name, o) = FI2mmSDHMM();
            o.Text.Orientation = 0;
            o.Units.Unit = UnitTypeId.Millimeters;
            o.Units.Accuracy = 0.01;
            return ("FI - 2mm CDH(mm)", o);
        }

        //ESTILO DE COTA 2MM SIN DECIMALES HORIZONTAL CENTIMETROS
        public static (string name, DimStyleOptions opt) FI2mmSDHCM()
        {
            var (name, o) = FI2mmSDHMM();
            o.Text.Orientation = 0;
            o.Units.Unit = UnitTypeId.Centimeters;
            o.Units.Accuracy = 1.0;
            return ("FI - 2mm SDH(cm)", o);
        }

        //ESTILO DE COTA 2MM CON DECIMALES HORIZONTAL CENTIMETROS
        public static (string name, DimStyleOptions opt) FI2mmCDHCM()
        {
            var (name, o) = FI2mmSDHMM();
            o.Text.Orientation = 0;
            o.Units.Unit = UnitTypeId.Centimeters;
            o.Units.Accuracy = 0.01;
            return ("FI - 2mm CDH(cm)", o);
        }

        //ESTILO DE COTA 2MM SIN DECIMALES HORIZONTAL METROS

        public static (string name, DimStyleOptions opt) FI2mmSDHM()
        {
            var (name, o) = FI2mmSDHMM();
            o.Text.Orientation = 0;
            o.Units.Unit = UnitTypeId.Meters;
            o.Units.Accuracy = 1.0;
            return ("FI - 2mm SDH(m)", o);
        }

        //ESTILO DE COTA 2MM CON DECIMALES HORIZONTAL METROS
        public static (string name, DimStyleOptions opt) FI2mmCDHM()
        {
            var (name, o) = FI2mmSDHMM();
            o.Text.Orientation = 0;
            o.Units.Unit = UnitTypeId.Meters;
            o.Units.Accuracy = 0.01;
            return ("FI - 2mm CDH(m)", o);
        }

        // Perfil adicional para debug/testing
        public static (string name, DimStyleOptions opt) DebugStyle()
        {
            var (name, o) = FI2mmSDHMM();
            o.Text.SizeMm = 2.5;
            o.Graphics.DimLineWeight = 1;
            o.Graphics.Color = new Color(0, 128, 255); // Azul
            return ("FI - Debug Style", o);
        }
    }
}


//SERÍA BUENO AGREGAR UNA REVISIÓN QUE DETECTE SI HAY COTAS QUE SI TIENEN DECIMALES PERO NO LOS ESTÁ MOSTRANDO