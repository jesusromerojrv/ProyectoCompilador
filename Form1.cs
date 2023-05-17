using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using ListViewItem = System.Windows.Forms.ListViewItem;
using ProyectoArbolBinario;
using MessageBox = System.Windows.Forms.MessageBox;
using System.Threading;
using Application = System.Windows.Forms.Application;
using System.Collections;
using Label = System.Windows.Controls.Label;
using HorizontalAlignment = System.Windows.Forms.HorizontalAlignment;
//using System.Diagnostics;

namespace ProyectoCompilador
{
    /// <summary>
    /// Clase principal que muestra un compilador.
    /// Contiene eventos y métodos que nos permiten realizar el analicis léxico(tabla de simbolos), sintáctico(generación del árbol) y errores de diversos tipos.
    /// </summary>
    /// <remarks>
    /// <para>Con esta clase se puede abrir y guardar un archivo, muestra la ayuda del programa, un menú para borrar datos, un menú que 
    /// nos permite abrir un archivo con un ejemplo de código en lenguaje Java y lenguaje llamado BlackCat, por medio de botones realiza
    /// el analizador léxico, sintáctico y errores léxicos, sintácticos y semánticos, contiene un campo de texto para mostrar las palabras reservadas y un ckeck List para seleccionar
    /// el lenguaje que queremos usar.</para>
    /// </remarks>
    /// <supuestos>Para que esta clase funcione es necesario seleccionar un lenguaje "Java o BlackCat" e ingresar código para analizarlo.
    /// </supuestos>
    /// <Autor>
    /// 18131243 Gómez Montes Zaida Sugey 
    /// 18131279 Romero Vázquez Jesús
    /// 18131225 Castañeda Limones Carlos Elian
    /// </Autor>
    /// <FechaCreacion>Jueves 16 de septiembre del 2021
    /// </FechaCreacion>
    public partial class Form1 : Form
    {
        //************************************************************************************************************
        // Variables Locales
        //************************************************************************************************************
        #region VARIABLES LOCALES 
        private Nodo raiz;
        private Arbol arbol;
        Grafico grafico;
        string cadena = "";
        string nom = "";
        string a = "";
        bool tabCuadruplo = false;
        bool masexpresiones = false;
        int inicioCiclo = 5;
        int inicioCicloElse = 6;
        int conteo = 5;
        ListViewItem lex = new ListViewItem();
        private Stack pilaOp;
        private Stack pilaOpInv;
        private Stack pilaOpInv2;
        private Stack pilaLexico;
        bool titulos = false;
        bool errorLlave = false;
        string[] operador = { "+", "-", "*", "/" };
        #endregion

        public Form1()
        {
            //creamos un hilo
            Thread t = new Thread(new ThreadStart(SplashStart));

            //arrancamos el hilo
            t.Start();

            //ponemos a dormir la forma principal
            Thread.Sleep(7900);
            InitializeComponent();
            t.Abort();
            arbol = new Arbol();
            pilaOp = new Stack();
            pilaOpInv = new Stack();
            pilaOpInv2 = new Stack();
            pilaLexico = new Stack();

            btnAnalisisSemantico.MouseHover += new System.EventHandler(this.Pasa_EncimaS);

            btnAnalisisSemantico.MouseLeave += this.Quitar_EncimaS;

            Bitmap imagen = new Bitmap(Application.StartupPath + @"\img\Errores.png");
            btnAnalisisSemantico.Image = imagen;
            //ON - OFF
            pictureBoxSalir.MouseHover += new System.EventHandler(this.Pasa_EncimaON);

            pictureBoxSalir.MouseLeave += this.Quitar_EncimaON;

            Bitmap imagen2 = new Bitmap(Application.StartupPath + @"\img\ic_power_ON.png");
            pictureBoxSalir.Image = imagen2;
        }

        //************************************************************************************************************
        // Variables locales
        //************************************************************************************************************
        #region VARIABLES LOCALES 
        RegexLexer csLexer = new RegexLexer();
        bool load;
        List<string> palabrasReservadas;
        private string Title;
        List<String> contenido = new List<string>();
        List<String> tab = new List<string>();
        ListViewItem listaLinea = new ListViewItem();
        #endregion

        //************************************************************************************************************
        // Eventos
        //************************************************************************************************************
        #region EVENTOS 

        #region EVENTOS DEL MOUSE
        /// <summary>
        /// EVENTO CLICK PARA EFECTOS DEL MOUSE QUE PASA ENCIMA DEL BOTÓN ERRORES
        /// </summary>       
        private void Pasa_EncimaS(object obj, EventArgs evt)
        {
            Bitmap imagen = new Bitmap(Application.StartupPath + @"\img\Errores.gif");
            btnAnalisisSemantico.Image = imagen;
        }

        /// <summary>
        /// EVENTO CLICK PARA EFECTOS DEL MOUSE QUE QUITA ENCIMA DEL BOTÓN ERRORES
        /// </summary> 
        private void Quitar_EncimaS(object obj, EventArgs evt)
        {
            Bitmap imagen = new Bitmap(Application.StartupPath + @"\img\Errores.png");
            btnAnalisisSemantico.Image = imagen;
        }

        /// <summary>
        /// EVENTO CLICK PARA EFECTOS DEL MOUSE QUE PASA ENCIMA DEL BOTÓN DE ENCENDER
        /// </summary> 
        private void Pasa_EncimaON(object obj, EventArgs evt)
        {
            Bitmap imagen = new Bitmap(Application.StartupPath + @"\img\ic_power.png");
            pictureBoxSalir.Image = imagen;
        }

        /// <summary>
        /// EVENTO CLICK PARA EFECTOS DEL MOUSE QUE QUITA ENCIMA DEL BOTÓN DE ENCENDER
        /// </summary> 
        private void Quitar_EncimaON(object obj, EventArgs evt)
        {
            Bitmap imagen = new Bitmap(Application.StartupPath + @"\img\ic_power_ON.png");
            pictureBoxSalir.Image = imagen;
        }

        #endregion

        #region EVENTO PARA ABRIR UN ARCHIVO DE LAS PALABRAS RESERVADAS
        /// <summary>
        /// EVENTO CLICK DEL BOTON JAVA PARA ABRIR UN ARCHIVO CON LAS PALABRAS RESERVADAS:
        /// </summary>
        private void btnJava_Click(object sender, EventArgs e)
        {

            try
            {
                OpenFileDialog ofd = new OpenFileDialog();

                ofd.Title = "Compilador                                                                    Abrir Archivo                                                                       ";
                ofd.ShowDialog();
                // ofd.Filter = "Archivos ed#(*.ed)|*.ed";
                if (File.Exists(ofd.FileName))
                {
                    using (Stream stream = ofd.OpenFile())
                    {
                        //MessageBox.Sh
                        leerArchivo(ofd.FileName);
                        //nomarchivox = ofd.FileName;

                        //txt_direccion.Text = ofd.FileName;
                        //tabControl1.Visible = true;
                    }
                }
            } catch(Exception){

                //MessageBox.Show("El archivo no se abrio correctamente");

            }

        }

        #endregion

        #region EVENTO PARA QUE EL CÓDIGO INGRESADO CAMBIE
        /// <summary>
        /// EVENTO PARA QUE EL TEXTO QUE SE INGRESE EN EL PagCodigo se modifique automáticamente.
        /// </summary>
        private void PagCodigo_TextChanged(object sender, EventArgs e)
        {
            if (load)
            {
                titulos = false;
                listViewToken.Items.Clear();
                AnalizeCode(PagCodigo.Text, 1,"1");
                dividirTextoxLinea();
                //bloquesLexico();
            }
            if(PagCodigo.Text == "")
            {
                listViewToken.Items.Clear();
                titulos = false;
            }
 
        }
        #endregion

