# 🎮 FiveM Optimizer Pro 2026

**Professional Windows Optimization Suite for Gaming**

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Version](https://img.shields.io/badge/Version-2026.01.001-blue.svg)](https://github.com/nexxysg-pixel/FiveMOptimizerPro-2026/releases)
[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4.svg)](https://dotnet.microsoft.com/download)

---

## 📋 Descripción

FiveM Optimizer Pro es una **suite completa de optimización** especializada en gaming, con enfoque especial en **FiveM**. Contiene **150+ optimizaciones reales** respaldadas por documentación técnica oficial (Microsoft Docs, NVIDIA, AMD, Intel).

### Características principales:

- ✅ **150+ Optimizaciones reales** - Sin placebos, todas con fuente técnica
- ✅ **Monitoreo en tiempo real** - CPU, GPU, RAM, Disco, Red
- ✅ **5 Perfiles predefinidos** - Gaming Extreme, Streaming, Default, Battery Saver, Custom
- ✅ **One-Click Revert** - Revierte todos los cambios en un clic
- ✅ **Gestor de Procesos** - Termina procesos innecesarios
- ✅ **Limpiador de archivos** - Cache, Temp, Logs
- ✅ **Log completo** - Historial detallado de todas las operaciones
- ✅ **Tema Oscuro/Claro** - Interface moderna y profesional
- ✅ **Portable** - No requiere instalación
- ✅ **Seguro** - Backup automático de cambios en registro

---

## 💻 Requisitos del Sistema

### Hardware mínimo:
- **Procesador:** Intel i5 6ª gen o AMD Ryzen 5 1600
- **RAM:** 8 GB
- **Disco:** 500 MB espacio libre
- **GPU:** Cualquier GPU compatible (NVIDIA RTX 30xx+ recomendado)

### Hardware objetivo (optimizado):
- **Procesador:** AMD Ryzen 7 7435HS
- **GPU:** NVIDIA RTX 3050 4GB
- **RAM:** 16 GB DDR5

### Software:
- **Windows:** 10 (21H2+) o Windows 11
- **Runtime:** .NET 8.0 Windows Desktop Runtime
- **Permisos:** Administrador (requerido)

---

## 🚀 Instalación y Uso

### Opción 1: Descargar Ejecutable (Recomendado)

```bash
# Descargar la versión compilada desde Releases
https://github.com/nexxysg-pixel/FiveMOptimizerPro-2026/releases

# Ejecutar como administrador
FiveMOptimizerPro.exe
```

### Opción 2: Compilar desde fuente

#### Requisitos previos:
- Visual Studio 2022 Community/Professional
- .NET 8.0 SDK
- Git

#### Pasos:

```bash
# 1. Clonar repositorio
git clone https://github.com/nexxysg-pixel/FiveMOptimizerPro-2026.git
cd FiveMOptimizerPro-2026

# 2. Restaurar dependencias
dotnet restore

# 3. Compilar en Release
dotnet publish -c Release -r win-x64 --self-contained false

# 4. Ejecutar
bin\Release\net8.0-windows\FiveMOptimizerPro.exe
```

---

## 🎯 Categorías de Optimizaciones

### 1. CPU (16 optimizaciones)
- Deshabilitar Core Parking
- Plan Ultimate Performance
- Deshabilitar C-States
- Prioridad de procesos foreground
- Timer resolution baja
- DMA Remapping disable
- Y más...

**Fuente:** [Microsoft Docs - Power Management](https://docs.microsoft.com/en-us/windows-hardware/design/device-experiences/oem-power-management)

### 2. GPU (18 optimizaciones)
- NVIDIA Low Latency Mode
- Deshabilitar HAGS
- Deshabilitar MPO
- Deshabilitar ULPS
- PCIe ASPM
- GPU Memory Optimization
- Y más...

**Fuentes:**
- [NVIDIA Performance Tuning](https://nvidia.custhelp.com/app/answers/detail/a_id/3050)
- [AMD Ryzen Optimization](https://www.amd.com/en/support)

### 3. RAM (12 optimizaciones)
- Large System Cache
- Disable Paging Executive
- Clear Standby Memory
- Virtual Memory Optimization
- Y más...

**Fuente:** [Microsoft - Memory Management](https://docs.microsoft.com/en-us/windows/win32/memory/memory-management)

### 4. Red (15 optimizaciones)
- Deshabilitar Nagle Algorithm
- TCP AutoTuning
- QoS Configuration
- Network Throttling Index
- Flush DNS/ARP
- Y más...

**Fuente:** [RFC 896 - Nagle Algorithm](https://tools.ietf.org/html/rfc896)

### 5. Servicios (20 optimizaciones)
- Desactivar DiagTrack
- Desactivar Windows Search
- Desactivar SysMain
- Desactivar Xbox Services
- Desactivar Print Spooler
- Y más...

**Fuente:** [Microsoft Services Documentation](https://docs.microsoft.com/en-us/windows/win32/services/services)

### 6. Procesos (12 optimizaciones)
- Terminar Chrome
- Terminar Discord
- Terminar OneDrive
- Terminar Spotify
- Terminar Steam
- Y más...

### 7. Almacenamiento (10 optimizaciones)
- NTFS Last Access Update
- TRIM Optimization
- Defrag Disable
- Y más...

**Fuente:** [NTFS Performance Tuning](https://docs.microsoft.com/en-us/windows/win32/fileio/disk-defragmentation)

### 8. FiveM (15 optimizaciones)
- Limpiar cache
- Optimizar settings.xml
- Limitar VRAM a 3GB
- Cache cleaner automático
- Y más...

**Fuente:** [FiveM Documentation](https://docs.fivem.net/)

### 9. Sistema General (16 optimizaciones)
- Deshabilitar animaciones
- Reducir efectos visuales
- Optimizar inicio
- Y más...

### 10. Audio (6 optimizaciones)
- Audio Priority
- Exclusive Mode
- Y más...

---

## 📊 Monitoreo en Tiempo Real

### Métricas disponibles:

| Métrica | Descripción | Unidad |
|---------|-------------|--------|
| CPU Usage | Porcentaje de uso del procesador | % |
| CPU Temp | Temperatura del procesador | °C |
| CPU Clock | Velocidad del reloj | MHz |
| GPU Usage | Porcentaje de uso de GPU | % |
| GPU Temp | Temperatura de GPU | °C |
| VRAM Used | Memoria de video utilizada | GB |
| RAM Usage | Porcentaje de uso de RAM | % |
| RAM Used | RAM utilizada | GB |
| Disk Usage | Uso del disco duro | % |
| Disk Speed | Velocidad de lectura/escritura | MB/s |
| Network Speed | Velocidad de red | Mbps |
| Ping | Latencia de red | ms |

---

## 🎮 Perfiles de Optimización

### 1. Gaming Extreme 🚀
**Para máximo rendimiento en FiveM y juegos competitivos**

Incluye:
- Todas las optimizaciones de CPU (máxima rendimiento)
- Optimizaciones GPU avanzadas
- Network low latency
- Desactivar servicios no esenciales

**Recomendado para:** Jugadores competitivos, streamers, creadores de contenido

### 2. Streaming 📹
**Balance entre FPS y disponibilidad de CPU para captura/streaming**

Incluye:
- Optimizaciones CPU moderadas
- GPU optimization
- Network priority
- Servicios reducidos

**Recomendado para:** Streamers, grabadores de video

### 3. Default ⚡
**Recomendado - Buen balance sin riesgos**

Incluye:
- Optimizaciones CPU y GPU seguras
- Network optimization
- File cleanup
- Service optimization

**Recomendado para:** Jugadores casuales, uso mixto

### 4. Battery Saver 🔋
**Bajo consumo de energía**

Incluye:
- Desactivar servicios innecesarios
- Reducir efectos visuales
- Power saving mode

**Recomendado para:** Laptops, sesiones largas de gaming

---

## ⚙️ Configuración Avanzada

### Archivo de configuración:
```
C:\Users\<USERNAME>\AppData\Roaming\FiveMOptimizerPro\config.json
```

### Logs:
```
C:\Users\<USERNAME>\AppData\Roaming\FiveMOptimizerPro\Logs\FiveMOptimizerPro_YYYY-MM-DD.log
```

### Backup de registro:
La aplicación crea automáticamente backups de los valores modificados en el registro de Windows, permitiendo revertir cambios sin riesgo.

---

## 🛡️ Seguridad y Reversión

### Mecanismo de Backup:
1. Antes de aplicar cualquier optimización, el valor original se guarda
2. Se mantiene un historial completo en `OptimizationHistory.json`
3. En cualquier momento, puedes revertir todos los cambios con un clic

### One-Click Revert:
```
Interfaz → Botón "↩️ Revertir Todo"
```

### Crear punto de restauración (Windows):
```powershell
# Ejecutar en PowerShell como admin
wmic.exe shadowcopy call create Volume="C:\"
```

---

## 🐛 Solución de Problemas

### Problema: La aplicación requiere permisos de administrador
**Solución:**
```bash
# Click derecho → Ejecutar como administrador
# O crear acceso directo con permisos elevados
```

### Problema: FiveM se ejecuta lento después de optimizar
**Solución:**
1. Click en "↩️ Revertir Todo"
2. Selecciona el perfil "Default"
3. Aplica nuevamente

### Problema: No se detecta GPU
**Solución:**
1. Actualiza los drivers de GPU
2. Reinicia la aplicación
3. Verifica compatibilidad de GPU en Device Manager

### Problema: Cambios en registro no se aplican
**Solución:**
1. Requiere permisos de administrador
2. Algunos valores requieren reinicio
3. Cierra programas que usen el registro (antivirus, etc.)

---

## 📖 Documentación Técnica

### Fuentes utilizadas:
- [Microsoft Docs - Windows Performance](https://docs.microsoft.com/)
- [NVIDIA Developer Blog](https://developer.nvidia.com/)
- [AMD Ryzen Documentation](https://www.amd.com/)
- [Intel Core Tuning](https://ark.intel.com/)
- [FiveM Official Docs](https://docs.fivem.net/)
- [RFC 896 - Nagle Algorithm](https://tools.ietf.org/html/rfc896)
- [Intel Performance Counter Monitor](https://www.intel.com/content/www/us/en/developer/articles/tool/performance-counter-monitor.html)

### Archivos modificados:
```
Registro de Windows:
- HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Processor
- HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters
- HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers

Archivos de sistema:
- Servicios Windows (.sys)
- Configuración de energía (powercfg)
- Configuración de red (netsh)
```

---

## 🤝 Contribuciones

Las contribuciones son bienvenidas. Por favor:

1. Fork el repositorio
2. Crea una rama feature (`git checkout -b feature/AmazingFeature`)
3. Commit cambios (`git commit -m 'Add AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

### Reportar bugs:
[GitHub Issues](https://github.com/nexxysg-pixel/FiveMOptimizerPro-2026/issues)

---

## 📝 Licencia

Este proyecto está bajo la Licencia MIT. Ver [`LICENSE`](LICENSE) para más detalles.

---

## 👨‍💻 Autor

**nexxysg-pixel**
- GitHub: [@nexxysg-pixel](https://github.com/nexxysg-pixel)
- Email: nexxy.sg@gmail.com

---

## ⚠️ Disclaimer

**IMPORTANTE:** Esta herramienta modifica valores del Registro de Windows. Aunque todas las operaciones son reversibles:

1. **Crear punto de restauración** antes de usar
2. **Hacer backup** de datos importantes
3. **Usar bajo tu propio riesgo**
4. **No recomendado para usuarios novatos** sin conocimiento técnico

El autor no se responsabiliza por daños causados por mal uso.

---

## 🙏 Agradecimientos

- Microsoft Windows Team
- NVIDIA Developer Relations
- AMD Ryzen Engineering
- Community de FiveM
- Inspiración en herramientas como:
  - O&O ShutUp++
  - Chris Titus Tech Tool
  - W10Privacy

---

## 📊 Estadísticas del Proyecto

- **Líneas de código:** 5000+
- **Optimizaciones:** 150+
- **Categorías:** 10
- **Perfiles:** 5
- **Tiempo de desarrollo:** 2 semanas

---

**Última actualización:** 10/04/2026  
**Estado:** Estable ✅  
**Versión:** 2026.01.001