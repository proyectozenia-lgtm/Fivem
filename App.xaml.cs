using System.Windows;

namespace FiveMOptimizerPro
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Aplicación principal de FiveM Optimizer Pro
        /// Verifica permisos de administrador al iniciar
        /// </summary>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Verificar permisos de administrador
            if (!IsRunningAsAdmin())
            {
                MessageBox.Show(
                    "Esta aplicación requiere permisos de administrador para funcionar correctamente.\n\n" +
                    "La aplicación se reiniciará con los permisos necesarios.",
                    "Permisos Requeridos",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                // Reiniciar como admin
                RestartAsAdmin();
                Current.Shutdown();
            }
        }

        /// <summary>
        /// Verifica si la aplicación está ejecutándose con permisos de administrador
        /// </summary>
        private static bool IsRunningAsAdmin()
        {
            var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            var principal = new System.Security.Principal.WindowsPrincipal(identity);
            return principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
        }

        /// <summary>
        /// Reinicia la aplicación con permisos de administrador
        /// </summary>
        private static void RestartAsAdmin()
        {
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName,
                UseShellExecute = true,
                Verb = "runas"
            };

            try
            {
                System.Diagnostics.Process.Start(psi);
            }
            catch
            {
                MessageBox.Show("No se pudieron obtener los permisos de administrador.", 
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}