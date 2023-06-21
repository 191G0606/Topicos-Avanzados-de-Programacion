using AgendaMVVM.Models;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AgendaMVVM.ViewModels
{
    public class ContactoViewModel : INotifyPropertyChanged
    {
        //campos
        int indice;

        //eventos
        public event PropertyChangedEventHandler? PropertyChanged;

        //propiedades
        public string Error { get; set; } = "";

        //pq tiene que notificar 
        public ObservableCollection<Contacto> Contactos { get; set; } = new ObservableCollection<Contacto>();

        //se instancian para quitar la advertencia de null
        public Contacto Contacto { get; set; } = new Contacto();

        //se agrega ? para quitar advertencua de null
        public ICommand? AgregarCommand { get; set; }

        public ICommand? EditarCommand { get; set; }

        public ICommand? EliminarCommand { get; set; }

        public ICommand? CambiarVistaCommand { get; set; }

        public ICommand? CancelarCommand { get; set; }

        //ver,eliminar, editar,agregar  en vista valores
        public string Vista { get; set; } = "ver";

        //metodos

        //create
        public void Agregar()
        {
            //los datos vienen desde el formulario esta vez no de parametros, vienen con los bindings

            //validar que tengan datos menos telefono
            Error = "";
            if (string.IsNullOrEmpty(Contacto.Nombre))
            {
                Error += "Escriba el nombre del contacto\n";
            }
            if (string.IsNullOrEmpty(Contacto.Direccion))
            {
                Error += "Escriba la direccion del contacto\n";
            }
            if (string.IsNullOrEmpty(Contacto.Email))
            {
                Error += "Escriba el correo del contacto\n";
            }
            if (Contacto.FechaNacimiento.Date > DateTime.Now.Date)
            {
                Error += "Escriba una fecha valida\n";
            }
            if (string.IsNullOrEmpty(Contacto.Telefono))
            {
                Error += "Escriba el telefono\n";
            }
            if (Error == "" && Contactos != null)
            {
                Contactos.Add(Contacto);
                Guardar();

                //Vista = "ver";
                CambiarVista("ver");
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));

        }

        //read


        //update
        public void Editar()
        {
            if (Contactos != null)
            { 
                Contactos[indice] = Contacto; //copiacontacto o contacto 

                Guardar();
                CambiarVista("ver");
            }
        }

        //delete
        public void Eliminar()
        {
            if (Contacto != null && Contactos != null)
            {
                Contactos.Remove(Contacto);
                Guardar();

                // Vista = "ver";
                CambiarVista("ver");
            }
        }

        public void Guardar()
        {
            var json = JsonConvert.SerializeObject(Contactos); //serializa obcollection
            File.WriteAllText("Contactos.json", json);
        }

        public void Cargar()
        {
            if (File.Exists("Contactos.json"))
            {
                var json = File.ReadAllText("Contactos.json");
                var datos = JsonConvert.DeserializeObject<ObservableCollection<Contacto>>(json);

                if (datos != null)
                {
                    Contactos = (ObservableCollection<Contacto>)datos;
                }
                else
                {
                    Contactos = new ObservableCollection<Contacto>();
                }
            }
        }

        public void CambiarVista(string vistaCambiar)
        {
            Vista = vistaCambiar;

            if (vistaCambiar == "agregar")
            {
                Contacto = new Contacto();
            }

            if (vistaCambiar == "editar")
            {
                if (Contactos != null)
                {
                    indice = Contactos.IndexOf(Contacto);
                }

                Contacto copiaContacto = new Contacto()
                {
                    Nombre = Contacto.Nombre,
                    Direccion = Contacto.Direccion,
                    Email = Contacto.Email,
                    Telefono = Contacto.Telefono,
                    FechaNacimiento = Contacto.FechaNacimiento
                };

                Contacto = copiaContacto;
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Vista"));
        }
        public void Cancelar()
        {
            Vista = "ver";
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Vista)));
        }

        //constructor ctor
        public ContactoViewModel()
        {
            Cargar();

            //propiedades apuntan a los metodos, se instala mvvm light para que aparezca relay te lo da en advertencias
            AgregarCommand = new RelayCommand(Agregar);
            EditarCommand = new RelayCommand(Editar);
            EliminarCommand = new RelayCommand(Eliminar);
            CancelarCommand = new RelayCommand(Cancelar);
            CambiarVistaCommand = new RelayCommand<string>(CambiarVista);
        }
    }
}
