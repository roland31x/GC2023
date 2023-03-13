using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;

namespace GC_C2_03_06_2023
{
    public partial class Form1 : Form
    {
        readonly Random rng = new Random();
        int W = 0;
        int H = 0;
        public Form1()
        {
            InitializeComponent();
            Height = 800;
            Width = 800;
            H = this.ClientSize.Height;
            W = this.ClientSize.Width;

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.White);
            Ex3(g);

        }
        //Se dau n puncte in plan si o constanta d > 0. Sa se determine toate punctele care sunt la o distanta mai mica sau egala cu d fata de un punct fixat q.
        private void Ex1(Graphics g)
        {
            Pen p = new Pen(Color.Red, 3);
            int d = 300;

            int Qx = rng.Next(10, W);
            int Qy = rng.Next(10, H);
            g.DrawEllipse(p, Qx - d, Qy - d, 2 * d, 2 * d);
            g.DrawEllipse(p, Qx, Qy, 3, 3);

            p.Color = Color.Black;
            for (int i = 0; i < 40; i++)
            {
                int x = rng.Next(10, W - 10);
                int y = rng.Next(10, H - 10);

                double dist = Math.Sqrt((Qx - x) * (Qx - x) + (Qy - y) * (Qy - y));
                if (dist <= d)
                {
                    p.Color = Color.Blue;
                }
                else p.Color = Color.Black;
                g.DrawEllipse(p, x, y, 3, 3);

            }
        }
        // Se da o multime de puncte in plan. Sa se determine triunghiul de arie minima a carui varfuri sa faca parte din multimea data.
        // opt: solutie optimizata bazata pe sortarea punctelor in raport cu relatia de ordine: (x1,y1) < (x2,y2) <=> x1*y2 < x2*y1 si testarea punctelor adiacente din sirul ordonat.
        private void Ex2(Graphics g)
        {
            Pen p = new Pen(Color.Black, 3);
            int n = rng.Next(3, 50);
            Point[] pts = new Point[n];
            for (int i = 0; i < n; i++)
            {
                int x = rng.Next(10, W - 10);
                int y = rng.Next(10, H - 10);
                g.DrawEllipse(p, x, y, 3, 3);
                pts[i] = new Point(x, y);
            }

            //p.Color = Color.Red;
            //double minareaT = H*W;
            //Point[] minptsT = new Point[3];
            //for(int i = 0; i < n; i++)
            //{
            //    Point j;
            //    for(int f = 0; f < n && f != i; f++)
            //    {
            //        if()
            //    }
            //    double a = GetDist(pts[i], pts[i + 1]);
            //    double b = GetDist(pts[i + 1], pts[i + 2]);
            //    double c = GetDist(pts[i], pts[i + 2]);
            //    double per = (a + b + c) / 2;
            //    double area = Math.Sqrt(per * (per - a) * (per - b) * (per - c));
            //    if (area < minareaT)
            //    {
            //        minareaT = area;
            //        minptsT[0] = pts[i];
            //        minptsT[1] = pts[i + 1];
            //        minptsT[2] = pts[i + 2];
            //    }
            //}
            //g.DrawLine(p, minptsT[0], minptsT[1]);
            //g.DrawLine(p, minptsT[1], minptsT[2]);
            //g.DrawLine(p, minptsT[0], minptsT[2]);

            double minarea = H * W;
            Point[] minpts = new Point[3];

            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    for (int k = j + 1; k < n; k++)
                    {
                        double a = GetDist(pts[i], pts[j]);
                        double b = GetDist(pts[j], pts[k]);
                        double c = GetDist(pts[i], pts[k]);
                        double per = (a + b + c) / 2;
                        double area = Math.Sqrt(per * (per - a) * (per - b) * (per - c));
                        if (area < minarea)
                        {
                            minarea = area;
                            minpts[0] = pts[i];
                            minpts[1] = pts[j];
                            minpts[2] = pts[k];
                        }
                    }
                }
            }
            p.Color = Color.Green;
            g.DrawLine(p, minpts[0], minpts[1]);
            g.DrawLine(p, minpts[1], minpts[2]);
            g.DrawLine(p, minpts[0], minpts[2]);

        }
        // se da o multime de puncte in plan. Sa se determine cercul de raza minima care sa contina toate punctele date in interior.
        private void Ex3(Graphics g)
        {
            Pen p = new Pen(Color.Black, 3);
            int n = rng.Next(11, 90);
            PointF[] pts = new PointF[n];
            for (int i = 0; i < n; i++)
            {
                int x = rng.Next(200, W - 200);
                int y = rng.Next(200, H - 200);
                g.DrawEllipse(p, x, y, 3, 3);
                pts[i] = new PointF(x, y);
            }

            PointF center = new PointF(0, 0);

            p.Color = Color.Green;
            double minR = H * W;

            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    double r = GetDist(pts[i], pts[j]) / 2;
                    if (r < minR)
                    {
                        double cx = (pts[i].X + pts[j].X) / 2;
                        double yx = (pts[i].Y + pts[j].Y) / 2;
                        PointF CenterCheck = new PointF((float)cx, (float)yx);
                        bool ok = true;
                        for (int k = 0; k < n; k++)
                        {
                            if (GetDist(CenterCheck, pts[k]) > r + 1)
                            {
                                ok = false;
                                break;
                            }
                        }
                        if (!ok)
                        {
                            continue;
                        }
                        else
                        {
                            minR = r;
                            center = CenterCheck;
                            g.DrawEllipse(p, (float)(center.X - minR), (float)(center.Y - minR), (float)(2 * minR), (float)(2 * minR));
                            p.Color = Color.Gray;
                            p.Width = 1;
                            g.DrawLine(p, pts[i], pts[j]);
                            return;
                            //Thread.Sleep(1000);
                        }
                    }
                }
            }
            PointF[] tr = new PointF[3];
            PointF[] best = new PointF[3];
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    for (int k = j + 1; k < n; k++)
                    {
                        double a = GetDist(pts[i], pts[j]);
                        double b = GetDist(pts[j], pts[k]);
                        double c = GetDist(pts[i], pts[k]);
                        double per = (a + b + c) / 2;
                        double area = Math.Sqrt(per * (per - a) * (per - b) * (per - c));
                        if (area == 0)
                        {
                            // MessageBox.Show("3 Collinear points!");
                            continue;
                        }
                        double R = (a * b * c) / (4 * area);
                        if (R < minR)
                        {
                            bool ok = true;
                            tr[0] = pts[i];
                            tr[1] = pts[j];
                            tr[2] = pts[k];
                            PointF CenterCheck = FindCircleCenter(tr);
                            for (int l = 0; l < n; l++)
                            {
                                if (GetDist(CenterCheck, pts[l]) > R + 1)
                                {
                                    ok = false;
                                    break;
                                }
                            }
                            if (!ok)
                            {
                                continue;
                            }
                            else
                            {
                                center = CenterCheck;
                                minR = R;
                                best[0] = pts[i];
                                best[1] = pts[j];
                                best[2] = pts[k];
                                p.Color = Color.Blue;
                                //g.DrawLine(p, best[1], best[2]);
                                //g.DrawLine(p, best[0], best[1]);
                                //g.DrawLine(p, best[0], best[2]);
                                //g.DrawEllipse(p, (float)(center.X - minR), (float)(center.Y - minR), (float)(2 * minR), (float)(2 * minR));
                                //Thread.Sleep(1000);
                            }
                        }
                    }
                }
            }
            p.Color = Color.Red;
            g.DrawEllipse(p, (int)(center.X - minR), (int)(center.Y - minR), (int)(2 * minR), (int)(2 * minR));
            g.DrawEllipse(p, (float)center.X, (float)center.Y, 3, 3);

            p.Color = Color.Gray;
            p.Width = 1;
            g.DrawLine(p, best[1], best[2]);
            g.DrawLine(p, best[0], best[1]);
            g.DrawLine(p, best[0], best[2]);


        }
        PointF FindCircleCenter(PointF[] t)
        {
            PointF A = new PointF(t[0].X, t[0].Y);
            PointF B = new PointF(t[1].X, t[1].Y);
            PointF C = new PointF(t[2].X, t[2].Y);
            double mAB = GetSlope(A, B);
            double mBC = GetSlope(B, C);
            if (mAB == 0)
            {
                // MessageBox.Show("ab slope is 0!!");
                PointF aux = A;
                A = C;
                B = A;
                C = aux;

            }
            if (mBC == 0)
            {
                // MessageBox.Show("bc slope is 0!!");
                PointF aux = A;
                A = B;
                B = aux;
                // A = aux;

            }
            PointF P = new PointF((B.X + C.X) / 2, (B.Y + C.Y) / 2);
            PointF Q = new PointF((A.X + B.X) / 2, (A.Y + B.Y) / 2);
            double xR = (P.Y - Q.Y + P.X / GetSlope(B, C) - Q.X / GetSlope(A, B)) / ((1 / GetSlope(B, C)) - (1 / GetSlope(A, B)));
            double yR = (Q.Y - (1 / GetSlope(A, B)) * xR) + Q.X / GetSlope(A, B);

            PointF Center = new PointF((float)xR, (float)yR);
            return Center;
        }
        double GetSlope(PointF A, PointF B)
        {
            //if (A.Y - B.Y == 0)
            //{
            //    //MessageBox.Show("slope is 0");
            //}
            if (B.X - A.X == 0)
            {
                return double.MaxValue;
            }
            return ((double)(B.Y - A.Y) / (double)(B.X - A.X));
        }
        double GetDist(PointF p1, PointF p2)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }

    }
}