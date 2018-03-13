using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ShootingGame
{
    public class Rock : Shape
    {
        public readonly GraphicsPath r_model = null;

        

        public Rock(PointF pos, int _small)
            : base(pos, _small)
        {
            r_model = GetPoly(Shape.rand.Next(6, 11), size, Shape.rand.Next(0, 30));
        }


        public override GraphicsPath GetPath()
        {
            GraphicsPath gpLocal = (GraphicsPath)r_model.Clone();

            Matrix mat = new Matrix();

            mat.Translate(base.Pos.X, base.Pos.Y);
            mat.Rotate(_fRot);
            gpLocal.Transform(mat);

            if (Pos.X > (base.clientSize.Width - size))
            {
                GraphicsPath gpWrap = (GraphicsPath)r_model.Clone();
                Matrix mat2 = new Matrix();
                mat2.Translate((Pos.X - base.clientSize.Width), Pos.Y);
                mat2.Rotate(_fRot, MatrixOrder.Prepend);
                gpWrap.Transform(mat2);
                gpLocal.AddPath(gpWrap, true);
            }

            else if (Pos.X < size)
            {
                GraphicsPath gpWrap = (GraphicsPath)r_model.Clone();
                Matrix mat2 = new Matrix();
                mat2.Translate(Pos.X + base.clientSize.Width, Pos.Y);
                mat2.Rotate(_fRot, MatrixOrder.Prepend);
                gpWrap.Transform(mat2);
                gpLocal.AddPath(gpWrap, true);
            }

            if (Pos.Y > (base.clientSize.Height - size))
            {
                GraphicsPath gpWrap = (GraphicsPath)r_model.Clone();
                Matrix mat2 = new Matrix();
                mat2.Translate(Pos.X, Pos.Y - base.clientSize.Height);
                mat2.Rotate(_fRot, MatrixOrder.Prepend);
                gpWrap.Transform(mat2);
                gpLocal.AddPath(gpWrap, true);
            }

            else if (Pos.Y < size)
            {
                GraphicsPath gpWrap = (GraphicsPath)r_model.Clone();
                //Do translation or movement
                Matrix mat2 = new Matrix();
                mat2.Translate(Pos.X, Pos.Y + base.clientSize.Height);
                mat2.Rotate(_fRot, MatrixOrder.Prepend);
                gpWrap.Transform(mat2);
                gpLocal.AddPath(gpWrap, true);
            }

            return gpLocal;
        }

    }
}
