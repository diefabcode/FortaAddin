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

#endregion

namespace Forta.Estructuras.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class PlantillaEstructural : IExternalCommand
    {
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
                }

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }



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

    }
}