        #region EVENTO PARA EL BOTÓN LÉXICO
        /// <summary>
        /// EVENTO DEL BOTON DEL ANALIZADOR LEXICO PARA MOSTRAR LA TABLA DE SIMBOLOS POR BLOQUES.
        /// </summary>
        private void btnLexico_Click(object sender, EventArgs e)
        {
            listViewToken.Items.Clear();
            if(PagCodigo.Text != "")
            {
                using (StreamReader sr = new StreamReader(@"..\..\RegexLexer.cs"))
                {
                    //tbxCode.Text = sr.ReadToEnd();

                    csLexer.AddTokenRule(@"\s+", "ESPACIO", true);
                    csLexer.AddTokenRule(@"\b[_a-zA-Z][\w]*\b", "IDENTIFICADOR");
                    csLexer.AddTokenRule("\".*?\"", "CADENA");
                    csLexer.AddTokenRule(@"'\\.'|'[^\\]'", "CARACTER");
                    csLexer.AddTokenRule("//[^\r\n]*", "COMENTARIO1");
                    csLexer.AddTokenRule("/[*].*?[*]/", "COMENTARIO2");
                    csLexer.AddTokenRule(@"\d*\.?\d+", "NUMERO");
                    csLexer.AddTokenRule(@"[\(\)\{\}\[\];,]", "DELIMITADOR");
                    csLexer.AddTokenRule(@"[\.=\+\-/*%]", "OPERADOR_ARITMÉTICO");
                    csLexer.AddTokenRule(@">|<|==|>=|<=|!", "OPERADOR_LÓGICO");

                    if (radioBtnJava.Checked == true)
                    {

                        palabrasReservadas = new List<string>() { "abstract", "as", "async", "await",
                "checked", "const", "continue", "default", "delegate", "base", "break", "case",
                "do", "else", "enum", "event", "explicit", "extern", "false", "finally",
                "fixed", "for", "foreach", "goto", "if", "implicit", "in", "interface",
                "internal", "is", "lock", "new", "null", "operator","catch",
                "out", "override", "params", "private", "protected", "public", "readonly",
                "ref", "return", "sealed", "sizeof", "stackalloc", "static",
                "switch", "this", "throw", "true", "try", "typeof", "namespace",
                "unchecked", "unsafe", "virtual", "void", "while", "float", "int", "long", "object",
                "get", "set", "new", "partial", "yield", "add", "remove", "value", "alias", "ascending",
                "descending", "from", "group", "into", "orderby", "select", "where",
                "join", "equals", "using","bool", "byte", "char", "decimal", "double", "dynamic",
                "sbyte", "short", "string", "uint", "ulong", "ushort", "var", "class", "struct" };

                    }
                    else if (radioBtnBlackCat.Checked == true)
                    {
                        palabrasReservadas = new List<string>() { "abstracto", "como", "asincrono", "esperar",
                "comprobar", "constante", "seguir", "defecto", "delegar", "base", "romper", "caso",
                "hacer", "sino", "enumeracion", "evento", "explicito", "externo", "falso", "finalmente",
                "reparar", "por", "porcada", "ir", "si", "implicito", "en", "interfaz",
                "interno", "es", "cerar", "nuevo", "nulo", "operador","captura",
                "fuera", "anular", "parametro", "privado", "protegido", "publico", "lectura",
                "arbitraria", "regresa", "sellado", "tamaño", "ampilar", "estatico",
                "cambio", "esto", "lanzar", "verdadero", "tratar", "tipo de", "nombre",
                "desenfrenado", "inseguro", "virtual", "vacio", "mientras", "flotador", "entero", "prolongar", "objeto",
                "obtener", "asignar", "nuevo", "parcial", "producir", "añadir", "borrar", "valor", "alias", "asendente",
                "desende ","desde", "grupo", "dentro", "ordenar", "seleccionar", "donde",
                "entrar", "igual", "utilizar","booleano", "byte", "caracter", "decimal", "doble", "dinamico",
                "sbyte", "corto", "cadena", "uentero", "ulargo", "ucorto", "var", "clase", "estructura", "desde", "grupo", "dentro", "ordenar", "seleccionar", "donde",
                "entrar", "igual", "utilizar","booleano", "byte", "caracter", "decimal", "doble", "dinamico",
                "sbyte", "corto", "cadena", "uentero", "ulargo", "ucorto", "var", "clase", "estructura" };
                    }
                    else
                    {
                        MessageBox.Show("No seleccionaste ningun lenguaje para " +
                                        "mostrar las palabras reservadas ", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                    csLexer.Compile(RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.ExplicitCapture);

                    load = true;
                    string tablaPadre = "";
                    AnalizeCode(PagCodigo.Text, 1, tablaPadre);
                    //btnAnalisisSemantico_Click(sender, e);
                    dividirTextoxLinea();
                    /*if(PagCodigo.Text.Contains('{'))    
                        bloquesLexico();
                    */
                    PagCodigo.Focus();
                }

                tcAnalizador.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("No se encontro código para realizar el analizador léxico. ", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        #endregion

        #region EVENTO PARA EL BOTÓN SINTÁCTICO
        /// <summary>
        /// EVENTO CLICK DEL BOTÓN SINTÁCTICO PARA MOSTRAR EL ÁRBOL SINTÁCTICO
        /// </summary>
        private void btnSintactico_Click(object sender, EventArgs e)
        {

            if (PagCodigo.Text != "")
            {
                llamada(PagCodigo.Text);

                tcAnalizador.SelectedIndex = 1;
                string a = "hola";
                Insertar(raiz,a);
            }
            else{
                MessageBox.Show("No se encontro código para realizar el analizador semántico. ", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        #endregion

        #region EVENTO PARA EL BOTÓN DIRECTORIO
        /// <summary>
        /// EVENTO CLICK DEL BOTON "DIRECTORIO" PARA ABRIR ESPECIFICAR LA RUTA DEL USUARIO Y PODER GENERAR EL ÁRBOL
        /// </summary>
        private void btnDirectorio_Click(object sender, EventArgs e)
        {
            string rutaDirectorio = string.Empty;
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                rutaDirectorio = fbd.SelectedPath;
            }
            txtPath.Text = rutaDirectorio;
        }
        #endregion

        #region EVENTOS DEL MENÚ ARCHIVO
        /// <summary>
        /// EVENTO PARA ABRIR UN ARCHIVO
        /// </summary>
        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            abrirarchivo();
        }

        /// <summary>
        /// EVENTO CLICK PARA GUARDAR UN ARCHIVO QUE VA A CONTENER LOS DATOS DE LA TABLA DE SIMBOLOS, PALABRAS RESERVADAS Y EL CODIGO.
        /// </summary>
        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog guardar = new SaveFileDialog();
            guardar.Filter = "documento de texto|*.txt";
            guardar.Title = "GUARDAR COMO";
            guardar.FileName = "Sin titulo";
            var resultado = guardar.ShowDialog();
            if (resultado == DialogResult.OK)
            {
                StreamWriter escribir = new StreamWriter(guardar.FileName);
                foreach (object line in PagCodigo.Lines)
                {
                    escribir.WriteLine(line);
                }
                escribir.Close();
            }
        }
        #endregion

        #region EVENTO PARA EL MENU BORRAR DATOS
        /// <summary>
        /// EVENTO CLICK PARA BORRAR LOS DATOS DE LA TABLA DE SIMBOLOS, PALABRAS RESERVADAS Y CODIGO.
        /// </summary>
        private void borrarDatosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Seguro que quieres BORRAR TODO sin guardar?", "ALERTA", MessageBoxButtons.YesNo,
               MessageBoxIcon.Warning);

            if (res == DialogResult.Yes)
            {

                txtLenguaje.Text = "";
                listViewToken.Items.Clear();

                PagCodigo.Text = "";

            }
        }

        #endregion

        #region EVENTO PARA SALIR DE LA APLICACION
        /// <summary>
        /// EVENTO CLICK DE UN PICTURE BOX PARA SALIR DE LA APLICACIÓN.
        /// </summary>
        private void pictureBoxSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region EVENTOS PARA EL MENU AYUDA DEL COMPILADOR
        /// <summary>
        /// EVENTO CLICK PARA HABLAR SOBRE EL COMPILADOR Y MOSTRAR LA INFORMACIÓN DEL EQUIPO.
        /// </summary>
        private void acercaDeCompEdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Compilador basado en lenguaje C# para \n " +
                            "leer proyectos de programacion escritos en el lenguaje \n" +
                            "Java y un lenguaje creado llamado -> 'blackCat'.\n" +
                            "Información de los colaboradores:\n" +
                            "Instituto Tecnológico de la Laguna\n" +
                            "Lenguajes y Automatás II\n" +
                            "Equipo #1 - Integrantes:\n" +
                            "-Zaida Sugey Gómez Montes -#18131243\n" +
                            "-Jesús Romero Vázquez -#18131279\n" +
                            "-Carlos Elian Castañeda Limones- -#18131225\n" +
                            "-Brandon Daniel Salazar López -#18131281\n" +
                            "-Marian Areli Alfaro Garza -#18131213\n" +
                            "-Johan Ismael López Flores -#18130568", "Acerca del compilador", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// EVENTO CLICK PARA EXPLICAR COMO FUNCIONA EL COMPILADOR MOSTRANDO UNA PEQUEÑA AYUDA.
        /// </summary>
        private void verLaAyudaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Ayuda GENERAL para el funcionamiento del Compilador \n" +
                "Este compilador esta basado en el lenguaje C# para leer proyectos\n" +
                "de programación escritos en el lenguaje Java y un lenguaje llamado\n " +
                "BlackCat.\n" +
                "Este compilador cuenta con un menu de archivos que nos permite abrir y guardar los datos\n" +
                "de las palabras reservadas, el código que haya digitalizado el usuario y la información de\n" +
                "la tabla de simbolos.\n" +
                "Cuenta con otro menú de Ayuda, donde nos permite ver la información del compilador\n, " +
                "y los datos personales de los colaboradores. De igual forma cuenta con un apartado\n" +
                "para ver la ayuda el compilador y conocer su funcionamiento.\n" +
                "También tiene un boton para borrar los datos.\n" +
                "Además contiene un menú donde nos facilita un ejemplo de código del lenguaje Java \n" +
                "y BlackCat.\n" +
                "Este compilador tiene dos botones que realiza el análisis Léxico(muestra la tabla de simbolos)\n" +
                ",un analizador semántico(muestra un árbol semántico según la línea que seleccione el usuario\n" +
                "con el mouse).\n" +
                "y un botón de ERRORES (muestra la línea, el tipo de error (error léxico, sintáctico y semántico)\n" +
                ",además de la descripción de dicho error para que pueda corregirlo el usuario).\n" +
                "Y por último, un botón para cerrar la aplicación. ", "Ayuda del compilador", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// EVENTO CLICK DEL MENÚ ITEMS PARA ABRIR UN ARCHIVO PDF QUE MUESTRA UN MANUAL DE USUARIO DEL COMPILADOR
        /// </summary>
        private void manualDeUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            System.Diagnostics.Process.Start(Application.StartupPath + @"\\Equipo 1 Manual de Usuario Compilador.pdf");
        }

        #endregion

        #region EVENTOS PARA SELECCIONAR EL LENGUAJE
        /// <summary>
        /// EVENTO CLICK PARA CAMBIAR LA SELECCION DEL INDICE DEL LIST BOX
        /// </summary>
        private void listBoxLenguaje_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listBoxLenguaje.SelectedIndex;
            int count = listBoxLenguaje.Items.Count;
            for (int x = 0; x < count; x++)
            {
                if (index != x)
                {
                    listBoxLenguaje.SetItemChecked(x, false);
                    
                }
                
            }

            listBoxLenguaje.SelectedIndex = 0;

            if ((string)listBoxLenguaje.Items[listBoxLenguaje.SelectedIndex] == "Java")
            {
                
                txtLenguaje.Text =
                "abstract\r\n" + "as\r\n" + "async\r\n" + "await\r\n" +
                "checked\r\n" + "const\r\n" + "continue\r\n" + "default\r\n" + "delegate\r\n" + "base\r\n" + "break\r\n" + "case\r\n" +
                "do\r\n" + "else\r\n" + "enum\r\n" + "event\r\n" + "explicit\r\n" + "extern\r\n" + "false\r\n" + "finally\r\n" +
                "fixed\r\n" + "for\r\n" + "foreach\r\n" + "goto\r\n" + "if\r\n" + "implicit\r\n" + "in\r\n" + "interface\r\n" +
                "internal\r\n" + "is\r\n" + "lock\r\n" + "new\r\n" + "null\r\n" + "operator\r\n" + "catch\r\n" +
                "out\r\n" + "override\r\n" + "params\r\n" + "private\r\n" + "protected\r\n" + "public\r\n" + "readonly\r\n" +
                "ref\r\n" + "return\r\n" + "sealed\r\n" + "sizeof\r\n" + "stackalloc\r\n" + "static\r\n" +
                "switch\r\n" + "this\r\n" + "throw\r\n" + "true\r\n" + "try\r\n" + "typeof\r\n" + "namespace\r\n" +
                "unchecked\r\n" + "unsafe\r\n" + "virtual\r\n" + "void\r\n" + "while\r\n" + "float\r\n" + "int\r\n" + "long\r\n" + "object\r\n" +
                "get\r\n" + "set\r\n" + "new\r\n" + "partial\r\n" + "yield\r\n" + "add\r\n" + "remove\r\n" + "value\r\n" + "alias\r\n" + "ascending\r\n" +
                "descending\r\n" + "from\r\n" + "group\r\n" + "into\r\n" + "orderby\r\n" + "select\r\n" + "where\r\n" +
                "join\r\n" + "equals\r\n" + "using\r\n" + "bool\r\n" + "byte\r\n" + "char\r\n" + "decimal\r\n" + "double\r\n" + "dynamic\r\n" +
                "sbyte\r\n" + "short\r\n" + "String\r\n" + "uint\r\n" + "ulong\r\n" + "ushort\r\n" + "var\r\n" + "class\r\n" + "struct\r\n";
            }
            else if ((string)listBoxLenguaje.Items[listBoxLenguaje.SelectedIndex] == "BlackCat")
            {
                
                txtLenguaje.Text =
                "abstracto\r\n" + "como\r\n" + "asincrono\r\n" + "esperar\r\n" +
                "comprobar\r\n" + "constante\r\n" + "seguir\r\n" + "defecto\r\n" + "delegar\r\n" + "base\r\n" + "romper\r\n" + "caso\r\n" +
                "hacer\r\n" + "contrario\r\n" + "enumeracion\r\n" + "evento\r\n" + "explicito\r\n" + "externo\r\n" + "falso\r\n" + "finalmente\r\n" +
                "reparar\r\n" + "por\r\n" + "porcada\r\n" + "ir\r\n" + "si\r\n" + "implicito\r\n" + "en\r\n" + "interfaz\r\n" +
                "interno\r\n" + "es\r\n" + "cerar\r\n" + "nuevo\r\n" + "nulo\r\n" + "operador\r\n" + "captura\r\n" +
                "fuera\r\n" + "anular\r\n" + "parametro\r\n" + "privado\r\n" + "protegido\r\n" + "publico\r\n" + "lectura\r\n" +
                "arbitraria\r\n" + "regresa\r\n" + "sellado\r\n" + "tamaño\r\n" + "ampilar\r\n" + "estatico\r\n" +
                "cambio\r\n" + "esto\r\n" + "lanzar\r\n" + "verdadero\r\n" + "tratar\r\n" + "tipo de\r\n" + "nombre\r\n" +
                "desenfrenado\r\n" + "inseguro\r\n" + "virtual\r\n" + "vacio\r\n" + "mientras\r\n" + "flotador\r\n" + "entero\r\n" + "prolongar\r\n" + "objeto\r\n" +
                "obtener\r\n" + "asignar\r\n" + "nuevo\r\n" + "parcial\r\n" + "producir\r\n" + "añadir\r\n" + "borrar\r\n" + "valor\r\n" + "alias\r\n" + "asendente\r\n" +
                "desende \r\n" + "desde\r\n" + "grupo\r\n" + "dentro\r\n" + "ordenar\r\n" + "seleccionar\r\n" + "donde\r\n" +
                "entrar\r\n" + "igual\r\n" + "utilizar\r\n" + "booleano\r\n" + "byte\r\n" + "caracter\r\n" + "decimal\r\n" + "doble\r\n" + "dinamico\r\n" +
                "sbyte\r\n" + "corto\r\n" + "cadena\r\n" + "uentero\r\n" + "ulargo\r\n" + "ucorto\r\n" + "var\r\n" + "clase\r\n" + "estructura\r\n" + "desde\r\n" + "grupo\r\n" + "dentro\r\n" + "ordenar\r\n" + "seleccionar\r\n" + "donde\r\n" +
                "entrar\r\n" + "igual\r\n" + "utilizar\r\n" + "booleano\r\n" + "byte\r\n" + "caracter\r\n" + "decimal\r\n" + "doble\r\n" + "dinamico\r\n" +
                "sbyte\r\n" + "corto\r\n" + "cadena\r\n" + "uentero\r\n" + "ulargo\r\n" + "ucorto\r\n" + "var\r\n" + "clase\r\n" + "estructura";
            }


        }

        /// <summary>
        /// EVENTO DE CARGA DEL FORMULARIO PARA QUE SE AGREGUEN LOS ITEMS AL LIST BOX Y CAMBIE EL INDICE.
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            listBoxLenguaje.Items.Add("Java", true);
            listBoxLenguaje.Items.Add("BlackCat", false);

            this.listBoxLenguaje_SelectedIndexChanged(sender, e);

        }

        #endregion

