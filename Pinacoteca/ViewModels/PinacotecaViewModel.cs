using Pinacoteca.Models;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace Pinacoteca.ViewModels
{
    public class PinacotecaViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public ObservableCollection<PinacotecaModel> ListaPinacotecas { get; set; } = new ObservableCollection<PinacotecaModel>();
        public PinacotecaModel Pinacoteca { get; set; } = new PinacotecaModel();

        int indice;

        //PROPIEDADES
        public string Error { get; set; } = "";
        public string Vista { get; set; } = "ver";

        //COMANDOS
        public ICommand? AgregarCommand { get; set; }
        public ICommand? EditarCommand { get; set; }
        public ICommand? EliminarCommand { get; set; }
        public ICommand? CambiarVistaCommand { get; set; }
        public ICommand? CancelarCommand { get; set; }


        //METODOS
        public void Agregar()
        {
            if (string.IsNullOrEmpty(Pinacoteca.Nombre))
            {
                Error += "No se ingresó el nombre.\n";
            }
            if (string.IsNullOrEmpty(Pinacoteca.Ciudad))
            {
                Error += "No se ingresó la ciudad.\n";
            }
            if (string.IsNullOrEmpty(Pinacoteca.Direccion))
            {
                Error += "No se ingresó la direccion.\n";
            }
            if (string.IsNullOrEmpty(Pinacoteca.MetrosCuadrados.ToString()))
            {
                Error += "No se ingresarón los metros cuadrados.\n";
            }
            // Verifica si el nombre ya existe en el ObservableCollection
            if (ListaPinacotecas.Any(pinacoteca => pinacoteca.Nombre == Pinacoteca.Nombre))
            {
                Error += "El nombre ya existe en la lista.\n";
            }

            if (Error == "" && ListaPinacotecas != null)
            {
                ListaPinacotecas.Add(Pinacoteca);
                Guardar();

                CambiarVista("ver");
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        public void Eliminar()
        {
            if (Pinacoteca != null && ListaPinacotecas != null)
            {
                ListaPinacotecas.Remove(Pinacoteca);
                Guardar();

                CambiarVista("ver");
            }
        }

        public void Modificar()
        {
            if (ListaPinacotecas != null)
            {
                ListaPinacotecas[indice] = Pinacoteca;

                Guardar();
                CambiarVista("ver");
            }
        }

        public void Cancelar()
        {
            Vista = "ver";
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Vista)));
        }

        public void CambiarVista(string vistaCambiar)
        {
            Vista = vistaCambiar;

            if (vistaCambiar == "agregar")
            {
                Pinacoteca = new PinacotecaModel();
            }

            if (vistaCambiar == "editar")
            {
                if (ListaPinacotecas != null)
                {
                    indice = ListaPinacotecas.IndexOf(Pinacoteca);
                }

                PinacotecaModel copiaPinacoteca = new PinacotecaModel()
                {
                    Nombre = Pinacoteca.Nombre,
                    Ciudad = Pinacoteca.Ciudad,
                    Direccion=Pinacoteca.Direccion,
                    MetrosCuadrados=Pinacoteca.MetrosCuadrados
                };

                Pinacoteca = copiaPinacoteca;
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Vista"));
        }

        //SERIALIZAR Y DESERIALIZAR
        public void Cargar()
        {
            if (File.Exists("RegistroPinacotecas.json"))
            {
                var json = File.ReadAllText("RegistroPinacotecas.json");
                var datos = JsonConvert.DeserializeObject<ObservableCollection<PinacotecaModel>>(json);

                if (datos != null)
                {
                    ListaPinacotecas = (ObservableCollection<PinacotecaModel>)datos;
                }
                else
                {
                    ListaPinacotecas = new ObservableCollection<PinacotecaModel>();
                }
            }
        }

        public void Guardar()
        {
            var json = JsonConvert.SerializeObject(ListaPinacotecas);
            File.WriteAllText("RegistroPinacotecas.json", json);
        }

        //CONSTRUCTOR
        public PinacotecaViewModel()
        {
            Cargar();

            //propiedades apuntan a los metodos, se instala mvvm light para que aparezca relay te lo da en advertencias
            AgregarCommand = new RelayCommand(Agregar);
            EditarCommand = new RelayCommand(Modificar);
            EliminarCommand = new RelayCommand(Eliminar);
            CancelarCommand = new RelayCommand(Cancelar);
            CambiarVistaCommand = new RelayCommand<string>(CambiarVista);
        }
    }
}
