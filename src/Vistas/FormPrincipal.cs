using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using transformador_mda.src.Controladores;
using transformador_mda.src.Dominio.ElementosUML;
using transformador_mda.src.Dominio.Retornos;

namespace transformador_mda.src.Vistas
{
    /// <summary>
    /// Enumeración con los índices de las imágenes del imageList asociado al TreeView
    /// </summary>
    enum EnumIndiceImagenes
    {
        CARPETA = 0,        /// <summary> Representa el icono de un directorio, posición 0. </summary>
        CLASE = 1,          /// <summary> Representa el icono de una clase, posición 1. </summary>
        ENUMERACION = 2,    /// <summary> Representa el icono de una enumeración, posición 2. </summary>
        INTERFAZ = 3,       /// <summary> Representa el icono de una interfaz, posición 3. </summary>
        NO_DEFINIDA = 4     /// <summary> No representa ningún icono del image list. </summary>
    };

    /// <summary>
    /// Enumeración con los índices de las columnas en el DataGridView de atributos.
    /// </summary>
    enum EnumColumnasAtributos
    {
        ID = 0,             /// <summary> Índice de la columna donde se almacenarán los identificadores. </summary>
        NOMBRE = 1,         /// <summary> Índice de la columna donde se almacenarán los nombres. </summary>
        TIPO_DATO = 2,      /// <summary> Índice de la columna donde se almacenarán los tipos de datos. </summary>
        DESCRIPCION = 3,    /// <summary> Índice de la columna donde se almacenarán los comentarios. </summary>
        NO_DEFINIDA = 4     /// <summary> Índice tope o columna no definida. </summary>
    }

    /// <summary>
    /// Enumeración con los índices de las columnas en el DataGridView de asociaciones.
    /// </summary>
    enum EnumColumnasAsociaciones
    {
        ID = 0,                 /// <summary> Índice de la columna donde se almacenarán los identificadores. </summary>
        TIPO_ASOCIACION = 1,    /// <summary> Índice de la columna donde se almacenarán los posibles tipos de asociación. </summary>
        DESCRIPCION = 2,        /// <summary> Índice de la columna donde se almacenarán los comentarios. </summary>  
        CLASE_ASOCIACION = 3,   /// <summary> Índice de la columna donde se almacenarán los identificadores de la clase asociada. </summary> 
        NO_DEFINIDA = 4         /// <summary> Índice tope o columna no definida. </summary>
    }

    /// <summary>
    /// Enumeración con los índices de las columnas en el DataGridView de parámetros.
    /// </summary>
    enum EnumColumnasParametros
    {
        ID = 0,             /// <summary> Índice de la columna donde se almacenarán los identificadores. </summary>
        NOMBRE = 1,         /// <summary> Índice de la columna donde se almacenarán los nombres. </summary>
        DESCRIPCION = 2,    /// <summary> Índice de la columna donde se almacenarán los comentarios. </summary>
        NO_DEFINIDA = 3     /// <summary> Índice tope o columna no definida. </summary>
    }

    /// <summary>
    /// Enumeración con los índices de las columnas en el DataGridView de literales.
    /// </summary>
    enum EnumColumnasLiterales
    {
        ID = 0,             /// <summary> Índice de la columna donde se almacenarán los identificadores. </summary>
        NOMBRE = 1,         /// <summary> Índice de la columna donde se almacenarán los nombres. </summary>
        DESCRIPCION = 2,    /// <summary> Índice de la columna donde se almacenarán los comentarios. </summary>
        NO_DEFINIDA = 3     /// <summary> Índice tope o columna no definida. </summary>
    }

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

        /// <summary>
        /// True si el panel ya está redimensionado, false si no.
        /// </summary>
        private bool _redimensionando_panelIzquierdo;

        /// <summary>
        /// Punto de partida de la redimensión.
        /// </summary>
        private int _redimensionar_inicio_x;

        /// <summary>
        /// Ancho del panelIzquierdo antes de la redimensión
        /// </summary>
        private int _panelIzquierdo_inicio_anchura;

        /// <summary>
        /// Borde izquierdo del panelIzquierdo visible y desde el que se permite la redimensión.
        /// </summary>
        private const int AnchoBordeRedimension = 5;

        /// <summary>
        /// Ancho mínimo permitido para el panelIzquierdo.
        /// </summary>
        private const int AnchoPanelIzquierdoMinimo = 150;

