using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoArbolBinario
{
    /// <summary>
    /// Clase secundaria para generar las propiedades de un nodo y el objeto datos.
    /// Contiene las propiedades para obtener y asignar un valor al nodo izquierdo, derecho y al objeto datos.
    /// </summary>
    /// <remarks>
    /// <para>Con esta clase se puede obtener y asignar un valor al nodo para que pueda ser utilizado en la clase Árbol.</para>
    /// </remarks>
    /// <supuestos>Para que esta clase funcione es necesario que por lo menos haya un dato.
    /// </supuestos>
    /// <Autor>
    /// 18131243 Gómez Montes Zaida Sugey 
    /// 18131279 Romero Vázquez Jesús
    /// 18131225 Castañeda Limones Carlos Elian
    ///          Johan Ismael López
    /// </Autor>
    /// <FechaCreacion>Miercoles 22 de septiembre del 2021
    /// </FechaCreacion>
    public class Nodo
    {
        //************************************************************************************************************
        // Variables Locales
        //************************************************************************************************************
        #region VARIABLES LOCALES
        private Object datos; //Son los datos que se van a ir generando respecto al nodo
        private Nodo nodoIzquierdo;
        private Nodo nodoDerecho;
        #endregion

        //************************************************************************************************************
        // Constructores
        //************************************************************************************************************
        #region CONSTRUCTORES

        //Constructor sin parametros
        public Nodo()
        {
            nodoDerecho = nodoIzquierdo = null;
        }
        //Constructor con el parametro datos
        public Nodo(Object datos)
        {
            this.datos = datos;
            nodoDerecho = nodoIzquierdo = null;
        }
        //Constructor con parametros
        public Nodo(Nodo derecho, Nodo izquierdo, Object valor)
        {
            this.nodoDerecho = derecho;
            this.nodoIzquierdo = izquierdo;
            this.datos = valor;
        }

        #endregion

        //************************************************************************************************************
        // Métodos
        //************************************************************************************************************
        #region MÉTODOS
        //NODO IZQUIERDO
        /// <summary>
        /// Método para utilizar las propiedades del nodo izquierdo.
        /// Contiene funciones para obtener o asignar el valor al nodo.
        /// </summary>
        public Nodo NodoIzquierdo { get => nodoIzquierdo; set => nodoIzquierdo = value; }

        //NODO DERECHO
        /// <summary>
        /// Método para utilizar las propiedades del nodo derecho.
        /// Contiene funciones para obtener o asignar el valor al nodo.
        /// </summary>
        public Nodo NodoDerecho { get => nodoDerecho; set => nodoDerecho = value; }

        //DATOS
        /// <summary>
        /// Método para utilizar las propiedades del objeto datos.
        /// Contiene funciones para obtener o asignar el valor al dato.
        /// </summary>
        public Object Datos { get => datos; set => datos = value; }
        #endregion
        
    
    }
}



