﻿namespace IRF_Project
{
    partial class Form1
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
            this.dgvTeams = new System.Windows.Forms.DataGridView();
            this.dgvResults = new System.Windows.Forms.DataGridView();
            this.btnSimulation = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTeams)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvTeams
            // 
            this.dgvTeams.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTeams.Location = new System.Drawing.Point(12, 30);
            this.dgvTeams.Name = "dgvTeams";
            this.dgvTeams.Size = new System.Drawing.Size(652, 180);
            this.dgvTeams.TabIndex = 0;
            // 
            // dgvResults
            // 
            this.dgvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResults.Location = new System.Drawing.Point(12, 245);
            this.dgvResults.Name = "dgvResults";
            this.dgvResults.Size = new System.Drawing.Size(652, 212);
            this.dgvResults.TabIndex = 2;
            // 
            // btnSimulation
            // 
            this.btnSimulation.Location = new System.Drawing.Point(680, 30);
            this.btnSimulation.Name = "btnSimulation";
            this.btnSimulation.Size = new System.Drawing.Size(88, 53);
            this.btnSimulation.TabIndex = 3;
            this.btnSimulation.Text = "Szimuláció";
            this.btnSimulation.UseVisualStyleBackColor = true;
            this.btnSimulation.Click += new System.EventHandler(this.btnSimulation_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(680, 245);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(88, 48);
            this.btnExport.TabIndex = 4;
            this.btnExport.Text = "Export (CSV)";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 229);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "A bajnokság végeredménye";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(337, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "A csapatok adatai (az erősségek 1-5 skálán látható, 1-gyenge, 5-erős)";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 440);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnSimulation);
            this.Controls.Add(this.dgvResults);
            this.Controls.Add(this.dgvTeams);
            this.Name = "Form1";
            this.Text = "Sportszimuláció";
            ((System.ComponentModel.ISupportInitialize)(this.dgvTeams)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvTeams;
        private System.Windows.Forms.DataGridView dgvResults;
        private System.Windows.Forms.Button btnSimulation;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

