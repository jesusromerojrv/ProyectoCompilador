using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoCompilador
{
    /// <summary>
    /// Clase para obtener las propiedades del token.
    /// Contiene métodos que nos permite obtener y asignar las propiedades del token.
    /// </summary>
    /// <remarks>
    /// <para>Con esta clase se puede hacer uso de los métodos para obtener y asignar el nombre, lexema, indice, linea, columna, tipo de dato, ID tabla, tabla padre de un token.</para>
    /// </remarks>
    /// <supuestos>Para que esta clase funcione es necesario declarar un objeto de esta clase Token para hacer uso de los métodos.
    /// </supuestos>
    /// <Autor>
    /// 18131243 Gómez Montes Zaida Sugey 
    /// 18131279 Romero Vázquez Jesús
    /// 18131225 Castañeda Limones Carlos Elian
    /// </Autor>
    /// <FechaCreacion>Jueves 16 de septiembre del 2021
    /// </FechaCreacion>
    class Token
    {
        //************************************************************************************************************
        // Constructores
        //************************************************************************************************************
        #region CONSTRUCTORES

        //CONSTRUCTOR CON EL PARAMETRO DE Name, Lexema, Index, Linea y Columna.
        public Token(string name, string lexema, int index, int linea, int columna, string tipoDato, int IdTabla, int Tabla_Padre)
        {
            Name = name;
            Lexema = lexema;
            Index = index;
            Linea = linea;
            Columna = columna;
            TipoDato = tipoDato;
            IDTabla = IdTabla;
            TablaPadre = Tabla_Padre;
        }
        #endregion

        //************************************************************************************************************
        // Métodos
        //************************************************************************************************************
        #region METODOS
        
        //Metodos para obtener y asignar
        public string Name { get; set; }
        public string Lexema { get; private set; }

        public int Index { get; private set; }
        public int Linea { get; private set; }
        public int Columna { get; private set; }
        public string TipoDato { get; private set; }
        public int IDTabla { get; private set; }
        public int TablaPadre { get; private set; }

        public int Lenght { get { return Lexema.Length; } }

        #endregion
    }
}

