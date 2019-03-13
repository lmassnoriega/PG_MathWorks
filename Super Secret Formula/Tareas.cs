using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Super_Secret_Formula
{
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class TareasFormula : Super_Secret_Formula.Common.BindableBase
    {
        private static Uri _Base = new Uri("ms-appx:///");

        private string _ID = String.Empty;
        private string _Nombre = String.Empty;
        private string _Linker = String.Empty;
        private string _RutaImagen = null;
        private ImageSource _Imagen = null;

        public TareasFormula(String ID, String Nombre, String Linker, String RutaImagen)
        {
            this._ID = ID;
            this._Nombre = Nombre;
            this._Linker = Linker;
            this._RutaImagen = RutaImagen;
        }
        public string ID
        {
            get { return this._ID; }
            set { this.SetProperty(ref this._ID, value); }
        }
        public string Nombre
        {
            get { return this._Nombre; }
            set { this.SetProperty(ref this._Nombre, value); }
        }
        public string Linker
        {
            get { return this._Linker; }
            set { this.SetProperty(ref this._Linker, value); }
        }
        public ImageSource Imagen
        {
            get
            {
                if (this._Imagen == null && this._RutaImagen!= null)
                {
                    this._Imagen= new BitmapImage(new Uri(TareasFormula._Base, this._RutaImagen));
                }
                return this._Imagen;
            }

            set
            {
                this._RutaImagen = null;
                this.SetProperty(ref this._Imagen, value);
            }
        }
        public void PonerImagen(String path)
        {
            this._Imagen = null;
            this._RutaImagen = path;
            this.OnPropertyChanged("Imagen");
        }

    }

    public class Tarea : TareasFormula
    {
        private GrupoTareas _Grupo;
        public Tarea(String ID, String Nombre, String Linker, String RutaImagen, GrupoTareas Grupo) :base(ID,Nombre,Linker,RutaImagen)
        {
            this._Grupo = Grupo;
        }
        public GrupoTareas Grupo
        {
            get { return this._Grupo; }
            set { this.SetProperty(ref this._Grupo, value); }
        }
    }

    public class GrupoTareas : TareasFormula
    {
        public GrupoTareas(String ID, String Nombre, String Linker, String RutaImagen) :base(ID,Nombre,Linker,RutaImagen)
        {

        }
        private ObservableCollection<Tarea> _Items = new ObservableCollection<Tarea>();
        public ObservableCollection<Tarea> Items
        {
            get { return this._Items; }
        }
        public IEnumerable<Tarea> TopItems
        {
            get { return this._Items.Take(6); }
        }
    }

    public sealed class FuenteTareas
    {
        private static FuenteTareas _Fuentes = new FuenteTareas();
        private ObservableCollection<GrupoTareas> _TodosLosGrupos = new ObservableCollection<GrupoTareas>();
        public ObservableCollection<GrupoTareas> TodosLosGrupos
        {
            get { return this._TodosLosGrupos; }
        }
        public static IEnumerable<GrupoTareas> ObtenerGrupos(string ID)
        {
            if (!ID.Equals("TodosLosGrupos")) throw new ArgumentException("Solo 'TodosLosGrupos' es soportado como colección");

            return _Fuentes.TodosLosGrupos;
        }
        public static GrupoTareas ObtenerGrupo(string ID)
        {
            var matches = _Fuentes.TodosLosGrupos.Where((Grupo) => Grupo.ID.Equals(ID));
            if (matches.Count() == 1) return matches.First();
            return null;
        }
        public static Tarea ObtenerTarea(string ID)
        {
            var matches = _Fuentes.TodosLosGrupos.SelectMany(Grupo => Grupo.Items).Where((Item) => Item.ID.Equals(ID));
            if (matches.Count() == 1) return matches.First();
            return null;
        }
        public FuenteTareas()
        {
            var Grupo = new GrupoTareas("Grupo1", "Matematicas Discretas", "Discretas", "");
            Grupo.Items.Add(new Tarea("Logica", "Logica de Primer Orden", "Logica", "Assets/Logica.jpg", Grupo));
            Grupo.Items.Add(new Tarea("Conjuntos", "Teoría de Conjuntos", "Conjuntos", "Assets/Conjuntos.jpg", Grupo));
            this._TodosLosGrupos.Add(Grupo);
        }
    }
}
