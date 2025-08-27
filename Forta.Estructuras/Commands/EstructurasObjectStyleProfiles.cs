using System.Collections.Generic;

namespace Forta.Estructuras.Commands
{
    public static class EstructurasObjectStyleProfiles
    {
        public class ModelWeightConfig
        {
            public int ProjectionWeight { get; set; }
            public int CutWeight { get; set; }
        }

        public class AnnotationWeightConfig
        {
            public int ProjectionWeight { get; set; }
        }

        public static ModelWeightConfig ModelWeights =>
            new ModelWeightConfig 
            { 
                ProjectionWeight = 1, 
                CutWeight = 2 
            };

        public static AnnotationWeightConfig AnnotationWeights =>
            new AnnotationWeightConfig 
            { 
                ProjectionWeight = 1 
            };

        public static Dictionary<string, string> AnnotationPatterns =>
            new Dictionary<string, string>
            {
                { "Callout Boundary", "FI Linea de Llamada" }, 
                { "Contorno de llamada", "FI Linea de Llamada" },
                { "Displacement Path", "FI Linea Punto" },     
                { "Camino de desplazamiento", "FI Linea Punto" },
                { "Plan Region", "Solid" },                 
                { "Región de plano", "Solid" },
                { "Reference Planes", "FI Linea de Planos de Referencia" }, 
                { "Planos de referencia", "FI Linea de Planos de Referencia" },
                { "Scope Boxes", "FI Linea de Cajas de Referencia" },       
                { "Cajas de referencia", "FI Linea de Cajas de Referencia" },
                { "Section Line", "FI Linea de Corte" },       
                { "Línea de sección", "FI Linea de Corte" }
            };
    }
}