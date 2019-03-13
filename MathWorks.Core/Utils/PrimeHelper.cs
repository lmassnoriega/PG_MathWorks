using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace MathWorks.Core.Utils
{
	public static class PrimeHelper
	{
		/// <summary>
		/// Recursive version of Euclidean Algorithm
		/// </summary>
		/// <param name="A">First integer</param>
		/// <param name="B">Second integer</param>
		/// <returns></returns>
		public static BigInteger GCD_Euclidean(BigInteger A, BigInteger B)
		{
			if (B == 0)
				return A;
			if (A == 0)
				return B;
			if (A > B)
				return GCD_Euclidean(B, A % B);
			else
				return GCD_Euclidean(B % A, A);
		}

		/// <summary>
		/// Non-recursive version of Euclidean GCD
		/// </summary>
		/// <param name="A">First integer</param>
		/// <param name="B">Second integer</param>
		/// <returns></returns>
		public static BigInteger GCD_Loop(BigInteger A, BigInteger B)
		{
			BigInteger R = BigInteger.One;
			while (B != 0)
			{
				R = A % B;
				A = B;
				B = R;
			}
			return A;
		}

		/// <summary>
		/// Function to retrieve prime factors of a number
		/// </summary>
		/// <param name="num">Number to dispose into prime factors</param>
		/// <returns></returns>
		public static List<BigInteger> FindFactors(BigInteger num)
		{
			List<BigInteger> result = new List<BigInteger>();

			// Take out the 2s.
			while (num % 2 == 0)
			{
				result.Add(2);
				num /= 2;
			}

			// Take out other primes.
			long factor = 3;
			while (factor * factor <= num)
			{
				if (num % factor == 0)
				{
					// This is a factor.
					result.Add(factor);
					num /= factor;
				}
				else
				{
					// Go to the next odd number.
					factor += 2;
				}
			}

			// If num is not 1, then whatever is left is prime.
			if (num > 1) result.Add(num);


			return result;
		}

		public static Boolean isPrime(BigInteger number)
		{
			BigInteger boundary = BigInteger.Divide(number,2);

			if (number == 1) return false;
			if (number == 2) return true;

			for (BigInteger i = 1; i <= boundary; ++i)
			{
				if (number % i == 0) return false;
			}

			return true;
		}
	}
}
