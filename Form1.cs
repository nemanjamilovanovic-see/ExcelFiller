using OfficeOpenXml;
namespace ExcelFiller
{
    public partial class Form1 : Form
    {
        // Putanja do deljene Excel datoteke na mrežnoj lokaciji
        private readonly string excelSharedPath = @"\\bss-testcomp01\PrimenaNLB\Izvestaj.xlsx";
        public Form1()
        {
            InitializeComponent();
            // Logika za prazno polje i izbor datuma za dateTimePickerPonovnaTest
            dateTimePickerPonovnaTest.Format = DateTimePickerFormat.Custom;
            dateTimePickerPonovnaTest.CustomFormat = " "; // prazno po defaultu
            dateTimePickerPonovnaTest.ValueChanged += (s, e) =>
            {
                dateTimePickerPonovnaTest.Format = DateTimePickerFormat.Custom;
                dateTimePickerPonovnaTest.CustomFormat = "dd.MM.yyyy";
            };
            dateTimePickerPonovnaTest.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                {
                    dateTimePickerPonovnaTest.CustomFormat = " ";
                }
            };
            buttonPretrazi.Click += buttonPretrazi_Click;
        }
    private void buttonPretrazi_Click(object? sender, EventArgs? e)
        {
            OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            // Pretraga poslednje verzije za izabrani modul (bez izbora datuma)
            string izabraniModul = comboBoxPretragaModul.SelectedItem?.ToString() ?? "";
            if (string.IsNullOrWhiteSpace(izabraniModul))
            {
                labelRezultat.Text = "Izaberite modul ili podmodul!";
                return;
            }
            string excelNazivModula = izabraniModul;
            if (izabraniModul.StartsWith("  - "))
            {
                excelNazivModula = "iBank (inHouse, web) - " + izabraniModul.Trim().Substring(3);
            }
            // Čitanje iz deljene mrežne lokacije
            string excelPath = excelSharedPath;
            if (!System.IO.File.Exists(excelPath))
            {
                labelRezultat.Text = "Excel fajl nije pronađen na deljenoj lokaciji!";
                return;
            }
            try
            {
                using (var package = new OfficeOpenXml.ExcelPackage(new System.IO.FileInfo(excelPath)))
                {
                    var ws = package.Workbook.Worksheets[0];
                    int rowCount = ws.Dimension?.End.Row ?? 1;
                    DateTime? poslednjiDatum = null;
                    string poslednjaVerzija = "Nema podataka za izabrani modul.";
                    for (int i = 2; i <= rowCount; i++)
                    {
                        string modulCell = ws.Cells[i, 2].Text.Trim();
                        string idVerzijaCell = ws.Cells[i, 3].Text.Trim();
                        // Datumi su pomereni na kolone K (11) i L (12)
                        string datumTestCell = ws.Cells[i, 11].Text.Trim();
                        string datumPonTestCell = ws.Cells[i, 12].Text.Trim();
                        DateTime datumTest, datumPonTest;
                        DateTime? maxDatum = null;
                        if (modulCell == excelNazivModula)
                        {
                            if (DateTime.TryParseExact(datumTestCell, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out datumTest))
                                maxDatum = datumTest;
                            if (DateTime.TryParseExact(datumPonTestCell, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out datumPonTest))
                                if (!maxDatum.HasValue || datumPonTest > maxDatum) maxDatum = datumPonTest;
                            if (maxDatum.HasValue && (!poslednjiDatum.HasValue || maxDatum > poslednjiDatum))
                            {
                                poslednjiDatum = maxDatum;
                                poslednjaVerzija = $"Poslednja verzija za {izabraniModul} je: {idVerzijaCell} (datum: {maxDatum:dd.MM.yyyy})";
                            }
                        }
                    }
                    labelRezultat.Text = poslednjaVerzija;
                }
            }
            catch (Exception ex)
            {
                labelRezultat.Text = $"Greška pri pretrazi: {ex.Message}";
            }
    }

