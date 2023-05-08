using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MasterMind_CombinacionNumerica
{
    public class Mastermind : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private int[] valores = new int[4];
        //public bool[] valoresBool = new bool[4];
        private int aciertos;
        private int oprtunidades;

        //#region propiedades bools
        //public bool Caja0 { get; set; }
        //public bool Caja1 { get; set; }
        //public bool Caja2 { get; set; }
        //public bool Caja3 { get; set; }
        //#endregion

        public int Aciertos
        {
            get { return aciertos; }
            set { aciertos = value; }
        }

        public int Oportunidades
        {
            get { return oprtunidades; }
            set
            {

                oprtunidades = value;

            }
        }

        public ICommand IniciarCommand { get; set; }

        public ICommand VerificarCommand { get; set; }
        public string Resultado { get; set; }
        public bool JuegoIniciado { get; set; } = false;

        public void Iniciar()
        {
            Random r = new Random();

            for (int i = 0; i < 4; i++)
            {
                valores[i] = r.Next(0, 10);
            }

            Oportunidades = 10;
            Aciertos = 0;
            JuegoIniciado = true;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        public void Verificar(int[] nums)
        {
            aciertos = 0;
            for (int i = 0; i < nums.Length; i++)
            {
                /*Se verifica que la combinacion sea igual que datosJuego*/
                if (valores[i] == nums[i])
                {
                    Aciertos++;
                    //valoresBool[2] = true;
                    //Caja0 = valoresBool[i];
                }

            }
            if (Aciertos != 4)
            {
                Oportunidades--;
            }
            if (aciertos == 4)
            {
                Resultado = "Felicidades adivinaste";
                JuegoIniciado = false;
            }
            else if (Oportunidades == 0)
            {
                Resultado = "Lo siento, se terminaron tus oportunidades";
                JuegoIniciado = false;
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        //constructor
        public Mastermind()
        {
            //se van a instanciar los comandos, se necesitan dos metodos para que funcionen los comandos.
            IniciarCommand = new RelayCommand(Iniciar);
            VerificarCommand = new RelayCommand<int[]>(Verificar);
        }

    }
}
