#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Forta.UI.WinForms;
using System.Linq;

#endregion

namespace Forta.Estructuras.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class PlantillaEstructural : IExternalCommand
    {

        #region EJECUCION DEL CODIGO
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                FrmPlantillaEstructuras form = new FrmPlantillaEstructuras();
                DialogResult resultado = form.ShowDialog();

                if (resultado == DialogResult.OK)
                {
                    string accion = form.Tag?.ToString();
                    

                    if (accion == "EstilosLinea")
                    {
                        AplicarEstilosLinea(commandData.Application.ActiveUIDocument.Document);
                        TaskDialog.Show("Éxito", "Estilos de línea aplicados correctamente.");
                    }
                    else if (accion == "InicializarFamilias")
                    {
                        InicializarFamiliasSuelos(commandData.Application.ActiveUIDocument.Document);
                        
                    }
                }

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }
        #endregion

        #region BOTON DE ESTILOS DE LINEA

        #region TODO REFERENTE A LINE PATTERNS(PATRONES DE LINEA)

        #region AQUI SE APLICAN LOS PATRONES DE LINEA QUE SE CREARON
        private void AplicarEstilosLinea(Document doc)
        {
            using (Transaction trans = new Transaction(doc, "Aplicar Estilos de Línea"))
            {
                trans.Start();

                try
                {
                    // 1. Eliminar todos los Line Patterns existentes (excepto los built-in)
                    EliminarLinePatterns(doc);

                    // 2. Crear los nuevos Line Patterns
                    CrearLinePatternContinua(doc);
                    CrearLinePatternEje(doc);
                    CrearLinePatternPunto(doc);

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.RollBack();
                    throw new Exception($"Error al aplicar estilos de línea: {ex.Message}");
                }
            }
        }

        #endregion

        #region AQUI SE ELIMINAN LOS ESTILOS DE LINEA EXISTENTES
        private void EliminarLinePatterns(Document doc)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc)
                .OfClass(typeof(LinePatternElement));

            List<ElementId> idsToDelete = new List<ElementId>();

            foreach (LinePatternElement pattern in collector)
            {
                // Solo eliminar patterns personalizados, no los built-in
                if (pattern.Name != "Solid" && pattern.Name != "<Invisible lines>")
                {
                    idsToDelete.Add(pattern.Id);
                }
            }

            if (idsToDelete.Count > 0)
            {
                doc.Delete(idsToDelete);
            }
        }

        #endregion

        #region AQUI SE CREAN LOS ESTILOS DE LINEA
        private void CrearLinePatternContinua(Document doc)
        {
            LinePattern linePattern = new LinePattern("Línea continua");
            linePattern.SetSegments(new List<LinePatternSegment>
    {
        new LinePatternSegment(LinePatternSegmentType.Dash, 3.175 / 304.8), // 3.175mm convertido a pies
        new LinePatternSegment(LinePatternSegmentType.Space, 3.175 / 304.8)  // 3.175mm convertido a pies
    });

            LinePatternElement.Create(doc, linePattern);
        }

        private void CrearLinePatternEje(Document doc)
        {
            LinePattern linePattern = new LinePattern("Línea de eje");
            linePattern.SetSegments(new List<LinePatternSegment>
    {
        new LinePatternSegment(LinePatternSegmentType.Dash, 12.7 / 304.8),   // 12.7mm
        new LinePatternSegment(LinePatternSegmentType.Space, 3.175 / 304.8), // 3.175mm
        new LinePatternSegment(LinePatternSegmentType.Dash, 3.175 / 304.8),  // 3.175mm
        new LinePatternSegment(LinePatternSegmentType.Space, 3.175 / 304.8)  // 3.175mm
    });

            LinePatternElement.Create(doc, linePattern);
        }

        private void CrearLinePatternPunto(Document doc)
        {
            LinePattern linePattern = new LinePattern("Linea Punto");
            linePattern.SetSegments(new List<LinePatternSegment>
    {
        new LinePatternSegment(LinePatternSegmentType.Dash, 4.7625 / 304.8), // 4.7625mm
        new LinePatternSegment(LinePatternSegmentType.Space, 2.3813 / 304.8) // 2.3813mm
    });

            LinePatternElement.Create(doc, linePattern);
        }

        #endregion

        #endregion

        #endregion

        #region CREACION DE MATERIALES


        #endregion

        #region CREACION DE PARAMETROS

        #endregion

        #region INICIALIZAR FAMILIAS DE SUELOS

        private void InicializarFamiliasSuelos(Document doc)
        {
            using (Transaction trans = new Transaction(doc, "Inicializar Familias de Suelos"))
            {
                trans.Start();

                try
                {
                    // Obtener todos los tipos de suelos existentes
                    var tiposSuelosExistentes = new FilteredElementCollector(doc)
                        .OfClass(typeof(FloorType))
                        .Cast<FloorType>()
                        .ToList();

                    

                    // Verificar qué tipos están en uso
                    var tiposEnUso = VerificarTiposSuelosEnUso(doc);
                    

                    // Definir las familias de suelos que queremos crear
                    var familiasSuelosDeseadas = new List<(string nombre, double espesor)>
{
                    ("LM-1", UnitUtils.ConvertToInternalUnits(0.15, UnitTypeId.Meters)), // 15 cm
                    ("LN-1", UnitUtils.ConvertToInternalUnits(0.05, UnitTypeId.Meters)), // 5 cm
                    ("FI-1", UnitUtils.ConvertToInternalUnits(0.10, UnitTypeId.Meters))  // 10 cm
};

                    

                    // Crear las familias de suelos deseadas si no existen
                    foreach (var (nombre, espesor) in familiasSuelosDeseadas)
                    {
                        if (!tiposSuelosExistentes.Any(t => t.Name == nombre))
                        {
                            CrearTipoSuelo(doc, nombre, espesor);
                        }
                        
                    }

                    // Eliminar tipos que no están en uso y no son los que queremos crear
                    var tiposParaEliminar = new List<ElementId>();

                    foreach (var tipoSuelo in tiposSuelosExistentes)
                    {
                        bool esDeseado = familiasSuelosDeseadas.Any(f => f.nombre == tipoSuelo.Name);
                        bool estaEnUso = tiposEnUso.Contains(tipoSuelo.Id);

                        if (!esDeseado && !estaEnUso)
                        {
                            tiposParaEliminar.Add(tipoSuelo.Id);
                        }
                        else if (!esDeseado && estaEnUso)
                        {
                            TaskDialog.Show("Información", $"No se pudo eliminar el suelo '{tipoSuelo.Name}' porque existe en el modelo.");
                        }
                    }

                    // Eliminar todos los tipos de una vez
                    if (tiposParaEliminar.Count > 0)
                    {
                        try
                        {
                            doc.Delete(tiposParaEliminar);
                            
                        }
                        catch (Exception ex)
                        {
                            TaskDialog.Show("Advertencia", $"Error al eliminar tipos: {ex.Message}");
                        }
                    }

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.RollBack();
                    throw new Exception("Error durante la inicialización de familias: " + ex.Message);
                }
            }
        }

        private HashSet<ElementId> VerificarTiposSuelosEnUso(Document doc)
        {
            var tiposEnUso = new HashSet<ElementId>();

            // Obtener todos los suelos en el proyecto
            var suelos = new FilteredElementCollector(doc)
                .OfClass(typeof(Floor))
                .Cast<Floor>()
                .ToList();

            foreach (var suelo in suelos)
            {
                tiposEnUso.Add(suelo.GetTypeId());
            }

            return tiposEnUso;
        }

        private void CrearTipoSuelo(Document doc, string nombre, double espesor)
        {
            try
            {
                // Buscar un tipo de suelo existente como plantilla
                var tipoPlantilla = new FilteredElementCollector(doc)
                    .OfClass(typeof(FloorType))
                    .Cast<FloorType>()
                    .FirstOrDefault();

                if (tipoPlantilla == null)
                {
                    throw new InvalidOperationException("No se encontró ningún tipo de suelo como plantilla.");
                }

                // Duplicar el tipo de suelo
                var nuevoTipo = tipoPlantilla.Duplicate(nombre) as FloorType;

                
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear el tipo de suelo '{nombre}': {ex.Message}");
            }
        }

        #endregion

        
    }
}
