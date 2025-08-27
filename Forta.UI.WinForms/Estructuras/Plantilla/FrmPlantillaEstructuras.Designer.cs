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
            this.chbx_depurarCotas = new System.Windows.Forms.CheckBox();
            this.btn_parametros = new System.Windows.Forms.Button();
            this.btn_subproyectos = new System.Windows.Forms.Button();
            this.btn_materiales = new System.Windows.Forms.Button();
            this.btn_cotas = new System.Windows.Forms.Button();
            this.btn_textos = new System.Windows.Forms.Button();
            this.btn_iniciaFamilias = new System.Windows.Forms.Button();
            this.btn_estilosLinea = new System.Windows.Forms.Button();
            this.btn_adquirirPropiedades = new System.Windows.Forms.Button();
            this.chbx_depurarTextos = new System.Windows.Forms.CheckBox();
            this.chbx_depurarLineas = new System.Windows.Forms.CheckBox();
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
            // chbx_depurarCotas
            // 
            this.chbx_depurarCotas.Appearance = System.Windows.Forms.Appearance.Button;
            this.chbx_depurarCotas.AutoSize = true;
            this.chbx_depurarCotas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chbx_depurarCotas.Image = global::Forta.UI.WinForms.Properties.Resources.Depurar85x26;
            this.chbx_depurarCotas.Location = new System.Drawing.Point(667, 184);
            this.chbx_depurarCotas.Name = "chbx_depurarCotas";
            this.chbx_depurarCotas.Size = new System.Drawing.Size(91, 32);
            this.chbx_depurarCotas.TabIndex = 15;
            this.chbx_depurarCotas.UseVisualStyleBackColor = false;
            this.chbx_depurarCotas.CheckedChanged += new System.EventHandler(this.chbx_depurarCotas_CheckedChanged);
            // 
            // btn_parametros
            // 
            this.btn_parametros.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.btn_parametros.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_parametros.Image = global::Forta.UI.WinForms.Properties.Resources.Parametros185x85;
            this.btn_parametros.Location = new System.Drawing.Point(43, 467);
            this.btn_parametros.Name = "btn_parametros";
            this.btn_parametros.Size = new System.Drawing.Size(185, 85);
            this.btn_parametros.TabIndex = 14;
            this.btn_parametros.UseVisualStyleBackColor = false;
            this.btn_parametros.Click += new System.EventHandler(this.btn_parametros_Click);
            // 
            // btn_subproyectos
            // 
            this.btn_subproyectos.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.btn_subproyectos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_subproyectos.Image = global::Forta.UI.WinForms.Properties.Resources.Subproyectos185x85;
            this.btn_subproyectos.Location = new System.Drawing.Point(573, 343);
            this.btn_subproyectos.Name = "btn_subproyectos";
            this.btn_subproyectos.Size = new System.Drawing.Size(185, 85);
            this.btn_subproyectos.TabIndex = 13;
            this.btn_subproyectos.UseVisualStyleBackColor = false;
            this.btn_subproyectos.Click += new System.EventHandler(this.btn_subproyectos_Click);
            // 
            // btn_materiales
            // 
            this.btn_materiales.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.btn_materiales.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_materiales.Image = global::Forta.UI.WinForms.Properties.Resources.Materiales185x85;
            this.btn_materiales.Location = new System.Drawing.Point(305, 343);
            this.btn_materiales.Name = "btn_materiales";
            this.btn_materiales.Size = new System.Drawing.Size(185, 85);
            this.btn_materiales.TabIndex = 12;
            this.btn_materiales.UseVisualStyleBackColor = false;
            this.btn_materiales.Click += new System.EventHandler(this.btn_materiales_Click);
            // 
            // btn_cotas
            // 
            this.btn_cotas.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.btn_cotas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_cotas.Image = global::Forta.UI.WinForms.Properties.Resources.EstilosCotas185x55;
            this.btn_cotas.Location = new System.Drawing.Point(573, 217);
            this.btn_cotas.Name = "btn_cotas";
            this.btn_cotas.Size = new System.Drawing.Size(185, 85);
            this.btn_cotas.TabIndex = 11;
            this.btn_cotas.UseVisualStyleBackColor = false;
            this.btn_cotas.Click += new System.EventHandler(this.btn_cotas_Click);
            // 
            // btn_textos
            // 
            this.btn_textos.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.btn_textos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_textos.Image = global::Forta.UI.WinForms.Properties.Resources.EstilosTextos185x85;
            this.btn_textos.Location = new System.Drawing.Point(305, 217);
            this.btn_textos.Name = "btn_textos";
            this.btn_textos.Size = new System.Drawing.Size(185, 85);
            this.btn_textos.TabIndex = 10;
            this.btn_textos.UseVisualStyleBackColor = false;
            this.btn_textos.Click += new System.EventHandler(this.btn_textos_Click);
            // 
            // btn_iniciaFamilias
            // 
            this.btn_iniciaFamilias.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.btn_iniciaFamilias.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_iniciaFamilias.Image = global::Forta.UI.WinForms.Properties.Resources.InicializaFamilias185x85;
            this.btn_iniciaFamilias.Location = new System.Drawing.Point(43, 343);
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
            this.btn_adquirirPropiedades.Location = new System.Drawing.Point(305, 86);
            this.btn_adquirirPropiedades.Name = "btn_adquirirPropiedades";
            this.btn_adquirirPropiedades.Size = new System.Drawing.Size(185, 85);
            this.btn_adquirirPropiedades.TabIndex = 6;
            this.btn_adquirirPropiedades.UseVisualStyleBackColor = false;
            // 
            // chbx_depurarTextos
            // 
            this.chbx_depurarTextos.Appearance = System.Windows.Forms.Appearance.Button;
            this.chbx_depurarTextos.AutoSize = true;
            this.chbx_depurarTextos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chbx_depurarTextos.Image = global::Forta.UI.WinForms.Properties.Resources.Depurar85x26;
            this.chbx_depurarTextos.Location = new System.Drawing.Point(399, 184);
            this.chbx_depurarTextos.Name = "chbx_depurarTextos";
            this.chbx_depurarTextos.Size = new System.Drawing.Size(91, 32);
            this.chbx_depurarTextos.TabIndex = 16;
            this.chbx_depurarTextos.UseVisualStyleBackColor = false;
            this.chbx_depurarTextos.CheckedChanged += new System.EventHandler(this.chbx_depurarTextos_CheckedChanged);
            // 
            // chbx_depurarLineas
            // 
            this.chbx_depurarLineas.Appearance = System.Windows.Forms.Appearance.Button;
            this.chbx_depurarLineas.AutoSize = true;
            this.chbx_depurarLineas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chbx_depurarLineas.Image = global::Forta.UI.WinForms.Properties.Resources.Depurar85x26;
            this.chbx_depurarLineas.Location = new System.Drawing.Point(137, 184);
            this.chbx_depurarLineas.Name = "chbx_depurarLineas";
            this.chbx_depurarLineas.Size = new System.Drawing.Size(91, 32);
            this.chbx_depurarLineas.TabIndex = 17;
            this.chbx_depurarLineas.UseVisualStyleBackColor = true;
            this.chbx_depurarLineas.CheckedChanged += new System.EventHandler(this.chbx_depurarLineas_CheckedChanged);
            // 
            // FrmPlantillaEstructuras
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(822, 577);
            this.Controls.Add(this.chbx_depurarLineas);
            this.Controls.Add(this.chbx_depurarTextos);
            this.Controls.Add(this.chbx_depurarCotas);
            this.Controls.Add(this.btn_parametros);
            this.Controls.Add(this.btn_subproyectos);
            this.Controls.Add(this.btn_materiales);
            this.Controls.Add(this.btn_cotas);
            this.Controls.Add(this.btn_textos);
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
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnl_PlantillaEstructura;
        private System.Windows.Forms.PictureBox pbx_cerrar;
        private System.Windows.Forms.PictureBox pbx_maximizar;
        private System.Windows.Forms.PictureBox pbx_minimizar;
        private System.Windows.Forms.Button btn_adquirirPropiedades;
        private System.Windows.Forms.Button btn_estilosLinea;
        private System.Windows.Forms.Button btn_iniciaFamilias;
        private System.Windows.Forms.Button btn_textos;
        private System.Windows.Forms.Button btn_cotas;
        private System.Windows.Forms.Button btn_materiales;
        private System.Windows.Forms.Button btn_subproyectos;
        private System.Windows.Forms.Button btn_parametros;
        private System.Windows.Forms.CheckBox chbx_depurarCotas;
        private System.Windows.Forms.CheckBox chbx_depurarTextos;
        private System.Windows.Forms.CheckBox chbx_depurarLineas;
    }
}