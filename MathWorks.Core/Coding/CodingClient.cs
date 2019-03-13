using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathWorks.Core.Coding
{
	public class CodingClient
	{
		private double progress;

		public double Progress
		{
			get { return progress; }
			set { progress = value; }
		}

		private List<String> words;

		public List<String> Words
		{
			get { return words; }
			set { words = value; }
		}



		private String[][] g;

		public String[][] G
		{
			get { return g; }
			set { g = value; }
		}

		public long suggestedDistance()
		{
			if (G.GetLength(0)% 24== 22)
			{
				return 4 * (G.GetLength(0) / 24) + 6;
			}
			else
			{
				return 4 * (G.GetLength(0) / 24) + 4;
			}
		}

		public void ConfigureGenerator(List<String> Rows)
		{
			G = new String[Rows.Count][];

			for (int i = 0; i < Rows.Count; i++)
			{
				String[] splt = Rows[i].Split(' ');
				G[i] = splt;
			}
		}
		
		public Task<List<String>> GetCodewordAsync()
		{
			return Task.Run<List<String>>(() => GetCodewords());
		}

		public List<String> GetCodewords()
		{
			List<String> codes = new List<String>();
			long bound = Convert.ToInt64(Math.Pow(2, G.GetLength(0)));
			for (long i = 0; i < bound; i++)
			{
				String baseN = completeBase(Convert.ToString(i, 2), G.GetLength(0));
				String[] CodeWord = new String[G[0].Length];
				for (int j = 0; j < CodeWord.Length; j++)
				{
					CodeWord[j] = "0";
				} 
				for (int j = 0; j < baseN.Length; j++)
				{
					if (!baseN.Substring(j,1).Equals("0"))
					{
						CodeWord = AddCode(CodeWord, G[j]);
					}
				}
				String word = "";
				for (int k = 0; k < CodeWord.Length; k++)
				{
					word += CodeWord[k];
				}
				codes.Add(word);
				Progress = i * 100 / bound;
				Debug.WriteLine(Progress);
				//Debug.WriteLine(word);
			}
			Words = codes;
			return codes;
		}

		public String[] AddCode(String[] code, String[] code1)
		{
			String[] result = new String[code.Length];
			for (int i = 0; i < code.Length; i++)
			{
				if (code[i].Equals("0"))
				{
					if (code1[i].Equals("0"))
					{
						result[i]= "0";
					}
					else
					{
						result[i] = "1";
					}
				}
				else
				{
					if (code1[i].Equals("0"))
					{
						result[i] = "1";
					}
					else
					{
						result[i] = "0";
					}
				}
			}
			return result;
		}

		public String completeBase(String baseNumber, int lenght)
		{
			String newBase = baseNumber;
			int Zeros = lenght - baseNumber.Length;
			for (int i = 0; i < Zeros; i++)
			{
				newBase = "0" + newBase;
			}
			return newBase;
		}

		public long minimalDistance(List<String> codewords)
		{
			long min = Weight(codewords[1]);
			for (int i = 0; i < codewords.Count; i++)
			{
				if (Weight(codewords[i])< min && Weight(codewords[i]) > 0)
				{
					min = Weight(codewords[i]);
				}
			}
			return min;
		}

		public long Weight(String Codeword)
		{
			long d = 0;
			for (int i = 0; i < Codeword.Length; i++)
			{
				if (!Codeword.Substring(i,1).Equals("0"))
				{
					d++;
				}
			}
			return d;
		}
	}
}
