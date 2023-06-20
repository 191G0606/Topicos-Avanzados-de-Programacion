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
        //campos
        int indice;
        public string Vista { get; set; } = "ver";

        //EVENTOS
        public event PropertyChangedEventHandler? PropertyChanged;

        //propiedades
        public string Error { get; set; } = "";
        public ObservableCollection<Libro> ListaLibros { get; set; } = new ObservableCollection<Libro>();
        public Libro Libro { get; set; } = new Libro();

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

                //Vista = "ver";
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

                // Vista = "ver";
                CambiarVista("ver");
            }
        }
        public void Editar()
        {
            if (ListaLibros != null)
            {
                ListaLibros[indice] = Libro; //copiacontacto o contacto 

                Guardar();
                CambiarVista("ver");
            }
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
        public void CambiarVista(string vistaCambiar)
        {
            Vista = vistaCambiar;

            if (vistaCambiar == "agregar")
            {
                Libro = new Libro();
            }

            if (vistaCambiar == "editar")
            {
                if (ListaLibros != null)
                {
                    indice = ListaLibros.IndexOf(Libro);
                }

                Libro copiaLibro = new Libro()
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
        public void Cancelar()
        {
            Vista = "ver";
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Vista)));
        }
        public void Guardar()
        {
            var json = JsonConvert.SerializeObject(ListaLibros);
            File.WriteAllText("Libros.json", json);
        }
        public void Cargar()
        {
            if (File.Exists("Libros.json"))
            {
                var json = File.ReadAllText("Libros.json");
                var datos = JsonConvert.DeserializeObject<ObservableCollection<Libro>>(json);

                if (datos != null)
                {
                    ListaLibros = (ObservableCollection<Libro>)datos;
                }
                else
                {
                    ListaLibros = new ObservableCollection<Libro>();
                }
            }
        }

        public LibroViewModel()
        {
            Cargar();

            //propiedades apuntan a los metodos, se instala mvvm light para que aparezca relay te lo da en advertencias
            AgregarCommand = new RelayCommand(Agregar);
            EditarCommand = new RelayCommand(Editar);
            EliminarCommand = new RelayCommand(Eliminar);
            CancelarCommand = new RelayCommand(Cancelar);
            BuscarImagenCommand = new RelayCommand(BuscarImagen);
            CambiarVistaCommand = new RelayCommand<string>(CambiarVista);
        }
    }
}
