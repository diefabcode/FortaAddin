#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Forta.Core.Plantillas.Generales.Lineas.EliminarAnteriores;
using Forta.Core.Plantillas.Generales.Lineas.LinePatterns;
using Forta.Core.Plantillas.Generales.Lineas.LineStyles;
using Forta.Core.Plantillas.Generales.Lineas.ObjectStyles;
using Forta.UI.WinForms;
using Forta.Core.Plantillas.Generales.Textos.TextStyles;
using Forta.Core.Plantillas.Generales.Cotas.DimensionStyles;
using System.Diagnostics;
using System;

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
                    else if (accion == "EstilosTexto")
                    {
                        AplicarTexto(commandData.Application.ActiveUIDocument.Document);
                        TaskDialog.Show("Éxito", "Se han creado los textos correctamente.");
                    }
                    else if (accion == "EstilosCotas")
                    {
                        AplicarCotas(commandData.Application.ActiveUIDocument.Document, form.DepurarCotas);
                        TaskDialog.Show("Éxito", "Se han creado/actualizado las cotas correctamente.");
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


        #region ESTILOS DE LINEA
        private void AplicarEstilosLinea(Document doc)
        {
            using (Transaction trans = new Transaction(doc, "Aplicar Estilos de Línea"))
            {
                trans.Start();
                try
                {
                    // 1) Patrones: limpiar y crear según perfiles de Estructuras
                    LinePatternsCleanup.DeleteCustom(doc);
                    foreach (var (name, segs) in EstructurasLinePatternProfiles.All())
                        LinePatternsService.CreateOrUpdate(doc, name, segs);

                    // 2) Object Styles (pesos por categoría)
                    ObjectStylesService.SetModelWeights(doc, proj: 1, cut: 2);
                    ObjectStylesService.SetAnnotationWeights(doc, proj: 1);

                    // 3) Object Styles (patrón por categoría de anotación)
                    ObjectStylesService.SetAnnotationPatterns(doc, new Dictionary<string, string>
            {
                { "Callout Boundary", "Linea de Llamada" }, { "Contorno de llamada", "Linea de Llamada" },
                { "Displacement Path", "Linea Punto" },     { "Camino de desplazamiento", "Linea Punto" },
                { "Plan Region", "Solid" },                 { "Región de plano", "Solid" },
                { "Reference Planes", "Linea de Planos de Referencia" }, { "Planos de referencia", "Linea de Planos de Referencia" },
                { "Scope Boxes", "Linea de Cajas de Referencia" },       { "Cajas de referencia", "Linea de Cajas de Referencia" },
                { "Section Line", "Linea de Corte" },       { "Línea de sección", "Linea de Corte" }
            });

                    // 4) Line Styles: borrar existentes, crear y configurar los tuyos
                    LineStylesService.RemoveCustom(doc);
                    LineStylesService.Ensure(doc, new[]
                    {
                "#1 Discontinua", "#1 Solida", "#1 Solida Roja",
                "#2 Discontinua", "#2 Solida", "#2 Solida Roja",
                "#3 Discontinua", "#3 Solida", "#3 Solida Roja"
            });

                    LineStylesService.SetProps(doc, "#1 Discontinua", 1, new Color(0, 0, 0), "Linea Discontinua");
                    LineStylesService.SetProps(doc, "#1 Solida", 1, new Color(0, 0, 0), "Solid");
                    LineStylesService.SetProps(doc, "#1 Solida Roja", 1, new Color(255, 0, 0), "Solid");
                    LineStylesService.SetProps(doc, "#2 Discontinua", 2, new Color(0, 0, 0), "Linea Discontinua");
                    LineStylesService.SetProps(doc, "#2 Solida", 2, new Color(0, 0, 0), "Solid");
                    LineStylesService.SetProps(doc, "#2 Solida Roja", 2, new Color(255, 0, 0), "Solid");
                    LineStylesService.SetProps(doc, "#3 Discontinua", 3, new Color(0, 0, 0), "Linea Discontinua");
                    LineStylesService.SetProps(doc, "#3 Solida", 3, new Color(0, 0, 0), "Solid");
                    LineStylesService.SetProps(doc, "#3 Solida Roja", 3, new Color(255, 0, 0), "Solid");

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

        #region ESTILOS DE TEXTO
        private void AplicarTexto(Document doc)
        {
            using (Transaction trans = new Transaction(doc, "Aplicar Texto"))
            {
                trans.Start();

                // Tus tres estilos actuales:
                TextStylesService.CreateOrUpdate(
                    doc,
                    name: "FI Flecha Arial 2mm",
                    font: "Arial",
                    sizeMm: 2,
                    color: new Color(0, 0, 0),
                    lineWeight: 1,
                    bold: false,
                    italic: false,
                    underline: false,
                    widthFactor: 1.0,
                    leaderOffsetMm: 1.5,
                    tabSizeMm: 8.0,
                    arrowType: "flecha"
                );

                TextStylesService.CreateOrUpdate(
                    doc,
                    name: "FI Punto Arial 2mm",
                    font: "Arial",
                    sizeMm: 2,
                    color: new Color(0, 0, 0),
                    lineWeight: 1,
                    bold: false,
                    italic: false,
                    underline: false,
                    widthFactor: 1.0,
                    leaderOffsetMm: 1.5,
                    tabSizeMm: 8.0,
                    arrowType: "punto"
                );

                TextStylesService.CreateOrUpdate(
                    doc,
                    name: "FI Diagonal Arial 2mm",
                    font: "Arial",
                    sizeMm: 2,
                    color: new Color(0, 0, 0),
                    lineWeight: 1,
                    bold: false,
                    italic: false,
                    underline: false,
                    widthFactor: 1.0,
                    leaderOffsetMm: 1.5,
                    tabSizeMm: 8.0,
                    arrowType: "diagonal"
                );

                trans.Commit();
            }

            
        }
        #endregion

        #region

        private void AplicarCotas(Document doc, bool depurarCotas)
        {
            using (var t = new Transaction(doc, "Aplicar Cotas – Estructuras"))
            {
                t.Start();
                try
                {
                    Debug.WriteLine("=== INICIANDO CREACIÓN DE COTAS ===");

                    var factories = new Func<(string name, DimStyleOptions opt)>[]
                    {
                EstructurasDimensionProfiles.FI2mmSDHMM,
                EstructurasDimensionProfiles.FI2mmCDHMM,
                EstructurasDimensionProfiles.FI2mmSDHCM,
                EstructurasDimensionProfiles.FI2mmCDHCM,
                EstructurasDimensionProfiles.FI2mmCDHM,
                EstructurasDimensionProfiles.FI2mmSDHM
                    };

                    foreach (var f in factories)
                    {
                        var (name, opt) = f();
                        Debug.WriteLine($"Creando estilo: {name}");
                        var id = DimensionStyleService.CreateOrUpdate(doc, name, opt);
                        Debug.WriteLine($"Estilo {name} creado con ID: {id}");
                    }

                    t.Commit();
                    Debug.WriteLine("=== COTAS CREADAS EXITOSAMENTE ===");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"ERROR en AplicarCotas: {ex.Message}");
                    Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                    t.RollBack();
                    throw;
                }
            }

            // 🔽 Fuera de la transacción (evita transacciones anidadas)
            if (depurarCotas)
            {
                var eliminadas = DimStyleCleanup.DepurarCotasNoFI(doc);
                Debug.WriteLine($"[DepurarCotas] Eliminadas: {eliminadas}");

                TaskDialog.Show("Forta – Cotas",
                    eliminadas > 0
                    ? $"Se eliminaron {eliminadas} cotas cuyo tipo no empieza con \"FI\"."
                    : "No se encontraron cotas para depurar.");
            }
        }
    }
    }
        

        #endregion

        #region BOTON DE MATERIALES


        #endregion

        #region CREACION DE PARAMETROS

        #endregion



    

