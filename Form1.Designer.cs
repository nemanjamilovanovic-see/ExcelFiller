namespace ExcelFiller
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            comboBoxModul = new ComboBox();
            textBoxIDVerzija = new TextBox();
            textBoxAsseco = new TextBox();
            textBoxNLBKB = new TextBox();
            dateTimePickerPrijem = new DateTimePicker();
            dateTimePickerTest = new DateTimePicker();
            dateTimePickerPonovnaTest = new DateTimePicker();
            textBoxRDM = new TextBox();
            textBoxRedosled = new TextBox();
            textBoxOdgovornoLice = new TextBox();
            textBoxNapomena = new TextBox();
            buttonDodaj = new Button();
            labelModul = new Label();
            labelIDVerzija = new Label();
            labelAsseco = new Label();
            labelNLBKB = new Label();
            labelPrijem = new Label();
            labelTest = new Label();
            labelPonovnaTest = new Label();
            labelRDM = new Label();
            labelRedosled = new Label();
            labelOdgovornoLice = new Label();
            labelNapomena = new Label();
            SuspendLayout();
            // 
            // comboBoxModul
            // 
            comboBoxModul.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxModul.FormattingEnabled = true;
            comboBoxModul.Items.AddRange(new object[] {
                "Glavna Knjiga",
                "Kreditno depozitni poslovi, uključujući i Faktoring",
                "Devizno poslovanje, uključujući FGW",
                "Poslovanje sa stanovništvom, trezor",
                "Domaći platni promet",
                "Kartično poslovanje",
                "Product Delivery",
                "iBank (inHouse, web)",
                "  - iBank - DB isporuke",
                "  - AdapterPlugInCMS",
                "  - AdapterPlugInRT",
                "  - CMSAdapter",
                "  - DEAuthenticRs",
                "  - DEWebAdminP",
                "  - DEWebApiP",
                "  - DEWebRemote",
                "  - DigitalBranchAPI",
                "  - EmailService",
                "  - FPSAdapter",
                "  - FxWeb2012Kombank",
                "  - HalcomNotifProxy",
                "  - KCIntegrationSvc",
                "  - PaymAdapt",
                "  - RemoteSigningManSrv",
                "  - SysIntAPI",
                "  - SysIntegrationAPI",
                "  - SysIntLib",
                "  - SysIntLib-KoBBg",
                "  - SysIntLibV2KBBG",
                "  - SysIntServiceKBBGse",
                "  - SysMngrKBBG-iBank",
                "  - SysMngrWSKBBG-iBank",
                "  - SysMngrWS-KOBBg",
                "  - SysMngrWSProxyKBBG",
                "  - SystemManagerWS",
                "  - SystemManagerWSProxy",
                "  - TanGeneratorKOBBG",
                "Critesys",
                "IPS-PGW",
                "Online Faktoring (SC4F)",
                "BC API",
                "Calculation servisi"
            });
            comboBoxModul.Location = new Point(300, 25);
            comboBoxModul.Name = "comboBoxModul";
            comboBoxModul.Size = new Size(250, 33);
            comboBoxModul.TabIndex = 0;
            // 
            // textBoxIDVerzija
            // 
            textBoxIDVerzija.Location = new Point(300, 65);
            textBoxIDVerzija.Name = "textBoxIDVerzija";
            textBoxIDVerzija.Size = new Size(250, 31);
            textBoxIDVerzija.TabIndex = 1;
            // 
            // textBoxAsseco
            // 
            textBoxAsseco.Location = new Point(300, 105);
            textBoxAsseco.Name = "textBoxAsseco";
            textBoxAsseco.Size = new Size(250, 31);
            textBoxAsseco.TabIndex = 2;
            // 
            // textBoxNLBKB
            // 
            textBoxNLBKB.Location = new Point(300, 145);
            textBoxNLBKB.Name = "textBoxNLBKB";
            textBoxNLBKB.Size = new Size(250, 31);
            textBoxNLBKB.TabIndex = 3;
            // 
            // dateTimePickerPrijem
            // 
            dateTimePickerPrijem.Format = DateTimePickerFormat.Custom;
            dateTimePickerPrijem.CustomFormat = "dd.MM.yyyy";
            dateTimePickerPrijem.Location = new Point(300, 185);
            dateTimePickerPrijem.Name = "dateTimePickerPrijem";
            dateTimePickerPrijem.Size = new Size(250, 31);
            dateTimePickerPrijem.TabIndex = 4;
            // 
            // dateTimePickerTest
            // 
            dateTimePickerTest.Format = DateTimePickerFormat.Custom;
            dateTimePickerTest.CustomFormat = "dd.MM.yyyy";
            dateTimePickerTest.Location = new Point(300, 225);
            dateTimePickerTest.Name = "dateTimePickerTest";
            dateTimePickerTest.Size = new Size(250, 31);
            dateTimePickerTest.TabIndex = 5;
            // 
            // dateTimePickerPonovnaTest
            // 
            dateTimePickerPonovnaTest.Format = DateTimePickerFormat.Custom;
            dateTimePickerPonovnaTest.CustomFormat = " "; // prazno po defaultu
            dateTimePickerPonovnaTest.Location = new Point(300, 265);
            dateTimePickerPonovnaTest.Name = "dateTimePickerPonovnaTest";
            dateTimePickerPonovnaTest.Size = new Size(250, 31);
            dateTimePickerPonovnaTest.TabIndex = 6;
            // 
            // textBoxRDM
            // 
            textBoxRDM.Location = new Point(300, 305);
            textBoxRDM.Name = "textBoxRDM";
            textBoxRDM.Size = new Size(250, 31);
            textBoxRDM.TabIndex = 7;
            // 
            // textBoxRedosled
            // 
            textBoxRedosled.Location = new Point(300, 345);
            textBoxRedosled.Name = "textBoxRedosled";
            textBoxRedosled.Size = new Size(250, 31);
            textBoxRedosled.TabIndex = 8;
            // 
            // textBoxOdgovornoLice
            // 
            textBoxOdgovornoLice.Location = new Point(300, 385);
            textBoxOdgovornoLice.Name = "textBoxOdgovornoLice";
            textBoxOdgovornoLice.Size = new Size(250, 31);
            textBoxOdgovornoLice.TabIndex = 9;
            // 
            // textBoxNapomena
            // 
            textBoxNapomena.Location = new Point(300, 425);
            textBoxNapomena.Name = "textBoxNapomena";
            textBoxNapomena.Size = new Size(250, 31);
            textBoxNapomena.TabIndex = 10;
            // 
            // buttonDodaj
            // 
            buttonDodaj.Location = new Point(300, 475);
            buttonDodaj.Name = "buttonDodaj";
            buttonDodaj.Size = new Size(250, 40);
            buttonDodaj.TabIndex = 11;
            buttonDodaj.Text = "Dodaj u Excel";
            buttonDodaj.UseVisualStyleBackColor = true;
            buttonDodaj.Click += buttonDodaj_Click;
            // 
            // labelModul
            // 
            labelModul.AutoSize = true;
            labelModul.Font = new Font("Segoe UI", 9F);
            labelModul.Location = new Point(30, 30);
            labelModul.Name = "labelModul";
            labelModul.Size = new Size(72, 25);
            labelModul.TabIndex = 12;
            labelModul.Text = "Modul*";
            // 
            // labelIDVerzija
            // 
            labelIDVerzija.AutoSize = true;
            labelIDVerzija.Font = new Font("Segoe UI", 9F);
            labelIDVerzija.Location = new Point(30, 70);
            labelIDVerzija.Name = "labelIDVerzija";
            labelIDVerzija.Size = new Size(92, 25);
            labelIDVerzija.TabIndex = 13;
            labelIDVerzija.Text = "ID verzije*";
            // 
            // labelAsseco
            // 
            labelAsseco.AutoSize = true;
            labelAsseco.Font = new Font("Segoe UI", 9F);
            labelAsseco.Location = new Point(30, 110);
            labelAsseco.Name = "labelAsseco";
            labelAsseco.Size = new Size(166, 25);
            labelAsseco.TabIndex = 14;
            labelAsseco.Text = "Br. zahteva Asseco*";
            // 
            // labelNLBKB
            // 
            labelNLBKB.AutoSize = true;
            labelNLBKB.Font = new Font("Segoe UI", 9F);
            labelNLBKB.Location = new Point(30, 150);
            labelNLBKB.Name = "labelNLBKB";
            labelNLBKB.Size = new Size(161, 25);
            labelNLBKB.TabIndex = 15;
            labelNLBKB.Text = "Br. zahteva NLBKB*";
            // 
            // labelPrijem
            // 
            labelPrijem.AutoSize = true;
            labelPrijem.Font = new Font("Segoe UI", 9F);
            labelPrijem.Location = new Point(30, 190);
            labelPrijem.Name = "labelPrijem";
            labelPrijem.Size = new Size(138, 25);
            labelPrijem.TabIndex = 16;
            labelPrijem.Text = "Datum prijema*";
            // 
            // labelTest
            // 
            labelTest.AutoSize = true;
            labelTest.Font = new Font("Segoe UI", 9F);
            labelTest.Location = new Point(30, 230);
            labelTest.Name = "labelTest";
            labelTest.Size = new Size(150, 25);
            labelTest.TabIndex = 17;
            labelTest.Text = "Datum testiranja*";
            // 
            // labelPonovnaTest
            // 
            labelPonovnaTest.AutoSize = true;
            labelPonovnaTest.Font = new Font("Segoe UI", 9F);
            labelPonovnaTest.Location = new Point(30, 270);
            labelPonovnaTest.Name = "labelPonovnaTest";
            labelPonovnaTest.Size = new Size(182, 25);
            labelPonovnaTest.TabIndex = 18;
            labelPonovnaTest.Text = "Datum pon. testne instalacije";
            // 
            // labelRDM
            // 
            labelRDM.AutoSize = true;
            labelRDM.Font = new Font("Segoe UI", 9F);
            labelRDM.Location = new Point(30, 310);
            labelRDM.Name = "labelRDM";
            labelRDM.Size = new Size(52, 25);
            labelRDM.TabIndex = 19;
            labelRDM.Text = "RDM";
            // 
            // labelRedosled
            // 
            labelRedosled.AutoSize = true;
            labelRedosled.Font = new Font("Segoe UI", 9F);
            labelRedosled.Location = new Point(30, 350);
            labelRedosled.Name = "labelRedosled";
            labelRedosled.Size = new Size(118, 25);
            labelRedosled.TabIndex = 20;
            labelRedosled.Text = "Redosled puštanja";
            // 
            // labelOdgovornoLice
            // 
            labelOdgovornoLice.AutoSize = true;
            labelOdgovornoLice.Font = new Font("Segoe UI", 9F);
            labelOdgovornoLice.Location = new Point(30, 390);
            labelOdgovornoLice.Name = "labelOdgovornoLice";
            labelOdgovornoLice.Size = new Size(72, 25);
            labelOdgovornoLice.TabIndex = 21;
            labelOdgovornoLice.Text = "BA odgovorno lice*";
            // 
            // labelNapomena
            // 
            labelNapomena.AutoSize = true;
            labelNapomena.Font = new Font("Segoe UI", 9F);
            labelNapomena.Location = new Point(30, 430);
            labelNapomena.Name = "labelNapomena";
            labelNapomena.Size = new Size(100, 25);
            labelNapomena.TabIndex = 22;
            labelNapomena.Text = "Napomena";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(588, 554);
            Controls.Add(comboBoxModul);
            Controls.Add(textBoxIDVerzija);
            Controls.Add(textBoxAsseco);
            Controls.Add(textBoxNLBKB);
            Controls.Add(dateTimePickerPrijem);
            Controls.Add(dateTimePickerTest);
            Controls.Add(dateTimePickerPonovnaTest);
            Controls.Add(textBoxRDM);
            Controls.Add(textBoxRedosled);
            Controls.Add(textBoxOdgovornoLice);
            Controls.Add(textBoxNapomena);
            Controls.Add(buttonDodaj);
            Controls.Add(labelModul);
            Controls.Add(labelIDVerzija);
            Controls.Add(labelAsseco);
            Controls.Add(labelNLBKB);
            Controls.Add(labelPrijem);
            Controls.Add(labelTest);
            Controls.Add(labelPonovnaTest);
            Controls.Add(labelRDM);
            Controls.Add(labelRedosled);
            Controls.Add(labelOdgovornoLice);
            Controls.Add(labelNapomena);
            Name = "Form1";
            Text = "Excel Filler";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.ComboBox comboBoxModul;
        private System.Windows.Forms.TextBox textBoxIDVerzija;
        private System.Windows.Forms.TextBox textBoxAsseco;
        private System.Windows.Forms.TextBox textBoxNLBKB;
        private System.Windows.Forms.DateTimePicker dateTimePickerPrijem;
        private System.Windows.Forms.DateTimePicker dateTimePickerTest;
        private System.Windows.Forms.DateTimePicker dateTimePickerPonovnaTest;
        private System.Windows.Forms.TextBox textBoxRDM;
        private System.Windows.Forms.TextBox textBoxRedosled;
        private System.Windows.Forms.TextBox textBoxOdgovornoLice;
        private System.Windows.Forms.TextBox textBoxNapomena;
        private System.Windows.Forms.Button buttonDodaj;
        private System.Windows.Forms.Label labelModul;
        private System.Windows.Forms.Label labelIDVerzija;
        private System.Windows.Forms.Label labelAsseco;
        private System.Windows.Forms.Label labelNLBKB;
        private System.Windows.Forms.Label labelPrijem;
        private System.Windows.Forms.Label labelTest;
        private System.Windows.Forms.Label labelPonovnaTest;
        private System.Windows.Forms.Label labelRDM;
        private System.Windows.Forms.Label labelRedosled;
        private System.Windows.Forms.Label labelOdgovornoLice;
        private System.Windows.Forms.Label labelNapomena;
    }
}
