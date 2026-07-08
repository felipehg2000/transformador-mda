using System;
using System.Windows.Forms;
using transformador_mda.src.Controladores;
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
            _controlador.Configuracion.RutaFicheroEntrada = textBoxRutaXMI.Text;

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
            textBoxNamespaceProyecto.Text = _controlador.Configuracion.Proyecto.Nombre;
            textBoxRutaXMI.Text = _controlador.Configuracion.RutaFicheroEntrada;

        }

        /// <summary>
        /// Abre el explorador de directorios y rellena el textBox de ruta raíz con el directorio seleccionado.
        /// </summary>
        /// <param name="sender">Componente que lanza el evento.</param>
        /// <param name="e">Argumentos asociados al evento.</param>
        private void buttonDirectorioRaiz_Click(object sender, EventArgs e)
        {
            string rutaDirectorio = Folder.GetFolderPath("Seleccione el directorio raíz del proyecto...");
            if (string.IsNullOrEmpty(rutaDirectorio))
                return;

            textBoxRutaRaiz.Text = rutaDirectorio;
        }

        /// <summary>
        /// Selecciona el directorio del XMI y hace el parser de este.
        /// </summary>
        /// <param name="sender">Componente que lanza el evento.</param>
        /// <param name="e">Argumentos asociados al evento.</param>
        private void buttonBuscarXMI_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxRutaRaiz.Text) ||
                string.IsNullOrEmpty(textBoxNombreProyecto.Text) ||
                string.IsNullOrEmpty(textBoxNamespaceProyecto.Text))
            {
                MessageBox.Show("Para acceder a esta opción debe rellenar el resto de campos del formulario.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            string rutaFichero = Files.GetFilePath("Buscar fichero XMI.",
                "Archivos XMI (*.xmi) | *.xmi| Archivos XML (*.xml) | *.xml| Todos los archivos (*.*) | *.*");
            if (string.IsNullOrEmpty(rutaFichero))
                return;

            _controlador.Configuracion.Proyecto.Nombre = textBoxNombreProyecto.Text;

            bool resultado = _controlador.LeerFichero(rutaFichero);
            if (!resultado)
            {
                MessageBox.Show("Ha habido un error en la lecutra.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            textBoxRutaXMI.Text = rutaFichero;
        }

        /// <summary>
        /// Reutiliza el nombre del proyecto para el espacio de nombres a usar
        /// </summary>
        /// <param name="sender">Componente que lanza el evento.</param>
        /// <param name="e">Argumentos asociados al evento.</param>
        private void textBoxNombreProyecto_TextChanged(object sender, EventArgs e)
        {
            string texto = textBoxNombreProyecto.Text;
            textBoxNamespaceProyecto.Text = texto;
        }

        /// <summary>
        /// Comprueba si todos los datos están rellenos, llama al método para generar la salida y muestra mensajes de resultado por pantalla.
        /// </summary>
        /// <param name="sender">Componente que lanza el evento.</param>
        /// <param name="e">Argumentos asociados al evento.</param>
        private void buttonGenerarSalida_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxRutaRaiz.Text) ||
                string.IsNullOrEmpty(textBoxNombreProyecto.Text) ||
                string.IsNullOrEmpty(textBoxNamespaceProyecto.Text) ||
                string.IsNullOrEmpty(textBoxRutaXMI.Text))
            {
                MessageBox.Show("Falta información necesaria para la generación de código, rellene todos los campos del formulario.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            if (_controlador.EscribirArtefactos())
                MessageBox.Show("Artefactos software generados correctamente.",
                    "Información",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            else
                MessageBox.Show("Ha ocurrido un error en la generación de código.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
        }

        #endregion
    }
}
