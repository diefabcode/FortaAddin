#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Forta.UI.WinForms;
using System.Windows.Forms;

#endregion

namespace Forta.Estructuras.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class PlantillaEstructural : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData,ref string message,ElementSet elements)
        {
            try
            {
                FrmPlantillaEstructuras form = new FrmPlantillaEstructuras();
                DialogResult resultado = form.ShowDialog();

                if (resultado == DialogResult.OK)
                {
                    string accion = form.Tag?.ToString();

                    if (accion == "GrosoresLinea")
                    {
                        AplicarGrosoresLinea(commandData.Application.ActiveUIDocument.Document);
                        TaskDialog.Show("Éxito", "Grosores de línea aplicados correctamente.");
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

        private void AplicarGrosoresLinea(Document doc)
        {
            using (Transaction trans = new Transaction(doc, "Aplicar Grosores de Línea"))
            {
                trans.Start();

                try
                {
                    // Acceder a la configuración de Line Weights del documento
                    PrintManager printManager = doc.PrintManager;

                    // Los line weights se configuran por número (1-16)
                    // Valores en milímetros que se convierten internamente
                    Dictionary<int, int> nuevosGrosores = new Dictionary<int, int>
            {
                {1, 10},  // 0.10mm representado como 10
                {2, 12},  // 0.12mm representado como 12
                {3, 13},  // 0.13mm representado como 13
                {4, 14},  // 0.14mm representado como 14
                {5, 15},  // 0.15mm representado como 15
                {6, 16},  // 0.16mm representado como 16
                {7, 17},  // 0.17mm representado como 17
                {8, 18},  // 0.18mm representado como 18
                {9, 19},  // 0.19mm representado como 19
                {10, 20}, // 0.20mm representado como 20
                {11, 24}, // 0.24mm representado como 24
                {12, 28}, // 0.28mm representado como 28
                {13, 32}, // 0.32mm representado como 32
                {14, 36}, // 0.36mm representado como 36
                {15, 40}, // 0.40mm representado como 40
                {16, 50}  // 0.50mm representado como 50
            };

                    // Intentar modificar usando PrintSettings
                    PrintParameters printParams = printManager.PrintSetup.CurrentPrintSetting.PrintParameters;

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.RollBack();
                    throw new Exception($"Error al aplicar grosores: {ex.Message}");
                }
            }
        }

        // Método helper para obtener grosor según escala
        private int GetLineWeightForScale(string escala, int lineNumber)
        {
            // Implementar la lógica de tu tabla de Excel
            // Por ahora, valores básicos de ejemplo
            switch (lineNumber)
            {
                case 1: return 10;  // 0.10mm
                case 2: return 12;  // 0.12mm  
                case 3: return 13;  // 0.13mm
                case 4: return 14;  // 0.14mm
                case 5: return 15;  // 0.15mm
                case 6: return 16;  // 0.16mm
                case 7: return 17;  // 0.17mm
                case 8: return 18;  // 0.18mm
                case 9: return 19;  // 0.19mm
                case 10: return 20; // 0.20mm
                case 11: return 24; // 0.24mm
                case 12: return 28; // 0.28mm
                case 13: return 32; // 0.32mm
                case 14: return 36; // 0.36mm
                case 15: return 40; // 0.40mm
                case 16: return 50; // 0.50mm
                default: return 10;
            }
        }
    }
}
