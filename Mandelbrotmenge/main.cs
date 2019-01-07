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

		public main()
		{
			InitializeComponent();
			MouseWheel += Main_MouseWheel;

			Size = Screen.PrimaryScreen.Bounds.Size;
			//Size = new Size(1000, 500);
			screenRatio = ClientSize.Width / (double)ClientSize.Height;

			CoordinateSystem.Size = ClientSize;

			CoordinateSystem.yMin = -2;
			CoordinateSystem.yMax = 2;
			CoordinateSystem.xMin = CoordinateSystem.yMin * screenRatio;
			CoordinateSystem.xMax = CoordinateSystem.yMax * screenRatio;
			CoordinateSystem.calcOrigin();

			CoordinateSystem.Pen = new Pen(Color.Red, 1);
			numberOfMaxIterations = 50;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			DateTime t = DateTime.Now;
			int numberOfIterations;
			double numberOfBlackPixel = 0;
			ComplexNumber c;
			ComplexNumber z;

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
					if (numberOfIterations == numberOfMaxIterations)
						numberOfBlackPixel++;
				}
			}

			#region Coordinate System drawing
			for (int i = 1; i < CoordinateSystem.xMax; i++)
			{
				e.Graphics.DrawLine(CoordinateSystem.Pen, new Point((int)(CoordinateSystem.Origin.X + i * CoordinateSystem.Size.Width / (CoordinateSystem.xMax - CoordinateSystem.xMin)), CoordinateSystem.Origin.Y - 5), new Point((int)(CoordinateSystem.Origin.X + i * CoordinateSystem.Size.Width / (CoordinateSystem.xMax - CoordinateSystem.xMin)), CoordinateSystem.Origin.Y + 5));
			}
			for (int i = -1; i > CoordinateSystem.xMin; i--)
			{
				e.Graphics.DrawLine(CoordinateSystem.Pen, new Point((int)(CoordinateSystem.Origin.X + i * CoordinateSystem.Size.Width / (CoordinateSystem.xMax - CoordinateSystem.xMin)), CoordinateSystem.Origin.Y - 5), new Point((int)(CoordinateSystem.Origin.X + i * CoordinateSystem.Size.Width / (CoordinateSystem.xMax - CoordinateSystem.xMin)), CoordinateSystem.Origin.Y + 5));
			}
			for (int i = 1; i < CoordinateSystem.yMax; i++)
			{
				e.Graphics.DrawLine(CoordinateSystem.Pen, new Point(CoordinateSystem.Origin.X - 5, (int)(CoordinateSystem.Origin.Y + i * CoordinateSystem.Size.Height / (CoordinateSystem.yMax - CoordinateSystem.yMin))), new Point(CoordinateSystem.Origin.X + 5, (int)(CoordinateSystem.Origin.Y + i * CoordinateSystem.Size.Height / (CoordinateSystem.yMax - CoordinateSystem.yMin))));
			}
			for (int i = -1; i > CoordinateSystem.yMin; i--)
			{
				e.Graphics.DrawLine(CoordinateSystem.Pen, new Point(CoordinateSystem.Origin.X - 5, (int)(CoordinateSystem.Origin.Y + i * CoordinateSystem.Size.Height / (CoordinateSystem.yMax - CoordinateSystem.yMin))), new Point(CoordinateSystem.Origin.X + 5, (int)(CoordinateSystem.Origin.Y + i * CoordinateSystem.Size.Height / (CoordinateSystem.yMax - CoordinateSystem.yMin))));
			}

			e.Graphics.DrawLine(CoordinateSystem.Pen, new Point(0, CoordinateSystem.Origin.Y), new Point(ClientSize.Width, CoordinateSystem.Origin.Y));
			e.Graphics.DrawLine(CoordinateSystem.Pen, new Point(CoordinateSystem.Origin.X, 0), new Point(CoordinateSystem.Origin.X, ClientSize.Height));
			#endregion
			string s = "xMin: " + CoordinateSystem.xMin + "\n" + "xMax: " + CoordinateSystem.xMax + "\n" + "yMin: " + CoordinateSystem.yMin + "\n" + "yMax: " + CoordinateSystem.yMax;
			e.Graphics.DrawString(s, Font, Brushes.Red, new PointF(0, 0));
		}

		private void main_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Escape:
					Application.Exit();
					break;
				case Keys.PrintScreen:
					Bitmap screenshot = new Bitmap(ClientSize.Width, ClientSize.Height);
					DrawToBitmap(screenshot, ClientRectangle);
					screenshot.Save(@"C:\Users\Jurek\Desktop\screen.png", System.Drawing.Imaging.ImageFormat.Png);
					break;
				default:
					break;
			}
		}

		private void main_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				PointF coord = ScreenToCoordinate(e.Location);

				double tempXMax = CoordinateSystem.xMax;
				double tempXMin = CoordinateSystem.xMin;

				double tempYMax = CoordinateSystem.yMax;
				double tempYMin = CoordinateSystem.yMin;

				CoordinateSystem.xMin = coord.X - (tempXMax - tempXMin) / 2;
				CoordinateSystem.xMax = coord.X + (tempXMax - tempXMin) / 2;

				CoordinateSystem.yMin = coord.Y - (tempYMax - tempYMin) / 2;
				CoordinateSystem.yMax = coord.Y + (tempYMax - tempYMin) / 2;

				CoordinateSystem.calcOrigin();

				Invalidate();
			}
			else if (e.Button == MouseButtons.Right)
			{
				CoordinateSystem.yMin = -2;
				CoordinateSystem.yMax = 2;

				CoordinateSystem.xMin = CoordinateSystem.yMin * screenRatio;
				CoordinateSystem.xMax = CoordinateSystem.yMax * screenRatio;

				CoordinateSystem.calcOrigin();

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
				CoordinateSystem.yMin *= 2;
				CoordinateSystem.yMax *= 2;

				CoordinateSystem.xMin = CoordinateSystem.yMin * screenRatio;
				CoordinateSystem.xMax = CoordinateSystem.yMax * screenRatio;
				Invalidate();
			}
		}

		private PointF ScreenToCoordinate(PointF p)
		{
			double x = (p.X - CoordinateSystem.Origin.X) / CoordinateSystem.xResolution;
			double y = (CoordinateSystem.Origin.Y - p.Y) / CoordinateSystem.yResolution;

			return new PointF((float)x, (float)y);
		}
	}
}