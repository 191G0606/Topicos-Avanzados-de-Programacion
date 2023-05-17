using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AdivinaElNumero
{
    public class JuegoAdivinar : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private int oportunidades;
        public int valor;

        public int Oportunidades
        {
            get { return oportunidades; }
            set
            {
                oportunidades = value;
            }
        }

        public int numero { get; set; } = 0;
        public ICommand? IniciarCommand { get; set; }

        public ICommand? VerificarCommand { get; set; }
        public ICommand? IntentarDeNuevoCommand { get; set; }

        public string Resultado { get; set; } = "";

        public bool JuegoIniciado { get; set; } = false;

        public void Iniciar()
        {
            Random r = new Random();

            valor = r.Next(0, 5000);

            Oportunidades = 10;
            JuegoIniciado = true;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));

        }

        public void Verificar()
        {
            Resultado = "";
            /*Se verifica que la combinacion sea igual que datosJuego*/
            if (valor == numero)
            {
                Resultado = $"Felicidades adivinaste!!! el número es: {numero}";
            }
            if (valor < numero)
            {
                Resultado = "Uff no, tu número es mayor";
                Oportunidades--;
            }
            else if (valor > numero)
            {
                Resultado = "Uff no, tu número es menor";
                Oportunidades--;
            }

            if (Oportunidades == 0)
            {
                Resultado = "Lo siento, se terminarón tus oportunidades :c";
                JuegoIniciado = false;
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        //constructor
        public JuegoAdivinar()
        {
            //se van a instanciar los comandos, se necesitan dos metodos para que funcionen los comandos.
            IniciarCommand = new RelayCommand(Iniciar);
            VerificarCommand = new RelayCommand(Verificar);
        }
    }
}
