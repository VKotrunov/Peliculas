using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

namespace Peliculas
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OpenFileDialog ofd;
        ObservableCollection<Pelicula> lista;
        Juego j;
        enum Generos
        {
            Comedia, Drama, Acción, Terror, CienciaFicción
        };

        enum Dificultad
        {
            Fácil, Normal, Difícil
        };
        public MainWindow()
        {
            InitializeComponent();
            ofd = new OpenFileDialog();
            ofd.InitialDirectory = Directory.GetCurrentDirectory();
            lista = new ObservableCollection<Pelicula>();
            getSamples();
            actualTextBlock.Text = "1";
            totalTextBlock.Text = lista.Count.ToString();
            peliculasListBox.DataContext = lista;
            generoComboBox.ItemsSource = Enum.GetValues(typeof(Generos));
            dificultadComboBox.ItemsSource = Enum.GetValues(typeof(Dificultad));
            contenedorJuego.DataContext = lista;
        }

        private void getSamples()
        {
            using (StreamReader jsonStream = File.OpenText("datos.json"))
            {
                var json = jsonStream.ReadToEnd();
                lista = JsonConvert.DeserializeObject<ObservableCollection<Pelicula>>(json);
            }
            peliculasListBox.DataContext = lista;
        }

        private void cargarJSONButton_Click(object sender, RoutedEventArgs e)
        {
            ofd.Filter = "Archivos JSON (*.json)|*.json";
            ofd.ShowDialog();
            try
            {
                ofd.OpenFile();

                using (StreamReader jsonStream = File.OpenText(ofd.FileName))
                {
                    var json = jsonStream.ReadToEnd();
                    lista = JsonConvert.DeserializeObject<ObservableCollection<Pelicula>>(json);
                }
                MessageBox.Show("Éxito al cargar", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                peliculasListBox.DataContext = lista;
            }
            catch (Exception)
            {
                MessageBox.Show("Debe seleccionar un archivo", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void guardarJSONButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Archivos JSON (*.json)|*.json";
            sfd.ShowDialog();
            string nombreFichero = sfd.FileName;
            if (string.IsNullOrEmpty(nombreFichero))
            {
                MessageBox.Show("No se guardó la información", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                string datos = JsonConvert.SerializeObject(lista);
                File.WriteAllText(nombreFichero, datos);
                MessageBox.Show("Guardado correctamente", "Guardado", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void examinarButton_Click(object sender, RoutedEventArgs e)
        {
            ofd.Filter = "Archivos de imágenes (*.bmp, *.jpg, *.jpeg)|*.bmp;*.jpg;*.jpeg";
            ofd.Title = "Examinar";
            ofd.ShowDialog();
            if (!string.IsNullOrEmpty(ofd.FileName))
                imagenTextBox.Text = ofd.FileName;
        }

        private void addPeliculaButton_Click(object sender, RoutedEventArgs e)
        {
            // No logro deshabilitar el botón al tener campos vacíos
            if (tituloTextBox.Text != "" && pistaTextBox.Text != "" && dificultadComboBox.SelectedItem.ToString() != null && imagenTextBox.Text != "" && generoComboBox.SelectedItem.ToString() != null) 
            {
                string titulo = tituloTextBox.Text;
                string pista = pistaTextBox.Text;
                string dificultad = dificultadComboBox.SelectedItem.ToString();
                string imagen = imagenTextBox.Text;
                string genero = generoComboBox.SelectedItem.ToString();
                lista.Add(new Pelicula(titulo, pista, dificultad, genero, imagen));
                totalTextBlock.Text = lista.Count.ToString();
            }
        }

        private void eliminarPeliculaButton_Click(object sender, RoutedEventArgs e)
        {
            lista.Remove((Pelicula)peliculasListBox.SelectedItem);
            totalTextBlock.Text = lista.Count.ToString();
        }

        private void deseleccionarPeliculaButton_Click(object sender, RoutedEventArgs e)
        {
            peliculasListBox.UnselectAll();
        }

        private void anteriorImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int actual = int.Parse(actualTextBlock.Text);
            if (actual > 1)
            {
                contenedorJuego.DataContext = lista[actual - 2];
                actualTextBlock.Text = (actual - 1).ToString();
                textoPistaTextBlock.Visibility = Visibility.Hidden;
                pistaCheckBox.IsChecked = false;
                pistaCheckBox.IsEnabled = true;
            }
        }

        private void siguienteImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int actual = int.Parse(actualTextBlock.Text);
            if (actual < lista.Count)
            {
                contenedorJuego.DataContext = lista[actual];
                actualTextBlock.Text = (actual + 1).ToString();
                textoPistaTextBlock.Visibility = Visibility.Hidden;
                pistaCheckBox.IsChecked = false;
                pistaCheckBox.IsEnabled = true;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            textoPistaTextBlock.Visibility = Visibility.Visible;
            pistaCheckBox.IsEnabled = false;
        }

        private void nuevaPartidaButton_Click(object sender, RoutedEventArgs e)
        {
            if (lista.Count < 5)
                MessageBox.Show("No se puede empezar partida hasta tener al menos 5 películas", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            else 
            {
                MessageBox.Show("¡EMPIEZA LA PARTIDA!", "Información", MessageBoxButton.OK, MessageBoxImage.None);
                j = new Juego(lista);
                j.EmpiezaJuego();
                puntosTextBlock.Text = j.GetPuntuacion();
            }
        }
    }
}
