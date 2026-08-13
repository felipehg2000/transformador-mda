using System.Threading.Tasks;
using System.Windows.Forms;
using transformador_mda.src.Dominio.ElementosUML;
using transformador_mda.src.Dominio.Retornos;

namespace transformador_mda.src.Features.IntegracionIA
{
    /// <summary>
    /// Interfaz a implementar por los distintos proveedores de IA para habilitar el polimorfismo y generar un patrón de factoría abstracta.
    /// </summary>
    public interface IProveedorIA
    {
        /// <summary>
        /// Función principal de los proveedores de IA, para generar el código de una clase concreta.
        /// </summary>
        /// <param name="param_proyecto">Información del proyecto completo.</param>
        /// <param name="param_clase">Información de la clase cuyo código se debe generar.</param>
        /// <param name="param_esqueleto">Código esqueleto de la clase generado. La IA se encargará de completar este esqueleto.</param>
        /// <param name="param_barra_progreso">Barra visual de progreso para que el usuario sepa que el programa no se ha quedado colgado.</param>
        /// <returns></returns>
        Task<RetornoBase> GenerarCodigo(ProyectoBase param_proyecto,
            Clase param_clase,
            string param_esqueleto,
            ToolStripProgressBar param_barra_progreso);
    }
}
