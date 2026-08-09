using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using transformador_mda.src.Dominio.Retornos;

namespace transformador_mda.src.Features.IntegracionIA.Dominio
{
    internal class RetornoFicheroIA : RetornoBase
    {
        internal InformacionFichero InformacionFichero { get; set; }

        public RetornoFicheroIA() : base()
        {
            InformacionFichero = new InformacionFichero();
        }

        public RetornoFicheroIA(bool param_exito, 
            string param_mensaje_usuario, 
            string param_mensaje_desarrollador, 
            InformacionFichero param_informacion_fichero) : base(param_exito, 
                param_mensaje_usuario, 
                param_mensaje_desarrollador)
        {
            InformacionFichero = param_informacion_fichero;
        }
    }
}
