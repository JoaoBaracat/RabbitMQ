namespace ClientUI
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
            this.getWeatherButton = new System.Windows.Forms.Button();
            this.getTimeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // getWeatherButton
            // 
            this.getWeatherButton.Location = new System.Drawing.Point(53, 44);
            this.getWeatherButton.Name = "getWeatherButton";
            this.getWeatherButton.Size = new System.Drawing.Size(162, 23);
            this.getWeatherButton.TabIndex = 0;
            this.getWeatherButton.Text = "Get Weather";
            this.getWeatherButton.UseVisualStyleBackColor = true;
            this.getWeatherButton.Click += new System.EventHandler(this.getWeatherButton_Click);
            // 
            // getTimeButton
            // 
            this.getTimeButton.Location = new System.Drawing.Point(53, 73);
            this.getTimeButton.Name = "getTimeButton";
            this.getTimeButton.Size = new System.Drawing.Size(162, 23);
            this.getTimeButton.TabIndex = 1;
            this.getTimeButton.Text = "Get Time";
            this.getTimeButton.UseVisualStyleBackColor = true;
            this.getTimeButton.Click += new System.EventHandler(this.getTimeButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(678, 367);
            this.Controls.Add(this.getTimeButton);
            this.Controls.Add(this.getWeatherButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button getWeatherButton;
        private System.Windows.Forms.Button getTimeButton;
    }
}

