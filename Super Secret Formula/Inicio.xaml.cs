using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class Inicio : Super_Secret_Formula.Common.LayoutAwarePage
    {
        bool Check1;
        bool Check2;
        int posicion;
        public Inicio()
        {
            this.InitializeComponent();
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

        DateTimeOffset Inicio1;
        DateTimeOffset Ultimo;
        DateTimeOffset Detener;
        int Ticks = 10;
        int Veces = 20;
        DispatcherTimer Temporizador;

        private void Animar()
        {
            Ticks = 1;
            Temporizador= new DispatcherTimer();
            Temporizador.Tick+=Temporizador_Tick;
            Temporizador.Interval = new TimeSpan(10);
            Inicio1 = DateTimeOffset.Now;
            Ultimo = Inicio1;
            Temporizador.Start();
        }

        private void Temporizador_Tick(object sender, object e)
        {
            DateTimeOffset Tiempo = DateTimeOffset.Now;
            TimeSpan Intervalo = Tiempo - Ultimo; 
            Ultimo = Tiempo;
            Anillo.IsActive = true;
            Ticks++;
            if (Ticks > Veces)
            {
                Detener = Tiempo;
                Temporizador.Stop();
                Intervalo = Detener - Inicio1;
                Anillo.IsActive = false;
            }
        }

        private async void MuestraMsg(String Msj)
        {
            var Mensaje = new Windows.UI.Popups.MessageDialog(Msj, "Super Secret Formula!");
            await Mensaje.ShowAsync();
        }

        private bool ComprobarEscrituraPolaca(String Cadena)
        {
            bool Switche = false;
            int i = 0;
            while( i< Cadena.Length && !Switche)
            {
                if (Cadena.Substring(i, 1).Equals("¬") || Cadena.Substring(i, 1).Equals("(") || Cadena.Substring(i, 1).Equals(")") || Cadena.Substring(i, 1).Equals("=") || Cadena.Substring(i, 1).Equals(">") || Cadena.Substring(i, 1).Equals("^") || Cadena.Substring((Cadena.Length)-1,1).Equals("N"))
                {
                    Switche = true;
                }
                else
                {
                    i++;
                }
            }
            return Switche;
        }

        private int DecisionII(String Cadena)
        {
            int Decision = 0;

            for (int i = 0; i < Cadena.Length; i++)
            {
                if (Cadena.Substring(i, 1).Equals("N"))
                {
                    Decision += 0;
                }
                else
                {
                    if (Cadena.Substring(i, 1).Equals("A") || Cadena.Substring(i, 1).Equals("K") || Cadena.Substring(i, 1).Equals("C") || Cadena.Substring(i, 1).Equals("E"))
                    {
                        Decision++;
                    }
                    else
                    {
                        Decision--;
                    }
                }
            }
            return Decision;
        }

        private void Comprueba_Click(object sender, RoutedEventArgs e)
        {
            Check1 = true; Check2 = true;
            if (!Input.Text.Equals(""))
            {
                SwitchLogico.IsOn = false;
                Animar();
                if (System.Convert.ToBoolean(Polaca_sw.IsChecked))
                {
                    if (ComprobarEscrituraPolaca(Input.Text))
                    {
                        MuestraMsg("La Cadena en Notación polaca no puede contener parentesis, Negaciones de la forma \"¬\". Asegurese de ingresar un cadena que cumple con estas condiciones.");
                    }
                    else
                    {
                        if (DecisionII(Input.Text) == -1)
                        {
                            SwitchLogico.IsOn = true;
                            Notado_Polaca.Text = Infija(Input.Text);
                        }
                        else
                        {
                            MuestraMsg("La Cadena ingresada, no es formula proposicional.");
                            Notado_Polaca.Text = "No se ha podido convertir de Manera Exitosa.";
                        }
                    }
                }
                else
                {
                    String Cadenita = Input.Text;
                    Cadenita = ConvertirACeros(Cadenita);
                    if ((Cadenita.Length) > 1)
                    {
                        while (Check1 == true || Check2 == true)
                        {
                            Cadenita = Decision(Cadenita);
                        }
                        if (Cadenita.Equals("0"))
                        {
                            SwitchLogico.IsOn = true;
                            EncenderConectivo(ConectivoPpal(ConvertirACeros(Input.Text)), posicion);
                            Notado_Polaca.Text = Polaca(CambiarNegaciones(Input.Text));
                            if (tautologia(Input.Text))
                            {
                                Tautologia.IsOn = true;
                            }
                        }
                        else
                        {
                            SwitchLogico.IsOn = false;
                            MuestraMsg("La Cadena ingresada, no es formula proposicional.");
                        }
                    }
                    else
                    {
                        if ((ConvertirACeros(Cadenita)).Equals("0"))
                        {
                            SwitchLogico.IsOn = true;
                            Notado_Polaca.Text = Polaca(CambiarNegaciones(Input.Text));
                        }
                        else
                        {
                            SwitchLogico.IsOn = false;
                            MuestraMsg("La Cadena ingresada, no es formula proposicional.");
                        }
                    }
                }
            }
            else
            {
                MuestraMsg("La Cadena ingresada no puede ser Vacía");
            }
        }

        private void EncenderConectivo(String Conectivo, int pos)
        {
            if (Conectivo.Equals("¬"))
            {
                Negacion.IsChecked = true;
            }
            else
            {
                if (Conectivo.Equals("^"))
                {
                    Conjuncion.IsChecked = true;
                }
                else
                {
                    if (Conectivo.Equals("V"))
                    {
                        Disyuncion.IsChecked = true;
                    }
                    else
                    {
                        if (Conectivo.Equals(">"))
                        {
                            Condicional.IsChecked = true;
                        }
                        else
                        {
                            Bicondicional.IsChecked = true;
                        }
                    }
                }
            }
            Posicionar.Text = pos.ToString();
        }

        private void Actualizar(object sender, TextChangedEventArgs e)
        {
            Tautologia.IsOn = false;
            SwitchLogico.IsOn = false; 
            Negacion.IsChecked = false;
            Conjuncion.IsChecked = false;
            Disyuncion.IsChecked = false;
            Condicional.IsChecked = false;
            Bicondicional.IsChecked = false;
            Posicionar.Text = "";
            Notado_Polaca.Text = "";
            Polaca_sw.IsChecked = false;
            if(!Input.Text.Equals(""))
            {
                Input.Text = Input.Text.ToUpper();
                Input.Select(Input.Text.Length, 0);
            }
        }

        private bool IsNegable(String Cadena)
		{
			bool Negable=false;
			int i=0;
			while (i<Cadena.Length && !Negable)
			{
				if(Cadena.Substring(i,1).Equals("¬"))
				{
					Negable=true;
				}
				i++;
			}
			return Negable;
		}

        private bool IsBinary(String Cadena)
		{
			bool Binario=false;
			int tamaño=Cadena.Length;
			int i=0;
			while (i<Cadena.Length)
			{
				if(Cadena.Length<5)
				{
					Binario=false;
				}
				else
				{
					if(Cadena.Length==5) 
					{
						if (Cadena.Substring(0,1).Equals("(") && Cadena.Substring(tamaño-1,1).Equals(")"))
						{
							if ((Cadena.Substring(1,1).Equals("0") && Cadena.Substring(3,1).Equals("0")) && ((!Cadena.Substring(2,1).Equals("¬") && !Cadena.Substring(2,1).Equals("0") && !Cadena.Substring(2,1).Equals(")") && !Cadena.Substring(2,1).Equals("("))))
							{
								Binario=true;
							}
							else
							{
								Binario=false;
							}
						}
					}
					else
					{
						try{
						if(Cadena.Substring(i,5).StartsWith("(") && Cadena.Substring(i,5).EndsWith(")"))
						{
							if((Cadena.Substring(i+1,1).Equals("0") && Cadena.Substring(i+3,1).Equals("0")) && ((!Cadena.Substring(i+2,1).Equals("¬") && !Cadena.Substring(i+2,1).Equals("0") && !Cadena.Substring(i+2,1).Equals(")") && !Cadena.Substring(i+2,1).Equals("("))))
							{
								int t=Cadena.Substring(0,i+1).Length;
								Cadena=(Cadena.Substring(0,t-1)+"0"+Cadena.Substring(t+4,tamaño-t-4));
								tamaño=Cadena.Length;
								Binario=true;
							}
						}
						}catch(Exception) { break; }
					}
				}
				i++;
			}
			return Binario;
		}

		private String Decision(String Cadena)
		{
			if(Cadena.Equals(ComprobarNegaciones(Cadena))) Check1=false;
			else Cadena=ComprobarNegaciones(Cadena);
			if(Cadena.Equals(ComprobarBinarios(Cadena))) Check2=false;
			else Cadena=ComprobarBinarios(Cadena);
            if (IsNegable(Cadena))
                Check1 = true;
            else
                Check1 = false;
			return Cadena;
		}

        private String ConvertirACeros(String Cadena)
		{
			String Cadena2="";
			for (int i = 0; i < Cadena.Length; i++)
			{
				if (!Cadena.Substring(i,1).Equals("(")) 	
				{
					if(!Cadena.Substring(i,1).Equals(")"))
					{
						if (!Cadena.Substring(i,1).Equals("V"))
						{
							if (!Cadena.Substring(i,1).Equals("^"))
							{
								if (!Cadena.Substring(i,1).Equals("¬"))
								{
									if (!Cadena.Substring(i,1).Equals(">"))
									{
										if (!Cadena.Substring(i,1).Equals("="))
										{
											Cadena2+="0";
										}
										else
										{
											Cadena2+=Cadena.Substring(i,1);
										}
									}
									else
									{
										Cadena2+=Cadena.Substring(i,1);
									}
								}
								else
								{
									Cadena2+=Cadena.Substring(i,1);
								}
							}
							else
							{
								Cadena2+=Cadena.Substring(i,1);
							}
						}
						else
						{
							Cadena2+=Cadena.Substring(i,1);
						}
					}
					else
					{
						Cadena2+=Cadena.Substring(i,1);
					}
				}
				else
				{
					Cadena2+=Cadena.Substring(i,1);
				}
			}
			return Cadena2;
		}

        private String ConectivoPpal(String Cadena)
		{
            this.posicion = 1;
			String ConectivoForm;
			Check1=true; Check2=true;
			if(Cadena.StartsWith("¬"))
			{
				ConectivoForm="¬";
			}
			else
			{
				while (!Cadena.StartsWith("(0"))
				{
					if(Check1)
					{
						if(!Cadena.Equals(ComprobarNegaciones(Cadena)))
						{
							Cadena=ComprobarNegaciones(Cadena);
							this.posicion ++;
						}
					}
                    if (Cadena.StartsWith("(0"))
                    {
                        Check2 = false;
                    }
                    else
                    {
                        if (IsNegable(Cadena))
                            Check1 = true;
                        else
                            Check1 = false;
                    }
					if (Check2)
					{
						if(Cadena.StartsWith("(0"))
						{
                            this.posicion++;
							Check2=false;
						}
						else
						{
                            if (!Cadena.Equals(ComprobarBinarios(Cadena)))
                            {
                                Cadena = ComprobarBinarios(Cadena);
                                this.posicion += 4;
                            }
						}
					}
				}
				this.posicion+=2;
				ConectivoForm=Cadena.Substring(2,1);
			}
			return ConectivoForm;
		}

        private String ComprobarBinarios(String Cadena)
		{
			int i=0;bool cambio=false;
			int tamaño=Cadena.Length;
			while (i<tamaño && !cambio)
			{
				if(Cadena.Length<5)
				{
                    cambio = true;
                    break;
				}
				else
				{
					if(Cadena.Length==5) 
					{
						if (Cadena.Substring(0,1).Equals("(") && Cadena.Substring(tamaño-1,1).Equals(")"))
						{
							if ((Cadena.Substring(1,1).Equals("0") && Cadena.Substring(3,1).Equals("0")) && ((!Cadena.Substring(2,1).Equals("¬") && !Cadena.Substring(2,1).Equals("0") && !Cadena.Substring(2,1).Equals(")") && !Cadena.Substring(2,1).Equals("("))))
							{
								Cadena="0";
								cambio=true;
							}
						}
					}
					else
					{
						try{
						if(Cadena.Substring(i,5).StartsWith("(") && Cadena.Substring(i,5).EndsWith(")"))
						{
							if((Cadena.Substring(i+1,1).Equals("0") && Cadena.Substring(i+3,1).Equals("0")) && ((!Cadena.Substring(i+2,1).Equals("¬") && !Cadena.Substring(i+2,1).Equals("0") && !Cadena.Substring(i+2,1).Equals(")") && !Cadena.Substring(i+2,1).Equals("("))))
							{
								int t=Cadena.Substring(0,i+1).Length;
								Cadena=(Cadena.Substring(0,t-1)+"0"+Cadena.Substring(t+4,tamaño-t-4));
								tamaño=Cadena.Length;
								cambio=true;
							}
						}
						}catch(Exception) { break; }
					}
				}
				i++;
			}
			return Cadena;
		}

        private String ComprobarNegaciones(String Cadena)
		{
			int i=0; bool cambio=false;
			int tamaño=Cadena.Length;
			while (i<tamaño && !cambio)
			{
				if(Cadena.Length==1)
				{
                    cambio = true;
					break;
				}
				else
				{
					if(Cadena.Length==2)
					{
						if (Cadena.Substring(0,1).Equals("¬") && Cadena.Substring(1,1).Equals("0"))
						{
							Cadena="0";
							cambio=true;
						}
					}
					else
					{
						try{
							if(Cadena.Substring(i,1).Equals("¬") && Cadena.Substring(i+1,1).Equals("0"))
							{
								int t=Cadena.Substring(0,i+1).Length;
								Cadena=(Cadena.Substring(0,t-1)+"0"+Cadena.Substring(t+1,tamaño-(t+1)));
								tamaño=Cadena.Length;
								cambio=true;
							}
						}catch(Exception) { break;} 
					}
				}
				i++;
			}
			return Cadena;
		}

        private void Refrescar(object sender, RoutedEventArgs e)
        {
            Input.Text = "";
            Windows.UI.Xaml.Media.Animation.Storyboard Pop = FindName("PopIn") as Windows.UI.Xaml.Media.Animation.Storyboard;
            if (Pop != null)
            {
                Pop.Begin();
            }
        }

        private String CambiarNegaciones(String Cadena)
        {
            String Cadenita="";
            int i = 0;
            while (i < Cadena.Length)
            {
                if (Cadena.Substring(i, 1).Equals("¬"))
                {
                    Cadenita += "N";
                }
                else
                {
                    Cadenita += Cadena.Substring(i, 1);
                }
                i++;
            }
            return Cadenita;
        }

        private String Polaca(String Cadena)
        {
            String Notacion = "";
            int i = 0;
            while(i<Cadena.Length)
            {
                if (Cadena.Substring(i, 1).Equals("^") || Cadena.Substring(i, 1).Equals("V") || Cadena.Substring(i, 1).Equals(">") || Cadena.Substring(i, 1).Equals("="))
                {
                    int j = i;
                    bool Parentesis=false;
                    while (j >= 0 && !Parentesis)
                    {
                        if (Cadena.Substring(j, 1).Equals("("))
                        {
                            String Cad = "";
                            for (int K = 0; K < Cadena.Length; K++)
                            {
                                if (K == j)
                                {
                                    if (Cadena.Substring(i, 1).Equals("^"))
                                    {
                                        Cad += "K";
                                    }
                                    else
                                    {
                                        if (Cadena.Substring(i, 1).Equals("V"))
                                        {
                                            Cad += "A";
                                        }
                                        else
                                        {
                                            if (Cadena.Substring(i, 1).Equals(">"))
                                            {
                                                Cad += "C";
                                            }
                                            else
                                            {
                                                Cad += "E";
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (K == i)
                                    {
                                        Cad += "";
                                    }
                                    else
                                    {
                                        Cad += Cadena.Substring(K, 1);
                                    }
                                }
                            }
                            Cadena = Cad;
                            Parentesis = true;
                        }
                        j--;
                    }
                }
                i++;
            }
            i=0;
            while (i < Cadena.Length)
            {
                if (!Cadena.Substring(i, 1).Equals(")"))
                {
                    Notacion += Cadena.Substring(i, 1);
                }
                i++;
            }
            return Notacion ;
        }

        private void AcercaDe(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Acerca_de));
        }

        private void Comienzo(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SelectorTareas), "TodosLosGrupos");
        }

        private void CambiarTituloI(object sender, RoutedEventArgs e)
        {
            Notacion_Polaca.Text = "Notación Infija de la Formula";
        }

        private void CambiarTituloII(object sender, RoutedEventArgs e)
        {
            Notacion_Polaca.Text = "Notación Polaca de la Formula";
        }

        private int BuscaUltimaPosicion(String Cadena)
        {
            int pos = 0,i=0;
            bool sw=true;
            while(i<Cadena.Length && sw)
            {
                if (Cadena.Substring(i, 1).Equals("A") || Cadena.Substring(i, 1).Equals("K") || Cadena.Substring(i, 1).Equals("C") || Cadena.Substring(i, 1).Equals("E"))
                {
                    pos = i;
                }
                else
                {
                    if (Cadena.Substring(i, 1).Equals("("))
                    {
                        sw = false;
                    }
                }
                i++;
            }
            return pos;
        }

        private String DefineConectivo(int X, String Cadena)
        {
            string Conectivo = "";
            switch (Cadena.Substring(X, 1))
            {
                case "A":
                    Conectivo = "V";
                    break;
                case "K":
                    Conectivo = "^";
                    break;
                case "E":
                    Conectivo = "=";
                    break;
                case "C":
                    Conectivo = ">";
                    break;
                case "N":
                    Conectivo = "¬";
                    break;
                default:
                    break;
            }
            return Conectivo;
        }

        private bool ExistenConectivo(String Cadena)
        {
            bool Existencia = false;
            int i=0;
            while (i < Cadena.Length && !Existencia)
            {
                if (Cadena.Substring(i, 1).Equals("A") || Cadena.Substring(i, 1).Equals("K") || Cadena.Substring(i, 1).Equals("C") || Cadena.Substring(i, 1).Equals("E"))
                {
                    Existencia = true;
                }
                else
                {
                    i++;
                }
            }
            return Existencia;
        }

        private String Infija(String Cadena)
        {
            while (ExistenConectivo(Cadena))
            {
                String Cad = "";
                for (int i = 0; i < BuscaUltimaPosicion(Cadena); i++)
                {
                    Cad += Cadena.Substring(i, 1);
                }
                Cad += "(";
                int j = BuscaUltimaPosicion(Cadena) + 1;
                int Parentesis = 0;
                int Letras = 0;
                //bool CasoEspecial = false;
                do
                {
                    Cad += Cadena.Substring(j, 1);
                    if (!Cadena.Substring(j, 1).Equals("A") && !Cadena.Substring(j, 1).Equals("C") && !Cadena.Substring(j, 1).Equals("E") && !Cadena.Substring(j, 1).Equals("K") && !Cadena.Substring(j, 1).Equals("(") && !Cadena.Substring(j, 1).Equals(")") && !Cadena.Substring(j, 1).Equals("N"))
                    {
                        Letras++;
                    }
                    else
                    {
                        if (Cadena.Substring(j, 1).Equals("("))
                        {
                            Parentesis++;
                            Letras -= 3;
                        }
                        else
                        {
                            if (Cadena.Substring(j, 1).Equals(")"))
                            {
                                Parentesis--;
                                Letras -= 2;
                            }
                            else
                            {
                                do
                                {
                                    Cad += Cadena.Substring(j, 1);
                                    j++;
                                } while (Cadena.Substring(j + 1, 1).Equals("N"));
                            }
                        }
                    }
                    //if (Letras == 1 & Parentesis == 0)
                    //{
                    //    CasoEspecial = true;
                    //}
                    j++;
                } while (Letras < 1 && Parentesis != 0 && j < Cadena.Length);
                //if (CasoEspecial)
                //{
                //    Cad += Cadena.Substring(j, 1);
                //}
                Cad += DefineConectivo(BuscaUltimaPosicion(Cadena), Cadena);
                Parentesis = 0;
                Letras = 0;
                bool termina = false;
                while (j < Cadena.Length && !termina)
                {
                    if (Cadena.Substring(j, 1).Equals("("))
                    {
                        Parentesis++;
                        Letras -= 3;
                    }
                    else
                    {
                        if (Cadena.Substring(j, 1).Equals(")"))
                        {
                            Parentesis--;
                        }
                        else
                        {
                            if(Cadena.Substring(j, 1).Equals("N"))
                            {
                                do
                                {
                                    Cad += Cadena.Substring(j, 1);
                                    j++;
                                } while (Cadena.Substring(j+1, 1).Equals("N")) ;
                            }
                            else
                            {
                                Letras++;
                            }
                        }
                    }
                    Cad += Cadena.Substring(j, 1);
                    j++;
                    if (Letras == 2 || Parentesis== 0)
                    {
                        termina = true;
                    }
                }
                Cad += ")";
                while (j < Cadena.Length)
                {
                    Cad += Cadena.Substring(j, 1);
                    j++;
                }
                Cadena = Cad;
            }
            if (!ExistenConectivo(Cadena))
            {
                String Cad = "";
                for (int i = 0; i < Cadena.Length; i++)
                {
                    if (Cadena.Substring(i, 1).Equals("N"))
                    {
                        Cad += DefineConectivo(i, Cadena);
                    }
                    else
                    {
                        Cad += Cadena.Substring(i, 1);
                    }
                }
                Cadena = Cad;
            }
            return Cadena;
        }

        private Char TablaVerdad(String val1, String val2, Char conectivo)
        {
	        switch(conectivo)
            {
		        case '^': if(val1.Equals("1") && val2.Equals("1")) return '1'; else return '0';
                case 'V': if (val1.Equals("0") && val2.Equals("0")) return '0'; else return '1';
                case '>': if (val1.Equals("1") && val2.Equals("0")) return '0'; else return '1';
                case '=': if (!val1.Equals(val2)) return '0'; else return '1';
                default: return '0';
            }
        }
        
        private String CambiarNegaciones2(String Cadena)
        {
            String Cadenita = "";
            int i = 0;
            int tam = Cadena.Length;

            while (i < tam)
            {
                if (Cadena.Substring(i, 1).Equals("¬"))
                {
                    if (Cadena.Substring(i + 1, 1).Equals("0"))
                    {
                        Cadenita += "1";
                        i++;
                    }
                    else
                    {
                        if (Cadena.Substring(i + 1, 1) == "1")
                        {
                            Cadenita += "0";
                            i++;
                        }
                        else
                        {
                            Cadenita += Cadena.Substring(i, 1);
                        }
                    }
                }
                else
                {
                    Cadenita += Cadena.Substring(i, 1);
                }
                i++;
            }
            return Cadenita;
        }

        private String reducir(String cadena)
        {
	        while(cadena.Length>1)
	        {
		        cadena=CambiarNegaciones2(cadena);
		        cadena=CambiarOperaciones(cadena);
	        }
            return cadena;
        }

        private String convertira(String Cadena, String elem, String nelem)
        {
	        String Cadena2="";
            for (int i = 0; i < Cadena.Length; i++)
            {
                if (Cadena.Substring(i, 1).Equals(elem))
                {
                    Cadena2 += nelem;
                }
                else
                {
                    Cadena2 += Cadena.Substring(i, 1);
                }
            }
	        return Cadena2;
        }

        private bool tautologia(String cadena)
        {
            String  cadena1 = convertira(cadena, "P", "0");
            cadena1 = convertira(cadena1, "Q", "0");
            String  cadena2 = convertira(cadena, "P", "0");
            cadena2 = convertira(cadena2, "Q", "1");
            String  cadena3 = convertira(cadena, "P", "1");
            cadena3 = convertira(cadena3, "Q", "0");
            String  cadena4 = convertira(cadena, "P", "1");
            cadena4 = convertira(cadena4, "Q", "1");

	        bool solucion=false;
	
	        String d1=reducir(cadena1);
	        String d2=reducir(cadena2);
	        String d3=reducir(cadena3);
	        String d4=reducir(cadena4);

	        if(d1.Equals("1") && d2.Equals("1") && d3.Equals("1") && d4.Equals("1"))
	        {
		        solucion=true;
	        }
            return solucion;
        }
         
        private String CambiarOperaciones(String cadena)
        {
	        String cadenita="";
            int tam=cadena.Length;
	        if(tam<5)
	        {
		        return cadena;
	        }
	        else
	        {
		        if(tam==5)
		        {
			        cadenita+=TablaVerdad(cadena.Substring(1,1),cadena.Substring(3,1),System.Convert.ToChar(cadena.Substring(2,1)));
		        }
		        else
		        {   
                    int i=0;
                    while(i<tam)
                    {
	                    if(cadena.Substring(i,1).Equals("("))
	                    {
		                    if(cadena.Substring(i+1,1).Equals("0") || cadena.Substring(i+1,1).Equals("1"))
		                    {
			                    if(cadena.Substring(i+2,1).Equals("V") || cadena.Substring(i+2,1).Equals("^") ||cadena.Substring(i+2,1).Equals(">") ||cadena.Substring(i+2,1).Equals("=") )
			                    {
				                    if(cadena.Substring(i+3,1).Equals("1") || cadena.Substring(i+3,1).Equals("0"))
				                    {
					                    if(cadena.Substring(i+4,1).Equals(")"))
					                    {
                                            cadenita += TablaVerdad(cadena.Substring(i + 1, 1), cadena.Substring(i + 3, 1),System.Convert.ToChar(cadena.Substring(i + 2, 1)));
						                    i=i+5;
					                    }
					                    else
					                    {
						                    cadenita+=cadena.Substring(i,1);
						                    i++;
					                    }
				                    }
				                    else
				                    {
					                    cadenita+=cadena.Substring(i,1);
					                    i++;
				                    }		
			                    }
			                    else
			                    {
				                    cadenita+=cadena.Substring(i,1);
				                    i++;
			                    }
		                    }
		                    else
		                    {
			                    cadenita+=cadena.Substring(i,1);
			                    i++;
		                    }	
	                    }
	                    else
	                    {
		                    cadenita+=cadena.Substring(i,1);
		                    i++;
	                    }
                    }
                }
	        }
            return cadenita;
        }

    }
}