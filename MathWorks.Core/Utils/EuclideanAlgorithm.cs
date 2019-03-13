using System;
using System.Collections.Generic;
using System.Numerics;

namespace MathWorks.Core.Utils
{
	public class EuclideanAlgorithm
	{
		/// <summary>
		/// Permite el calculo del GCD de dos números A y B de tal manera que se cumpla que dos coeficientes enteros X y Y multiplicados por los dos enteros sean igual al valor del GCD
		/// </summary>
		/// <returns>Un objeto del tipo EuclideanSolution con tres componentes, primera el GCD, segunda el coeficiente del entero mas pequeño y por ultimo el coeficiente del entero mas grande.</returns>
		public List<BigInteger> Calculate(BigInteger A, BigInteger B)
		{
			List<BigInteger> results = new List<BigInteger>();
			BigInteger val1 = A;
			BigInteger val2 = B;
			BigInteger x0 = 1, xn = 1;
			BigInteger y0 = 0, yn = 0;
			BigInteger x1 = 0;
			BigInteger y1 = 1;
			BigInteger q;
			BigInteger r = val1 % val2;

			while (r > 0)
			{
				q = val1 / val2;
				xn = x0 - q * x1;
				yn = y0 - q * y1;

				x0 = x1;
				y0 = y1;
				x1 = xn;
				y1 = yn;
				val1 = val2;
				val2 = r;
				r = val1 % val2;
			}

			results.Add(val2);
			if (BigInteger.Abs(A).CompareTo(BigInteger.Abs(B)) <1)
			{
				results.Add(xn);
				results.Add(yn);
			}
			else
			{
				results.Add(yn);
				results.Add(xn);
			}

			return results;
		}
	}
}
