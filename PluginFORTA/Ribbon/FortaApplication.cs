#region Namespaces
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Forta.Core;
using Forta.Core.Utils;
using Forta.Estructuras.Commands;
using Forta.HVAC.Commands;
using Forta.IE.Commands;
using Forta.IESP.Commands;
using Forta.IHS.Commands;


#endregion

namespace Forta.App.Ribbon
{
    public class FortaApplication : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication app)
        {
        

            //CREACIÓN DE LA NUEVA PESTAÑA
            app.CreateRibbonTab("FORTA");

            //CREACIÓN DE PÁNELES DENTRO DE LA PESTAÑA (ESTRUCTURA,HVAC,IHS,IE,IESP)
            RibbonPanel panelEstructura = app.CreateRibbonPanel("FORTA", "ESTRUCTURAS");
            RibbonPanel panelHVAC = app.CreateRibbonPanel("FORTA", "HVAC");
            RibbonPanel panelIHS = app.CreateRibbonPanel("FORTA", "HIDROSANITARIAS");
            RibbonPanel panelIE = app.CreateRibbonPanel("FORTA", "ELÉCTRICO");
            RibbonPanel panelIESP = app.CreateRibbonPanel("FORTA", "ESPECIALES");

            #region PUSHBUTTON PLANTILLA DE ESTRUCTURA
            //PUSHBUTTON ESTRUCTURA
            var datosPlaEstPushButton = new PushButtonData(
                "PlantillaEstructuralBtn",
                "Plantilla",
                typeof(PlantillaEstructural).Assembly.Location,
                "Forta.Estructuras.Commands.PlantillaEstructural");
            //COLOCANDO PUSHBUTTON DENTRO DEL PANEL
            PushButton plaEstPushButton = panelEstructura.AddItem(datosPlaEstPushButton) as PushButton;
            //COLOCANDO LAS PROPIEDADES
            plaEstPushButton.ToolTip = "Adquiere la plantilla de estructuras con las principales propiedades";
            plaEstPushButton.LongDescription = "Adquiere: Espesores de línea, Inicialización de familias principales, Inicialización de parámetros";


            // CARGAR IMÁGENES DESDE EL CORE
            var asmEstructuras = typeof(PlantillaEstructural).Assembly;
            plaEstPushButton.ToolTipImage = ImageLoader.FromResource(
                asmEstructuras,
                "Forta.Estructuras.Resources.EstPlantillaToolTip355x355.png"   // nombre exacto del recurso
            );
            plaEstPushButton.LargeImage = ImageLoader.FromResource(
                asmEstructuras,
                "Forta.Estructuras.Resources.EstructuraPlantilla32x32.png"
            );
            #endregion

            #region PUSHBUTTON PLANTILLA DE HVAC
            //PUSHBUTTON HVAC
            var datosPlaHVAC = new PushButtonData(
                "PlantillaHVACBtn",
                "Plantilla",
                typeof(FortaApplication).Assembly.Location,
                "Forta.HVAC.Commands.PlantillaHVAC"
                );

            var btnHVAC = panelHVAC.AddItem(datosPlaHVAC) as PushButton;
            btnHVAC.ToolTip = "Adquiere la plantilla de HVAC con las principales propiedades";
            btnHVAC.LongDescription = "Adquiere: Espesores de línea, Inicialización de familias , Inicialización de parámetros";
            //Cargar ícono desde el assembly de Forta.HVAC (recurso incrustado)
            var asmHVAC = typeof(PlantillaHVAC).Assembly;
            btnHVAC.ToolTipImage = ImageLoader.FromResource(asmHVAC, "Forta.HVAC.Resources.HVACToolTip355x355.png");
            btnHVAC.LargeImage = ImageLoader.FromResource(asmHVAC, "Forta.HVAC.Resources.HVACPlantilla 32x32.png");


            #endregion

            #region PUSHBUTTON PLANTILLA DE IHS
            //PUSHBUTTON IHS
            PushButtonData datosPlaIHS = new PushButtonData(
                "PlantillaIHSbtn",
                "Plantilla",
                typeof(FortaApplication).Assembly.Location,
                "Forta.IHS.Commands.PlantillaIHS");

            var btnIHS = panelIHS.AddItem(datosPlaIHS) as PushButton;
            btnIHS.ToolTip = "Adquiere la plantilla de IHS con las principales propiedades";
            btnIHS.LongDescription = "Adquiere: Espesores de línea, Inicialización de familias, Inicialización de parámetros";
            //Cargar ícono desde el assembly de Forta.IHS (recurso incrustado)
            var asmIHS = typeof(PlantillaIHS).Assembly;
            btnIHS.ToolTipImage = ImageLoader.FromResource(asmIHS, "Forta.IHS.Resources.IHSToolTip355x355.png");
            btnIHS.LargeImage = ImageLoader.FromResource(asmIHS, "Forta.IHS.Resources.IHSPlantilla32x32.png");
            
            #endregion

            #region PUSHBUTTON PLANTILLA DE ELECTRICO
            //PUSHBUTTON INSTALACIONES ELECTRICAS
            var datosPlaIE = new PushButtonData(
                "PlantillaElecbtn",
                "Plantilla",
                typeof(FortaApplication).Assembly.Location,
                "Forta.IE.Commands.PlantillaIE");

            var btnIE = panelIE.AddItem(datosPlaIE) as PushButton;
            btnIE.ToolTip = "Adquiere la plantilla de IE con las principales propiedades";
            btnIE.LongDescription = "Adquiere: Espesores de línea, Inicialización de familias , Inicialización de parámetros";

            //Cargar ícono desde el assembly de Forta.HVAC (recurso incrustado)
            var asmIE = typeof(PlantillaIE).Assembly;
            btnIE.ToolTipImage = ImageLoader.FromResource(asmIE,"Forta.IE.Resources.IEToolTip355x355.png");
            btnIE.LargeImage = ImageLoader.FromResource(asmIE,"Forta.IE.Resources.IEPlantilla32x32.png");
            #endregion

            #region PUSHBUTTON PLANTILLA DE IESPECIALES
            //PUSHBUTTON INSTALACIONES ESPECIALES
            var datosPlaIESP = new PushButtonData(
                "PlantillaEspebtn",
                "Plantilla",
                typeof(FortaApplication).Assembly.Location,
                "Forta.IESP.Commands.PlantillaIESP");

            var btnIESP = panelIESP.AddItem(datosPlaIESP) as PushButton;
            btnIESP.ToolTip = "Adquiere la plantilla de IESP con las principales propiedades";
            btnIESP.LongDescription = "Adquiere: Espesores de línea, Inicialización de familias , Inicialización de parámetros";

            //Cargar ícono desde el assembly de Forta.IESP (recurso incrustado)
            var asmIESP = typeof(PlantillaIESP).Assembly;
            btnIESP.ToolTipImage = ImageLoader.FromResource(asmIESP,"Forta.IESP.Resources.IESPToolTip355x355.png");
            btnIESP.LargeImage = ImageLoader.FromResource(asmIESP,"Forta.IESP.Resources.IESPPlantilla32x32.png");


            #endregion

            return Result.Succeeded;

        }


        #region
        private ImageSource ConvertImage(System.Drawing.Image image)
        {
            using (var ms = new System.IO.MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ms.Position = 0;
                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.CacheOption = BitmapCacheOption.OnLoad; // permite cerrar el stream
                bmp.StreamSource = ms;
                bmp.EndInit();
                bmp.Freeze(); // thread-safe y no requiere stream abierto
                return bmp;
            }

        }

        #endregion

        
        public Result OnShutdown(UIControlledApplication app)
        {
            return Result.Succeeded;
        }
    }
}
