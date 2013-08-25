namespace Handler.NetworkHandler.Proxy
{
    partial class ProxyDialog
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.checkBox_EnbaleProxy = new System.Windows.Forms.CheckBox();
            this.groupBox_ProxySettings = new System.Windows.Forms.GroupBox();
            this.numericUpDown_Port = new System.Windows.Forms.NumericUpDown();
            this.label_Port = new System.Windows.Forms.Label();
            this.textBox_ProxyPassword = new System.Windows.Forms.TextBox();
            this.textBox_ProxyUsername = new System.Windows.Forms.TextBox();
            this.textBox_WebProxy = new System.Windows.Forms.TextBox();
            this.label_ProxyPassword = new System.Windows.Forms.Label();
            this.label_ProxyUsername = new System.Windows.Forms.Label();
            this.label_WebProxy = new System.Windows.Forms.Label();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.button_OK = new System.Windows.Forms.Button();
            this.groupBox_ProxySettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Port)).BeginInit();
            this.SuspendLayout();
            // 
            // checkBox_EnbaleProxy
            // 
            this.checkBox_EnbaleProxy.AutoSize = true;
            this.checkBox_EnbaleProxy.Location = new System.Drawing.Point(12, 12);
            this.checkBox_EnbaleProxy.Name = "checkBox_EnbaleProxy";
            this.checkBox_EnbaleProxy.Size = new System.Drawing.Size(87, 17);
            this.checkBox_EnbaleProxy.TabIndex = 2;
            this.checkBox_EnbaleProxy.Text = "enable Proxy";
            this.checkBox_EnbaleProxy.UseVisualStyleBackColor = true;
            this.checkBox_EnbaleProxy.CheckedChanged += new System.EventHandler(this.checkBox_EnbaleProxy_CheckedChanged);
            // 
            // groupBox_ProxySettings
            // 
            this.groupBox_ProxySettings.Controls.Add(this.numericUpDown_Port);
            this.groupBox_ProxySettings.Controls.Add(this.label_Port);
            this.groupBox_ProxySettings.Controls.Add(this.textBox_ProxyPassword);
            this.groupBox_ProxySettings.Controls.Add(this.textBox_ProxyUsername);
            this.groupBox_ProxySettings.Controls.Add(this.textBox_WebProxy);
            this.groupBox_ProxySettings.Controls.Add(this.label_ProxyPassword);
            this.groupBox_ProxySettings.Controls.Add(this.label_ProxyUsername);
            this.groupBox_ProxySettings.Controls.Add(this.label_WebProxy);
            this.groupBox_ProxySettings.Enabled = false;
            this.groupBox_ProxySettings.Location = new System.Drawing.Point(12, 35);
            this.groupBox_ProxySettings.Name = "groupBox_ProxySettings";
            this.groupBox_ProxySettings.Size = new System.Drawing.Size(446, 105);
            this.groupBox_ProxySettings.TabIndex = 3;
            this.groupBox_ProxySettings.TabStop = false;
            this.groupBox_ProxySettings.Text = "Proxy Settings";
            // 
            // numericUpDown_Port
            // 
            this.numericUpDown_Port.Location = new System.Drawing.Point(380, 22);
            this.numericUpDown_Port.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.numericUpDown_Port.Name = "numericUpDown_Port";
            this.numericUpDown_Port.Size = new System.Drawing.Size(59, 20);
            this.numericUpDown_Port.TabIndex = 7;
            this.numericUpDown_Port.Tag = "";
            this.numericUpDown_Port.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown_Port.Value = new decimal(new int[] {
            3128,
            0,
            0,
            0});
            // 
            // label_Port
            // 
            this.label_Port.AutoSize = true;
            this.label_Port.Location = new System.Drawing.Point(345, 25);
            this.label_Port.Name = "label_Port";
            this.label_Port.Size = new System.Drawing.Size(29, 13);
            this.label_Port.TabIndex = 5;
            this.label_Port.Text = "Port:";
            // 
            // textBox_ProxyPassword
            // 
            this.textBox_ProxyPassword.Location = new System.Drawing.Point(82, 74);
            this.textBox_ProxyPassword.Name = "textBox_ProxyPassword";
            this.textBox_ProxyPassword.Size = new System.Drawing.Size(357, 20);
            this.textBox_ProxyPassword.TabIndex = 4;
            this.textBox_ProxyPassword.UseSystemPasswordChar = true;
            this.textBox_ProxyPassword.TextChanged += new System.EventHandler(this.textBox_ProxyPassword_TextChanged);
            // 
            // textBox_ProxyUsername
            // 
            this.textBox_ProxyUsername.Location = new System.Drawing.Point(82, 48);
            this.textBox_ProxyUsername.Name = "textBox_ProxyUsername";
            this.textBox_ProxyUsername.Size = new System.Drawing.Size(357, 20);
            this.textBox_ProxyUsername.TabIndex = 3;
            this.textBox_ProxyUsername.TextChanged += new System.EventHandler(this.textBox_ProxyUsername_TextChanged);
            // 
            // textBox_WebProxy
            // 
            this.textBox_WebProxy.Location = new System.Drawing.Point(82, 22);
            this.textBox_WebProxy.Name = "textBox_WebProxy";
            this.textBox_WebProxy.Size = new System.Drawing.Size(257, 20);
            this.textBox_WebProxy.TabIndex = 2;
            this.textBox_WebProxy.TextChanged += new System.EventHandler(this.textBox_WebProxy_TextChanged);
            // 
            // label_ProxyPassword
            // 
            this.label_ProxyPassword.AutoSize = true;
            this.label_ProxyPassword.Location = new System.Drawing.Point(17, 77);
            this.label_ProxyPassword.Name = "label_ProxyPassword";
            this.label_ProxyPassword.Size = new System.Drawing.Size(56, 13);
            this.label_ProxyPassword.TabIndex = 2;
            this.label_ProxyPassword.Text = "Password:";
            // 
            // label_ProxyUsername
            // 
            this.label_ProxyUsername.AutoSize = true;
            this.label_ProxyUsername.Location = new System.Drawing.Point(17, 51);
            this.label_ProxyUsername.Name = "label_ProxyUsername";
            this.label_ProxyUsername.Size = new System.Drawing.Size(58, 13);
            this.label_ProxyUsername.TabIndex = 1;
            this.label_ProxyUsername.Text = "Username:";
            // 
            // label_WebProxy
            // 
            this.label_WebProxy.AutoSize = true;
            this.label_WebProxy.Location = new System.Drawing.Point(17, 25);
            this.label_WebProxy.Name = "label_WebProxy";
            this.label_WebProxy.Size = new System.Drawing.Size(59, 13);
            this.label_WebProxy.TabIndex = 0;
            this.label_WebProxy.Text = "WebProxy:";
            // 
            // button_Cancel
            // 
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(383, 147);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.button_Cancel.TabIndex = 4;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // button_OK
            // 
            this.button_OK.Location = new System.Drawing.Point(302, 147);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(75, 23);
            this.button_OK.TabIndex = 5;
            this.button_OK.Text = "OK";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // ProxyDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_Cancel;
            this.ClientSize = new System.Drawing.Size(470, 176);
            this.ControlBox = false;
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.checkBox_EnbaleProxy);
            this.Controls.Add(this.groupBox_ProxySettings);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProxyDialog";
            this.ShowIcon = false;
            this.Text = "ProxyDialog";
            this.Load += new System.EventHandler(this.ProxyDialog_Load);
            this.groupBox_ProxySettings.ResumeLayout(false);
            this.groupBox_ProxySettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Port)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox_EnbaleProxy;
        private System.Windows.Forms.GroupBox groupBox_ProxySettings;
        private System.Windows.Forms.NumericUpDown numericUpDown_Port;
        private System.Windows.Forms.Label label_Port;
        private System.Windows.Forms.TextBox textBox_ProxyPassword;
        private System.Windows.Forms.TextBox textBox_ProxyUsername;
        private System.Windows.Forms.TextBox textBox_WebProxy;
        private System.Windows.Forms.Label label_ProxyPassword;
        private System.Windows.Forms.Label label_ProxyUsername;
        private System.Windows.Forms.Label label_WebProxy;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Button button_OK;

    }
}