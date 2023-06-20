
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DatosLibros.Views
{
    /// <summary>
    /// Lógica de interacción para UCAgregarView.xaml
    /// </summary>
    public partial class UCAgregarView : UserControl
    {
        private string filePath;

        public UCAgregarView()
        {
            InitializeComponent();
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    //OpenFileDialog openFileDialog1 = new OpenFileDialog();
        //    //openFileDialog1.Filter = "Archivos de imágen (.jpg)|*.jpg|All Files (*.*)|*.*";
        //    //openFileDialog1.FilterIndex = 1;
        //    //openFileDialog1.Multiselect = true;
        //    //bool? checarOK = openFileDialog1.ShowDialog();
        //    //if (checarOK == true)
        //    //{
        //    //    //imagen.source = openFileDialog1.FileName.ToString;
        //    //    //(System.Windows.Media.ImageSource)
        //    //    image.Source = new BitmapImage(new Uri(openFileDialog1.FileName));
        //    //    ruta.Text = openFileDialog1.FileName;
        //    //}
        //}
    }
}
