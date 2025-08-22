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

        #region AQUI SE EJECUTA TODO EL CODIGO REFERENTE A LINEAS
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
                    CrearLinePatternDiscontinua(doc);
                    CrearLinePatternEje(doc);
                    CrearLinePatternPunto(doc);
                    CrearLineadeLLamada(doc);
                    CrearLineaPatternCajaReferencia(doc);
                    CrearLineaPatternPlanosReferencia(doc);
                    CrearLineaPatternOculta(doc);
                    CrearLineaPatternProyeccion(doc);
                    CrearLineaPatternCorte(doc);

                    // 3. Asignar grosores de líneas
                    ConfigurarObjectStyles(doc);
                    ConfigurarObjectStylesAnotacion(doc);

                    // 4. Configurar estilos de linea
                    // 4. Configurar Line Styles
                    ConfigurarLineStyles(doc);

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

        #region TODO REFERENTE A LINE PATTERNS(PATRONES DE LINEA)

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
        private void CrearLinePatternDiscontinua(Document doc)
        {
            LinePattern linePattern = new LinePattern("Linea Discontinua");
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

        private void CrearLineadeLLamada(Document doc)
        {
            LinePattern linePattern = new LinePattern("Linea de Llamada");
            linePattern.SetSegments(new List<LinePatternSegment>
    {
        new LinePatternSegment(LinePatternSegmentType.Dash, 15 / 304.8), // 15mm
        new LinePatternSegment(LinePatternSegmentType.Space, 2.5 / 304.8), // 2.5mm
        new LinePatternSegment(LinePatternSegmentType.Dash, 5/304.8), // 5
        new LinePatternSegment(LinePatternSegmentType.Space, 2.5 / 304.8), // 2.5mm
        new LinePatternSegment(LinePatternSegmentType.Dash, 5/304.8), // 5
        new LinePatternSegment(LinePatternSegmentType.Space, 2.5 / 304.8) // 2.5mm

    });

            LinePatternElement.Create(doc, linePattern);
        }

        private void CrearLineaPatternCajaReferencia(Document doc)
        {
            LinePattern linePattern = new LinePattern("Linea de Cajas de Referencia");
            linePattern.SetSegments(new List<LinePatternSegment>
    {
        new LinePatternSegment(LinePatternSegmentType.Dash, 3 / 304.8), // 3mm
        new LinePatternSegment(LinePatternSegmentType.Space, 3 / 304.8), // 3mm       

    });

            LinePatternElement.Create(doc, linePattern);
        }

        private void CrearLineaPatternPlanosReferencia(Document doc)
        {
            LinePattern linePattern = new LinePattern("Linea de Planos de Referencia");
            linePattern.SetSegments(new List<LinePatternSegment>
    {
        new LinePatternSegment(LinePatternSegmentType.Dash, 3 / 304.8), // 3mm
        new LinePatternSegment(LinePatternSegmentType.Space, 3 / 304.8), // 3mm       

    });

            LinePatternElement.Create(doc, linePattern);
        }

        private void CrearLineaPatternOculta(Document doc)
        {
            LinePattern linePattern = new LinePattern("Linea Oculta");
            linePattern.SetSegments(new List<LinePatternSegment>
    {
        new LinePatternSegment(LinePatternSegmentType.Dash, 4.7625 / 304.8), // 4.7625mm
        new LinePatternSegment(LinePatternSegmentType.Space, 2.3813 / 304.8), // 2.3813mm       

    });

            LinePatternElement.Create(doc, linePattern);
        }

        private void CrearLineaPatternProyeccion(Document doc)
        {
            LinePattern linePattern = new LinePattern("Linea de Proyección");
            linePattern.SetSegments(new List<LinePatternSegment>
    {
        new LinePatternSegment(LinePatternSegmentType.Dash, 2 / 304.8), // 2mm
        new LinePatternSegment(LinePatternSegmentType.Space, 2 / 304.8), // 2mm       

    });

            LinePatternElement.Create(doc, linePattern);
        }

        private void CrearLineaPatternCorte(Document doc)
        {
            LinePattern linePattern = new LinePattern("Linea de Corte");
            linePattern.SetSegments(new List<LinePatternSegment>
    {
        new LinePatternSegment(LinePatternSegmentType.Dash, 6.35 / 304.8), // 6.35mm
        new LinePatternSegment(LinePatternSegmentType.Space, 4.7625 / 304.8), // 4.7625mm
        new LinePatternSegment(LinePatternSegmentType.Dot, 0), // 
        new LinePatternSegment(LinePatternSegmentType.Space, 4.7625 / 304.8), // 4.7625mm

    });

            LinePatternElement.Create(doc, linePattern);
        }

        #endregion

        #endregion

        #region TODO LO REFERENTE A OBJECT STYLES (ESTILOS DE OBJETO)

        #region OBJETOS DE MODELO

        #region ASIGNAR GROSORES DE LINEA

        //OBJETOS DE MODELO//
        private void ConfigurarObjectStyles(Document doc)
        {
            try
            {
                //Obtener todas las categorías del modelo
                Categories categories = doc.Settings.Categories;

                foreach(Category category in categories)
                    {
                    //Solo procesar categorias de modelo (no anotación)
                    if (category.CategoryType == CategoryType.Model && category.CanAddSubcategory)
                    {
                        ConfigurarGrosoresCategoria(category);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al confugurar Object Styles: {ex.Message}");
            }
        }

        private void ConfigurarGrosoresCategoria(Category category)
        {
            try
            {
                // Configurar grosor de proyección = 1
                if (category.GetLineWeight(GraphicsStyleType.Projection) != 1)
                {
                    category.SetLineWeight(1, GraphicsStyleType.Projection);
                }

                // Configurar grosor de corte = 2
                if (category.GetLineWeight(GraphicsStyleType.Cut) != 2)
                {
                    category.SetLineWeight(2, GraphicsStyleType.Cut);
                }
            }
            catch (Exception ex)
            {
                // Si hay error con una categoría específica, continuamos con las demás
                Debug.WriteLine($"Error configurando categoría {category.Name}: {ex.Message}");
            }
        }

        //OBJETOS DE ANOTACIÓN//

        #endregion

        #endregion

        #region OBJETOS DE ANOTACIÓN

        private void ConfigurarObjectStylesAnotacion(Document doc)
        {
            try
            {
                // Obtener todas las categorías de anotación
                Categories categories = doc.Settings.Categories;

                foreach (Category category in categories)
                {
                    // Solo procesar categorías de anotación
                    if (category.CategoryType == CategoryType.Annotation)
                    {
                        // Configurar grosor de proyección = 1 para todas
                        ConfigurarGrosorAnotacion(category);

                        // Configurar line patterns específicos
                        ConfigurarLinePatternAnotacion(doc, category);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al configurar Object Styles de anotación: {ex.Message}");
            }
        }

        private void ConfigurarGrosorAnotacion(Category category)
        {
            try
            {
                // Configurar grosor de proyección = 1 para todas las categorías de anotación
                if (category.GetLineWeight(GraphicsStyleType.Projection) != 1)
                {
                    category.SetLineWeight(1, GraphicsStyleType.Projection);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error configurando grosor para categoría de anotación {category.Name}: {ex.Message}");
            }
        }

        private void ConfigurarLinePatternAnotacion(Document doc, Category category)
        {
            try
            {
                string linePatternName = "";

                // Determinar qué line pattern usar según el nombre de la categoría (inglés y español)
                switch (category.Name)
                {
                    case "Callout Boundary":
                    case "Contorno de llamada":
                        linePatternName = "Linea de llamada";
                        break;
                    case "Displacement Path":
                    case "Camino de desplazamiento":
                        linePatternName = "Linea Punto";
                        break;
                    case "Plan Region":
                    case "Región de plano":
                        linePatternName = "Línea continua";
                        break;
                    case "Reference Planes":
                    case "Planos de referencia":
                        linePatternName = "Linea de Planos de Referencia";
                        break;
                    case "Scope Boxes":
                    case "Cajas de referencia":
                        linePatternName = "Linea de Cajas de Referencia";
                        break;
                    case "Section Line":
                    case "Línea de sección":
                        linePatternName = "Linea de Corte";
                        break;
                }

                // Si se encontró un patrón específico, aplicarlo
                if (!string.IsNullOrEmpty(linePatternName))
                {
                    AsignarLinePattern(doc, category, linePatternName);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error configurando line pattern para categoría {category.Name}: {ex.Message}");
            }
        }

        private void AsignarLinePattern(Document doc, Category category, string linePatternName)
        {
            try
            {
                // Buscar el line pattern por nombre
                FilteredElementCollector collector = new FilteredElementCollector(doc)
                    .OfClass(typeof(LinePatternElement));

                LinePatternElement linePatternElement = null;

                foreach (LinePatternElement pattern in collector)
                {
                    if (pattern.Name == linePatternName)
                    {
                        linePatternElement = pattern;
                        break;
                    }
                }

                if (linePatternElement != null)
                {
                    // Asignar el line pattern a la categoría
                    category.SetLinePatternId(linePatternElement.Id, GraphicsStyleType.Projection);
                }
                else
                {
                    Debug.WriteLine($"Advertencia: No se encontró el line pattern '{linePatternName}' para la categoría '{category.Name}'");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error asignando line pattern '{linePatternName}' a categoría '{category.Name}': {ex.Message}");
            }
        }

        #endregion

        #endregion

        #region ESTILOS DE LÍNEA

        #region CONFIGURACION DE LINE STYLES (ESTILOS DE LINEA)

        private void ConfigurarLineStyles(Document doc)
        {
            try
            {
                // Obtener la categoría principal de líneas
                Category linesCategory = doc.Settings.Categories.get_Item(BuiltInCategory.OST_Lines);

                // PRIMERO: Eliminar todos los estilos personalizados (que no empiecen con "<")
                EliminarLineStylesPersonalizados(doc, linesCategory.SubCategories, null);

                // SEGUNDO: Crear los nuevos estilos de línea
                CrearNuevosLineStyles(doc);

                // TERCERO: Configurar propiedades de todos los estilos
                ConfigurarPropiedadesLineStyles(doc);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al configurar Line Styles: {ex.Message}");
            }
        }

        private void EliminarLineStylesPersonalizados(Document doc, CategoryNameMap lineCategories, HashSet<string> estilosBuiltIn)
        {
            try
            {
                List<ElementId> idsToDelete = new List<ElementId>();

                foreach (Category lineStyle in lineCategories)
                {
                    // Eliminar TODOS los estilos que NO empiecen con "<"
                    if (!lineStyle.Name.StartsWith("<"))
                    {
                        idsToDelete.Add(lineStyle.Id);
                        Debug.WriteLine($"Marcando para eliminar: {lineStyle.Name}");
                    }
                    else
                    {
                        Debug.WriteLine($"Conservando estilo built-in: {lineStyle.Name}");
                    }
                }

                if (idsToDelete.Count > 0)
                {
                    Debug.WriteLine($"Eliminando {idsToDelete.Count} estilos de línea personalizados");
                    doc.Delete(idsToDelete);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error eliminando Line Styles personalizados: {ex.Message}");
            }
        }

        private void CrearNuevosLineStyles(Document doc)
        {
            try
            {
                // Obtener la categoría principal de líneas
                Category linesCategory = doc.Settings.Categories.get_Item(BuiltInCategory.OST_Lines);

                // Lista de nuevos estilos a crear según tu tabla
                var nuevosEstilos = new List<string>
        {
            "#1 Discontinua", "#1 Solida", "#1 Solida Roja",
            "#2 Discontinua", "#2 Solida", "#2 Solida Roja",
            "#3 Discontinua", "#3 Solida", "#3 Solida Roja"
            
        };

                foreach (string nombreEstilo in nuevosEstilos)
                {
                    // Verificar si el estilo ya existe
                    if (!ExisteLineStyle(linesCategory, nombreEstilo))
                    {
                        // Crear nueva subcategoría (Line Style)
                        Category nuevoEstilo = doc.Settings.Categories.NewSubcategory(linesCategory, nombreEstilo);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error creando nuevos Line Styles: {ex.Message}");
            }
        }

        private bool ExisteLineStyle(Category linesCategory, string nombreEstilo)
        {
            foreach (Category subCat in linesCategory.SubCategories)
            {
                if (subCat.Name == nombreEstilo)
                {
                    return true;
                }
            }
            return false;
        }

        private void ConfigurarPropiedadesLineStyles(Document doc)
        {
            try
            {
                Category linesCategory = doc.Settings.Categories.get_Item(BuiltInCategory.OST_Lines);

                foreach (Category lineStyle in linesCategory.SubCategories)
                {
                    ConfigurarPropiedadesIndividuales(doc, lineStyle);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error configurando propiedades de Line Styles: {ex.Message}");
            }
        }

        private void ConfigurarPropiedadesIndividuales(Document doc, Category lineStyle)
        {
            try
            {
                string nombreEstilo = lineStyle.Name;

                // Configurar según la tabla que proporcionaste
                switch (nombreEstilo)
                {
                    case "#1 Discontinua":
                        ConfigurarEstilo(doc, lineStyle, 1, new Color(0, 0, 0), "Linea Discontinua");
                        break;
                    case "#1 Solida":
                        ConfigurarEstilo(doc, lineStyle, 1, new Color(0, 0, 0), "Solid");
                        break;
                    case "#1 Solida Roja":
                        ConfigurarEstilo(doc, lineStyle, 1, new Color(255, 0, 0), "Solid");
                        break;
                    case "#2 Discontinua":
                        ConfigurarEstilo(doc, lineStyle, 2, new Color(0, 0, 0), "Linea Discontinua");
                        break;
                    case "#2 Solida":
                        ConfigurarEstilo(doc, lineStyle, 2, new Color(0, 0, 0), "Solid");
                        break;
                    case "#2 Solida Roja":
                        ConfigurarEstilo(doc, lineStyle, 2, new Color(255, 0, 0), "Solid");
                        break;
                    case "#3 Discontinua":
                        ConfigurarEstilo(doc, lineStyle, 3, new Color(0, 0, 0), "Linea Discontinua");
                        break;
                    case "#3 Solida":
                        ConfigurarEstilo(doc, lineStyle, 3, new Color(0, 0, 0), "Solid");
                        break;
                    case "#3 Solida Roja":
                        ConfigurarEstilo(doc, lineStyle, 3, new Color(255, 0, 0), "Solid");
                        break;
                    case "<Boceto>":
                    case "<Sketch>":
                    //    ConfigurarEstilo(doc, lineStyle, 2, new Color(255, 0, 255), "Solid");
                    //    break;
                    //case "<Contorno de carga basada en área>":
                    //    ConfigurarEstilo(doc, lineStyle, 1, new Color(0, 0, 0), "Solid");
                    //    break;
                    case "<Contorno de área>":
                    case "<Area Boundary>":
                        ConfigurarEstilo(doc, lineStyle, 2, new Color(128, 0, 255), "Solid");
                        break;
                    //case "<Derribado>":
                    //case "<Demolished>":
                    //    ConfigurarEstilo(doc, lineStyle, 1, new Color(0, 0, 0), "Solid");
                    //    break;
                    //case "<Eje de rotación>":
                    //case "<Axis of Rotation>":
                    //    ConfigurarEstilo(doc, lineStyle, 2, new Color(0, 0, 255), "Solid");
                    //    break;
                    case "<Eje>":
                    case "<Centerline>":
                        ConfigurarEstilo(doc, lineStyle, 1, new Color(0, 0, 0), "Solid");
                        break;
                    //case "<Elevado>":
                    //case "<Overhead>":
                    //    ConfigurarEstilo(doc, lineStyle, 3, new Color(0, 0, 0), "Solid");
                    //    break;
                    //case "<Envolvente de mallazo>":
                    //case "<Fabric Envelope>":
                    //    ConfigurarEstilo(doc, lineStyle, 1, new Color(127, 127, 127), "Solid");
                    //    break;
                    case "<Líneas anchas>":
                    case "<Wide Lines>":
                        ConfigurarEstilo(doc, lineStyle, 1, new Color(0, 0, 0), "Solid");
                        break;
                    //case "<Líneas de aislamiento>":
                    //case "<Insulation Batting Lines>":
                    //    ConfigurarEstilo(doc, lineStyle, 1, new Color(0, 255, 0), "Solid");
                    //    break;
                    //case "<Líneas de camino del recorrido>":
                    //case "<Path of Travel Lines>":
                    //    ConfigurarEstilo(doc, lineStyle, 1, new Color(0, 166, 0), "Solid");
                    //    break;
                    case "<Líneas finas>":
                    case "<Thin Lines>":
                        ConfigurarEstilo(doc, lineStyle, 1, new Color(0, 0, 0), "Solid");
                        break;
                    case "<Líneas medias>":
                    case "<Medium Lines>":
                        ConfigurarEstilo(doc, lineStyle, 2, new Color(0, 0, 0), "Solid");
                        break;
                    case "<Líneas ocultas>":
                    case "<Hidden Lines>":
                        ConfigurarEstilo(doc, lineStyle, 2, new Color(0, 166, 0), "Solid");
                        break;
                    //case "<Más allá>":
                    //case "<Beyond>":
                    //    ConfigurarEstilo(doc, lineStyle, 1, new Color(0, 166, 0), "Solid");
                    //    break;
                    //case "<Oculto>":
                    //case "<Hidden>":
                    //    ConfigurarEstilo(doc, lineStyle, 1, new Color(0, 0, 0), "Solid");
                    //    break;
                    //case "<Separación de espacios>":
                    //case "<Space Separation>":
                    //    ConfigurarEstilo(doc, lineStyle, 1, new Color(0, 255, 0), "Solid");
                    //    break;
                    //case "<Separación de habitación>":
                    //case "<Room Separation>":
                    //    ConfigurarEstilo(doc, lineStyle, 3, new Color(0, 255, 255), "Solid");
                    //    break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error configurando estilo individual {lineStyle.Name}: {ex.Message}");
            }
        }

        private void ConfigurarEstilo(Document doc, Category lineStyle, int grosor, Color color, string linePatternName)
        {
            try
            {
                // Configurar grosor
                lineStyle.SetLineWeight(grosor, GraphicsStyleType.Projection);

                // Configurar color
                lineStyle.LineColor = color;

                // Configurar line pattern si no es "Solid"
                if (linePatternName != "Solid")
                {
                    AsignarLinePatternALineStyle(doc, lineStyle, linePatternName);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error configurando estilo {lineStyle.Name}: {ex.Message}");
            }
        }

        private void AsignarLinePatternALineStyle(Document doc, Category lineStyle, string linePatternName)
        {
            try
            {
                // Buscar el line pattern por nombre
                FilteredElementCollector collector = new FilteredElementCollector(doc)
                    .OfClass(typeof(LinePatternElement));

                foreach (LinePatternElement pattern in collector)
                {
                    if (pattern.Name == linePatternName)
                    {
                        lineStyle.SetLinePatternId(pattern.Id, GraphicsStyleType.Projection);
                        return;
                    }
                }

                Debug.WriteLine($"Advertencia: No se encontró el line pattern '{linePatternName}' para el estilo '{lineStyle.Name}'");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error asignando line pattern '{linePatternName}' a estilo '{lineStyle.Name}': {ex.Message}");
            }
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