        #endregion

        #region "Move Form Info"
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        #endregion "Move Form Info"

        #region "Constructor"

        /// <summary>
        /// Constructor de la clase, inicializa atributos
        /// </summary>
        public FormPrincipal()
        {
            InitializeComponent();

            _controlador = new Principal();

            _controlador.CargarClave();
            richTextBoxClaveOpenAI.Text = _controlador.ClaveOpenAI;

            // Mostrar panel adecuado
            tableLayoutPanelInicio.Visible = true;
            tableLayoutPanelInicio.Enabled = true;
            tableLayoutPanelInicio.BringToFront();

            ActivarPantallaTrabajo(true);

            // Ajusta las columnas 
            AjustarAnchoTablas();
        }

        #endregion

        #region "Menú Configuración"

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

            panelAbrirXMI.Enabled = false;
            panelAbrirXMI.Visible = false;
            panelAbrirXMI.SendToBack();

            if (_controlador?.Configuracion?.Proyecto == null)
                return;

            RellenarArbolProyecto();

            panelPrincipal.Enabled = true;
            panelPrincipal.Visible = true;
            panelPrincipal.BringToFront();

            panelSelClase.Enabled = true;
            panelSelClase.Visible = true;
            panelSelClase.BringToFront();

        }

        /// <summary>
        /// Muestra el panel para importar XMI y desactiva el panel principal.
        /// </summary>
        /// <param name="sender">Componente que lanza el evento.</param>
        /// <param name="e">Argumentos asociados al evento.</param>
        private void importarXMIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CentrarPanel(panelAbrirXMI);
            panelAbrirXMI.Enabled = true;
            panelAbrirXMI.Visible = true;
            panelAbrirXMI.BringToFront();

            panelPrincipal.Enabled = false;
        }

        /// <summary>
        /// Copia el código generado en el portapapeles.
        /// </summary>
        /// <param name="sender">Componente que lanza el evento.</param>
        /// <param name="e">Argumentos asociados al evento.</param>
        private async void buttonCopiar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(richTextBoxCodigo.Text))
                return;

            Clipboard.SetText(richTextBoxCodigo.Text);

            // Efecto visual
            Color colorOriginal = richTextBoxCodigo.BackColor;
            richTextBoxCodigo.BackColor = SystemColors.Control;
            await Task.Delay(150);
            richTextBoxCodigo.BackColor = colorOriginal;
        }

        #endregion

        #region "Redimensión ancho panelIzquierdo"

        /// <summary>
        /// Controla las variables de redimensión por si el click se queda pulsado.
        /// </summary>
        /// <param name="sender">Componente que lanza el evento.</param>
        /// <param name="e">Argumentos asociados al evento.</param>
        private void PanelIzquierdo_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            var control = sender as Control;
            if (control == null)
                return;

            Point puntoEnPanel = panelIzquierdo.PointToClient(control.PointToScreen(e.Location));
            if (!EstaEnBordeDerechoPanelIzquierdo(puntoEnPanel))
                return;

            _redimensionando_panelIzquierdo = true;
            _redimensionar_inicio_x = Control.MousePosition.X;
            _panelIzquierdo_inicio_anchura = panelIzquierdo.Width;
            panelIzquierdo.Capture = true;
        }

        /// <summary>
        /// Modifica el cursos al salir del panelIzquierdo 
        /// </summary>
        /// <param name="sender">Componente que lanza el evento.</param>
        /// <param name="e">Argumentos asociados al evento.</param>
        private void PanelIzquierdo_MouseLeave(object sender, EventArgs e)
        {
            if (_redimensionando_panelIzquierdo)
                return;

            RestaurarCursorPanelIzquierdo();
        }

        /// <summary>
        /// Comprueba la posición para controlar el aspecto del cursor, si el click está pulsado modifica el ancho del panel.
        /// </summary>
        /// <param name="sender">Componente que lanza el evento.</param>
        /// <param name="e">Argumentos asociados al evento.</param>
        private void PanelIzquierdo_MouseMove(object sender, MouseEventArgs e)
        {
            if (_redimensionando_panelIzquierdo)
            {
                int delta = Control.MousePosition.X - _redimensionar_inicio_x;
                int nuevoAncho = _panelIzquierdo_inicio_anchura + delta;

                if (nuevoAncho < AnchoPanelIzquierdoMinimo)
                    nuevoAncho = AnchoPanelIzquierdoMinimo;

                panelIzquierdo.Width = nuevoAncho;
                return;
            }

            ActualizarCursorRedimension(sender as Control, e);
        }

        /// <summary>
        /// Modifica el cursor al salir del PanelIzquierdo
        /// </summary>
        /// <param name="sender">Componente que lanza el evento.</param>
        /// <param name="e">Argumentos asociados al evento.</param>
        private void PanelIzquierdo_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_redimensionando_panelIzquierdo)
                return;

            _redimensionando_panelIzquierdo = false;
            panelIzquierdo.Capture = false;
            RestaurarCursorPanelIzquierdo();
        }

        /// <summary>
        /// Método encargado de definir el punto donde se encuentra el cursor para mostrar un aspecto u otro.
        /// </summary>
        /// <param name="sender">Componente que lanza el evento.</param>
        /// <param name="e">Argumentos asociados al evento.</param>
        private void ActualizarCursorRedimension(Control control, MouseEventArgs e)
        {
            if (control == null)
                return;

            Point puntoEnPanel = panelIzquierdo.PointToClient(control.PointToScreen(e.Location));
            bool sobreBorde = EstaEnBordeDerechoPanelIzquierdo(puntoEnPanel);
            Cursor cursorDeseado = sobreBorde ? Cursors.SizeWE : Cursors.Default;

            if (panelIzquierdo.Cursor != cursorDeseado)
                panelIzquierdo.Cursor = cursorDeseado;

            if (control.Cursor != cursorDeseado)
                control.Cursor = cursorDeseado;
        }

        /// <summary>
        /// Comprueba si está en el borde izquierdo del panel o no para saber si modificar o no el aspecto del cursor.
        /// </summary>
        /// <param name="param_punto_en_panel">Punto en el que se encuentra el cursor.</param>
        /// <returns></returns>
        private bool EstaEnBordeDerechoPanelIzquierdo(Point param_punto_en_panel)
        {
            return param_punto_en_panel.X >= panelIzquierdo.Width - AnchoBordeRedimension &&
                   param_punto_en_panel.X <= panelIzquierdo.Width;
        }

        /// <summary>
        /// Método encargado de devolver el estilo por defecto al cursor.
        /// </summary>
        private void RestaurarCursorPanelIzquierdo()
        {
            panelIzquierdo.Cursor = Cursors.Default;
            if (treeView != null)
                treeView.Cursor = Cursors.Default;
            if (textBoxBuscarComponente != null)
                textBoxBuscarComponente.Cursor = Cursors.Default;
        }

        #endregion

        #region "Redimension DataGridView"

        /// <summary>
        /// Cada vez que se redimensionan los dataGridView se ajusta el ancho de sus columnas.
        /// </summary>
        /// <param name="sender">Componente que lanza el evento.</param>
        /// <param name="e">Argumentos asociados al evento.</param>
        private void dataGridView_Resize(object sender, EventArgs e)
        {
            AjustarAnchoTablas();
        }

        /// <summary>
        /// Ajusta el ancho de las columnas de los DataGridView de los formularios.
        /// </summary>
        private void AjustarAnchoTablas()
        {
            int ancho = dataGridViewAtributos.Width;
            dataGridViewAtributos.Columns[(int)EnumColumnasAtributos.NOMBRE].Width = (int)(ancho * 0.25);
            dataGridViewAtributos.Columns[(int)EnumColumnasAtributos.TIPO_DATO].Width = (int)(ancho * 0.25);
            dataGridViewAtributos.Columns[(int)EnumColumnasAtributos.DESCRIPCION].Width = (int)(ancho * 0.5);

            ancho = dataGridViewAsociaciones.Width;
            dataGridViewAsociaciones.Columns[(int)EnumColumnasAsociaciones.TIPO_ASOCIACION].Width = (int)(ancho * 0.25);
            dataGridViewAsociaciones.Columns[(int)EnumColumnasAsociaciones.DESCRIPCION].Width = (int)(ancho * 0.75);

            ancho = dataGridViewParametros.Width;
            dataGridViewParametros.Columns[(int)EnumColumnasParametros.NOMBRE].Width = (int)(ancho * 0.25);
            dataGridViewParametros.Columns[(int)EnumColumnasParametros.DESCRIPCION].Width = (int)(ancho * 0.75);

            ancho = dataGridViewLiterales.Width;
            dataGridViewLiterales.Columns[(int)EnumColumnasLiterales.NOMBRE].Width = (int)(ancho * 0.25);
            dataGridViewLiterales.Columns[(int)EnumColumnasLiterales.DESCRIPCION].Width = (int)(ancho * 0.75);
        }

        #endregion

        #region "Panel ImportarXMI"

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

            ActivarBotonContinuar();
        }

        /// <summary>
        /// Carga el nombre del proyecto en el objeto de configuración.
        /// </summary>
        /// <param name="sender">Componente que lanza el evento.</param>
        /// <param name="e">Argumentos asociados al evento.</param>
        private void textBoxNombreProyecto_TextChanged(object sender, EventArgs e)
        {
            _controlador.Configuracion.Proyecto.Nombre = textBoxNombreProyecto.Text;
            ActivarBotonContinuar();
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
            ActivarBotonContinuar();
        }

        /// <summary>
        /// Activa o desactiva el botón para completar la lectura del fichero XMI.
        /// </summary>
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

        /// <summary>
        /// Carga el árbol de ficheros y directorios y muestra el panel principal
        /// </summary>
        /// <param name="sender">Componente que lanza el evento.</param>
        /// <param name="e">Argumentos asociados al evento.</param>
        private void buttonContinuar_Click(object sender, EventArgs e)
        {
            panelAbrirXMI.Enabled = false;
            panelAbrirXMI.Visible = false;
            panelAbrirXMI.SendToBack();

            if (_controlador?.Configuracion?.Proyecto == null)
                return;

            RellenarArbolProyecto();

            panelPrincipal.Enabled = true;
            panelPrincipal.Visible = true;
            panelPrincipal.BringToFront();

            panelSelClase.Enabled = true;
            panelSelClase.Visible = true;
            panelSelClase.BringToFront();
        }

        /// <summary>
        /// Comprueba la existencia de los elementos, genera el directorio del proyecto y recorre los directorios principales.
        /// </summary>
        private void RellenarArbolProyecto()
        {
            if (treeView == null || treeView.ImageList == null)
                return;

            treeView.Nodes.Clear();

            TreeNode nodo_raiz = new TreeNode(_controlador.Configuracion.Proyecto.Nombre)
            {
                Tag = _controlador.Configuracion.Proyecto,
                ImageIndex = (int)EnumIndiceImagenes.CARPETA,
                SelectedImageIndex = (int)EnumIndiceImagenes.CARPETA
            };

            if (_controlador.Configuracion.Proyecto.Directorios != null)
            {
                foreach (Directorio directorio in _controlador.Configuracion.Proyecto.Directorios)
                    nodo_raiz.Nodes.Add(CrearNodoDirectorio(directorio));
            }

            treeView.Nodes.Add(nodo_raiz);
            nodo_raiz.Expand();
        }

        /// <summary>
        /// Genera los subdirectdorios y las clases asociadas a un directorio principal.
        /// </summary>
        /// <param name="param_directorio">Directorio principal del que se generarán los subdirectorios y clases.</param>
        /// <returns>Nodo del directorio generado.</returns>
        private TreeNode CrearNodoDirectorio(Directorio param_directorio)
        {
            TreeNode ret_nodo_directorio = new TreeNode(param_directorio.Nombre)
            {
                Tag = param_directorio,
                ImageIndex = (int)EnumIndiceImagenes.CARPETA,
                SelectedImageIndex = (int)EnumIndiceImagenes.CARPETA
            };

            if (param_directorio.Subdirectorios != null)
            {
                foreach (Directorio subdirectorio in param_directorio.Subdirectorios)
                    ret_nodo_directorio.Nodes.Add(CrearNodoDirectorio(subdirectorio));
            }

            if (param_directorio.Clases != null)
            {
                foreach (Clase clase in param_directorio.Clases)
                {
                    int indice_icono = ObtenerClaveIcono(clase);
                    var nodo_clase = new TreeNode(clase.Nombre)
                    {
                        Tag = clase,
                        ImageIndex = indice_icono,
                        SelectedImageIndex = indice_icono
                    };

                    ret_nodo_directorio.Nodes.Add(nodo_clase);
                }
            }

            return ret_nodo_directorio;
        }

        /// <summary>
        /// Según el tipo de clase decide el icono a mostrar en el tree view.
        /// </summary>
        /// <param name="param_clase">Clase para asociar el tipo de icono.</param>
        /// <returns>Índice del tipo de icono en el image list.</returns>
        private int ObtenerClaveIcono(Clase param_clase)
        {
            if (param_clase == null)
                return (int)EnumIndiceImagenes.NO_DEFINIDA;

            switch (param_clase.TipoClase)
            {
                case EnumTiposClases.INTERFAZ:
                    return (int)EnumIndiceImagenes.INTERFAZ;
                case EnumTiposClases.ENUMERACION:
                    return (int)EnumIndiceImagenes.ENUMERACION;
                default:
                    return (int)EnumIndiceImagenes.CLASE;
            }
        }

        /// <summary>
        /// Arrastra el panel de importación de XMI.
        /// </summary>
        /// <param name="sender">Componente que lanza el evento.</param>
        /// <param name="e">Argumentos asociados al evento.</param>
        private void labelTituloXMI_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(panelAbrirXMI.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        /// <summary>
        /// Cierra el panel de importación de XMI.
        /// </summary>
        /// <param name="sender">Componente que lanza el evento.</param>
        /// <param name="e">Argumentos asociados al evento.</param>
        private void buttonCerrarXMI_Click(object sender, EventArgs e)
        {
            panelAbrirXMI.Visible = false;
            panelAbrirXMI.Enabled = false;
            panelAbrirXMI.SendToBack();
        }

        #endregion

        #region "Rellenar formularios y actualizar comentarios"

        /// <summary>
        /// Selecciona la clase y muestra sus datos.
        /// </summary>
        /// <param name="sender">Componente que lanza el evento.</param>
        /// <param name="e">Argumentos asociados al evento.</param>
        private void treeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (!(e.Node.Tag is Clase clase))
                return;

            splitContainer.Visible = true;
            splitContainer.Enabled = true;
            splitContainer.BringToFront();

            labelTituloEdicion.Text = "Clase: " + clase.Nombre;

            if (clase.TipoClase == EnumTiposClases.ENUMERACION)
            {
                tabControlEnumeraciones.Visible = true;
                tabControlEnumeraciones.Enabled = true;
                tabControlEnumeraciones.BringToFront();

                RellenarFormularioEnum(clase);
            }
            else
            {
                tabControlClases.Visible = true;
                tabControlClases.Enabled = true;
                tabControlClases.BringToFront();

                RellenarFormularioClase(clase);
            }
        }

        /// <summary>
        /// Rellena el formulario con los datos de una clase o interfaz.
        /// </summary>
        /// <param name="param_clase">Clase de tipo interfaz, clase abstracto o clase normal.</param>
        private void RellenarFormularioClase(Clase param_clase)
        {
            richTextBoxClase.Text = string.Empty;
            listView.Items.Clear();
            dataGridViewAsociaciones.Rows.Clear();
            dataGridViewAtributos.Rows.Clear();

            richTextBoxMetodo.Text = string.Empty;
            richTextBoxRetorno.Text = string.Empty;
            dataGridViewParametros.Rows.Clear();

            richTextBoxClase.Text = param_clase.Comentario;
            richTextBoxClase.Tag = param_clase.Id;
            foreach (Metodo metodo in param_clase.Metodos)
            {
                ListViewItem item = new ListViewItem(metodo.Nombre)
                {
                    Tag = metodo
                };

                listView.Items.Add(item);
            }

            foreach (Atributo atributo in param_clase.Atributos)
            {
                dataGridViewAtributos.Rows.Add(atributo.Id, atributo.Nombre, "void", atributo.Comentario);
            }

            foreach (Asociacion asociacion in param_clase.Asociaciones)
            {
                dataGridViewAsociaciones.Rows.Add(asociacion.Id, ObtenerTipoAsociacion(asociacion.TipoAsociacion), asociacion.Comentario);
            }
        }

        /// <summary>
        /// Rellena el formulario de las enumeraciones con sus literales.
        /// </summary>
        /// <param name="param_clase">Clase de tipo enumeración.</param>
        private void RellenarFormularioEnum(Clase param_clase)
        {
            richTextBoxComentarioEnum.Text = string.Empty;
            dataGridViewLiterales.Rows.Clear();

            richTextBoxComentarioEnum.Text = param_clase.Comentario;
            richTextBoxComentarioEnum.Tag = param_clase.Id;
            foreach (ElementoBase literal in param_clase.Literales)
            {
                dataGridViewLiterales.Rows.Add(literal.Id, literal.Nombre, literal.Comentario);
            }
        }

        /// <summary>
        /// Rellena la información de un método según la selección del item en un list view.
        /// </summary>
        /// <param name="sender">Componente que lanza el evento.</param>
        /// <param name="e">Argumentos asociados al evento.</param>
        private void listView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            richTextBoxMetodo.Text = string.Empty;
            richTextBoxRetorno.Text = string.Empty;
            dataGridViewParametros.Rows.Clear();

            labelTitulo.Text = "Comentarios del método: " + e.Item.Text;

            if (!(e.Item.Tag is Metodo metodo))
                return;

            richTextBoxMetodo.Text = metodo.Comentario;
            richTextBoxRetorno.Text = metodo.Retorno.Comentario;

            foreach (Parametro parametro in metodo.Parametros)
            {
                dataGridViewParametros.Rows.Add(parametro.Id, parametro.Nombre, parametro.Comentario);
            }
        }

        /// <summary>
        /// Graba la información de los comentarios genéricos en la variable del controlador principal.
        /// </summary>
        /// <param name="sender">Componente que lanza el evento.</param>
        /// <param name="e">Argumentos asociados al evento.</param>
        private void buttonGrabarInfo_Click(object sender, EventArgs e)
        {
            string id_clase = richTextBoxClase.Tag?.ToString() ?? string.Empty;
            _controlador.ActualizarClase(id_clase, richTextBoxClase.Text);

            foreach (DataGridViewRow fila in dataGridViewAtributos.Rows)
            {
                string id = fila.Cells[(int)EnumColumnasAtributos.ID].Value?.ToString() ?? string.Empty;
                string comentario = fila.Cells[(int)EnumColumnasAtributos.DESCRIPCION].Value?.ToString() ?? string.Empty;

                _controlador.ActualizarAtributo(id, comentario);
            }

            foreach (DataGridViewRow fila in dataGridViewAsociaciones.Rows)
            {
                string id = fila.Cells[(int)EnumColumnasAsociaciones.ID].Value?.ToString() ?? string.Empty;
                string comentario = fila.Cells[(int)EnumColumnasAsociaciones.DESCRIPCION].Value?.ToString() ?? string.Empty;

                _controlador.ActualizarAsociaciones(id, comentario);
            }
        }

        /// <summary>
        /// Graba la información del comentario del método, parámetros y retorno en la variable del controlador
        /// </summary>
        /// <param name="sender">Componente que lanza el evento.</param>
        /// <param name="e">Argumentos asociados al evento.</param>
        private void buttonGrabarMetodo_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count == 0)
                return;

            if (!(listView.SelectedItems[0].Tag is Metodo metodo))
                return;

            foreach (DataGridViewRow fila in dataGridViewParametros.Rows)
            {
                string id = fila.Cells[(int)EnumColumnasParametros.ID].Value?.ToString() ?? string.Empty;
                string comentario = fila.Cells[(int)EnumColumnasParametros.DESCRIPCION].Value?.ToString() ?? string.Empty;

                _controlador.ActualizarParametro(id, comentario);
            }

            _controlador.ActualizarMetodos(metodo.Id, richTextBoxMetodo.Text, richTextBoxRetorno.Text);
        }

        /// <summary>
        /// Graba la información del comentario de las enumeraciones en la variable del controlador.
        /// </summary>
        /// <param name="sender">Componente que lanza el evento.</param>
        /// <param name="e">Argumentos asociados al evento.</param>
        private void buttonGrabarComentariosEnum_Click(object sender, EventArgs e)
        {
            if (!(treeView.SelectedNode.Tag is Clase clase_seleccionada))
                return;

            if (clase_seleccionada.TipoClase != EnumTiposClases.ENUMERACION)
                return;

            string id_clase = richTextBoxComentarioEnum.Tag?.ToString() ?? string.Empty;
            _controlador.ActualizarClase(id_clase, richTextBoxComentarioEnum.Text);

            foreach (DataGridViewRow fila in dataGridViewLiterales.Rows)
            {
                string id = fila.Cells[(int)EnumColumnasLiterales.ID].Value?.ToString() ?? string.Empty;
                string comentario = fila.Cells[(int)EnumColumnasLiterales.DESCRIPCION].Value?.ToString() ?? string.Empty;

                _controlador.ActualizarLiteral(id, comentario);
            }
        }

        #endregion

        #region "Generación de código de una clase"

        /// <summary>
        /// Genera el código esqueleto de la clase que el usuario está visualizando.
        /// </summary>
        /// <param name="sender">Componente que lanza el evento.</param>
        /// <param name="e">Argumentos asociados al evento.</param>
        private void generarEsqueletoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ActivarPantallaTrabajo(false);
            Clase clase_seleccionada = ObtenerClaseSeleccionada();
            if (clase_seleccionada == null)
                return;

            RetornoBase retorno = _controlador.GenerarEsqueletoClase(clase_seleccionada, toolStripProgressBar);
            if (!retorno.Exito)
                MessageBox.Show(retorno.TextoUsuario,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

            PrepararVistaDespuesDeGenerarCodigo(retorno.TextoDesarrollador, retorno.Exito);
        }

        /// <summary>
        /// Genera el código completo de la clase que el usuario está visualizando.
        /// </summary>
        /// <param name="sender">Componente que lanza el evento.</param>
        /// <param name="e">Argumentos asociados al evento.</param>
        private async void generarCódigoCompletoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_controlador.ClaveOpenAI))
            {
                MessageBox.Show("Es necesario configurar una clave de OpenAI para generar el código completo",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            ActivarPantallaTrabajo(false);
            Clase clase_seleccionada = ObtenerClaseSeleccionada();
            if (clase_seleccionada == null)
            {
                ActivarPantallaTrabajo(true);
                return;
            }

            RetornoMultiple retorno = await _controlador.GenerarCodigoIAClase(clase_seleccionada, true, toolStripProgressBar);
            do
            {
                if (retorno.Exito)
                    break;

                if (retorno.Estado == RetornoMultiple.EnumEstado.CONFIRMACION_USUARIO)
                {
                    DialogResult respuesta = MessageBox.Show(retorno.TextoDesarrollador,
                        "Aviso",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information);

                    if (respuesta == DialogResult.No)
                    {
                        ActivarPantallaTrabajo(true);
                        return;
                    }

                    retorno = await _controlador.GenerarCodigoIAClase(clase_seleccionada, true, toolStripProgressBar, true);
                }
                else
                {
                    MessageBox.Show(retorno.TextoUsuario,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    ActivarPantallaTrabajo(true);
                    return;
                }
            }
            while (true);

            PrepararVistaDespuesDeGenerarCodigo(retorno.TextoDesarrollador, retorno.Exito);
        }

        /// <summary>
        /// Obtiene la clase que el usuario está visualizando.
        /// </summary>
        /// <returns>Null si no tienen ninguna clase abierta o el objeto clase que esté visualizando.</returns>
        private Clase ObtenerClaseSeleccionada()
        {
            if (treeView.Nodes.Count <= 0)
            {
                MessageBox.Show("Debe seleccionar un fichero XMI o un proyecto y una clase en el explorador de soluicones para poder generar código.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                ActivarPantallaTrabajo(true);
                return null;
            }
            if (!(treeView.SelectedNode.Tag is Clase ret_clase))
            {
                MessageBox.Show("Debe seleccionar una clase en el explorador de soluicones para poder generar código.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                ActivarPantallaTrabajo(true);
                return null;
            }

            return ret_clase;
        }

        /// <summary>
        /// Después de la generación de código activa la pantalla de trabajo y rellena el rich text.
        /// </summary>
        /// <param name="param_texto">Texto a mostrar en el rich text.</param>
        /// <param name="param_mostrar_texto">True para mostrar el texto, false para no mostrarlo.</param>
        private void PrepararVistaDespuesDeGenerarCodigo(string param_texto, bool param_mostrar_texto)
        {
            if (param_mostrar_texto)
                richTextBoxCodigo.Text = param_texto;

            ActivarPantallaTrabajo(true);
            toolStripProgressBar.Value = 0;
            toolStripProgressBar.Maximum = 0;
        }

        #endregion

        #region "Traducir enumeraciones"

        /// <summary>
        /// Traduce los valores de una enumeración a lenguaje natural.
        /// </summary>
        /// <param name="param_asociacion">Índice de la enumeración a traducir.</param>
        /// <returns>Texto de la enumeración en lenguaje natural.</returns>
        static private string ObtenerTipoAsociacion(EnumTipoAsociacion param_asociacion)
        {
            switch (param_asociacion)
            {
                case EnumTipoAsociacion.ASOCIACION:
                    return "Asociación";
                case EnumTipoAsociacion.AGREGACION:
                    return "Agregación";
                case EnumTipoAsociacion.COMPOSICION:
                    return "Composicion";
                case EnumTipoAsociacion.HERENCIA:
                    return "Herencia";
                case EnumTipoAsociacion.DEPENDENCIA:
                    return "Dependencia";
                default:
                    return "No definida";
            }
        }

        #endregion

        #region "Métodos auxiliares"

        /// <summary>
        /// Activa y desactiva los paneles de trabajo para que el usuario no pueda interactuar con ella si hay un proceso en marcha.
        /// </summary>
        /// <param name="param_activar">True para activar el espacio de trabajo, false para desactivarlo.</param>
        private void ActivarPantallaTrabajo(bool param_activar)
        {
            menuStrip.Enabled = param_activar;
            panelIzquierdo.Enabled = param_activar;
            treeView.Enabled = param_activar;
            splitContainer.Enabled = param_activar;
            panelSelClase.Enabled = param_activar;

            if (param_activar)
            {
                this.Cursor = Cursors.Default;
            }
            else
            {
                this.Cursor = Cursors.WaitCursor;
            }
        }

        /// <summary>
        /// Método auxiliar para centrar los paneles a visualizar.
        /// </summary>
        /// <param name="param_panel">Panel a centrar en mitad de su contenedor.</param>
        private void CentrarPanel(Panel param_panel)
        {
            if (param_panel.Parent == null)
                return;

            Control contenedor = param_panel.Parent;
            param_panel.Location = new Point((contenedor.ClientSize.Width - param_panel.Width) / 2,
                (contenedor.ClientSize.Height - param_panel.Height) / 2);
        }

        #endregion

        #region "Panel de ClaveOpenAI"

        /// <summary>
        /// Cierra el panel.
        /// </summary>
        /// <param name="sender">Componente que lanza el evento.</param>
        /// <param name="e">Argumentos asociados al evento.</param>
        private void buttonClaveOpenAICerrar_Click(object sender, EventArgs e)
        {
            panelClaveOpenAI.Enabled = false;
            panelClaveOpenAI.Visible = false;
            panelClaveOpenAI.SendToBack();
            ActivarPantallaTrabajo(true);
        }

        /// <summary>
        /// Centra y muestra el panel
        /// </summary>
        /// <param name="sender">Componente que lanza el evento.</param>
        /// <param name="e">Argumentos asociados al evento.</param>
        private void añadirClaveOpenAIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActivarPantallaTrabajo(false);
            CentrarPanel(panelClaveOpenAI);
            panelClaveOpenAI.Enabled = true;
            panelClaveOpenAI.Visible = true;
            panelClaveOpenAI.BringToFront();
        }

        /// <summary>
        /// Cambia el icono del cursor
        /// </summary>
        /// <param name="sender">Componente que lanza el evento.</param>
        /// <param name="e">Argumentos asociados al evento.</param>
        private void panelClaveOpenAICentral_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Permite que el panel se mueva por la pantalla
        /// </summary>
        /// <param name="sender">Componente que lanza el evento.</param>
        /// <param name="e">Argumentos asociados al evento.</param>
        private void labelClaveOpenAI_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(panelClaveOpenAI.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        /// <summary>
        /// Graba la clave open ai en la instalación
        /// </summary>
        /// <param name="sender">Componente que lanza el evento.</param>
        /// <param name="e">Argumentos asociados al evento.</param>
        private void buttonClaveOpenAIGrabar_Click(object sender, EventArgs e)
        {
            _controlador.ClaveOpenAI = richTextBoxClaveOpenAI.Text;
            _controlador.GuardarClave();
            buttonClaveOpenAICerrar_Click(sender, e);
        }

        #endregion
    }
}
