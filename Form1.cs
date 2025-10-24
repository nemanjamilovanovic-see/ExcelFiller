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
                        // Nova šema: C/D/E su datumi, ostalo pomereno udesno.
                        string modulCell = ws.Cells[i, 1].Text.Trim(); // A
                        string idVerzijaCell = ws.Cells[i, 2].Text.Trim(); // B
                        string datumTestCell = ws.Cells[i, 4].Text.Trim(); // D: Datum testiranja
                        string datumProdukcioneCell = ws.Cells[i, 5].Text.Trim(); // E: Datum produkcione
                        DateTime datumTest, datumProdukcione;
                        DateTime? maxDatum = null;
                        if (modulCell == excelNazivModula)
                        {
                            if (DateTime.TryParseExact(datumTestCell, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out datumTest))
                                maxDatum = datumTest;
                            if (DateTime.TryParseExact(datumProdukcioneCell, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out datumProdukcione))
                                if (!maxDatum.HasValue || datumProdukcione > maxDatum) maxDatum = datumProdukcione;
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
            string datumProdukcione = dateTimePickerPonovnaTest.CustomFormat != " " ? dateTimePickerPonovnaTest.Value.ToString("dd.MM.yyyy") : "";

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
                        // Nova šema bez RB i sa datumima u C/D/E
                        ws.Cells[1, 1].Value = "Modul (aplikacija)";                 // A
                        ws.Cells[1, 2].Value = "ID verzija";                         // B
                        ws.Cells[1, 3].Value = "Datum prijema verzije";              // C
                        ws.Cells[1, 4].Value = "Datum spuštenja na test";            // D
                        ws.Cells[1, 5].Value = "Datum produkcione instalacije";   // E
                        ws.Cells[1, 6].Value = "Broj zahteva /incidenta Asseco";     // F
                        ws.Cells[1, 7].Value = "Broj zahteva /incidenta NLBKB";      // G
                        ws.Cells[1, 8].Value = "Server";                             // H
                        ws.Cells[1, 9].Value = "Baza";                               // I
                        ws.Cells[1, 10].Value = "Spisak ID-jeva u verziji";          // J
                        ws.Cells[1, 11].Value = "Redosled puštanja";                 // K
                        ws.Cells[1, 12].Value = "BA odgovorno lice";                 // L
                        ws.Cells[1, 13].Value = "Napomena";                          // M
                    }
                    else
                    {
                        ws = package.Workbook.Worksheets[0];
                    }
                    int lastRow = ws.Dimension?.End.Row ?? 1;
                    int newRow = lastRow + 1;
                    ws.Cells[newRow, 1].Value = modul; // A
                    ws.Cells[newRow, 2].Value = idVerzija; // B
                    ws.Cells[newRow, 3].Value = datumPrijem; // C
                    ws.Cells[newRow, 4].Value = datumTest; // D
                    ws.Cells[newRow, 5].Value = string.IsNullOrEmpty(datumProdukcione) ? null : datumProdukcione; // E
                    ws.Cells[newRow, 6].Value = asseco; // F
                    ws.Cells[newRow, 7].Value = nlbkb; // G
                    ws.Cells[newRow, 8].Value = string.IsNullOrEmpty(server) ? null : server; // H
                    ws.Cells[newRow, 9].Value = string.IsNullOrEmpty(baza) ? null : baza; // I
                    ws.Cells[newRow, 10].Value = string.IsNullOrEmpty(spisakIdJeva) ? null : spisakIdJeva; // J
                    ws.Cells[newRow, 11].Value = string.IsNullOrEmpty(redosled) ? null : redosled; // K
                    ws.Cells[newRow, 12].Value = odgovornoLice; // L
                    ws.Cells[newRow, 13].Value = string.IsNullOrEmpty(napomena) ? null : napomena; // M
                    package.Save();
                }
                MessageBox.Show("Podaci su uspešno upisani u Excel!", "Uspeh", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Excel fajl nije moguće upisati. Najčešći razlog je da je fajl otvoren na drugom računaru ili programu, ili nemate dozvolu za upis. Zatvorite fajl 'Izvestaj.xlsx' na deljenoj lokaciji i pokušajte ponovo.", "Upozorenje: Excel fajl nije dostupan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void comboBoxModul_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
