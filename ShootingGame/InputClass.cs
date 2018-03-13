using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using System.Drawing;

namespace ShootingGame
{
    public class InputClass
    {
        public Boolean Up { get; set; } = false;
        public Boolean Down { get; set; } = false;
        public Boolean Left { get; set; } = false;
        public Boolean Right { get; set; } = false;
        public Boolean Space { get; set; } = false;
        public Boolean Pause { get; set; } = false;
        public Boolean ShipDead { get; set; } = false;
        public Boolean Restart { get; set; } = false;

        //property contain key value
        //private List<Keys> oldKey = new List<Keys>(5);

        //private List<Keys> uKey = new List<Keys>(5);

        //private List<Keys> dKey = new List<Keys>(5);

        private Keys newKey;

        private Keys upKey;

        //function for get new key from main form
        public void getKey(Keys k)
        {
            newKey = k;
            KeyboardUpdate();
            //Console.WriteLine(key);
        }

        //function for check the pressed key is release or not
        public void KeyUp(Keys k)
        {
            upKey = k;
            KeyboardUpdate();
            //Console.WriteLine(key);
        }

        public void KeyboardUpdate()
        {
            if (!newKey.ToString().Equals("None"))
            {
                switch (newKey)
                {
                    case Keys.A:
                        Left = true;
                        break;
                    case Keys.W:
                        Up = true;
                        break;
                    case Keys.D:
                        Right = true;
                        break;
                    case Keys.S:
                        Down = true;
                        break;
                    case Keys.Space:
                        Space = true;
                        break;
                    case Keys.Escape:
                        if (!Pause)
                            Pause = true;
                        else
                            Pause = false;
                        break;
                    case Keys.Enter:
                        Restart = true;
                        break;
                }
                newKey = new Keys();
            }
            else if (!upKey.ToString().Equals("None"))
            {
                switch (upKey)
                {
                    case Keys.A:
                        Left = false;
                        break;
                    case Keys.W:
                        Up = false;
                        break;
                    case Keys.D:
                        Right = false;
                        break;
                    case Keys.S:
                        Down = false;
                        break;
                    case Keys.Space:
                        Space = false;
                        break;
                    case Keys.Enter:
                        Space = false;
                        break;
                }
                upKey = new Keys();
            }

        }

        public void ResetData()
        {
            Up = false;
            Down = false;
            Left = false;
            Right = false;
            Space = false;
            Pause = false;
            ShipDead = false;
            Restart = false;       
            newKey = new Keys();
            upKey = new Keys();
        }


        public void UpdateInput()
        {
            while (true)
            {
                Thread.Sleep(0);
                GamePadState gamePad = GamePad.GetState(PlayerIndex.One);

                //xbox controller part
                if (gamePad.IsConnected)
                {


                    if (!ShipDead || !Pause)
                    {
                        //if (gamePad.ThumbSticks.Left.X <0)
                        //    Left = true;
                        //else
                        //    Left = false;

                        //if (gamePad.ThumbSticks.Left.X >0)
                        //    Right = true;
                        //else
                        //    Right = false;

                        //if (gamePad.ThumbSticks.Left.Y >0)
                        //    Up = true;
                        //else
                        //    Up = false;

                        //if (gamePad.ThumbSticks.Left.Y <0)
                        //    Down = true;
                        //else
                        //    Down = false;




                        if (gamePad.DPad.Left.Equals(ButtonState.Pressed))
                            Left = true;
                        else
                            Left = false;

                        if (gamePad.DPad.Right.Equals(ButtonState.Pressed))
                            Right = true;
                        else
                            Right = false;

                        if (gamePad.DPad.Up.Equals(ButtonState.Pressed))
                            Up = true;
                        else
                            Up = false;

                        if (gamePad.DPad.Down.Equals(ButtonState.Pressed))
                            Down = true;
                        else
                            Down = false;

                        if (gamePad.Buttons.A.Equals(ButtonState.Pressed))
                            Space = true;
                        else
                            Space = false;

                        if (gamePad.Buttons.Start.Equals(ButtonState.Pressed))
                        {
                            if (!Pause)
                                Pause = true;
                            else
                                Pause = false;
                            Thread.Sleep(300);
                        }
                        
                    }
                    else
                    {
                        if (gamePad.Buttons.B.Equals(ButtonState.Pressed))
                            Restart = true;
                        else
                            Restart = false;
                    }

                }
            }

        }


    }
}
