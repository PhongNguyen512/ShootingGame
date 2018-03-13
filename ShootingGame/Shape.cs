using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ShootingGame
{
    public abstract class Shape
    {
        public int small { get; set; } = 0;

        public bool Hit { get; set; }

        public PointF Pos;//{ get; set; }

        public Boolean IsMarkedForDeath { get; set; } = false;

        protected float _fRot;

        public float _fRotInc;

        public float SpeedX;

        public float SpeedY;

        public static Random rand = new Random();

        public const int maxPolySize = 50;

        public int size = 0;

        private Color c;

        protected Size clientSize;

        public Shape(PointF p, int _small)
        {
            Hit = false;
            small = _small;
            Pos = p;
            _fRot = 0;
            _fRotInc = (float)(rand.NextDouble() * (3.0 - -3.0) + -3.0);
            SpeedX = (float)(rand.NextDouble() * (2.5 - -2.5) + -2.5);
            SpeedY = (float)(rand.NextDouble() * (2.5 - -2.5) + -2.5);

            if (_small == 1)
                size = 50;
            else if (_small == 2)
                size = 40;
            else
                size = 30;
        }


        public void Render(BufferedGraphics a, Size maxSize)
        {
            if (small == 1)
                c = Color.Blue;
            else if (small == 2)
                c = Color.Yellow;
            else
                c = Color.Green;
                                         
            clientSize = maxSize;
            a.Graphics.DrawPath(new Pen( c ), GetPath());
            Tick(clientSize);
            //a.Graphics.DrawEllipse(new Pen(Color.White), new Rectangle((int)Pos.X - maxPolySize, (int)Pos.Y - maxPolySize, maxPolySize * 2, maxPolySize * 2));
        }

        public void RenderSafe(BufferedGraphics a, Size maxSize)
        {

            clientSize = maxSize;
            a.Graphics.DrawPath(new Pen(Color.FromArgb(rand.Next(256), rand.Next(256),rand.Next(256))), GetPath());
            Tick(clientSize);
            
        }

        public abstract GraphicsPath GetPath();

        public void Tick(Size maxSize)
        {
            //if (size == 0)
            //    size = maxPolySize / small;

            _fRot += _fRotInc;
            //if ((Pos.X - maxPolySize + SpeedX > 0) && (Pos.X + maxPolySize + SpeedX < maxSize.Width))
            //    Pos.X += SpeedX;
            //else
            //{
            //    SpeedX *= -1;

                //if (Pos.X - maxPolySize + SpeedX < 0)
                //    Pos.X = maxPolySize;
                //else if (Pos.X + maxPolySize + SpeedX > maxSize.Width)
                //    Pos.X = 0;
            //}

            //if ((Pos.Y - maxPolySize + SpeedY > 0) && (Pos.Y + maxPolySize + SpeedY < maxSize.Height))
            //    Pos.Y += SpeedY;
            //else
            //{
               // SpeedY *= -1;

                //if (Pos.Y - maxPolySize + SpeedY < 0)
                //    Pos.Y = maxPolySize;
                //else if (Pos.Y + maxPolySize + SpeedY > maxSize.Height)
                //    Pos.Y =0;

            //}
            Pos.X += SpeedX;
            Pos.Y += SpeedY;


            if (Pos.X > maxSize.Width + 1)
                Pos.X = 0;
            else if (Pos.X < -1)
                Pos.X = maxSize.Width;
            else if (Pos.Y > maxSize.Height + 1)
                Pos.Y = 0;
            else if (Pos.Y < -1)
                Pos.Y = maxSize.Height;
        }


        public GraphicsPath GetPoly(int side, float radMax, float radVariance)
        {
            List<PointF> verts = new List<PointF>();
            double angle = 0;

            for (int i = 0; i < side; i++, angle += (Math.PI * 2) / side)
            {
                float radLocal = (float)(rand.NextDouble() * radVariance);
                verts.Add(new PointF((float)(Math.Cos(angle) * (radMax - radLocal)), (float)(Math.Sin(angle) * (radMax - radLocal))));
            }

            GraphicsPath local = new GraphicsPath();
            local.AddPolygon(verts.ToArray());
            return local;
        }


    }

}
