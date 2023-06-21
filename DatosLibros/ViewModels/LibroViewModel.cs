using DatosLibros.Models;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace DatosLibros.ViewModels
{
    public class LibroViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public ObservableCollection<LibroModel> ListaLibros { get; set; } = new ObservableCollection<LibroModel>();
        public LibroModel Libro { get; set; } = new LibroModel();

        int indice;

        //propiedades
        public string Error { get; set; } = "";
        public string Vista { get; set; } = "ver";

        //COMANDOS
        public ICommand? AgregarCommand { get; set; }
        public ICommand? EditarCommand { get; set; }
        public ICommand? EliminarCommand { get; set; }
        public ICommand? CambiarVistaCommand { get; set; }
        public ICommand? CancelarCommand { get; set; }
        public ICommand? BuscarImagenCommand { get; set; }

        //METODOS
        public void Agregar()
        {
            Error = "";

            if (string.IsNullOrEmpty(Libro.Titulo))
            {
                Error += "Escriba el titulo del Libro\n";
            }
            if (string.IsNullOrEmpty(Libro.Autor))
            {
                Error += "Escriba el autor del Libro\n";
            }
            if (string.IsNullOrEmpty(Libro.TituloOriginal))
            {
                Error += "Escriba el titulo original del Libro\n";
            }
            if (string.IsNullOrEmpty(Libro.Genero))
            {
                Error += "Escriba el genero del Libro\n";
            }
            if (string.IsNullOrEmpty(Libro.Reseña))
            {
                Error += "Escriba la reseña del Libro";
            }
            if (string.IsNullOrEmpty(Libro.RutaImagen))
            {
                Error += "Escriba la ruta de la imagen";
            }

            if (Error == "" && ListaLibros != null)
            {
                ListaLibros.Add(Libro);
                Guardar();
                
                CambiarVista("ver");
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
        public void Eliminar()
        {
            if (Libro != null && ListaLibros != null)
            {
                ListaLibros.Remove(Libro);
                Guardar();

                CambiarVista("ver");
            }
        }

        public void Modificar()
        {
            if (ListaLibros != null)
            {
                ListaLibros[indice] = Libro;

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
                Libro = new LibroModel();
            }

            if (vistaCambiar == "editar")
            {
                if (ListaLibros != null)
                {
                    indice = ListaLibros.IndexOf(Libro);
                }

                LibroModel copiaLibro = new LibroModel()
                {
                    Titulo = Libro.Titulo,
                    Autor = Libro.Autor,
                    TituloOriginal = Libro.TituloOriginal,
                    Genero = Libro.Genero,
                    Reseña = Libro.Reseña,
                    RutaImagen = Libro.RutaImagen
                };

                Libro = copiaLibro;
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Vista"));
        }

        public void BuscarImagen()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Archivos de imágen (.jpg)|*.jpg|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Multiselect = false;

            bool? checarOK = openFileDialog1.ShowDialog();

            if (checarOK == true)
            {
                Libro.RutaImagen = openFileDialog1.FileName;
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        //SERIALIZAR Y DESERIALIZAR
        public void Cargar()
        {
            if (File.Exists("Libros.json"))
            {
                var json = File.ReadAllText("Libros.json");
                var datos = JsonConvert.DeserializeObject<ObservableCollection<LibroModel>>(json);

                if (datos != null)
                {
                    ListaLibros = (ObservableCollection<LibroModel>)datos;
                }
                else
                {
                    ListaLibros = new ObservableCollection<LibroModel>();
                }
            }
        }

        public void Guardar()
        {
            var json = JsonConvert.SerializeObject(ListaLibros);
            File.WriteAllText("Libros.json", json);
        }

        //CONSTRUCTOR
        public LibroViewModel()
        {
            Cargar();

            //propiedades apuntan a los metodos, se instala mvvm light para que aparezca relay te lo da en advertencias
            AgregarCommand = new RelayCommand(Agregar);
            EditarCommand = new RelayCommand(Modificar);
            EliminarCommand = new RelayCommand(Eliminar);
            CancelarCommand = new RelayCommand(Cancelar);
            CambiarVistaCommand = new RelayCommand<string>(CambiarVista);
            BuscarImagenCommand = new RelayCommand(BuscarImagen);
        }
    }
}
