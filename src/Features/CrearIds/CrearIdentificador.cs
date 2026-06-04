using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace transformador_mda.src.Features.CrearIds
{
    public class CrearIdentificador
    {
        static public string CrearGUID()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