        #region EVENTOS PARA ABRIR UN EJEMPLO EN JAVA O BLACKCAT
        /// <summary>
        /// EVENTO CLICK PARA ABRIR UN ARCHIVO TXT CON UN EJEMPLO EN JAVA 
        /// </summary>
        private void javaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "ABRIR ARCHIVO";
                ofd.ShowDialog();
                if (File.Exists(ofd.FileName))
                {
                    using (Stream stream = ofd.OpenFile())
                    {
                        leerarchivo(ofd.FileName);
                    }

                }
            }
            catch (Exception)
            {

                MessageBox.Show("El archivo no se abrio correctamente");


            }

        }

        /// <summary>
        /// EVENTO CLICK PARA ABRIR UN ARCHIVO TXT CON UN EJEMPLO EN EL LENGUAJE BLACKCAT
        /// </summary>
        private void blackCatToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "ABRIR ARCHIVO";
                ofd.ShowDialog();
                if (File.Exists(ofd.FileName))
                {
                    using (Stream stream = ofd.OpenFile())
                    {
                        leerarchivo(ofd.FileName);
                    }

                }
            }
            catch (Exception)
            {

                MessageBox.Show("El archivo no se abrio correctamente");


            }
        }

        #endregion

        #region EVENTO DEL MOUSE PARA GENERAR EL ARBOL BINARIO
        /// <summary>
        /// EVENTO DEL MOUSE PARA CUANDO EL USUARIO SELECCIONE UNA LINEA DEL CODIGO Y SE GENERE EL ARBOL
        /// </summary>
        private void PagCodigo_MouseDown(object sender, MouseEventArgs e)
        {
            if(checkBoxMouse.Checked == true) { 
            if(txtPath.Text != "") {
                
            string codigo = "";
            int lineaCodigo = this.PagCodigo.GetLineFromCharIndex(this.PagCodigo.SelectionStart);
      
            if (lineaCodigo != 0)
            {
                codigo = PagCodigo.Lines.ElementAt(lineaCodigo);
                llamada(codigo);
            }
            }
            }
        }

        #endregion

        #region EVENTO DEL BOTÓN ERRORES

        /// <summary>
        /// EVENTO CLICK DEL BOTÓN "ERRORES" PARA VISUALIZAR EN UNA LISTA LOS ERRORES LÉXICOS, SINTÁCTICOS Y SEMÁNTICOS
        /// </summary>
        private void btnAnalisisSemantico_Click(object sender, EventArgs e)
        {
            listViewError.Items.Clear();
            dividirTextoxLinea();
            analizaLlaves();
            lexico();
            comprobarExpresionAlgebraica();
        }
        #endregion

        #region EVENTO DEL BOTÓN CUADRUPLOS
        /// <summary>
        /// EVENTO CLICK DEL BOTÓN "CUADRUPLOS" PARA GENERAR EL CODIGO INTERMEDIO CON CUADRUPLOS
        /// </summary>
        private void btnCuadruplos_Click(object sender, EventArgs e)
        {
            masexpresiones = false;
            errorLlave = false;
            inicioCiclo = 5;
            inicioCicloElse = 6;
            lblTrue.Text = " ";
            //listViewError.Items.Clear();
            lblVerdadero.Text = "";
            lblFalso.Text = "";
            tabCuadruplo = true;

            //analizaLlaves();
            dividirTextoxLinea();
            lexico();   
            comprobarCuadruplos();
            if (errorLlave == true)
            {
                MessageBox.Show("FALTA CERRAR LLAVE");
                listViewCuadruplos.Items.Clear();
                lblTrue.Text = "";
                lblFalso.Text = "";
                lblVerdadero.Text = "";
            }
        }
        #endregion

        #endregion

        //************************************************************************************************************
        // METODOS
        //************************************************************************************************************
        #region METODOS

        #region MÉTODO PARA EL SPLASH
        /// <summary>
        /// MÉTODO PARA INICIAR EL FORMULARIO DONDE SE ENCUENTRA EL SPLASH
        /// </summary>
        public void SplashStart()
        {
           // Application.Run(new Splash());
        }
        #endregion

        #region MÉTODOS PARA ARCHIVO 

        /// <summary>
        /// METODO PARA LEER UN ARCHIVO
        /// </summary>
        /// <param name="nomarchivo">el parametro corresponde al nombre del archivo</param>
        public void leerArchivo(string nomarchivo)
        {
            StreamReader reader = new StreamReader(nomarchivo, System.Text.Encoding.Default);
            string texto;
            texto = reader.ReadToEnd();
            reader.Close();
            txtLenguaje.Text = texto;
        }

        /// <summary>
        /// METODO PARA ABRIR UN ARCHIVO
        /// </summary>
        public void abrirarchivo()
        {

            try
            {
                OpenFileDialog ofd = new OpenFileDialog();

                //ofd.Title = "Compilador#                                                                     Abrir Archivo                                                                       ";
                ofd.Title = "ABRIR ARCHIVO";
                ofd.ShowDialog();
                // ofd.Filter = "Archivos ed#(*.ed)|*.ed";
                if (File.Exists(ofd.FileName))
                {
                    using (Stream stream = ofd.OpenFile())
                    {
                        leerarchivo(ofd.FileName);

                    }

                }
            }
            catch (Exception)
            {

                MessageBox.Show("El archivo no se abrio correctamente");


            }

        }

        /// <summary>
        /// METODO PARA LEER UN ARCHIVO 
        /// </summary>
        /// <param name="nomarchivo">parametro para el nombre del archivo</param>
        public void leerarchivo(string nomarchivo)
        {
            StreamReader reader = new StreamReader(nomarchivo, System.Text.Encoding.Default);
            string texto;
            texto = reader.ReadToEnd();
            reader.Close();

            PagCodigo.Text = texto;

            


        }
        #endregion

        #region MÉTODO PARA DIVIDIR TEXTO POR LÍNEA

        /// <summary>
        /// MÉTODO PARA DIVIDIR EL CÓDIGO QUE INGRESE EL USUARIO PERO POR LÍNEAS
        /// </summary>
        public void dividirTextoxLinea()
        {
            contenido.Clear();                                          // Limpiar lista contenido

            String codigo = PagCodigo.Text;                             // Poner todo el código en la variable
            String palabra = "";                                        // Variable auxiliar                    
            codigo += '\n';                                             // Concatenar un salto de linea                                            

            foreach (Char c in codigo)
            {                                                           // Por cada caracter en el código
                if (c == '\n')
                {                                                       // Si se encuantra un salto de linea                                                      
                    String p = "";                                      // Variable local 

                    if (palabra.Contains("//"))
                    {
                        p = palabra.Substring(0, palabra.Length - 2);   // Enviar la palabra sin las diagonales de comentarios
                    }
                    else
                    {
                        p += palabra;                                   // p = palabra, palabra contiene la linea actual
                    }
                    contenido.Add(p);                                   // Añadir p a la lista    
                    palabra = "";                                       // Limpiar la palabra            
                }
                else if (c == '\r' || c == '\t')                        // Condición para ignorar otros caracteres especiales
                { }
                else
                {
                    if (palabra.Contains("//")) { }                       // Eliminar comentario, pero no //
                    else if (c == ' ' && palabra.Length == 0) { }         // Eliminar espacios iniciales    
                    else
                    {
                        palabra += c;                                   // Sumar el caracter a la linea}
                    }
                }

                string a = "";
            }
            

        }
        #endregion

        #region MÉTODOS PARA EL ANALIZADOR LÉXICO

        /// <summary>
        /// METODO PARA ANALIZAR EL CODIGO QUE SE INGRESA
        /// </summary>
        private void AnalizeCode(string texto, int indiceT, string tablaPadre)
        {

            if (titulos == true)
            {
                ListViewItem listaTitulos = new ListViewItem("Token");

                listaTitulos.SubItems.Add("Lexema");
                listaTitulos.SubItems.Add("Linea");
                listaTitulos.SubItems.Add("Columna");
                listaTitulos.SubItems.Add("Indice");
                listaTitulos.SubItems.Add("Tipo de Dato");
                listaTitulos.SubItems.Add("ID Tabla");
                listaTitulos.SubItems.Add("Tabla Padre");

                listViewToken.Items.Add(" ");
                listViewToken.Items.Add(listaTitulos).BackColor = Color.FromArgb(255, 255, 67);

            }
            titulos = true;
            int n = 0, e = 0;

            foreach (var tk in csLexer.GetTokens(texto))
            {

                if (tk.Name == "ERROR")
                {
                    e++;
                }
                if (tk.Name == "IDENTIFICADOR")
                {
                    if (palabrasReservadas.Contains(tk.Lexema))
                    {
                        tk.Name = "RESERVADO";
                    }
                }
                //ListViewItem listaNombre = new ListViewItem(tk.Name);
                ListViewItem listaLexema = new ListViewItem(tk.Name);

                listaLexema.SubItems.Add(tk.Lexema);
                listaLexema.SubItems.Add(tk.Linea.ToString());
                listaLexema.SubItems.Add(tk.Columna.ToString());
                listaLexema.SubItems.Add(tk.Index.ToString());
                listaLexema.SubItems.Add(tk.TipoDato.ToString());
                listaLexema.SubItems.Add(indiceT.ToString());
                listaLexema.SubItems.Add(tablaPadre.ToString());

                listViewToken.Items.Add(listaLexema).BackColor = Color.FromArgb(105, 255, 239);

                n++;

            }


            label1.Text = string.Format("Analizador Lexico - {0} tokens {1} errores", n, e);

            //AGREGA DATOS EN LA COLUMNA LLAMADA TIPO DE DATOS, SOLO CUANDO ES UN IDENTIFICADOR
           
            for (int a = 0; a < listViewToken.Items.Count; a++)
            {
                if (listViewToken.Items[a].SubItems[0].Text == "IDENTIFICADOR" && texto.Contains(" ") && listViewToken.Items[a - 1].SubItems[0].Text == "RESERVADO")
                {
                    string dato = listViewToken.Items[a - 1].SubItems[1].Text;
                    listViewToken.Items[a].SubItems[5].Text = dato;
                }


            }

        }

        /// <summary>
        /// METODO PARA REALIZAR LA LLAMADA DEL ANALIZADOR LEXICO
        /// </summary>
        /// <param name="codigo">parametro para el codigo que se ingresa</param>
        public void llamada(string codigo)
        {
            if (codigo != "")
            {
                if (radioBtnJava.Checked == true)
                {
                    if (palabrasReservadas.Contains(codigo)) ;
                    {
                        cadena = Regex.Replace(codigo, "abstract | as |async |await |checked |const |continue |default |delegate |base |break |case |" +
                    "do |else |enum |event |explicit |extern |false |finally |fixed |for |foreach |goto |if |implicit |in |interface |internal |is |lock |new |null |operator |catch |" +
                    "out|override |params |private |protected |public |readonly |ref |return |sealed |izeof |stackalloc |static |switch |this |throw |true |try |typeof |namespace |" +
                    "unchecked |unsafe |virtual |void |while |float |int |long |object |get |set |new |partial |yield |add |remove |value |alias |ascending |" +
                    "descending |from |group |into |orderby |select |where |" +
                    "join|equals |using |bool |byte |char |decimal |double |dynamic |" +
                    "sbyte |short |string |uint |ulong |ushort |var |class |struct", "");
                        //txtLenguaje.Text = cadena;
                    }
                }
                else if (radioBtnBlackCat.Checked == true)
                {
                    if (palabrasReservadas.Contains(codigo)) ;
                    {
                        cadena = Regex.Replace(codigo, "abstracto | como |asincrono |esperar |comprobar |constante |seguir |defecto |delegar |base |romper |caso |" +
                    "hacer |sino |enumeracion |evento |explicito |externo|falso |finalmente |reparar |por |porcada |ir |si |implicito |en |interfaz |interno |es |cerar |nuevo |nulo |operador |captura |" +
                    "fuera|anular |parametro |privado |protegido |publico |lectura |arbitraria |regresa |sellado |tamaño |ampilar |estatico |cambio |esto |lanzar |verdadero |tratar |tipo de |nombre |" +
                    "desenfrenado |inseguro |virtual |vacio |mientras |flotante |entero |prolongar |objeto |obtener |asignar |nuevo |parcial |producir |añadir |borrar |valor |alias |asendente |" +
                    "desende |desde |grupo |dentro |ordenar |seleccionar |donde |" +
                    "entrar|igual |utilizar |booleano |byte |caracter |decimal |doble |dinamico |" +
                    "sbyte |corto |cadena |uentero |ulargo |ucorto |var |clase |estructura", "");
                        //txtLenguaje.Text = cadena;
                    }
                }
                
                arbol.insertarEnCola(cadena);
                raiz = arbol.crearArbol();

                //arbol.Limpiar();
                //Se insertan en las etiquetas los recorrimientos (Infijo, Postfijo y Prefijo).
                //pre, in, post
                grafico = new Grafico(arbol.NodoDot);
                grafico.DrawTree();
                ShowTree();
                }
            else
            {
                MessageBox.Show("Debes ingresar una expresión aritmética. Intentalo de nuevo", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        /// <summary>
        /// MÉTODO PARA MOSTRAR TABLAS DE SIMBOLOS POR CADA BLOQUE DE CÓDIGO
        /// </summary>
        public void bloquesLexico()
        {
            int numTablas = 0;

            foreach (String s in contenido)
            {
                if (s.Contains('{'))
                {
                    pilaLexico.Push("{");
                    numTablas++;
                }
                else if (s.Contains('}'))
                {
                    pilaLexico.Push("}");
                }
                else
                {
                    pilaLexico.Push(s);
                }
            }
            string[] lexico = new string[numTablas];
            //pilaLexico.Pop();
            int i = 0;
            while (pilaLexico.Count != 0)
            {
                if (pilaLexico.Peek() == "}")
                {
                    string valAnterior = pilaLexico.Pop().ToString();
                }

                if (pilaLexico.Peek() != "}")
                {
                    lexico[i] += pilaLexico.Pop().ToString() + " ";
                    string valSiguiente = pilaLexico.Peek().ToString();
                    if (valSiguiente == "}")
                    {
                        i++;
                    }
                    if (valSiguiente == "{")
                    {
                        if (i == 0)
                        {
                            i++;
                        }
                        else
                        {
                            i--;
                        }
                        //i--;
                    }
                    if (pilaLexico.Peek() == "{")
                    {
                        pilaLexico.Pop();
                        if (pilaLexico.Count != 0)
                        {
                            if (pilaLexico.Peek() == "}")
                                i += 2;
                        }
                    }
                }
            }
            for (int j = 0; j < lexico.Length; j++)
            {
                AnalizeCode(lexico[j], numTablas + 1, "1");
                numTablas--;
            }
        }
        #endregion

        #region MÉTODOS PARA GENERAR ÁRBOL SINTÁCTICO

        /// <summary>
        /// METODO PARA MOSTRAR EL ARBOL SINTÁCTICO
        /// </summary>
        private void ShowTree()
        {
            if (File.Exists(@txtPath.Text + "/Imagen.png"))
            {
                using (FileStream img = new FileStream(@txtPath.Text + "/Imagen.png", FileMode.Open, FileAccess.Read))
                {
                    pbArbol.Image = Bitmap.FromStream(img);


                }
            }
            else
            {
                MessageBox.Show("No se ha podido abrir el archivo.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            pbArbol.Refresh();
        }

        /// <summary>
        /// MÉTODO PARA INSERTAR UN NODO PARA EL ÁRBOL SINTÁCTICO
        /// </summary>
        /// <param name="ar">PARAMETRO QUE RECIBE UN OBJETO DE LA CLASE Nodo.</param>
        /// <param name="cad">PARAMETRO QUE RECIBE UNA CADENA.</param>
        private void Insertar(Nodo ar, string cad)
        {

            if (ar == null)
            {
                Nodo nuevo = new Nodo(cad);
                ar = nuevo;
                new Nodo(null, nuevo, nuevo);
                object sender = null;
                EventArgs e = null;

            }
            else
            {
                string valorRaiz = ar.Datos.ToString();
                if (cad != valorRaiz)
                {
                    Insertar(ar.NodoIzquierdo, cad);
                }
                else
                {
                    Insertar(ar.NodoDerecho, cad);
                }
            }
        }

        /// <summary>
        /// MÉTODO PARA PASAR UNA CADENA INFIJA A POSFIJA
        /// </summary>
        /// <param name="stringeninfijo">PARAMETRO QUE RECIBE UNA CADENA INFIJA</param>
        /// <returns></returns>
        private string PasarStringPosfijo(string stringeninfijo)
        {
            int tamanio = stringeninfijo.Length;
            Stack<char> pila = new Stack<char>();
            StringBuilder stringenposfijo = new StringBuilder();
            for (int i = 0; i < tamanio; i++)
            {
                if ((stringeninfijo[i] >= '0') && (stringeninfijo[i] <= '9'))
                {
                    stringenposfijo.Append(" ");
                    stringenposfijo.Append(stringeninfijo[i]);

                }
                else if (stringeninfijo[i] == '(')
                {
                    pila.Push(stringeninfijo[i]);
                }
                else if ((stringeninfijo[i] == '*') || (stringeninfijo[i] == '+') || (stringeninfijo[i] == '-') || (stringeninfijo[i] == '/'))
                {
                    while ((pila.Count > 0) && (pila.Peek() != '('))
                    {
                        if (precedenciadeoperadores(pila.Peek(), stringeninfijo[i]))
                        {
                            stringenposfijo.Append(pila.Pop());
                        }
                        else
                        {
                            break;
                        }
                    }
                    pila.Push(stringeninfijo[i]);
                }
                else if (stringeninfijo[i] == ')')
                {
                    while ((pila.Count > 0) && (pila.Peek() != '('))
                    {
                        stringenposfijo.Append(pila.Pop());
                    }
                    if (pila.Count > 0)
                        pila.Pop(); //quita el parentesis izquierdo de la pila
                }
                if (stringeninfijo[i] == ' ')
                {
                    pila.Push(stringeninfijo[i]);
                }
            }
            while (pila.Count > 0)
            {
                stringenposfijo.Append(pila.Pop());
            }
            return stringenposfijo.ToString();
        }

        /// <summary>
        /// MÉTODO PARA VERIFICAR LA PRECEDENCIA DE OPERADORES
        /// </summary>
        /// <param name="top">PARAMETRO QUE RECIBE UN CARACTER QUE SE ENCUENTRA EN EL TOPE</param>
        /// <param name="p_2">PARAMETRO QUE RECIBE UN CARACTER QUE SE ENCUENTRA EN LA POSICIÓN DOS</param>
        /// <returns></returns>
        public bool precedenciadeoperadores(char top, char p_2)
        {
            if (top == '+' && p_2 == '*') // + tiene menor precedencia que *
                return false;
            if (top == '*' && p_2 == '-') // * tiene mayor precedencia que -
                return true;
            if (top == '+' && p_2 == '-') // + tiene la misma precedencia que +
                return true;
            return true;
        }

        /// <summary>
        /// MÉTODO PARA EVALUAR UN RESULTADO DE LA CONVERSIÓN QUE SE HIZO A POSFIJA
        /// </summary>
        /// <param name="posfija">PARAMETRO QUE RECIBE UNA CADENA QUE ESTA EN POSFIJA</param>
        /// <returns></returns>
        public int EvaluarRes(string posfija)
        {
            Stack<int> pilaResultado = new Stack<int>();
            int tama = posfija.Length;
            for (int i = 0; i < tama; i++)
            {
                if ((posfija[i] == '*') || (posfija[i] == '+') || (posfija[i] == '-') || (posfija[i] == ' '))
                {
                    int resz = DimeOperador(pilaResultado.Pop(), pilaResultado.Pop(), posfija[i]);
                    pilaResultado.Push(resz);
                }
                else if ((posfija[i] >= '0') || (posfija[i] <= '9'))
                {
                    pilaResultado.Push((int)(posfija[i] - '0'));
                }
            }
            return pilaResultado.Pop();
        }

        /// <summary>
        /// MÉTODO PARA LA DIMENSIÓN DE UN OPERADOR
        /// </summary>
        /// <param name="p">PARAMETRO ENTERO QUE RECIBE EL PRIMER OPERADOR</param>
        /// <param name="p_2">PARAMETRO ENTERO QUE RECIBE EL SEGUNDO OPERADOR</param>
        /// <param name="p_3">PARAMETRO ENTERO QUE RECIBE EL TERCER OPERADOR</param>
        /// <returns></returns>
        public int DimeOperador(int p, int p_2, char p_3)
        {
            switch (p_3)
            {
                case '+':
                    return p_2 + p;
                case '-':
                    return p_2 - p;
                case '*':
                    return p_2 * p;
                case '/':
                    return p_2 / p;
                default:
                    return -1;
            }
        }
        #endregion

        #region MÉTODOS PARA VERIFICACIÓN DE ERRORES

        #region MÉTODOS PARA ERRORES LÉXICOS Y SINTÁCTICOS

        /// <summary>
        /// MÉTODO PARA ANALIZAR LOS ERRORES SINTÁCTICOS Y LÉXICOS DEL CÓDIGO
        /// </summary>
        public void lexico()
        {
            //listViewError.Items.Clear();
            string[] palabrasResevadasBasicas = { "int", "string", "double", "char", "float", "bool" };
            RegexLexer csIdentificador = new RegexLexer();
            int linea = 0;

            Regex rgxId = new Regex(@"\b[_a-zA-Z][\w]*\b");
            Regex rgxNumEnteros = new Regex(@"^-?[0-9]+$");
            Regex rgxNumDoubles = new Regex(@"\d+\.\d");
            Regex rgxNumFloatInt = new Regex(@"^-?[0-9]+[fF?]+$");
            Regex rgxNumFloatDec = new Regex(@"\d+\.\d+[fF?]+$");
            Regex rgxString = new Regex("\".*?\"");
            Regex rgxChar = new Regex(@"'\\.'|'[^\\]'");

            foreach (String s in contenido)
            {
                linea++;
                int tam = 0;
                int d = 0;
                string ultElem = "";
                string el = "";
                bool control = false;
                bool controlDato = false;
                for (int i = 0; i < palabrasResevadasBasicas.Length; i++)
                {
                    if (s.Contains(palabrasResevadasBasicas[i]))
                    {
                        for (int q = 0; q < s.Length; q++)
                        {
                            if (s[q] == ' ')
                                tam++;
                        }
                        string[] arrReservadas = new string[tam + 2];
                        for (int q = 0; q < s.Length; q++)
                        {
                            if (control == true)
                            {
                                if (s[q] == ' ')
                                {
                                    arrReservadas[d] = el;
                                    el = "";
                                    d++;
                                    float h = 10;
                                }
                            }
                            if (s[q] == ' ' && el == "int" | el == "double" | el == "float" | el == "char" | el == "string")
                            {
                                control = true;
                                arrReservadas[d] = el;
                                el = "";
                                d++;
                            }
                            else
                            {
                                el += s[q];
                                if (el.Contains(";"))
                                {
                                    q = s.Length - 1;
                                }
                                if (s[q] == ' ')
                                {
                                    el = "";
                                }
                            }
                        }
                        arrReservadas[d] = el;
                        //arrReservadas = s.Split(' ');
                        foreach (char c in arrReservadas[d])
                        {
                            if (c == ';')
                            {
                                arrReservadas[d + 1] = ";";
                            }
                            else
                            {
                                ultElem += c;
                                arrReservadas[d] = ultElem;
                            }
                        }
                        for (int k = 0; k < arrReservadas.Length; k++)
                        {
                            if (arrReservadas[k] == "int" | arrReservadas[k] == "string" | arrReservadas[k] == "double" | arrReservadas[k] == "float" | arrReservadas[k] == "char" && rgxId.IsMatch(arrReservadas[k + 1]))
                            {
                                if (arrReservadas[k + 2] == "=" | arrReservadas[k + 2] == ";")
                                {
                                    //MessageBox.Show("Expresion correcta");
                                    if (arrReservadas[k + 2] == "=")
                                    {
                                        switch (arrReservadas[k])
                                        {
                                            case "int":
                                                {
                                                    if (arrReservadas[arrReservadas.Length - 1] == ";" | arrReservadas[d + 1] == ";")
                                                    {
                                                        for (int u = 0; u < arrReservadas.Length; u++)
                                                        {
                                                            var isNumericI = int.TryParse(arrReservadas[u], out int n);
                                                            var isNumericD = double.TryParse(arrReservadas[u], out double n2);
                                                            if (isNumericI == true | isNumericD == true)
                                                            {
                                                                controlDato = true;
                                                            }
                                                            if (rgxNumEnteros.IsMatch(arrReservadas[u]))
                                                            {
                                                                controlDato = false;
                                                            }
                                                            if (arrReservadas[u] == ";")
                                                            {
                                                                u = arrReservadas.Length - 1;
                                                            }
                                                            else if (controlDato == true)
                                                            {
                                                                ListViewItem error = new ListViewItem(linea.ToString());
                                                                error.SubItems.Add("Semantico");
                                                                error.SubItems.Add("Int: El valor no corresponde al tipo de dato.");
                                                                listViewError.Items.Add(error).BackColor = Color.FromArgb(247, 75, 64);
                                                                u = arrReservadas.Length - 1;
                                                                //listViewError.ForeColor = Color.Blue; pinta las letras
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ListViewItem error = new ListViewItem(linea.ToString());
                                                        error.SubItems.Add("Semántico");
                                                        error.SubItems.Add("Falta agregar ; en la expresion");
                                                        listViewError.Items.Add(error).BackColor = Color.FromArgb(247, 75, 64);
                                                    }
                                                }
                                                break;
                                            case "double":
                                                {
                                                    if (arrReservadas[arrReservadas.Length - 1] == ";" | arrReservadas[d + 1] == ";")
                                                    {
                                                        for (int u = 0; u < arrReservadas.Length; u++)
                                                        {
                                                            var isNumericI = int.TryParse(arrReservadas[u], out int n);
                                                            var isNumericD = double.TryParse(arrReservadas[u], out double n2);
                                                            if (isNumericI == true | isNumericD == true)
                                                            {
                                                                controlDato = true;
                                                            }
                                                            if (rgxNumDoubles.IsMatch(arrReservadas[u]))
                                                            {
                                                                controlDato = false;
                                                            }
                                                            else if (controlDato == true)
                                                            {
                                                                ListViewItem error = new ListViewItem(linea.ToString());
                                                                error.SubItems.Add("Semántico");
                                                                error.SubItems.Add("Double: El valor no corresponde al tipo de dato.");
                                                                listViewError.Items.Add(error).BackColor = Color.FromArgb(247, 75, 64);
                                                                u = arrReservadas.Length - 1;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ListViewItem error = new ListViewItem(linea.ToString());
                                                        error.SubItems.Add("Semántico");
                                                        error.SubItems.Add("Falta agregar ; en la expresion");
                                                        listViewError.Items.Add(error).BackColor = Color.FromArgb(247, 75, 64);
                                                    }
                                                }
                                                break;
                                            case "float":
                                                {
                                                    if (arrReservadas[arrReservadas.Length - 1] == ";" | arrReservadas[d + 1] == ";")
                                                    {
                                                        for (int u = 0; u < arrReservadas.Length; u++)
                                                        {
                                                            var isNumericI = int.TryParse(arrReservadas[u], out int n);
                                                            var isNumericD = double.TryParse(arrReservadas[u], out double n2);
                                                            if (isNumericI == true | isNumericD == true)
                                                            {
                                                                controlDato = true;
                                                            }
                                                            if (rgxNumFloatInt.IsMatch(arrReservadas[u]) | rgxNumEnteros.IsMatch(arrReservadas[u]) | rgxNumFloatDec.IsMatch(arrReservadas[u]))
                                                            {
                                                                controlDato = false;
                                                            }
                                                            else if (controlDato == true)
                                                            {
                                                                ListViewItem error = new ListViewItem(linea.ToString());
                                                                error.SubItems.Add("Semántico");
                                                                error.SubItems.Add("Float: El valor no corresponde al tipo de dato.");
                                                                listViewError.Items.Add(error).BackColor = Color.FromArgb(247, 75, 64);
                                                                u = arrReservadas.Length - 1;
                                                                //listViewError.ForeColor = Color.Blue; pinta las letras
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ListViewItem error = new ListViewItem(linea.ToString());
                                                        error.SubItems.Add("Semántico");
                                                        error.SubItems.Add("Falta agregar ; en la expresion");
                                                        listViewError.Items.Add(error).BackColor = Color.FromArgb(247, 75, 64);
                                                    }
                                                }
                                                break;
                                            case "string":
                                                {
                                                    if (arrReservadas[arrReservadas.Length - 1] == ";" | arrReservadas[d + 1] == ";")
                                                    {
                                                        for (int u = 0; u < arrReservadas.Length; u++)
                                                        {
                                                            var isNumericI = int.TryParse(arrReservadas[u], out int n);
                                                            var isNumericD = double.TryParse(arrReservadas[u], out double n2);
                                                            if (isNumericI == true | isNumericD == true | arrReservadas[u].Contains('"'))
                                                            {
                                                                controlDato = true;
                                                            }
                                                            if (rgxString.IsMatch(arrReservadas[u]))
                                                            {
                                                                controlDato = false;
                                                            }
                                                            else if (controlDato == true)
                                                            {
                                                                ListViewItem error = new ListViewItem(linea.ToString());
                                                                error.SubItems.Add("Semántico");
                                                                error.SubItems.Add("String: El valor no corresponde al tipo de dato.");
                                                                listViewError.Items.Add(error).BackColor = Color.FromArgb(247, 75, 64);
                                                                u = arrReservadas.Length - 1;
                                                            }
                                                        }
                                                    }

                                                    else
                                                    {
                                                        ListViewItem error = new ListViewItem(linea.ToString());
                                                        error.SubItems.Add("Semántico");
                                                        error.SubItems.Add("Falta agregar ; en la expresion");
                                                        listViewError.Items.Add(error).BackColor = Color.FromArgb(247, 75, 64);
                                                    }
                                                }
                                                break;
                                            case "char":
                                                {
                                                    if (arrReservadas[arrReservadas.Length - 1] == ";" | arrReservadas[d + 1] == ";")
                                                    {
                                                        for (int u = 0; u < arrReservadas.Length; u++)
                                                        {

                                                            if (arrReservadas[arrReservadas.Length - 1] != ";")
                                                            {
                                                                ListViewItem error = new ListViewItem(linea.ToString());
                                                                error.SubItems.Add("Semántico");
                                                                error.SubItems.Add("Falta agregar ; en la expresion");
                                                                u = arrReservadas.Length - 1;
                                                                listViewError.Items.Add(error).BackColor = Color.FromArgb(247, 75, 64);
                                                            }
                                                            if (controlDato == false && arrReservadas[u] == ";")
                                                            {
                                                                ListViewItem error = new ListViewItem(linea.ToString());
                                                                error.SubItems.Add("Semántico");
                                                                error.SubItems.Add("Char: El valor no corresponde al tipo de dato.");
                                                                listViewError.Items.Add(error).BackColor = Color.FromArgb(247, 75, 64);
                                                            }
                                                            var isNumericI = int.TryParse(arrReservadas[u], out int n);
                                                            var isNumericD = double.TryParse(arrReservadas[u], out double n2);
                                                            if (isNumericI == true | isNumericD == true | arrReservadas[u].Contains("'"))
                                                            {
                                                                controlDato = true;
                                                            }
                                                            if (rgxChar.IsMatch(arrReservadas[u]))
                                                            {
                                                                controlDato = false;
                                                                break;
                                                            }
                                                            else if (controlDato == true)
                                                            {
                                                                ListViewItem error = new ListViewItem(linea.ToString());
                                                                error.SubItems.Add("Semántico");
                                                                error.SubItems.Add("Char: El valor no corresponde al tipo de dato.");
                                                                listViewError.Items.Add(error).BackColor = Color.FromArgb(247, 75, 64);
                                                                u = arrReservadas.Length - 1;
                                                                //listViewError.ForeColor = Color.Blue; pinta las letras
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ListViewItem error = new ListViewItem(linea.ToString());
                                                        error.SubItems.Add("Semántico");
                                                        error.SubItems.Add("Falta agregar ; en la expresion");
                                                        listViewError.Items.Add(error).BackColor = Color.FromArgb(247, 75, 64);
                                                    }
                                                }
                                                break;

                                        }
                                    }
                                }
                                else
                                {
                                    ListViewItem error = new ListViewItem(linea.ToString());
                                    error.SubItems.Add("Léxico");
                                    error.SubItems.Add("Identificador mal definido: verifica el nombre de la variable.");
                                    listViewError.Items.Add(error).BackColor = Color.FromArgb(247, 75, 64);
                                }
                                k = arrReservadas.Length - 1;
                            }
                            else
                            {
                                ListViewItem error = new ListViewItem(linea.ToString());
                                error.SubItems.Add("Léxico");
                                error.SubItems.Add("Tipo de dato: declara el nombre del dato.");
                                listViewError.Items.Add(error).BackColor = Color.FromArgb(247, 75, 64);
                                k = arrReservadas.Length - 1;
                            }

                        }
                        i = palabrasResevadasBasicas.Length - 1;
                    }
                }
            }
        }

        #endregion

        #region MÉTODOS DE ERRORES SINTÁCTICOS
        /// <summary>
        /// MÉTODO PARA ANALIZAR LAS LLAVES Y PARENTESIS, Y MOSTRAR LOS ERRORES SINTÁCTICOS
        /// </summary>
        /// <returns>RETORNA EL VALOR QUE SE PRODUJO</returns>
        public bool analizaLlaves()
        {
            int linea = 1;
            int lineaP = 1;
            bool error = true;
            Stack<int> llaves = new Stack<int>();
            Stack<int> parentesis = new Stack<int>();

            foreach (String s in contenido)
            {
                if (s.Contains('{'))
                {
                    foreach (Char c in s)
                    {
                        if (c == '{')
                        {
                            llaves.Push(linea);
                        }
                    }
                }
                if (s.Contains('('))
                {
                    foreach (Char c in s)
                    {
                        if (c == '(')
                        {
                            parentesis.Push(lineaP);
                        }
                    }
                }
                if (s.Contains('}'))
                {
                    foreach (Char c in s)
                    {
                        if (llaves.Count > 0)
                        {
                            if (c == '}')
                            {
                                llaves.Pop();
                            }
                        }
                        else if (c == '}' && llaves.Count == 0)
                        {
                            ListViewItem listaTitulos = new ListViewItem(linea.ToString());
                            listaTitulos.SubItems.Add("Sintácticos");
                            listaTitulos.SubItems.Add("Falta abrir llave.");
                            listViewError.Items.Add(listaTitulos).BackColor = Color.FromArgb(247, 75, 64); //si el contador es <0, mostrar mensaje
                            error = false;
                        }
                    }
                }
                if (s.Contains(')'))
                {
                    foreach (Char c in s)
                    {
                        if (parentesis.Count > 0)
                        {
                            if (c == ')')
                            {
                                parentesis.Pop();
                            }
                        }
                        else if (c == ')' && parentesis.Count == 0)
                        {
                            ListViewItem listaTitulos = new ListViewItem(lineaP.ToString());
                            listaTitulos.SubItems.Add("Sintácticos");
                            listaTitulos.SubItems.Add("Falta abrir parentesis.");
                            listViewError.Items.Add(listaTitulos).BackColor = Color.FromArgb(247, 75, 64); //si el contador es <0, mostrar mensaje
                            error = false;
                        }
                    }
                }
                linea++;
                lineaP++;
            }
            int i = 0;
            int k = 0;
            while (llaves.Count > 0)
            {
                i = llaves.Pop();
                ListViewItem listaTitulos = new ListViewItem(i.ToString());
                listaTitulos.SubItems.Add("Sintácticos");
                listaTitulos.SubItems.Add("Falta cerrar llave.");
                listViewError.Items.Add(listaTitulos).BackColor = Color.FromArgb(247, 75, 64);  // Si el contador es < 0, mostrar mensaje    
                error = false;
            }
            while (parentesis.Count > 0)
            {
                k = parentesis.Pop();
                ListViewItem listaTitulos = new ListViewItem(k.ToString());
                listaTitulos.SubItems.Add("Sintácticos");
                listaTitulos.SubItems.Add("Falta cerrar parentesis.");
                listViewError.Items.Add(listaTitulos).BackColor = Color.FromArgb(247, 75, 64);  // Si el contador es < 0, mostrar mensaje    
                error = false;
            }
            return error;
        }

        /// <summary>
        /// MÉTODO PARA LLENAR UNA PILA CON LOS OPERADORES
        /// </summary>
        /// <param name="linea">PARAMETRO QUE RECIBE LA LÍNEA</param>
        /// <param name="texto">PARAMETRO QUE RECIBE UNA CADENA</param>
        private void llenarPilaOperandos(int linea, string texto)
        {
            string valorPila = PasarStringPosfijo(texto);
            string[] cont = valorPila.Split(' ');
            string[] numero = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            pilaOp.Clear();
            pilaOpInv.Clear();

            for (int i = 0; i < cont.Length; i++)
            {
                pilaOp.Push(cont[i]);
                //pilaOpInv.Push(pilaOp.Pop());
                if (pilaOp.Peek() == "")
                {
                    pilaOp.Pop();
                }
            }
            while (pilaOp.Count != 0)
            {
                pilaOpInv.Push(pilaOp.Pop());
            }
            verificarExp(linea);
        }

        private void condicional()
        {
            foreach (String s in contenido)
            {
                if (s.Contains("int"))
                {

                }
            }
        }
        /// <summary>
        /// MÉTODO PARA VERIFICAR UNA EXPRESIÓN EN UNA LÍNEA DEL CÓDIGO Y MOSTRAR LOS ERRORES SINTÁCTICOS
        /// </summary>
        /// <param name="linea">PARAMETRO QUE RECIBE LA LÍNEA DEL CÓDIGO</param>
        private void verificarExp(int linea)
        {
            // listViewCuadruplos.Items.Clear();

            int a, b = 0;
            int[] valor = new int[1];
            bool comp = false;
            Stack pila2 = new Stack();

            while (pilaOpInv.Count != 0)
            {
                string elemento = pilaOpInv.Pop().ToString();
                pilaOp.Push(elemento);
                if (elemento.All(char.IsDigit))
                {
                    pila2.Push(elemento);
                }
                else if (operador.Contains(elemento) && pila2.Count == 2)
                {
                    if (masexpresiones == true)
                    {
                        if (lblFalso.Text != "")
                        {
                            listViewCuadruplos.Items.RemoveAt(listViewCuadruplos.Items.Count - 1);
                            masexpresiones = false;
                            inicioCicloElse = listViewCuadruplos.Items.Count;
                        }
                        else if(lblFalso.Text == "") { 
                        listViewCuadruplos.Items.RemoveAt(listViewCuadruplos.Items.Count-1);
                        listViewCuadruplos.Items.RemoveAt(listViewCuadruplos.Items.Count-1);
                        listViewCuadruplos.Items.RemoveAt(listViewCuadruplos.Items.Count-1);
                        listViewCuadruplos.Items.RemoveAt(listViewCuadruplos.Items.Count-1);
                        conteo -= 3;
                        inicioCiclo = listViewCuadruplos.Items.Count;
                        masexpresiones = false;
                        }
                    }
                    switch (elemento)
                    {
                        case "+":
                            {
                                //int val1 = int.Parse(pila2.Pop().ToString()) + int.Parse(pila2.Pop().ToString());
                                //pila2.Push(val1.ToString());
                                //listViewCuadruplos.Items.Add("asd");

                                int arg2 = int.Parse(pila2.Pop().ToString());
                                ListViewItem listaCuadruplos = new ListViewItem("+"); //operador
                                listaCuadruplos.SubItems.Add(pila2.Peek().ToString()); //arg1
                                listaCuadruplos.SubItems.Add(arg2.ToString());   //arg2



                                int val1 = arg2 + int.Parse(pila2.Pop().ToString());
                                listaCuadruplos.SubItems.Add(val1.ToString());//result temporal 
                                listViewCuadruplos.Items.Add(listaCuadruplos);
                                pila2.Push(val1.ToString());

                                //listViewEnsamblador.Items.Add("MOV CX, " + arg2.ToString());
                               
                            }
                            break;

                        case "-":
                            {
                                //int val1 = int.Parse(pila2.Pop().ToString()) - int.Parse(pila2.Pop().ToString());
                                //pila2.Push(val1.ToString());
                                int arg2 = int.Parse(pila2.Pop().ToString());
                                ListViewItem listaCuadruplos = new ListViewItem("-"); //operador
                                listaCuadruplos.SubItems.Add(pila2.Peek().ToString()); //arg1
                                listaCuadruplos.SubItems.Add(arg2.ToString());   //arg2



                                int val1 = arg2 - int.Parse(pila2.Pop().ToString());
                                listaCuadruplos.SubItems.Add(val1.ToString());//result temporal 
                                listViewCuadruplos.Items.Add(listaCuadruplos);
                                pila2.Push(val1.ToString());
                            }
                            break;

                        case "*":
                            {
                                //int val1 = int.Parse(pila2.Pop().ToString()) * int.Parse(pila2.Pop().ToString());
                                //pila2.Push(val1.ToString());
                                int arg2 = int.Parse(pila2.Pop().ToString());
                                ListViewItem listaCuadruplos = new ListViewItem("*"); //operador
                                listaCuadruplos.SubItems.Add(pila2.Peek().ToString()); //arg1
                                listaCuadruplos.SubItems.Add(arg2.ToString());   //arg2



                                int val1 = arg2 * int.Parse(pila2.Pop().ToString());
                                listaCuadruplos.SubItems.Add(val1.ToString());//result temporal 
                                listViewCuadruplos.Items.Add(listaCuadruplos);
                                pila2.Push(val1.ToString());
                            }
                            break;

                        case "/":
                            {
                                //int val1 = int.Parse(pila2.Pop().ToString()) / int.Parse(pila2.Pop().ToString());
                                //pila2.Push(val1.ToString());
                                int arg2 = int.Parse(pila2.Pop().ToString());
                                ListViewItem listaCuadruplos = new ListViewItem("/"); //operador
                                listaCuadruplos.SubItems.Add(pila2.Peek().ToString()); //arg1
                                listaCuadruplos.SubItems.Add(arg2.ToString());   //arg2



                                int val1 = arg2 / int.Parse(pila2.Pop().ToString());
                                listaCuadruplos.SubItems.Add(val1.ToString());//result temporal 
                                listViewCuadruplos.Items.Add(listaCuadruplos);
                                pila2.Push(val1.ToString());
                            }
                            break;

                        default:
                            MessageBox.Show("Operador no identificado");
                            break;
                    }
                }
                else
                {
                    ListViewItem error = new ListViewItem(linea.ToString());
                    error.SubItems.Add("Sintáctico");
                    error.SubItems.Add("Expresion incorrecta: verifica que los operadores y operandos sean correctos.");
                    listViewError.Items.Add(error).BackColor = Color.FromArgb(247, 75, 64);
                }

            }
            int reng = 0;
            int i = 2;
            string[] s = nom.Split(' ');

            if(tabCuadruplo == true) { 
            for (int k = 0; k < listViewCuadruplos.Items.Count; k++)
            {
                reng++;
            }
            if(lblFalso.Text == "") {
                    
            for (int k = inicioCiclo; k < listViewCuadruplos.Items.Count; k++)
            {
                if(k+1 < reng && lblFalso.Text == "") {
                if (listViewCuadruplos.Items[k].SubItems[3].Text == listViewCuadruplos.Items[k + 1].SubItems[1].Text)
                {
                    listViewCuadruplos.Items[k].SubItems[3].Text = "temp" + i;
                    listViewCuadruplos.Items[k+1].SubItems[1].Text = "temp" + i;
                    conteo++;
                }
                else if (listViewToken.Items[k].SubItems[3].Text == listViewToken.Items[k + 1].SubItems[2].Text)
                {

                    listViewCuadruplos.Items[k].SubItems[3].Text = "temp" + i;
                    listViewCuadruplos.Items[k+1].SubItems[2].Text = "temp" + i;
                    conteo++;
                            }
                    i++;
                }
                if(k == reng-1)
                {
                    listViewCuadruplos.Items[k].SubItems[3].Text = "temp" + i;
                    
                    ListViewItem listaCuadruplosFinal = new ListViewItem("="); //operador
                    listaCuadruplosFinal.SubItems.Add("temp" + i); //arg1
                    listaCuadruplosFinal.SubItems.Add(" ");   //arg2
                    listaCuadruplosFinal.SubItems.Add(s[1]);   //arg2
                    listViewCuadruplos.Items.Add(listaCuadruplosFinal);
                    conteo++;
                }
            }
                    listViewCuadruplos.Items.Add("goto finif");
                    listViewCuadruplos.Items.Add("else").BackColor = Color.Orange;
                    ListViewItem lbl = new ListViewItem("CUADRUPLO"); //operador
                    lbl.SubItems.Add("DE LA PARTE"); //arg1
                    lbl.SubItems.Add("FALSA"); //arg2
                    listViewCuadruplos.Items.Add(lbl).BackColor = Color.FromArgb(255, 85, 85);
                    //listViewCuadruplos.Items.Add(new ListViewItem("CUADRUPLO DE LA PARTE FALSA", listGrupo));
                   
                    listViewCuadruplos.Items.Add("finif").BackColor = Color.Orange;
                    masexpresiones = true;
                    conteo += 4;
                }
                if (lblFalso.Text != "")
                {
                    for (int k = inicioCicloElse; k < listViewCuadruplos.Items.Count; k++)
                    {
                        if (k + 1 < reng && lblFalso.Text != "")
                        {
                            if (listViewCuadruplos.Items[k].SubItems[3].Text == listViewCuadruplos.Items[k + 1].SubItems[1].Text)
                            {
                                listViewCuadruplos.Items[k].SubItems[3].Text = "temp" + i;
                                listViewCuadruplos.Items[k + 1].SubItems[1].Text = "temp" + i;
                                //i++;
                            }
                            else if (listViewToken.Items[k].SubItems[3].Text == listViewToken.Items[k + 1].SubItems[2].Text)
                            {

                                listViewCuadruplos.Items[k].SubItems[3].Text = "temp" + i;
                                listViewCuadruplos.Items[k + 1].SubItems[2].Text = "temp" + i;
                                //i++;
                            }
                            i++;
                        }
                        if (k == reng - 1)
                        {
                            listViewCuadruplos.Items[k].SubItems[3].Text = "temp" + i;

                            ListViewItem listaCuadruplosFinal = new ListViewItem("="); //operador
                            listaCuadruplosFinal.SubItems.Add("temp" + i); //arg1
                            listaCuadruplosFinal.SubItems.Add(" ");   //arg2
                            listaCuadruplosFinal.SubItems.Add(s[1]);   //arg2
                            listViewCuadruplos.Items.Add(listaCuadruplosFinal);
                        }
                    }
                    listViewCuadruplos.Items.Add("finelse").BackColor = Color.Orange;
                    masexpresiones = true;
                }
            }
           

            if (pila2.Count == 1)
            {

            }
            else if (pila2.Count > 1)
            {
                ListViewItem error = new ListViewItem(linea.ToString());
                error.SubItems.Add("Sintáctico");
                error.SubItems.Add("Expresion incorrecta: verifica que los operadores y operandos sean correctos.");
                listViewError.Items.Add(error).BackColor = Color.FromArgb(247, 75, 64);
            }
        }

        /// <summary>
        /// MÉTODO PARA COMPROBAR UNA EXPRESIÓN ALGEBRAICA
        /// </summary>
        public void comprobarExpresionAlgebraica()
        {
            Regex rgxOpAlgebraicas = new Regex(@"[0-9]{ 1} ([+] |[-] |[\/] |[*])[0 - 9]{ 1}[,]*");
            int linea = 0;
            foreach (String s in contenido)
            {
                linea++;
                int tam = 0;
                int d = 0;
                string ultElem = "";
                string el = "";
                bool control = false;

                if (s.Contains("="))
                {
                    for (int q = 0; q < s.Length; q++)
                    {
                        if (s[q] == ' ')
                            tam++;
                    }
                    string[] arrReservadas = new string[tam + 2];
                    for (int q = 0; q < s.Length; q++)
                    {
                        if (control == true)
                        {
                            if (s[q] == ' ')
                            {
                                arrReservadas[d] = el;
                                el = "";
                                d++;
                                float h = 10;
                            }
                        }
                        if (s[q] == ' ' && el == "int" | el == "double" | el == "float" | el == "char" | el == "string")
                        {
                            control = true;
                            arrReservadas[d] = el;
                            el = "";
                            d++;
                        }
                        else
                        {
                            el += s[q];
                            if (el.Contains(";"))
                            {
                                q = s.Length - 1;
                            }
                            if (s[q] == ' ')
                            {
                                el = "";
                            }
                        }
                    }
                    arrReservadas[d] = el;
                    bool t = false;
                    string exp = "";
                    for (int a = 0; a < arrReservadas.Length; a++)
                    {
                        if (arrReservadas[a] == "=")
                        {
                            t = true;
                        }
                        if (t == true)
                        {
                            if (arrReservadas[a] != " ")
                            {
                                exp += arrReservadas[a];
                                exp += " ";
                            }
                        }
                    }
                    arrReservadas[arrReservadas.Length - 1] = exp;
                    string expresionFinal = arrReservadas[arrReservadas.Length - 1];
                    char[] arrExp = expresionFinal.ToCharArray();
                    expresionFinal = "";
                    for (int c = 0; c < arrExp.Length; c++)
                    {
                        if (c > 0)
                        {
                            if (c < arrExp.Length - 1)
                                expresionFinal += arrExp[c];
                        }
                    }
                    if (expresionFinal.Contains("+") | expresionFinal.Contains("*") | expresionFinal.Contains("/") | expresionFinal.Contains("-") | expresionFinal.Contains("^"))
                    {
                        llenarPilaOperandos(linea, expresionFinal);
                    }
                }

            }

        }
        #endregion

        #endregion

        #region GENERACION DE CUADRUPLOS

        /// <summary>
        /// MÉTODO PARA COMPROBAR EXPRESIONES ALGEBRAICAS PERO GENERANDO CUADRUPLOS
        /// </summary>
        public void comprobarCuadruplos()
        {
            
            listViewCuadruplos.Items.Clear();
            ListViewItem listaCuadruplos2 = new ListViewItem("Condicion if");//operador
            listaCuadruplos2.SubItems[0].BackColor = Color.Orange;
            listViewCuadruplos.Items.Add(listaCuadruplos2);
            Regex rgxOpAlgebraicas = new Regex(@"[0-9]{ 1} ([+] |[-] |[\/] |[*])[0 - 9]{ 1}[,]*");
            int linea = 0;
            int ads = 0;
            bool condicion = false;
            string f = "";
            string nombreVar = "";
            string expresionFinal = "";
            bool masExpresion = false;
            bool parentesis = false;
            //String s = "";

            //int index = listBoxLenguaje.SelectedIndex;
            if (radioBtnJava.Checked == true)
            {
                //SI ELIGEN EL IDIOMA DE JAVA QUE SE REALICE LO SIGUIENTE:
                foreach (String s in contenido)
                {

                    nom = s;
                    linea++;
                    int tam = 0;
                    int d = 0;
                    string ultElem = "";
                    string el = "";
                    bool control = false;
                    
                    if (s.Contains("else"))
                    {
                        if (expresionFinal != "")
                            condicion = false;
                        else
                            condicion = true;
                    }
                    if (s.Contains("if"))
                    {
                        if (s.Contains("{"))
                        {
                            masExpresion = true;
                            for(int x = 0; x< contenido.Count; x++)
                            {
                                if (contenido[x].Contains("else") && contenido[x].Contains("}"))
                                {

                                }else if (contenido[x].Contains("else"))
                                {
                                    errorLlave = true;
                                }
                            }
                        }
                        else
                        {
                            masExpresion = false;
                        }
                        for (int q = 0; q < s.Length; q++)
                        {
                            if (s[q] == ' ')
                                tam++;
                        }
                        string[] arrReservadas = new string[tam + 2];
                        for (int q = 0; q < s.Length; q++)
                        {
                            if (control == false)
                            {
                                if (s[q] == ' ')
                                {
                                    arrReservadas[d] = el;
                                    el = "";
                                    d++;
                                    float h = 10;
                                }
                            }
                            if (s[q] == ' ' && el == "int" | el == "double" | el == "float" | el == "char" | el == "string")
                            {
                                control = true;
                                arrReservadas[d] = el;
                                el = "";
                                d++;
                            }
                            else
                            {
                                el += s[q];
                                if (el.Contains(";"))
                                {
                                    q = s.Length - 1;
                                }
                                if (s[q] == ' ')
                                {
                                    el = "";
                                }
                            }
                        }
                        arrReservadas[d] = el;
                        if (f != "")
                        {
                             a = f.Replace(";", "");
                            switch (arrReservadas[3])
                            {
                                case "<=":
                                    {
                                        if (int.Parse(a) <= int.Parse(arrReservadas[4]))
                                        {
                                            ListViewItem listaCuadruplos = new ListViewItem("<="); //operador
                                            listaCuadruplos.SubItems.Add(nombreVar); //arg1
                                            listaCuadruplos.SubItems.Add(arrReservadas[4]);   //arg2
                                            listaCuadruplos.SubItems.Add("t1");   //arg2

                                            listViewCuadruplos.Items.Add(listaCuadruplos).BackColor = Color.Cyan;
                                            condicion = true;

                                            ListViewItem parteVerdadera = new ListViewItem("IfFalse"); //operador
                                            parteVerdadera.SubItems.Add("t1"); //arg1
                                            parteVerdadera.SubItems.Add(""); //arg2
                                            parteVerdadera.SubItems.Add("goto if"); //resutado
                                            listViewCuadruplos.Items.Add(parteVerdadera);   //arg2

                                            listViewCuadruplos.Items.Add("if").BackColor = Color.Orange;   //arg2
                                            listViewCuadruplos.Items.Add("").BackColor = Color.LightGreen;
                                            lblTrue.Visible = true;
                                            lblTrue.Text = "CUADRUPLO DE LA PARTE VERDADERA";
                                            lblTrue.BackColor = Color.LightGreen;

                                            listViewCuadruplos.Items.Add(parteVerdadera);
                                            
                                        }
                                        else
                                        {
                                            lblTrue.Visible = false;
                                            ListViewItem listaCuadruplos = new ListViewItem("<="); //operador
                                            listaCuadruplos.SubItems.Add(nombreVar); //arg1
                                            listaCuadruplos.SubItems.Add(arrReservadas[4]);   //arg2
                                            listaCuadruplos.SubItems.Add("t1");   //arg2

                                            listViewCuadruplos.Items.Add(listaCuadruplos).BackColor = Color.Cyan;
                                            ListViewItem parteVerdadera = new ListViewItem("IfFalse"); //operador
                                            parteVerdadera.SubItems.Add("t1"); //arg1
                                            parteVerdadera.SubItems.Add(""); //arg2
                                            parteVerdadera.SubItems.Add("goto else"); //resutado
                                            listViewCuadruplos.Items.Add(parteVerdadera);   //arg2

                                            listViewCuadruplos.Items.Add("").BackColor = Color.LightGreen; ;   //arg2
                                            lblVerdadero.Text = "CUADRUPLO DE LA PARTE VERDADERA";
                                            lblVerdadero.BackColor = Color.LightGreen;
                                            listViewCuadruplos.Items.Add("else").BackColor = Color.Orange;
                                            listViewCuadruplos.Items.Add("").BackColor = Color.FromArgb(255, 85, 85);
                                            lblFalso.Text = "CUADRUPLO DE LA PARTE FALSA";
                                            lblFalso.BackColor = Color.FromArgb(255, 85, 85);
                                        }
                                    }
                                    break;
                                case ">=":
                                    {
                                        if (int.Parse(a) >= int.Parse(arrReservadas[4]))
                                        {
                                            ListViewItem listaCuadruplos = new ListViewItem(">="); //operador
                                            listaCuadruplos.SubItems.Add(nombreVar); //arg1
                                            listaCuadruplos.SubItems.Add(arrReservadas[4]);   //arg2
                                            listaCuadruplos.SubItems.Add("t1");   //arg2

                                            listViewCuadruplos.Items.Add(listaCuadruplos).BackColor = Color.Cyan;
                                            condicion = true;

                                            ListViewItem parteVerdadera = new ListViewItem("IfFalse"); //operador
                                            parteVerdadera.SubItems.Add("t1"); //arg1
                                            parteVerdadera.SubItems.Add(""); //arg2
                                            parteVerdadera.SubItems.Add("goto if"); //resutado
                                            listViewCuadruplos.Items.Add(parteVerdadera);   //arg2

                                            listViewCuadruplos.Items.Add("if").BackColor = Color.Orange;   //arg2
                                            listViewCuadruplos.Items.Add("").BackColor = Color.LightGreen; ;
                                            lblTrue.Visible = true;
                                            lblTrue.Text = "CUADRUPLO DE LA PARTE VERDADERA";
                                            lblTrue.BackColor = Color.LightGreen;
                                        }
                                        else
                                        {
                                            lblTrue.Visible = false;
                                            ListViewItem listaCuadruplos = new ListViewItem(">="); //operador
                                            listaCuadruplos.SubItems.Add(nombreVar); //arg1
                                            listaCuadruplos.SubItems.Add(arrReservadas[4]);   //arg2
                                            listaCuadruplos.SubItems.Add("t1");   //arg2

                                            listViewCuadruplos.Items.Add(listaCuadruplos).BackColor = Color.Cyan;
                                            ListViewItem parteVerdadera = new ListViewItem("IfFalse"); //operador
                                            parteVerdadera.SubItems.Add("t1"); //arg1
                                            parteVerdadera.SubItems.Add(""); //arg2
                                            parteVerdadera.SubItems.Add("goto else"); //resutado
                                            listViewCuadruplos.Items.Add(parteVerdadera);   //arg2

                                            listViewCuadruplos.Items.Add("").BackColor = Color.LightGreen; ;   //arg2
                                            lblVerdadero.Text = "CUADRUPLO DE LA PARTE VERDADERA";
                                            lblVerdadero.BackColor = Color.LightGreen;
                                            listViewCuadruplos.Items.Add("else").BackColor = Color.Orange;
                                            listViewCuadruplos.Items.Add("").BackColor = Color.FromArgb(255, 85, 85);
                                            lblFalso.Text = "CUADRUPLO DE LA PARTE FALSA";
                                            lblFalso.BackColor = Color.FromArgb(255, 85, 85);
                                        }
                                    }
                                    break;
                                case "<":
                                    {
                                        if (int.Parse(a) < int.Parse(arrReservadas[4]))
                                        {
                                            ListViewItem listaCuadruplos = new ListViewItem("<"); //operador
                                            listaCuadruplos.SubItems.Add(nombreVar); //arg1
                                            listaCuadruplos.SubItems.Add(arrReservadas[4]);   //arg2
                                            listaCuadruplos.SubItems.Add("t1");   //arg2

                                            listViewCuadruplos.Items.Add(listaCuadruplos).BackColor = Color.Cyan;
                                            condicion = true;

                                            ListViewItem parteVerdadera = new ListViewItem("IfFalse"); //operador
                                            parteVerdadera.SubItems.Add("t1"); //arg1
                                            parteVerdadera.SubItems.Add(""); //arg2
                                            parteVerdadera.SubItems.Add("goto if"); //resutado
                                            listViewCuadruplos.Items.Add(parteVerdadera);   //arg2

                                            listViewCuadruplos.Items.Add("if").BackColor = Color.Orange;   //arg2
                                            listViewCuadruplos.Items.Add("").BackColor = Color.LightGreen; ;
                                            lblTrue.Visible = true;
                                            lblTrue.Text = "CUADRUPLO DE LA PARTE VERDADERA";
                                            lblTrue.BackColor = Color.LightGreen;
                                        }
                                        else
                                        {
                                            lblTrue.Visible = false;
                                            ListViewItem listaCuadruplos = new ListViewItem("<"); //operador
                                            listaCuadruplos.SubItems.Add(nombreVar); //arg1
                                            listaCuadruplos.SubItems.Add(arrReservadas[4]);   //arg2
                                            listaCuadruplos.SubItems.Add("t1");   //arg2

                                            listViewCuadruplos.Items.Add(listaCuadruplos).BackColor = Color.Cyan;
                                            ListViewItem parteVerdadera = new ListViewItem("IfFalse"); //operador
                                            parteVerdadera.SubItems.Add("t1"); //arg1
                                            parteVerdadera.SubItems.Add(""); //arg2
                                            parteVerdadera.SubItems.Add("goto else"); //resutado
                                            listViewCuadruplos.Items.Add(parteVerdadera);   //arg2

                                            listViewCuadruplos.Items.Add("").BackColor = Color.LightGreen; ;   //arg2
                                            lblVerdadero.Text = "CUADRUPLO DE LA PARTE VERDADERA";
                                            lblVerdadero.BackColor = Color.LightGreen;
                                            listViewCuadruplos.Items.Add("else").BackColor = Color.Orange;
                                            listViewCuadruplos.Items.Add("").BackColor = Color.FromArgb(255, 85, 85);
                                            lblFalso.Text = "CUADRUPLO DE LA PARTE FALSA";
                                            lblFalso.BackColor = Color.FromArgb(255, 85, 85);
                                        }
                                    }
                                    break;
                                case ">":
                                    {
                                        if (int.Parse(a) > int.Parse(arrReservadas[4]))
                                        {
                                            ListViewItem listaCuadruplos = new ListViewItem(">"); //operador
                                            listaCuadruplos.SubItems.Add(nombreVar); //arg1
                                            listaCuadruplos.SubItems.Add(arrReservadas[4]);   //arg2
                                            listaCuadruplos.SubItems.Add("t1");   //arg2

                                            listViewCuadruplos.Items.Add(listaCuadruplos).BackColor = Color.Cyan;
                                            condicion = true;

                                            ListViewItem parteVerdadera = new ListViewItem("IfFalse"); //operador
                                            parteVerdadera.SubItems.Add("t1"); //arg1
                                            parteVerdadera.SubItems.Add(""); //arg2
                                            parteVerdadera.SubItems.Add("goto if"); //resutado
                                            listViewCuadruplos.Items.Add(parteVerdadera);   //arg2

                                            listViewCuadruplos.Items.Add("if").BackColor = Color.Orange;   //arg2
                                            listViewCuadruplos.Items.Add("").BackColor = Color.LightGreen; ;
                                            lblTrue.Visible = true;
                                            lblTrue.Text = "CUADRUPLO DE LA PARTE VERDADERA";
                                            lblTrue.BackColor = Color.LightGreen;
                                        }
                                        else
                                        {
                                            lblTrue.Visible = false;
                                            ListViewItem listaCuadruplos = new ListViewItem(">"); //operador
                                            listaCuadruplos.SubItems.Add(nombreVar); //arg1
                                            listaCuadruplos.SubItems.Add(arrReservadas[4]);   //arg2
                                            listaCuadruplos.SubItems.Add("t1");   //arg2

                                            listViewCuadruplos.Items.Add(listaCuadruplos).BackColor = Color.Cyan;
                                            ListViewItem parteVerdadera = new ListViewItem("IfFalse"); //operador
                                            parteVerdadera.SubItems.Add("t1"); //arg1
                                            parteVerdadera.SubItems.Add(""); //arg2
                                            parteVerdadera.SubItems.Add("goto else"); //resutado
                                            listViewCuadruplos.Items.Add(parteVerdadera);   //arg2

                                            listViewCuadruplos.Items.Add("").BackColor = Color.LightGreen; ;   //arg2
                                            lblVerdadero.Text = "CUADRUPLO DE LA PARTE VERDADERA";
                                            lblVerdadero.BackColor = Color.LightGreen;
                                            listViewCuadruplos.Items.Add("else").BackColor = Color.Orange;
                                            listViewCuadruplos.Items.Add("").BackColor = Color.FromArgb(255, 85, 85);
                                            lblFalso.Text = "CUADRUPLO DE LA PARTE FALSA";
                                            lblFalso.BackColor = Color.FromArgb(255, 85, 85);
                                        }
                                    }
                                    break;
                                case "==":
                                    {
                                        if (int.Parse(a) == int.Parse(arrReservadas[4]))
                                        {
                                            ListViewItem listaCuadruplos = new ListViewItem("=="); //operador
                                            listaCuadruplos.SubItems.Add(nombreVar); //arg1
                                            listaCuadruplos.SubItems.Add(arrReservadas[4]);   //arg2
                                            listaCuadruplos.SubItems.Add("t1");   //arg2

                                            listViewCuadruplos.Items.Add(listaCuadruplos).BackColor = Color.Cyan;
                                            condicion = true;

                                            ListViewItem parteVerdadera = new ListViewItem("IfFalse"); //operador
                                            parteVerdadera.SubItems.Add("t1"); //arg1
                                            parteVerdadera.SubItems.Add(""); //arg2
                                            parteVerdadera.SubItems.Add("goto if"); //resutado
                                            listViewCuadruplos.Items.Add(parteVerdadera);   //arg2

                                            listViewCuadruplos.Items.Add("if").BackColor = Color.Orange;   //arg2
                                            listViewCuadruplos.Items.Add("").BackColor = Color.LightGreen; ;
                                            lblTrue.Visible = true;
                                            lblTrue.Text = "CUADRUPLO DE LA PARTE VERDADERA";
                                            lblTrue.BackColor = Color.LightGreen;
                                        }
                                        else
                                        {
                                            lblTrue.Visible = false;
                                            ListViewItem listaCuadruplos = new ListViewItem("=="); //operador
                                            listaCuadruplos.SubItems.Add(nombreVar); //arg1
                                            listaCuadruplos.SubItems.Add(arrReservadas[4]);   //arg2
                                            listaCuadruplos.SubItems.Add("t1");   //arg2

                                            listViewCuadruplos.Items.Add(listaCuadruplos).BackColor = Color.Cyan;
                                            ListViewItem parteVerdadera = new ListViewItem("IfFalse"); //operador
                                            parteVerdadera.SubItems.Add("t1"); //arg1
                                            parteVerdadera.SubItems.Add(""); //arg2
                                            parteVerdadera.SubItems.Add("goto else"); //resutado
                                            listViewCuadruplos.Items.Add(parteVerdadera);   //arg2

                                            listViewCuadruplos.Items.Add("").BackColor = Color.LightGreen; ;   //arg2
                                            lblVerdadero.Text = "CUADRUPLO DE LA PARTE VERDADERA";
                                            lblVerdadero.BackColor = Color.LightGreen;
                                            listViewCuadruplos.Items.Add("else").BackColor = Color.Orange;
                                            listViewCuadruplos.Items.Add("").BackColor = Color.FromArgb(255, 85, 85);
                                            lblFalso.Text = "CUADRUPLO DE LA PARTE FALSA";
                                            lblFalso.BackColor = Color.FromArgb(255, 85, 85);
                                        }
                                    }
                                    break;
                                case "!=":
                                    {
                                        if (int.Parse(a) != int.Parse(arrReservadas[4]))
                                        {
                                            ListViewItem listaCuadruplos = new ListViewItem("!="); //operador
                                            listaCuadruplos.SubItems.Add(nombreVar); //arg1
                                            listaCuadruplos.SubItems.Add(arrReservadas[4]);   //arg2
                                            listaCuadruplos.SubItems.Add("t1");   //arg2

                                            listViewCuadruplos.Items.Add(listaCuadruplos).BackColor = Color.Cyan;
                                            condicion = true;

                                            ListViewItem parteVerdadera = new ListViewItem("IfFalse"); //operador
                                            parteVerdadera.SubItems.Add("t1"); //arg1
                                            parteVerdadera.SubItems.Add(""); //arg2
                                            parteVerdadera.SubItems.Add("goto if"); //resutado
                                            listViewCuadruplos.Items.Add(parteVerdadera);   //arg2

                                            listViewCuadruplos.Items.Add("if").BackColor = Color.Orange;   //arg2
                                            listViewCuadruplos.Items.Add("").BackColor = Color.LightGreen; ;
                                            lblTrue.Visible = true;
                                            lblTrue.Text = "CUADRUPLO DE LA PARTE VERDADERA";
                                            lblTrue.BackColor = Color.LightGreen;
                                        }
                                        else
                                        {
                                            lblTrue.Visible = false;
                                            ListViewItem listaCuadruplos = new ListViewItem("!="); //operador
                                            listaCuadruplos.SubItems.Add(nombreVar); //arg1
                                            listaCuadruplos.SubItems.Add(arrReservadas[4]);   //arg2
                                            listaCuadruplos.SubItems.Add("t1");   //arg2

                                            listViewCuadruplos.Items.Add(listaCuadruplos).BackColor = Color.Cyan;
                                            ListViewItem parteVerdadera = new ListViewItem("IfFalse"); //operador
                                            parteVerdadera.SubItems.Add("t1"); //arg1
                                            parteVerdadera.SubItems.Add(""); //arg2
                                            parteVerdadera.SubItems.Add("goto else"); //resutado
                                            listViewCuadruplos.Items.Add(parteVerdadera);   //arg2

                                            listViewCuadruplos.Items.Add("").BackColor = Color.LightGreen; ;   //arg2
                                            lblVerdadero.Text = "CUADRUPLO DE LA PARTE VERDADERA";
                                            lblVerdadero.BackColor = Color.LightGreen;
                                            listViewCuadruplos.Items.Add("else").BackColor = Color.Orange;
                                            listViewCuadruplos.Items.Add("").BackColor = Color.FromArgb(255, 85, 85);
                                            lblFalso.Text = "CUADRUPLO DE LA PARTE FALSA";
                                            lblFalso.BackColor = Color.FromArgb(255,85,85);
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    if (s.Contains("}") && s.Contains("else"))
                    {

                    }
                    else if (s.Contains("else") && contenido[ads-2].Contains("if") != true)
                    {
                        MessageBox.Show("FALTA CERRAR LLAVE");
                        listViewCuadruplos.Items.Clear();
                        masExpresion = false;
                        lblTrue.Text = "";
                        lblFalso.Text = "";
                        lblVerdadero.Text = "";
                    }
                    if (s.Contains("=") && s.Contains("int") | s.Contains("double") | s.Contains("char") | s.Contains("float") )
                    {

                        for (int q = 0; q < s.Length; q++)
                        {
                            if (s[q] == ' ')
                                tam++;
                        }
                        string[] arrReservadas = new string[tam + 2];
                        for (int q = 0; q < s.Length; q++)
                        {
                            if (control == true)
                            {
                                if (s[q] == ' ')
                                {
                                    arrReservadas[d] = el;
                                    el = "";
                                    d++;
                                    float h = 10;
                                }
                            }
                            if (s[q] == ' ' && el == "int" | el == "double" | el == "float" | el == "char" | el == "string")
                            {
                                control = true;
                                arrReservadas[d] = el;
                                el = "";
                                d++;
                            }
                            else
                            {
                                el += s[q];
                                if (el.Contains(";"))
                                {
                                    q = s.Length - 1;
                                }
                                if (s[q] == ' ')
                                {
                                    el = "";
                                }
                            }
                        }
                        arrReservadas[d] = el;

                        nombreVar = arrReservadas[1];
                        f = arrReservadas[3];
                        
                        if (f == "" | condicion == true)
                        {
                            //condicion = false;
                            if (masExpresion == true && expresionFinal != "" && parentesis == true)
                            {
                                MessageBox.Show("EXPRESION INCORRECTA");
                                listViewCuadruplos.Items.Clear();
                                lblVerdadero.Text = "";
                                lblVerdadero.Visible = false;
                                lblTrue.Text = "";
                                lblFalso.Text = "";
                            }
                            bool t = false;
                            string exp = "";
                            for (int a = 0; a < arrReservadas.Length; a++)
                            {
                                if (arrReservadas[a] == "=")
                                {
                                    t = true;
                                }
                                if (t == true)
                                {
                                    if (arrReservadas[a] != " ")
                                    {
                                        exp += arrReservadas[a];
                                        exp += " ";
                                    }
                                }
                            }
                            arrReservadas[arrReservadas.Length - 1] = exp;
                            expresionFinal = arrReservadas[arrReservadas.Length - 1];
                            char[] arrExp = expresionFinal.ToCharArray();
                            expresionFinal = "";
                            for (int c = 0; c < arrExp.Length; c++)
                            {
                                if (c > 0)
                                {
                                    if (c < arrExp.Length - 1)
                                        expresionFinal += arrExp[c];
                                }
                            }
                           if(masExpresion == false) { 
                            if (expresionFinal.Contains("+") | expresionFinal.Contains("*") | expresionFinal.Contains("/") | expresionFinal.Contains("-") | expresionFinal.Contains("^") && masExpresion == false)
                            {
                                llenarPilaOperandos(linea, expresionFinal);
                                if (masExpresion == false)
                                {
                                    masExpresion = true;
                                        parentesis = true;
                                }
                            }
                            }else if(masExpresion == true && listViewCuadruplos.Items.Count != 0)
                            {
                                if (expresionFinal.Contains("+") | expresionFinal.Contains("*") | expresionFinal.Contains("/") | expresionFinal.Contains("-") | expresionFinal.Contains("^") && masExpresion == true)
                                {
                                    llenarPilaOperandos(linea, expresionFinal);
                                    
                                }
                            }
                           
                            /*   if (expresionFinal.Contains("+") | expresionFinal.Contains("*") | expresionFinal.Contains("/") | expresionFinal.Contains("-") | expresionFinal.Contains("^") && parentesis == true)
                               {
                                   llenarPilaOperandos(linea, expresionFinal);
                               }*/
                        }
                    }
                    
                    ads++;
                }
            }
            else if (radioBtnBlackCat.Checked == true)
            {
                //SI ELIGEN EL IDIOMA DE BLACKCAT QUE SE REALICE LO SIGUIENTE:
                foreach (String s in contenido)
                {

                    nom = s;
                    linea++;
                    int tam = 0;
                    int d = 0;
                    string ultElem = "";
                    string el = "";
                    bool control = false;
                    if (s == "sino")
                    {
                        if (expresionFinal != "")
                            condicion = false;
                        else
                            condicion = true;
                    }
                    if (s.Contains("si "))
                    {
                        for (int q = 0; q < s.Length; q++)
                        {
                            if (s[q] == ' ')
                                tam++;
                        }
                        string[] arrReservadas = new string[tam + 2];
                        for (int q = 0; q < s.Length; q++)
                        {
                            if (control == false)
                            {
                                if (s[q] == ' ')
                                {
                                    arrReservadas[d] = el;
                                    el = "";
                                    d++;
                                    float h = 10;
                                }
                            }
                            if (s[q] == ' ' && el == "entero" | el == "doble" | el == "flotante" | el == "caracter" | el == "cadena")
                            {
                                control = true;
                                arrReservadas[d] = el;
                                el = "";
                                d++;
                            }
                            else
                            {
                                el += s[q];
                                if (el.Contains(";"))
                                {
                                    q = s.Length - 1;
                                }
                                if (s[q] == ' ')
                                {
                                    el = "";
                                }
                            }
                        }
                        arrReservadas[d] = el;
                        if (f != "")
                        {
                            string a = f.Replace(";", "");
                            switch (arrReservadas[3])
                            {
                                case "<=":
                                    {
                                        if (int.Parse(a) <= int.Parse(arrReservadas[4]))
                                        {
                                            condicion = true;
                                        }
                                    }
                                    break;
                                case ">=":
                                    {
                                        if (int.Parse(a) >= int.Parse(arrReservadas[4]))
                                        {
                                            condicion = true;
                                        }
                                    }
                                    break;
                                case "<":
                                    {
                                        if (int.Parse(a) < int.Parse(arrReservadas[4]))
                                        {
                                            condicion = true;
                                        }
                                    }
                                    break;
                                case ">":
                                    {
                                        if (int.Parse(a) > int.Parse(arrReservadas[4]))
                                        {
                                            condicion = true;
                                        }
                                    }
                                    break;
                                case "==":
                                    {
                                        if (int.Parse(a) == int.Parse(arrReservadas[4]))
                                        {
                                            condicion = true;
                                        }
                                    }
                                    break;
                                case "!=":
                                    {
                                        if (int.Parse(a) != int.Parse(arrReservadas[4]))
                                        {
                                            condicion = true;
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    if (s.Contains("=") && s.Contains("entero") | s.Contains("doble") | s.Contains("caracter") | s.Contains("flotante"))
                    {
                        for (int q = 0; q < s.Length; q++)
                        {
                            if (s[q] == ' ')
                                tam++;
                        }
                        string[] arrReservadas = new string[tam + 2];
                        for (int q = 0; q < s.Length; q++)
                        {
                            if (control == true)
                            {
                                if (s[q] == ' ')
                                {
                                    arrReservadas[d] = el;
                                    el = "";
                                    d++;
                                    float h = 10;
                                }
                            }
                            if (s[q] == ' ' && el == "entero" | el == "doble" | el == "flotante" | el == "caracter" | el == "cadena")
                            {
                                control = true;
                                arrReservadas[d] = el;
                                el = "";
                                d++;
                            }
                            else
                            {
                                el += s[q];
                                if (el.Contains(";"))
                                {
                                    q = s.Length - 1;
                                }
                                if (s[q] == ' ')
                                {
                                    el = "";
                                }
                            }
                        }
                        arrReservadas[d] = el;

                        f = arrReservadas[3];
                        if (f == "" | condicion == true)
                        {
                            condicion = false;
                            bool t = false;
                            string exp = "";
                            for (int a = 0; a < arrReservadas.Length; a++)
                            {
                                if (arrReservadas[a] == "=")
                                {
                                    t = true;
                                }
                                if (t == true)
                                {
                                    if (arrReservadas[a] != " ")
                                    {
                                        exp += arrReservadas[a];
                                        exp += " ";
                                    }
                                }
                            }
                            arrReservadas[arrReservadas.Length - 1] = exp;
                            expresionFinal = arrReservadas[arrReservadas.Length - 1];
                            char[] arrExp = expresionFinal.ToCharArray();
                            expresionFinal = "";
                            for (int c = 0; c < arrExp.Length; c++)
                            {
                                if (c > 0)
                                {
                                    if (c < arrExp.Length - 1)
                                        expresionFinal += arrExp[c];
                                }
                            }
                            if (expresionFinal.Contains("+") | expresionFinal.Contains("*") | expresionFinal.Contains("/") | expresionFinal.Contains("-") | expresionFinal.Contains("^"))
                            {
                                llenarPilaOperandos(linea, expresionFinal);
                            }
                        }
                    }
                }
            }
            else
            {

            }
            

        }


        #endregion

        #endregion

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Ensamblador_Click(object sender, EventArgs e)
        {
            listViewCuadruplos.Items.Clear();
            listViewEnsamblador.Items.Clear();
            btnCuadruplos_Click(sender,e);
            if (listViewCuadruplos.Items.Count != 0)
            {
                ListViewItem ensamblador = new ListViewItem();
                string ax = "MOV AX, ";
                string dx = "MOV DX, ";
                string cmp = "CMP DX, AX";
                listViewEnsamblador.Items.Add("-- RECUPERA LOS VALORES DE LAS VARIABLES HACIA LOS REGISTROS");
                listViewEnsamblador.Items.Add(ax + listViewCuadruplos.Items[1].SubItems[1].Text);
                listViewEnsamblador.Items.Add(" ");
                listViewEnsamblador.Items.Add("-- ASIGNA LOS VALORES A LAS VARIABLES");
                listViewEnsamblador.Items.Add(ax + a);
                listViewEnsamblador.Items.Add(dx + listViewCuadruplos.Items[1].SubItems[2].Text);
                listViewEnsamblador.Items.Add(" ");
                listViewEnsamblador.Items.Add("-- REALIZA LA COMPARACIÓN CON LOS VALORES DE LOS REGISTROS");
                listViewEnsamblador.Items.Add(cmp);
                listViewEnsamblador.Items.Add("JG lblTrue:");
                listViewEnsamblador.Items.Add("JMP lblFalse:");
                listViewEnsamblador.Items.Add(" ");

                int ren = 0;
                for (int a = 0; a <= listViewCuadruplos.Items.Count; a++)
                {
                    ren++;
                }
                    if (listViewCuadruplos.Items[2].SubItems[3].Text == "goto else")
                {
                    listViewEnsamblador.Items.Add("lblFalse");
                    for (int i= 0; i < ren; i++)
                    {
                        if (i + 1 < ren) { 
                            if (listViewCuadruplos.Items[i].SubItems[0].Text == "+" | listViewCuadruplos.Items[i].SubItems[0].Text == "/" | listViewCuadruplos.Items[i].SubItems[0].Text == "*" | listViewCuadruplos.Items[i].SubItems[0].Text == "-")
                        {
                            string valoor = listViewCuadruplos.Items[i].SubItems[0].Text;
                            switch (valoor)
                            {
                                case "+":
                                    {
                                        if (!listViewCuadruplos.Items[i].SubItems[1].Text.Contains("t"))
                                        {
                                            listViewEnsamblador.Items.Add("ADD CX, " + listViewCuadruplos.Items[i].SubItems[1].Text);
                                                if (!listViewCuadruplos.Items[i].SubItems[2].Text.Contains("t"))
                                                {
                                                    listViewEnsamblador.Items.Add("ADD CX, " + listViewCuadruplos.Items[i].SubItems[2].Text);
                                                }
                                        }
                                        else if (!listViewCuadruplos.Items[i].SubItems[2].Text.Contains("t"))
                                            {
                                                listViewEnsamblador.Items.Add("ADD CX, " + listViewCuadruplos.Items[i].SubItems[2].Text);
                                            }
                                    }
                                    break;

                                    case "-":
                                        {
                                            if (!listViewCuadruplos.Items[i].SubItems[1].Text.Contains("t"))
                                            {
                                                listViewEnsamblador.Items.Add("ADD CX, " + listViewCuadruplos.Items[i].SubItems[1].Text);
                                                if (!listViewCuadruplos.Items[i].SubItems[2].Text.Contains("t"))
                                                {
                                                    listViewEnsamblador.Items.Add("SUB CX, " + listViewCuadruplos.Items[i].SubItems[2].Text);
                                                }
                                            }
                                            else if (!listViewCuadruplos.Items[i].SubItems[2].Text.Contains("t"))
                                            {
                                                listViewEnsamblador.Items.Add("SUB CX, " + listViewCuadruplos.Items[i].SubItems[2].Text);
                                            }
                                        }
                                        break;
                                }
                        }
                        }
                    }
                    listViewEnsamblador.Items.Add("MOV DX, CX");
                    listViewEnsamblador.Items.Add("JMP labelFinElse");
                    listViewEnsamblador.Items.Add("");               
                    listViewEnsamblador.Items.Add("labelFinIf:");
                    listViewEnsamblador.Items.Add("");
                    listViewEnsamblador.Items.Add("-- ASIGNA LOS VALORES DE LOS REGISTROS A LAS VARIABLES");
                    listViewEnsamblador.Items.Add("labelFinElse");
                    listViewEnsamblador.Items.Add("MOV " + listViewCuadruplos.Items[1].SubItems[1].Text + " , DX");
                    listViewEnsamblador.Items.Add("");
                    listViewEnsamblador.Items.Add("-- RECUPERA LOS VALORES DE LOS REGISTROS EN LA PILA");
                    listViewEnsamblador.Items.Add("POP DX");
                    listViewEnsamblador.Items.Add("POP AX");
                    listViewEnsamblador.Items.Add("POP CX");
                }
                else if(listViewCuadruplos.Items[2].SubItems[3].Text == "goto if")
                {
                    listViewEnsamblador.Items.Add("lblIf");
                    for (int i = 0; i < ren; i++)
                    {
                        if (i + 1 < ren)
                        {
                            if (listViewCuadruplos.Items[i].SubItems[0].Text == "+" | listViewCuadruplos.Items[i].SubItems[0].Text == "/" | listViewCuadruplos.Items[i].SubItems[0].Text == "*" | listViewCuadruplos.Items[i].SubItems[0].Text == "-")
                            {
                                string valoor = listViewCuadruplos.Items[i].SubItems[0].Text;
                                switch (valoor)
                                {
                                    case "+":
                                        {
                                            if (!listViewCuadruplos.Items[i].SubItems[1].Text.Contains("t"))
                                            {
                                                listViewEnsamblador.Items.Add("ADD CX, " + listViewCuadruplos.Items[i].SubItems[1].Text);
                                                if (!listViewCuadruplos.Items[i].SubItems[2].Text.Contains("t"))
                                                {
                                                    listViewEnsamblador.Items.Add("ADD CX, " + listViewCuadruplos.Items[i].SubItems[2].Text);
                                                }
                                            }
                                            else if (!listViewCuadruplos.Items[i].SubItems[2].Text.Contains("t"))
                                            {
                                                listViewEnsamblador.Items.Add("ADD CX, " + listViewCuadruplos.Items[i].SubItems[2].Text);
                                            }
                                        }
                                        break;

                                    case "-":
                                        {
                                            if (!listViewCuadruplos.Items[i].SubItems[1].Text.Contains("t"))
                                            {
                                                listViewEnsamblador.Items.Add("ADD CX, " + listViewCuadruplos.Items[i].SubItems[1].Text);
                                                if (!listViewCuadruplos.Items[i].SubItems[2].Text.Contains("t"))
                                                {
                                                    listViewEnsamblador.Items.Add("SUB CX, " + listViewCuadruplos.Items[i].SubItems[2].Text);
                                                }
                                            }
                                            else if (!listViewCuadruplos.Items[i].SubItems[2].Text.Contains("t"))
                                            {
                                                listViewEnsamblador.Items.Add("SUB CX, " + listViewCuadruplos.Items[i].SubItems[2].Text);
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                    }
                    listViewEnsamblador.Items.Add("MOV AX, CX");
                    listViewEnsamblador.Items.Add("JMP labelFinIf");
                    listViewEnsamblador.Items.Add("");
                    
                    listViewEnsamblador.Items.Add("");
                    listViewEnsamblador.Items.Add("-- ASIGNA LOS VALORES DE LOS REGISTROS A LAS VARIABLES");
                    listViewEnsamblador.Items.Add("labelFinIf");
                    listViewEnsamblador.Items.Add("MOV " + listViewCuadruplos.Items[1].SubItems[1].Text + " , AX");
                    listViewEnsamblador.Items.Add("");
                    listViewEnsamblador.Items.Add("labelFinElse:");
                    listViewEnsamblador.Items.Add("");
                    listViewEnsamblador.Items.Add("-- RECUPERA LOS VALORES DE LOS REGISTROS EN LA PILA");
                    listViewEnsamblador.Items.Add("POP DX");
                    listViewEnsamblador.Items.Add("POP AX");
                    listViewEnsamblador.Items.Add("POP CX");
                }

            }
        }

        private void radioBtnBlackCat_CheckedChanged(object sender, EventArgs e)
        {
            if(radioBtnBlackCat.Checked == true)
            {
                txtLenguaje.Text =
                 "abstracto\r\n" + "como\r\n" + "asincrono\r\n" + "esperar\r\n" +
                 "comprobar\r\n" + "constante\r\n" + "seguir\r\n" + "defecto\r\n" + "delegar\r\n" + "base\r\n" + "romper\r\n" + "caso\r\n" +
                 "hacer\r\n" + "contrario\r\n" + "enumeracion\r\n" + "evento\r\n" + "explicito\r\n" + "externo\r\n" + "falso\r\n" + "finalmente\r\n" +
                 "reparar\r\n" + "por\r\n" + "porcada\r\n" + "ir\r\n" + "si\r\n" + "implicito\r\n" + "en\r\n" + "interfaz\r\n" +
                 "interno\r\n" + "es\r\n" + "cerar\r\n" + "nuevo\r\n" + "nulo\r\n" + "operador\r\n" + "captura\r\n" +
                 "fuera\r\n" + "anular\r\n" + "parametro\r\n" + "privado\r\n" + "protegido\r\n" + "publico\r\n" + "lectura\r\n" +
                 "arbitraria\r\n" + "regresa\r\n" + "sellado\r\n" + "tamaño\r\n" + "ampilar\r\n" + "estatico\r\n" +
                 "cambio\r\n" + "esto\r\n" + "lanzar\r\n" + "verdadero\r\n" + "tratar\r\n" + "tipo de\r\n" + "nombre\r\n" +
                 "desenfrenado\r\n" + "inseguro\r\n" + "virtual\r\n" + "vacio\r\n" + "mientras\r\n" + "flotador\r\n" + "entero\r\n" + "prolongar\r\n" + "objeto\r\n" +
                 "obtener\r\n" + "asignar\r\n" + "nuevo\r\n" + "parcial\r\n" + "producir\r\n" + "añadir\r\n" + "borrar\r\n" + "valor\r\n" + "alias\r\n" + "asendente\r\n" +
                 "desende \r\n" + "desde\r\n" + "grupo\r\n" + "dentro\r\n" + "ordenar\r\n" + "seleccionar\r\n" + "donde\r\n" +
                 "entrar\r\n" + "igual\r\n" + "utilizar\r\n" + "booleano\r\n" + "byte\r\n" + "caracter\r\n" + "decimal\r\n" + "doble\r\n" + "dinamico\r\n" +
                 "sbyte\r\n" + "corto\r\n" + "cadena\r\n" + "uentero\r\n" + "ulargo\r\n" + "ucorto\r\n" + "var\r\n" + "clase\r\n" + "estructura\r\n" + "desde\r\n" + "grupo\r\n" + "dentro\r\n" + "ordenar\r\n" + "seleccionar\r\n" + "donde\r\n" +
                 "entrar\r\n" + "igual\r\n" + "utilizar\r\n" + "booleano\r\n" + "byte\r\n" + "caracter\r\n" + "decimal\r\n" + "doble\r\n" + "dinamico\r\n" +
                 "sbyte\r\n" + "corto\r\n" + "cadena\r\n" + "uentero\r\n" + "ulargo\r\n" + "ucorto\r\n" + "var\r\n" + "clase\r\n" + "estructura";

            }
        }

        private void radioBtnJava_CheckedChanged(object sender, EventArgs e)
        {
            txtLenguaje.Text =
               "abstract\r\n" + "as\r\n" + "async\r\n" + "await\r\n" +
               "checked\r\n" + "const\r\n" + "continue\r\n" + "default\r\n" + "delegate\r\n" + "base\r\n" + "break\r\n" + "case\r\n" +
               "do\r\n" + "else\r\n" + "enum\r\n" + "event\r\n" + "explicit\r\n" + "extern\r\n" + "false\r\n" + "finally\r\n" +
               "fixed\r\n" + "for\r\n" + "foreach\r\n" + "goto\r\n" + "if\r\n" + "implicit\r\n" + "in\r\n" + "interface\r\n" +
               "internal\r\n" + "is\r\n" + "lock\r\n" + "new\r\n" + "null\r\n" + "operator\r\n" + "catch\r\n" +
               "out\r\n" + "override\r\n" + "params\r\n" + "private\r\n" + "protected\r\n" + "public\r\n" + "readonly\r\n" +
               "ref\r\n" + "return\r\n" + "sealed\r\n" + "sizeof\r\n" + "stackalloc\r\n" + "static\r\n" +
               "switch\r\n" + "this\r\n" + "throw\r\n" + "true\r\n" + "try\r\n" + "typeof\r\n" + "namespace\r\n" +
               "unchecked\r\n" + "unsafe\r\n" + "virtual\r\n" + "void\r\n" + "while\r\n" + "float\r\n" + "int\r\n" + "long\r\n" + "object\r\n" +
               "get\r\n" + "set\r\n" + "new\r\n" + "partial\r\n" + "yield\r\n" + "add\r\n" + "remove\r\n" + "value\r\n" + "alias\r\n" + "ascending\r\n" +
               "descending\r\n" + "from\r\n" + "group\r\n" + "into\r\n" + "orderby\r\n" + "select\r\n" + "where\r\n" +
               "join\r\n" + "equals\r\n" + "using\r\n" + "bool\r\n" + "byte\r\n" + "char\r\n" + "decimal\r\n" + "double\r\n" + "dynamic\r\n" +
               "sbyte\r\n" + "short\r\n" + "String\r\n" + "uint\r\n" + "ulong\r\n" + "ushort\r\n" + "var\r\n" + "class\r\n" + "struct\r\n";
        }
    }
}
