﻿namespace T3
{
    partial class Frm_Inf_NC_Aplic_Caja
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_Inf_NC_Aplic_Caja));
            this.panel1 = new System.Windows.Forms.Panel();
            this._Chk_PorAplicar = new System.Windows.Forms.CheckBox();
            this._Bt_Limpiar = new System.Windows.Forms.Button();
            this._Bt_Caja_2 = new System.Windows.Forms.Button();
            this._Txt_Caja_2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this._Bt_Caja = new System.Windows.Forms.Button();
            this._Txt_Caja = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this._Bt_Consultar = new System.Windows.Forms.Button();
            this._Rpv_Main = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this._Er_Error = new System.Windows.Forms.ErrorProvider(this.components);
            this._Bt_Exportar = new System.Windows.Forms.Button();
            this._Sfd_1 = new System.Windows.Forms.SaveFileDialog();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._Er_Error)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._Bt_Exportar);
            this.panel1.Controls.Add(this._Chk_PorAplicar);
            this.panel1.Controls.Add(this._Bt_Limpiar);
            this.panel1.Controls.Add(this._Bt_Caja_2);
            this.panel1.Controls.Add(this._Txt_Caja_2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this._Bt_Caja);
            this.panel1.Controls.Add(this._Txt_Caja);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this._Bt_Consultar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(773, 61);
            this.panel1.TabIndex = 2;
            // 
            // _Chk_PorAplicar
            // 
            this._Chk_PorAplicar.AutoSize = true;
            this._Chk_PorAplicar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._Chk_PorAplicar.Location = new System.Drawing.Point(12, 38);
            this._Chk_PorAplicar.Name = "_Chk_PorAplicar";
            this._Chk_PorAplicar.Size = new System.Drawing.Size(153, 17);
            this._Chk_PorAplicar.TabIndex = 120;
            this._Chk_PorAplicar.Text = "NC por aplicar en caja";
            this._Chk_PorAplicar.UseVisualStyleBackColor = true;
            this._Chk_PorAplicar.CheckedChanged += new System.EventHandler(this._Chk_PorAplicar_CheckedChanged);
            // 
            // _Bt_Limpiar
            // 
            this._Bt_Limpiar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._Bt_Limpiar.Image = ((System.Drawing.Image)(resources.GetObject("_Bt_Limpiar.Image")));
            this._Bt_Limpiar.Location = new System.Drawing.Point(481, 12);
            this._Bt_Limpiar.Name = "_Bt_Limpiar";
            this._Bt_Limpiar.Size = new System.Drawing.Size(25, 18);
            this._Bt_Limpiar.TabIndex = 119;
            this._Bt_Limpiar.UseVisualStyleBackColor = true;
            this._Bt_Limpiar.Click += new System.EventHandler(this._Bt_Limpiar_Click);
            // 
            // _Bt_Caja_2
            // 
            this._Bt_Caja_2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._Bt_Caja_2.Image = ((System.Drawing.Image)(resources.GetObject("_Bt_Caja_2.Image")));
            this._Bt_Caja_2.Location = new System.Drawing.Point(449, 12);
            this._Bt_Caja_2.Name = "_Bt_Caja_2";
            this._Bt_Caja_2.Size = new System.Drawing.Size(26, 18);
            this._Bt_Caja_2.TabIndex = 69;
            this._Bt_Caja_2.Text = "...";
            this._Bt_Caja_2.UseVisualStyleBackColor = true;
            this._Bt_Caja_2.Click += new System.EventHandler(this._Bt_Caja_2_Click);
            // 
            // _Txt_Caja_2
            // 
            this._Txt_Caja_2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this._Txt_Caja_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._Txt_Caja_2.Enabled = false;
            this._Txt_Caja_2.Location = new System.Drawing.Point(321, 12);
            this._Txt_Caja_2.Name = "_Txt_Caja_2";
            this._Txt_Caja_2.ReadOnly = true;
            this._Txt_Caja_2.Size = new System.Drawing.Size(122, 20);
            this._Txt_Caja_2.TabIndex = 68;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(250, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 67;
            this.label1.Text = "Caja Hasta:";
            // 
            // _Bt_Caja
            // 
            this._Bt_Caja.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._Bt_Caja.Image = ((System.Drawing.Image)(resources.GetObject("_Bt_Caja.Image")));
            this._Bt_Caja.Location = new System.Drawing.Point(208, 12);
            this._Bt_Caja.Name = "_Bt_Caja";
            this._Bt_Caja.Size = new System.Drawing.Size(26, 18);
            this._Bt_Caja.TabIndex = 66;
            this._Bt_Caja.Text = "...";
            this._Bt_Caja.UseVisualStyleBackColor = true;
            this._Bt_Caja.Click += new System.EventHandler(this._Bt_Caja_Click);
            // 
            // _Txt_Caja
            // 
            this._Txt_Caja.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this._Txt_Caja.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._Txt_Caja.Enabled = false;
            this._Txt_Caja.Location = new System.Drawing.Point(80, 12);
            this._Txt_Caja.Name = "_Txt_Caja";
            this._Txt_Caja.ReadOnly = true;
            this._Txt_Caja.Size = new System.Drawing.Size(122, 20);
            this._Txt_Caja.TabIndex = 65;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(7, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 12);
            this.label5.TabIndex = 64;
            this.label5.Text = "Caja Desde:";
            // 
            // _Bt_Consultar
            // 
            this._Bt_Consultar.Cursor = System.Windows.Forms.Cursors.Hand;
            this._Bt_Consultar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Khaki;
            this._Bt_Consultar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._Bt_Consultar.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._Bt_Consultar.Image = ((System.Drawing.Image)(resources.GetObject("_Bt_Consultar.Image")));
            this._Bt_Consultar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._Bt_Consultar.Location = new System.Drawing.Point(526, 5);
            this._Bt_Consultar.Name = "_Bt_Consultar";
            this._Bt_Consultar.Size = new System.Drawing.Size(117, 33);
            this._Bt_Consultar.TabIndex = 63;
            this._Bt_Consultar.Text = "Consultar";
            this._Bt_Consultar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._Bt_Consultar.UseVisualStyleBackColor = true;
            this._Bt_Consultar.Click += new System.EventHandler(this._Bt_Consultar_Click);
            // 
            // _Rpv_Main
            // 
            this._Rpv_Main.ActiveViewIndex = -1;
            this._Rpv_Main.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._Rpv_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this._Rpv_Main.Location = new System.Drawing.Point(0, 61);
            this._Rpv_Main.Name = "_Rpv_Main";
            this._Rpv_Main.SelectionFormula = "";
            this._Rpv_Main.ShowParameterPanelButton = false;
            this._Rpv_Main.Size = new System.Drawing.Size(773, 390);
            this._Rpv_Main.TabIndex = 21;
            this._Rpv_Main.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            this._Rpv_Main.ViewTimeSelectionFormula = "";
            // 
            // _Er_Error
            // 
            this._Er_Error.ContainerControl = this;
            // 
            // _Bt_Exportar
            // 
            this._Bt_Exportar.Cursor = System.Windows.Forms.Cursors.Hand;
            this._Bt_Exportar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._Bt_Exportar.Image = global::T3.Properties.Resources.excel1;
            this._Bt_Exportar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._Bt_Exportar.Location = new System.Drawing.Point(649, 5);
            this._Bt_Exportar.Name = "_Bt_Exportar";
            this._Bt_Exportar.Size = new System.Drawing.Size(104, 33);
            this._Bt_Exportar.TabIndex = 121;
            this._Bt_Exportar.Text = "Exportar";
            this._Bt_Exportar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._Bt_Exportar.UseVisualStyleBackColor = true;
            this._Bt_Exportar.Click += new System.EventHandler(this._Bt_Exportar_Click);
            // 
            // _Sfd_1
            // 
            this._Sfd_1.Filter = "xls files (*.xls)|*.xls";
            // 
            // Frm_Inf_NC_Aplic_Caja
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 451);
            this.Controls.Add(this._Rpv_Main);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Frm_Inf_NC_Aplic_Caja";
            this.Text = "NC Aplicadas En Caja";
            this.Load += new System.EventHandler(this.Frm_Inf_NC_Aplic_Caja_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._Er_Error)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer _Rpv_Main;
        private System.Windows.Forms.Button _Bt_Caja;
        private System.Windows.Forms.TextBox _Txt_Caja;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button _Bt_Consultar;
        private System.Windows.Forms.ErrorProvider _Er_Error;
        private System.Windows.Forms.Button _Bt_Caja_2;
        private System.Windows.Forms.TextBox _Txt_Caja_2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button _Bt_Limpiar;
        private System.Windows.Forms.CheckBox _Chk_PorAplicar;
        private System.Windows.Forms.Button _Bt_Exportar;
        private System.Windows.Forms.SaveFileDialog _Sfd_1;
    }
}