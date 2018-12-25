using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandelbrotmenge
{
	class ComplexNumber
	{
		private double real;
		private double imaginary;

		/// <summary>
		/// 
		/// </summary>
		public double Real
		{
			get { return real; }
			set { real = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public double Imaginary
		{
			get { return imaginary; }
			set { imaginary = value; }
		}

		public ComplexNumber(double real, double imaginary)
		{
			this.real = real;
			this.imaginary = imaginary;

		}

		#region Operator overload
		public static ComplexNumber operator +(ComplexNumber left, ComplexNumber right)
		{
			return new ComplexNumber(left.Real + right.Real, left.Imaginary + right.Imaginary);
		}

		public static ComplexNumber operator -(ComplexNumber left, ComplexNumber right)
		{
			return new ComplexNumber(left.Real - right.Real, left.Imaginary - right.Imaginary);
		}

		public static ComplexNumber operator *(ComplexNumber left, ComplexNumber right)
		{
			return new ComplexNumber(left.Real * right.Real - left.Imaginary * right.Imaginary, left.Real * right.Imaginary + left.Imaginary * right.Real);
		}

		public static ComplexNumber operator /(ComplexNumber left, ComplexNumber right)
		{
			return new ComplexNumber(((left.Real * right.Real) + (left.Imaginary * right.Imaginary)) / ((right.Real * right.Real) + (right.Imaginary * right.Imaginary)), ((left.Imaginary * right.Real) - (left.Real * right.Imaginary))/ ((right.Real * right.Real) + (right.Imaginary * right.Imaginary)));
		}
		#endregion

		public double Sqrt()
		{
			return Math.Sqrt(real * real + imaginary * imaginary);
		}

		public override string ToString()
		{
			string s = "";
			if (real != 0)
				s += real;
			if (imaginary > 0)
				s += "+";
			if (imaginary != 0)
				s += imaginary + "i";

			return s;
		}
	}
}
