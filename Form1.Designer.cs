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
            tabControl = new TabControl();
            tabPageUnos = new TabPage();
            comboBoxModul = new ComboBox();
            textBoxIDVerzija = new TextBox();
            textBoxAsseco = new TextBox();
            textBoxNLBKB = new TextBox();
            dateTimePickerPrijem = new DateTimePicker();
            dateTimePickerTest = new DateTimePicker();
            dateTimePickerPonovnaTest = new DateTimePicker();
            textBoxServer = new TextBox();
            labelBaza = new Label();
            textBoxBaza = new TextBox();
            labelSpisakID = new Label();
            textBoxSpisakID = new TextBox();
            textBoxRedosled = new TextBox();
            comboBoxOdgovornoLice = new ComboBox();
            textBoxNapomena = new TextBox();
            labelPutanjaIsporuke = new Label();
            textBoxPutanjaIsporuke = new TextBox();
            buttonDodaj = new Button();
            labelModul = new Label();
            labelIDVerzija = new Label();
            labelAsseco = new Label();
            labelNLBKB = new Label();
            labelPrijem = new Label();
            labelTest = new Label();
            labelPonovnaTest = new Label();
            labelServer = new Label();
            labelRedosled = new Label();
            labelOdgovornoLice = new Label();
            labelNapomena = new Label();
            tabPagePretraga = new TabPage();
            comboBoxPretragaModul = new ComboBox();
            buttonPretrazi = new Button();
            labelRezultat = new Label();
            tabControl.SuspendLayout();
            tabPageUnos.SuspendLayout();
            tabPagePretraga.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabPageUnos);
            tabControl.Controls.Add(tabPagePretraga);
            tabControl.Location = new Point(0, 0);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(588, 720);
            tabControl.TabIndex = 100;
            // 
            // tabPageUnos
            // 
            tabPageUnos.Controls.Add(comboBoxModul);
            tabPageUnos.Controls.Add(textBoxIDVerzija);
            tabPageUnos.Controls.Add(textBoxAsseco);
            tabPageUnos.Controls.Add(textBoxNLBKB);
            tabPageUnos.Controls.Add(dateTimePickerPrijem);
            tabPageUnos.Controls.Add(dateTimePickerTest);
            tabPageUnos.Controls.Add(dateTimePickerPonovnaTest);
            tabPageUnos.Controls.Add(textBoxServer);
            tabPageUnos.Controls.Add(textBoxRedosled);
            tabPageUnos.Controls.Add(comboBoxOdgovornoLice);
            tabPageUnos.Controls.Add(textBoxNapomena);
            tabPageUnos.Controls.Add(labelPutanjaIsporuke);
            tabPageUnos.Controls.Add(textBoxPutanjaIsporuke);
            tabPageUnos.Controls.Add(buttonDodaj);
            tabPageUnos.Controls.Add(labelModul);
            tabPageUnos.Controls.Add(labelIDVerzija);
            tabPageUnos.Controls.Add(labelAsseco);
            tabPageUnos.Controls.Add(labelNLBKB);
            tabPageUnos.Controls.Add(labelPrijem);
            tabPageUnos.Controls.Add(labelTest);
            tabPageUnos.Controls.Add(labelPonovnaTest);
            tabPageUnos.Controls.Add(labelServer);
            tabPageUnos.Controls.Add(labelBaza);
            tabPageUnos.Controls.Add(textBoxBaza);
            tabPageUnos.Controls.Add(labelSpisakID);
            tabPageUnos.Controls.Add(textBoxSpisakID);
            tabPageUnos.Controls.Add(labelRedosled);
            tabPageUnos.Controls.Add(labelOdgovornoLice);
            tabPageUnos.Controls.Add(labelNapomena);
            tabPageUnos.Location = new Point(4, 34);
            tabPageUnos.Name = "tabPageUnos";
            tabPageUnos.Size = new Size(580, 682);
            tabPageUnos.TabIndex = 0;
            tabPageUnos.Text = "Unos";
            tabPageUnos.UseVisualStyleBackColor = true;
            // 
            // comboBoxModul
            // 
            comboBoxModul.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxModul.FormattingEnabled = true;
            comboBoxModul.Items.AddRange(new object[] { "GK", "KDP", "DevPP", "CMS", "Trezor", "CS", "PD", "PP", "RiskManag", "iBank", "PGW", "BankaP", "BCAPI", "FGW" });
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
            dateTimePickerPrijem.CustomFormat = "dd.MM.yyyy";
            dateTimePickerPrijem.Format = DateTimePickerFormat.Custom;
            dateTimePickerPrijem.Location = new Point(300, 185);
            dateTimePickerPrijem.Name = "dateTimePickerPrijem";
            dateTimePickerPrijem.Size = new Size(250, 31);
            dateTimePickerPrijem.TabIndex = 4;
            // 
            // dateTimePickerTest
            // 
            dateTimePickerTest.CustomFormat = "dd.MM.yyyy";
            dateTimePickerTest.Format = DateTimePickerFormat.Custom;
            dateTimePickerTest.Location = new Point(300, 225);
            dateTimePickerTest.Name = "dateTimePickerTest";
            dateTimePickerTest.Size = new Size(250, 31);
            dateTimePickerTest.TabIndex = 5;
            // 
            // dateTimePickerPonovnaTest
            // 
            dateTimePickerPonovnaTest.CustomFormat = " ";
            dateTimePickerPonovnaTest.Format = DateTimePickerFormat.Custom;
            dateTimePickerPonovnaTest.Location = new Point(300, 265);
            dateTimePickerPonovnaTest.Name = "dateTimePickerPonovnaTest";
            dateTimePickerPonovnaTest.Size = new Size(250, 31);
            dateTimePickerPonovnaTest.TabIndex = 6;
            // 
            // textBoxServer
            // 
            textBoxServer.Location = new Point(300, 305);
            textBoxServer.Name = "textBoxServer";
            textBoxServer.Size = new Size(250, 31);
            textBoxServer.TabIndex = 7;

            // labelBaza
            //
            labelBaza.AutoSize = true;
            labelBaza.Font = new Font("Segoe UI", 9F);
            labelBaza.Location = new Point(30, 350);
            labelBaza.Name = "labelBaza";
            labelBaza.Size = new Size(66, 25);
            labelBaza.TabIndex = 19;
            labelBaza.Text = "Baza";

            // textBoxBaza
            //
            textBoxBaza.Location = new Point(300, 345);
            textBoxBaza.Name = "textBoxBaza";
            textBoxBaza.Size = new Size(250, 31);
            textBoxBaza.TabIndex = 8;

            // labelSpisakID
            //
            labelSpisakID.AutoSize = true;
            labelSpisakID.ForeColor = Color.Red;
            labelSpisakID.Font = new Font("Segoe UI", 9F);
            labelSpisakID.Location = new Point(30, 390);
            labelSpisakID.Name = "labelSpisakID";
            labelSpisakID.Size = new Size(197, 25);
            labelSpisakID.TabIndex = 20;
            labelSpisakID.Text = "Broj zahteva u CA alatu*";

            // textBoxSpisakID
            //
            textBoxSpisakID.Location = new Point(300, 385);
            textBoxSpisakID.Name = "textBoxSpisakID";
            textBoxSpisakID.Size = new Size(250, 31);
            textBoxSpisakID.TabIndex = 9;
            // 
            // textBoxRedosled
            // 
            textBoxRedosled.Location = new Point(300, 425);
            textBoxRedosled.Name = "textBoxRedosled";
            textBoxRedosled.Size = new Size(250, 31);
            textBoxRedosled.TabIndex = 10;
            // 
            // comboBoxOdgovornoLice
            // 
            comboBoxOdgovornoLice.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxOdgovornoLice.FormattingEnabled = true;
            comboBoxOdgovornoLice.Items.AddRange(new object[] { "Nemanja Milovanovic", "Milica Madic", "Luka Stanojcic", "Nikola Dokmanovic", "Nenad Jurisin" });
            comboBoxOdgovornoLice.Location = new Point(300, 465);
            comboBoxOdgovornoLice.Name = "comboBoxOdgovornoLice";
            comboBoxOdgovornoLice.Size = new Size(250, 33);
            comboBoxOdgovornoLice.TabIndex = 11;
            // 
            // textBoxNapomena
            // 
            textBoxNapomena.Location = new Point(300, 505);
            textBoxNapomena.Name = "textBoxNapomena";
            textBoxNapomena.Size = new Size(250, 31);
            textBoxNapomena.TabIndex = 12;

            // labelPutanjaIsporuke
            //
            labelPutanjaIsporuke.AutoSize = true;
            labelPutanjaIsporuke.ForeColor = Color.Red;
            labelPutanjaIsporuke.Font = new Font("Segoe UI", 9F);
            labelPutanjaIsporuke.Location = new Point(30, 545);
            labelPutanjaIsporuke.Name = "labelPutanjaIsporuke";
            labelPutanjaIsporuke.Size = new Size(153, 25);
            labelPutanjaIsporuke.TabIndex = 27;
            labelPutanjaIsporuke.Text = "Putanja isporuke*";

            // textBoxPutanjaIsporuke
            //
            textBoxPutanjaIsporuke.Location = new Point(300, 545);
            textBoxPutanjaIsporuke.Name = "textBoxPutanjaIsporuke";
            textBoxPutanjaIsporuke.Size = new Size(250, 31);
            textBoxPutanjaIsporuke.TabIndex = 13;
            // 
            // buttonDodaj
            // 
            buttonDodaj.Location = new Point(300, 605);
            buttonDodaj.Name = "buttonDodaj";
            buttonDodaj.Size = new Size(250, 40);
            buttonDodaj.TabIndex = 14;
            buttonDodaj.Text = "Dodaj u Excel";
            buttonDodaj.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            buttonDodaj.UseVisualStyleBackColor = true;
            buttonDodaj.Click += buttonDodaj_Click;
            // 
            // labelModul
            // 
            labelModul.AutoSize = true;
            labelModul.Font = new Font("Segoe UI", 9F);
            labelModul.ForeColor = Color.Red;
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
            labelIDVerzija.ForeColor = Color.Red;
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
            labelAsseco.ForeColor = Color.Red;
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
            labelNLBKB.ForeColor = Color.Red;
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
            labelPrijem.ForeColor = Color.Red;
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
            labelTest.ForeColor = Color.Red;
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
            labelPonovnaTest.Size = new Size(240, 25);
            labelPonovnaTest.TabIndex = 18;
            labelPonovnaTest.Text = "Datum produkcione instalacije";
            // 
            // labelServer
            // 
            labelServer.AutoSize = true;
            labelServer.Font = new Font("Segoe UI", 9F);
            labelServer.ForeColor = Color.Red;
            labelServer.Location = new Point(30, 310);
            labelServer.Name = "labelServer";
            labelServer.Size = new Size(72, 25);
            labelServer.TabIndex = 19;
            labelServer.Text = "Server*";
            // 
            // labelRedosled
            // 
            labelRedosled.AutoSize = true;
            labelRedosled.Font = new Font("Segoe UI", 9F);
            labelRedosled.Location = new Point(30, 430);
            labelRedosled.Name = "labelRedosled";
            labelRedosled.Size = new Size(157, 25);
            labelRedosled.TabIndex = 20;
            labelRedosled.Text = "Redosled puštanja";
            // 
            // labelOdgovornoLice
            // 
            labelOdgovornoLice.AutoSize = true;
            labelOdgovornoLice.Font = new Font("Segoe UI", 9F);
            labelOdgovornoLice.ForeColor = Color.Red;
            labelOdgovornoLice.Location = new Point(30, 470);
            labelOdgovornoLice.Name = "labelOdgovornoLice";
            labelOdgovornoLice.Size = new Size(168, 25);
            labelOdgovornoLice.TabIndex = 21;
            labelOdgovornoLice.Text = "BA odgovorno lice*";
            // 
            // labelNapomena
            // 
            labelNapomena.AutoSize = true;
            labelNapomena.Font = new Font("Segoe UI", 9F);
            labelNapomena.Location = new Point(30, 510);
            labelNapomena.Name = "labelNapomena";
            labelNapomena.Size = new Size(100, 25);
            labelNapomena.TabIndex = 22;
            labelNapomena.Text = "Napomena";
            // 
            // tabPagePretraga
            // 
            tabPagePretraga.Controls.Add(comboBoxPretragaModul);
            tabPagePretraga.Controls.Add(buttonPretrazi);
            tabPagePretraga.Controls.Add(labelRezultat);
            tabPagePretraga.Location = new Point(4, 34);
            tabPagePretraga.Name = "tabPagePretraga";
            tabPagePretraga.Size = new Size(580, 682);
            tabPagePretraga.TabIndex = 1;
            tabPagePretraga.Text = "Pretraga";
            tabPagePretraga.UseVisualStyleBackColor = true;
            // 
            // comboBoxPretragaModul
            // 
            comboBoxPretragaModul.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxPretragaModul.FormattingEnabled = true;
            comboBoxPretragaModul.Items.AddRange(new object[] { "GK", "KDP", "DevPP", "CMS", "Trezor", "CS", "PD", "PP", "RiskManag", "iBank", "PGW", "BankaP", "BCAPI", "FGW" });
            comboBoxPretragaModul.Location = new Point(40, 40);
            comboBoxPretragaModul.Name = "comboBoxPretragaModul";
            comboBoxPretragaModul.Size = new Size(300, 33);
            comboBoxPretragaModul.TabIndex = 0;
            // 
            // buttonPretrazi
            // 
            buttonPretrazi.Location = new Point(200, 107);
            buttonPretrazi.Name = "buttonPretrazi";
            buttonPretrazi.Size = new Size(140, 40);
            buttonPretrazi.TabIndex = 2;
            buttonPretrazi.Text = "Pretraži";
            buttonPretrazi.UseVisualStyleBackColor = true;
            // 
            // labelRezultat
            // 
            labelRezultat.Location = new Point(40, 140);
            labelRezultat.Name = "labelRezultat";
            labelRezultat.Size = new Size(450, 60);
            labelRezultat.TabIndex = 3;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(588, 720);
            Controls.Add(tabControl);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "Form1";
            Text = "Excel Filler";
            tabControl.ResumeLayout(false);
            tabPageUnos.ResumeLayout(false);
            tabPageUnos.PerformLayout();
            tabPagePretraga.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageUnos;
        private System.Windows.Forms.TabPage tabPagePretraga;
        private System.Windows.Forms.ComboBox comboBoxModul;
        private System.Windows.Forms.ComboBox comboBoxPretragaModul;
        private System.Windows.Forms.Button buttonPretrazi;
        private System.Windows.Forms.Label labelRezultat;
        private System.Windows.Forms.TextBox textBoxIDVerzija;
        private System.Windows.Forms.TextBox textBoxAsseco;
        private System.Windows.Forms.TextBox textBoxNLBKB;
        private System.Windows.Forms.DateTimePicker dateTimePickerPrijem;
        private System.Windows.Forms.DateTimePicker dateTimePickerTest;
        private System.Windows.Forms.DateTimePicker dateTimePickerPonovnaTest;
    private System.Windows.Forms.TextBox textBoxServer;
    private System.Windows.Forms.Label labelBaza;
    private System.Windows.Forms.TextBox textBoxBaza;
    private System.Windows.Forms.Label labelSpisakID;
    private System.Windows.Forms.TextBox textBoxSpisakID;
        private System.Windows.Forms.TextBox textBoxRedosled;
        private System.Windows.Forms.ComboBox comboBoxOdgovornoLice;
        private System.Windows.Forms.TextBox textBoxNapomena;
        private System.Windows.Forms.Label labelPutanjaIsporuke;
        private System.Windows.Forms.TextBox textBoxPutanjaIsporuke;
        private System.Windows.Forms.Button buttonDodaj;
        private System.Windows.Forms.Label labelModul;
        private System.Windows.Forms.Label labelIDVerzija;
        private System.Windows.Forms.Label labelAsseco;
        private System.Windows.Forms.Label labelNLBKB;
        private System.Windows.Forms.Label labelPrijem;
        private System.Windows.Forms.Label labelTest;
        private System.Windows.Forms.Label labelPonovnaTest;
    private System.Windows.Forms.Label labelServer;
        private System.Windows.Forms.Label labelRedosled;
        private System.Windows.Forms.Label labelOdgovornoLice;
        private System.Windows.Forms.Label labelNapomena;
    }
}
