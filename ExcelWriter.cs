using System;
using System.IO;
using OfficeOpenXml;

namespace ExcelFiller
{
    internal class ExcelWriter
    {
        public void AppendRow(
            string excelPath,
            string modul,
            string idVerzija,
            string datumPrijem,
            string datumTest,
            string datumProdukcione,
            string asseco,
            string nlbkb,
            string server,
            string baza,
            string spisakIdJeva,
            string redosled,
            string odgovornoLice,
            string napomena,
            string putanjaIsporuke)
        {
            if (string.IsNullOrWhiteSpace(excelPath))
                throw new ArgumentException("Excel path is required", nameof(excelPath));

            EnsureDirectoryExists(excelPath);

            var fileInfo = new FileInfo(excelPath);
            bool noviFajl = !fileInfo.Exists;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet ws;
                if (noviFajl)
                {
                    ws = package.Workbook.Worksheets.Add("Podaci");
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
                    ws.Cells[1, 14].Value = "Putanja isporuke";                  // N
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
                ws.Cells[newRow, 14].Value = string.IsNullOrEmpty(putanjaIsporuke) ? null : putanjaIsporuke; // N

                package.Save();
            }
        }

        private static void EnsureDirectoryExists(string excelPath)
        {
            string? dir = Path.GetDirectoryName(excelPath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }
    }
}
