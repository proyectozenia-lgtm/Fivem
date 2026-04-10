namespace FiveMOptimizerPro.Models
{
    /// <summary>
    /// Resultado de la aplicación de una optimización
    /// </summary>
    public class OptimizationResult
    {
        /// <summary>
        /// ID de la optimización
        /// </summary>
        public string OptimizationId { get; set; } = string.Empty;

        /// <summary>
        /// Nombre de la optimización
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// True si se aplicó correctamente
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Mensaje de error si la aplicación falló
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// Fecha y hora de aplicación
        /// </summary>
        public DateTime AppliedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// Cambio anterior para poder revertir
        /// </summary>
        public string PreviousValue { get; set; } = string.Empty;

        /// <summary>
        /// Nuevo valor aplicado
        /// </summary>
        public string NewValue { get; set; } = string.Empty;

        /// <summary>
        /// Requiere reinicio
        /// </summary>
        public bool RequiresReboot { get; set; }

        /// <summary>
        /// Detalles técnicos de la operación
        /// </summary>
        public string Details { get; set; } = string.Empty;
    }
}