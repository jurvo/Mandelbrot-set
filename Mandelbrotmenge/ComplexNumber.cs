using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandelbrotmenge
{
	class ComplexNumber
	{
		public static readonly ComplexNumber I = new ComplexNumber(0, 1);
		private double real;
		private double imaginary;

		/// <summary>
		/// The real component of the Number.
		/// </summary>
		public double Real
		{
			get { return real; }
			set { real = value; }
		}

		/// <summary>
		/// The imaginary component of the Number.
		/// </summary>
		public double Imaginary
		{
			get { return imaginary; }
			set { imaginary = value; }
		}

		/// <summary>
		/// Instantiated a new Complex Number with a read and a imaginary component.
		/// </summary>
		/// <param name="real">The real component of the Number.</param>
		/// <param name="imaginary">The imaginary component of the Number</param>
		public ComplexNumber(double real, double imaginary)
		{
			this.real = real;
			this.imaginary = imaginary;
		}

		public ComplexNumber(System.Drawing.PointF point)
		{
			this.real = point.X;
			this.imaginary = point.Y;
		}

		#region Operator overload
		public static ComplexNumber operator +(ComplexNumber left, ComplexNumber right) => new ComplexNumber(left.Real + right.Real, left.Imaginary + right.Imaginary);

		public static ComplexNumber operator -(ComplexNumber left, ComplexNumber right) => new ComplexNumber(left.Real - right.Real, left.Imaginary - right.Imaginary);

		public static ComplexNumber operator *(ComplexNumber left, ComplexNumber right) => new ComplexNumber(left.Real * right.Real - left.Imaginary * right.Imaginary, left.Real * right.Imaginary + left.Imaginary * right.Real);

		public static ComplexNumber operator *(int left, ComplexNumber right) => new ComplexNumber(left * right.Real, left * right.Imaginary);
		public static ComplexNumber operator *(double left, ComplexNumber right) => new ComplexNumber(left * right.Real, left * right.Imaginary);

		public static ComplexNumber operator *(ComplexNumber left, int right) => new ComplexNumber(right * left.Real, right * left.Imaginary);
		public static ComplexNumber operator *(ComplexNumber left, double right) => new ComplexNumber(right * left.Real, right * left.Imaginary);

		public static ComplexNumber operator /(ComplexNumber left, ComplexNumber right) => new ComplexNumber(((left.Real * right.Real) + (left.Imaginary * right.Imaginary)) / ((right.Real * right.Real) + (right.Imaginary * right.Imaginary)), ((left.Imaginary * right.Real) - (left.Real * right.Imaginary)) / ((right.Real * right.Real) + (right.Imaginary * right.Imaginary)));

		public static implicit operator ComplexNumber(int i) => new ComplexNumber(i, 0);
		public static implicit operator ComplexNumber(double d) => new ComplexNumber(d, 0);
		#endregion

		/// <summary>
		/// Returns the absolute value of this Complex Number calculate with pythagorean theorem
		/// </summary>
		/// <returns></returns>
		public double AbsoluteValue()
		{
			return Math.Sqrt(real * real + imaginary * imaginary);
		}

		public override string ToString()
		{
			string s = real.ToString();
			if (imaginary > 0)
				s += "+";
			if (imaginary != 0)
				s += imaginary + "i";
			return s;
		}
	}
}

