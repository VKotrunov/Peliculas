using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peliculas
{
    public class Juego
    {
        private const int PUNTOS_ACIERTO_FACIL = 100;
        private const int PUNTOS_ACIERTO_NORMAL = 200;
        private const int PUNTOS_ACIERTO_DIFICIL = 100;
        private bool pistaUsada;
        private int puntos;
        private ObservableCollection<Pelicula> peliculasPartida;

        public Juego(ObservableCollection<Pelicula> peliculas)
        {
            SeleccionaPeliculas(peliculas);
        }

        private void SeleccionaPeliculas(ObservableCollection<Pelicula> peliculas)
        {
            peliculasPartida = new ObservableCollection<Pelicula>();
            Random seed = new Random();
            while (peliculasPartida.Count < 5)
            {
                int posicionPelicula = seed.Next(0, peliculas.Count);
                if (!peliculasPartida.Contains(peliculas[posicionPelicula]))
                    peliculasPartida.Add(peliculas[posicionPelicula]);
            }
        }

        public void EmpiezaJuego()
        {
            pistaUsada = false;
            puntos = 0;
        }

        public string GetPuntuacion() 
        {
            return puntos.ToString();
        }

        public ObservableCollection<Pelicula> GetPeliculas() 
        {
            return peliculasPartida;
        }
    }
}
