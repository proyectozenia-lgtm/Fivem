namespace FiveMOptimizerPro.Models
{
    /// <summary>
    /// Métricas del sistema en tiempo real
    /// </summary>
    public class SystemMetrics
    {
        /// <summary>
        /// Porcentaje de uso de CPU (0-100)
        /// </summary>
        public float CpuUsage { get; set; }

        /// <summary>
        /// Temperatura del CPU en grados Celsius
        /// </summary>
        public float CpuTemperature { get; set; }

        /// <summary>
        /// Velocidad del reloj del CPU en MHz
        /// </summary>
        public float CpuClockSpeed { get; set; }

        /// <summary>
        /// Porcentaje de uso de RAM (0-100)
        /// </summary>
        public float RamUsage { get; set; }

        /// <summary>
        /// RAM total disponible en GB
        /// </summary>
        public float RamTotal { get; set; }

        /// <summary>
        /// RAM utilizada en GB
        /// </summary>
        public float RamUsed { get; set; }

        /// <summary>
        /// Porcentaje de uso de GPU (0-100)
        /// </summary>
        public float GpuUsage { get; set; }

        /// <summary>
        /// Temperatura de la GPU en grados Celsius
        /// </summary>
        public float GpuTemperature { get; set; }

        /// <summary>
        /// VRAM utilizada en GB
        /// </summary>
        public float VramUsed { get; set; }

        /// <summary>
        /// VRAM total disponible en GB
        /// </summary>
        public float VramTotal { get; set; }

        /// <summary>
        /// Uso de disco en porcentaje (0-100)
        /// </summary>
        public float DiskUsage { get; set; }

        /// <summary>
        /// Velocidad de lectura de disco en MB/s
        /// </summary>
        public float DiskReadSpeed { get; set; }

        /// <summary>
        /// Velocidad de escritura de disco en MB/s
        /// </summary>
        public float DiskWriteSpeed { get; set; }

        /// <summary>
        /// Velocidad de red descendente en Mbps
        /// </summary>
        public float NetworkDownSpeed { get; set; }

        /// <summary>
        /// Velocidad de red ascendente en Mbps
        /// </summary>
        public float NetworkUpSpeed { get; set; }

        /// <summary>
        /// Ping actual en milisegundos
        /// </summary>
        public float Ping { get; set; }

        /// <summary>
        /// FPS actual en el juego (si está disponible)
        /// </summary>
        public float CurrentFps { get; set; }

        /// <summary>
        /// Marca de tiempo
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}