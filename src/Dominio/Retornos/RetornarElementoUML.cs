using transformador_mda.src.Dominio.ElementosUML;

namespace transformador_mda.src.Dominio.Retornos
{
    public class RetornarElementoUML : RetornoBase
    {
        public object Elemento { get; set; }

        public RetornarElementoUML(bool param_exito, 
            string param_texto_usuario, 
            ElementoBase param_elemento, 
            string param_texto_desarrollador = null) : 
            base(param_exito, 
                param_texto_usuario, 
                param_texto_desarrollador)
        {
            Elemento = param_elemento;
        }
    }
}
