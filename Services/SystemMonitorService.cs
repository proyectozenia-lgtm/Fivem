using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using FiveMOptimizerPro.Models;
using FiveMOptimizerPro.Helpers;
using LibreHardwareMonitor.Hardware;
using System.Diagnostics;

namespace FiveMOptimizerPro.Services
{
    /// <summary>
    /// Servicio de monitoreo del sistema en tiempo real
    /// Obtiene métricas de CPU, GPU, RAM, discos y red
    /// Fuente: LibreHardwareMonitor para lecturas de hardware
    /// </summary>
    public class SystemMonitorService : IDisposable
    {
        private readonly Computer _computer;
        private readonly PerformanceCounter _cpuCounter;
        private readonly PerformanceCounter _ramCounter;
        private SystemMetrics _lastMetrics;
        private CancellationTokenSource? _cancellationTokenSource;
        private Task? _monitoringTask;

        public event Action<SystemMetrics>? OnMetricsUpdated;

        public SystemMonitorService()
        {
            _computer = new Computer
            {
                IsCpuEnabled = true,
                IsGpuEnabled = true,
                IsMemoryEnabled = true,
                IsMotherboardEnabled = true,
                IsControllerEnabled = false,
                IsNetworkEnabled = false,
                IsStorageEnabled = true
            };

            _computer.Open();

            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            _ramCounter = new PerformanceCounter("Memory", "% Committed Bytes In Use");

            _lastMetrics = new SystemMetrics();
        }

        /// <summary>
        /// Inicia el monitoreo continuo del sistema
        /// </summary>
        public void StartMonitoring()
        {
            if (_cancellationTokenSource != null)
                return;

            _cancellationTokenSource = new CancellationTokenSource();
            _monitoringTask = MonitorLoop(_cancellationTokenSource.Token);

            Logger.Info("SystemMonitorService", "Monitoreo del sistema iniciado");
        }

        /// <summary>
        /// Detiene el monitoreo del sistema
        /// </summary>
        public void StopMonitoring()
        {
            _cancellationTokenSource?.Cancel();
            _monitoringTask?.Wait(5000);
            _cancellationTokenSource = null;

            Logger.Info("SystemMonitorService", "Monitoreo del sistema detenido");
        }

        /// <summary>
        /// Loop principal de monitoreo
        /// </summary>
        private async Task MonitorLoop(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var metrics = GetCurrentMetrics();
                    _lastMetrics = metrics;
                    OnMetricsUpdated?.Invoke(metrics);

                    await Task.Delay(Constants.SystemMonitorIntervalMs, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Logger.Error("SystemMonitorService", "Error en loop de monitoreo", ex);
                }
            }
        }

        /// <summary>
        /// Obtiene las métricas actuales del sistema
        /// </summary>
        public SystemMetrics GetCurrentMetrics()
        {
            try
            {
                var metrics = new SystemMetrics
                {
                    Timestamp = DateTime.Now,
                    CpuUsage = GetCpuUsage(),
                    CpuTemperature = GetCpuTemperature(),
                    CpuClockSpeed = GetCpuClockSpeed(),
                    RamUsage = GetRamUsagePercentage(),
                    RamTotal = GetTotalRamGB(),
                    RamUsed = GetUsedRamGB(),
                    GpuUsage = GetGpuUsage(),
                    GpuTemperature = GetGpuTemperature(),
                    VramUsed = GetVramUsed(),
                    VramTotal = GetVramTotal(),
                    DiskUsage = GetDiskUsage(),
                    DiskReadSpeed = GetDiskReadSpeed(),
                    DiskWriteSpeed = GetDiskWriteSpeed()
                };

                return metrics;
            }
            catch (Exception ex)
            {
                Logger.Error("SystemMonitorService", "Error obteniendo métricas", ex);
                return _lastMetrics;
            }
        }

