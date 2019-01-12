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
		bool drawCoord;
		bool drawInfo;
		bool drawCoordInfo;
		bool drawPerfInfo;
		bool drawCenterCoordInfo;
		bool drawCrosshair;

		Bitmap b;

		public main()
		{
			InitializeComponent();
			MouseWheel += Main_MouseWheel;

			Size = Screen.PrimaryScreen.Bounds.Size; //set fullscreen

			#region Init of CoordinateSystem
			screenRatio = ClientSize.Width / (double)ClientSize.Height;
			CoordinateSystem.Size = ClientSize;
			CoordinateSystem.yMin = -2;
			CoordinateSystem.yMax = 2;
			CoordinateSystem.xMin = CoordinateSystem.yMin * screenRatio;
			CoordinateSystem.xMax = CoordinateSystem.yMax * screenRatio;
			CoordinateSystem.calcOrigin();
			CoordinateSystem.Pen = new Pen(Color.Red, 1);
			#endregion

			#region Settings
			numberOfMaxIterations = 100; //Number of max Iteration to proof that c is bounded
			drawCoord = true; //flag to control if the axis is drawn
			drawInfo = true; //flag to control if the informations are drawn, if false, non of the infos  is drawn, wheter or not the flags are true
			drawCoordInfo = true; //flag to control if values like xMin and xMax are drawn
			drawPerfInfo = true; //flag to control if the delta time is drawn
			drawCenterCoordInfo = true; //flag to control if the centerscreen coordinate is drawn
			drawCrosshair = true; //flag to control if the crosshair is drawn
			#endregion
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			DateTime t = DateTime.Now;

			#region set drawing
			int numberOfIterations;
			ComplexNumber c;
			ComplexNumber z;
			b = new Bitmap(ClientSize.Width, ClientSize.Height);

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
					int x = numberOfIterations * 255 / numberOfMaxIterations;
					b.SetPixel((int)i, (int)j, Color.FromArgb(255, x, x, x));
				}
			}

			if (b != null)
				e.Graphics.DrawImage(b, new PointF(0, 0));
			#endregion

			#region Coordinate System drawing
			if (drawCoord)
			{
				//x-Achse
				if (CoordinateSystem.Origin.Y > 0 && CoordinateSystem.Origin.Y < ClientSize.Height)
				{
					e.Graphics.DrawLine(CoordinateSystem.Pen, new Point(0, CoordinateSystem.Origin.Y), new Point(ClientSize.Width, CoordinateSystem.Origin.Y));
					for (int i = 1; i < CoordinateSystem.xMax; i++)
					{
						e.Graphics.DrawLine(CoordinateSystem.Pen, new Point((int)(CoordinateSystem.Origin.X + i * CoordinateSystem.Size.Width / (CoordinateSystem.xMax - CoordinateSystem.xMin)), CoordinateSystem.Origin.Y - 5), new Point((int)(CoordinateSystem.Origin.X + i * CoordinateSystem.Size.Width / (CoordinateSystem.xMax - CoordinateSystem.xMin)), CoordinateSystem.Origin.Y + 5));
					}
					for (int i = -1; i > CoordinateSystem.xMin; i--)
					{
						e.Graphics.DrawLine(CoordinateSystem.Pen, (float)(CoordinateSystem.Origin.X + i * CoordinateSystem.Size.Width / (CoordinateSystem.xMax - CoordinateSystem.xMin)), CoordinateSystem.Origin.Y - 5, (float)(CoordinateSystem.Origin.X + i * CoordinateSystem.Size.Width / (CoordinateSystem.xMax - CoordinateSystem.xMin)), CoordinateSystem.Origin.Y + 5);
					}
				}
				//y-Achse
				if (CoordinateSystem.Origin.X > 0 && CoordinateSystem.Origin.X < ClientSize.Width)
				{
					e.Graphics.DrawLine(CoordinateSystem.Pen, new Point(CoordinateSystem.Origin.X, 0), new Point(CoordinateSystem.Origin.X, ClientSize.Height));
					for (int i = 1; i < CoordinateSystem.yMax; i++)
					{
						e.Graphics.DrawLine(CoordinateSystem.Pen, CoordinateSystem.Origin.X - 5, (float)(CoordinateSystem.Origin.Y + i * CoordinateSystem.Size.Height / (CoordinateSystem.yMax - CoordinateSystem.yMin)), CoordinateSystem.Origin.X + 5, (float)(CoordinateSystem.Origin.Y + i * CoordinateSystem.Size.Height / (CoordinateSystem.yMax - CoordinateSystem.yMin)));
					}
					for (int i = -1; i > CoordinateSystem.yMin; i--)
					{
						e.Graphics.DrawLine(CoordinateSystem.Pen, CoordinateSystem.Origin.X - 5, (float)(CoordinateSystem.Origin.Y + i * CoordinateSystem.Size.Height / (CoordinateSystem.yMax - CoordinateSystem.yMin)), CoordinateSystem.Origin.X + 5, (float)(CoordinateSystem.Origin.Y + i * CoordinateSystem.Size.Height / (CoordinateSystem.yMax - CoordinateSystem.yMin)));
					}
				}
			}
			#endregion

			#region info drawing
			string s = "";
			if (drawCoordInfo)
				s += "xMin: " + CoordinateSystem.xMin + "\nxMax: " + CoordinateSystem.xMax + "\nyMin: " + CoordinateSystem.yMin + "\nyMax: " + CoordinateSystem.yMax + "\n";
			if (drawPerfInfo)
				s += (DateTime.Now - t).ToString()+ "\n";
			if (drawCenterCoordInfo)
				s += ScreenToCoordinate(new PointF(ClientSize.Width / 2, ClientSize.Height / 2)).ToString() + "\n";
			if (drawInfo)
				e.Graphics.DrawString(s, Font, Brushes.Red, new PointF(0, 0));

			if (drawCrosshair)
			{
				e.Graphics.DrawLine(CoordinateSystem.Pen, ClientSize.Width / 2, ClientSize.Height / 2 - 5, ClientSize.Width / 2, ClientSize.Height / 2 + 5);
				e.Graphics.DrawLine(CoordinateSystem.Pen, ClientSize.Width / 2 - 5, ClientSize.Height / 2, ClientSize.Width / 2 + 5, ClientSize.Height / 2 );
			}
			#endregion
		}

		private void main_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Escape:
					Application.Exit();
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
				PointF x = ScreenToCoordinate(new PointF(ClientSize.Width / 2, ClientSize.Height / 2));
				CoordinateSystem.xMin = CoordinateSystem.xMin + Math.Abs(x.X - CoordinateSystem.xMin) / 2;
				CoordinateSystem.xMax = CoordinateSystem.xMax - Math.Abs(x.X - CoordinateSystem.xMax) / 2;

				CoordinateSystem.yMin = CoordinateSystem.yMin + Math.Abs(x.Y - CoordinateSystem.yMin) / 2;
				CoordinateSystem.yMax = CoordinateSystem.yMax - Math.Abs(x.Y - CoordinateSystem.yMax) / 2;
				CoordinateSystem.calcOrigin();
				Invalidate();
			}
			else if (e.Delta < 0)
			{
				PointF x = ScreenToCoordinate(new PointF(ClientSize.Width / 2, ClientSize.Height / 2));
				CoordinateSystem.xMin = CoordinateSystem.xMin - Math.Abs(x.X - CoordinateSystem.xMin);
				CoordinateSystem.xMax = CoordinateSystem.xMax + Math.Abs(x.X - CoordinateSystem.xMax);

				CoordinateSystem.yMin = CoordinateSystem.yMin - Math.Abs(x.Y - CoordinateSystem.yMin);
				CoordinateSystem.yMax = CoordinateSystem.yMax + Math.Abs(x.Y - CoordinateSystem.yMax);
				CoordinateSystem.calcOrigin();
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