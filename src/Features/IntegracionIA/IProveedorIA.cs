using System.Threading.Tasks;
using transformador_mda.src.Dominio.ElementosUML;
using transformador_mda.src.Dominio.Retornos;

namespace transformador_mda.src.Features.IntegracionIA
{
    public interface IProveedorIA
    {
        Task<RetornoBase> GenerarCodigo(Clase param_clase,
            string param_ruta_json,
            string param_esqueleto);
    }
}
