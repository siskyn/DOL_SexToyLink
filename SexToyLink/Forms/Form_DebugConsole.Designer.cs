namespace SexToyLink.Forms
{
    partial class Form_DebugConsole
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_DebugConsole));
            this.textBox_console = new System.Windows.Forms.TextBox();
            this.button_Copy = new System.Windows.Forms.Button();
            this.button_startMonitor = new System.Windows.Forms.Button();
            this.buttonStopMonitor = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox_console
            // 
            this.textBox_console.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_console.Location = new System.Drawing.Point(0, 0);
            this.textBox_console.Multiline = true;
            this.textBox_console.Name = "textBox_console";
            this.textBox_console.ReadOnly = true;
            this.textBox_console.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_console.Size = new System.Drawing.Size(734, 559);
            this.textBox_console.TabIndex = 0;
            // 
            // button_Copy
            // 
            this.button_Copy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Copy.Location = new System.Drawing.Point(0, 558);
            this.button_Copy.Name = "button_Copy";
            this.button_Copy.Size = new System.Drawing.Size(250, 35);
            this.button_Copy.TabIndex = 1;
            this.button_Copy.Text = "Copy Console";
            this.button_Copy.UseVisualStyleBackColor = true;
            this.button_Copy.Click += new System.EventHandler(this.button_Copy_Click);
            // 
            // button_startMonitor
            // 
            this.button_startMonitor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_startMonitor.Location = new System.Drawing.Point(250, 558);
            this.button_startMonitor.Name = "button_startMonitor";
            this.button_startMonitor.Size = new System.Drawing.Size(250, 35);
            this.button_startMonitor.TabIndex = 2;
            this.button_startMonitor.Text = "Start Monitoring";
            this.button_startMonitor.UseVisualStyleBackColor = true;
            this.button_startMonitor.Click += new System.EventHandler(this.button_startMonitor_Click);
            // 
            // buttonStopMonitor
            // 
            this.buttonStopMonitor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStopMonitor.Location = new System.Drawing.Point(500, 558);
            this.buttonStopMonitor.Name = "buttonStopMonitor";
            this.buttonStopMonitor.Size = new System.Drawing.Size(250, 35);
            this.buttonStopMonitor.TabIndex = 3;
            this.buttonStopMonitor.Text = "Stop monitoring";
            this.buttonStopMonitor.UseVisualStyleBackColor = true;
            this.buttonStopMonitor.Click += new System.EventHandler(this.buttonStopMonitor_Click);
            // 
            // Form_DebugConsole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 592);
            this.Controls.Add(this.buttonStopMonitor);
            this.Controls.Add(this.button_startMonitor);
            this.Controls.Add(this.button_Copy);
            this.Controls.Add(this.textBox_console);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(750, 7000);
            this.MinimumSize = new System.Drawing.Size(750, 176);
            this.Name = "Form_DebugConsole";
            this.Text = "SexToyLink - Debug Console";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_DebugConsole_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_console;
        private System.Windows.Forms.Button button_Copy;
        private System.Windows.Forms.Button button_startMonitor;
        private System.Windows.Forms.Button buttonStopMonitor;
    }
}