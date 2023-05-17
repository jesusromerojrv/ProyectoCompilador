using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoCompilador
{
    /// <summary>
    /// Clase para mostrar un splash al inicio del programa
    /// Contiene un evento para el timer de la barra de progreso.
    /// </summary>
    /// <remarks>
    /// <para>Con esta clase se muestra al inicio una barra de proceso con un timer especificado.</para>
    /// </remarks>
    /// <supuestos>Para que esta clase funcione es necesario tener un método en el frame principal para correr el frame del splash.
    /// </supuestos>
    /// <Autor>
    /// 18131225 Castañeda Limones Carlos Elian
    /// 18130568 López Flores Johan Ismael
    /// </Autor>
    /// <FechaCreacion>Miércoles 22 de septiembre del 2021
    /// </FechaCreacion>
    public partial class Splash : Form
    {
        //************************************************************************************************************
        // Constructores
        //************************************************************************************************************
        #region CONSTRUCTORES
        public Splash()
        {
            InitializeComponent();
        }
        #endregion

        //************************************************************************************************************
        // EVENTO DEL TIMER
        //************************************************************************************************************
        #region EVENTO
        private void timer1_Tick(object sender, EventArgs e)
        {
            //incrementamos la barra 
            progressBar1.Increment(1);

            progressBar1.BackColor = Color.Green;

            //si llega al maximo paramos el timer
            if (progressBar1.Value == 100)
                timer1.Stop();
        }
        #endregion
    }
}
