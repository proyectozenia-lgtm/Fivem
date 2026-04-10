using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FiveMOptimizerPro.Models;
using FiveMOptimizerPro.Services;
using FiveMOptimizerPro.Helpers;
using System.Collections.ObjectModel;

namespace FiveMOptimizerPro.ViewModels
{
    /// <summary>
    /// ViewModel principal de la aplicación
    /// Maneja la lógica de la interfaz y coordina los servicios
    /// </summary>
    public partial class MainViewModel : ObservableObject
    {
        private readonly OptimizationService _optimizationService;
        private readonly SystemMonitorService _monitorService;
        private readonly ProcessService _processService;
        private readonly FileCleanerService _fileCleanerService;

        [ObservableProperty]
        private SystemMetrics currentMetrics = new();

        [ObservableProperty]
        private bool isOptimizing;

        [ObservableProperty]
        private string statusMessage = "Listo";

        [ObservableProperty]
        private float overallHealthScore;

        [ObservableProperty]
        private ObservableCollection<OptimizationResult> appliedOptimizations = new();

        [ObservableProperty]
        private ObservableCollection<ProcessInfo> runningProcesses = new();

        [ObservableProperty]
        private string selectedTheme = "Dark";

        public MainViewModel()
        {
            _optimizationService = new OptimizationService();
            _monitorService = new SystemMonitorService();
            _processService = new ProcessService();
            _fileCleanerService = new FileCleanerService();

            InitializeMonitoring();
            Logger.Initialize();

            StatusMessage = "FiveM Optimizer Pro iniciado correctamente";
        }

        /// <summary>
        /// Inicializa el monitoreo del sistema
        /// </summary>
        private void InitializeMonitoring()
        {
            _monitorService.OnMetricsUpdated += (metrics) =>
            {
                CurrentMetrics = metrics;
                CalculateHealthScore();
            };

            _monitorService.StartMonitoring();
            Logger.Info("MainViewModel", "Monitoreo del sistema iniciado");
        }

        /// <summary>
        /// Calcula el score de salud del sistema (0-100)
        /// </summary>
        private void CalculateHealthScore()
        {
            var score = 100f;

            // Penalizar por uso alto de CPU
            if (CurrentMetrics.CpuUsage > 80)
                score -= (CurrentMetrics.CpuUsage - 80) * 0.5f;

            // Penalizar por uso alto de RAM
            if (CurrentMetrics.RamUsage > 85)
                score -= (CurrentMetrics.RamUsage - 85) * 0.5f;

            // Penalizar por uso alto de GPU
            if (CurrentMetrics.GpuUsage > 90)
                score -= (CurrentMetrics.GpuUsage - 90) * 0.3f;

            // Penalizar por temperatura alta
            if (CurrentMetrics.CpuTemperature > 85)
                score -= (CurrentMetrics.CpuTemperature - 85) * 0.2f;

            OverallHealthScore = Math.Max(0, Math.Min(100, score));
        }

        /// <summary>
        /// Aplica un perfil de optimización completo
        /// </summary>
        [RelayCommand]
        public async Task ApplyProfile(string profileType)
        {
            if (IsOptimizing)
            {
                StatusMessage = "Ya hay una optimización en progreso";
                return;
            }

            IsOptimizing = true;
            StatusMessage = $"Aplicando perfil: {profileType}...";

            try
            {
                var profile = GetProfileOptimizations(profileType);
                var results = await _optimizationService.ApplyProfile(profile);

                foreach (var result in results)
                {
                    AppliedOptimizations.Add(result);
                    Logger.Info("MainViewModel", $"Optimización aplicada: {result.Name}");
                }

                StatusMessage = $"Perfil {profileType} aplicado correctamente ({results.Count} optimizaciones)";
                Logger.Info("MainViewModel", $"Perfil {profileType} aplicado");
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error aplicando perfil: {ex.Message}";
                Logger.Error("MainViewModel", $"Error aplicando perfil {profileType}", ex);
            }
            finally
            {
                IsOptimizing = false;
            }
        }

        /// <summary>
        /// Revierte todas las optimizaciones aplicadas
        /// </summary>
        [RelayCommand]
        public async Task RevertAllOptimizations()
        {
            if (IsOptimizing)
            {
                StatusMessage = "Ya hay una optimización en progreso";
                return;
            }

            IsOptimizing = true;
            StatusMessage = "Revirtiendo todas las optimizaciones...";

            try
            {
                await _optimizationService.RevertAllOptimizations();
                AppliedOptimizations.Clear();
                StatusMessage = "Todas las optimizaciones han sido revertidas";
                Logger.Info("MainViewModel", "Todas las optimizaciones revertidas");
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error revirtiendo optimizaciones: {ex.Message}";
                Logger.Error("MainViewModel", "Error revirtiendo optimizaciones", ex);
            }
            finally
            {
                IsOptimizing = false;
            }
        }

