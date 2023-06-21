﻿using System;
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
using System.Windows.Shapes;

namespace DatosLibros.Views
{
    /// <summary>
    /// Lógica de interacción para BibliotecaView.xaml
    /// </summary>
    public partial class BibliotecaView : Window
    {
        
        public BibliotecaView()
        {

        }

        private void UCVerDatosLibroView_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
