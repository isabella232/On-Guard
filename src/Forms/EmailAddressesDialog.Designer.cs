﻿namespace OnGuardCore
{
  partial class EmailAddressesDialog
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
      this.AddButton = new System.Windows.Forms.Button();
      this.EditButton = new System.Windows.Forms.Button();
      this.DeleteButton = new System.Windows.Forms.Button();
      this.oKButton = new System.Windows.Forms.Button();
      this.cancelButton = new System.Windows.Forms.Button();
      this.emailAddressList = new System.Windows.Forms.ListView();
      this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
      this.SuspendLayout();
      // 
      // AddButton
      // 
      this.AddButton.Location = new System.Drawing.Point(321, 55);
      this.AddButton.Name = "AddButton";
      this.AddButton.Size = new System.Drawing.Size(75, 23);
      this.AddButton.TabIndex = 1;
      this.AddButton.Text = "Add";
      this.AddButton.UseVisualStyleBackColor = true;
      this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
      // 
      // EditButton
      // 
      this.EditButton.Enabled = false;
      this.EditButton.Location = new System.Drawing.Point(321, 95);
      this.EditButton.Name = "EditButton";
      this.EditButton.Size = new System.Drawing.Size(75, 23);
      this.EditButton.TabIndex = 2;
      this.EditButton.Text = "Edit";
      this.EditButton.UseVisualStyleBackColor = true;
      this.EditButton.Click += new System.EventHandler(this.EditButton_Click);
      // 
      // DeleteButton
      // 
      this.DeleteButton.Enabled = false;
      this.DeleteButton.Location = new System.Drawing.Point(321, 135);
      this.DeleteButton.Name = "DeleteButton";
      this.DeleteButton.Size = new System.Drawing.Size(75, 23);
      this.DeleteButton.TabIndex = 3;
      this.DeleteButton.Text = "Delete";
      this.DeleteButton.UseVisualStyleBackColor = true;
      this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
      // 
      // oKButton
      // 
      this.oKButton.Location = new System.Drawing.Point(125, 196);
      this.oKButton.Name = "oKButton";
      this.oKButton.Size = new System.Drawing.Size(75, 23);
      this.oKButton.TabIndex = 4;
      this.oKButton.Text = "OK";
      this.oKButton.UseVisualStyleBackColor = true;
      this.oKButton.Click += new System.EventHandler(this.OKButton_Click);
      // 
      // cancelButton
      // 
      this.cancelButton.Location = new System.Drawing.Point(211, 196);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(75, 23);
      this.cancelButton.TabIndex = 5;
      this.cancelButton.Text = "Cancel";
      this.cancelButton.UseVisualStyleBackColor = true;
      this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
      // 
      // emailAddressList
      // 
      this.emailAddressList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
      this.emailAddressList.FullRowSelect = true;
      this.emailAddressList.GridLines = true;
      this.emailAddressList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.emailAddressList.Location = new System.Drawing.Point(12, 27);
      this.emailAddressList.MultiSelect = false;
      this.emailAddressList.Name = "emailAddressList";
      this.emailAddressList.Size = new System.Drawing.Size(298, 148);
      this.emailAddressList.TabIndex = 0;
      this.emailAddressList.UseCompatibleStateImageBehavior = false;
      this.emailAddressList.View = System.Windows.Forms.View.Details;
      this.emailAddressList.ItemActivate += new System.EventHandler(this.OnActivate);
      this.emailAddressList.SelectedIndexChanged += new System.EventHandler(this.OnSelectedIndexChanged);
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "Email Address";
      this.columnHeader1.Width = 277;
      // 
      // EmailAddressesDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
      this.ClientSize = new System.Drawing.Size(410, 234);
      this.Controls.Add(this.emailAddressList);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.oKButton);
      this.Controls.Add(this.DeleteButton);
      this.Controls.Add(this.EditButton);
      this.Controls.Add(this.AddButton);
      this.Name = "EmailAddressesDialog";
      this.Text = "Add/Edit/Delete Email Addresses";
      this.ResumeLayout(false);

    }

        #endregion
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button EditButton;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Button oKButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ListView emailAddressList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}