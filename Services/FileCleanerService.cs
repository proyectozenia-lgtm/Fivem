using System.IO;
using FiveMOptimizerPro.Helpers;

namespace FiveMOptimizerPro.Services
{
    /// <summary>
    /// Servicio para limpieza de archivos temporales y caché
    /// Libera espacio en disco eliminando archivos innecesarios
    /// </summary>
    public class FileCleanerService
    {
        /// <summary>
        /// Estructura de resultado de limpieza
        /// </summary>
        public class CleanupResult
        {
            public long BytesFreed { get; set; }
            public int FilesDeleted { get; set; }
            public int DirectoriesDeleted { get; set; }
            public List<string> Errors { get; set; } = new();

            public string GetSizeFreedFormatted()
            {
                if (BytesFreed < 1024)
                    return $"{BytesFreed} B";
                else if (BytesFreed < 1024 * 1024)
                    return $"{BytesFreed / 1024.0:F2} KB";
                else if (BytesFreed < 1024 * 1024 * 1024)
                    return $"{BytesFreed / (1024.0 * 1024):F2} MB";
                else
                    return $"{BytesFreed / (1024.0 * 1024 * 1024):F2} GB";
            }
        }

        /// <summary>
        /// Limpia archivos temporales de Windows
        /// </summary>
        public async Task<CleanupResult> CleanWindowsTemp()
        {
            var result = new CleanupResult();

            var tempPath = Path.GetTempPath();
            await CleanDirectoryAsync(tempPath, result);

            Logger.Info("FileCleanerService", $"Temp limpiado: {result.GetSizeFreedFormatted()}");
            return result;
        }

        /// <summary>
        /// Limpia caché de FiveM
        /// </summary>
        public async Task<CleanupResult> CleanFiveMCache()
        {
            var result = new CleanupResult();

            if (Directory.Exists(Constants.FiveMCachePath))
            {
                await CleanDirectoryAsync(Constants.FiveMCachePath, result);
                Logger.Info("FileCleanerService", $"Caché de FiveM limpiado: {result.GetSizeFreedFormatted()}");
            }

            return result;
        }

        /// <summary>
        /// Limpia el caché del navegador Chrome
        /// </summary>
        public async Task<CleanupResult> CleanChromeCache()
        {
            var result = new CleanupResult();

            var chromeCache = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Google\\Chrome\\User Data\\Default\\Cache"
            );

            if (Directory.Exists(chromeCache))
            {
                await CleanDirectoryAsync(chromeCache, result);
                Logger.Info("FileCleanerService", $"Caché de Chrome limpiado: {result.GetSizeFreedFormatted()}");
            }

            return result;
        }

