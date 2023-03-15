using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GC_C3_03_13_2023
{
    struct Segment
    {
        public Point start { get; set; } 
        public Point end { get; set; }
        public bool Swept { get; set; }
        public int ID { get; set; }

        public Segment(Point p, Point q, int id)
        {
            ID = id;
            Swept = false;
            if(p.X < q.X)
            {
                start = p;
                end = q;
            }
            else if(p.X == q.X)
            {
                if(p.Y < q.Y)
                {
                    start = p;
                    end = q;
                }
            }
            else
            {
                start = q;
                end = p;
            }
        }
        public override string ToString()
        {
            return $"({start.X},{start.Y}) -> ({end.X},{end.Y})";
        }
    }
    struct SweepPoint
    {
        public Point P { get; set; }
        public bool IsStartPoint { get; set; }
        public Segment Parent { get; set; }
        public bool IsIntersection { get; set; }

        public Segment Parent2 { get; set; }

        public SweepPoint(Segment S, bool isStart)
        {
            Parent = S;
            //Parent2 = S;
            if (isStart)
            {
                IsStartPoint = true;
                P = S.start;
            }
            else
            {
                IsStartPoint = false;
                P = S.end;
            }
            IsIntersection = false;
        }
        public SweepPoint(Segment S, Segment S2, Point I)
        {
            Parent = S;
            Parent2 = S2;
            P = I;
            IsStartPoint = false;
            IsIntersection = true;
        }
        public override string ToString()
        {
            return P.ToString();
        }
    }
}
