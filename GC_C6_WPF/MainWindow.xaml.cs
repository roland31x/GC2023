using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Input.Manipulations;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace GC_C6_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Point> points = new List<Point>();
        List<Triangle> triangles = new List<Triangle>();

        Line? last;
        Ellipse? set;
        bool done = false;
        public MainWindow()
        {
            InitializeComponent();
            MouseDown += MainWindow_MouseDown;
            MouseMove += MainWindow_MouseMove;
        }

        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if(!done && points.Count > 0)
            {
                if(last != null)
                {
                    MainCanvas.Children.Remove(last);
                }
                double y = e.GetPosition(this).Y;
                double x = e.GetPosition(this).X;
                Point clicked = new Point(x, y);
                last = HoverLine(points.Last(), clicked, Colors.Coral);                   
            }
        }

        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!done)
            {
                double y = e.GetPosition(this).Y;
                double x = e.GetPosition(this).X;

                if (points.Count == 0)
                {
                    Ellipse st = new Ellipse()
                    {
                        Width = 20,
                        Height = 20,
                        Fill = new SolidColorBrush(Colors.Orange),
                        Stroke = new SolidColorBrush(Colors.Black),
                    };
                    st.MouseDown += Finish_Click;
                    st.MouseEnter += St_MouseEnter;
                    st.MouseLeave += St_MouseLeave;
                    Canvas.SetTop(st, y - st.Width / 2);
                    Canvas.SetLeft(st, x - st.Height / 2);
                    MainCanvas.Children.Add(st);
                    Canvas.SetZIndex(st, 1);
                    set = st;
                    
                }
                if(points.Count == 0)
                {
                    DrawCircle(5, x, y).MouseDown += Finish_Click;
                }
                else DrawCircle(5, x, y);
               
                Point clicked = new Point(x, y);
                if (points.Count > 0)
                {
                    DrawLine(points.Last(), clicked, Colors.Gray);
                }
                points.Add(clicked);
            }          
        }
        private Ellipse DrawCircle(double width, double x, double y)
        {           
            Ellipse c = new Ellipse()
            {
                Width = width,
                Height = width,
                Fill = new SolidColorBrush(Colors.Black),
                Stroke = new SolidColorBrush(Colors.Black),
            };
            Canvas.SetTop(c, y - c.Width / 2);
            Canvas.SetLeft(c, x - c.Height / 2);
            MainCanvas.Children.Add(c);
            Canvas.SetZIndex(c, 2);
            return c;
        }
        private Ellipse DrawCircle(double width, Point p, Color cl)
        {
            Ellipse c = new Ellipse()
            {
                Width = width,
                Height = width,
                Fill = new SolidColorBrush(cl),
                Stroke = new SolidColorBrush(cl),
            };
            Canvas.SetTop(c, p.Y - c.Width / 2);
            Canvas.SetLeft(c, p.X - c.Height / 2);
            MainCanvas.Children.Add(c);
            Canvas.SetZIndex(c, 2);
            return c;
        }
        private void St_MouseLeave(object sender, MouseEventArgs e)
        {
            set.Fill = new SolidColorBrush(Colors.Orange);
        }

        private void St_MouseEnter(object sender, MouseEventArgs e)
        {
            set.Fill = new SolidColorBrush(Colors.Red);
        }

        private void Finish_Click(object sender, RoutedEventArgs e)
        {
            
            DrawLine(points.First(), points.Last(), Colors.Gray);
            done = true;
            MainCanvas.Children.Remove(last);
            MainCanvas.Children.Remove(set);
        }
        Line DrawLine(Point p, Point q, Color c)
        {
            Line l = new Line()
            {
                X1 = p.X,
                X2 = q.X,
                Y1 = p.Y,
                Y2 = q.Y,
                StrokeThickness = 2,
                Stroke = new SolidColorBrush(c),
                Fill = new SolidColorBrush(c),
            };
            MainCanvas.Children.Add(l);
            return l;
        }
        Line HoverLine(Point p, Point q, Color c)
        {
            Line l = new Line()
            {
                X1 = p.X,
                X2 = q.X,
                Y1 = p.Y,
                Y2 = q.Y,
                StrokeThickness = 2,
                Stroke = new SolidColorBrush(c),
                Fill = new SolidColorBrush(c),
            };
            l.MouseDown += MainWindow_MouseDown;
            MainCanvas.Children.Add(l);
            Canvas.SetZIndex(l, -1);
            return l;
        }
        void TriangulateRandomPts(object sender, RoutedEventArgs e)
        {
            if (!done)
            {
                done = true;
                Random rng = new Random();
                int n = 10;

                for (int i = 0; i < n; i++)
                {
                    double x = rng.Next(100, (int)Width - 100);
                    double y = rng.Next(100, (int)Height - 100);
                    Point PT = new Point(x, y);
                    points.Add(PT);
                    DrawCircle(5, x, y);
                }
                List<Segment> segs = new List<Segment>();
                for (int i = 0; i < points.Count; i++)
                {
                    for (int j = i + 1; j < points.Count; j++)
                    {
                        segs.Add(new Segment(points[i], points[j]));
                    }
                }
                for (int i = 0; i < segs.Count; i++)
                {
                    for (int j = 0; j < segs.Count; j++)
                    {
                        if (segs[i].Dist < segs[j].Dist)
                        {
                            (segs[i], segs[j]) = (segs[j], segs[i]);
                        }
                    }
                }
                List<Segment> good = new List<Segment>();
                for (int i = 0; i < segs.Count; i++)
                {
                    bool ok = true;
                    for (int j = 0; j < good.Count; j++)
                    {
                        if (segs[i].p == good[j].p || segs[i].p == good[j].q || segs[i].q == good[j].p || segs[i].q == good[j].q)
                        {
                            continue;
                        }
                        if (segs[i].Intersects(good[j]))
                        {
                            ok = false;
                            break;
                        }
                    }
                    if (ok)
                    {
                        good.Add(segs[i]);
                    }
                }
                foreach (Segment seg in good)
                {
                    DrawLine(seg.p, seg.q, Colors.Aquamarine);
                }
            }
        }
        private void TR_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 1; i < points.Count; i++)
            {
                for (int j = 1; j < points.Count; j++)
                {
                    if (!IsLeft(points[i], points[0], points[j]))
                    {
                        (points[i], points[j]) = (points[j], points[i]);
                    }
                }
            }
            for(int i = 2; i < points.Count - 1; i++)
            {
                DrawLine(points[0], points[i], Colors.Red);
            }
        }
        bool IsLeft(Point r, Point p, Point q)
        {
            double Det = p.X * q.Y + p.Y * r.X + q.X * r.Y - r.X * q.Y - r.Y * p.X - q.X * p.Y;
            if (Det < 0)
            {
                return false;
            }
            return true;
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            points = new List<Point>();
            done = false;
            MainCanvas.Children.Clear();
        }

        private async void PolyTriangulate(object sender, RoutedEventArgs e)
        {
            int n = points.Count;
            //for(int i = 0; i < points.Count; i++)
            //{
            //    for(int j = 0; j < points.Count; j++)
            //    {
            //        if()
            //    }
            //}
            List<Segment> segs = new List<Segment>();
            for(int i = 0; i < n - 1; i++)
            {
                for(int j = 0; j < n; j++)
                {
                    if(j == i || j == i - 1 || j == i + 1)
                    {
                        continue;
                    }
                    int next = j % n;
                    if(i == 0 && j == n - 1)
                    {
                        break;
                    }
                    bool ok = true;
                    Segment toCheck = new Segment(points[i], points[next]);
                    Line drew = DrawLine(toCheck.p, toCheck.q, Colors.Red);
                    for (int k = 0; k < segs.Count; k++)
                    {
                        if(toCheck.p == segs[k].p || toCheck.p == segs[k].q || toCheck.q == segs[k].p || toCheck.q == segs[k].q)
                        {
                            continue;
                        }
                        if (toCheck.Intersects(segs[k]))
                        {
                            ok = false;
                            break;
                        }
                        
                    }
                    //for(int k = 0; k < points.Count; k++)
                    //{
                    //    if(k == i || k == next || (k + 1) % n == next || (k + 1) % n == i)
                    //    {
                    //        continue;
                    //    }
                    //    if (toCheck.Intersects(new Segment(points[k], points[(k + 1) % n])))
                    //    {
                    //        ok = false;
                    //        break;
                    //    }
                    //}
                    //int prev = i - 1;
                    //if(prev < 0)
                    //{
                    //    prev = n - 1;
                    //}
                    if (!IsDiagonala(points[i], points[j], points))
                    {
                        ok = false;
                    }
                    //if (!IsReflex(points[i], points))
                    //{
                    //    Ellipse helpppp = DrawCircle(10, points[i].X, points[i].Y);
                    //    helpppp.Fill = Brushes.Red;
                    //    drew.Fill = Brushes.Red;
                    //    if (!(IsLeft(points[i + 1], points[i], points[j]) && IsLeft(points[j], points[i], points[prev])))
                    //    {
                    //        ok = false;
                    //    }
                        
                    //}
                    //else
                    //{
                    //    Ellipse helpppp = DrawCircle(10, points[i].X, points[i].Y);
                    //    helpppp.Fill = Brushes.Blue;
                    //    drew.Fill = Brushes.Blue;
                    //    if (!IsLeft(points[i + 1], points[i], points[next]) && !IsLeft(points[next], points[i], points[prev]))
                    //    {
                    //        ok = false;
                    //    }
                    //}
                    await Task.Delay(50);
                    if (ok)
                    {
                        segs.Add(toCheck);
                    }
                    else
                    {                       
                        MainCanvas.Children.Remove(drew);
                    }
                    
                }
                //if(segs.Count == n - 3)
                //{
                //    break;
                //}
            }
            for(int i = 0; i < segs.Count; i++)
            {
                DrawLine(segs[i].q, segs[i].p, Colors.Black);
            }
        }

        private async void Otectomie(object sender, RoutedEventArgs e)
        {
            List<Point> tempPoly = new List<Point>();

            List<Segment> segs = new List<Segment>();
            foreach(Point p in points)
            {
                tempPoly.Add(p);
            }
            int n = tempPoly.Count;
            while(n > 3)
            {
                for (int i = 0; i < tempPoly.Count; i++)
                {
                    int next = ( i + 2 ) % n;
                    bool ok = true;
                    Segment toCheck = new Segment(tempPoly[i], tempPoly[next]);
                    Line drew = DrawLine(toCheck.p, toCheck.q, Colors.Red);
                    for (int k = 0; k < segs.Count; k++)
                    {
                        if (toCheck.p == segs[k].p || toCheck.p == segs[k].q || toCheck.q == segs[k].p || toCheck.q == segs[k].q)
                        {
                            continue;
                        }
                        if (toCheck.Intersects(segs[k]))
                        {
                            ok = false;
                            break;
                        }

                    }
                    for (int k = 0; k < tempPoly.Count; k++)
                    {
                        if (k == i || k == next || (k + 1) % n == next || (k + 1) % n == i)
                        {
                            continue;
                        }
                        if (toCheck.Intersects(new Segment(tempPoly[k], tempPoly[(k + 1) % n])))
                        {
                            ok = false;
                            break;
                        }
                    }
                    int prev = i - 1;
                    if (prev < 0)
                    {
                        prev = n - 1;
                    }
                    if (IsLeft(tempPoly[(i + 1) % n], tempPoly[i], tempPoly[prev]))
                    {
                        //Ellipse helpppp = DrawCircle(10, tempPoly[i].X, tempPoly[i].Y);
                        //helpppp.Fill = Brushes.Red;
                        drew.Stroke = Brushes.Red;
                        if (!(IsLeft(tempPoly[(i + 1) % n], tempPoly[i], tempPoly[next]) && IsLeft(tempPoly[next], tempPoly[i], tempPoly[prev])))
                        {
                            ok = false;
                        }

                    }
                    else
                    {
                        //Ellipse helpppp = DrawCircle(10, tempPoly[i].X, tempPoly[i].Y);
                        //helpppp.Fill = Brushes.Blue;
                        drew.Stroke = Brushes.Blue;
                        if (!IsLeft(tempPoly[(i + 1) % n], tempPoly[i], tempPoly[next]) && !IsLeft(tempPoly[next], tempPoly[i], tempPoly[prev]))
                        {
                            ok = false;
                        }
                    }
                    await Task.Delay(50);
                    if (ok)
                    {
                        triangles.Add(new Triangle(toCheck.q, toCheck.p, tempPoly[(i + 1) % n]));
                        segs.Add(toCheck);
                        n--;
                        tempPoly.Remove(tempPoly[(i + 1) % n]);
                        MainCanvas.Children.Remove(drew);
                        DrawLine(toCheck.q, toCheck.p, Colors.Black);                     
                        break;
                    }
                    else
                    {
                        MainCanvas.Children.Remove(drew);
                    }

                }
                
            }
            triangles.Add(new Triangle(tempPoly[0], tempPoly[1], tempPoly[2]));
        }
        private void ColorUp()
        {
            int[] tag = new int[points.Count];
            tag[points.IndexOf(triangles.Last().pts[0])] = 1;
            DrawCircle(10, triangles.Last().pts[0], Colors.Red);
            tag[points.IndexOf(triangles.Last().pts[1])] = 2;
            DrawCircle(10, triangles.Last().pts[1], Colors.LimeGreen);
            tag[points.IndexOf(triangles.Last().pts[2])] = 3;
            DrawCircle(10, triangles.Last().pts[2], Colors.Blue);
            int n = triangles.Count - 1;
            while(n > 0)
            {
                for (int i = triangles.Count - 2; i >= 0; i--)
                {
                    int howmanymissing = 0;
                    int j = 0;
                    for (int l = 0; l < 3; l++)
                    {
                        if (tag[points.IndexOf(triangles[i].pts[l])] == 0)
                        {
                            howmanymissing++;
                            j = l;
                        }
                        
                    }
                    if (howmanymissing == 1)
                    {
                        int alreadytagged = 0;
                        for (int k = 0; k < 3; k++)
                        {
                            if (tag[points.IndexOf(triangles[i].pts[k])] != 0)
                            {
                                alreadytagged += tag[points.IndexOf(triangles[i].pts[k])];
                            }
                        }
                        tag[points.IndexOf(triangles[i].pts[j])] = 6 - alreadytagged;
                        if (tag[points.IndexOf(triangles[i].pts[j])] == 1)
                        {
                            DrawCircle(10, triangles[i].pts[j], Colors.Red);
                        }
                        if (tag[points.IndexOf(triangles[i].pts[j])] == 2)
                        {
                            DrawCircle(10, triangles[i].pts[j], Colors.LimeGreen);
                        }
                        if (tag[points.IndexOf(triangles[i].pts[j])] == 3)
                        {
                            DrawCircle(10, triangles[i].pts[j], Colors.Blue);
                        }
                        n--;
                        break;
                    }
                }
            }          
        }
        public double AreaOfAllTriangles1()
        {
            double area = 0;
            foreach(Triangle t in triangles)
            {
                area += Math.Abs(t.Area());
            }
            
            return area;
        }
        public double AreaOfAllTriangles2()
        {
            double area = 0;
            Point arbitrary = new Point(0, 0);
            for(int i = 0; i < points.Count; i++)
            {
                area += new Triangle(arbitrary, points[i], points[(i + 1) % points.Count]).Area();
            }

            return Math.Abs(area);
        }
        private void ColorUpClick(object sender, RoutedEventArgs e)
        {
            ColorUp();
        }
        private void AreaClick(object sender, RoutedEventArgs e)
        {
            Area1Box.Text = AreaOfAllTriangles1().ToString();
            Area2Box.Text = AreaOfAllTriangles2().ToString();
        }

        private async void SimpleToMonotonPolygonsConvert(object sender, RoutedEventArgs e)
        {
            List<List<Point>> polygons = new List<List<Point>>();
            List<Point> initial = new List<Point>();
            List<Segment> diags = new List<Segment>();
            polygons.Add(initial);
            foreach(Point p in points)
            {
                initial.Add(p);
            }
            Point[] sorted = new Point[points.Count];
            for(int i = 0; i < points.Count; i++)
            {
                sorted[i] = points[i];
                int j = i;
                while(j >= 1 && sorted[j].Y < sorted[j - 1].Y)
                {
                    (sorted[j], sorted[j - 1]) = (sorted[j - 1], sorted[j]);
                    j--;
                }                
            }
            for(int i = 0; i < sorted.Length; i++)
            {
                if (IsReflex(sorted[i], points))
                {
                    int ptindex = points.IndexOf(sorted[i]);
                    Point prev = points[(ptindex - 1 + points.Count) % points.Count];
                    Point next = points[(ptindex + 1) % points.Count];
                    if(prev.Y < sorted[i].Y && next.Y < sorted[i].Y) // vf de unire
                    {
                        Ellipse unire = DrawCircle(10, sorted[i].X, sorted[i].Y);
                        unire.Fill = Brushes.BlueViolet;
                       
                        for(int j = i + 1; j < points.Count; j++)
                        {
                            Point nextafternext = sorted[j];
                            if (IsDiagonalaDinReflex(sorted[i], nextafternext, points))
                            {
                                bool ok = true;
                                bool verybad = false;
                                Segment toadd = new Segment(sorted[i], nextafternext);
                                foreach(Segment s in diags)
                                {
                                    if (s.p == toadd.p && s.q != toadd.q)
                                        continue;
                                    if (s.q == toadd.p && s.p != toadd.q)
                                        continue;
                                    if (s.q == toadd.p && s.p == toadd.q)
                                    {
                                        verybad = true;
                                        break;
                                    }                                       
                                    if(s.q == toadd.q && s.p == toadd.p)
                                    {
                                        verybad = true;
                                        break;
                                    }
                                    if (s.Intersects(toadd))
                                    {
                                        ok = false; 
                                        break;
                                    }                                      
                                }
                                if (verybad)
                                {
                                    break;
                                }
                                if (!ok)
                                {
                                    continue;
                                }
                                DrawLine(sorted[i], nextafternext, Colors.Black);
                                PartitionPoligon(polygons, sorted[i], nextafternext);
                                break;
                            }
                        }
                        

                    }
                    else if(prev.Y > sorted[i].Y && next.Y > sorted[i].Y) // vf de separare
                    {
                        Ellipse unire = DrawCircle(10, sorted[i].X, sorted[i].Y);
                        unire.Fill = Brushes.YellowGreen;

                        for (int j = i - 1; j >= 0; j--)
                        {
                            Point nextafternext = sorted[j];
                            if (IsDiagonalaDinReflex(sorted[i], nextafternext, points))
                            {
                                DrawLine(sorted[i], nextafternext, Colors.Black);
                                PartitionPoligon(polygons, sorted[i], nextafternext);
                                break;
                            }

                        }
                    }
                }
            }
            await Task.Delay(5000);
            foreach(List<Point> poligon in polygons)
            {
                MonotonTriangulate(poligon);
                await Task.Delay(2000);

                //List<Line> drew = new List<Line>();
                for (int i = 0; i < poligon.Count; i++)
                {
                    DrawLine(poligon[i], poligon[(i + 1) % poligon.Count], Colors.Black);
                    await Task.Delay(250);

                }
                await Task.Delay(1000);
                //foreach (Line l in drew)
                //{
                //    MainCanvas.Children.Remove(l);
                //}
            }
        }

        private void PartitionPoligon(List<List<Point>> polygons, Point point, Point nextafternext)
        {
            for(int i = 0; i < polygons.Count; i++)
            {
                if (polygons[i].Contains(point) && polygons[i].Contains(nextafternext))
                {
                    List<Point> toDivide = polygons[i];

                    List<Point> newpoly = new List<Point>();
                    List<Point> toremove = new List<Point>();
                    int P1ind = toDivide.IndexOf(point);
                    int P2ind = toDivide.IndexOf(nextafternext);

                    int Idx = Math.Min(P1ind, P2ind);
                    while (Idx != Math.Max(P1ind, P2ind))
                    {
                        newpoly.Add(toDivide[Idx]);
                        if (Idx != P1ind && Idx != P2ind)
                        {
                            toremove.Add((toDivide[Idx]));
                        }
                        Idx++;
                        Idx %= toDivide.Count;
                    }
                    newpoly.Add(toDivide[Math.Max(P1ind,P2ind)]);
                    foreach (Point p in toremove)
                    {
                        toDivide.Remove(p);
                    }
                    polygons.Add(newpoly);

                    break;
                }
            }
        }

        bool IsDiagonalaDinReflex(Point reflex, Point q, List<Point> pts)
        {
            Point p = reflex;
            bool ok = true;
            int i = pts.IndexOf(p);
            int j = pts.IndexOf(q);
            Point next = pts[(i + 1) % pts.Count];
            Point prev = pts[((i - 1) + pts.Count) % pts.Count];
            Segment toCheck = new Segment(p, q);
            for (int k = 0; k < pts.Count; k++)
            {
                if (k == i || k == j || (k + 1) % pts.Count == j || (k + 1) % pts.Count == i)
                {
                    continue;
                }
                if (toCheck.Intersects(new Segment(pts[k], pts[(k + 1) % pts.Count])))
                {
                    ok = false;
                    break;
                }
            }
            if (!IsLeft(next, p, q) && !IsLeft(q, p, prev))
                ok = false;
            return ok;
        }
        bool IsDiagonalaDinConvex(Point convex, Point q, List<Point> pts)
        {
            Point p = convex;
            bool ok = true;
            int i = pts.IndexOf(p);
            int j = pts.IndexOf(q);
            Point next = pts[(i + 1) % pts.Count];
            Point prev = pts[((i - 1) + pts.Count) % pts.Count];
            Segment toCheck = new Segment(p, q);
            for (int k = 0; k < pts.Count; k++)
            {
                if (k == i || k == j || (k + 1) % pts.Count == j || (k + 1) % pts.Count == i)
                {
                    continue;
                }
                if (toCheck.Intersects(new Segment(pts[k], pts[(k + 1) % pts.Count])))
                {
                    ok = false;
                    break;
                }
            }
            if (!(IsLeft(next, p, q) && IsLeft(q, p, prev)))
                ok = false;
            return ok;
        }
        bool IsReflex(Point p, List<Point> pts)
        {
            int i = pts.IndexOf(p);
            int next = (i + 1) % pts.Count;
            int prev = ((i - 1) + pts.Count) % pts.Count;        
            if (IsLeft(pts[next], points[i], points[prev]))
            {
                return false;
            }
            return true;
        }
        async void MonotonTriangulate(List<Point> points)
        {
            Point[] sorted = new Point[points.Count];
            for (int i = 0; i < points.Count; i++)
            {
                sorted[i] = points[i];
                int j = i;
                while (j >= 1 && sorted[j].Y < sorted[j - 1].Y)
                {
                    (sorted[j], sorted[j - 1]) = (sorted[j - 1], sorted[j]);
                    j--;
                }
            }
            //for (int i = 0; i < sorted.Length; i++)
            //{
            //    Label order = new Label() { Content = i.ToString() };
            //    Canvas.SetTop(order, sorted[i].Y);
            //    Canvas.SetLeft(order, sorted[i].X);
            //    MainCanvas.Children.Add(order);
            //}
            List<Point> ALine = new List<Point>();
            List<Point> BLine = new List<Point>();
            Point MinPoint = sorted.Last();
            Point MaxPoint = sorted.First();
            int Adx = points.IndexOf(MaxPoint);
            int Bdx = points.IndexOf(MinPoint);
            while (Adx != points.IndexOf(MinPoint))
            {
                ALine.Add(points[Adx]);
                DrawLine(points[Adx], points[(Adx + 1) % points.Count], Colors.Red);
                Adx++;
                Adx = Adx % points.Count;
            }
            ALine.Add(points[Adx]);
            while (Bdx != points.IndexOf(MaxPoint))
            {
                BLine.Add(points[Bdx]);
                DrawLine(points[Bdx], points[(Bdx + 1) % points.Count], Colors.Blue);
                Bdx++;
                Bdx = Bdx % points.Count;
            }
            BLine.Add(points[Bdx]);

            Stack<Point> stack = new Stack<Point>();
            stack.Push(sorted[0]);
            stack.Push(sorted[1]);

            for (int j = 2; j < points.Count - 1; j++)
            {

                if ((ALine.Contains(stack.Peek()) && !ALine.Contains(sorted[j])) || (BLine.Contains(stack.Peek()) && !BLine.Contains(sorted[j])))
                {
                    //this.Background = Brushes.BlanchedAlmond;
                    while (stack.Count > 1)
                    {
                        Point TopOfStack = stack.Pop();
                        DrawLine(sorted[j], TopOfStack, Colors.Black);
                        //await Task.Delay(1000);
                    }
                    if (stack.Count > 0)
                    {
                        stack.Pop();
                    }
                    stack.Push(sorted[j - 1]);
                    stack.Push(sorted[j]);
                }
                else
                {
                    //this.Background = Brushes.White;
                    List<Point> temp = new List<Point>();
                    Point LastDeleted = stack.Pop();
                    for (int i = 0; i < stack.Count; i++)
                    {
                        temp.Add(stack.Pop());
                    }
                    for (int i = temp.Count - 1; i >= 0; i--)
                    {
                        Point tocheck = temp[i];
                        bool ok = true;
                        //for (int k = 0; k < points.Count; k++)
                        //{
                        //    if ((tocheck == points[k] && sorted[j] == points[(k + 1) % points.Count]) || (tocheck == points[(k + 1) % points.Count] && sorted[j] == points[k]))
                        //        ok = false;
                        //}
                        if (IsDiagonala(sorted[j], tocheck, points) && ok)
                        {
                            DrawLine(sorted[j], tocheck, Colors.Gray);
                            LastDeleted = tocheck;
                        }
                        else
                        {
                            //Line drew = DrawLine(sorted[j], tocheck, Colors.Red);
                            //await Task.Delay(1000);
                            //MainCanvas.Children.Remove(drew);
                            stack.Push(tocheck);
                        }
                    }

                    stack.Push(LastDeleted);

                    stack.Push(sorted[j]);
                }
                await Task.Delay(100);
            }
            stack.Pop();
            for (int i = 0; i < stack.Count - 1; i++)
            {
                DrawLine(sorted.Last(), stack.Pop(), Colors.DarkBlue);
            }
        }
        private void MonotonTriangulate_Click(object sender, RoutedEventArgs e)
        {
            MonotonTriangulate(points);

        }
        bool IsDiagonala(Point from,Point to, List<Point> pts)
        {
            Segment tocheck = new Segment(from, to);
            for (int k = 0; k < points.Count; k++)
            {
                if (k == points.IndexOf(from) || k == points.IndexOf(to) || (k + 1) % points.Count == points.IndexOf(to) || (k + 1) % points.Count == points.IndexOf(from))
                {
                    continue;
                }
                if (tocheck.Intersects(new Segment(points[k], points[(k + 1) % points.Count])))
                {
                    return false;
                }
            }
            if (IsReflex(from, pts))
            {
                if(IsDiagonalaDinReflex(from, to, pts))
                {
                    return true;
                }
            }
            else
            {
                if(IsDiagonalaDinConvex(from, to , pts))
                {
                    return true;
                }
            }
            return false;
        }

    }

    class Triangle
    {
        public Point[] pts { get; set; }
        public Triangle(Point p1, Point p2, Point p3)
        {
            pts = new Point[] { p1 , p2 , p3 };
        }
        public double Area()
        {
            return 0.5 * ( (pts[0].X * (pts[1].Y - pts[2].Y)) + (pts[1].X * (pts[2].Y - pts[0].Y)) + (pts[2].X * (pts[0].Y - pts[1].Y)) );
        }
    }
    class Segment
    {
        public Point p;
        public Point q;
        public double Dist;
        public Segment(Point p, Point q)
        {
            this.p = p;
            this.q = q;
            Dist = GetDist();
        }
        public Segment(Line l)
        {
            this.p = new Point(l.X1, l.Y1);
            this.q = new Point(l.X2, l.Y2);
            Dist = GetDist();
        }
        public bool Intersects(Segment other)
        {
            return doIntersect(this, other);
        }
        bool doIntersect(Segment s1, Segment s2)
        {
            Point p1 = s1.p, q1 = s1.q, p2 = s2.p, q2 = s2.q;

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
            double val = (q.Y - p.Y) * (r.X - q.X) -
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
        double GetDist()
        {
            return Math.Sqrt((p.X - q.X) * (p.X - q.X) + (p.Y - q.Y) * (p.Y - q.Y));
        }
    }
}
