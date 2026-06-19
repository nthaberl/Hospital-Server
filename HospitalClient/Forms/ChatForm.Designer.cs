namespace HospitalClient.Forms
{
    partial class ChatForm
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
            this.comboboxRecipient = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.textboxMessage = new System.Windows.Forms.TextBox();
            this.buttonSend = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonExit = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboboxRecipient
            // 
            this.comboboxRecipient.FormattingEnabled = true;
            this.comboboxRecipient.Location = new System.Drawing.Point(110, 68);
            this.comboboxRecipient.Name = "comboboxRecipient";
            this.comboboxRecipient.Size = new System.Drawing.Size(309, 21);
            this.comboboxRecipient.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select Recipient";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(110, 207);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(323, 134);
            this.listBox1.TabIndex = 2;
            // 
            // textboxMessage
            // 
            this.textboxMessage.Location = new System.Drawing.Point(110, 115);
            this.textboxMessage.Name = "textboxMessage";
            this.textboxMessage.Size = new System.Drawing.Size(309, 20);
            this.textboxMessage.TabIndex = 3;
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(110, 155);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(75, 23);
            this.buttonSend.TabIndex = 4;
            this.buttonSend.Text = "Send";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(54, 118);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Message";
            // 
            // buttonExit
            // 
            this.buttonExit.Location = new System.Drawing.Point(201, 401);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(116, 23);
            this.buttonExit.TabIndex = 6;
            this.buttonExit.Text = "Return to Dashboard";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(106, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(152, 24);
            this.label3.TabIndex = 7;
            this.label3.Text = "Send a Message";
            // 
            // ChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(525, 450);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.textboxMessage);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboboxRecipient);
            this.Name = "ChatForm";
            this.Text = "ChatForm";
            this.Load += new System.EventHandler(this.ChatForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboboxRecipient;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TextBox textboxMessage;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.Label label3;
    }
}