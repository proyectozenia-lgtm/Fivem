namespace FiveMOptimizerPro.Helpers
{
    /// <summary>
    /// Constantes y configuraciones globales
    /// Fuentes: Microsoft Docs, NVIDIA/AMD Docs, FiveM Docs
    /// </summary>
    public static class Constants
    {
        // INFORMACIÓN DE LA APLICACIÓN
        public const string AppName = "FiveM Optimizer Pro";
        public const string AppVersion = "2026.01.001";
        public const string AppAuthor = "nexxysg-pixel";
        public const string AppUrl = "https://github.com/nexxysg-pixel/FiveMOptimizerPro-2026";

        // RUTAS CRÍTICAS
        public static readonly string FiveMPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "FiveM"
        );

        public static readonly string FiveMCachePath = Path.Combine(FiveMPath, "FiveM Application Data", "cache");
        public static readonly string FiveMSettingsPath = Path.Combine(FiveMPath, "FiveM Application Data");
        public static readonly string AppDataPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "FiveMOptimizerPro"
        );

        // REGISTROS DE WINDOWS
        public const string RegistryPathNetwork = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters";
        public const string RegistryPathNDIS = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NDIS\Parameters";
        public const string RegistryPathSystem = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management";
        public const string RegistryPathGPU = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers";
        public const string RegistryPathPerformance = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl";

        // SERVICIOS WINDOWS A OPTIMIZAR
        public static readonly Dictionary<string, string> WindowsServices = new()
        {
            // Servicios no esenciales para gaming
            { "DiagTrack", "Connected User Experiences and Telemetry" },
            { "dmwappushservice", "dmwappushservice" },
            { "WSearch", "Windows Search" },
            { "SysMain", "SysMain (Superfetch)" },
            { "PrintSpooler", "Print Spooler" },
            { "wuauserv", "Windows Update" },
            { "BITS", "Background Intelligent Transfer Service" },
            { "DoSvc", "Delivery Optimization" },
            { "MapsBroker", "Downloaded Maps Manager" },
            { "lmhosts", "TCP/IP NetBIOS Helper" },
            { "NetTcpPortSharing", "Net.Tcp Port Sharing Service" },
            { "PcaSvc", "Program Compatibility Assistant Service" },
            { "RemoteRegistry", "Remote Registry" },
            { "SharedAccess", "Internet Connection Sharing" },
            { "upnphost", "UPnP Device Host" },
            { "WinRM", "Windows Remote Management" },
            { "Themes", "Themes" },
            { "TabletInputService", "Touch Keyboard and Handwriting Panel Service" },
            { "XblAuthManager", "Xbox Live Auth Manager" },
            { "XblGameSave", "Xbox Live Game Save Service" },
            { "XboxNetApiSvc", "Xbox Live Networking Service" },
            { "cbdhsvc", "CoreMessaging" }
        };

        // PROCESOS A TERMINAR ANTES DE JUGAR
        public static readonly string[] ProcessesToKill = new[]
        {
            "chrome",
            "firefox",
            "discord",
            "spotify",
            "steammessagingclient",
            "armoury_crate",
            "onedrive",
            "skype",
            "thunderbird",
            "slack",
            "teams",
            "obs"
        };

        // CONFIGURACIÓN DE RED (Microsoft Docs)
        // https://docs.microsoft.com/en-us/windows-server/networking/technologies/tcpip/tcp-ip-4
        public const int TcpAutoTuningLevel = 3; // Restricted (optimizado para gaming)
        public const int TcpInitialRTO = 3000; // Initial RTO en ms
        public const int TcpMaxDataRetransmissions = 5;
        public const int TcpAckFrequency = 1; // Acknowledgment frequency
        public const int DisableTaskOffload = 0; // Desactivar descarga de tareas

        // CONFIGURACIÓN DE GPU (NVIDIA/AMD Docs)
        // https://nvidia.custhelp.com/app/answers/detail/a_id/3050
        public const string GpuLowLatencyMode = "1"; // Baja latencia
        public const string GpuPCIeAspm = "0"; // Desactivar ASPM
        public const string GpuHAGS = "0"; // Hardware Accelerated GPU Scheduling desactivado para compatibilidad
        public const string GpuTDR = "0"; // Timeout Detection and Recovery

        // CONFIGURACIÓN DE CPU (Windows Performance Tuning)
        // https://docs.microsoft.com/en-us/windows-hardware/design/device-experiences/oem-power-management
        public const int DisableCoreParking = 1;
        public const int Win32PrioritySeparation = 26; // Favorecer procesos en foreground
        public const int LargeSystemCache = 1; // Usar más RAM para cache del sistema
        public const int DisablePagingExecutive = 1; // Mantener ejecutables en RAM

        // CONFIGURACIÓN FiveM
        public const int FiveMVramLimit = 3072; // Limitar VRAM a 3GB (compatible con RTX 3050)
        public const int FiveMProcessPriority = 2; // High priority
        public const int FiveMCacheClearIntervalHours = 24;

        // BENCHMARKS
        public const int BenchmarkDurationSeconds = 30;
        public const int SystemMonitorIntervalMs = 1000; // Actualizar cada segundo
    }

    /// <summary>
    /// Optimizaciones predefinidas por categoría
    /// </summary>
    public static class OptimizationCategories
    {
        public const string CPU = "CPU";
        public const string GPU = "GPU";
        public const string RAM = "RAM";
        public const string Network = "Red";
        public const string Services = "Servicios";
        public const string Processes = "Procesos";
        public const string Storage = "Almacenamiento";
        public const string FiveM = "FiveM";
        public const string System = "Sistema";
        public const string Audio = "Audio";
    }
}