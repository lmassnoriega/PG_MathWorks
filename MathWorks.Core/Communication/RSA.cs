using MathWorks.Core.Utils;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace MathWorks.Core.Communication
{
    public class RSA
    {
		#region Common Attributes

		private BigInteger p;

		public BigInteger P
		{
			get { return p; }
			set { p = value; }
		}

		private BigInteger q;

		public BigInteger Q
		{
			get { return q; }
			set { q = value; }
		}

		private BigInteger e;

		public BigInteger E
		{
			get { return e; }
			set { e = value; }
		}
		
		#endregion

		#region Common Methods

		public BigInteger PhiN()
		{
			if (!P.Equals(null) && !Q.Equals(null))
			{
				return BigInteger.Multiply(P-1, Q-1);
			}
			else
			{
				return BigInteger.Zero;
			}
		}

		public BigInteger N()
		{
			if (!P.Equals(null) && !Q.Equals(null))
			{
				return BigInteger.Multiply(P , Q);
			}
			else
			{
				return BigInteger.Zero;
			}
		}

		public BigInteger D()
		{
			if (!P.Equals(null) && !Q.Equals(null))
			{
				EuclideanAlgorithm client = new EuclideanAlgorithm();
				List<BigInteger> result = client.Calculate(PhiN(), E);
				if (result[1] < 0)
					result[1] = result[1] + PhiN();

				return result[1];
			}
			else
			{
				return 0;
			}
		}
		
		public BigInteger Encrypt(BigInteger value)
		{
			return BigInteger.ModPow(value, E, N());
		}

		public BigInteger Decrypt(BigInteger value)
		{
			return BigInteger.ModPow(value, D(), N());
		}

		#endregion

		#region Block Coding

		private List<String> alphabet;

		public List<String> Alphabet
		{
			get {
				if (alphabet == null)
				{
					alphabet = new List<string>();
				}
				return alphabet; }
			set { alphabet = value; }
		}

		public bool CheckText(String code)
		{
			code = code.ToUpper();
			for (int i = 0; i < code.Length; i++)
			{
				if (!Alphabet.Contains(code.Substring(i,1)))
				{
					return false;
				}
			}
			return true;
		}

		public String BlockToValue(String value)
		{
			value = value.ToUpper();
			String val = "";
			for (int i = 0; i < value.Length; i++)
			{
				val += Alphabet.IndexOf(value.Substring(i, 1));
			}
			return val;
		}

		public BigInteger ValueToBaseBig(String value)
		{
			BigInteger baseN = BigInteger.Zero;
			for (int i = 0; i < value.Length; i++)
			{
				baseN += BigInteger.Parse(value.Substring(i, 1)) * BigInteger.Pow(Alphabet.Count, (value.Length - i-1));
			}
			return baseN;
		}
				
		public String CipherToBase(BigInteger value)
		{
			String BaseNumber = "";
			while (value!=BigInteger.Zero)
			{
				BaseNumber += value % (Alphabet.Count);
				value /= Alphabet.Count;
			}
			return Reverse(BaseNumber);
		}

		public String Reverse(string s)
		{
			char[] charArray = s.ToCharArray();
			Array.Reverse(charArray);
			return new string(charArray);
		}

		public String BaseToBlock(String value)
		{
			String Block = "";
			for (int i = 0; i < value.Length; i++)
			{
				Block += Alphabet[Convert.ToInt32(value.Substring(i,1))];
			}
			return Block;
		}

		public String Encrypt(String PlainBlock)
		{
			String v1 = BlockToValue(PlainBlock);
			BigInteger v2 = ValueToBaseBig(v1);
			BigInteger v3 = Encrypt(v2);
			String v4 = CipherToBase(v3);
			return BaseToBlock(v4);
		}

		public String Decrypt(String CipherBlock)
		{
			String v1 = BlockToValue(CipherBlock);
			BigInteger v2 = ValueToBaseBig(v1);
			BigInteger v3 = Decrypt(v2);
			String v4 = CipherToBase(v3);
			return BaseToBlock(v4);
		}
		#endregion

	}
}
