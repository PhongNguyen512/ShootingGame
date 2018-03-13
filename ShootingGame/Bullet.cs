using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace ShootingGame
{
    public class Bullet
    {

        public GraphicsPath _bullet { get; private set; } = null;

        public PointF Translation { get; set; }

        public float Rotation { get; set; } = 0;

        public PointF Direction;

        public static List<Bullet> ListBullet = new List<Bullet>();

        public PointF[] cloneP = new PointF[100];

        public Boolean IsMarkedForDeath { get; set; } = false;

        public GraphicsPath GetPath()
        {
            GraphicsPath gpClone = (GraphicsPath)_bullet.Clone();

            Matrix mat = new Matrix();

            mat.Translate(Translation.X, Translation.Y);           

            gpClone.Transform(mat);

            cloneP = gpClone.PathPoints;        

            return gpClone;
        }

        public void MoveBullet()
        {
            Translation = new PointF(Translation.X + (float)Math.Sin(Rotation * Math.PI / 180) * 5, Translation.Y - (float)Math.Cos(Rotation * Math.PI / 180) * 5);
        }

        public Bullet()
        {
            _bullet = new GraphicsPath();
            _bullet.FillMode = FillMode.Winding;

            //_bullet.AddRectangle(new RectangleF (-50, , 10, 10));
            _bullet.AddEllipse(-5, -5, 10, 10);
        }




    }
}
