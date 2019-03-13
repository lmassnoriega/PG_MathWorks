using System;
using System.Drawing;
using ZXing;

namespace MathWorks.Core.Coding
{
	public class EANHelper
	{
		public static int CheckSumEAN13(string code)
		{
			if (code == null || code.Length != 12)
				throw new ArgumentException("Code length should be 12, i.e. excluding the checksum digit");

			int sum = 0;
			for (int i = 0; i < 12; i++)
			{
				int v;
				if (!int.TryParse(code[i].ToString(), out v))
					throw new ArgumentException("Invalid character encountered in specified code.");
				sum += (i % 2 == 0 ? v : v * 3);
			}
			int check = 10 - (sum % 10);
			return check % 10;
		}

		public static Bitmap GenerateBarCode(string EANCode)
		{
			BarcodeWriter writer = new BarcodeWriter
			{
				Format = BarcodeFormat.EAN_13,
				Renderer = new ZXing.Rendering.BitmapRenderer()
				{
					Foreground = Color.Black,
					Background = Color.White
				},				
				Options = new ZXing.Common.EncodingOptions
				{
					Height = 400,
					Width = 700
				}				
			};
			var bmpToEncode = writer.Write(EANCode);

			return bmpToEncode;
		}		
	}
}
