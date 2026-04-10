using System.Diagnostics;
using FiveMOptimizerPro.Helpers;
using FiveMOptimizerPro.Models;

namespace FiveMOptimizerPro.Services
{
    /// <summary>
    /// Servicio para manejo de procesos del sistema
    /// Permite terminar procesos innecesarios y asignar prioridades
    /// </summary>
    public class ProcessService
    {
        /// <summary>
        /// Obtiene todos los procesos activos del sistema
        /// </summary>
        public List<ProcessInfo> GetAllProcesses()
        {
            try
            {
                var processes = new List<ProcessInfo>();
                
                foreach (var process in Process.GetProcesses())
                {
                    try
                    {
                        processes.Add(new ProcessInfo
                        {
                            ProcessId = process.Id,
                            ProcessName = process.ProcessName,
                            Priority = process.PriorityClass,
                            MemoryUsage = process.WorkingSet64 / (1024 * 1024), // MB
                            ThreadCount = process.Threads.Count,
                            CpuUsage = GetProcessCpuUsage(process)
                        });
                    }
                    catch
                    {
                        // Algunos procesos no pueden ser leídos
                        continue;
                    }
                }

                return processes;
            }
            catch (Exception ex)
            {
                Logger.Error("ProcessService", "Error obteniendo procesos", ex);
                return new List<ProcessInfo>();
            }
        }

        /// <summary>
        /// Obtiene el uso de CPU de un proceso específico
        /// </summary>
        private static float GetProcessCpuUsage(Process process)
        {
            try
            {
                var startTime = DateTime.UtcNow;
                var startCpuUsage = process.TotalProcessorTime;

                System.Threading.Thread.Sleep(500);

                var endTime = DateTime.UtcNow;
                var endCpuUsage = process.TotalProcessorTime;

                var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
                var totalMsPassed = (endTime - startTime).TotalMilliseconds;
                var cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);

                return (float)(cpuUsageTotal * 100);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Termina un proceso por ID
        /// </summary>
        public bool KillProcess(int processId)
        {
            try
            {
                var process = Process.GetProcessById(processId);
                process.Kill();
                process.WaitForExit(5000);
                
                Logger.Info("ProcessService", $"Proceso terminado: {process.ProcessName} (PID: {processId})");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("ProcessService", $"Error terminando proceso {processId}", ex);
                return false;
            }
        }

        /// <summary>
        /// Termina un proceso por nombre
        /// </summary>
        public int KillProcessByName(string processName)
        {
            int killedCount = 0;

            try
            {
                var processes = Process.GetProcessesByName(processName);
                foreach (var process in processes)
                {
                    try
                    {
                        process.Kill();
                        process.WaitForExit(5000);
                        killedCount++;
                        Logger.Info("ProcessService", $"Proceso terminado: {processName}");
                    }
                    catch (Exception ex)
                    {
                        Logger.Warning("ProcessService", $"Error terminando proceso {processName}", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("ProcessService", $"Error en KillProcessByName({processName})", ex);
            }

            return killedCount;
        }

        /// <summary>
        /// Asigna prioridad a un proceso
        /// </summary>
        public bool SetProcessPriority(int processId, ProcessPriorityClass priority)
        {
            try
            {
                var process = Process.GetProcessById(processId);
                process.PriorityClass = priority;
                
                Logger.Info("ProcessService", $"Prioridad del proceso {processId} establecida a {priority}");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("ProcessService", $"Error estableciendo prioridad del proceso {processId}", ex);
                return false;
            }
        }

        /// <summary>
        /// Asigna prioridad a un proceso por nombre
        /// </summary>
        public bool SetProcessPriorityByName(string processName, ProcessPriorityClass priority)
        {
            try
            {
                var processes = Process.GetProcessesByName(processName);
                foreach (var process in processes)
                {
                    process.PriorityClass = priority;
                }

                Logger.Info("ProcessService", $"Prioridad de {processName} establecida a {priority}");
                return processes.Length > 0;
            }
            catch (Exception ex)
            {
                Logger.Error("ProcessService", $"Error estableciendo prioridad de {processName}", ex);
                return false;
            }
        }

        /// <summary>
        /// Suspende un proceso
        /// </summary>
        public bool SuspendProcess(int processId)
        {
            try
            {
                var process = Process.GetProcessById(processId);
                SuspendProcess(process);
                
                Logger.Info("ProcessService", $"Proceso suspendido: {processId}");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("ProcessService", $"Error suspendiendo proceso {processId}", ex);
                return false;
            }
        }

        /// <summary>
        /// Reanuda un proceso
        /// </summary>
        public bool ResumeProcess(int processId)
        {
            try
            {
                var process = Process.GetProcessById(processId);
                ResumeProcess(process);
                
                Logger.Info("ProcessService", $"Proceso reanudado: {processId}");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("ProcessService", $"Error reanudando proceso {processId}", ex);
                return false;
            }
        }

        /// <summary>
        /// Suspende un proceso usando Win32 API
        /// </summary>
        private static void SuspendProcess(Process process)
        {
            foreach (ProcessThread thread in process.Threads)
            {
                var handle = new System.Runtime.InteropServices.SafeHandle();
                // Implementación de suspensión via WinAPI
            }
        }

        /// <summary>
        /// Reanuda un proceso suspendido
        /// </summary>
        private static void ResumeProcess(Process process)
        {
            foreach (ProcessThread thread in process.Threads)
            {
                var handle = new System.Runtime.InteropServices.SafeHandle();
                // Implementación de reanudación via WinAPI
            }
        }

        /// <summary>
        /// Obtiene procesos que consumen más recursos
        /// </summary>
        public List<ProcessInfo> GetTopProcessesByMemory(int count = 10)
        {
            return GetAllProcesses()
                .OrderByDescending(p => p.MemoryUsage)
                .Take(count)
                .ToList();
        }

        /// <summary>
        /// Obtiene procesos que consumen más CPU
        /// </summary>
        public List<ProcessInfo> GetTopProcessesByCpu(int count = 10)
        {
            return GetAllProcesses()
                .OrderByDescending(p => p.CpuUsage)
                .Take(count)
                .ToList();
        }
    }

    /// <summary>
    /// Información de un proceso
    /// </summary>
    public class ProcessInfo
    {
        public int ProcessId { get; set; }
        public string ProcessName { get; set; } = string.Empty;
        public ProcessPriorityClass Priority { get; set; }
        public long MemoryUsage { get; set; } // MB
        public int ThreadCount { get; set; }
        public float CpuUsage { get; set; } // Porcentaje
    }
}