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

        private void pbx_cerrar_Click(object sender, EventArgs e)
        {
            this.Close(); // Cierra solo la ventana del plugin
        }

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

        private void pbx_minimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized; // Minimiza la ventana
        }

        private void pnl_PlantillaEstructura_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
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

        private void FrmPlantillaEstructuras_Load(object sender, EventArgs e)
        {

        }

        private void btn_grosoresLinea_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show(
                "¿Seguro que deseas adquirir todas las propiedades de grosores de líneas?...",
                "Confirmar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                // Indicar al comando que ejecute la lógica
                this.DialogResult = DialogResult.OK;
                this.Tag = "GrosoresLinea"; // Identificar qué acción
                this.Close();
            }
        }

    }
}
