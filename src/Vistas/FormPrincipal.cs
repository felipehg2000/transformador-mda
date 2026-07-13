using System;
using System.Windows.Forms;
using transformador_mda.src.Controladores;
using transformador_mda.src.Dominio.Retornos;
using transformador_mda.src.Features.ExploradorFicheros;

namespace transformador_mda.src.Vistas
{
    /// <summary>
    /// Formuulario principal. Vista de la aplicación.
    /// </summary>
    public partial class FormPrincipal : Form
    {
        #region "Atributos"

        /// <summary>
        /// Controlador para conectar la vista con la lógica del generador de código.
        /// </summary>
        private Principal _controlador;

        #endregion

        #region "Constructor"

        /// <summary>
        /// Constructor de la clase, inicializa atributos
        /// </summary>
        public FormPrincipal()
        {
            InitializeComponent();

            _controlador = new Principal();
        }

        #endregion

        #region "Eventos"

        /// <summary>
        /// Evento para exportar la configuración que está usando el generador.
        /// </summary>
        /// <param name="sender">Componente que lanza el evento.</param>
        /// <param name="e">Argumentos asociados al evento.</param>
        private void exportarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _controlador.Configuracion.RutaFicheroEntrada = textBoxRutaFicheroDeEntrada.Text;

            if (_controlador.ExportarConfiguracion())
                MessageBox.Show("Fichero exportado correctamente.",
                    "Información",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
        }

        /// <summary>
        /// Evento para importar la configuración que va a usar el generador.
        /// </summary>
        /// <param name="sender">Componente que lanza el evento.</param>
        /// <param name="e">Argumentos asociados al evento.</param>
        private void importarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool resultado = _controlador.ImportarConfiguracion();
            if (resultado && _controlador.Configuracion == null)
            {
                MessageBox.Show("Ha ocurrido un error al importar el fichero.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            else if (!resultado)
                return;

            textBoxRutaRaiz.Text = "Ya no se usa, quitar cuando se modifiquen las interfaces.";
            textBoxNombreProyecto.Text = _controlador.Configuracion.Proyecto.Nombre;
            textBoxRutaFicheroDeEntrada.Text = _controlador.Configuracion.RutaFicheroEntrada;

        }

        /// <summary>
        /// Abre el explorador de directorios y rellena el textBox de ruta raíz con el directorio seleccionado, si no hay nombre de proyecto se coge el nombre del directorio.
        /// </summary>
        /// <param name="sender">Componente que lanza el evento.</param>
        /// <param name="e">Argumentos asociados al evento.</param>
        private void buttonDirectorioRaiz_Click(object sender, EventArgs e)
        {
            RetornoMultiple retorno = _controlador.BuscarDirectorioRaiz();
            if (retorno.Estado == RetornoMultiple.EnumEstado.CANCELADO_POR_USUARIO)
                return;

            textBoxRutaRaiz.Text = _controlador.Configuracion.RutaRaiz;

            if (textBoxNombreProyecto.Text != _controlador.Configuracion.Proyecto.Nombre)
                textBoxNombreProyecto.Text = _controlador.Configuracion.Proyecto.Nombre;
        }

        /// <summary>
        /// Carga el nombre del proyecto en el objeto de configuración.
        /// </summary>
        /// <param name="sender">Componente que lanza el evento.</param>
        /// <param name="e">Argumentos asociados al evento.</param>
        private void textBoxNombreProyecto_TextChanged(object sender, EventArgs e)
        {
            _controlador.Configuracion.Proyecto.Nombre = textBoxNombreProyecto.Text;
        }

        /// <summary>
        /// Llama al método que selecciona el archivo de entrada y lo procesa.
        /// </summary>
        /// <param name="sender">Componente que lanza el evento.</param>
        /// <param name="e">Argumentos asociados al evento.</param>
        private void buttonBuscarFicheroDeEntrada_Click(object sender, EventArgs e)
        {
            RetornoMultiple retorno = _controlador.BuscarFichero();
            if (!retorno.Exito)
            {
                MessageBox.Show(retorno.TextoUsuario,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            else if (retorno.Exito && retorno.Estado == RetornoMultiple.EnumEstado.CANCELADO_POR_USUARIO)
                return;

            textBoxRutaFicheroDeEntrada.Text = _controlador.Configuracion.RutaFicheroEntrada;
        }

        #endregion


        #region "Métodos privados"

        private void ActivarBotonContinuar()
        {
            if (string.IsNullOrEmpty(textBoxRutaRaiz.Text) &&
                string.IsNullOrEmpty(textBoxNombreProyecto.Text) &&
                string.IsNullOrEmpty(textBoxRutaFicheroDeEntrada.Text))
            {
                buttonContinuar.Enabled = false;
            }
            else
            {
                buttonContinuar.Enabled = true;
            }
        }

        #endregion

        private void buttonContinuar_Click(object sender, EventArgs e)
        {

        }
    }
}
