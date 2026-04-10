namespace FiveMOptimizerPro.Models
{
    /// <summary>
    /// Perfil de optimización guardable y cargable
    /// </summary>
    public class Profile
    {
        /// <summary>
        /// Nombre del perfil
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Descripción del perfil
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de creación
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Fecha de última modificación
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// IDs de optimizaciones incluidas en este perfil
        /// </summary>
        public List<string> OptimizationIds { get; set; } = new();

        /// <summary>
        /// Tipo de perfil predefinido
        /// </summary>
        public ProfileType ProfileType { get; set; }

        /// <summary>
        /// Versión de la aplicación cuando se creó
        /// </summary>
        public string Version { get; set; } = "2026.01.001";
    }

    /// <summary>
    /// Tipos de perfil predefinidos
    /// </summary>
    public enum ProfileType
    {
        Custom,
        GamingExtreme,
        Streaming,
        Default,
        BatterySaver
    }
}