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
        public FrmPlantillaEstructuras()
        {
            InitializeComponent();
        }

        private void FrmPlantillaEstructuras_Load(object sender, EventArgs e)
        {
           
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
    }
}
