namespace SexToyLink.Forms
{
    partial class Form_Device_Details
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Device_Details));
            this.listView_devicesOral = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.listView_devicesBreasts = new System.Windows.Forms.ListView();
            this.listView_devicesGenital = new System.Windows.Forms.ListView();
            this.listView_devicesAnal = new System.Windows.Forms.ListView();
            this.listView_devicesAll = new System.Windows.Forms.ListView();
            this.label5 = new System.Windows.Forms.Label();
            this.button_Delete_oral = new System.Windows.Forms.Button();
            this.button_Delete_breast = new System.Windows.Forms.Button();
            this.button_Delete_anal = new System.Windows.Forms.Button();
            this.button_delete_genital = new System.Windows.Forms.Button();
            this.button_close = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listView_devicesOral
            // 
            this.listView_devicesOral.AllowDrop = true;
            this.listView_devicesOral.HideSelection = false;
            this.listView_devicesOral.Location = new System.Drawing.Point(261, 28);
            this.listView_devicesOral.MultiSelect = false;
            this.listView_devicesOral.Name = "listView_devicesOral";
            this.listView_devicesOral.Size = new System.Drawing.Size(225, 204);
            this.listView_devicesOral.TabIndex = 0;
            this.listView_devicesOral.UseCompatibleStateImageBehavior = false;
            this.listView_devicesOral.View = System.Windows.Forms.View.Tile;
            this.listView_devicesOral.DragDrop += new System.Windows.Forms.DragEventHandler(this.listView_devicesAny_DragDrop);
            this.listView_devicesOral.DragOver += new System.Windows.Forms.DragEventHandler(this.listView_devicesAny_DragOver);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(339, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Oral Devices";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(566, 273);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Anal Devices";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(312, 273);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(119, 13);
            this.Label3.TabIndex = 3;
            this.Label3.Text = "Vagina / Penis Devices";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(566, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Breast Devices";
            // 
            // listView_devicesBreasts
            // 
            this.listView_devicesBreasts.AllowDrop = true;
            this.listView_devicesBreasts.HideSelection = false;
            this.listView_devicesBreasts.Location = new System.Drawing.Point(492, 28);
            this.listView_devicesBreasts.MultiSelect = false;
            this.listView_devicesBreasts.Name = "listView_devicesBreasts";
            this.listView_devicesBreasts.Size = new System.Drawing.Size(225, 204);
            this.listView_devicesBreasts.TabIndex = 5;
            this.listView_devicesBreasts.UseCompatibleStateImageBehavior = false;
            this.listView_devicesBreasts.View = System.Windows.Forms.View.Tile;
            this.listView_devicesBreasts.DragDrop += new System.Windows.Forms.DragEventHandler(this.listView_devicesAny_DragDrop);
            this.listView_devicesBreasts.DragOver += new System.Windows.Forms.DragEventHandler(this.listView_devicesAny_DragOver);
            // 
            // listView_devicesGenital
            // 
            this.listView_devicesGenital.AllowDrop = true;
            this.listView_devicesGenital.HideSelection = false;
            this.listView_devicesGenital.Location = new System.Drawing.Point(261, 289);
            this.listView_devicesGenital.MultiSelect = false;
            this.listView_devicesGenital.Name = "listView_devicesGenital";
            this.listView_devicesGenital.Size = new System.Drawing.Size(225, 204);
            this.listView_devicesGenital.TabIndex = 6;
            this.listView_devicesGenital.UseCompatibleStateImageBehavior = false;
            this.listView_devicesGenital.View = System.Windows.Forms.View.Tile;
            this.listView_devicesGenital.DragDrop += new System.Windows.Forms.DragEventHandler(this.listView_devicesAny_DragDrop);
            this.listView_devicesGenital.DragOver += new System.Windows.Forms.DragEventHandler(this.listView_devicesAny_DragOver);
            // 
            // listView_devicesAnal
            // 
            this.listView_devicesAnal.AllowDrop = true;
            this.listView_devicesAnal.HideSelection = false;
            this.listView_devicesAnal.Location = new System.Drawing.Point(492, 289);
            this.listView_devicesAnal.MultiSelect = false;
            this.listView_devicesAnal.Name = "listView_devicesAnal";
            this.listView_devicesAnal.Size = new System.Drawing.Size(225, 204);
            this.listView_devicesAnal.TabIndex = 7;
            this.listView_devicesAnal.UseCompatibleStateImageBehavior = false;
            this.listView_devicesAnal.View = System.Windows.Forms.View.Tile;
            this.listView_devicesAnal.DragDrop += new System.Windows.Forms.DragEventHandler(this.listView_devicesAny_DragDrop);
            this.listView_devicesAnal.DragOver += new System.Windows.Forms.DragEventHandler(this.listView_devicesAny_DragOver);
            // 
            // listView_devicesAll
            // 
            this.listView_devicesAll.HideSelection = false;
            this.listView_devicesAll.Location = new System.Drawing.Point(12, 28);
            this.listView_devicesAll.MultiSelect = false;
            this.listView_devicesAll.Name = "listView_devicesAll";
            this.listView_devicesAll.Size = new System.Drawing.Size(225, 496);
            this.listView_devicesAll.TabIndex = 8;
            this.listView_devicesAll.UseCompatibleStateImageBehavior = false;
            this.listView_devicesAll.View = System.Windows.Forms.View.Tile;
            this.listView_devicesAll.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listView_devicesAll_ItemDrag);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(71, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "All devices";
            // 
            // button_Delete_oral
            // 
            this.button_Delete_oral.Location = new System.Drawing.Point(261, 233);
            this.button_Delete_oral.Name = "button_Delete_oral";
            this.button_Delete_oral.Size = new System.Drawing.Size(225, 25);
            this.button_Delete_oral.TabIndex = 11;
            this.button_Delete_oral.Text = "Remove selected";
            this.button_Delete_oral.UseVisualStyleBackColor = true;
            this.button_Delete_oral.Click += new System.EventHandler(this.button_Delete_oral_Click);
            // 
            // button_Delete_breast
            // 
            this.button_Delete_breast.Location = new System.Drawing.Point(492, 233);
            this.button_Delete_breast.Name = "button_Delete_breast";
            this.button_Delete_breast.Size = new System.Drawing.Size(225, 25);
            this.button_Delete_breast.TabIndex = 12;
            this.button_Delete_breast.Text = "Remove selected";
            this.button_Delete_breast.UseVisualStyleBackColor = true;
            this.button_Delete_breast.Click += new System.EventHandler(this.button_Delete_breast_Click);
            // 
            // button_Delete_anal
            // 
            this.button_Delete_anal.Location = new System.Drawing.Point(492, 499);
            this.button_Delete_anal.Name = "button_Delete_anal";
            this.button_Delete_anal.Size = new System.Drawing.Size(225, 25);
            this.button_Delete_anal.TabIndex = 14;
            this.button_Delete_anal.Text = "Remove selected";
            this.button_Delete_anal.UseVisualStyleBackColor = true;
            this.button_Delete_anal.Click += new System.EventHandler(this.button_Delete_anal_Click);
            // 
            // button_delete_genital
            // 
            this.button_delete_genital.Location = new System.Drawing.Point(261, 499);
            this.button_delete_genital.Name = "button_delete_genital";
            this.button_delete_genital.Size = new System.Drawing.Size(225, 25);
            this.button_delete_genital.TabIndex = 13;
            this.button_delete_genital.Text = "Remove selected";
            this.button_delete_genital.UseVisualStyleBackColor = true;
            this.button_delete_genital.Click += new System.EventHandler(this.button_delete_genital_Click);
            // 
            // button_close
            // 
            this.button_close.Location = new System.Drawing.Point(12, 530);
            this.button_close.Name = "button_close";
            this.button_close.Size = new System.Drawing.Size(703, 32);
            this.button_close.TabIndex = 15;
            this.button_close.Text = "Close";
            this.button_close.UseVisualStyleBackColor = true;
            this.button_close.Click += new System.EventHandler(this.button_close_Click);
            // 
            // Form_Device_Details
            // 
            this.AcceptButton = this.button_close;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(715, 562);
            this.ControlBox = false;
            this.Controls.Add(this.button_close);
            this.Controls.Add(this.button_Delete_anal);
            this.Controls.Add(this.button_delete_genital);
            this.Controls.Add(this.button_Delete_breast);
            this.Controls.Add(this.button_Delete_oral);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.listView_devicesAll);
            this.Controls.Add(this.listView_devicesAnal);
            this.Controls.Add(this.listView_devicesGenital);
            this.Controls.Add(this.listView_devicesBreasts);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listView_devicesOral);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_Device_Details";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Devices Details";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Device_Details_FormClosing);
            this.Load += new System.EventHandler(this.Form_Device_Details_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView_devicesOral;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label Label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListView listView_devicesBreasts;
        private System.Windows.Forms.ListView listView_devicesGenital;
        private System.Windows.Forms.ListView listView_devicesAnal;
        private System.Windows.Forms.ListView listView_devicesAll;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button_Delete_oral;
        private System.Windows.Forms.Button button_Delete_breast;
        private System.Windows.Forms.Button button_Delete_anal;
        private System.Windows.Forms.Button button_delete_genital;
        private System.Windows.Forms.Button button_close;
    }
}