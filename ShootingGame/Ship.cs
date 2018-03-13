using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace ShootingGame
{
    public class Ship
    {
        private Random rand = new Random();

        public Boolean Protect { get; set; } = false;
        public Boolean IsMarkedForDeath { get; set; } = false;
        public GraphicsPath _model { get; private set; } = null;
        public float Scale { private get; set; } = 1;
        public float Rotation { get; set; } = 0;
        public PointF Translation { private get; set; }

        public PointF Head { get; set; }

        public PointF Tail { private get; set; }

        private List<RectangleF> l = new List<RectangleF>();

        public PointF[] cloneP = new PointF[100];

        private Size cSize;

        public float scale = 0.6f;

        private const int widthMainBody = 20;
        private const int heighMainBody = 30;

        public void DrawShip(BufferedGraphics bg,Ship shipModel)
        {
            if (!IsMarkedForDeath)
                bg.Graphics.FillPath(new SolidBrush(System.Drawing.Color.Aqua), shipModel.GetPath());
            else
                bg.Graphics.FillPath(new SolidBrush(Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255))), shipModel.GetPath());
        }

        public GraphicsPath GetPath()
        {
            GraphicsPath gpClone = (GraphicsPath)_model.Clone();

            Matrix mat = new Matrix();

            mat.Translate(Translation.X, Translation.Y);
            mat.Rotate(Rotation, MatrixOrder.Prepend);
            mat.Scale(scale, scale);               

            gpClone.Transform(mat);

            cloneP = gpClone.PathPoints;
            Head = new PointF(cloneP[10].X + widthMainBody / 2, cloneP[10].Y);
            Tail = new PointF(cloneP[10].X + widthMainBody / 2, cloneP[10].Y + heighMainBody);
            //Console.WriteLine("Inside Ship, Head Point is: " + Head);

            if (Translation.X > (cSize.Width - widthMainBody))
            {
                GraphicsPath gpWrap = (GraphicsPath)_model.Clone();
                Matrix mat2 = new Matrix();
                mat2.Translate((Translation.X - cSize.Width), Translation.Y);
                mat2.Rotate(Rotation, MatrixOrder.Prepend);
                mat2.Scale(scale, scale);
                gpWrap.Transform(mat2);
                gpClone.AddPath(gpWrap, true);
            }

            else if (Translation.X < widthMainBody)
            {
                GraphicsPath gpWrap = (GraphicsPath)_model.Clone();
                Matrix mat2 = new Matrix();
                mat2.Translate(Translation.X + cSize.Width, Translation.Y);
                mat2.Rotate(Rotation, MatrixOrder.Prepend);
                mat2.Scale(scale, scale);
                gpWrap.Transform(mat2);
                gpClone.AddPath(gpWrap, true);
            }

            if (Translation.Y > (cSize.Height - widthMainBody))
            {
                GraphicsPath gpWrap = (GraphicsPath)_model.Clone();
                Matrix mat2 = new Matrix();
                mat2.Translate(Translation.X, Translation.Y - cSize.Height);
                mat2.Rotate(Rotation, MatrixOrder.Prepend);
                mat2.Scale(scale, scale);
                gpWrap.Transform(mat2);
                gpClone.AddPath(gpWrap, true);
            }

            else if (Translation.Y < widthMainBody)
            {
                GraphicsPath gpWrap = (GraphicsPath)_model.Clone();
                //Do translation or movement
                Matrix mat2 = new Matrix();
                mat2.Translate(Translation.X, Translation.Y + cSize.Height);
                mat2.Rotate(Rotation, MatrixOrder.Prepend);
                mat2.Scale(scale, scale);
                gpWrap.Transform(mat2);
                gpClone.AddPath(gpWrap, true);
            }








            return gpClone;
        }

        public void getSize(Size ClientSize)
        {
            cSize = ClientSize;
        }


        public Ship()
        {
            

            l.Add(new RectangleF(-10, -15, widthMainBody, heighMainBody));
            l.Add(new RectangleF(-20, -5, 10, 10));
            l.Add(new RectangleF(10, -5, 10, 10));

            Head = new PointF(0, 25);

            _model = new GraphicsPath();
            _model.FillMode = FillMode.Winding;

            _model.AddLine(-20, -30, -20, 30);
            _model.AddLine(-20, 30, -35, 20);
            _model.AddLine(-35, 20, -35, -20);
            _model.AddLine(-35, -20, -20, -30);

            _model.AddLine(20, -30, 20, 30);
            _model.AddLine(20, 30, 35, 20);
            _model.AddLine(35, 20, 35, -20);
            _model.AddLine(35, -20, 20, -30);

            
            _model.AddRectangles(l.ToArray());

        }


    }
}
