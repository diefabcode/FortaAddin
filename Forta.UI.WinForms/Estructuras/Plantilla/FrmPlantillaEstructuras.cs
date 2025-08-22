using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using Forta.Core.Utils; // Usar Utils, no Utilidades

namespace Forta.UI.WinForms
{
    public partial class FrmPlantillaEstructuras : Form
    {

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        public FrmPlantillaEstructuras()
        {
            InitializeComponent();
        }


        #region BOTONES SUPERIORES DE VENTANA Y PANEL

        //BOTON QUE CIERRA LA VENTANA DEL PLUGIN
        private void pbx_cerrar_Click(object sender, EventArgs e)
        {
            this.Close(); // Cierra solo la ventana del plugin (no cierra REVIT)
        }

        //BOTON QUE MAXIMIZA LA VENTANA DEL PLUGIN
        private void pbx_maximizar_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal; // Restaura si ya está maximizado
            }
            else
            {
                this.WindowState = FormWindowState.Maximized; // Maximiza la ventana
            }
        }

        //BOTON QUE MINIMIZA LA VENTANA DEL PLUGIN
        private void pbx_minimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized; // Minimiza la ventana
        }


        #region METODOS PARA PODER ARRASTRAR VENTANA DESDE EL PANEL

        //Se ejecuta cuando haces clic y mantienes presionado el mouse sobre el panel
        private void pnl_PlantillaEstructura_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true; //indico que estoy arrastrando
            dragCursorPoint = Cursor.Position; //guarda la posicion actual del cursor
            dragFormPoint = this.Location; //guarda la posicion actual de la ventana (formulario)
        }

        private void pnl_PLantillaEstructuras_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));

            }
        }

        private void pnl_PlantillaEstructura_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }
        #endregion

        #endregion

        #region EVENTO DE BOTON DE ESTILOS DE LINEA
        private void btn_estilosLinea_Click_1(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show(
        "¿Seguro que deseas aplicar los estilos de línea estándar? Esto eliminará todos los line patterns existentes y creará nuevos.",
        "Estilos de Línea",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.OK;
                this.Tag = "EstilosLinea";
                this.Close();
            }
        }
        #endregion

        #region EVENTO DE BOTON INICIALIZA FAMILIAS
        private void btn_iniciaFamilias_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show(
        "¿Estás seguro de que quieres inicializar tus familias?\n\n" +
        "Las familias que no estén en uso (modeladas) serán eliminadas.",
        "Inicializar Familias",
        MessageBoxButtons.OKCancel,
        MessageBoxIcon.Question);

            if (resultado == DialogResult.OK)
            {
                this.DialogResult = DialogResult.OK;
                this.Tag = "InicializarFamilias";
                this.Close();
            }
        }



        #endregion

        #region EVENTO DE BOTON ESTILOS DE COTA Y TEXTO
        private void btn_textosCotas_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show(
                "Se crearán nuevos estilod de texto y cotas", "Estilos de Texto y Cotas",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if ( resultado == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.OK;
                this.Tag = "EstilosTextoCotas";
                this.Close();
            }

        }
        #endregion
    }
}

