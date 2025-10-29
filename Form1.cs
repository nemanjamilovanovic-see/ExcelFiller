using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExcelFiller
{
    public partial class Form1 : Form
    {
        // Shared Excel file path on network
        private readonly string excelSharedPath = @"\\bss-testcomp01\PrimenaNLB\Izvestaj.xlsx";

        // Custom dropdown menus for Server and Baza
        private ContextMenuStrip? menuServers;
        private ContextMenuStrip? menuBaze;

        // In-field arrow buttons
        private Button? btnServersDrop;
        private Button? btnBazeDrop;

    // Visual state stored per-button via closures in CreateArrowButton()

        // Targets for which menu is currently operating
        private Control? serversTarget;
        private Control? bazeTarget;

        // Options
        private readonly string[] serverOptions = new[] { "PEXSTAGE", "PEXIZVSTAGE", "LB", "PEXNSSTAGE", "PreProd" };
        private readonly string[] bazaOptions = new[] { "wbanka_kbg_work", "wbanka_kbg_work_PP" };

        public Form1()
        {
            InitializeComponent();

            // DateTimePicker (optional date) for PonovnaTest
            dateTimePickerPonovnaTest.Format = DateTimePickerFormat.Custom;
            dateTimePickerPonovnaTest.CustomFormat = " ";
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

            // Auto-fill NLBKB when Asseco changes
            textBoxAsseco.Leave += async (s, e) =>
            {
                string assecoBroj = textBoxAsseco.Text.Trim();
                if (!string.IsNullOrEmpty(assecoBroj))
                {
                    try
                    {
                        string nlbkb = await NadjiNLBKBBrojAsync(assecoBroj);
                        if (!string.IsNullOrEmpty(nlbkb))
                        {
                            textBoxNLBKB.Text = nlbkb;
                        }
                        else
                        {
                            // Nije pronađena nijedna vrednost za "Broj zahteva /incidenta NLBKB"
                            MessageBox.Show("Nije pronađen broj zahteva/incidenta NLBKB.", "Nije pronađeno", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            textBoxNLBKB.Text = "Na koji se ID odnosi";
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Greška pri povezivanju na bazu: {ex.Message}", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            };

            // Add arrows and custom menus to existing TextBoxes (no ComboBox overlays)
            try
            {
                // Server
                textBoxServer.KeyDown += (s, e) =>
                {
                    if ((e.Alt && e.KeyCode == Keys.Down) || e.KeyCode == Keys.F4)
                    {
                        ShowServersMenu();
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    }
                };
                btnServersDrop = CreateArrowButton();
                this.Controls.Add(btnServersDrop);
                btnServersDrop.BringToFront();
                PositionDropButton(btnServersDrop, textBoxServer);
                btnServersDrop.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                textBoxServer.SizeChanged += (s, e) => PositionDropButton(btnServersDrop!, textBoxServer);
                textBoxServer.LocationChanged += (s, e) => PositionDropButton(btnServersDrop!, textBoxServer);
                btnServersDrop.Click += (s, e) => ShowServersMenu();

                // Baza
                textBoxBaza.KeyDown += (s, e) =>
                {
                    if ((e.Alt && e.KeyCode == Keys.Down) || e.KeyCode == Keys.F4)
                    {
                        ShowBazeMenu();
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    }
                };
                btnBazeDrop = CreateArrowButton();
                this.Controls.Add(btnBazeDrop);
                btnBazeDrop.BringToFront();
                PositionDropButton(btnBazeDrop, textBoxBaza);
                btnBazeDrop.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                textBoxBaza.SizeChanged += (s, e) => PositionDropButton(btnBazeDrop!, textBoxBaza);
                textBoxBaza.LocationChanged += (s, e) => PositionDropButton(btnBazeDrop!, textBoxBaza);
                btnBazeDrop.Click += (s, e) => ShowBazeMenu();
            }
            catch
            {
                // If controls not yet created, skip without crashing
            }
        }

        private Button CreateArrowButton()
        {
            bool hoverFlag = false;
            bool pressedFlag = false;
            var btn = new Button
            {
                Text = string.Empty,
                TabStop = false,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btn.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btn.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                int padX = 2, padY = 1;
                var inner = new Rectangle(padX, padY, btn.Width - 2 * padX, btn.Height - 2 * padY);
                if (inner.Width < 6 || inner.Height < 6) inner = new Rectangle(0, 0, btn.Width, btn.Height);
                if (hoverFlag || pressedFlag)
                {
                    using var hoverBrush = new SolidBrush(Color.FromArgb(pressedFlag ? 16 : 8, 0, 0, 0));
                    g.FillRectangle(hoverBrush, inner);
                }
                float cx = inner.Left + inner.Width / 2f;
                float cy = inner.Top + inner.Height / 2f + 0.5f;
                // Make chevron a bit larger relative to the smaller button to reduce whitespace
                float w = Math.Min(9.5f, inner.Width * 0.70f);
                float h = Math.Min(4.5f, inner.Height * 0.42f);
                var p1 = new PointF(cx - w / 2f, cy - h / 2f);
                var p2 = new PointF(cx, cy + h / 2f);
                var p3 = new PointF(cx + w / 2f, cy - h / 2f);
                using var pen = new Pen(Color.Black, 1.1f) { StartCap = System.Drawing.Drawing2D.LineCap.Round, EndCap = System.Drawing.Drawing2D.LineCap.Round };
                g.DrawLines(pen, new[] { p1, p2, p3 });
            };
            btn.MouseEnter += (s, e) => { hoverFlag = true; ((Button)s!).Invalidate(); };
            btn.MouseLeave += (s, e) => { hoverFlag = false; pressedFlag = false; ((Button)s!).Invalidate(); };
            btn.MouseDown += (s, e) => { if (e.Button == MouseButtons.Left) { pressedFlag = true; ((Button)s!).Invalidate(); } };
            btn.MouseUp += (s, e) => { pressedFlag = false; ((Button)s!).Invalidate(); };
            return btn;
        }

        private void PositionDropButton(Button btn, Control target)
        {
            // Inset the button so TextBox border lines remain visible
            int inset = 5; // px from right/top/bottom 
            int bw = Math.Max(10, SystemInformation.VerticalScrollBarWidth - 8);
            btn.Width = bw;
            btn.Height = Math.Max(10, target.Height - inset * 2);
            btn.Location = new Point(target.Right - bw - inset, target.Top + inset);
            btn.BackColor = target.BackColor;
            btn.Parent = target.Parent ?? this;
            btn.UseVisualStyleBackColor = false;
            btn.BringToFront();
        }

        private void EnsureServersMenu()
        {
            if (menuServers != null) return;
            menuServers = new ContextMenuStrip()
            {
                AutoClose = true,
                ShowImageMargin = false
            };
            foreach (var opt in serverOptions)
            {
                var item = new ToolStripMenuItem(opt) { CheckOnClick = true };
                menuServers.Items.Add(item);
            }
            menuServers.Closing += (s, e) =>
            {
                if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
                {
                    e.Cancel = true; // keep open for multi-select
                }
                if (btnServersDrop != null) btnServersDrop.Invalidate();
            };
            menuServers.ItemClicked += (s, e) =>
            {
                if (serversTarget != null)
                {
                    this.BeginInvoke((Action)(() => UpdateControlFromMenu(menuServers!, serversTarget)));
                }
            };
        }

        private void EnsureBazeMenu()
        {
            if (menuBaze != null) return;
            menuBaze = new ContextMenuStrip()
            {
                AutoClose = true,
                ShowImageMargin = false
            };
            foreach (var opt in bazaOptions)
            {
                var item = new ToolStripMenuItem(opt) { CheckOnClick = true };
                menuBaze.Items.Add(item);
            }
            menuBaze.Closing += (s, e) =>
            {
                if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
                {
                    e.Cancel = true; // keep open for multi-select
                }
                if (btnBazeDrop != null) btnBazeDrop.Invalidate();
            };
            menuBaze.ItemClicked += (s, e) =>
            {
                if (bazeTarget != null)
                {
                    this.BeginInvoke((Action)(() => UpdateControlFromMenu(menuBaze!, bazeTarget)));
                }
            };
        }

        private void SyncMenuChecks(ContextMenuStrip menu, Control target)
        {
            var selected = new HashSet<string>((target.Text ?? string.Empty)
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim()), StringComparer.OrdinalIgnoreCase);
            foreach (var mi in menu.Items.OfType<ToolStripMenuItem>())
            {
                var text = (mi.Text ?? string.Empty).Trim();
                mi.Checked = selected.Contains(text);
            }
        }

        private void UpdateControlFromMenu(ContextMenuStrip menu, Control target)
        {
            var parts = menu.Items.OfType<ToolStripMenuItem>()
                .Where(i => i.Checked)
                .Select(i => i.Text)
                .ToList();
            target.Text = string.Join(", ", parts);
        }

        private void ShowServersMenu()
        {
            serversTarget = textBoxServer;
            EnsureServersMenu();
            SyncMenuChecks(menuServers!, serversTarget);
            menuServers!.AutoSize = false;
            var pref = menuServers.GetPreferredSize(new Size(serversTarget.Width, 800));
            menuServers.Size = new Size(serversTarget.Width, Math.Min(pref.Height, 300));
            // Slightly shift left and up to sit exactly under the field border
            var clientPt = new Point(-2, serversTarget.Height - 2);
            var screenPoint = serversTarget.PointToScreen(clientPt);
            menuServers.Show(screenPoint);
        }

        private void ShowBazeMenu()
        {
            bazeTarget = textBoxBaza;
            EnsureBazeMenu();
            SyncMenuChecks(menuBaze!, bazeTarget);
            menuBaze!.AutoSize = false;
            var pref = menuBaze.GetPreferredSize(new Size(bazeTarget.Width, 800));
            menuBaze.Size = new Size(bazeTarget.Width, Math.Min(pref.Height, 300));
            // Slightly shift left and up to sit exactly under the field border
            var clientPt = new Point(-2, bazeTarget.Height - 2);
            var screenPoint = bazeTarget.PointToScreen(clientPt);
            menuBaze.Show(screenPoint);
        }

        private async Task<string> NadjiNLBKBBrojAsync(string assecoBroj)
        {
            // MSSQL: server=spdb; database=izvestajiLIVE; table=dbo.tickets; lookup by 'id', return 'Customer Reference'
            string connStr = "Server=spdb;Database=izvestajiLIVE;Integrated Security=True;TrustServerCertificate=True;";
            string query = @"SELECT TOP 1 [Customer Reference], [Short name] FROM dbo.tickets WHERE [id] = @asseco";
            using (var conn = new Microsoft.Data.SqlClient.SqlConnection(connStr))
            using (var cmd = new Microsoft.Data.SqlClient.SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@asseco", assecoBroj.Trim());
                try
                {
                    await conn.OpenAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspeh otvaranja konekcije na bazu: {ex.Message}", "Konekcija nije uspela", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return string.Empty;
                }
                string? customerRef = null;
                string? shortName = null;
                try
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            customerRef = reader.IsDBNull(0) ? null : reader[0]?.ToString();
                            shortName = reader.IsDBNull(1) ? null : reader[1]?.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Neuspeh izvršavanja upita: {ex.Message}", "Konekcija nije uspela", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return string.Empty;
                }
                if (!string.IsNullOrWhiteSpace(customerRef))
                {
                    string cleanResult = customerRef.Replace("(", "").Replace(")", "").Replace(" ", "").Trim();
                    MessageBox.Show($"Broj zahteva pronađen: {cleanResult}", "DEBUG", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return cleanResult;
                }
                if (!string.IsNullOrWhiteSpace(shortName))
                {
                    var match = System.Text.RegularExpressions.Regex.Match(shortName, @":\s*\(([^)]+)\)");
                    if (match.Success)
                    {
                        string id = match.Groups[1].Value.Trim();
                        MessageBox.Show($"ID iz Short name pronađen: {id}", "DEBUG", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return id;
                    }
                }
                string fallbackQuery = "SELECT [Customer Reference], [Short name] FROM dbo.tickets WHERE [id] LIKE '%' + @asseco + '%'";
                using (var fallbackCmd = new Microsoft.Data.SqlClient.SqlCommand(fallbackQuery, conn))
                {
                    fallbackCmd.Parameters.AddWithValue("@asseco", assecoBroj.Trim());
                    using (var reader = await fallbackCmd.ExecuteReaderAsync())
                    {
                        string? firstNonEmpty = null;
                        string? firstShortNameId = null;
                        while (await reader.ReadAsync())
                        {
                            var val = reader.IsDBNull(0) ? null : reader[0]?.ToString();
                            var sn = reader.IsDBNull(1) ? null : reader[1]?.ToString();
                            if (firstNonEmpty == null && !string.IsNullOrWhiteSpace(val))
                            {
                                firstNonEmpty = val.Replace("(", "").Replace(")", "").Replace(" ", "").Trim();
                            }
                            if (firstShortNameId == null && !string.IsNullOrWhiteSpace(sn))
                            {
                                var match = System.Text.RegularExpressions.Regex.Match(sn, @":\s*\(([^)]+)\)");
                                if (match.Success)
                                {
                                    firstShortNameId = match.Groups[1].Value.Trim();
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(firstNonEmpty))
                        {
                            MessageBox.Show($"Broj zahteva pronađen: {firstNonEmpty}", "DEBUG", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return firstNonEmpty;
                        }
                        if (!string.IsNullOrEmpty(firstShortNameId))
                        {
                            MessageBox.Show($"ID iz Short name pronađen: {firstShortNameId}", "DEBUG", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return firstShortNameId;
                        }
                    }
                }
                return string.Empty;
            }
        }

        private void buttonPretrazi_Click(object? sender, EventArgs e)
        {
            OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
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
                        string modulCell = ws.Cells[i, 1].Text.Trim();
                        string idVerzijaCell = ws.Cells[i, 2].Text.Trim();
                        string datumTestCell = ws.Cells[i, 4].Text.Trim();
                        string datumProdukcioneCell = ws.Cells[i, 5].Text.Trim();
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
            // Validation
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

            string modul = comboBoxModul.SelectedItem?.ToString() ?? "";
            if (modul.StartsWith("  - "))
            {
                modul = "iBank (inHouse, web) - " + modul.Trim().Substring(3);
            }
            string idVerzija = textBoxIDVerzija.Text.Trim();
            string asseco = textBoxAsseco.Text.Trim();
            string nlbkb = textBoxNLBKB.Text.Trim();
            string server = textBoxServer.Text.Trim();
            string baza = textBoxBaza.Text.Trim();
            string spisakIdJeva = textBoxSpisakID.Text.Trim();
            string redosled = textBoxRedosled.Text.Trim();
            string napomena = textBoxNapomena.Text.Trim();
            string odgovornoLice = comboBoxOdgovornoLice.SelectedItem?.ToString() ?? "";
            string datumPrijem = dateTimePickerPrijem.Value.ToString("dd.MM.yyyy");
            string datumTest = dateTimePickerTest.Value.ToString("dd.MM.yyyy");
            string datumProdukcione = dateTimePickerPonovnaTest.CustomFormat != " " ? dateTimePickerPonovnaTest.Value.ToString("dd.MM.yyyy") : "";

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
                        // New schema without RB and with dates in C/D/E
                        ws.Cells[1, 1].Value = "Modul (aplikacija)";                 // A
                        ws.Cells[1, 2].Value = "ID verzija";                         // B
                        ws.Cells[1, 3].Value = "Datum prijema verzije";              // C
                        ws.Cells[1, 4].Value = "Datum spuštenja na test";            // D
                        ws.Cells[1, 5].Value = "Datum produkcione instalacije";      // E
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
    }
}
