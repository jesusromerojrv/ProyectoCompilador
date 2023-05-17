using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Diagnostics;

namespace ProyectoArbolBinario
{
    /// <summary>
    /// Clase para generar el gráfico.
    /// Contiene todas las funciones para dibujar el árbol sintáctico.
    /// </summary>
    /// <remarks>
    /// <para>Con esta clase nos permite dibujar, crear el archivo (imágen), empezar el gráfico segun el recorrido y establecer
    /// el directorio donde se esta ejecutando.</para>
    /// </remarks>
    /// <supuestos>Para que esta clase funcione es necesario instalar una herramienta de software llamado graphviz para el
    /// diseño del diagrama. Y se necesita especificar en el botón "Directorio", la dirección del usuario. 
    /// </supuestos>
    /// <Autor>
    /// 18131243 Gómez Montes Zaida Sugey 
    /// 18131279 Romero Vázquez Jesús
    /// 18131225 Castañeda Limones Carlos Elian
    /// </Autor>
    /// <FechaCreacion>Miercoles 22 de septiembre del 2021
    /// </FechaCreacion>
    public class Grafico
    {
        //************************************************************************************************************
        // Variables Locales
        //************************************************************************************************************
        #region VARIABLES LOCALES
        Nodo arbol; //Se crea un objeto de la clase Nodo
        //proporciona informacion de la ruta del perfil de usuario:
        private string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile); 
        //Variable para asignar un archivo de texto que contiene ordenes a ejecutar:
        private string command = @"/c Batch.bat"; 
        private int i, j;
        #endregion

        //************************************************************************************************************
        // Constructores
        //************************************************************************************************************
        #region CONSTRUCTORES
        //Constructor con el parametro arbol 
        public Grafico(Nodo arbol)
        {
            this.arbol = arbol;
        }
        #endregion

        //************************************************************************************************************
        // Métodos
        //************************************************************************************************************
        #region METODOS
        
        /// <summary>
        /// Método que nos permite dibujar el árbol.
        /// </summary>
        public void DrawTree()
        {
            CreateFileDot();
            ExecuteDot();
        }

        /// <summary>
        /// Método que nos permite crear el archivo donde se va a generar el árbol.
        /// </summary>
        /// <returns>Regresa el valor de la cadena que se escribio en el archivo.</returns>
        private string CreateFileDot()
        {
            string cadenaDot = "";
            StartFileDot(arbol, ref cadenaDot);
            using (StreamWriter archivo = new StreamWriter(path + @"\Arbol.dot"))
            {
                archivo.WriteLine(cadenaDot);
                archivo.Close();
            }
            return cadenaDot;
        }

        /// <summary>
        /// Método que nos permite iniciar con el archivo donde se esta generando el gráfico.
        /// </summary>
        /// <param name="arbol">Es el nodo del árbol.</param>
        /// <param name="cadenaDot">Es la referencia donde se esta creando el gráfico segun sus propiedades.</param>
        private void StartFileDot(Nodo arbol, ref string cadenaDot)
        {
            if (arbol != null)
            {
                cadenaDot += "digraph Grafico {\nnode [style=bold, fillcolor=blue];\n";  //se expecifica las propiedades del gráfico.
                Recorrido(arbol, ref cadenaDot);
                cadenaDot += "\n}"; //se agrega un salto de linea para que se vaya organizando el diagrama.
            }
        }

        /// <summary>
        /// Método que nos permite realizar el recorrido del árbol. 
        /// </summary>
        /// <param name="arbol">Es el nodo del árbol.</param>
        /// <param name="cadenaDot">Es la referencia donde se esta creando el gráfico segun sus propiedades.</param>
        private void Recorrido(Nodo arbol, ref string cadenaDot)
        {
            if (arbol != null)
            {
                cadenaDot += $"{arbol.Datos}\n";
                if (arbol.NodoIzquierdo != null)
                {
                    i = arbol.Datos.ToString().IndexOf("[");
                    j = arbol.NodoIzquierdo.Datos.ToString().IndexOf("[");
                    cadenaDot += $"{arbol.Datos.ToString().Remove(i)}->{arbol.NodoIzquierdo.Datos.ToString().Remove(j)};\n";
                }
                if (arbol.NodoDerecho != null)
                {
                    i = arbol.Datos.ToString().IndexOf("[");
                    j = arbol.NodoDerecho.Datos.ToString().IndexOf("[");
                    cadenaDot += $"{arbol.Datos.ToString().Remove(i)}->{arbol.NodoDerecho.Datos.ToString().Remove(j)};\n";
                }
                Recorrido(arbol.NodoIzquierdo, ref cadenaDot);
                Recorrido(arbol.NodoDerecho, ref cadenaDot);
            }
        }

        /// <summary>
        /// Método que nos permite ejecutar el proceso para que el cmd nos genere el archivo imagen. 
        /// </summary>
        private void ExecuteDot()
        {
            Directory.SetCurrentDirectory(path); 
            using (Process proceso = new Process())
            {
                ProcessStartInfo Info = new ProcessStartInfo("cmd", command);
                Info.CreateNoWindow = true;
                proceso.StartInfo = Info;
                proceso.Start();
                proceso.WaitForExit();
                proceso.Close();

            }
        }
        #endregion
    }
}
