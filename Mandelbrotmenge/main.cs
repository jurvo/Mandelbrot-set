using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mandelbrotmenge
{
	public partial class main : Form
	{
		public main()
		{
			InitializeComponent();
			DoubleBuffered = true;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			e.Graphics.DrawLine(Pens.Black, new Point(0, ClientSize.Height / 2), new Point(ClientSize.Width, ClientSize.Height / 2));
			e.Graphics.DrawLine(Pens.Black, new Point(ClientSize.Width * 2 / 3, 0), new Point(ClientSize.Width * 2 / 3, ClientSize.Height));

			int numberOfIterations;
			ComplexNumber c;
			ComplexNumber z;
			for (double i = 0; i < ClientSize.Width; i++)
			{
				for (double j = 0; j < ClientSize.Height; j++)
				{
					numberOfIterations = 0;
					c = new ComplexNumber((i - 600) / 300, (j - 300) / 300);
					z = c;
					while (numberOfIterations < 100 && z.Sqrt() < 2)
					{
						z = z * z + c;
						numberOfIterations++;
					}
					if (z.Sqrt() < 2)
						e.Graphics.DrawLine(Pens.Black, new Point(Convert.ToInt32(i), Convert.ToInt32(j)), new Point(Convert.ToInt32(i), Convert.ToInt32(j + 1)));
				}
			}

			/*
			 Mitte(600,300) = 0,0
			 (0,0) = -2,-1
			 (0,300) = -2,0


			 */
		}
	}
}