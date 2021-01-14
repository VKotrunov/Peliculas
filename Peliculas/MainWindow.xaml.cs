using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Peliculas
{
    public partial class MainWindow : Window
    {
        const int PUNTOS_ACIERTO_FACIL = 100;
        const int PUNTOS_ACIERTO_NORMAL = 200;
        const int PUNTOS_ACIERTO_DIFICIL = 300;
        int puntosPartida;
        int aciertos;
        OpenFileDialog ofd;
        ObservableCollection<Pelicula> lista;
        ObservableCollection<Pelicula> peliculasPartida;
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
            getSamples();
        }

        private void getSamples()
        {
            lista = new ObservableCollection<Pelicula>();
            using (StreamReader jsonStream = File.OpenText("datos.json"))
            {
                var json = jsonStream.ReadToEnd();
                lista = JsonConvert.DeserializeObject<ObservableCollection<Pelicula>>(json);
            }
            peliculasListBox.DataContext = lista;
            contenedorJuego.DataContext = lista;
            actualTextBlock.Text = "1";
            totalTextBlock.Text = lista.Count.ToString();
            generoComboBox.ItemsSource = Enum.GetValues(typeof(Generos));
            dificultadComboBox.ItemsSource = Enum.GetValues(typeof(Dificultad));
            peliculasPartida = lista;
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
                MessageBox.Show("No se guardó la información", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
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
            string titulo = tituloTextBox.Text;
            string pista = pistaTextBox.Text;
            string dificultad = dificultadComboBox.SelectedItem.ToString();
            string imagen = imagenTextBox.Text;
            string genero = generoComboBox.SelectedItem.ToString();
            lista.Add(new Pelicula(titulo, pista, dificultad, genero, imagen));
            totalTextBlock.Text = lista.Count.ToString();
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
                contenedorJuego.DataContext = peliculasPartida[actual - 2];
                actualTextBlock.Text = (actual - 1).ToString();
                validarTextBox.Text = "";
                GestionarCheckBox(actual - 2);
            }
        }

        private void siguienteImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int actual = int.Parse(actualTextBlock.Text);
            if (actual < peliculasPartida.Count)
            {
                contenedorJuego.DataContext = peliculasPartida[actual];
                actualTextBlock.Text = (actual + 1).ToString();
                validarTextBox.Text = "";
                GestionarCheckBox(actual);
            }
        }

        private void GestionarCheckBox(int pos) 
        {
            pistaCheckBox.IsChecked = peliculasPartida[pos].PistaVista;
            // Si la pista está vista, no puede deshabilitar
            pistaCheckBox.IsEnabled = !peliculasPartida[pos].PistaVista;
            if (peliculasPartida[pos].PistaVista)
                textoPistaTextBlock.Visibility = Visibility.Visible;
            else
                textoPistaTextBlock.Visibility = Visibility.Hidden;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            textoPistaTextBlock.Visibility = Visibility.Visible;
            int actual = int.Parse(actualTextBlock.Text);
            peliculasPartida[actual-1].PistaVista = true;
            pistaCheckBox.IsEnabled = false;
        }

        private void nuevaPartidaButton_Click(object sender, RoutedEventArgs e)
        {
            if (lista.Count < 5)
                MessageBox.Show("No se puede empezar partida hasta tener al menos 5 películas", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                EmpiezaJuego();
        }

        private void SeleccionaPeliculas()
        {
            peliculasPartida = new ObservableCollection<Pelicula>();
            // Para generar una nueva referencia y evitar tocar la lista original
            // Con esto evito tener que recorrer la lista original y resetear los valores a cada vez que inicio el juego
            ObservableCollection<Pelicula> listaAux = new ObservableCollection<Pelicula>();
            foreach (Pelicula p in lista)
                listaAux.Add(new Pelicula(p.Titulo, p.Pista, p.Dificultad, p.Genero, p.Imagen));
            Random seed = new Random();
            while (peliculasPartida.Count < 5)
            {
                int posicionPelicula = seed.Next(0, listaAux.Count);
                if (!peliculasPartida.Contains(listaAux[posicionPelicula])) 
                    peliculasPartida.Add(listaAux[posicionPelicula]);
            }
        }
        private void EmpiezaJuego() 
        {
            MessageBox.Show("¡EMPIEZA LA PARTIDA!", "Información", MessageBoxButton.OK, MessageBoxImage.None);
            SeleccionaPeliculas();
            puntosPartida = 0;
            puntosTextBlock.Text = puntosPartida.ToString();
            textoPistaTextBlock.Visibility = Visibility.Hidden;
            pistaCheckBox.IsChecked = false;
            pistaCheckBox.IsEnabled = true;
            contenedorJuego.DataContext = peliculasPartida[0];
            actualTextBlock.Text = "1";
            totalTextBlock.Text = peliculasPartida.Count.ToString();
            aciertos = 0;
        }

        private void validarButton_Click(object sender, RoutedEventArgs e)
        {
            int actual = int.Parse(actualTextBlock.Text)-1;
            string titulo = peliculasPartida[actual].Titulo.ToLower().Trim();
            string intento = validarTextBox.Text.ToLower().Trim();
            if (titulo.Equals(intento)) 
            {
                peliculasPartida[actual].Resuelta = true;
                Puntuar(actual);
                aciertos++;
                CompruebaFinal();
            }
        }

        private void Puntuar(int pos) 
        {
            string dificultad = peliculasPartida[pos].Dificultad;
            bool pistaVista = peliculasPartida[pos].PistaVista;
            int puntosGanados = 0;
            if (dificultad.Equals("Fácil"))
                puntosGanados = PUNTOS_ACIERTO_FACIL;
            else if(dificultad.Equals("Normal"))
                puntosGanados = PUNTOS_ACIERTO_NORMAL;
            else if (dificultad.Equals("Difícil"))
                puntosGanados = PUNTOS_ACIERTO_DIFICIL;
            if (pistaVista)
                puntosGanados /= 2;
            puntosPartida += puntosGanados;
            puntosTextBlock.Text = puntosPartida.ToString();
        }
        private void CompruebaFinal() 
        {
            if (aciertos == 5)
            {
                MessageBox.Show("¡GANASTE! \nHas hecho un total de "+puntosPartida+" puntos.", "Fin de la partida", MessageBoxButton.OK, MessageBoxImage.None);
                if (MessageBox.Show("¿Desea volver a jugar?", "Jugar otra vez", MessageBoxButton.YesNo, MessageBoxImage.None) == MessageBoxResult.Yes)
                    EmpiezaJuego();
            }
        }
    }
}
