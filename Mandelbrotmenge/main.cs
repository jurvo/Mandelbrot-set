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
		int numberOfMaxIterations;

		public main()
		{
			InitializeComponent();
			Size = Screen.PrimaryScreen.Bounds.Size;

			CoordinateSystem.CenterPoint = new Point(ClientSize.Width / 2, ClientSize.Height / 2);
			CoordinateSystem.Size = ClientSize;
			CoordinateSystem.xMin = -2;
			CoordinateSystem.xMax = 2;
			CoordinateSystem.yMin = -1;
			CoordinateSystem.yMax = 1;

			numberOfMaxIterations = 20;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			
			int numberOfIterations;
			double numberOfBlackPixel = 0;
			ComplexNumber c;
			ComplexNumber z;

			for (double i = 0; i < ClientSize.Width; i++)
			{
				for (double j = 0; j < ClientSize.Height; j++)
				{
					numberOfIterations = 0;
					c = new ComplexNumber((i - CoordinateSystem.CenterPoint.X) / (CoordinateSystem.Size.Width / (CoordinateSystem.xMax - CoordinateSystem.xMin)), (j - CoordinateSystem.CenterPoint.Y) / (CoordinateSystem.Size.Height / (CoordinateSystem.yMax - CoordinateSystem.yMin)));
					z = c;
					while (numberOfIterations < numberOfMaxIterations && z.Sqrt() < 2)
					{
						z = z * z + c;
						numberOfIterations++;
					}
					if (z.Sqrt() < 2)
					{
						e.Graphics.FillRectangle(Brushes.Black, (float)i, (float)j, 1, 1);
						numberOfBlackPixel++;
					}
				}
			}
			
			e.Graphics.DrawString((numberOfBlackPixel / (ClientSize.Height * ClientSize.Width)).ToString(), Font, Brushes.Black, new PointF(0, 0));

			e.Graphics.DrawLine(Pens.Red, new Point(0, CoordinateSystem.CenterPoint.Y), new Point(ClientSize.Width, CoordinateSystem.CenterPoint.Y));
			e.Graphics.DrawLine(Pens.Red, new Point(CoordinateSystem.CenterPoint.X, 0), new Point(CoordinateSystem.CenterPoint.X, ClientSize.Height));
			

			/*
			 Mitte(600,300) = 0,0
			 (0,0) = -2,-1
			 (0,300) = -2,0


			 */
		}

		private void main_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
				Application.Exit();
		}
	}
}