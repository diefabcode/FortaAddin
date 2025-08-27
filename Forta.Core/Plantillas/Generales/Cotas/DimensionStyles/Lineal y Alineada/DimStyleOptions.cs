using Autodesk.Revit.DB;

namespace Forta.Core.Plantillas.Generales.Cotas.DimensionStyles
{
    public class DimTextOptions
    {
        public string Font = "Arial";
        public double SizeMm = 2.0;
        public double WidthFactor = 1.0;
        public int Bold = 0, Italic = 0, Underline = 0;
        public int Background = 0; // 0 transparente
        public double OffsetFromDimLineMm = 0.7938;
        // 0 = Arriba-luego-izq (dependiendo build)
        public int Orientation = 0;
    }

    public class DimGraphicsOptions
    {
        public int DimLineWeight = 1;
        public int TickLineWeight = 1;
        public double DimLineExtensionMm = 0.0;
        public double DimLineExtensionFlippedMm = 2.3813;
        public double WitnessGapMm = 1.5875;
        public double WitnessExtensionMm = 2.3813;
        public Color Color = new Color(0, 0, 0);
        // Nombres preferidos para marcas
        public string[] TickPreferred = { "Arrow Filled 15 Degree", "Flecha 15" };
        public string[] InsideTickPreferred = { "Diagonal 1/16", "Diagonal 1/8", "Diagonal 3 mm", "Diagonal 3/32", "Diagonal" };
    }

    public class DimUnitsOptions
    {
        // Unidades principales
        public ForgeTypeId Spec = SpecTypeId.Length;
        public ForgeTypeId Unit = UnitTypeId.Meters;
        public double Accuracy = 0.01; // 2 decimales
        // Alternativas
        public bool UseAlternate = false;
        public ForgeTypeId AltUnit = UnitTypeId.Meters;
        public double AltAccuracy = 0.01;
    }

    public class DimEqualityOptions
    {
        public string EqText = "EQ";
        // 1 ≈ Longitud total; 2 ≈ Marca y línea, etc. (varía por build)
        public int EqFormula = 1;
        public int EqDisplay = 2;
    }

    public class DimStyleOptions
    {
        public DimTextOptions Text = new DimTextOptions();
        public DimGraphicsOptions Graphics = new DimGraphicsOptions();
        public DimUnitsOptions Units = new DimUnitsOptions();
        public DimEqualityOptions Equality = new DimEqualityOptions();
    }
}

