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
			Size = new Size(1800,900);

			CoordinateSystem.CenterPoint = new Point(0, ClientSize.Height / 2);
			CoordinateSystem.Size = ClientSize;
			CoordinateSystem.xMin = 0;
			CoordinateSystem.xMax = 0.8;
			CoordinateSystem.yMin = -0.2;
			CoordinateSystem.yMax = 0.2;

			numberOfMaxIterations = 50;
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
					/*if (z.Sqrt() < 2)
					{
						e.Graphics.FillRectangle(Brushes.Black, (float)i, (float)j, 1, 1);
						numberOfBlackPixel++;
					}*/
					e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(255, numberOfIterations * 255/numberOfMaxIterations, numberOfIterations * 255 / numberOfMaxIterations, numberOfIterations * 255 / numberOfMaxIterations)), (float)i, (float)j, 1, 1);
				}
			}

			//e.Graphics.DrawString((numberOfBlackPixel / (ClientSize.Height * ClientSize.Width)).ToString(), Font, Brushes.Black, new PointF(0, 0));
			e.Graphics.DrawString((MousePosition.X -CoordinateSystem.CenterPoint.X) / (CoordinateSystem.Size.Width / (CoordinateSystem.xMax - CoordinateSystem.xMin)) + " " + (MousePosition.Y - CoordinateSystem.CenterPoint.Y) / (CoordinateSystem.Size.Height / (CoordinateSystem.yMax - CoordinateSystem.yMin)), Font, Brushes.Black, new Point(0,0));

			e.Graphics.DrawLine(Pens.Black, new Point(0, CoordinateSystem.CenterPoint.Y), new Point(ClientSize.Width, CoordinateSystem.CenterPoint.Y));
			e.Graphics.DrawLine(Pens.Black, new Point(CoordinateSystem.CenterPoint.X, 0), new Point(CoordinateSystem.CenterPoint.X, ClientSize.Height));
		}

		private void main_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
				Application.Exit();
		}
	}
}