namespace FaceDetection
{
    partial class FaceDetection
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
            this.components = new System.ComponentModel.Container();
            this.btnStart = new System.Windows.Forms.Button();
            this.imgbWebcam = new Emgu.CV.UI.ImageBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.imgbShow = new Emgu.CV.UI.ImageBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtThreshold = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dgvChecked = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.imgbWebcam)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgbShow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChecked)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(902, 12);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(124, 63);
            this.btnStart.TabIndex = 3;
            this.btnStart.Text = "Start detection and recognise";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // imgbWebcam
            // 
            this.imgbWebcam.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imgbWebcam.Location = new System.Drawing.Point(12, 12);
            this.imgbWebcam.Name = "imgbWebcam";
            this.imgbWebcam.Size = new System.Drawing.Size(766, 590);
            this.imgbWebcam.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgbWebcam.TabIndex = 2;
            this.imgbWebcam.TabStop = false;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(897, 203);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(124, 41);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save face";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(871, 167);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(169, 30);
            this.txtName.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(821, 170);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 24);
            this.label1.TabIndex = 6;
            this.label1.Text = "Name";
            // 
            // imgbShow
            // 
            this.imgbShow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imgbShow.Location = new System.Drawing.Point(818, 282);
            this.imgbShow.Name = "imgbShow";
            this.imgbShow.Size = new System.Drawing.Size(287, 258);
            this.imgbShow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgbShow.TabIndex = 7;
            this.imgbShow.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(926, 255);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 24);
            this.label2.TabIndex = 8;
            this.label2.Text = "Face save";
            // 
            // txtThreshold
            // 
            this.txtThreshold.Location = new System.Drawing.Point(871, 107);
            this.txtThreshold.Name = "txtThreshold";
            this.txtThreshold.Size = new System.Drawing.Size(169, 30);
            this.txtThreshold.TabIndex = 9;
            this.txtThreshold.TextChanged += new System.EventHandler(this.txtThreshold_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(798, 113);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 24);
            this.label3.TabIndex = 10;
            this.label3.Text = "Threshold";
            // 
            // dgvChecked
            // 
            this.dgvChecked.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvChecked.Location = new System.Drawing.Point(1126, 12);
            this.dgvChecked.Name = "dgvChecked";
            this.dgvChecked.RowHeadersWidth = 51;
            this.dgvChecked.RowTemplate.Height = 24;
            this.dgvChecked.Size = new System.Drawing.Size(392, 528);
            this.dgvChecked.TabIndex = 11;
            // 
            // FaceDetection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1530, 607);
            this.Controls.Add(this.dgvChecked);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtThreshold);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.imgbShow);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.imgbWebcam);
            this.Controls.Add(this.btnStart);
            this.Font = new System.Drawing.Font("Saysettha OT", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FaceDetection";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.imgbWebcam)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgbShow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChecked)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnStart;
        private Emgu.CV.UI.ImageBox imgbWebcam;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label1;
        private Emgu.CV.UI.ImageBox imgbShow;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtThreshold;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dgvChecked;
    }
}

