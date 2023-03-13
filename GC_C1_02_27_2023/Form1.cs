using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace GC_C1_02_27_2023
{
    public partial class Form1 : Form
    {
        bool wasDrawn = false;
        public Form1()
        {
            InitializeComponent();
            Height = 800;
            Width = 800;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (!wasDrawn)
            {
                Graphics g = e.Graphics;
                //Ex_1(g);
                //Ex_2(g);
                Ex_3(g);
                wasDrawn = true;
            }          
        }
        private void Ex_3(Graphics g) 
        {
            // se da o multime de puncte si un punct Q, se sa afle cercul de raza maxima care il contine doar pe Q.
            Random rng = new Random();
            Pen p = new Pen(Color.Black, 3);
            int n = rng.Next(10, 50);
            int x = 0, y = 0;
            Point[] M = new Point[n];
            for (int i = 0; i < n; i++)
            {
                x = rng.Next(10, (int)this.ClientSize.Width - 10);
                y = rng.Next(10, (int)this.ClientSize.Height - 10);
                M[i] = new Point(x, y);
                g.DrawEllipse(p, x, y, 3, 3);
            }
            // pct Q:
            x = rng.Next(10, (int)this.ClientSize.Width - 10);
            y = rng.Next(10, (int)this.ClientSize.Height - 10);
            Point Q = new Point(x, y);
            p.Color = Color.Red;
            g.DrawEllipse(p, x, y, 3, 3);

            Point Closest = new Point(0, 0);
            double mindist = 1600000;
            for(int i = 0; i < n; i++)
            {
                double check = Math.Sqrt((x - M[i].X) * (x - M[i].X) + (y - M[i].Y) * (y - M[i].Y));
                if (check < mindist)
                {
                    mindist = check;
                    Closest = M[i];
                }
            }

            p.Width = 1;
            p.Color = Color.Gray;
            g.DrawLine(p, Q, Closest);
            g.DrawEllipse(p, x - (int)mindist, y - (int)mindist, (int)(2*mindist), (int)(2*mindist));

        }
        private void Ex_2(Graphics g)
        {
            // se dau doua multimi M1, M2 , sa se afle cel mai scurt drum al fiecarui punct din M1 catre un punct din M2.
            Random rng = new Random();
            Pen p = new Pen(Color.Black, 3);
            int n = rng.Next(10, 50);
            Point[] M2 = new Point[n];
            for(int i = 0; i < n; i++)
            {
                int x = rng.Next(10, (int)this.ClientSize.Width - 10);
                int y = rng.Next(10, (int)this.ClientSize.Height - 10);
                M2[i] = new Point(x, y);
                g.DrawEllipse(p, x, y, 3, 3);
            }
            n = rng.Next(10, 50);
            double distance = 0;
            Point closest = new Point(0, 0);
            
            for (int i = 0; i < n; i++)
            {
                distance = 1500;
                int x = rng.Next(10, (int)this.ClientSize.Width - 10);
                int y = rng.Next(10, (int)this.ClientSize.Height - 10);
                Point pp = new Point(x, y);
                p.Color = Color.Red;
                p.Width = 3;
                g.DrawEllipse(p, x, y, 3, 3);

                for (int j = 0; j < M2.Length; j++)
                {
                    double check = Math.Sqrt((x - M2[j].X) * (x - M2[j].X) + (y - M2[j].Y) * (y - M2[j].Y));
                    if(check < distance)
                    {
                        distance = check;
                        closest = M2[j];
                    }
                }
                               
                p.Color = Color.Blue;
                p.Width = 1;
                g.DrawLine(p, closest, pp);
            }
        }
        private void Ex_1(Graphics g)
        {
            // se da o multime de puncte in plan, sa se determine dreptunghiul cu aria cea mai mica care cuprinde toate punctele
            Random rng = new Random();
            Pen p = new Pen(Color.Black, 3);
            int n = rng.Next(10,150);
            int x_min = 800, x_max = 0, y_min = 800, y_max = 0;
            for (int i = 0; i < n; i++)
            {
                int x = rng.Next(10, (int)this.ClientSize.Width - 10);
                int y = rng.Next(10, (int)this.ClientSize.Height - 10);
                g.DrawEllipse(p, x, y, 3, 3);
                
                x_min = Math.Min(x, x_min);
                x_max = Math.Max(x, x_max);
                y_min = Math.Min(y, y_min);
                y_max = Math.Max(y, y_max);
            }
            p.Color = Color.Red;
            g.DrawRectangle(p, x_min, y_min, x_max - x_min + 3, y_max - y_min + 3);
        }
    }
}