        /// <summary>
        /// Obtiene el uso de CPU en porcentaje
        /// </summary>
        private float GetCpuUsage()
        {
            try
            {
                return _cpuCounter.NextValue();
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Obtiene la temperatura del CPU
        /// </summary>
        private float GetCpuTemperature()
        {
            try
            {
                foreach (var hardware in _computer.Hardware)
                {
                    if (hardware.HardwareType == HardwareType.Cpu)
                    {
                        hardware.Update();
                        foreach (var sensor in hardware.Sensors)
                        {
                            if (sensor.SensorType == SensorType.Temperature && sensor.Name.Contains("Package"))
                            {
                                return (float?)sensor.Value ?? 0;
                            }
                        }
                    }
                }
            }
            catch
            {
                // Silenciar errores
            }

            return 0;
        }

        /// <summary>
        /// Obtiene la velocidad del reloj del CPU en MHz
        /// </summary>
        private float GetCpuClockSpeed()
        {
            try
            {
                foreach (var hardware in _computer.Hardware)
                {
                    if (hardware.HardwareType == HardwareType.Cpu)
                    {
                        hardware.Update();
                        foreach (var sensor in hardware.Sensors)
                        {
                            if (sensor.SensorType == SensorType.Clock)
                            {
                                return (float?)sensor.Value ?? 0;
                            }
                        }
                    }
                }
            }
            catch
            {
                // Silenciar errores
            }

            return 0;
        }

        /// <summary>
        /// Obtiene el uso de RAM en porcentaje
        /// </summary>
        private float GetRamUsagePercentage()
        {
            try
            {
                return _ramCounter.NextValue();
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Obtiene la RAM total en GB
        /// </summary>
        private float GetTotalRamGB()
        {
            try
            {
                var totalMemory = GC.GetTotalMemory(false);
                var info = System.Diagnostics.ProcessInfo.GetCurrentProcess().TotalProcessorTime;
                
                // Alternativa usando WMI
                var osInfo = new System.Management.ManagementObjectSearcher(
                    "SELECT TotalVisibleMemorySize FROM Win32_OperatingSystem").Get();
                
                foreach (var item in osInfo)
                {
                    var totalKB = ulong.Parse(item["TotalVisibleMemorySize"].ToString());
                    return totalKB / (1024 * 1024f);
                }
            }
            catch
            {
                // Usar valor estimado
                return 16; // Asumir 16GB por defecto
            }

            return 0;
        }

        /// <summary>
        /// Obtiene la RAM utilizada en GB
        /// </summary>
        private float GetUsedRamGB()
        {
            try
            {
                var totalRam = GetTotalRamGB();
                var usagePercent = GetRamUsagePercentage();
                return (totalRam * usagePercent) / 100f;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Obtiene el uso de GPU en porcentaje
        /// </summary>
        private float GetGpuUsage()
        {
            try
            {
                foreach (var hardware in _computer.Hardware)
                {
                    if (hardware.HardwareType == HardwareType.GpuNvidia || 
                        hardware.HardwareType == HardwareType.GpuAmd)
                    {
                        hardware.Update();
                        foreach (var sensor in hardware.Sensors)
                        {
                            if (sensor.SensorType == SensorType.Load && sensor.Name.Contains("GPU Core"))
                            {
                                return (float?)sensor.Value ?? 0;
                            }
                        }
                    }
                }
            }
            catch
            {
                // Silenciar errores
            }

            return 0;
        }

        /// <summary>
        /// Obtiene la temperatura de la GPU
        /// </summary>
        private float GetGpuTemperature()
        {
            try
            {
                foreach (var hardware in _computer.Hardware)
                {
                    if (hardware.HardwareType == HardwareType.GpuNvidia || 
                        hardware.HardwareType == HardwareType.GpuAmd)
                    {
                        hardware.Update();
                        foreach (var sensor in hardware.Sensors)
                        {
                            if (sensor.SensorType == SensorType.Temperature)
                            {
                                return (float?)sensor.Value ?? 0;
                            }
                        }
                    }
                }
            }
            catch
            {
                // Silenciar errores
            }

            return 0;
        }

        /// <summary>
        /// Obtiene la VRAM utilizada en GB
        /// </summary>
        private float GetVramUsed()
        {
            try
            {
                foreach (var hardware in _computer.Hardware)
                {
                    if (hardware.HardwareType == HardwareType.GpuNvidia || 
                        hardware.HardwareType == HardwareType.GpuAmd)
                    {
                        hardware.Update();
                        foreach (var sensor in hardware.Sensors)
                        {
                            if (sensor.SensorType == SensorType.SmallData && sensor.Name.Contains("Memory Used"))
                            {
                                return (float?)sensor.Value ?? 0;
                            }
                        }
                    }
                }
            }
            catch
            {
                // Silenciar errores
            }

            return 0;
        }

        /// <summary>
        /// Obtiene la VRAM total en GB
        /// </summary>
        private float GetVramTotal()
        {
            try
            {
                foreach (var hardware in _computer.Hardware)
                {
                    if (hardware.HardwareType == HardwareType.GpuNvidia || 
                        hardware.HardwareType == HardwareType.GpuAmd)
                    {
                        hardware.Update();
                        foreach (var sensor in hardware.Sensors)
                        {
                            if (sensor.SensorType == SensorType.SmallData && sensor.Name.Contains("Memory Total"))
                            {
                                return (float?)sensor.Value ?? 0;
                            }
                        }
                    }
                }
            }
            catch
            {
                // Silenciar errores
            }

            return 4; // Asumir 4GB por defecto (RTX 3050)
        }

        /// <summary>
        /// Obtiene el uso del disco en porcentaje
        /// </summary>
        private float GetDiskUsage()
        {
            try
            {
                var drive = DriveInfo.GetDrives().FirstOrDefault(d => d.IsReady);
                if (drive != null)
                {
                    var usage = ((drive.TotalSize - drive.AvailableFreeSpace) * 100f) / drive.TotalSize;
                    return usage;
                }
            }
            catch
            {
                // Silenciar errores
            }

            return 0;
        }

        /// <summary>
        /// Obtiene la velocidad de lectura de disco en MB/s
        /// </summary>
        private float GetDiskReadSpeed()
        {
            try
            {
                var counter = new PerformanceCounter("PhysicalDisk", "Disk Read Bytes/sec", "_Total");
                var readSpeed = counter.NextValue() / (1024 * 1024f); // Convertir a MB/s
                counter.Dispose();
                return readSpeed;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Obtiene la velocidad de escritura de disco en MB/s
        /// </summary>
        private float GetDiskWriteSpeed()
        {
            try
            {
                var counter = new PerformanceCounter("PhysicalDisk", "Disk Write Bytes/sec", "_Total");
                var writeSpeed = counter.NextValue() / (1024 * 1024f); // Convertir a MB/s
                counter.Dispose();
                return writeSpeed;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Libera recursos
        /// </summary>
        public void Dispose()
        {
            StopMonitoring();
            _computer?.Close();
            _cpuCounter?.Dispose();
            _ramCounter?.Dispose();
        }
    }
}