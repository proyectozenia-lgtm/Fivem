namespace FiveMOptimizerPro.Models
{
    /// <summary>
    /// Representa una optimización individual del sistema
    /// </summary>
    public class Optimization
    {
        /// <summary>
        /// Identificador único de la optimización
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Nombre descriptivo de la optimización
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Descripción detallada de qué hace la optimización
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Categoría (CPU, GPU, RAM, Network, Services, etc.)
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Documento fuente o referencia técnica
        /// </summary>
        public string Source { get; set; } = string.Empty;

        /// <summary>
        /// Impacto estimado: Low, Medium, High
        /// </summary>
        public string ImpactLevel { get; set; } = "Medium";

        /// <summary>
        /// True si la optimización está habilitada
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Función a ejecutar para aplicar la optimización
        /// </summary>
        public Func<Task<bool>>? ApplyOptimization { get; set; }

        /// <summary>
        /// Función a ejecutar para revertir la optimización
        /// </summary>
        public Func<Task<bool>>? RevertOptimization { get; set; }

        /// <summary>
        /// Riesgo de inestabilidad: Low, Medium, High
        /// </summary>
        public string RiskLevel { get; set; } = "Low";

        /// <summary>
        /// Requiere reinicio del sistema
        /// </summary>
        public bool RequiresReboot { get; set; }

        /// <summary>
        /// Solo compatible con Windows 10
        /// </summary>
        public bool Windows10Only { get; set; }

        /// <summary>
        /// Solo compatible con Windows 11
        /// </summary>
        public bool Windows11Only { get; set; }
    }
}