        /// <summary>
        /// Limpia el caché de Windows Update
        /// </summary>
        public async Task<CleanupResult> CleanWindowsUpdateCache()
        {
            var result = new CleanupResult();

            var updatePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Windows),
                "SoftwareDistribution\\Download"
            );

            if (Directory.Exists(updatePath))
            {
                await CleanDirectoryAsync(updatePath, result);
                Logger.Info("FileCleanerService", $"Caché de Windows Update limpiado: {result.GetSizeFreedFormatted()}");
            }

            return result;
        }

        /// <summary>
        /// Limpia la papelera de reciclaje
        /// </summary>
        public bool EmptyRecycleBin()
        {
            try
            {
                var shell = new Shell.Shell();
                var recycle = shell.NameSpace(10);
                
                if (recycle.Items().Count > 0)
                {
                    recycle.Items().RemoveAll();
                    Logger.Info("FileCleanerService", "Papelera de reciclaje vaciada");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.Warning("FileCleanerService", "Error vaciando papelera", ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Limpia archivos prefetch (StartUp Cache)
        /// </summary>
        public async Task<CleanupResult> CleanPrefetchCache()
        {
            var result = new CleanupResult();

            var prefetchPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Windows),
                "Prefetch"
            );

            if (Directory.Exists(prefetchPath))
            {
                // Solo eliminar archivos .pf más antiguos de 7 días
                var now = DateTime.Now;
                var files = Directory.GetFiles(prefetchPath, "*.pf");

                foreach (var file in files)
                {
                    try
                    {
                        var fileInfo = new FileInfo(file);
                        if ((now - fileInfo.LastAccessTime).TotalDays > 7)
                        {
                            var size = fileInfo.Length;
                            File.Delete(file);
                            result.BytesFreed += size;
                            result.FilesDeleted++;
                        }
                    }
                    catch (Exception ex)
                    {
                        result.Errors.Add($"Error limpiando {file}: {ex.Message}");
                    }
                }

                Logger.Info("FileCleanerService", $"Prefetch limpiado: {result.GetSizeFreedFormatted()}");
            }

            return result;
        }

        /// <summary>
        /// Limpia archivos de registro temporal
        /// </summary>
        public async Task<CleanupResult> CleanEventLogs()
        {
            var result = new CleanupResult();

            try
            {
                // Limpiar registros de eventos más antiguos de 30 días
                var now = DateTime.Now;
                var appDataPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "FiveMOptimizerPro\\Logs"
                );

                if (Directory.Exists(appDataPath))
                {
                    var logs = Directory.GetFiles(appDataPath, "*.log");
                    foreach (var log in logs)
                    {
                        var fileInfo = new FileInfo(log);
                        if ((now - fileInfo.CreationTime).TotalDays > 30)
                        {
                            var size = fileInfo.Length;
                            File.Delete(log);
                            result.BytesFreed += size;
                            result.FilesDeleted++;
                        }
                    }
                }

                Logger.Info("FileCleanerService", $"Logs limpios: {result.GetSizeFreedFormatted()}");
            }
            catch (Exception ex)
            {
                Logger.Error("FileCleanerService", "Error limpiando logs", ex);
                result.Errors.Add($"Error limpiando logs: {ex.Message}");
            }

            return result;
        }

        /// <summary>
        /// Limpia todos los cachés y archivos temporales
        /// </summary>
        public async Task<CleanupResult> CleanAll()
        {
            var totalResult = new CleanupResult();

            Logger.Info("FileCleanerService", "Iniciando limpieza completa...");

            var tasks = new List<Task<CleanupResult>>
            {
                CleanWindowsTemp(),
                CleanFiveMCache(),
                CleanChromeCache(),
                CleanPrefetchCache(),
                CleanEventLogs()
            };

            var results = await Task.WhenAll(tasks);

            foreach (var result in results)
            {
                totalResult.BytesFreed += result.BytesFreed;
                totalResult.FilesDeleted += result.FilesDeleted;
                totalResult.DirectoriesDeleted += result.DirectoriesDeleted;
                totalResult.Errors.AddRange(result.Errors);
            }

            Logger.Info("FileCleanerService", $"Limpieza completa finalizada: {totalResult.GetSizeFreedFormatted()}");
            return totalResult;
        }

        /// <summary>
        /// Limpia un directorio recursivamente
        /// </summary>
        private async Task CleanDirectoryAsync(string path, CleanupResult result)
        {
            try
            {
                var dirInfo = new DirectoryInfo(path);

                // Eliminar archivos
                foreach (var file in dirInfo.GetFiles())
                {
                    try
                    {
                        result.BytesFreed += file.Length;
                        file.Delete();
                        result.FilesDeleted++;
                    }
                    catch (Exception ex)
                    {
                        result.Errors.Add($"Error eliminando {file.FullName}: {ex.Message}");
                    }
                }

                // Eliminar subdirectorios
                foreach (var subDir in dirInfo.GetDirectories())
                {
                    try
                    {
                        await CleanDirectoryAsync(subDir.FullName, result);
                        subDir.Delete(false);
                        result.DirectoriesDeleted++;
                    }
                    catch (Exception ex)
                    {
                        result.Errors.Add($"Error eliminando {subDir.FullName}: {ex.Message}");
                    }
                }

                await Task.Delay(10);
            }
            catch (Exception ex)
            {
                Logger.Error("FileCleanerService", $"Error limpiando directorio {path}", ex);
                result.Errors.Add($"Error limpiando {path}: {ex.Message}");
            }
        }
    }
}