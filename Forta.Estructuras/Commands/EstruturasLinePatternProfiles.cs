using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace Forta.Estructuras.Commands
{
    public static class EstructurasLinePatternProfiles
    {
        public static (string name, IList<(LinePatternSegmentType, double)> segs) Discontinua =>
            ("Linea Discontinua", new List<(LinePatternSegmentType, double)>{
                (LinePatternSegmentType.Dash, 3.175/304.8),
                (LinePatternSegmentType.Space,3.175/304.8)
            });

        public static (string, IList<(LinePatternSegmentType, double)>) Eje =>
            ("Línea de eje", new List<(LinePatternSegmentType, double)>{
                (LinePatternSegmentType.Dash, 12.7/304.8),
                (LinePatternSegmentType.Space,3.175/304.8),
                (LinePatternSegmentType.Dash, 3.175/304.8),
                (LinePatternSegmentType.Space,3.175/304.8)
            });

        public static (string, IList<(LinePatternSegmentType, double)>) Punto =>
            ("Linea Punto", new List<(LinePatternSegmentType, double)>{
                (LinePatternSegmentType.Dash, 4.7625/304.8),
                (LinePatternSegmentType.Space,2.3813/304.8)
            });

        public static (string, IList<(LinePatternSegmentType, double)>) Llamada =>
            ("Linea de Llamada", new List<(LinePatternSegmentType, double)>{
                (LinePatternSegmentType.Dash, 15/304.8),
                (LinePatternSegmentType.Space,2.5/304.8),
                (LinePatternSegmentType.Dash, 5/304.8),
                (LinePatternSegmentType.Space,2.5/304.8),
                (LinePatternSegmentType.Dash, 5/304.8),
                (LinePatternSegmentType.Space,2.5/304.8)
            });

        public static (string, IList<(LinePatternSegmentType, double)>) CajasRef =>
            ("Linea de Cajas de Referencia", new List<(LinePatternSegmentType, double)>{
                (LinePatternSegmentType.Dash, 3/304.8),
                (LinePatternSegmentType.Space,3/304.8)
            });

        public static (string, IList<(LinePatternSegmentType, double)>) PlanosRef => CajasRef;

        public static (string, IList<(LinePatternSegmentType, double)>) Oculta =>
            ("Linea Oculta", new List<(LinePatternSegmentType, double)>{
                (LinePatternSegmentType.Dash, 4.7625/304.8),
                (LinePatternSegmentType.Space,2.3813/304.8)
            });

        public static (string, IList<(LinePatternSegmentType, double)>) Proyeccion =>
            ("Linea de Proyección", new List<(LinePatternSegmentType, double)>{
                (LinePatternSegmentType.Dash, 2/304.8),
                (LinePatternSegmentType.Space,2/304.8)
            });

        public static (string, IList<(LinePatternSegmentType, double)>) Corte =>
            ("Linea de Corte", new List<(LinePatternSegmentType, double)>{
                (LinePatternSegmentType.Dash, 6.35/304.8),
                (LinePatternSegmentType.Space,4.7625/304.8),
                (LinePatternSegmentType.Dot, 0),
                (LinePatternSegmentType.Space,4.7625/304.8)
            });

        public static IEnumerable<(string, IList<(LinePatternSegmentType, double)>)> All()
        {
            yield return Discontinua;
            yield return Eje;
            yield return Punto;
            yield return Llamada;
            yield return CajasRef;
            yield return PlanosRef;
            yield return Oculta;
            yield return Proyeccion;
            yield return Corte;
        }
    }
}

