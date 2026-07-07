using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace transformador_mda.src.Dominio.Retornos
{
    public class RetornarElementoUML : RetornoBase
    {
        public object Elemento { get; set; }

        public RetornarElementoUML(bool param_exito, 
            string param_texto_usuario, 
            Elemento param_elemento, 
            string param_texto_desarrollador = null) : 
            base(param_exito, 
                param_texto_usuario, 
                param_texto_desarrollador)
        {
            Elemento = param_elemento;
        }
    }
}
