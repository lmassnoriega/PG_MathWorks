using MathWorks.Core.Coding;
using MathWorks.Core.Communication;
using MathWorks.Core.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace MathWorks.Test
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		#region Handlers

		private void button_Click(object sender, RoutedEventArgs e)
		{
			if (Convert.ToBoolean(option1.IsChecked))
			{
				Option1();
			}
			else
			{
				if (Convert.ToBoolean(option2.IsChecked))
				{
					Option2();
				}
				else
				{
					if (Convert.ToBoolean(option3.IsChecked))
					{
						Option3();
					}
					else
					{
						Option4();
					}
				}
			}
		}

		private void AddToAlphabet(object sender, RoutedEventArgs e)
		{
			if (!String.IsNullOrWhiteSpace(Digit.Text))
			{
				if (!Alphabet.Items.Contains(Digit.Text.ToUpper()))
				{
					Alphabet.Items.Add(Digit.Text.ToUpper());
				}
			}
		}

		private void DeleteFromAlphabet(object sender, RoutedEventArgs e)
		{
			if (Alphabet.SelectedIndex != -1)
			{
				Alphabet.Items.RemoveAt(Alphabet.SelectedIndex);
			}
		}

		private void ClearAlphabet(object sender, RoutedEventArgs e)
		{
			Alphabet.Items.Clear();
		}

		#endregion
		
		#region Options

		public void Option1()
		{
			EuclideanAlgorithm client = new EuclideanAlgorithm();
			if (!String.IsNullOrWhiteSpace(Number1.Text) && !String.IsNullOrWhiteSpace(Number2.Text))
			{
				BigInteger A = BigInteger.Parse(Number1.Text);
				BigInteger B = BigInteger.Parse(Number2.Text);
				List<BigInteger> res = client.Calculate(A, B);
				GCD.Text = "" + res[0];
				X.Text = "" + res[1];
				Y.Text = "" + res[2];
			}
			else
			{
				Message("Los cuadros de texto de números a comprobar no pueden ser vacíos.", 3);
			}
		}

		public void Option2()
		{
			RSA client = new RSA();
			bool canDoBlock = false;
			if (Convert.ToBoolean(RSA_Block.IsChecked))
			{
				if (Alphabet.Items.Count == 0)
				{
					Message("No hay alfabeto definido para cifrar",3);
				}
				else
				{
					for (int i = 0; i < Alphabet.Items.Count; i++)
					{
						client.Alphabet.Add(Alphabet.Items[i].ToString());
					}
					canDoBlock = true;
				}
			}
			
			// Check common variables
			if (Convert.ToBoolean(encryptOption.IsChecked))
			{
				if (!String.IsNullOrWhiteSpace(NumberP.Text) && !String.IsNullOrWhiteSpace(NumberQ.Text) && !String.IsNullOrWhiteSpace(NumberE.Text))
				{
					client.P = BigInteger.Parse(NumberP.Text);
					client.Q = BigInteger.Parse(NumberQ.Text);
					NumberN.Text = "" + client.N();
					client.E = BigInteger.Parse(NumberE.Text);

					if (Convert.ToBoolean(RSA_Plane.IsChecked))
					{
						if (!String.IsNullOrWhiteSpace(PlainText.Text))
						{
							if (BigInteger.Parse(PlainText.Text).CompareTo(client.N())<0)
							{
								//TODO: Cifrar texto plano
								TextEncrypt.Text = "" + client.Encrypt(BigInteger.Parse(PlainText.Text));
							}
							else
							{
								Message("El texto plano debe estar entre 0 y " + BigInteger.Subtract(client.N(),1),2);
							}
						}
						else
						{
							Message("El texto a cifrar no puede ser vacío", 2);
						}
					}
					else
					{
						//Block coding

						if (!String.IsNullOrWhiteSpace(PlainBlock.Text) && canDoBlock)
						{
							if (client.CheckText(PlainBlock.Text))
							{
								//TODO Encrypt Block
								BlockEncrypt.Text = client.Encrypt(PlainBlock.Text);
							}
							else
							{
								Message("El bloque a cifrar contiene caracteres que no estan en el alfabeto", 2);
							}
						}
						else
						{
							Message("El bloque a cifrar no puede ser vacío", 2);
						}
					}
				}
			}

			if (Convert.ToBoolean(decryptOption.IsChecked))
			{
				if (!String.IsNullOrWhiteSpace(NumberE.Text) && !String.IsNullOrWhiteSpace(NumberN.Text))
				{
					client.E = BigInteger.Parse(NumberE.Text);
					List<BigInteger> factors = PrimeHelper.FindFactors(BigInteger.Parse(NumberN.Text));
					client.P = factors[0];
					client.Q = factors[1];

					if (Convert.ToBoolean(RSA_Plane.IsChecked))
					{
						if (!String.IsNullOrWhiteSpace(CipherText.Text))
						{
							if (BigInteger.Parse(CipherText.Text).CompareTo(client.N()) < 0)
							{
								PlainDecrypt.Text = "" + client.Decrypt(BigInteger.Parse(CipherText.Text));
							}
							else
							{
								Message("El texto cifrado debe estar entre 0 y " + BigInteger.Subtract(client.N(), 1), 2);
							}
						}
						else
						{
							Message("El texto a descifrar no puede ser vacío", 2);
						}
					}
					else
					{
						//Block coding

						if (!String.IsNullOrWhiteSpace(CipherBlock.Text) && canDoBlock)
						{
							if (client.CheckText(CipherBlock.Text))
							{
								//TODO Decrypt Block
								BlockDecrypt.Text = client.Decrypt(CipherBlock.Text);

							}
							else
							{
								Message("El bloque cifrado contiene caracteres que no estan en el alfabeto", 2);
							}
						}
						else
						{
							Message("El bloque a descifrar no puede ser vacío", 2);
						}
					}
				}
				else
				{
					Message("Faltan datos para poder descifrar bloques o palabras",2);
				}
			}
		}

		public void Option3()
		{
			ChecksumDigit.Text = ""+EANHelper.CheckSumEAN13(MainCode.Text);
			String eanCode = MainCode.Text + ChecksumDigit.Text;
			image.Source = null;
			image.Source = BitmapToImageSource(EANHelper.GenerateBarCode(eanCode));
		}

		public async void Option4()
		{
			OpenFileDialog dlg = new OpenFileDialog()
			{
				DefaultExt = ".txt",
				Filter = "Text documents (.txt)|*.txt"
			};

			dlg.ShowDialog();
			List<String> Rows = new List<String>();
			if (dlg.FileName != null)
			{
				string line;

				// Read the file and display it line by line.
				System.IO.StreamReader file = new System.IO.StreamReader(dlg.FileName);
				while ((line = file.ReadLine()) != null)
				{
					//Console.WriteLine(line);
					String rev = line.Replace("[", "");
					rev = rev.Replace("]", "");
					Rows.Add(rev);
				}

				file.Close();
				if (Rows.Count > 0)
				{
					String[] splt = Rows[0].Split(' ');
					int cols = splt.Length;
					confDataGrid(cols, Rows);
					CodingClient client = new CodingClient();
					progressBar.DataContext = null;
					progressBar.DataContext = client;
					client.ConfigureGenerator(Rows);
					suggested_dist.Text = "" + client.suggestedDistance();
					Task<List<String>> strs = client.GetCodewordAsync();
					await strs.ContinueWith((t) => {
						real_dist.Text = "" + client.minimalDistance(strs.Result);
						Codewords.Items.Clear();
						for (int i = 0; i < strs.Result.Count; i++)
						{
							Codewords.Items.Add(strs.Result[i]);
						}
					}, TaskScheduler.FromCurrentSynchronizationContext());
				}
			}
		}

		#endregion

		#region Utilities

		private void Message(String msg, int code)
		{
			switch (code)
			{
				case 0:
					MessageBox.Show(msg, "MathWorks", MessageBoxButton.OK, MessageBoxImage.Information);
					break;
				case 1:
					MessageBox.Show(msg, "MathWorks", MessageBoxButton.OK, MessageBoxImage.Warning);
					break;
				case 2:
					MessageBox.Show(msg, "MathWorks", MessageBoxButton.OK, MessageBoxImage.Exclamation);
					break;
				case 3:
					MessageBox.Show(msg, "MathWorks", MessageBoxButton.OK, MessageBoxImage.Error);
					break;
				default:
					MessageBox.Show(msg, "MathWorks", MessageBoxButton.OK, MessageBoxImage.None);
					break;
			}
		}

		private void confDataGrid(int columns, List<String> items)
		{
			dataGrid.ItemsSource = null;
			dataGrid.Columns.Clear();
			dataGrid.Items.Clear();
			for (int i = 0; i < columns; i++)
			{
				dataGrid.Columns.Add(new System.Windows.Controls.DataGridTextColumn()
				{
					Binding = new Binding("[" + i + "]"),
				});
			}
			List<String[]> itemsnew = new List<String[]>();
			for (int i = 0; i < items.Count; i++)
			{
				string[] arr = items[i].Split(' ');
				itemsnew.Add(arr);
			}
			dataGrid.ItemsSource = itemsnew;

		}

		BitmapImage BitmapToImageSource(Bitmap bitmap)
		{
			using (MemoryStream memory = new MemoryStream())
			{
				bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
				memory.Position = 0;
				BitmapImage bitmapimage = new BitmapImage();
				bitmapimage.BeginInit();
				bitmapimage.StreamSource = memory;
				bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
				bitmapimage.EndInit();
				return bitmapimage;
			}
		}
		
		#endregion


	}
}
