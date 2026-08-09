using System.Threading.Tasks;
using transformador_mda.src.Dominio.ElementosUML;
using transformador_mda.src.Dominio.Retornos;

namespace transformador_mda.src.Features.IntegracionIA
{
    public interface IProveedorIA
    {
        Task<RetornoBase> GenerarCodigo(ProyectoBase param_proyecto,
            Clase param_clase,
            string param_esqueleto);
    }
}
