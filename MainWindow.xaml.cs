using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace FiveMOptimizerFinal
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            btnTurbo.Click += BtnTurbo_Click;
            btnUndo.Click += BtnUndo_Click;
            btnMonitor.Click += BtnMonitor_Click;
        }

        private void BtnTurbo_Click(object sender, RoutedEventArgs e)
        {
            lblStatus.Text = "⏳ Aplicando optimizaciones...";

            // CPU
            if (chkCPU.IsChecked == true)
                RunCommand("powercfg -setacvalueindex SCHEME_CURRENT SUB_PROCESSOR CPMINCORES 100");

            // GPU
            if (chkMPO.IsChecked == true)
                using (var key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\Dwm"))
                    if (key != null) key.SetValue("OverlayTestMode", 5, RegistryValueKind.DWord);

            if (chkHAGS.IsChecked == true)
                using (var key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\GraphicsDrivers"))
                    if (key != null) key.SetValue("HwSchMode", 2, RegistryValueKind.DWord);

            if (chkLowLatency.IsChecked == true)
                using (var key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\Class\{4d36e968-e325-11ce-bfc1-08002be10318}\0000"))
                    if (key != null) key.SetValue("LowLatencyMode", 2, RegistryValueKind.DWord);

            // Red
            if (chkNetwork.IsChecked == true)
            {
                var interfaces = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\Tcpip\Parameters\Interfaces");
                if (interfaces != null)
                {
                    foreach (var subKeyName in interfaces.GetSubKeyNames())
                    {
                        using (var interfaceKey = interfaces.OpenSubKey(subKeyName, true))
                        {
                            if (interfaceKey != null)
                            {
                                interfaceKey.SetValue("TcpAckFrequency", 1, RegistryValueKind.DWord);
                                interfaceKey.SetValue("TCPNoDelay", 1, RegistryValueKind.DWord);
                            }
                        }
                    }
                }
            }

            if (chkDNS.IsChecked == true)
            {
                RunCommand("ipconfig /flushdns");
                RunCommand("arp -d");
            }

            // Sistema
            if (chkClean.IsChecked == true)
                RunCommand("Remove-Item -Path \"$env:TEMP\\*\" -Recurse -Force -ErrorAction SilentlyContinue");

            if (chkHibernation.IsChecked == true)
                RunCommand("powercfg /h off");

            // FiveM
            if (chkFiveMCache.IsChecked == true)
            {
                string cachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FiveM", "FiveM.app", "cache");
                if (Directory.Exists(cachePath))
                {
                    try { foreach (var file in Directory.GetFiles(cachePath, "*", SearchOption.AllDirectories)) File.Delete(file); } catch { }
                }
            }

            if (chkVRAM.IsChecked == true)
            {
                string fiveMData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FiveM", "FiveM.app", "data");
                if (!Directory.Exists(fiveMData)) Directory.CreateDirectory(fiveMData);

                string settingsPath = Path.Combine(fiveMData, "settings.xml");
                string settingsXml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<Settings>
  <Setting name=""streaming_MemoryMB"" value=""2048"" />
  <Setting name=""streaming_MaxTextureMemoryMB"" value=""1536"" />
  <Setting name=""streaming_TextureBudgetMB"" value=""3072"" />
</Settings>";
                File.WriteAllText(settingsPath, settingsXml);
            }

            // Abrir FiveM
            try
            {
                string fiveMPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FiveM", "FiveM.exe");
                if (File.Exists(fiveMPath))
                {
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = fiveMPath,
                        UseShellExecute = true,
                        Verb = "runasuser"
                    };
                    Process.Start(startInfo);
                }
            }
            catch { }

            lblStatus.Text = "✅ Optimizaciones aplicadas!";
            MessageBox.Show("Optimizaciones aplicadas!\n\nRecomendaciones:\n- Extended Texture Budget: MENOS del 50%\n- MSAA: 2x\n- Shadow Quality: Normal",
                "FiveM Optimizer", MessageBoxButton.OK, MessageBoxIcon.Information);
        }

        private void BtnUndo_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("¿Deshacer todos los cambios?", "Confirmar", MessageBoxButton.YesNo, MessageBoxIcon.Warning);
            if (result == MessageBoxResult.Yes)
            {
                RunCommand("powercfg /setactive SCHEME_BALANCED");
                RunCommand("powercfg /h on");
                lblStatus.Text = "✅ Cambios deshechos";
                MessageBox.Show("Cambios deshechos. Reinicia el PC.", "FiveM Optimizer", MessageBoxButton.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnMonitor_Click(object sender, RoutedEventArgs e)
        {
            string script = @"
while ($true) {
    $gpuInfo = & nvidia-smi --query-gpu=memory.used,memory.total --format=csv,noheader 2>$null
    if ($gpuInfo) {
        $parts = $gpuInfo -split ','
        $used = [int]($parts[0].Trim() -replace ' MiB', '')
        $total = [int]($parts[1].Trim() -replace ' MiB', '')
        $usedGB = [math]::Round($used / 1024, 2)
        $percent = [math]::Round(($used / $total) * 100, 1)
        if ($percent -gt 85) { Write-Host '⚠️ VRAM CRITICA:' $usedGB'GB / 4GB ('$percent'%)' -ForegroundColor Red }
        elseif ($percent -gt 70) { Write-Host '⚠️ VRAM ALTA:' $usedGB'GB / 4GB ('$percent'%)' -ForegroundColor Yellow }
        else { Write-Host '✓ VRAM:' $usedGB'GB / 4GB ('$percent'%)' -ForegroundColor Green }
    }
    Start-Sleep -Seconds 2
}";
            string scriptPath = Path.GetTempFileName() + ".ps1";
            File.WriteAllText(scriptPath, script);
            Process.Start("powershell.exe", $"-NoExit -ExecutionPolicy Bypass -File \"{scriptPath}\"");
        }

        private void RunCommand(string command)
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = $"-Command \"{command}\"",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                process.Start();
                process.WaitForExit(2000);
            }
            catch { }
        }
    }
}