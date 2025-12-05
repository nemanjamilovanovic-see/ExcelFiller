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

        // Excel writer helper
        private readonly ExcelWriter excelWriter = new ExcelWriter();

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

            // Validation UI: disable Dodaj dok obavezna polja nisu popunjena
            buttonDodaj.Enabled = false;
            comboBoxModul.SelectedIndexChanged += (s, e) => UpdateDodajEnabled();
            textBoxIDVerzija.TextChanged += (s, e) => UpdateDodajEnabled();
            textBoxAsseco.TextChanged += (s, e) => UpdateDodajEnabled();
            textBoxNLBKB.TextChanged += (s, e) => UpdateDodajEnabled();
            dateTimePickerPrijem.ValueChanged += (s, e) => UpdateDodajEnabled();
            dateTimePickerTest.ValueChanged += (s, e) => UpdateDodajEnabled();
            textBoxServer.TextChanged += (s, e) => UpdateDodajEnabled();
            textBoxSpisakID.TextChanged += (s, e) => UpdateDodajEnabled();
            comboBoxOdgovornoLice.SelectedIndexChanged += (s, e) => UpdateDodajEnabled();
            textBoxPutanjaIsporuke.TextChanged += (s, e) => UpdateDodajEnabled();

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

        private void UpdateDodajEnabled()
        {
            bool sviPopunjeni =
                comboBoxModul.SelectedIndex != -1 &&
                !string.IsNullOrWhiteSpace(textBoxIDVerzija.Text) &&
                !string.IsNullOrWhiteSpace(textBoxAsseco.Text) &&
                !string.IsNullOrWhiteSpace(textBoxNLBKB.Text) &&
                !string.IsNullOrWhiteSpace(textBoxServer.Text) &&
                !string.IsNullOrWhiteSpace(textBoxSpisakID.Text) &&
                !string.IsNullOrWhiteSpace(textBoxPutanjaIsporuke.Text) &&
                comboBoxOdgovornoLice.SelectedIndex != -1;

            buttonDodaj.Enabled = sviPopunjeni;
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
                labelRezultat.Text = "Izaberite modul!";
                return;
            }
            string excelNazivModula = izabraniModul;
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
                string.IsNullOrWhiteSpace(textBoxPutanjaIsporuke.Text) ||
                comboBoxOdgovornoLice.SelectedIndex == -1)
            {
                MessageBox.Show("Popunite sva obavezna polja označena zvezdicom (*)!", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string modul = comboBoxModul.SelectedItem?.ToString() ?? "";
            string idVerzija = textBoxIDVerzija.Text.Trim();
            string asseco = textBoxAsseco.Text.Trim();
            string nlbkb = textBoxNLBKB.Text.Trim();
            string server = textBoxServer.Text.Trim();
            string baza = textBoxBaza.Text.Trim();
            string spisakIdJeva = textBoxSpisakID.Text.Trim();
            string redosled = textBoxRedosled.Text.Trim();
            string napomena = textBoxNapomena.Text.Trim();
            string putanjaIsporuke = textBoxPutanjaIsporuke.Text.Trim();
            string odgovornoLice = comboBoxOdgovornoLice.SelectedItem?.ToString() ?? "";
            string datumPrijem = dateTimePickerPrijem.Value.ToString("dd.MM.yyyy");
            string datumTest = dateTimePickerTest.Value.ToString("dd.MM.yyyy");
            string datumProdukcione = dateTimePickerPonovnaTest.CustomFormat != " " ? dateTimePickerPonovnaTest.Value.ToString("dd.MM.yyyy") : "";

            try
            {
                // Glavni Excel
                excelWriter.AppendRow(
                    excelSharedPath,
                    modul,
                    idVerzija,
                    datumPrijem,
                    datumTest,
                    datumProdukcione,
                    asseco,
                    nlbkb,
                    server,
                    baza,
                    spisakIdJeva,
                    redosled,
                    odgovornoLice,
                    napomena,
                    putanjaIsporuke);

                // Upis u dodatni Excel fajl po modulu (ako je definisan)
                try
                {
                    string? secondaryPath = GetSecondaryExcelPathForModule(modul);
                    if (!string.IsNullOrWhiteSpace(secondaryPath))
                    {
                        excelWriter.AppendRow(
                            secondaryPath,
                            modul,
                            idVerzija,
                            datumPrijem,
                            datumTest,
                            datumProdukcione,
                            asseco,
                            nlbkb,
                            server,
                            baza,
                            spisakIdJeva,
                            redosled,
                            odgovornoLice,
                            napomena,
                            putanjaIsporuke);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Podaci su upisani u glavni Excel, ali nije uspelo upisivanje u dodatni Excel fajl: {ex.Message}",
                        "Upozorenje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                MessageBox.Show("Podaci su uspešno upisani u Excel!", "Uspeh", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Excel fajl nije moguće upisati. Najčešći razlog je da je fajl otvoren na drugom računaru ili programu, ili nemate dozvolu za upis. Zatvorite fajl 'Izvestaj.xlsx' na deljenoj lokaciji i pokušajte ponovo.", "Upozorenje: Excel fajl nije dostupan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private static string? GetSecondaryExcelPathForModule(string modul)
        {
            modul = modul.Trim();

            // PD -> PD.xlsx u Nikola
            if (string.Equals(modul, "PD", StringComparison.OrdinalIgnoreCase))
            {
                return @"\\bss-testcomp01\PrimenaNLB\Nikola\PD.xlsx";
            }

            // Luka folder
            string[] lukaModules =
            {
                "CS",
                "iBank",
                "KDP",
                "PGW"
            };
            if (lukaModules.Contains(modul, StringComparer.OrdinalIgnoreCase))
            {
                return $@"\\bss-testcomp01\PrimenaNLB\Luka\{modul}.xlsx";
            }

            // Nemanja folder
            string[] nemanjaModules = { "DevPP", "GK", "Trezor" };
            if (nemanjaModules.Contains(modul, StringComparer.OrdinalIgnoreCase))
            {
                return $@"\\bss-testcomp01\PrimenaNLB\Nemanja\{modul}.xlsx";
            }

            // Milica folder
            string[] milicaModules = { "BankaP", "RiskManag", "CMS", "PP", "BCAPI" };
            if (milicaModules.Contains(modul, StringComparer.OrdinalIgnoreCase))
            {
                return $@"\\bss-testcomp01\PrimenaNLB\Milica\{modul}.xlsx";
            }

            return null;
        }

    }
}
