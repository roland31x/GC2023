namespace GC_C4_03_20_2023
{
    public partial class Form1 : Form
    {
        readonly Random random = new Random();
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
            ConvexHullAndrew(g);

        }
        void ConvexHullSimple(Graphics g)
        {
            Pen p = new Pen(Color.Gray, 3);
            List<Point> S = new List<Point>();
            int n = random.Next(10, 25);
            for(int i = 0; i < n; i++)
            {
                int x = random.Next(100, 700);
                int y = random.Next(100, 700);
                S.Add(new Point(x, y));
                g.DrawEllipse(p, x, y, 3, 3);
            }
            List<Segment> okP = new List<Segment>();
            for(int i = 0; i < S.Count; i++)
            {
                for(int j = 0; j < S.Count; j++)
                {
                    if (S[i] == S[j])
                    {
                        continue;
                    }
                    bool ok = true;
                    for(int k = 0; k < S.Count; k++)
                    {
                        if (S[k] == S[j] || S[k] == S[i]) 
                        {
                            continue;
                        }
                        if(IsRight(S[k], S[i], S[j]))
                        {
                            ok = false;
                        }
                    }
                    if (ok)
                    {
                        okP.Add(new Segment(S[i], S[j]));
                    }
                }
            }
            p = new Pen(Color.Gray, 1);
            for (int i = 0; i < okP.Count; i++)
            {
                
                DrawLine(p,g,okP[i]);
                //Thread.Sleep(1000);
            }
            //g.DrawLine(p, okP[0], okP[okP.Count - 1]);
        }
        void DrawLine(Pen p,Graphics g, Segment s)
        {
            g.DrawLine(p, s.s, s.e);
        }
        bool IsRight(Point r, Point p, Point q)
        {
            int Det = p.X*q.Y + p.Y*r.X + q.X*r.Y - r.X*q.Y - r.Y*p.X - q.X*p.Y;
            if(Det < 0)
            {
                return false;
            }
            return true; ;
        }
        void ConvexHullAndrew(Graphics g)
        {
            Pen p = new Pen(Color.Gray, 3);
            PriorityQueue<Point, int> S = new PriorityQueue<Point, int>();
            int n = random.Next(10, 25);
            for (int i = 0; i < n; i++)
            {
                int x = random.Next(100, 700);
                int y = random.Next(100, 700);
                S.Enqueue(new Point(x, y),x);
                g.DrawEllipse(p, x, y, 3, 3);
            }
            List<Point> points = new List<Point>();
            while(S.Count > 0)
            {
                points.Add(S.Dequeue());
            }


            List<Point> Lsup = new List<Point>
            {
                points[0],
                points[1]
            };
            int j = 2;
            for(int i = 2; i < points.Count; i++)
            {
                Lsup.Add(points[i]);
                while(Lsup.Count > 2 && !IsRight(Lsup[j], Lsup[j-1], Lsup[j-2]))
                {
                    Lsup.Remove(Lsup[j - 1]);
                    j--;
                }
                j++;
            }
            for(int i = 0; i < Lsup.Count - 1; i++)
            {
                g.DrawLine(p, Lsup[i], Lsup[i + 1]);
            }


            List<Point> Linf = new List<Point>
            {
                points[points.Count - 1],
                points[points.Count - 2]
            };

            j = 2;
            for(int i = points.Count - 3; i >= 0; i--)
            {
                Linf.Add(points[i]);
                while (Linf.Count > 2 && !IsRight(Linf[j], Linf[j - 1], Linf[j - 2]))
                {
                    Linf.Remove(Linf[j - 1]);
                    j--;
                }
                j++;
            }
            for (int i = 0; i < Linf.Count - 1; i++)
            {
                g.DrawLine(p, Linf[i], Linf[i + 1]);
            }
        }
    }
    class Segment
    {
        public Point s { get; set; }
        public Point e { get; set; }
        public Segment(Point s, Point e)
        {
            this.s = s;
            this.e = e;
        }
    }
}