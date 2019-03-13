using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página de elementos está documentada en http://go.microsoft.com/fwlink/?LinkId=234233

namespace Super_Secret_Formula
{
    /// <summary>
    /// Página en la que se muestra una colección de vistas previas de elementos. En la aplicación dividida, esta página
    /// se usa para mostrar y seleccionar uno de los grupos disponibles.
    /// </summary>
    public sealed partial class SelectorTareas : Super_Secret_Formula.Common.LayoutAwarePage
    {
        public SelectorTareas()
        {
            this.InitializeComponent();
            ActualizarTile();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // TODO: Assign a bindable collection of items to this.DefaultViewModel["Items"]
            var Fuentes = FuenteTareas.ObtenerGrupos((String)navigationParameter);
            this.DefaultViewModel["Grupos"] = Fuentes;
        }

        private void AcercaDe(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Acerca_de));
        }

        private void ClickItem(object sender, ItemClickEventArgs e)
        {
            var Seleccion = (Tarea)e.ClickedItem;
            if (Seleccion != null)
            {
                if (Seleccion.Linker.Equals("Logica"))
                {
                    this.Frame.Navigate(typeof(Inicio));
                }
                else
                {
                    this.Frame.Navigate(typeof(Conjuntos));
                }
            }
        }

        private XmlDocument CrearIconoCuadradoTexto(string Texto)
        {
            var TileXML = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquareText02);
            TileXML.GetElementsByTagName("text")[0].AppendChild(TileXML.CreateTextNode("Uninorte"));
            TileXML.GetElementsByTagName("text")[1].AppendChild(TileXML.CreateTextNode(Texto));
            return TileXML;
        }
        
        private void ActualizarTile()
        {
            var Actualizador = TileUpdateManager.CreateTileUpdaterForApplication();
            Actualizador.Update(
                new TileNotification(CrearIconoCuadradoTexto("Bienvenido Ismael Gutierrez."))
            {
                ExpirationTime = DateTimeOffset.UtcNow.AddMinutes(0.4)
            });
        }

    }
}
