using System.ComponentModel;


namespace Peliculas
{
    public class Pelicula : INotifyPropertyChanged
    {

        private string _titulo;
        public string Titulo
        {
            get => _titulo;
            set
            {
                if (_titulo != value)
                {
                    _titulo = value;
                    NotifyPropertyChanged("Titulo");
                }
            }
        }

        private string _pista;
        public string Pista
        {
            get => _pista;
            set
            {
                if (_pista != value)
                {
                    _pista = value;
                    NotifyPropertyChanged("Pista");
                }
            }
        }

        private string _dificultad;
        public string Dificultad
        {
            get => _dificultad;
            set
            {
                if (_dificultad != value)
                {
                    _dificultad = value;
                    NotifyPropertyChanged("Dificultad");
                }
            }
        }

        private string _genero;
        public string Genero
        {
            get => _genero;
            set
            {
                if (_genero != value)
                {
                    _genero = value;
                    NotifyPropertyChanged("Genero");
                }
            }
        }

        private string _imagen;
        public string Imagen
        {
            get => _imagen;
            set
            {
                if (_imagen != value)
                {
                    _imagen = value;
                    NotifyPropertyChanged("Imagen");
                }
            }
        }

        private bool _pistaVista;
        public bool PistaVista
        {
            get => _pistaVista;
            set
            {
                if (_pistaVista != value)
                {
                    _pistaVista = value;
                    NotifyPropertyChanged("PistaVista");
                }
            }
        }

        private bool _resuelta;
        public bool Resuelta
        {
            get => _resuelta;
            set
            {
                if (_resuelta != value)
                {
                    _resuelta = value;
                    NotifyPropertyChanged("Resuelta");
                }
            }
        }

        public Pelicula(string titulo, string pista, string dificultad, string genero, string imagen)
        {
            Titulo = titulo;
            Pista = pista;
            Dificultad = dificultad;
            Genero = genero;
            Imagen = imagen;
            PistaVista = false;
            Resuelta = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
