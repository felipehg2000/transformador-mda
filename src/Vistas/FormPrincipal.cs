using System;
using System.Drawing;
using System.Windows.Forms;
using transformador_mda.src.Controladores;
using transformador_mda.src.Dominio.Retornos;
using transformador_mda.src.Dominio.ElementosUML;

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

        private Clase _clase_mostrada;

        #endregion

        #region "Constructor"

        /// <summary>
        /// Constructor de la clase, inicializa atributos
        /// </summary>
        public FormPrincipal()
        {
            InitializeComponent();

            _controlador = new Principal();

            // Ajusta las columnas 
            AjustarAnchoTablas();  
        }

        #endregion

        #region "Menú"

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
        /// Muestra el panel para importar XMI y desactiva el panel principal.
        /// </summary>
        /// <param name="sender">Componente que lanza el evento.</param>
        /// <param name="e">Argumentos asociados al evento.</param>
        private void importarXMIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelAbrirXMI.Enabled = true;
            panelAbrirXMI.Visible = true;
            panelAbrirXMI.BringToFront();

            panelPrincipal.Enabled = false;
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
            {
                panelPrincipal.Enabled = true;
                return;
            }

            RellenarArbolProyecto();
            panelPrincipal.Enabled = true;
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
                    return (int) EnumIndiceImagenes.INTERFAZ;
                case EnumTiposClases.ENUMERACION:
                    return (int)EnumIndiceImagenes.ENUMERACION;
                default:
                    return (int) EnumIndiceImagenes.CLASE;
            }
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
                dataGridViewAsociaciones.Rows.Add(asociacion.Id, asociacion.Nombre, asociacion.Comentario);
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

            if (!(e.Item.Tag is Metodo metodo))
                return;

            richTextBoxMetodo.Text = metodo.Comentario;

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
        /// Graba la información del comentario del método en la variable del controlador
        /// </summary>
        /// <param name="sender">Componente que lanza el evento.</param>
        /// <param name="e">Argumentos asociados al evento.</param>
        private void buttonGrabarMetodo_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count == 0)
                return;

            if (!(listView.SelectedItems[0].Tag is Metodo metodo))
                return;

            _controlador.ActualizarMetodos(metodo.Id, richTextBoxMetodo.Text);
        }

        /// <summary>
        /// Graba la información del comentario de los parámetros en la variable del controlador.
        /// </summary>
        /// <param name="sender">Componente que lanza el evento.</param>
        /// <param name="e">Argumentos asociados al evento.</param>
        private void buttonGrabarParametros_Click(object sender, EventArgs e)
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

        #region "Generación de código"

        /// <summary>
        /// Genera el esqueleto del código y lo muestra.
        /// </summary>
        /// <param name="sender">Componente que lanza el evento.</param>
        /// <param name="e">Argumentos asociados al evento.</param>
        private void generarEsqueletoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(treeView.SelectedNode.Tag is Clase clase_seleccionada))
                return;

            RetornoBase retorno = _controlador.GenerarCodigo(clase_seleccionada);
            if (!retorno.Exito)
                MessageBox.Show(retorno.TextoUsuario,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

            richTextBoxCodigo.Text = retorno.TextoDesarrollador;
        }

        #endregion
    }
}
