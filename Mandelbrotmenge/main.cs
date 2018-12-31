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
		double screenRatio;

		//TODO:
		//	get display ratio from screen....

		public main()
		{
			InitializeComponent();
			MouseWheel += Main_MouseWheel;

			Size = Screen.PrimaryScreen.Bounds.Size;
			//ClientSize = new Size(1000, 1000);
			screenRatio = (double)ClientSize.Width / (double)ClientSize.Height;

			//CoordinateSystem.CenterPoint = new Point(ClientSize.Width / 2, ClientSize.Height / 2);
			CoordinateSystem.Size = ClientSize;

			CoordinateSystem.yMin = -2;
			CoordinateSystem.yMax = 2;

			CoordinateSystem.xMin = CoordinateSystem.yMin * screenRatio;
			CoordinateSystem.xMax = CoordinateSystem.yMax * screenRatio;

			CoordinateSystem.calcCenter();

			CoordinateSystem.Pen = new Pen(Color.Red, 1);
			numberOfMaxIterations = 100;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			int numberOfIterations;
			double numberOfBlackPixel = 0;
			ComplexNumber c;
			ComplexNumber z;
			int[] array = new int[ClientSize.Height];


			for (double i = 0; i < ClientSize.Width; i++)
			{
				for (double j = 0; j < ClientSize.Height; j++)
				{
					numberOfIterations = 0;
					z = c = new ComplexNumber(ScreenToCoordinate(new PointF((float)i, (float)j)));
					while (numberOfIterations < numberOfMaxIterations && z.Sqrt() < 2)
					{
						z = z * z + c;
						numberOfIterations++;
					}
					e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(255, numberOfIterations * 255 / numberOfMaxIterations, numberOfIterations * 255 / numberOfMaxIterations, numberOfIterations * 255 / numberOfMaxIterations)), (float)i, (float)j, 1, 1);
					array[(int)j] = numberOfIterations;
					if (numberOfIterations == numberOfMaxIterations)
						numberOfBlackPixel++;
				}

				//int value = array[0];
				//int start = 0;
				//int end = 0;
				//for (int j = 0; j < ClientSize.Height; j++)
				//{
				//	if (value == array[j])
				//	{
				//		end = j;
				//	}
				//	else
				//	{
				//		e.Graphics.DrawLine(new Pen(Color.FromArgb(255, array[j] * 255 / numberOfMaxIterations, array[j] * 255 / numberOfMaxIterations, array[j] * 255 / numberOfMaxIterations), 1), new Point((int)i,start), new Point((int)i,end));
				//		start = j;
				//		value = array[j];
				//	}
				//}
			}

			#region Coordinate System drawing
			for (int i = 1; i < CoordinateSystem.xMax; i++)
			{
				e.Graphics.DrawLine(CoordinateSystem.Pen, new Point((int)(CoordinateSystem.CenterPoint.X + i * CoordinateSystem.Size.Width / (CoordinateSystem.xMax - CoordinateSystem.xMin)), CoordinateSystem.CenterPoint.Y - 5), new Point((int)(CoordinateSystem.CenterPoint.X + i * CoordinateSystem.Size.Width / (CoordinateSystem.xMax - CoordinateSystem.xMin)), CoordinateSystem.CenterPoint.Y + 5));
			}
			for (int i = -1; i > CoordinateSystem.xMin; i--)
			{
				e.Graphics.DrawLine(CoordinateSystem.Pen, new Point((int)(CoordinateSystem.CenterPoint.X + i * CoordinateSystem.Size.Width / (CoordinateSystem.xMax - CoordinateSystem.xMin)), CoordinateSystem.CenterPoint.Y - 5), new Point((int)(CoordinateSystem.CenterPoint.X + i * CoordinateSystem.Size.Width / (CoordinateSystem.xMax - CoordinateSystem.xMin)), CoordinateSystem.CenterPoint.Y + 5));
			}
			for (int i = 1; i < CoordinateSystem.yMax; i++)
			{
				e.Graphics.DrawLine(CoordinateSystem.Pen, new Point(CoordinateSystem.CenterPoint.X - 5, (int)(CoordinateSystem.CenterPoint.Y + i * CoordinateSystem.Size.Height / (CoordinateSystem.yMax - CoordinateSystem.yMin))), new Point(CoordinateSystem.CenterPoint.X + 5, (int)(CoordinateSystem.CenterPoint.Y + i * CoordinateSystem.Size.Height / (CoordinateSystem.yMax - CoordinateSystem.yMin))));
			}
			for (int i = -1; i > CoordinateSystem.yMin; i--)
			{
				e.Graphics.DrawLine(CoordinateSystem.Pen, new Point(CoordinateSystem.CenterPoint.X - 5, (int)(CoordinateSystem.CenterPoint.Y + i * CoordinateSystem.Size.Height / (CoordinateSystem.yMax - CoordinateSystem.yMin))), new Point(CoordinateSystem.CenterPoint.X + 5, (int)(CoordinateSystem.CenterPoint.Y + i * CoordinateSystem.Size.Height / (CoordinateSystem.yMax - CoordinateSystem.yMin))));
			}

			e.Graphics.DrawLine(CoordinateSystem.Pen, new Point(0, CoordinateSystem.CenterPoint.Y), new Point(ClientSize.Width, CoordinateSystem.CenterPoint.Y));
			e.Graphics.DrawLine(CoordinateSystem.Pen, new Point(CoordinateSystem.CenterPoint.X, 0), new Point(CoordinateSystem.CenterPoint.X, ClientSize.Height));
			#endregion
		}

		private void main_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
				Application.Exit();
		}

		private void main_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				
			}
			else if (e.Button == MouseButtons.Right)
			{
				CoordinateSystem.yMin = -2;
				CoordinateSystem.yMax = 2;

				CoordinateSystem.xMin = CoordinateSystem.yMin * 16 / 9;
				CoordinateSystem.xMax = CoordinateSystem.yMax * 16 / 9;
			
				CoordinateSystem.calcCenter();

				Invalidate();
			}
		}

		private void Main_MouseWheel(object sender, MouseEventArgs e)
		{
			if (e.Delta > 0)
			{
				CoordinateSystem.yMin /= 2;
				CoordinateSystem.yMax /= 2;

				CoordinateSystem.xMin = CoordinateSystem.yMin * screenRatio;
				CoordinateSystem.xMax = CoordinateSystem.yMax * screenRatio;
				Invalidate();
			}
			else if (e.Delta < 0)
			{
				
			}
		}

		private PointF ScreenToCoordinate(PointF p)
		{
			double x = (p.X - CoordinateSystem.CenterPoint.X) / CoordinateSystem.xResolution;
			double y = (p.Y - CoordinateSystem.CenterPoint.Y) / CoordinateSystem.yResolution;

			return new PointF((float)x, (float)y);
		}
	}
}