        private void buttonDodaj_Click(object sender, EventArgs e)
        {
            // Validacija obaveznih polja
            if (comboBoxModul.SelectedIndex == -1 ||
                string.IsNullOrWhiteSpace(textBoxIDVerzija.Text) ||
                string.IsNullOrWhiteSpace(textBoxAsseco.Text) ||
                string.IsNullOrWhiteSpace(textBoxNLBKB.Text) ||
                string.IsNullOrWhiteSpace(textBoxServer.Text) ||
                string.IsNullOrWhiteSpace(textBoxBaza.Text) ||
                comboBoxOdgovornoLice.SelectedIndex == -1)
            {
                MessageBox.Show("Popunite sva obavezna polja označena zvezdicom (*)!", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Priprema podataka za upis u Excel
            string modul = comboBoxModul.SelectedItem?.ToString() ?? "";
            // Ako je podmodul (pocinje sa "  - "), upisi "iBank (inHouse, web) - NazivPodmodula"
            if (modul.StartsWith("  - "))
            {
                modul = "iBank (inHouse, web) - " + modul.Trim().Substring(3);
            }
            string idVerzija = textBoxIDVerzija.Text.Trim();
            string asseco = textBoxAsseco.Text.Trim();
            string nlbkb = textBoxNLBKB.Text.Trim();
            string server = textBoxServer.Text.Trim(); // vrednost za kolonu "Server" (F)
            string baza = textBoxBaza.Text.Trim();
            string spisakIdJeva = textBoxSpisakID.Text.Trim();
            string redosled = textBoxRedosled.Text.Trim();
            string napomena = textBoxNapomena.Text.Trim();
            string odgovornoLice = comboBoxOdgovornoLice.SelectedItem?.ToString() ?? "";
            string datumPrijem = dateTimePickerPrijem.Value.ToString("dd.MM.yyyy");
            string datumTest = dateTimePickerTest.Value.ToString("dd.MM.yyyy");
            string datumPonovnaTest = dateTimePickerPonovnaTest.CustomFormat != " " ? dateTimePickerPonovnaTest.Value.ToString("dd.MM.yyyy") : "";

            // Upis u Excel fajl koristeći EPPlus
            // Upis u deljenu mrežnu lokaciju
            string excelPath = excelSharedPath;
            string? excelDir = System.IO.Path.GetDirectoryName(excelPath);
            if (string.IsNullOrEmpty(excelDir) || !System.IO.Directory.Exists(excelDir))
            {
                MessageBox.Show("Deljena lokacija nije dostupna: \\bss-testcomp01\\PrimenaNLB", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var fileInfo = new System.IO.FileInfo(excelPath);
            bool noviFajl = !fileInfo.Exists;
            try
            {
                OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                using (var package = new OfficeOpenXml.ExcelPackage(fileInfo))
                {
                    OfficeOpenXml.ExcelWorksheet ws;
                    if (noviFajl)
                    {
                        ws = package.Workbook.Worksheets.Add("Podaci");
                        // Zaglavlja
                        ws.Cells[1, 1].Value = "RB";
                        ws.Cells[1, 2].Value = "Modul (aplikacija)";
                        ws.Cells[1, 3].Value = "ID verzija";
                        ws.Cells[1, 4].Value = "Broj zahteva /incidenta Asseco";
                        ws.Cells[1, 5].Value = "Broj zahteva /incidenta NLBKB";
                        ws.Cells[1, 6].Value = "Server";
                        ws.Cells[1, 7].Value = "Baza"; // NOVO
                        ws.Cells[1, 8].Value = "Spisak ID-jeva u verziji"; // NOVO
                        ws.Cells[1, 9].Value = "Redosled puštanja";
                        ws.Cells[1, 10].Value = "Datum prijema verzije";
                        ws.Cells[1, 11].Value = "Datum spuštenja na test";
                        ws.Cells[1, 12].Value = "Datum ponovne testne instalacije";
                        ws.Cells[1, 13].Value = "BA odgovorno lice";
                        ws.Cells[1, 14].Value = "Napomena";
                    }
                    else
                    {
                        ws = package.Workbook.Worksheets[0];
                    }
                    int lastRow = ws.Dimension?.End.Row ?? 1;
                    int newRow = lastRow + 1;
                    ws.Cells[newRow, 1].Value = newRow - 1; // RB
                    ws.Cells[newRow, 2].Value = modul;
                    ws.Cells[newRow, 3].Value = idVerzija;
                    ws.Cells[newRow, 4].Value = asseco;
                    ws.Cells[newRow, 5].Value = nlbkb;
                    ws.Cells[newRow, 6].Value = string.IsNullOrEmpty(server) ? null : server; // Server (F)
                    ws.Cells[newRow, 7].Value = string.IsNullOrEmpty(baza) ? null : baza; // G
                    ws.Cells[newRow, 8].Value = string.IsNullOrEmpty(spisakIdJeva) ? null : spisakIdJeva; // H
                    ws.Cells[newRow, 9].Value = string.IsNullOrEmpty(redosled) ? null : redosled; // I
                    ws.Cells[newRow, 10].Value = datumPrijem; // J
                    ws.Cells[newRow, 11].Value = datumTest; // K
                    ws.Cells[newRow, 12].Value = string.IsNullOrEmpty(datumPonovnaTest) ? null : datumPonovnaTest; // L
                    ws.Cells[newRow, 13].Value = odgovornoLice; // M
                    ws.Cells[newRow, 14].Value = string.IsNullOrEmpty(napomena) ? null : napomena; // N
                    package.Save();
                }
                MessageBox.Show("Podaci su uspešno upisani u Excel!", "Uspeh", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                if (ex is System.IO.IOException && ex.Message.Contains("because it is being used by another process"))
                {
                    MessageBox.Show("Excel fajl je otvoren. Zatvorite fajl 'Izvestaj.xlsx' na deljenoj lokaciji da bi upis bio moguć.", "Excel fajl je otvoren", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"Greška pri upisu u Excel: {ex.Message}", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void comboBoxModul_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
