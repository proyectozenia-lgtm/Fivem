using Microsoft.Win32;
using FiveMOptimizerPro.Models;
using FiveMOptimizerPro.Helpers;

namespace FiveMOptimizerPro.Services
{
    /// <summary>
    /// Servicio para manipulación segura del registro de Windows
    /// Todas las operaciones se registran en log para poder revertir
    /// </summary>
    public class RegistryService
    {
        private readonly Dictionary<string, (string OriginalValue, RegistryValueKind Kind)> _valueBackup = new();

        /// <summary>
        /// Obtiene un valor del registro de Windows
        /// </summary>
        public object? GetRegistryValue(string keyPath, string valueName, object? defaultValue = null)
        {
            try
            {
                using (var key = Registry.LocalMachine.OpenSubKey(ExtractKeyPath(keyPath)))
                {
                    if (key == null)
                    {
                        Logger.Warning("RegistryService", $"Clave del registro no encontrada: {keyPath}");
                        return defaultValue;
                    }

                    var value = key.GetValue(valueName, defaultValue);
                    Logger.Debug("RegistryService", $"Valor leído: {keyPath}\\{valueName} = {value}");
                    return value;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("RegistryService", $"Error leyendo registro: {keyPath}\\{valueName}", ex);
                return defaultValue;
            }
        }

        /// <summary>
        /// Establece un valor en el registro de Windows
        /// Realiza backup automático para poder revertir
        /// </summary>
        public bool SetRegistryValue(string keyPath, string valueName, object value, RegistryValueKind valueKind)
        {
            try
            {
                // Hacer backup del valor anterior
                var backupKey = $"{keyPath}\\{valueName}";
                if (!_valueBackup.ContainsKey(backupKey))
                {
                    var originalValue = GetRegistryValue(keyPath, valueName)?.ToString() ?? string.Empty;
                    _valueBackup[backupKey] = (originalValue, valueKind);
                }

                using (var key = Registry.LocalMachine.OpenSubKey(ExtractKeyPath(keyPath), writable: true))
                {
                    if (key == null)
                    {
                        Logger.Warning("RegistryService", $"No se puede abrir clave del registro: {keyPath}");
                        return false;
                    }

                    key.SetValue(valueName, value, valueKind);
                    Logger.Info("RegistryService", $"Valor establecido: {keyPath}\\{valueName} = {value}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("RegistryService", $"Error estableciendo valor en registro: {keyPath}\\{valueName}", ex);
                return false;
            }
        }

        /// <summary>
        /// Crea una clave del registro si no existe
        /// </summary>
        public bool CreateRegistryKey(string keyPath)
        {
            try
            {
                var path = ExtractKeyPath(keyPath);
                Registry.LocalMachine.CreateSubKey(path);
                Logger.Info("RegistryService", $"Clave del registro creada: {keyPath}");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("RegistryService", $"Error creando clave del registro: {keyPath}", ex);
                return false;
            }
        }

        /// <summary>
        /// Elimina un valor del registro
        /// </summary>
        public bool DeleteRegistryValue(string keyPath, string valueName)
        {
            try
            {
                // Hacer backup antes de eliminar
                var backupKey = $"{keyPath}\\{valueName}";
                if (!_valueBackup.ContainsKey(backupKey))
                {
                    var originalValue = GetRegistryValue(keyPath, valueName)?.ToString() ?? string.Empty;
                    _valueBackup[backupKey] = (originalValue, RegistryValueKind.String);
                }

                using (var key = Registry.LocalMachine.OpenSubKey(ExtractKeyPath(keyPath), writable: true))
                {
                    if (key == null)
                    {
                        return false;
                    }

                    key.DeleteValue(valueName, false);
                    Logger.Info("RegistryService", $"Valor eliminado: {keyPath}\\{valueName}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("RegistryService", $"Error eliminando valor del registro: {keyPath}\\{valueName}", ex);
                return false;
            }
        }

        /// <summary>
        /// Revierte un valor a su estado anterior (backup)
        /// </summary>
        public bool RevertRegistryValue(string keyPath, string valueName)
        {
            try
            {
                var backupKey = $"{keyPath}\\{valueName}";
                if (!_valueBackup.ContainsKey(backupKey))
                {
                    Logger.Warning("RegistryService", $"No hay backup disponible para: {backupKey}");
                    return false;
                }

                var (originalValue, kind) = _valueBackup[backupKey];

                if (string.IsNullOrEmpty(originalValue))
                {
                    DeleteRegistryValue(keyPath, valueName);
                }
                else
                {
                    SetRegistryValue(keyPath, valueName, originalValue, kind);
                }

                _valueBackup.Remove(backupKey);
                Logger.Info("RegistryService", $"Valor revertido: {keyPath}\\{valueName}");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("RegistryService", $"Error revirtiendo valor: {keyPath}\\{valueName}", ex);
                return false;
            }
        }

        /// <summary>
        /// Revierte todos los cambios registrados
        /// </summary>
        public async Task<bool> RevertAllChanges()
        {
            try
            {
                Logger.Info("RegistryService", "Revirtiendo todos los cambios del registro...");
                
                foreach (var backupKey in _valueBackup.Keys.ToList())
                {
                    var parts = backupKey.LastIndexOf('\\');
                    var keyPath = backupKey.Substring(0, parts);
                    var valueName = backupKey.Substring(parts + 1);
                    
                    RevertRegistryValue(keyPath, valueName);
                    await Task.Delay(100); // Pequeño delay entre operaciones
                }

                _valueBackup.Clear();
                Logger.Info("RegistryService", "Todos los cambios han sido revertidos");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("RegistryService", "Error revirtiendo todos los cambios", ex);
                return false;
            }
        }

        /// <summary>
        /// Extrae la ruta de la clave sin el prefijo HKEY_LOCAL_MACHINE
        /// </summary>
        private static string ExtractKeyPath(string keyPath)
        {
            if (keyPath.StartsWith(@"HKEY_LOCAL_MACHINE\"))
            {
                return keyPath.Substring(@"HKEY_LOCAL_MACHINE\".Length);
            }

            return keyPath;
        }
    }
}