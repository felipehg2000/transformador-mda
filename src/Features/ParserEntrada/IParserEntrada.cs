using transformador_mda.src.Dominio;

namespace transformador_mda.src.Features.ParserEntrada
{
    public abstract class IParserEntrada
    {
        protected string _ruta_fichero;

        public IParserEntrada(string param_ruta_fichero)
        {
            _ruta_fichero = param_ruta_fichero;
        }

        public abstract ProyectoBase Parsear();

        protected abstract bool EsFormatoValido();

        protected abstract void LeerFichero();

        protected abstract void ProcesarElemento();

        protected abstract void ProcesarPaquete();

        protected abstract void ProcesarClase();

        protected abstract void ProcesarAtributoParametro();    

        protected abstract void ProcesarMetodo();

        protected abstract void ProcesarAsociacion();
    }
}
