using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Xml.Linq;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.Windows.Media;
using System.Net;
using System.Globalization;


using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Plugins;
using Styx.Pathing;

namespace ProfileChanger
{
    partial class ProfileChangerUI
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.BSave = new System.Windows.Forms.Button();
            this.CB2 = new System.Windows.Forms.CheckBox();
            this.CB3 = new System.Windows.Forms.CheckBox();
            this.CB4 = new System.Windows.Forms.CheckBox();
            this.CB5 = new System.Windows.Forms.CheckBox();
            this.CB6 = new System.Windows.Forms.CheckBox();
            this.CB1 = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.CB7 = new System.Windows.Forms.CheckBox();
            this.CB8 = new System.Windows.Forms.CheckBox();
            this.CB9 = new System.Windows.Forms.CheckBox();
            this.CB10 = new System.Windows.Forms.CheckBox();
            this.CB11 = new System.Windows.Forms.CheckBox();
            this.CB12 = new System.Windows.Forms.CheckBox();
            this.tb1 = new System.Windows.Forms.TextBox();
            this.tb2 = new System.Windows.Forms.TextBox();
            this.tb3 = new System.Windows.Forms.TextBox();
            this.tb4 = new System.Windows.Forms.TextBox();
            this.tb5 = new System.Windows.Forms.TextBox();
            this.tb6 = new System.Windows.Forms.TextBox();
            this.tb7 = new System.Windows.Forms.TextBox();
            this.tb8 = new System.Windows.Forms.TextBox();
            this.tb9 = new System.Windows.Forms.TextBox();
            this.tb11 = new System.Windows.Forms.TextBox();
            this.tb10 = new System.Windows.Forms.TextBox();
            this.tb12 = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.DGb = new System.Windows.Forms.Button();
            this.DG = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(51, 133);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Golden Lotus";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(51, 159);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Tillers";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(51, 185);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Cloud Serpents";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(51, 211);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "The Anglers";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(51, 237);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Landfall";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(51, 263);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "The Klaxxi";
            // 
            // BSave
            // 
            this.BSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BSave.Location = new System.Drawing.Point(185, 453);
            this.BSave.Name = "BSave";
            this.BSave.Size = new System.Drawing.Size(127, 26);
            this.BSave.TabIndex = 5;
            this.BSave.Text = "Save Settings";
            this.BSave.UseVisualStyleBackColor = true;
            this.BSave.Click += new System.EventHandler(this.BSave_Click);
            // 
            // CB2
            // 
            this.CB2.AutoSize = true;
            this.CB2.Enabled = false;
            this.CB2.Location = new System.Drawing.Point(28, 159);
            this.CB2.Name = "CB2";
            this.CB2.Size = new System.Drawing.Size(15, 14);
            this.CB2.TabIndex = 6;
            this.CB2.UseVisualStyleBackColor = true;
            // 
            // CB3
            // 
            this.CB3.AutoSize = true;
            this.CB3.Enabled = false;
            this.CB3.Location = new System.Drawing.Point(28, 185);
            this.CB3.Name = "CB3";
            this.CB3.Size = new System.Drawing.Size(15, 14);
            this.CB3.TabIndex = 6;
            this.CB3.UseVisualStyleBackColor = true;
            // 
            // CB4
            // 
            this.CB4.AutoSize = true;
            this.CB4.Enabled = false;
            this.CB4.Location = new System.Drawing.Point(28, 211);
            this.CB4.Name = "CB4";
            this.CB4.Size = new System.Drawing.Size(15, 14);
            this.CB4.TabIndex = 6;
            this.CB4.UseVisualStyleBackColor = true;
            // 
            // CB5
            // 
            this.CB5.AutoSize = true;
            this.CB5.Enabled = false;
            this.CB5.Location = new System.Drawing.Point(28, 237);
            this.CB5.Name = "CB5";
            this.CB5.Size = new System.Drawing.Size(15, 14);
            this.CB5.TabIndex = 6;
            this.CB5.UseVisualStyleBackColor = true;
            // 
            // CB6
            // 
            this.CB6.AutoSize = true;
            this.CB6.Enabled = false;
            this.CB6.Location = new System.Drawing.Point(28, 263);
            this.CB6.Name = "CB6";
            this.CB6.Size = new System.Drawing.Size(15, 14);
            this.CB6.TabIndex = 6;
            this.CB6.UseVisualStyleBackColor = true;
            // 
            // CB1
            // 
            this.CB1.AutoSize = true;
            this.CB1.Enabled = false;
            this.CB1.Location = new System.Drawing.Point(28, 133);
            this.CB1.Name = "CB1";
            this.CB1.Size = new System.Drawing.Size(15, 14);
            this.CB1.TabIndex = 8;
            this.CB1.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(51, 289);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(69, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "Shado Pan";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(51, 315);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(93, 13);
            this.label11.TabIndex = 1;
            this.label11.Text = "Isle of Thunder";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(51, 341);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(128, 13);
            this.label12.TabIndex = 1;
            this.label12.Text = "Isle of Thunder (PvP)";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(51, 367);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(104, 13);
            this.label13.TabIndex = 1;
            this.label13.Text = "August Celestials";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(51, 393);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(52, 13);
            this.label14.TabIndex = 1;
            this.label14.Text = "Not Used";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(51, 420);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(52, 13);
            this.label15.TabIndex = 1;
            this.label15.Text = "Not Used";
            // 
            // CB7
            // 
            this.CB7.AutoSize = true;
            this.CB7.Enabled = false;
            this.CB7.Location = new System.Drawing.Point(28, 289);
            this.CB7.Name = "CB7";
            this.CB7.Size = new System.Drawing.Size(15, 14);
            this.CB7.TabIndex = 21;
            this.CB7.UseVisualStyleBackColor = true;
            // 
            // CB8
            // 
            this.CB8.AutoSize = true;
            this.CB8.Enabled = false;
            this.CB8.Location = new System.Drawing.Point(28, 315);
            this.CB8.Name = "CB8";
            this.CB8.Size = new System.Drawing.Size(15, 14);
            this.CB8.TabIndex = 22;
            this.CB8.UseVisualStyleBackColor = true;
            // 
            // CB9
            // 
            this.CB9.AutoSize = true;
            this.CB9.Enabled = false;
            this.CB9.Location = new System.Drawing.Point(28, 341);
            this.CB9.Name = "CB9";
            this.CB9.Size = new System.Drawing.Size(15, 14);
            this.CB9.TabIndex = 23;
            this.CB9.UseVisualStyleBackColor = true;
            // 
            // CB10
            // 
            this.CB10.AutoSize = true;
            this.CB10.Enabled = false;
            this.CB10.Location = new System.Drawing.Point(28, 367);
            this.CB10.Name = "CB10";
            this.CB10.Size = new System.Drawing.Size(15, 14);
            this.CB10.TabIndex = 24;
            this.CB10.UseVisualStyleBackColor = true;
            // 
            // CB11
            // 
            this.CB11.AutoSize = true;
            this.CB11.Enabled = false;
            this.CB11.Location = new System.Drawing.Point(28, 393);
            this.CB11.Name = "CB11";
            this.CB11.Size = new System.Drawing.Size(15, 14);
            this.CB11.TabIndex = 25;
            this.CB11.UseVisualStyleBackColor = true;
            // 
            // CB12
            // 
            this.CB12.AutoSize = true;
            this.CB12.Enabled = false;
            this.CB12.Location = new System.Drawing.Point(28, 419);
            this.CB12.Name = "CB12";
            this.CB12.Size = new System.Drawing.Size(15, 14);
            this.CB12.TabIndex = 26;
            this.CB12.UseVisualStyleBackColor = true;
            // 
            // tb1
            // 
            this.tb1.Enabled = false;
            this.tb1.Location = new System.Drawing.Point(211, 130);
            this.tb1.Name = "tb1";
            this.tb1.Size = new System.Drawing.Size(100, 20);
            this.tb1.TabIndex = 27;
            // 
            // tb2
            // 
            this.tb2.Enabled = false;
            this.tb2.Location = new System.Drawing.Point(211, 156);
            this.tb2.Name = "tb2";
            this.tb2.Size = new System.Drawing.Size(100, 20);
            this.tb2.TabIndex = 28;
            // 
            // tb3
            // 
            this.tb3.Enabled = false;
            this.tb3.Location = new System.Drawing.Point(211, 182);
            this.tb3.Name = "tb3";
            this.tb3.Size = new System.Drawing.Size(100, 20);
            this.tb3.TabIndex = 29;
            // 
            // tb4
            // 
            this.tb4.Enabled = false;
            this.tb4.Location = new System.Drawing.Point(211, 208);
            this.tb4.Name = "tb4";
            this.tb4.Size = new System.Drawing.Size(100, 20);
            this.tb4.TabIndex = 30;
            // 
            // tb5
            // 
            this.tb5.Enabled = false;
            this.tb5.Location = new System.Drawing.Point(211, 234);
            this.tb5.Name = "tb5";
            this.tb5.Size = new System.Drawing.Size(100, 20);
            this.tb5.TabIndex = 31;
            // 
            // tb6
            // 
            this.tb6.Enabled = false;
            this.tb6.Location = new System.Drawing.Point(211, 260);
            this.tb6.Name = "tb6";
            this.tb6.Size = new System.Drawing.Size(100, 20);
            this.tb6.TabIndex = 32;
            // 
            // tb7
            // 
            this.tb7.Enabled = false;
            this.tb7.Location = new System.Drawing.Point(211, 286);
            this.tb7.Name = "tb7";
            this.tb7.Size = new System.Drawing.Size(100, 20);
            this.tb7.TabIndex = 33;
            // 
            // tb8
            // 
            this.tb8.Enabled = false;
            this.tb8.Location = new System.Drawing.Point(211, 312);
            this.tb8.Name = "tb8";
            this.tb8.Size = new System.Drawing.Size(100, 20);
            this.tb8.TabIndex = 34;
            // 
            // tb9
            // 
            this.tb9.Enabled = false;
            this.tb9.Location = new System.Drawing.Point(211, 338);
            this.tb9.Name = "tb9";
            this.tb9.Size = new System.Drawing.Size(100, 20);
            this.tb9.TabIndex = 35;
            // 
            // tb11
            // 
            this.tb11.Enabled = false;
            this.tb11.Location = new System.Drawing.Point(211, 390);
            this.tb11.Name = "tb11";
            this.tb11.Size = new System.Drawing.Size(100, 20);
            this.tb11.TabIndex = 36;
            // 
            // tb10
            // 
            this.tb10.Enabled = false;
            this.tb10.Location = new System.Drawing.Point(211, 364);
            this.tb10.Name = "tb10";
            this.tb10.Size = new System.Drawing.Size(100, 20);
            this.tb10.TabIndex = 37;
            // 
            // tb12
            // 
            this.tb12.Enabled = false;
            this.tb12.Location = new System.Drawing.Point(211, 417);
            this.tb12.Name = "tb12";
            this.tb12.Size = new System.Drawing.Size(100, 20);
            this.tb12.TabIndex = 38;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // DGb
            // 
            this.DGb.Location = new System.Drawing.Point(237, 47);
            this.DGb.Name = "DGb";
            this.DGb.Size = new System.Drawing.Size(75, 23);
            this.DGb.TabIndex = 40;
            this.DGb.Text = "Browse";
            this.DGb.UseVisualStyleBackColor = true;
            this.DGb.Click += new System.EventHandler(this.button2_Click);
            // 
            // DG
            // 
            this.DG.Enabled = false;
            this.DG.Location = new System.Drawing.Point(28, 50);
            this.DG.Name = "DG";
            this.DG.Size = new System.Drawing.Size(203, 20);
            this.DG.TabIndex = 41;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(28, 34);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(177, 13);
            this.label7.TabIndex = 42;
            this.label7.Text = "Locate the DailyGrind.xml file:";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(29, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(61, 17);
            this.label8.TabIndex = 43;
            this.label8.Text = "Step 1)";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(25, 85);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(61, 17);
            this.label9.TabIndex = 44;
            this.label9.Text = "Step 2)";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(25, 111);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(120, 13);
            this.label16.TabIndex = 45;
            this.label16.Text = "Select your profiles:";
            // 
            // ProfileChangerUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 494);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.DG);
            this.Controls.Add(this.DGb);
            this.Controls.Add(this.tb12);
            this.Controls.Add(this.tb10);
            this.Controls.Add(this.tb11);
            this.Controls.Add(this.tb9);
            this.Controls.Add(this.tb8);
            this.Controls.Add(this.tb7);
            this.Controls.Add(this.tb6);
            this.Controls.Add(this.tb5);
            this.Controls.Add(this.tb4);
            this.Controls.Add(this.tb3);
            this.Controls.Add(this.tb2);
            this.Controls.Add(this.tb1);
            this.Controls.Add(this.CB12);
            this.Controls.Add(this.CB11);
            this.Controls.Add(this.CB10);
            this.Controls.Add(this.CB9);
            this.Controls.Add(this.CB8);
            this.Controls.Add(this.CB7);
            this.Controls.Add(this.CB1);
            this.Controls.Add(this.CB6);
            this.Controls.Add(this.CB5);
            this.Controls.Add(this.CB4);
            this.Controls.Add(this.CB3);
            this.Controls.Add(this.CB2);
            this.Controls.Add(this.BSave);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ProfileChangerUI";
            this.Text = "Profile Changer";
            this.Load += new System.EventHandler(this.ProfileChangerUI_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button BSave;
        private System.Windows.Forms.CheckBox CB1; 
        private System.Windows.Forms.CheckBox CB2;
        private System.Windows.Forms.CheckBox CB3;
        private System.Windows.Forms.CheckBox CB4;
        private System.Windows.Forms.CheckBox CB5;
        private System.Windows.Forms.CheckBox CB6;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label15;
        private CheckBox CB7;
        private CheckBox CB8;
        private CheckBox CB9;
        private CheckBox CB10;
        private CheckBox CB11;
        private CheckBox CB12;
        private TextBox tb1;
        private TextBox tb2;
        private TextBox tb3;
        private TextBox tb4;
        private TextBox tb5;
        private TextBox tb6;
        private TextBox tb7;
        private TextBox tb8;
        private TextBox tb9;
        private TextBox tb11;
        private TextBox tb10;
        private TextBox tb12;
        private OpenFileDialog openFileDialog1;
        private Button DGb;
        private TextBox DG;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label label16;
    }
}

