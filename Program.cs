namespace ExcelFiller
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Global exception handlers to avoid silent crash and show message to the user
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (s, e) =>
            {
                try
                {
                    MessageBox.Show($"Neočekivana greška:\n\n{e.Exception.Message}", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch { /* ignore */ }
            };
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                try
                {
                    var ex = e.ExceptionObject as Exception;
                    MessageBox.Show($"Neočekivana greška (global):\n\n{ex?.Message}", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch { /* ignore */ }
            };

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            try
            {
                Application.Run(new Form1());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri pokretanju aplikacije:\n\n{ex.Message}", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}