        /// <summary>
        /// Limpia archivos temporales y caché
        /// </summary>
        [RelayCommand]
        public async Task CleanupSystem()
        {
            if (IsOptimizing)
            {
                StatusMessage = "Ya hay una limpieza en progreso";
                return;
            }

            IsOptimizing = true;
            StatusMessage = "Limpiando archivos temporales...";

            try
            {
                var result = await _fileCleanerService.CleanAll();
                StatusMessage = $"Limpieza completa: {result.GetSizeFreedFormatted()} liberados";
                Logger.Info("MainViewModel", $"Limpieza completada: {result.GetSizeFreedFormatted()}");
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error en limpieza: {ex.Message}";
                Logger.Error("MainViewModel", "Error en limpieza", ex);
            }
            finally
            {
                IsOptimizing = false;
            }
        }

        /// <summary>
        /// Mata procesos innecesarios
        /// </summary>
        [RelayCommand]
        public Task KillBackgroundProcesses()
        {
            StatusMessage = "Terminando procesos en background...";

            try
            {
                int killed = 0;
                foreach (var process in Constants.ProcessesToKill)
                {
                    killed += _processService.KillProcessByName(process);
                }

                StatusMessage = $"{killed} procesos terminados";
                Logger.Info("MainViewModel", $"{killed} procesos terminados");
                RefreshProcessList();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error terminando procesos: {ex.Message}";
                Logger.Error("MainViewModel", "Error terminando procesos", ex);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Actualiza la lista de procesos activos
        /// </summary>
        [RelayCommand]
        public Task RefreshProcessList()
        {
            try
            {
                var processes = _processService.GetTopProcessesByMemory(20);
                RunningProcesses.Clear();

                foreach (var process in processes)
                {
                    RunningProcesses.Add(process);
                }

                StatusMessage = $"{RunningProcesses.Count} procesos listados";
            }
            catch (Exception ex)
            {
                Logger.Error("MainViewModel", "Error refrescando lista de procesos", ex);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Cambia el tema de la aplicación
        /// </summary>
        [RelayCommand]
        public Task ToggleTheme()
        {
            SelectedTheme = SelectedTheme == "Dark" ? "Light" : "Dark";
            Logger.Info("MainViewModel", $"Tema cambiado a {SelectedTheme}");
            return Task.CompletedTask;
        }

        /// <summary>
        /// Obtiene los IDs de optimizaciones para un perfil específico
        /// </summary>
        private List<string> GetProfileOptimizations(string profileType)
        {
            return profileType switch
            {
                "GamingExtreme" => GetGamingExtremeProfile(),
                "Streaming" => GetStreamingProfile(),
                "BatterySaver" => GetBatterySaverProfile(),
                _ => GetDefaultProfile()
            };
        }

        /// <summary>
        /// Perfil Gaming Extreme - Máximo rendimiento
        /// </summary>
        private List<string> GetGamingExtremeProfile()
        {
            return new List<string>
            {
                // CPU Optimizations
                "cpu_001", "cpu_002", "cpu_003", "cpu_004", "cpu_006",
                // GPU Optimizations
                "gpu_001", "gpu_002", "gpu_003",
                // RAM Optimizations
                "ram_001",
                // Network Optimizations
                "net_001",
                // FiveM Optimizations
                "fivem_001",
                // Service Optimizations
                "svc_DiagTrack", "svc_WSearch", "svc_SysMain",
            };
        }

        /// <summary>
        /// Perfil Streaming - Equilibrio entre FPS y CPU
        /// </summary>
        private List<string> GetStreamingProfile()
        {
            return new List<string>
            {
                "cpu_002", "cpu_004",
                "gpu_001", "gpu_002",
                "ram_001",
                "svc_DiagTrack", "svc_WSearch"
            };
        }

        /// <summary>
        /// Perfil Battery Saver - Bajo consumo
        /// </summary>
        private List<string> GetBatterySaverProfile()
        {
            return new List<string>
            {
                "svc_DiagTrack", "svc_WSearch", "svc_PrintSpooler"
            };
        }

        /// <summary>
        /// Perfil Default - Recomendado
        /// </summary>
        private List<string> GetDefaultProfile()
        {
            return new List<string>
            {
                "cpu_001", "cpu_002", "cpu_004",
                "gpu_001", "gpu_002",
                "ram_001",
                "net_001",
                "svc_DiagTrack", "svc_WSearch",
                "fivem_001"
            };
        }

        /// <summary>
        /// Limpia recursos
        /// </summary>
        public void Cleanup()
        {
            _monitorService.StopMonitoring();
            _monitorService.Dispose();
            Logger.Info("MainViewModel", "Aplicación cerrada correctamente");
        }
    }
}