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
        private bool pista;
        private int puntos;
        private ObservableCollection<Pelicula> peliculas;

        public Juego(ObservableCollection<Pelicula> peliculas)
        {
            this.peliculas = new ObservableCollection<Pelicula>();
            SeleccionaPeliculas(peliculas);
        }

        private void SeleccionaPeliculas(ObservableCollection<Pelicula> peliculas)
        {
            Random seed = new Random();
            while (this.peliculas.Count <= 5)
            {
                int posicionPelicula = seed.Next(0, peliculas.Count);
                if (!this.peliculas.Contains(peliculas[posicionPelicula]))
                    this.peliculas.Add(peliculas[posicionPelicula]);
            }
        }

        public void EmpiezaJuego()
        {
            pista = false;
            puntos = 0;
        }

        public string GetPuntuacion() 
        {
            return puntos.ToString();
        }
    }
}
