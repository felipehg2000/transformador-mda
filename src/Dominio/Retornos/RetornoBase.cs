using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace transformador_mda.src.Dominio.Retornos
{
    public class RetornoBase
    {
        public bool Exito { get; set; }

        public string TextoUsuario { get; set; }

        public string TextoDesarrollador { get; set; }

        public RetornoBase(bool param_exito, string param_texto_usuario, string param_texto_desarrollador = null)
        {
            Exito = param_exito;
            TextoUsuario = param_texto_usuario;
            TextoDesarrollador = param_texto_desarrollador;
        }
    }
}
