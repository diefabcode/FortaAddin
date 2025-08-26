using Autodesk.Revit.DB;
using Forta.Core.Plantillas.Generales.Cotas.DimensionStyles;

namespace Forta.Estructuras.Commands
{
    public static class EstructurasDimensionProfiles
    {
        private static readonly string[] TickPref = {
            "Flecha 15 grados rellenada",  // PRIMERA PRIORIDAD
            "Arrow Filled 15 Degree",      // SEGUNDA PRIORIDAD
            "Flecha 20 grados rellenada",
            "Arrow Filled 20 Degree",
            "Flecha 30 grados rellenada",  // Última opción
            "Arrow Filled 30 Degree",
            "Arrow Filled",
            "Filled Arrow"
        };

                private static readonly string[] InsideTickPref = {
            "Diagonal 1/16",
            "Diagonal 1/8",
            "Diagonal 3 mm",
            "Diagonal 3/32",
            "Diagonal"
        };

        private static string NameSuffix(string baseName, string unitTag)
    => $"{baseName}({unitTag})";

        private const string BaseFont = "Arial";
        private const double BaseTextSizeMm = 2.0;
        private const double BaseOffsetFromDimLineMm = 0.7938;
        private const int BaseDimLineWeight = 1;
        private const int BaseTickLineWeight = 1;
        private const double BaseDimLineExtensionMm = 0.0;
        private const double BaseDimLineExtensionFlippedMm = 2.3813;
        private const double BaseWitnessGapMm = 1.5875;
        private const double BaseWitnessExtensionMm = 2.3813;
        private static readonly Color BaseColor = new Color(0, 0, 0);
        private const int OrientationHorizontal = 0;
        private const int TextBackgroundTransparent = 1;
        private const int EqFormulaTotalLength = 1;   // Longitud total
        private const int EqDisplayMarkAndLine = 2;   // “Marca y línea”

        //ESTILO DE COTA 2MM SIN DECIMALES HORIZONTAL UNIDADES MILIMETROS
        public static (string name, DimStyleOptions opt) FI2mmSDHMM()
        {
            var opt = new DimStyleOptions
            {
                Text = new DimTextOptions
                {
                    Font = BaseFont,
                    SizeMm = BaseTextSizeMm,
                    WidthFactor = 1.0,
                    Bold = 0,
                    Italic = 0,
                    Underline = 0,
                    Background = TextBackgroundTransparent,
                    OffsetFromDimLineMm = BaseOffsetFromDimLineMm,
                    Orientation = OrientationHorizontal
                },
                Graphics = new DimGraphicsOptions
                {
                    DimLineWeight = BaseDimLineWeight,
                    TickLineWeight = BaseTickLineWeight,
                    DimLineExtensionMm = BaseDimLineExtensionMm,
                    DimLineExtensionFlippedMm = BaseDimLineExtensionFlippedMm,
                    WitnessGapMm = BaseWitnessGapMm,
                    WitnessExtensionMm = BaseWitnessExtensionMm,
                    Color = BaseColor,
                    TickPreferred = TickPref,
                    InsideTickPreferred = InsideTickPref
                },
                Units = new DimUnitsOptions
                {
                    Unit = UnitTypeId.Millimeters,
                    Accuracy = 1.0 // sin decimales en mm
                },
                Equality = new DimEqualityOptions
                {
                    EqText = "EQ",
                    EqFormula = EqFormulaTotalLength,
                    EqDisplay = EqDisplayMarkAndLine
                }
            };
            return (NameSuffix("FI - 2mm SDH", "mm"), opt);
        }

        //ESTILO DE COTA 2MM CON DECIMALES HORIZONTAL UNIDADES MILIMETROS
        public static (string name, DimStyleOptions opt) FI2mmCDHMM()
        {
            var (_, o) = FI2mmSDHMM();
            o.Units.Unit = UnitTypeId.Millimeters;
            o.Units.Accuracy = 0.01;
            return (NameSuffix("FI - 2mm CDH", "mm"), o);
        }

        //ESTILO DE COTA 2MM SIN DECIMALES HORIZONTAL CENTIMETROS
        public static (string name, DimStyleOptions opt) FI2mmSDHCM()
        {
            var (_, o) = FI2mmSDHMM();
            o.Units.Unit = UnitTypeId.Centimeters;
            o.Units.Accuracy = 1.0;
            return (NameSuffix("FI - 2mm SDH", "cm"), o);
        }

        //ESTILO DE COTA 2MM CON DECIMALES HORIZONTAL CENTIMETROS
        public static (string name, DimStyleOptions opt) FI2mmCDHCM()
        {
            var (_, o) = FI2mmSDHMM();
            o.Units.Unit = UnitTypeId.Centimeters;
            o.Units.Accuracy = 0.01;
            return (NameSuffix("FI - 2mm CDH", "cm"), o);
        }

        //ESTILO DE COTA 2MM SIN DECIMALES HORIZONTAL METROS

        public static (string name, DimStyleOptions opt) FI2mmSDHM()
        {
            var (_, o) = FI2mmSDHMM();
            o.Units.Unit = UnitTypeId.Meters;
            o.Units.Accuracy = 1.0;
            return (NameSuffix("FI - 2mm SDH", "m"), o);
        }

        //ESTILO DE COTA 2MM CON DECIMALES HORIZONTAL METROS
        public static (string name, DimStyleOptions opt) FI2mmCDHM()
        {
            var (_, o) = FI2mmSDHMM();
            o.Units.Unit = UnitTypeId.Meters;
            o.Units.Accuracy = 0.01;
            return (NameSuffix("FI - 2mm CDH", "m"), o);
        }
    }
}


//SERÍA BUENO AGREGAR UNA REVISIÓN QUE DETECTE SI HAY COTAS QUE SI TIENEN DECIMALES PERO NO LOS ESTÁ MOSTRANDO