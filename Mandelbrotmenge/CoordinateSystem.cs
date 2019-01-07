using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Mandelbrotmenge
{
	static class CoordinateSystem
	{
		public static Pen Pen;
		/// <summary>
		/// The origin point in px.
		/// </summary>
		public static Point Origin;
		public static Size Size;
		public static double xMin;
		public static double xMax;
		public static double yMin;
		public static double yMax;

		public static double xResolution
		{
			get { return Size.Width / (xMax - xMin); }
		}

		public static double yResolution
		{
			get { return Size.Height / (yMax - yMin); }
		}

		public static void calcOrigin()
		{
			Origin = new Point((int)(-1 * xMin * xResolution), (int)(yMax * yResolution));
		}
	}
}