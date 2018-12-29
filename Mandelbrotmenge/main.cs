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
		double zoomFactor;

		public main()
		{
			InitializeComponent();
			MouseWheel += Main_MouseWheel;

			Size = Screen.PrimaryScreen.Bounds.Size;

			zoomFactor = 1;
			//CoordinateSystem.CenterPoint = new Point(ClientSize.Width / 2, ClientSize.Height / 2);
			CoordinateSystem.Size = ClientSize;
			CoordinateSystem.xMin = -2;
			CoordinateSystem.xMax = 1;
			CoordinateSystem.yMin = -1;
			CoordinateSystem.yMax = 1;

			CoordinateSystem.CenterPoint = new Point((int)(-1 * CoordinateSystem.xMin * CoordinateSystem.Size.Width / (CoordinateSystem.xMax - CoordinateSystem.xMin)), (int)(CoordinateSystem.yMax * CoordinateSystem.Size.Height / (CoordinateSystem.yMax - CoordinateSystem.yMin)));


			CoordinateSystem.Pen = new Pen(Color.Red, 1);
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
					z = c = new ComplexNumber(ScreenToCoordinate(new PointF((float)i, (float)j)));
					while (numberOfIterations < numberOfMaxIterations && z.Sqrt() < 2)
					{
						z = z * z + c;
						numberOfIterations++;
					}
					if (numberOfIterations == numberOfMaxIterations)
						e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(255, numberOfIterations * 255 / numberOfMaxIterations, numberOfIterations * 255 / numberOfMaxIterations, numberOfIterations * 255 / numberOfMaxIterations)), (float)i, (float)j, 1, 1);
				}
			}

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
		}

		private void main_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
				Application.Exit();
			if (e.KeyCode == Keys.Space)
			{

			}
		}

		private void main_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{

			}
			else if (e.Button == MouseButtons.Right)
			{

			}
		}

		private void Main_MouseWheel(object sender, MouseEventArgs e)
		{
			if (e.Delta > 0)
			{

				Invalidate();
			}
			else if (e.Delta < 0)
			{

				Invalidate();
			}
		}

		private PointF ScreenToCoordinate(PointF p)
		{
			double x = (p.X - CoordinateSystem.CenterPoint.X) / (CoordinateSystem.Size.Width / (CoordinateSystem.xMax - CoordinateSystem.xMin));
			double y = (p.Y - CoordinateSystem.CenterPoint.Y) / (CoordinateSystem.Size.Height / (CoordinateSystem.yMax - CoordinateSystem.yMin));

			return new PointF((float)x, (float)y);
		}
	}
}