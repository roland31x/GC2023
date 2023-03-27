using System.Security.Cryptography;

namespace GC_C5_03_27_2023
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Width = 800;
            Height = 800;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            //ConvexHullJarvis(g);
            ConvexHullGraham(g);
        }

        private void ConvexHullJarvis(Graphics g)
        {
            Random rng = new Random();
            Pen p = new Pen(Color.DarkOliveGreen, 3);
            int n = rng.Next(10, 50);
            Point min = new Point(0, 0);
            int ymin = 800;
            List<Point> points = new List<Point>();
            List<Point> chs = new List<Point>();
            for(int i =0; i < n; i++)
            {
                int x = rng.Next(100, 700);
                int y = rng.Next(100, 700);
                Point point = new Point(x, y);
                if (y < ymin)
                {
                    ymin = y;
                    min = point;
                }
                
                points.Add(point);
                g.DrawEllipse(p, x, y, 3, 3);
            }
            p.Width = 0;
            p.Color = Color.DarkRed;
            chs.Add(min);
            int k = 0;
            bool valid = true;
            while (valid)
            {
                Point pivot;
                do
                {
                    pivot = points[rng.Next(points.Count)];
                } while (pivot == chs[k]);
                for(int i = 0; i < n; i++)
                {
                    if (IsRight(points[i], chs[k], pivot))
                    {
                        pivot = points[i];
                    }
                }
                if(pivot != chs[0])
                {
                    chs.Add(pivot);
                    k++;
                }
                else
                {
                    valid = false;
                }
                
            }
            for(int i = 0; i < chs.Count - 1; i++)
            {
                g.DrawLine(p, chs[i], chs[i + 1]);
            }
            g.DrawLine(p, chs.First(), chs.Last());
        }
        private void ConvexHullGraham(Graphics g)
        {
            Random rng = new Random();
            Pen p = new Pen(Color.DarkOliveGreen, 3);
            int n = rng.Next(50, 100);
            Point min = new Point(0, 0);
            int ymin = 800;
            List<Point> points = new List<Point>();
            List<Point> chs = new List<Point>();
            for (int i = 0; i < n; i++)
            {
                int x = rng.Next(100, 700);
                int y = rng.Next(100, 700);
                Point point = new Point(x, y);
                if (y < ymin)
                {
                    ymin = y;
                    min = point;
                }

                points.Add(point);
                g.DrawEllipse(p, x, y, 3, 3);
            }
            p.Width = 1;
            p.Color = Color.Coral;
            points.Remove(min);
            n--;
            for(int i = 0; i < n; i++)
            {
                for(int j = 0; j < n; j++)
                {
                    if (!IsRight(points[i], min, points[j]))
                    {
                        (points[i], points[j]) = (points[j], points[i]);
                    }
                }
            }
            chs.Add(min);
            chs.Add(points[0]);
            chs.Add(points[1]);
            for(int i = 2; i < n; i++)
            {
                while (IsRight(chs[chs.Count - 1], chs[chs.Count - 2], points[i]))
                {
                    chs.Remove(chs.Last());
                }
                chs.Add(points[i]);
            }

            for (int i = 0; i < chs.Count - 1; i++)
            {
                g.DrawLine(p, chs[i], chs[i + 1]);
            }
            g.DrawLine(p, chs.First(), chs.Last());
            //for (int i = 0; i < n; i++)
            //{
            //    g.DrawString(i.ToString(), new Font("Arial", 12),new SolidBrush(Color.Black), points[i]);
            //}
        }
        bool IsRight(Point r, Point p, Point q)
        {
            int Det = p.X * q.Y + p.Y * r.X + q.X * r.Y - r.X * q.Y - r.Y * p.X - q.X * p.Y;
            if (Det < 0)
            {
                return true;
            }
            return false;
        }
    }
}