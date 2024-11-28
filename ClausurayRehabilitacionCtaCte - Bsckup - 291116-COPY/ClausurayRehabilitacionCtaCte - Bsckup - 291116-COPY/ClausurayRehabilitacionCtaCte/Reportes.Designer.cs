namespace ClausurayRehabilitacionCtaCte
{
    partial class Reportes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Reportes));
            this.dgvrep = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.cbreportes = new System.Windows.Forms.ComboBox();
            this.btn_limpiar = new System.Windows.Forms.Button();
            this.btn_excel = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.btn_pdf = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.btnbuscar = new System.Windows.Forms.Button();
            this.rclau = new System.Windows.Forms.RadioButton();
            this.rrehab = new System.Windows.Forms.RadioButton();
            this.rpres = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvrep)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvrep
            // 
            this.dgvrep.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dgvrep.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.dgvrep.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvrep.Location = new System.Drawing.Point(8, 203);
            this.dgvrep.Name = "dgvrep";
            this.dgvrep.Size = new System.Drawing.Size(997, 399);
            this.dgvrep.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(5, 124);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(160, 18);
            this.label2.TabIndex = 5;
            this.label2.Text = "Seleccione el reporte:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // cbreportes
            // 
            this.cbreportes.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbreportes.BackColor = System.Drawing.Color.Orange;
            this.cbreportes.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbreportes.FormattingEnabled = true;
            this.cbreportes.Location = new System.Drawing.Point(527, 123);
            this.cbreportes.MaximumSize = new System.Drawing.Size(600, 0);
            this.cbreportes.Name = "cbreportes";
            this.cbreportes.Size = new System.Drawing.Size(485, 24);
            this.cbreportes.TabIndex = 6;
            this.cbreportes.SelectedIndexChanged += new System.EventHandler(this.cbreportes_SelectedIndexChanged);
            // 
            // btn_limpiar
            // 
            this.btn_limpiar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btn_limpiar.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_limpiar.Location = new System.Drawing.Point(347, 615);
            this.btn_limpiar.Name = "btn_limpiar";
            this.btn_limpiar.Size = new System.Drawing.Size(75, 23);
            this.btn_limpiar.TabIndex = 7;
            this.btn_limpiar.Text = "Limpiar";
            this.btn_limpiar.UseVisualStyleBackColor = true;
            this.btn_limpiar.Click += new System.EventHandler(this.btn_limpiar_Click);
            // 
            // btn_excel
            // 
            this.btn_excel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btn_excel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_excel.Location = new System.Drawing.Point(462, 615);
            this.btn_excel.Name = "btn_excel";
            this.btn_excel.Size = new System.Drawing.Size(104, 23);
            this.btn_excel.TabIndex = 8;
            this.btn_excel.Text = "Exportar Excel";
            this.btn_excel.UseVisualStyleBackColor = true;
            this.btn_excel.Click += new System.EventHandler(this.btn_excel_Click);
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Image = global::ClausurayRehabilitacionCtaCte.Properties.Resources.logobcpblanco1;
            this.label11.Location = new System.Drawing.Point(412, 2);
            this.label11.MaximumSize = new System.Drawing.Size(500, 300);
            this.label11.MinimumSize = new System.Drawing.Size(150, 70);
            this.label11.Name = "label11";
            this.label11.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label11.Size = new System.Drawing.Size(150, 70);
            this.label11.TabIndex = 31;
            this.label11.Click += new System.EventHandler(this.label11_Click);
            // 
            // btn_pdf
            // 
            this.btn_pdf.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btn_pdf.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_pdf.Location = new System.Drawing.Point(605, 615);
            this.btn_pdf.Name = "btn_pdf";
            this.btn_pdf.Size = new System.Drawing.Size(101, 23);
            this.btn_pdf.TabIndex = 32;
            this.btn_pdf.Text = "Exportar PDF";
            this.btn_pdf.UseVisualStyleBackColor = true;
            this.btn_pdf.Click += new System.EventHandler(this.btn_pdf_Click);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(18, 175);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 18);
            this.label1.TabIndex = 33;
            this.label1.Text = "Fecha Inicio:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(396, 175);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 18);
            this.label3.TabIndex = 34;
            this.label3.Text = "Fecha Fin:";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dateTimePicker1.CalendarMonthBackground = System.Drawing.Color.White;
            this.dateTimePicker1.CalendarTitleBackColor = System.Drawing.Color.Silver;
            this.dateTimePicker1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker1.Location = new System.Drawing.Point(144, 175);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(227, 21);
            this.dateTimePicker1.TabIndex = 35;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dateTimePicker2.CalendarMonthBackground = System.Drawing.Color.White;
            this.dateTimePicker2.CalendarTitleBackColor = System.Drawing.Color.Silver;
            this.dateTimePicker2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker2.Location = new System.Drawing.Point(508, 175);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(225, 21);
            this.dateTimePicker2.TabIndex = 36;
            this.dateTimePicker2.ValueChanged += new System.EventHandler(this.dateTimePicker2_ValueChanged);
            // 
            // btnbuscar
            // 
            this.btnbuscar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnbuscar.Location = new System.Drawing.Point(797, 174);
            this.btnbuscar.Name = "btnbuscar";
            this.btnbuscar.Size = new System.Drawing.Size(75, 23);
            this.btnbuscar.TabIndex = 37;
            this.btnbuscar.Text = "Buscar";
            this.btnbuscar.UseVisualStyleBackColor = true;
            this.btnbuscar.Click += new System.EventHandler(this.button1_Click);
            // 
            // rclau
            // 
            this.rclau.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rclau.AutoSize = true;
            this.rclau.BackColor = System.Drawing.Color.Transparent;
            this.rclau.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rclau.ForeColor = System.Drawing.Color.White;
            this.rclau.Location = new System.Drawing.Point(167, 124);
            this.rclau.Name = "rclau";
            this.rclau.Size = new System.Drawing.Size(85, 21);
            this.rclau.TabIndex = 7;
            this.rclau.TabStop = true;
            this.rclau.Text = "Clausura";
            this.rclau.UseVisualStyleBackColor = false;
            this.rclau.CheckedChanged += new System.EventHandler(this.rclau_CheckedChanged);
            // 
            // rrehab
            // 
            this.rrehab.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rrehab.AutoSize = true;
            this.rrehab.BackColor = System.Drawing.Color.Transparent;
            this.rrehab.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rrehab.ForeColor = System.Drawing.Color.White;
            this.rrehab.Location = new System.Drawing.Point(251, 124);
            this.rrehab.Name = "rrehab";
            this.rrehab.Size = new System.Drawing.Size(117, 21);
            this.rrehab.TabIndex = 8;
            this.rrehab.TabStop = true;
            this.rrehab.Text = "Rehabilitación";
            this.rrehab.UseVisualStyleBackColor = false;
            this.rrehab.CheckedChanged += new System.EventHandler(this.rrehab_CheckedChanged);
            // 
            // rpres
            // 
            this.rpres.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rpres.AutoSize = true;
            this.rpres.BackColor = System.Drawing.Color.Transparent;
            this.rpres.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rpres.ForeColor = System.Drawing.Color.White;
            this.rpres.Location = new System.Drawing.Point(365, 124);
            this.rpres.Name = "rpres";
            this.rpres.Size = new System.Drawing.Size(159, 21);
            this.rpres.TabIndex = 9;
            this.rpres.TabStop = true;
            this.rpres.Text = "Rehab. Prescripción";
            this.rpres.UseVisualStyleBackColor = false;
            this.rpres.CheckedChanged += new System.EventHandler(this.rpres_CheckedChanged);
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(387, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(203, 18);
            this.label4.TabIndex = 38;
            this.label4.Text = "REPORTES DEL SISTEMA";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // Reportes
            // 
            this.AcceptButton = this.btnbuscar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::ClausurayRehabilitacionCtaCte.Properties.Resources.fondo;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1020, 650);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.rpres);
            this.Controls.Add(this.rrehab);
            this.Controls.Add(this.btnbuscar);
            this.Controls.Add(this.rclau);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbreportes);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_pdf);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.btn_excel);
            this.Controls.Add(this.btn_limpiar);
            this.Controls.Add(this.dgvrep);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1022, 688);
            this.Name = "Reportes";
            this.Text = "Reportes";
            this.Load += new System.EventHandler(this.Reportes_Load_1);
            ((System.ComponentModel.ISupportInitialize)(this.dgvrep)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvrep;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbreportes;
        private System.Windows.Forms.Button btn_limpiar;
        private System.Windows.Forms.Button btn_excel;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btn_pdf;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Button btnbuscar;
        private System.Windows.Forms.RadioButton rpres;
        private System.Windows.Forms.RadioButton rrehab;
        private System.Windows.Forms.RadioButton rclau;
        private System.Windows.Forms.Label label4;
    }
}