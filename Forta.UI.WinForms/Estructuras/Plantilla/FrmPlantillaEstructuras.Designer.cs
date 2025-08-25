namespace Forta.UI.WinForms
{
    partial class FrmPlantillaEstructuras
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnl_PlantillaEstructura = new System.Windows.Forms.Panel();
            this.pbx_minimizar = new System.Windows.Forms.PictureBox();
            this.pbx_maximizar = new System.Windows.Forms.PictureBox();
            this.pbx_cerrar = new System.Windows.Forms.PictureBox();
            this.btn_textosCotas = new System.Windows.Forms.Button();
            this.btn_iniciaFamilias = new System.Windows.Forms.Button();
            this.btn_estilosLinea = new System.Windows.Forms.Button();
            this.btn_adquirirPropiedades = new System.Windows.Forms.Button();
            this.pnl_PlantillaEstructura.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_minimizar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_maximizar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_cerrar)).BeginInit();
            this.SuspendLayout();
            // 
            // pnl_PlantillaEstructura
            // 
            this.pnl_PlantillaEstructura.BackColor = System.Drawing.Color.Gray;
            this.pnl_PlantillaEstructura.Controls.Add(this.pbx_minimizar);
            this.pnl_PlantillaEstructura.Controls.Add(this.pbx_maximizar);
            this.pnl_PlantillaEstructura.Controls.Add(this.pbx_cerrar);
            this.pnl_PlantillaEstructura.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnl_PlantillaEstructura.Location = new System.Drawing.Point(0, 0);
            this.pnl_PlantillaEstructura.Name = "pnl_PlantillaEstructura";
            this.pnl_PlantillaEstructura.Size = new System.Drawing.Size(822, 77);
            this.pnl_PlantillaEstructura.TabIndex = 0;
            this.pnl_PlantillaEstructura.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnl_PlantillaEstructura_MouseDown);
            this.pnl_PlantillaEstructura.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnl_PLantillaEstructuras_MouseMove);
            this.pnl_PlantillaEstructura.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnl_PlantillaEstructura_MouseUp);
            // 
            // pbx_minimizar
            // 
            this.pbx_minimizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbx_minimizar.Image = global::Forta.UI.WinForms.Properties.Resources.Minimizar60x60;
            this.pbx_minimizar.Location = new System.Drawing.Point(630, 7);
            this.pbx_minimizar.Name = "pbx_minimizar";
            this.pbx_minimizar.Size = new System.Drawing.Size(61, 64);
            this.pbx_minimizar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbx_minimizar.TabIndex = 2;
            this.pbx_minimizar.TabStop = false;
            this.pbx_minimizar.Click += new System.EventHandler(this.pbx_minimizar_Click);
            // 
            // pbx_maximizar
            // 
            this.pbx_maximizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbx_maximizar.Image = global::Forta.UI.WinForms.Properties.Resources.Maximizar60x60;
            this.pbx_maximizar.Location = new System.Drawing.Point(697, 7);
            this.pbx_maximizar.Name = "pbx_maximizar";
            this.pbx_maximizar.Size = new System.Drawing.Size(61, 64);
            this.pbx_maximizar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbx_maximizar.TabIndex = 1;
            this.pbx_maximizar.TabStop = false;
            this.pbx_maximizar.Click += new System.EventHandler(this.pbx_maximizar_Click);
            // 
            // pbx_cerrar
            // 
            this.pbx_cerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbx_cerrar.Image = global::Forta.UI.WinForms.Properties.Resources.Cerrar60x60;
            this.pbx_cerrar.Location = new System.Drawing.Point(764, 7);
            this.pbx_cerrar.Name = "pbx_cerrar";
            this.pbx_cerrar.Size = new System.Drawing.Size(55, 64);
            this.pbx_cerrar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbx_cerrar.TabIndex = 1;
            this.pbx_cerrar.TabStop = false;
            this.pbx_cerrar.Click += new System.EventHandler(this.pbx_cerrar_Click);
            // 
            // btn_textosCotas
            // 
            this.btn_textosCotas.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.btn_textosCotas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_textosCotas.Image = global::Forta.UI.WinForms.Properties.Resources.EstilosTextoCotas185x85;
            this.btn_textosCotas.Location = new System.Drawing.Point(305, 217);
            this.btn_textosCotas.Name = "btn_textosCotas";
            this.btn_textosCotas.Size = new System.Drawing.Size(185, 85);
            this.btn_textosCotas.TabIndex = 9;
            this.btn_textosCotas.UseVisualStyleBackColor = false;
            this.btn_textosCotas.Click += new System.EventHandler(this.btn_textosCotas_Click);
            // 
            // btn_iniciaFamilias
            // 
            this.btn_iniciaFamilias.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.btn_iniciaFamilias.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_iniciaFamilias.Image = global::Forta.UI.WinForms.Properties.Resources.InicializaFamilias185x85;
            this.btn_iniciaFamilias.Location = new System.Drawing.Point(43, 335);
            this.btn_iniciaFamilias.Name = "btn_iniciaFamilias";
            this.btn_iniciaFamilias.Size = new System.Drawing.Size(185, 85);
            this.btn_iniciaFamilias.TabIndex = 8;
            this.btn_iniciaFamilias.UseVisualStyleBackColor = false;
            this.btn_iniciaFamilias.Click += new System.EventHandler(this.btn_iniciaFamilias_Click);
            // 
            // btn_estilosLinea
            // 
            this.btn_estilosLinea.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.btn_estilosLinea.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_estilosLinea.Image = global::Forta.UI.WinForms.Properties.Resources.EstilosLinea185x851;
            this.btn_estilosLinea.Location = new System.Drawing.Point(43, 217);
            this.btn_estilosLinea.Name = "btn_estilosLinea";
            this.btn_estilosLinea.Size = new System.Drawing.Size(185, 85);
            this.btn_estilosLinea.TabIndex = 7;
            this.btn_estilosLinea.UseVisualStyleBackColor = false;
            this.btn_estilosLinea.Click += new System.EventHandler(this.btn_estilosLinea_Click_1);
            // 
            // btn_adquirirPropiedades
            // 
            this.btn_adquirirPropiedades.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.btn_adquirirPropiedades.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_adquirirPropiedades.Image = global::Forta.UI.WinForms.Properties.Resources.AdquirirPropiedades185x85;
            this.btn_adquirirPropiedades.Location = new System.Drawing.Point(305, 95);
            this.btn_adquirirPropiedades.Name = "btn_adquirirPropiedades";
            this.btn_adquirirPropiedades.Size = new System.Drawing.Size(185, 85);
            this.btn_adquirirPropiedades.TabIndex = 6;
            this.btn_adquirirPropiedades.UseVisualStyleBackColor = false;
            // 
            // FrmPlantillaEstructuras
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(822, 577);
            this.Controls.Add(this.btn_textosCotas);
            this.Controls.Add(this.btn_iniciaFamilias);
            this.Controls.Add(this.btn_estilosLinea);
            this.Controls.Add(this.btn_adquirirPropiedades);
            this.Controls.Add(this.pnl_PlantillaEstructura);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmPlantillaEstructuras";
            this.Text = "FrmPlantillaEstructuras";
            this.pnl_PlantillaEstructura.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbx_minimizar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_maximizar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_cerrar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnl_PlantillaEstructura;
        private System.Windows.Forms.PictureBox pbx_cerrar;
        private System.Windows.Forms.PictureBox pbx_maximizar;
        private System.Windows.Forms.PictureBox pbx_minimizar;
        private System.Windows.Forms.Button btn_adquirirPropiedades;
        private System.Windows.Forms.Button btn_estilosLinea;
        private System.Windows.Forms.Button btn_iniciaFamilias;
        private System.Windows.Forms.Button btn_textosCotas;
    }
}