using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Eyes
{
	public partial class Form1 : Form
	{
		List<Face> smiles = new List<Face>();
		Random rnd = new Random();
		public Form1()
		{
			InitializeComponent();
			DoubleBuffered = true;
			
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			
		}
		class Pupil
		{
			public float x;
			public float y;
			public float r;
			public Pupil(float ax, float ay, float ar)
			{
				x = ax;
				y = ay;
				r = ar;
			}
		}
		class Eye
		{
			public float x;
			public float y;
			public float r;
			public Pupil pupil;
			public Eye(float ax, float ay, float ar, Pupil p)
			{
				x = ax;
				y = ay;
				r = ar;
				pupil = p;
			}

		}
		class Face
		{
			float x;
			float y;
			float r;
			public Eye leftEye;
			public Eye rightEye;
			public Pupil p1;
			public Pupil p2;
			public Color c;
			public Face(float ax, float ay, float ar, Color color)
			{
				x = ax;
				y = ay;
				r = ar;
				p1 = new Pupil(x+r/2-r/8, y+r-r/8, r / 8);
				p2 = new Pupil(x+3*r/2-r/8, y+r-r/8, r / 8);
				leftEye = new Eye(x + r/4, y + 3*r/4, r / 4, p1);
				rightEye = new Eye(x + 5*r/4, y + 3*r/4, r / 4, p2);
				c = color;
			}
			public void Draw(Graphics g)
			{
				SolidBrush brush = new SolidBrush(c);
				g.FillEllipse(brush, x, y, r*2, r*2);
				g.FillEllipse(Brushes.White, leftEye.x, leftEye.y, leftEye.r*2, leftEye.r*2);
				g.FillEllipse(Brushes.White, rightEye.x, rightEye.y, rightEye.r*2, rightEye.r*2);
				g.FillEllipse(Brushes.Black, p1.x, p1.y, p1.r * 2, p1.r * 2);
				g.FillEllipse(Brushes.Black, p2.x, p2.y, p2.r * 2, p2.r * 2);
			}
		}

		private void Form1_Paint(object sender, PaintEventArgs e)
		{
			foreach (Face f in smiles)
			{
				f.Draw(e.Graphics);
			}
		}

		private void Form1_MouseClick(object sender, MouseEventArgs e)
		{
			float rad = rnd.Next(25,100);
			Face f = new Face(e.X - rad/2,e.Y - rad/2, rad, Color.FromArgb(rnd.Next(255), rnd.Next(255), rnd.Next(255)));
			smiles.Add(f);
			Refresh();
		}

		private void Form1_MouseMove(object sender, MouseEventArgs e)
		{
			foreach (Face f in smiles)
			{
				float cent_l_x = f.leftEye.x + f.leftEye.r;
				float cent_l_y = f.leftEye.y + f.leftEye.r;
				float cent_r_x = f.rightEye.x + f.rightEye.r;
				float cent_r_y = f.rightEye.y + f.rightEye.r;
				float c1 = (float)Math.Sqrt((cent_l_y - e.Y) * (cent_l_y - e.Y) + (e.X - cent_l_x) * (e.X - cent_l_x));
				float c2 = (float)Math.Sqrt((cent_r_y - e.Y) * (cent_r_y - e.Y) + (e.X - cent_r_x) * (e.X - cent_r_x));

				if (c1 > f.rightEye.r - f.p2.r) 
				{
					float cos1 = (e.X - cent_l_x) / c1;
					float sin1 = (cent_l_y - e.Y) / c1;
					f.p1.x = cos1 * (f.leftEye.r - f.p1.r) + cent_l_x - f.p1.r;
					f.p1.y = -sin1 * (f.leftEye.r - f.p1.r) + cent_l_y - f.p1.r;
				}
				else
				{
					f.p1.x = e.X - f.p1.r;
					f.p1.y = e.Y - f.p1.r;
				}
				if(c2> f.rightEye.r - f.p2.r)
				{ 
					float cos2 = (e.X - cent_r_x) / c2;
					float sin2 = (cent_r_y - e.Y) / c2;
					f.p2.x = cos2 * (f.rightEye.r - f.p2.r) + cent_r_x - f.p2.r;
					f.p2.y = -sin2 * (f.rightEye.r - f.p2.r) + cent_r_y - f.p2.r;
				}
				else
				{
					f.p2.x = e.X - f.p2.r;
					f.p2.y = e.Y - f.p2.r;
				}
			}
			Refresh();
		}
	}
}
