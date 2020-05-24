namespace Cinema_VP
{
    partial class TicketDisplay
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
            this.lblHall = new System.Windows.Forms.Label();
            this.lblRow = new System.Windows.Forms.Label();
            this.lblSeat = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblNumber = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblPrice = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblHall
            // 
            this.lblHall.AutoSize = true;
            this.lblHall.BackColor = System.Drawing.SystemColors.Window;
            this.lblHall.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHall.Location = new System.Drawing.Point(55, 157);
            this.lblHall.Name = "lblHall";
            this.lblHall.Size = new System.Drawing.Size(40, 20);
            this.lblHall.TabIndex = 0;
            this.lblHall.Text = "Hall";
            // 
            // lblRow
            // 
            this.lblRow.AutoSize = true;
            this.lblRow.BackColor = System.Drawing.SystemColors.Window;
            this.lblRow.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRow.Location = new System.Drawing.Point(192, 157);
            this.lblRow.Name = "lblRow";
            this.lblRow.Size = new System.Drawing.Size(44, 20);
            this.lblRow.TabIndex = 1;
            this.lblRow.Text = "Row";
            // 
            // lblSeat
            // 
            this.lblSeat.AutoSize = true;
            this.lblSeat.BackColor = System.Drawing.SystemColors.Window;
            this.lblSeat.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSeat.Location = new System.Drawing.Point(320, 157);
            this.lblSeat.Name = "lblSeat";
            this.lblSeat.Size = new System.Drawing.Size(47, 20);
            this.lblSeat.TabIndex = 2;
            this.lblSeat.Text = "Seat";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.BackColor = System.Drawing.SystemColors.Window;
            this.lblTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.Location = new System.Drawing.Point(469, 157);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(47, 20);
            this.lblTime.TabIndex = 3;
            this.lblTime.Text = "Time";
            // 
            // lblNumber
            // 
            this.lblNumber.AutoSize = true;
            this.lblNumber.BackColor = System.Drawing.SystemColors.Window;
            this.lblNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumber.Location = new System.Drawing.Point(244, 298);
            this.lblNumber.Name = "lblNumber";
            this.lblNumber.Size = new System.Drawing.Size(71, 20);
            this.lblNumber.TabIndex = 4;
            this.lblNumber.Text = "Number";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.SystemColors.Window;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(39, 29);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(185, 42);
            this.lblTitle.TabIndex = 5;
            this.lblTitle.Text = "ф и л ь м";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Window;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(41, 255);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 25);
            this.label1.TabIndex = 6;
            this.label1.Text = "Цена";
            // 
            // lblPrice
            // 
            this.lblPrice.AutoSize = true;
            this.lblPrice.BackColor = System.Drawing.SystemColors.Window;
            this.lblPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrice.Location = new System.Drawing.Point(48, 288);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(55, 29);
            this.lblPrice.TabIndex = 7;
            this.lblPrice.Text = "---р";
            // 
            // TicketDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Cinema_VP.Properties.Resources.Ticket_maket;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(742, 346);
            this.Controls.Add(this.lblPrice);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblNumber);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lblSeat);
            this.Controls.Add(this.lblRow);
            this.Controls.Add(this.lblHall);
            this.Name = "TicketDisplay";
            this.Text = "Билет";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblHall;
        private System.Windows.Forms.Label lblRow;
        private System.Windows.Forms.Label lblSeat;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblNumber;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblPrice;
    }
}