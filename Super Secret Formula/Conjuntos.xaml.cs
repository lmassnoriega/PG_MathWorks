using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Super_Secret_Formula
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class Conjuntos : Super_Secret_Formula.Common.LayoutAwarePage
    {
        string[] elementos = new string[100];
        int tam = 0;
        int i = 0;
        Queue<string> potencia = new Queue<string>(100);
        Pareja[] Relacion1 = new Pareja[100];
        Pareja[] Relacion2 = new Pareja[100];
        int tam1 = 0, tam2 = 0, j = 0;

        public Conjuntos()
        {
            this.InitializeComponent();
            PaginaInicial();
        }

        public class Pareja
        {
            public Pareja(string text)
            {
                Elem1 = text.Substring(1, 1);
                Elem2 = text.Substring(3, 1);
            }
            public Pareja(string item1, string item2)
            {
                Elem1 = item1;
                Elem2 = item2;
            }
            public string Elem1 { get; set; }
            public string Elem2 { get; set; }
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
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        //Julio Composicion de Relaciones

        private void AgregarPareja(object sender, RoutedEventArgs e)
        {
            if (isPareja(((TextBox)FindName("Ingreso")).Text))
            {
                if (!exists(new Pareja(((TextBox)FindName("Ingreso")).Text)))
                {
                    if (System.Convert.ToBoolean(((RadioButton)FindName("InsertaS")).IsChecked))
                    {
                        Relacion1[i] = new Pareja(((TextBox)FindName("Ingreso")).Text);
                        i++; tam1++;
                        ((ListBox)FindName("RelS")).Items.Add(((TextBox)FindName("Ingreso")).Text);
                    }
                    else
                    {
                        Relacion2[j] = new Pareja(((TextBox)FindName("Ingreso")).Text);
                        j++; tam2++;
                        ((ListBox)FindName("RelR")).Items.Add(((TextBox)FindName("Ingreso")).Text);
                    }
                }
                else MuestraMsg("Esta pareja ordenada ya existe en esta relación!");
            }
            else MuestraMsg("Este elemento no es una pareja ordenada");
        }

        public bool isPareja(string text)
        {
            if (!text.StartsWith("(") || !text.EndsWith(")")) return false;
            else if (!text.Substring(2, 1).Equals(",")) return false;
            else return true;
        }

        public bool exists(Pareja p)
        {
            if (System.Convert.ToBoolean(((RadioButton)FindName("InsertaS")).IsChecked))
            {
                for (int i = 0; i < tam1; i++)
                {
                    if (Relacion1[i] != null)
                    {
                        if (Relacion1[i].Elem1.Equals(p.Elem1) && Relacion1[i].Elem2.Equals(p.Elem2)) return true;
                    }
                }
            }

            if (System.Convert.ToBoolean(((RadioButton)FindName("InsertaR")).IsChecked))
            {
                for (int i = 0; i < tam2; i++)
                {
                    if (Relacion2[i] != null)
                    {
                        if (Relacion2[i].Elem1.Equals(p.Elem1) && Relacion2[i].Elem2.Equals(p.Elem2)) return true;
                    }
                }
            }
            return false;
        }

        public void Composicion(Pareja[] Relacion1, int tam1, Pareja[] Relacion2, int tam2)
        {
            Queue<Pareja> comp = new Queue<Pareja>(100);

            for (int i = 0; i < tam2; i++)
            {
                Pareja p = Relacion2[i];

                for (int j = 0; j < tam1; j++)
                {
                    if (p.Elem2.Equals(Relacion1[j].Elem1))
                    {
                        comp.Enqueue(new Pareja(Relacion2[i].Elem1, Relacion1[j].Elem2));
                    }

                }
            }

            ((ListBox)FindName("Compuesto")).Items.Clear();
            if (comp.Count == 0)
            {
                MuestraMsg("Para estas dos relaciones, esta composición tiene 0 elementos.");
                ((ListBox)FindName("Compuesto")).Items.Add("Vacio");
            }
            else
            {
                for (int i = 0; i < comp.Count; i++)
                {
                    Pareja p = comp.Dequeue();
                    ((ListBox)FindName("Compuesto")).Items.Add("(" + p.Elem1 + "," + p.Elem2 + ")");
                }
            }
        }

        private void HallarComposicion(object sender, RoutedEventArgs e)
        {
            if (System.Convert.ToBoolean(((RadioButton)FindName("ComponeR")).IsChecked)) Composicion(Relacion1, tam1, Relacion2, tam2);
            if (System.Convert.ToBoolean(((RadioButton)FindName("ComponeS")).IsChecked)) Composicion(Relacion2, tam2, Relacion1, tam1);
        }

        private void RelacionSDelete(object sender, RoutedEventArgs e)
        {
            ((ListBox)FindName("RelS")).Items.Clear();
            Relacion1 = new Pareja[100];
            tam1 = 0;
            i = 0;
        }

        private void RelacionRDelete(object sender, RoutedEventArgs e)
        {
            ((ListBox)FindName("RelR")).Items.Clear();
            Relacion2 = new Pareja[100];
            tam2 = 0;
            j = 0;
        }

        //Julio Conjunto Potencia
        
        public bool exists(string elemento)
        {
            for (int i = 0; i < tam; i++)
            {
                if (elementos[i].Equals(elemento)) return true;
            }

            return false;
        }

        public int Binario(int n)
        {
            int dig = n % 2;

            if (n == 0) return 0;
            else return Binario(n / 2) * 10 + dig;
        }

        private void Potencia()
        {
            Queue<string> values = new Queue<string>(100);
            string x;
            if (elementos[0].Equals("EMPTY"))
            {
                ((ListBox)FindName("VistaPotencia")).Items.Clear();
                ((ListBox)FindName("VistaPotencia")).Items.Add("{0}");
            }
            else
            {
                for (int i = 0; i < Math.Pow(2, tam); i++)
                {
                    string limiter = "" + Binario(i);
                    x = "";
                    for (int j = 0; j < tam - limiter.Length; j++)
                    {
                        x += "0";
                    }
                    x += Binario(i);
                    values.Enqueue(x);
                }

                for (int i = 0; i < Math.Pow(2, tam); i++)
                {
                    x = values.Dequeue();
                    string s = "{";
                    for (int j = 0; j < x.Length; j++)
                    {
                        if (x.Substring(j, 1).Equals("1"))
                        {
                            s += elementos[j] + ",";
                        }
                    }
                    if (s.EndsWith(","))
                    {
                        s = s.Substring(0, s.Length - 1) + "}";
                    }
                    else
                    {
                        s += "}";
                        if (s.Equals("{}")) s = "0";
                    }

                    potencia.Enqueue(s);
                }

                ((ListBox)FindName("VistaPotencia")).Items.Clear();
                for (int i = 0; i < Math.Pow(2, tam); i++) ((ListBox)FindName("VistaPotencia")).Items.Add(potencia.Dequeue());
            }


        }

        private void ConjPotencia(object sender, RoutedEventArgs e)
        {
            Potencia();
            ((TextBlock)FindName("Anotacion")).Text = "El conjunto potencia contiene: " + Math.Pow(2, tam) + " elementos";
        }

        private void NuevoConjunto(object sender, RoutedEventArgs e)
        {
            SacarRespuesta("Al introducir un nuevo conjunto, se borrará el actual.\n¿Está seguro de realizar esta operación?");
        }

        private void AgregarAConjunto(object sender, RoutedEventArgs e)
        {
            if (((TextBox)FindName("Ingreso")).Text.Equals("empty") || ((TextBox)FindName("Ingreso")).Text.Equals("EMPTY"))
            {
                if (tam == 0)
                {
                    MuestraMsg("Ha introducido un conjunto vacío.");
                    elementos[0] = "EMPTY";
                }
                ((Button)FindName("PotenciaBoton")).IsEnabled = true;
            }
            else if (((TextBox)FindName("Ingreso")).Text.StartsWith("{") && !((TextBox)FindName("Ingreso")).Text.EndsWith("}"))
            {
                MuestraMsg("Ha introducido un subconjunto incorrecto. Verifique de nuevo.");
                ((TextBox)FindName("Ingreso")).Text = "";
            }
            else if (((TextBox)FindName("Ingreso")).Text.IndexOf(",") != -1)
            {

                if (!((TextBox)FindName("Ingreso")).Text.StartsWith("{"))
                {
                    MuestraMsg("Introduzca un sólo elemento del conjunto.");
                    ((TextBox)FindName("Ingreso")).Text = "";
                }
                else
                {
                    if (!exists(((TextBox)FindName("Ingreso")).Text))
                    {
                        elementos[i] = ((TextBox)FindName("Ingreso")).Text;
                        i++; tam++;
                        ((ListBox)FindName("VistaElementos")).Items.Add(((TextBox)FindName("Ingreso")).Text);
                    }
                    else MuestraMsg("Este elemento ya existe en el conjunto");
                    ((TextBox)FindName("Ingreso")).Text = "";
                    ((Button)FindName("PotenciaBoton")).IsEnabled = true;
                }
            }
            else if (((TextBox)FindName("Ingreso")).Text.Equals("") || ((TextBox)FindName("Ingreso")).Text.IndexOf(" ") != -1) MuestraMsg("No ha escrito un elemento del conjunto!");
            else
            {
                if (!exists(((TextBox)FindName("Ingreso")).Text))
                {
                    elementos[i] = ((TextBox)FindName("Ingreso")).Text;
                    i++; tam++;
                    ((ListBox)FindName("VistaElementos")).Items.Add(((TextBox)FindName("Ingreso")).Text);
                }
                else MuestraMsg("Este elemento ya existe en el conjunto");
                ((TextBox)FindName("Ingreso")).Text = "";
                ((Button)FindName("PotenciaBoton")).IsEnabled = true;
            }
        }

        //Lucho interfaz Grafica + Elemento Inverso

        private void NuevoConjuntoIngresado(IUICommand command)
        {
            tam = 0; i = 0; potencia.Clear(); elementos = new string[100];
            ((ListBox)FindName("VistaElementos")).Items.Clear(); ((ListBox)FindName("VistaPotencia")).Items.Clear();
            ((Button)FindName("PotenciaBoton")).IsEnabled = false;
            ((TextBlock)FindName("Anotacion")).Text = "No se ha calculado el Conjunto Potencia.";
            ((CheckBox)FindName("Vacio")).IsChecked = false;
        } 

        private async void SacarRespuesta(String Msj)
        {
            var MensajeEvaluado = new Windows.UI.Popups.MessageDialog(Msj, "Super Secret Formula!");
            MensajeEvaluado.Commands.Add(new UICommand("Proceder",new UICommandInvokedHandler(this.NuevoConjuntoIngresado)));
            MensajeEvaluado.Commands.Add(new UICommand("Cancelar"));
            MensajeEvaluado.DefaultCommandIndex = 1;
            MensajeEvaluado.CancelCommandIndex = 1;
            await MensajeEvaluado.ShowAsync();
        }

        private async void MuestraMsg(String Msj)
        {
            var Mensaje = new Windows.UI.Popups.MessageDialog(Msj, "Super Secret Formula!");
            await Mensaje.ShowAsync();
        }

        private void AcercaDe(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Acerca_de));
        }

        private void Comienzo(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SelectorTareas),"TodosLosGrupos");
        }

        private void PaginaInicial()
        {
            Contenido.Children.Clear();
            Contenido.RowDefinitions.Clear();
            Contenido.ColumnDefinitions.Clear();
            for (int i = 0; i < 5; i++)
            {
                RowDefinition Nueva = new RowDefinition();
                Nueva.MaxHeight = 50;
                Contenido.RowDefinitions.Add(Nueva);
            }
            
            TextBlock Bienvenida = new TextBlock();
            Bienvenida.Text = "Bienvenido por favor seleccione entre las opciones disponibles para comenzar.";
            Bienvenida.FontSize = 20;
            Bienvenida.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            Grid.SetRow(Bienvenida, 0);
            Contenido.Children.Add(Bienvenida);

            RadioButton Bono1 = new RadioButton();
            Bono1.Content = "Composicion de Funciones";
            Bono1.IsChecked = System.Convert.ToBoolean(true);
            Bono1.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            Bono1.Name = "Bono1";
            Grid.SetRow(Bono1, 1);
            Contenido.Children.Add(Bono1);

            RadioButton Bono2 = new RadioButton();
            Bono2.Content = "Conjunto Potencia de un Conjunto Dado";
            Bono2.Name = "Bono2";
            Bono2.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            Grid.SetRow(Bono2, 2);
            Contenido.Children.Add(Bono2);

            RadioButton Bono3 = new RadioButton();
            Bono3.Content = "Clase inversa en un Conjunto Z(n)";
            Bono3.Name = "Bono3";
            Bono3.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            Grid.SetRow(Bono3, 3);
            Contenido.Children.Add(Bono3);

            Button Seleccion = new Button();
            Seleccion.Content = "Aceptar Seleccion y comenzar.";
            Seleccion.Name = "Seleccion";
            Seleccion.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            Seleccion.Click += SeleccionarPagina;
            Grid.SetRow(Seleccion, 4);
            Contenido.Children.Add(Seleccion);   
        }

        private void SeleccionarPagina(object sender, RoutedEventArgs e)
        {
            Windows.UI.Xaml.Controls.RadioButton Potencia = FindName("Bono2") as Windows.UI.Xaml.Controls.RadioButton;
            Windows.UI.Xaml.Controls.RadioButton Composicion = FindName("Bono1") as Windows.UI.Xaml.Controls.RadioButton;
            Windows.UI.Xaml.Controls.RadioButton Inversos = FindName("Bono3") as Windows.UI.Xaml.Controls.RadioButton;

            if (System.Convert.ToBoolean(Potencia.IsChecked))
            {
                ConjuntoPotencia();
            }
            else
            {
                if (System.Convert.ToBoolean(Composicion.IsChecked))
                {
                    Componer();
                }
                else
                {
                    Inverso();
                }
            }
        }

        private void AnimarGrid()
        {
            Windows.UI.Xaml.Media.Animation.Storyboard Animacion = FindName("AnimacionGrid") as Windows.UI.Xaml.Media.Animation.Storyboard;
            if (Animacion != null)
            {
                Animacion.Begin();
            }
        }

        private void ConjuntoPotencia()
        {
            Contenido.Children.Clear();
            Contenido.RowDefinitions.Clear();
            for (int i = 0; i < 2; i++)
            {
                ColumnDefinition Nueva = new ColumnDefinition();
                Nueva.MaxWidth = 400;
                Contenido.ColumnDefinitions.Add(Nueva);
            }
            for (int i = 0; i < 6; i++)
            {
                RowDefinition Nueva = new RowDefinition();
                if(i==4)
                {
                    Nueva.MaxHeight = 300;
                }
                else
                {
                    Nueva.MaxHeight= 40;
                }
                Contenido.RowDefinitions.Add(Nueva);
            }

            TextBlock Instruccion = new TextBlock();
            Instruccion.Text = "Escriba el siguiente elemento del conjunto:";
            Instruccion.FontStretch = Windows.UI.Text.FontStretch.Normal;
            Instruccion.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            Instruccion.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            Instruccion.FontSize = 18;
            Grid.SetColumn(Instruccion,0);
            Grid.SetRow(Instruccion,0);
            Contenido.Children.Add(Instruccion);

            TextBox Elemento = new TextBox();
            Elemento.Name = "Ingreso";
            Elemento.Width = 200;
            Elemento.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            Elemento.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            Elemento.FontSize = 14;
            Grid.SetColumn(Elemento,0);
            Grid.SetRow(Elemento,1);
            Contenido.Children.Add(Elemento);

            Button Pot = new Button();
            Pot.Name = "PotenciaBoton";
            Pot.Content = "Conjunto Potencia del Conjunto Ingresado";
            Pot.IsEnabled = false;
            Pot.Click += ConjPotencia;
            Pot.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            Pot.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            Pot.FontSize = 14;
            Grid.SetColumn(Pot,0);
            Grid.SetRow(Pot,3);
            Contenido.Children.Add(Pot);

            ListBox VistaElementos = new ListBox();
            VistaElementos.Name = "VistaElementos";
            VistaElementos.MaxHeight = VistaElementos.MinHeight= 280;
            VistaElementos.MaxWidth = VistaElementos.MinWidth = 300;
            VistaElementos.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            VistaElementos.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Bottom;
            Grid.SetColumn(VistaElementos,0);
            Grid.SetRow(VistaElementos,4);
            Contenido.Children.Add(VistaElementos);

            Button Nuevo = new Button();
            Nuevo.Name = "Nuevo";
            Nuevo.Content = "Nuevo Conjunto";
            Nuevo.Click += NuevoConjunto;
            Nuevo.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Bottom;
            Nuevo.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            Nuevo.FontSize = 14;
            Grid.SetColumn(Nuevo, 0);
            Grid.SetRow(Nuevo, 5);
            Contenido.Children.Add(Nuevo);

            // Segunda Columna

            Button Guardar = new Button();
            Guardar.Name = "Guardar";
            Guardar.Content = "Agregar Elemento al Conjunto";
            Guardar.Click += AgregarAConjunto;
            Guardar.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            Guardar.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            Guardar.FontSize = 14;
            Grid.SetColumn(Guardar, 1);
            Grid.SetRow(Guardar, 3);
            Contenido.Children.Add(Guardar);

            ListBox VistaPotencia = new ListBox();
            VistaPotencia.Name = "VistaPotencia";
            VistaPotencia.MaxHeight = VistaPotencia.MinHeight = 280;
            VistaPotencia.MaxWidth = VistaPotencia.MinWidth = 300;
            VistaPotencia.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            VistaPotencia.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Bottom;
            Grid.SetColumn(VistaPotencia, 1);
            Grid.SetRow(VistaPotencia, 4);
            Contenido.Children.Add(VistaPotencia);

            TextBlock Anotacion = new TextBlock();
            Anotacion.Name = "Anotacion";
            Anotacion.Text = "No se ha calculado el Conjunto Potencia.";
            Anotacion.FontStretch = Windows.UI.Text.FontStretch.Normal;
            Anotacion.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Bottom;
            Anotacion.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            Anotacion.FontSize = 16;
            Grid.SetColumn(Anotacion, 1);
            Grid.SetRow(Anotacion, 5);
            Contenido.Children.Add(Anotacion);
            AnimarGrid();
        }

        private void Inverso()
        {
            Contenido.Children.Clear();
            Contenido.RowDefinitions.Clear();
            for (int i = 0; i < 3; i++)
            {
                ColumnDefinition Nueva = new ColumnDefinition();
                Nueva.MaxWidth = 500;
                Contenido.ColumnDefinitions.Add(Nueva);
            }
            StackPanel Panel = new StackPanel();
            Panel.Name = "Panel";
            TextBlock Bienvenida = new TextBlock();
            Bienvenida.Text = "Hola, para poder trabajar, necesitamos que especifiques la dimension de Z(n)";
            Bienvenida.TextWrapping = TextWrapping.Wrap;
            Bienvenida.FontStretch = Windows.UI.Text.FontStretch.Normal;
            Bienvenida.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            Bienvenida.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            Bienvenida.FontSize = 24;
            Bienvenida.Height = 100;
            Panel.Children.Add(Bienvenida);
            TextBlock Dimension1 = new TextBlock();
            Dimension1.Text = "Por favor establezca la dimension de Z(n)";
            Dimension1.Name = "Dimension_lbl";
            Dimension1.FontSize = 20;
            Dimension1.Height = 30;
            Dimension1.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            Dimension1.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            Panel.Children.Add(Dimension1);
            TextBox Dimension = new TextBox();
            Dimension.Name = "Dimension";
            Dimension.TextChanged += BorrarTodo;
            Dimension.Width = 380;
            Panel.Children.Add(Dimension);
            TextBlock Elemento1 = new TextBlock();
            Elemento1.Margin = new Thickness(0, 30, 0, 0);
            Elemento1.Text = "Ahora Ingrese el Elemento a buscar.";
            Elemento1.TextWrapping = TextWrapping.Wrap;
            Elemento1.Name = "Elemento_lbl";
            Elemento1.FontSize = 20;
            Elemento1.Height = 30;
            Elemento1.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            Elemento1.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            Panel.Children.Add(Elemento1);
            TextBox Elemento = new TextBox();
            Elemento.Name = "Elemento";
            Elemento.TextChanged += BorrarTodo;
            Elemento.Width = 380;
            Panel.Children.Add(Elemento);
            Button Busqueda = new Button();
            Busqueda.Name = "Busqueda";
            Busqueda.Click += BuscarInversos;
            Busqueda.Content = "Buscar el Inverso en Z(n)";
            Busqueda.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            Busqueda.Width = 300;
            Busqueda.Height = 60;
            Busqueda.Margin = new Thickness(0, 50, 0, 0);
            Panel.Children.Add(Busqueda);
            Grid.SetColumn(Panel, 0);
            Contenido.Children.Add(Panel);

            StackPanel Panel2 = new StackPanel();
            Panel2.Name = "Panel2";
            Panel2.Margin = new Thickness(0, 100, 0, 0);
            TextBlock Class = new TextBlock();
            Class.Text = "Clase en Z(n)";
            Class.Name = "Class";
            Class.FontSize = 20;
            Class.Height = 30;
            Class.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            Class.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            Panel2.Children.Add(Class);
            TextBox Clase = new TextBox();
            Clase.Name = "Clase";
            Clase.IsReadOnly = true;
            Clase.Width = 380;
            Panel2.Children.Add(Clase);
            TextBlock Inverse = new TextBlock();
            Inverse.Margin = new Thickness(0, 30, 0, 0);
            Inverse.Text = "Inverso en Z(n)";
            Inverse.Name = "Inverse";
            Inverse.FontSize = 20;
            Inverse.Height = 30;
            Inverse.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            Inverse.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            Panel2.Children.Add(Inverse);
            TextBox Inverso = new TextBox();
            Inverso.Name = "Inverso";
            Inverso.IsReadOnly = true;
            Inverso.Width = 380;
            Panel2.Children.Add(Inverso);
            ToggleSwitch Abeliano = new ToggleSwitch();
            Abeliano.Name = "InversoTotal";
            Abeliano.OffContent = "No";
            Abeliano.OnContent = "Si";
            Abeliano.IsOn = false;
            Abeliano.Header = "¿Inverso para toda clase en Z(n)?";
            Abeliano.FontSize = 20;
            Abeliano.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            Abeliano.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            Abeliano.IsEnabled = false;
            Panel2.Children.Add(Abeliano);
            Grid.SetColumn(Panel2, 1);
            Contenido.Children.Add(Panel2);
            AnimarGrid();
        }

        private void BorrarTodo(object sender, TextChangedEventArgs e)
        {
            ((ToggleSwitch)FindName("InversoTotal")).IsOn = false;
            ((TextBox)FindName("Inverso")).Text = "";
            ((TextBox)FindName("Clase")).Text = "";
        }

        private void BuscarInversos(object sender, RoutedEventArgs e)
        {
            if (((TextBox)FindName("Dimension")).Text.Equals("") || ((TextBox)FindName("Elemento")).Text.Equals(""))
            {
                MuestraMsg("La Dimensión de la Relación de Equivalencia ni el elemento que desea buscar pueden ser vacios.");
            }
            else
            {
                if (!ComprobarEscrituraNumerica(((TextBox)FindName("Dimension")).Text) || !ComprobarEscrituraNumerica(((TextBox)FindName("Elemento")).Text))
                {
                    MuestraMsg("Asegurese de no ingresar Caracteres que NO sean numericos, esto podria impedir que las operaciones se realicen correctamente");
                }
                else
                {
                    int ElementoTotal = System.Convert.ToInt32(((TextBox)FindName("Elemento")).Text);

                    if (ElementoTotal < 0)
                    {
                        if (Math.Abs(ElementoTotal) <= (System.Convert.ToInt32(((TextBox)FindName("Dimension")).Text)))
                        {
                            ElementoTotal = (System.Convert.ToInt32(((TextBox)FindName("Dimension")).Text)) - Math.Abs(ElementoTotal);
                        }
                        else
                        {
                            ElementoTotal = 0;
                            MuestraMsg("No se ha podido hallar el Inverso para numeros negativos cuyo valor absoluto sea mayor que la Dimension de Z(n).");
                        }
                    }

                    ((ToggleSwitch)FindName("InversoTotal")).IsOn = PrimoN(System.Convert.ToInt32(((TextBox)FindName("Dimension")).Text));
                    ((TextBox)FindName("Clase")).Text = ((ElementoTotal)% (System.Convert.ToInt32(((TextBox)FindName("Dimension")).Text))).ToString();
                    if (MCD(System.Convert.ToInt32(((TextBox)FindName("Dimension")).Text),ElementoTotal) == 1)
                    {
                        int Inverse = (RetorneInverso(System.Convert.ToInt32(((TextBox)FindName("Dimension")).Text), ElementoTotal));

                        if (Inverse < 0)
                        {
                            Inverse = System.Convert.ToInt32(((TextBox)FindName("Dimension")).Text) + Inverse;
                        }

                        ((TextBox)FindName("Inverso")).Text =Inverse.ToString();
                    }
                    else
                    {
                        ((TextBox)FindName("Inverso")).Text = "No existe el Inverso Multiplicativo para esta clase.";
                    }
                }
            }
        }

        private int MCD(int Num1, int Num2)
        {
            if (Num2 == 0)
                return Num1;
            else
                return MCD(Num2, Num1 % Num2);
        }

        private bool ComprobarEscrituraNumerica(String Cadena)
        {
            try
            {
                int Numero = System.Convert.ToInt32(Cadena);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private bool PrimoN(int N)
        {
            int div = 0;
            for (int i = 1; i <= N / 2; i++)
            {
                if (N % i == 0)
                {
                    div++;
                }
            }
            if (div != 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        int RetorneInverso(int Num1,int Num2)
        {
          long[] array = new long[3];
          int x=0,y=0,x2 = 1,x1 = 0,y2 = 0,y1 = 1,q = 0, r = 0;
          if(Num2==0)
          {
          array[0]=Num1;
          array[1]=1; 
          array[2]=0;  
          }
          else
          {
           while(Num2>0) 
              {
              q = (Num1/Num2); 
              r = Num1 - q*Num2; 
              x = x2-q*x1; 
              y = y2 - q*y1; 
              Num1 = Num2; 
              Num2 = r; 
              x2 = x1; 
              x1 = x; 
              y2 = y1;             
              y1 = y; 
              }
              array[0] = Num1;
              array[1] = x2;
              array[2] = y2;
            }
            return y2;
        }
        
        private void Componer()
        {
            Contenido.Children.Clear();
            Contenido.RowDefinitions.Clear();
            for (int i = 0; i < 3; i++)
            {
                ColumnDefinition Nueva = new ColumnDefinition();
                Nueva.MaxWidth = 300;
                Contenido.ColumnDefinitions.Add(Nueva);
            }
            for (int i = 0; i < 4; i++)
            {
                RowDefinition Nueva = new RowDefinition();
                if (i == 2)
                {
                    Nueva.MaxHeight = 350;
                }
                else
                {
                    if (i == 0)
                    {
                        Nueva.MaxHeight = 100;
                    }
                    else
                    {
                        Nueva.MaxHeight = 40;
                    }
                  
                }
                Contenido.RowDefinitions.Add(Nueva);
            }

            //Columna 1

            StackPanel Panel1 = new StackPanel();
            Panel1.Name = "Panel1";

            TextBlock Introducir = new TextBlock();
            Introducir.Text = "Introduzca la pareja ordenada: ";
            Introducir.FontSize = 18;
            Introducir.Height = 20;
            Introducir.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            Introducir.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            Panel1.Children.Add(Introducir);
            TextBox Ingreso = new TextBox();
            Ingreso.Name = "Ingreso";
            Ingreso.Width = 200;
            Panel1.Children.Add(Ingreso);
            Button Agregar = new Button();
            Agregar.Name = "Agregar";
            Agregar.Click += AgregarPareja;
            Agregar.Content = "Agregar Pareja Ordenada";
            Agregar.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            Agregar.FontSize = 14;
            Agregar.Height = 40;
            Agregar.Margin = new Thickness(0, 5, 0, 0);
            Panel1.Children.Add(Agregar);
            Grid.SetColumn(Panel1, 0);
            Grid.SetRow(Panel1, 0);
            Contenido.Children.Add(Panel1);

            TextBlock RelacionS = new TextBlock();
            RelacionS.Text = "Relación S";
            RelacionS.FontSize = 16;
            RelacionS.Height = 20;
            RelacionS.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Bottom;
            RelacionS.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
            Grid.SetRow(RelacionS, 1);
            Grid.SetColumn(RelacionS, 0);
            Contenido.Children.Add(RelacionS);

            ListBox RelS = new ListBox();
            RelS.Name = "RelS";
            RelS.MaxHeight = RelS.MinHeight = 350;
            RelS.MaxWidth = RelS.MinWidth = 280;
            RelS.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
            RelS.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
            Grid.SetColumn(RelS, 0);
            Grid.SetRow(RelS, 2);
            Contenido.Children.Add(RelS);

            Button BorrarS = new Button();
            BorrarS.Name = "BorrarS";
            BorrarS.Click += RelacionSDelete;
            BorrarS.Content = "Borrar Relación S";
            BorrarS.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            BorrarS.FontSize = 16;
            BorrarS.Height = 40;
            BorrarS.Margin = new Thickness(0, 5, 0, 0);
            Grid.SetColumn(BorrarS, 0);
            Grid.SetRow(BorrarS, 3);
            Contenido.Children.Add(BorrarS);


            //Columna 2

            StackPanel Panel2 = new StackPanel();
            Panel2.Name = "Panel2";
            Panel2.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            RadioButton InsertaS = new RadioButton();
            InsertaS.Name = "InsertaS";
            InsertaS.Content = "Relación S";
            InsertaS.GroupName = "Relaciones";
            InsertaS.IsChecked = true;
            InsertaS.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
            Panel2.Children.Add(InsertaS);

            RadioButton InsertaR = new RadioButton();
            InsertaR.Name = "InsertaR";
            InsertaR.Content = "Relación R";
            InsertaR.GroupName = "Relaciones";
            InsertaR.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
            Panel2.Children.Add(InsertaR);
            Grid.SetColumn(Panel2, 1);
            Grid.SetRow(Panel1, 0);
            Contenido.Children.Add(Panel2);

            TextBlock RelacionR = new TextBlock();
            RelacionR.Text = "Relación R";
            RelacionR.FontSize = 16;
            RelacionR.Height = 20;
            RelacionR.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Bottom;
            RelacionR.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
            Grid.SetRow(RelacionR, 1);
            Grid.SetColumn(RelacionR, 1);
            Contenido.Children.Add(RelacionR);

            ListBox RelR = new ListBox();
            RelR.Name = "RelR";
            RelR.MaxHeight = RelR.MinHeight = 350;
            RelR.MaxWidth = RelR.MinWidth = 280;
            RelR.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
            RelR.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
            Grid.SetColumn(RelR, 1);
            Grid.SetRow(RelR, 2);
            Contenido.Children.Add(RelR);

            Button BorrarR = new Button();
            BorrarR.Name = "BorrarR";
            BorrarR.Click += RelacionRDelete;
            BorrarR.Content = "Borrar Relación R";
            BorrarR.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            BorrarR.FontSize = 16;
            BorrarR.Height = 40;
            BorrarR.Margin = new Thickness(0, 5, 0, 0);
            Grid.SetColumn(BorrarR, 1);
            Grid.SetRow(BorrarR, 3);
            Contenido.Children.Add(BorrarR);


            //Columna 3

            StackPanel Panel3 = new StackPanel();
            Panel3.Name = "Panel3";
            Panel3.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            
            RadioButton ComponeS = new RadioButton();
            ComponeS.Name = "ComponeS";
            ComponeS.Content = "R•S";
            ComponeS.GroupName = "Composiciones";
            ComponeS.IsChecked = true;
            ComponeS.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
            Panel3.Children.Add(ComponeS);

            RadioButton ComponeR = new RadioButton();
            ComponeR.Name = "ComponeR";
            ComponeR.Content = "S•R";
            ComponeR.GroupName = "Composiciones";
            ComponeR.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
            Panel3.Children.Add(ComponeR);
            Grid.SetColumn(Panel3, 2);
            Grid.SetRow(Panel1, 0);
            Contenido.Children.Add(Panel3);

            TextBlock ComposicionTotal = new TextBlock();
            ComposicionTotal.Text = "Composición";
            ComposicionTotal.FontSize = 16;
            ComposicionTotal.Height = 20;
            ComposicionTotal.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Bottom;
            ComposicionTotal.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
            Grid.SetRow(ComposicionTotal, 1);
            Grid.SetColumn(ComposicionTotal, 2);
            Contenido.Children.Add(ComposicionTotal);

            ListBox Compuesto = new ListBox();
            Compuesto.Name = "Compuesto";
            Compuesto.MaxHeight = Compuesto.MinHeight = 350;
            Compuesto.MaxWidth = Compuesto.MinWidth = 280;
            Compuesto.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
            Compuesto.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
            Grid.SetColumn(Compuesto, 2);
            Grid.SetRow(Compuesto, 2);
            Contenido.Children.Add(Compuesto);

            Button BorrarCompuesto = new Button();
            BorrarCompuesto.Name = "BorrarCompuesto";
            BorrarCompuesto.Click += HallarComposicion;
            BorrarCompuesto.Content = "Composición de las 2 Relaciones";
            BorrarCompuesto.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            BorrarCompuesto.FontSize = 16;
            BorrarCompuesto.Height = 40;
            BorrarCompuesto.Margin = new Thickness(0, 5, 0, 0);
            Grid.SetColumn(BorrarCompuesto, 2);
            Grid.SetRow(BorrarCompuesto, 3);
            Contenido.Children.Add(BorrarCompuesto);

            AnimarGrid();
        }

        private void IrAInicio(object sender, RoutedEventArgs e)
        {
            PaginaInicial();
            AnimarGrid();
        }

    }
}
