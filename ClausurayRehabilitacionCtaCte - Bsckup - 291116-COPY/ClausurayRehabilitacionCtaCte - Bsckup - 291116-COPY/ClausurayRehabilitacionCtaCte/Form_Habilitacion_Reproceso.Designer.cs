namespace ClausurayRehabilitacionCtaCte
{
    partial class Form_Habilitacion_Reproceso
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Habilitacion_Reproceso));
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.calendar = new System.Windows.Forms.MonthCalendar();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.cbcc = new System.Windows.Forms.CheckBox();
            this.cbct = new System.Windows.Forms.CheckBox();
            this.cbpc = new System.Windows.Forms.CheckBox();
            this.cbpr = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(41, 103);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(340, 18);
            this.label12.TabIndex = 40;
            this.label12.Text = "HABILITAR FECHA PARA REPROCESO";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Image = global::ClausurayRehabilitacionCtaCte.Properties.Resources.logobcpblanco1;
            this.label11.Location = new System.Drawing.Point(137, 42);
            this.label11.MaximumSize = new System.Drawing.Size(500, 300);
            this.label11.MinimumSize = new System.Drawing.Size(150, 40);
            this.label11.Name = "label11";
            this.label11.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label11.Size = new System.Drawing.Size(150, 40);
            this.label11.TabIndex = 39;
            this.label11.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // calendar
            // 
            this.calendar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.calendar.BackColor = System.Drawing.Color.White;
            this.calendar.ForeColor = System.Drawing.Color.Black;
            this.calendar.Location = new System.Drawing.Point(129, 281);
            this.calendar.Name = "calendar";
            this.calendar.TabIndex = 41;
            this.calendar.TitleBackColor = System.Drawing.Color.LightGray;
            this.calendar.TitleForeColor = System.Drawing.Color.Black;
            this.calendar.TrailingForeColor = System.Drawing.Color.Orange;
            this.calendar.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.calendar_DateChanged);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(41, 217);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(340, 18);
            this.label1.TabIndex = 42;
            this.label1.Text = "Seleccione una fecha:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button1.AutoSize = true;
            this.button1.BackColor = System.Drawing.Color.LightGray;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(129, 468);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(73, 30);
            this.button1.TabIndex = 43;
            this.button1.Text = "Aceptar";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button2.AutoSize = true;
            this.button2.BackColor = System.Drawing.Color.LightGray;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.Black;
            this.button2.Location = new System.Drawing.Point(216, 468);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 30);
            this.button2.TabIndex = 44;
            this.button2.Text = "Salir";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // cbcc
            // 
            this.cbcc.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbcc.AutoSize = true;
            this.cbcc.BackColor = System.Drawing.Color.Transparent;
            this.cbcc.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbcc.ForeColor = System.Drawing.Color.White;
            this.cbcc.Location = new System.Drawing.Point(67, 142);
            this.cbcc.Name = "cbcc";
            this.cbcc.Size = new System.Drawing.Size(141, 20);
            this.cbcc.TabIndex = 45;
            this.cbcc.Text = "Carga de Circulares";
            this.cbcc.UseVisualStyleBackColor = false;
            // 
            // cbct
            // 
            this.cbct.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbct.AutoSize = true;
            this.cbct.BackColor = System.Drawing.Color.Transparent;
            this.cbct.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbct.ForeColor = System.Drawing.Color.White;
            this.cbct.Location = new System.Drawing.Point(216, 142);
            this.cbct.Name = "cbct";
            this.cbct.Size = new System.Drawing.Size(120, 20);
            this.cbct.TabIndex = 46;
            this.cbct.Text = "Carga de Tablas";
            this.cbct.UseVisualStyleBackColor = false;
            // 
            // cbpc
            // 
            this.cbpc.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbpc.AutoSize = true;
            this.cbpc.BackColor = System.Drawing.Color.Transparent;
            this.cbpc.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbpc.ForeColor = System.Drawing.Color.White;
            this.cbpc.Location = new System.Drawing.Point(67, 180);
            this.cbpc.Name = "cbpc";
            this.cbpc.Size = new System.Drawing.Size(113, 20);
            this.cbpc.TabIndex = 47;
            this.cbpc.Text = "Proc. Clausura";
            this.cbpc.UseVisualStyleBackColor = false;
            // 
            // cbpr
            // 
            this.cbpr.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbpr.AutoSize = true;
            this.cbpr.BackColor = System.Drawing.Color.Transparent;
            this.cbpr.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbpr.ForeColor = System.Drawing.Color.White;
            this.cbpr.Location = new System.Drawing.Point(216, 180);
            this.cbpr.Name = "cbpr";
            this.cbpr.Size = new System.Drawing.Size(143, 20);
            this.cbpr.TabIndex = 48;
            this.cbpr.Text = "Proc. Rehabilitación";
            this.cbpr.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(37, 246);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(340, 18);
            this.label2.TabIndex = 49;
            this.label2.Text = "(La fecha debe ser la de Proceso)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Form_Habilitacion_Reproceso
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::ClausurayRehabilitacionCtaCte.Properties.Resources.fondo;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(415, 511);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbpr);
            this.Controls.Add(this.cbpc);
            this.Controls.Add(this.cbct);
            this.Controls.Add(this.cbcc);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.calendar);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(431, 549);
            this.Name = "Form_Habilitacion_Reproceso";
            this.Text = "Habilitación de Reproceso";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.MonthCalendar calendar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox cbcc;
        private System.Windows.Forms.CheckBox cbct;
        private System.Windows.Forms.CheckBox cbpc;
        private System.Windows.Forms.CheckBox cbpr;
        private System.Windows.Forms.Label label2;
    }
}