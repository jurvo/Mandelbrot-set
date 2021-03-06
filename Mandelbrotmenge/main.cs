﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mandelbrotmenge
{
	public partial class main : Form
	{
		private delegate void SafeCallDelegate();

		/// <summary>
		/// Ration between width and height.
		/// </summary>
		private double screenRatio;
		/// <summary>
		/// Number of max Iteration to proof that c is bounded.
		/// </summary>
		private int numberOfMaxIterations;
		/// <summary>
		/// Flag to control if the axis is drawn.
		/// </summary>
		private bool drawAxis;
		/// <summary>
		/// Flag to control if the informations are drawn, if false, non of the infos  is drawn, wheter or not the flags are true.
		/// </summary>
		private bool drawInfo;
		/// <summary>
		/// Flag to control if values like xMin and xMax are drawn.
		/// </summary>
		private bool drawCoordInfo;
		/// <summary>
		/// Flag to control if the delta time is drawn.
		/// </summary>
		private bool drawPerfInfo;
		/// <summary>
		/// Flag to control if the centerscreen coordinate is drawn.
		/// </summary>
		private bool drawCenterCoordInfo;
		/// <summary>
		/// Flag to control if the crosshair is drawn.
		/// </summary>
		private bool drawCrosshair;
		/// <summary>
		/// Flag to control if the number of iterations per pixel is drawn.
		/// </summary>
		private bool drawNumberOfIterations;

		private bool updateMandelbrot;

		bool q1ready = false, q2ready = false, q3ready = false, q4ready = false;
		Bitmap b, q1, q2, q3, q4;

		public main()
		{
			InitializeComponent();
			MouseWheel += Main_MouseWheel;

			Size = Screen.PrimaryScreen.Bounds.Size; //set fullscreen

			updateMandelbrot = true;

			q1 = new Bitmap(ClientSize.Width / 2, ClientSize.Height / 2);
			q2 = new Bitmap(ClientSize.Width / 2, ClientSize.Height / 2);
			q3 = new Bitmap(ClientSize.Width / 2, ClientSize.Height / 2);
			q4 = new Bitmap(ClientSize.Width / 2, ClientSize.Height / 2);

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
			numberOfMaxIterations = 100;
			drawAxis = true;
			drawInfo = true;
			drawCoordInfo = true;
			drawPerfInfo = true;
			drawCenterCoordInfo = true;
			drawCrosshair = true;
			drawNumberOfIterations = true;
			#endregion
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			e.Graphics.DrawString("working...", Font, Brushes.Red, new PointF(ClientSize.Width / 2 - e.Graphics.MeasureString("working...", Font).Width / 2, ClientSize.Height / 2 - e.Graphics.MeasureString("working...", Font).Height / 2));
			DateTime t = DateTime.Now;

			#region Mandelbrotset drawing
			/*
			if (b == null || updateMandelbrot)
			{
				updateMandelbrot = false;
			
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
			}
			e.Graphics.DrawImage(b, new PointF(0, 0));*/
			#endregion

			#region new Mandelbrotset drawing
			if (updateMandelbrot)
			{
				updateMandelbrot = false;
				q1ready = q2ready = q3ready = q4ready = false;
				new Thread(new ThreadStart(calcQ1)).Start();
				new Thread(new ThreadStart(calcQ2)).Start();
				new Thread(new ThreadStart(calcQ3)).Start();
				new Thread(new ThreadStart(calcQ4)).Start();
			}
			if (q1ready)
				e.Graphics.DrawImage(q1, new PointF(0, 0));
			if (q2ready)
				e.Graphics.DrawImage(q2, new PointF(ClientSize.Width / 2, 0));
			if (q3ready)
				e.Graphics.DrawImage(q3, new PointF(0, ClientSize.Height / 2));
			if (q4ready)
				e.Graphics.DrawImage(q4, new PointF(ClientSize.Width / 2, ClientSize.Height / 2));
			#endregion

			#region Coordinate-System drawing
			if (drawAxis)
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

			#region Info drawing
			string s = "";
			if (drawCoordInfo)
				s += "x ∈ [" + CoordinateSystem.xMin + ";" + CoordinateSystem.xMax + "]\ny ∈ [" + CoordinateSystem.yMin + ";" + CoordinateSystem.yMax + "]\n";
			if (drawPerfInfo)
				s += "Time in sec to calculate the set: " + (DateTime.Now - t).TotalSeconds.ToString() + " s\n";
			if (drawCenterCoordInfo)
				s += "Coordinates of the screencenter: " + ScreenToCoordinate(new PointF(ClientSize.Width / 2, ClientSize.Height / 2)).ToString() + "\n";
			if (drawNumberOfIterations)
				s += "Number of Iterations per Pixel:" + numberOfMaxIterations + "\n";
			if (drawInfo)
				e.Graphics.DrawString(s, Font, Brushes.Red, new PointF(0, 0));

			if (drawCrosshair)
			{
				e.Graphics.DrawLine(CoordinateSystem.Pen, ClientSize.Width / 2, ClientSize.Height / 2 - 5, ClientSize.Width / 2, ClientSize.Height / 2 + 5);
				e.Graphics.DrawLine(CoordinateSystem.Pen, ClientSize.Width / 2 - 5, ClientSize.Height / 2, ClientSize.Width / 2 + 5, ClientSize.Height / 2);
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
				case Keys.C:
					drawCrosshair = !drawCrosshair;
					Invalidate();
					break;
				case Keys.O:
					drawAxis = !drawAxis;
					Invalidate();
					break;
				case Keys.I:
					drawInfo = !drawInfo;
					Invalidate();
					break;
				case Keys.Space:
					Invalidate();
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

				updateMandelbrot = true;
				Invalidate();
			}
			else if (e.Button == MouseButtons.Right)
			{
				CoordinateSystem.yMin = -2;
				CoordinateSystem.yMax = 2;

				CoordinateSystem.xMin = CoordinateSystem.yMin * screenRatio;
				CoordinateSystem.xMax = CoordinateSystem.yMax * screenRatio;

				CoordinateSystem.calcOrigin();

				updateMandelbrot = true;
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
				updateMandelbrot = true;
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
				updateMandelbrot = true;
				Invalidate();
			}
		}

		private PointF ScreenToCoordinate(PointF p)
		{
			double x = (p.X - CoordinateSystem.Origin.X) / CoordinateSystem.xResolution;
			double y = (CoordinateSystem.Origin.Y - p.Y) / CoordinateSystem.yResolution;

			return new PointF((float)x, (float)y);
		}

		private void calcQ1()
		{
			q1ready = false;
			ComplexNumber z;

			for (double i = 0; i < ClientSize.Width / 2; i++)
			{
				for (double j = 0; j < ClientSize.Height / 2; j++)
				{
					z = new ComplexNumber(ScreenToCoordinate(new PointF((float)i, (float)j)));
					int x = DrawCondition(z);
					q1.SetPixel((int)i, (int)j, Color.FromArgb(255, x, x, x));
				}
			}
			q1ready = true;
			Invoke(new SafeCallDelegate(Refresh));
		}

		private void calcQ2()
		{
			q2ready = false;
			ComplexNumber c;
			ComplexNumber z;

			for (double i = ClientSize.Width / 2; i < ClientSize.Width; i++)
			{
				for (double j = 0; j < ClientSize.Height / 2; j++)
				{
					z = new ComplexNumber(ScreenToCoordinate(new PointF((float)i, (float)j)));
					int x = DrawCondition(z);
					q2.SetPixel((int)i - ClientSize.Width / 2, (int)j, Color.FromArgb(255, x, x, x));
				}
			}
			q2ready = true;
			Invoke(new SafeCallDelegate(Refresh));
		}

		private void calcQ3()
		{
			q3ready = false;
			ComplexNumber c;
			ComplexNumber z;

			for (double i = 0; i < ClientSize.Width / 2; i++)
			{
				for (double j = ClientSize.Height / 2; j < ClientSize.Height; j++)
				{
					z = new ComplexNumber(ScreenToCoordinate(new PointF((float)i, (float)j)));
					int x = DrawCondition(z);
					q3.SetPixel((int)i, (int)j - ClientSize.Height / 2, Color.FromArgb(255, x, x, x));
				}
			}
			q3ready = true;
			Invoke(new SafeCallDelegate(Refresh));
		}

		private void calcQ4()
		{
			q4ready = false;
			ComplexNumber c;
			ComplexNumber z;

			for (double i = ClientSize.Width / 2; i < ClientSize.Width; i++)
			{
				for (double j = ClientSize.Height / 2; j < ClientSize.Height; j++)
				{
					z = new ComplexNumber(ScreenToCoordinate(new PointF((float)i, (float)j)));
					int x = DrawCondition(z);
					q4.SetPixel((int)i - ClientSize.Width / 2, (int)j - ClientSize.Height / 2, Color.FromArgb(255, x, x, x));
				}
			}
			q4ready = true;
			Invoke(new SafeCallDelegate(Refresh));
		}

		private int DrawCondition(ComplexNumber z)
		{
			int numberOfIterations = 0;
			ComplexNumber c = z;
			if (z.Real < 0.25 && z.Real > -0.5 && z.Imaginary < 0.4 && z.Imaginary > -0.4)
				numberOfIterations = numberOfMaxIterations;
			while (numberOfIterations < numberOfMaxIterations && z.AbsoluteValue() < 2)
			{
				z = z * z + c;
				numberOfIterations++;
			}
			return numberOfIterations * 255 / numberOfMaxIterations;
		}
	}
}