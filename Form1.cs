using System;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;

namespace GC_C3_03_13_2023
{
    public partial class Form1 : Form
    {
        readonly Random rng = new Random();

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
            //ProblemaCuplaj(g);
            SweepLine(g);

        }
        void ProblemaCuplaj(Graphics g)
        {
            Pen pen = new Pen(Color.Black, 3);

            int n = 2 * rng.Next(10, 30);
            Point[] initial = new Point[n];
            int[] ok = new int[n];
            for (int i = 0; i < n; i++)
            {
                int x = rng.Next(100, ClientSize.Width - 100);
                int y = rng.Next(100, ClientSize.Height - 100);
                Point p = new Point(x, y);
                initial[i] = p;
                g.DrawEllipse(pen, x, y, 3, 3);
                //Thread.Sleep(1);
            }

            pen.Width = 1;

            double[,] distances = new double[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    distances[i, j] = GetDist(initial[i], initial[j]);
                }
            }
            for (int k = 0; k < n / 2; k++)
            {
                double mindist = 800 * 800;
                int minIindex = 0;
                int minJindex = 0;
                for (int i = 0; i < n; i++)
                {
                    if (ok[i] == 1)
                    {
                        continue;
                    }
                    for (int j = 0; j < n; j++)
                    {
                        if (ok[j] == 0 && i != j && distances[i, j] < mindist)
                        {
                            minJindex = j;
                            minIindex = i;
                            mindist = distances[i, j];
                        }
                    }

                }
                ok[minJindex] = 1;
                ok[minIindex] = 1;
                g.DrawLine(pen, initial[minIindex], initial[minJindex]);
            }
        }
        void SegmentIntersectionON2(Graphics g)
        {
            Pen pen = new Pen(Color.Black, 1);
            int n = rng.Next(5, 11);
            List<Segment> segments = new List<Segment>();
            for (int i = 0; i < n; i++)
            {
                int x1 = rng.Next(100, 700);
                int x2 = rng.Next(100, 700);
                int y1 = rng.Next(100, 700);
                int y2 = rng.Next(100, 700);
                Segment toAdd = new Segment(new Point(x1, y1), new Point(x2, y2), i);
                segments.Add(toAdd);


                g.DrawLine(pen, toAdd.start, toAdd.end);
                g.DrawString(i.ToString(), new Font("Arial", 12), new SolidBrush(Color.Black), toAdd.start);
            }
            foreach(Segment s1 in segments)
            {
                foreach(Segment s2 in segments)
                {
                    if (s1.ID != s2.ID && doIntersect(s1, s2))
                    {
                        Intersect(s1, s2, g);
                    }
                }
            }
        }
        void SweepLine(Graphics g)
        {

            Pen pen = new Pen(Color.Black, 1);
            int n = rng.Next(5, 11);
            SortedList<int, Segment> S = new SortedList<int, Segment>();
            PriorityQueue<SweepPoint,int> sps = new PriorityQueue<SweepPoint, int>();
            for (int i = 0; i < n; i++)
            {
                int x1 = rng.Next(100, 700);
                int x2 = rng.Next(100, 700);
                int y1 = rng.Next(100, 700);
                int y2 = rng.Next(100, 700);
                Segment toAdd = new Segment(new Point(x1, y1), new Point(x2, y2), i);
                sps.Enqueue(new SweepPoint(toAdd, true), toAdd.start.X);
                sps.Enqueue(new SweepPoint(toAdd, false), toAdd.end.X);


                g.DrawLine(pen, toAdd.start, toAdd.end);
                g.DrawString(i.ToString(), new Font("Arial", 12), new SolidBrush(Color.Black), toAdd.start);
            }
            Pen p = new Pen(Color.Orange, 3);
            pen.Color = Color.Green;
            //List<Segment> Q = new List<Segment>();

            while(sps.Count > 0)
            {
                SweepPoint Next = sps.Dequeue();
                if (Next.IsStartPoint)
                {
                    p.Color = Color.Red;
                    g.DrawLine(pen, Next.Parent.start.X, Next.Parent.start.Y + 800, Next.Parent.start.X , Next.Parent.start.Y - 800);
                    Segment toAdd = Next.Parent;
                    foreach(Segment s2 in S.Values)
                    {
                        Intersect(Next.Parent, s2, g, sps);
                    }
                    S.Add(toAdd.start.Y, toAdd);
                    //MessageBox.Show($"Adding {toAdd.ID}");
                    ////p.Color = Color.Orange;
                    //string St = "";
                    //foreach (Segment s in S.Values)
                    //{
                    //    St += s.ID.ToString() + ",";
                    //}
                    //MessageBox.Show(St);
                    //g.DrawLine(p, toAdd.start, toAdd.end);
                    //Thread.Sleep(1000);

                    //int neighbor = S.IndexOfValue(toAdd) - 1;
                    //if (neighbor < 0)
                    //{
                    //    if (neighbor + 2 > S.Count - 1)
                    //    {
                    //        continue;
                    //    }
                    //    Intersect(toAdd, S.Values[neighbor + 2], g, sps);
                    //    //MessageBox.Show($"Checking intersection: {toAdd.ID},{S.Values[neighbor + 2].ID}");
                    //}
                    //else
                    //{
                    //    Intersect(S.Values[neighbor], toAdd, g, sps);
                    //    //MessageBox.Show($"Checking intersection: {S.Values[neighbor].ID}, {toAdd.ID}");

                    //    if (neighbor + 2 > S.Count - 1)
                    //    {
                    //        continue;
                    //    }
                    //    else
                    //    {
                    //        Intersect(toAdd, S.Values[neighbor + 2], g, sps);

                    //        //MessageBox.Show($"Checking intersection: {toAdd.ID},{S.Values[neighbor + 2].ID}");
                    //    }
                    //}
                }
                else if(!Next.IsIntersection)
                {
                    ////MessageBox.Show($"Removing {Next.Parent.ID}");
                    g.DrawLine(pen, Next.Parent.end.X, Next.Parent.end.Y + 800, Next.Parent.end.X, Next.Parent.end.Y - 800);
                    //try
                    //{
                    //    int index = S.IndexOfValue(Next.Parent);
                    //    Intersect(S.Values[index - 1], S.Values[index + 1], g, sps);

                    //   // MessageBox.Show($"Checking intersection: {S.Values[index - 1].ID},{S.Values[index + 1].ID}");
                    //}
                    //catch (Exception)
                    //{

                    //    //p.Color = Color.Black;
                    //    //g.DrawLine(p, sps.Values[i].Parent.start, sps.Values[i].Parent.end);
                    //    //Thread.Sleep(1000);

                    //    S.RemoveAt(S.IndexOfValue(Next.Parent));


                    //    //string St1 = "";
                    //    //foreach (Segment s in S.Values)
                    //    //{
                    //    //    St1 += s.ID.ToString() + ",";
                    //    //}
                    //    //MessageBox.Show(St1);

                    //    continue;
                    //}
                   // p.Color = Color.Black;

                    //g.DrawLine(p, sps.Values[i].Parent.start, sps.Values[i].Parent.end);
                    //Thread.Sleep(1000);
                    
                    S.RemoveAt(S.IndexOfValue(Next.Parent));

                    //MessageBox.Show($"Removing {sps.Values[i].Parent.ID}");

                    //string St = "";
                    //foreach (Segment s in S.Values)
                    //{
                    //    St += s.ID.ToString() + ",";
                    //}
                    //MessageBox.Show(St);
                }
                else
                {
                    g.DrawLine(pen, Next.P.X, Next.P.Y + 800, Next.P.X, Next.P.Y - 800);
                    //MessageBox.Show($"{Next.Parent.ID} intersects {Next.Parent2.ID}");
                    //S.RemoveAt(S.IndexOfValue(Next.Parent));
                    //S.RemoveAt(S.IndexOfValue(Next.Parent2));
                    //if(Next.Parent.start.Y < Next.Parent2.start.Y)
                    //{
                    //    int indexcheck = S.IndexOfValue(Next.Parent) - 1;
                    //    S.Add(Next.P.Y + 1, Next.Parent);
                    //    if(indexcheck >= 0)
                    //    {
                    //        Intersect(Next.Parent, S.Values[indexcheck],g, sps);
                    //    }

                    //    S.Add(Next.P.Y - 1, Next.Parent2);
                    //    indexcheck = S.IndexOfValue(Next.Parent) + 1;
                    //    if(indexcheck < S.Count)
                    //    {
                    //        Intersect(Next.Parent, S.Values[indexcheck], g, sps);
                    //    }


                    //}
                    //else
                    //{
                    //    S.Add(Next.P.Y - 1, Next.Parent);
                    //    int indexcheck = S.IndexOfValue(Next.Parent) + 1;
                    //    if (indexcheck < S.Count)
                    //    {
                    //        Intersect(Next.Parent, S.Values[indexcheck], g, sps);
                    //    }

                    //    S.Add(Next.P.Y + 1, Next.Parent2);
                    //    indexcheck = S.IndexOfValue(Next.Parent2) - 1;
                    //    if (indexcheck >= 0)
                    //    {
                    //        Intersect(Next.Parent2, S.Values[indexcheck], g, sps);
                    //    }
                    //}
                    //string St = "";
                    //foreach (Segment s in S.Values)
                    //{
                    //    St += s.ID.ToString() + ",";
                    //}
                    //MessageBox.Show(St);
                }
            }

        }
        void Intersect(Segment s1, Segment s2, Graphics g, PriorityQueue<SweepPoint, int> Q)
        {
            Pen p = new Pen(Color.Red, 3);
            if (!doIntersect(s1, s2))
            {
                return;
            }

            int x1 = s1.start.X;
            int y1 = s1.start.Y;
            int x2 = s1.end.X;
            int y2 = s1.end.Y;
            int x3 = s2.start.X;
            int y3 = s2.start.Y;
            int x4 = s2.end.X;
            int y4 = s2.end.Y;
            

            int xcoord = ((x1 * y2 - y1 * x2) * (x3 - x4) - (x1 - x2) * (x3 * y4 - y3 * x4)) / ((x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4));
            int ycoord = ((x1 * y2 - y1 * x2) * (y3 - y4) - (y1 - y2) * (x3 * y4 - y3 * x4)) / ((x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4));

            Q.Enqueue(new SweepPoint(s1, s2, new Point(xcoord, ycoord)),xcoord);
            g.DrawEllipse(p, xcoord, ycoord, 3, 3);
        }
        void Intersect(Segment s1, Segment s2, Graphics g)
        {
            if (!doIntersect(s1, s2))
            {
                return;
            }
            Pen p = new Pen(Color.Red, 3);

            int x1 = s1.start.X;
            int y1 = s1.start.Y;
            int x2 = s1.end.X;
            int y2 = s1.end.Y;
            int x3 = s2.start.X;
            int y3 = s2.start.Y;
            int x4 = s2.end.X;
            int y4 = s2.end.Y;


            int xcoord = ((x1 * y2 - y1 * x2) * (x3 - x4) - (x1 - x2) * (x3 * y4 - y3 * x4)) / ((x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4));
            int ycoord = ((x1 * y2 - y1 * x2) * (y3 - y4) - (y1 - y2) * (x3 * y4 - y3 * x4)) / ((x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4));

            g.DrawEllipse(p, xcoord, ycoord, 3, 3);
        }
        bool doIntersect(Segment s1, Segment s2)
        {
            Point p1 = s1.start, q1 = s1.end, p2 = s2.start, q2 = s2.end;

            // Find the four orientations needed for general and
            // special cases
            int o1 = orientation(p1, q1, p2);
            int o2 = orientation(p1, q1, q2);
            int o3 = orientation(p2, q2, p1);
            int o4 = orientation(p2, q2, q1);

            // General case
            if (o1 != o2 && o3 != o4)
                return true;

            // Special Cases
            // p1, q1 and p2 are collinear and p2 lies on segment p1q1
            if (o1 == 0 && onSegment(p1, p2, q1)) return true;

            // p1, q1 and q2 are collinear and q2 lies on segment p1q1
            if (o2 == 0 && onSegment(p1, q2, q1)) return true;

            // p2, q2 and p1 are collinear and p1 lies on segment p2q2
            if (o3 == 0 && onSegment(p2, p1, q2)) return true;

            // p2, q2 and q1 are collinear and q1 lies on segment p2q2
            if (o4 == 0 && onSegment(p2, q1, q2)) return true;

            return false; // Doesn't fall in any of the above cases
        }
        int orientation(Point p, Point q, Point r)
        {
            // See https://www.geeksforgeeks.org/orientation-3-ordered-points/
            // for details of below formula.
            int val = (q.Y - p.Y) * (r.X - q.X) -
                      (q.X - p.X) * (r.Y - q.Y);

            if (val == 0) return 0;  // collinear

            return (val > 0) ? 1 : 2; // clock or counterclock wise
        }
        bool onSegment(Point p, Point q, Point r)
        {
            if (q.X <= Math.Max(p.X, r.X) && q.Y >= Math.Min(p.X, r.X) &&
                q.Y <= Math.Max(p.Y, r.Y) && q.Y >= Math.Min(p.Y, r.Y))
                return true;

            return false;
        }
        double GetDist(Point p1, Point p2)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}