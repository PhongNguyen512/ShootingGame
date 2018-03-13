using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ShootingGame
{
    public class GameInfo
    {
        public int Score { get; private set; } = 0;
        public int Life { get; set; } = 0;
        public int Level { get; set; } = 0;

        private Font f = new Font(FontFamily.GenericMonospace, 15);

        private Font f3 = new Font(FontFamily.GenericMonospace, 8);

        private Font f2 = new Font(FontFamily.GenericMonospace, 30);

        private Random rand = new Random();

        public void Display(BufferedGraphics a, List<Shape> list)
        {
            
            a.Graphics.DrawString("Score:" + Score.ToString("D6"), f, new SolidBrush(Color.White), 10, 10);
            a.Graphics.DrawString("Life:" + Life.ToString(), f, new SolidBrush(Color.White), 10, 30);
            a.Graphics.DrawString("Level:" + Level.ToString(), f, new SolidBrush(Color.White), 10, 50);
            a.Graphics.DrawString("Rock left:" + list.Count().ToString(), f, new SolidBrush(Color.White), 10, 70);

            a.Graphics.DrawString("W/A/S/D - Up/Left/Down/Right", f3, new SolidBrush(Color.White), 10, 100);
            a.Graphics.DrawString("Space/Xbox A - Shot", f3, new SolidBrush(Color.White), 10, 115);
            a.Graphics.DrawString("Esc/Xbox Start - Up/Left/Down/Right", f3, new SolidBrush(Color.White), 10, 130);
            a.Graphics.DrawString("The ship will stop moving for few seconds after touch the rock", f3, new SolidBrush(Color.White), 10, 145);
        }
        public void GameOver(BufferedGraphics a, Size s)
        {

            a.Graphics.DrawString("Game Over", f2, new SolidBrush(Color.White), rand.Next(40,s.Width-40) , rand.Next(40, s.Height - 40));

            a.Graphics.DrawString("Error 404...Where my SHIP...", f, new SolidBrush(Color.White), s.Width/2 -100, s.Height/2);

            a.Graphics.DrawString("Press Enter / Xbox B to Restart", f, new SolidBrush(Color.Red), s.Width / 2 - 100, 50);
        }

        public void addScore(int Add)
        {
            Score += Add;
        }

    }
}
