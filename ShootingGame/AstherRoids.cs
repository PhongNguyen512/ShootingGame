using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Threading;
//using UsingController;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Media;
using System.IO;

namespace ShootingGame
{
    public partial class AstherRoids : Form
    {
        //for rock
        public List<Shape> shapeLIST = new List<Shape>();
        LinkedList<Region> regionLIST = new LinkedList<Region>();

        private Random rand = new Random();

        private float rot = 0;

        private InputClass input = new InputClass();

        private Thread inputThread = null;

        public Ship shipModel;

        private List<Bullet> listBullet = new List<Bullet>();

        public PointF centerPoint;

        public PointF MovePoint;

        public PointF Direction;

        private bool Move = false;

        private Font foo = null;

        private GameInfo info = new GameInfo();

        private Stopwatch sw = new Stopwatch();

        private Stopwatch sw4Protect = new Stopwatch();

        protected Size s;
        
        //Sometime visual studio show errors about missing music files in Resources.
        //Unknow reason. 
        //Solution: Go to Resources of the solution properties. Delete current file and add again in Additional Files folder      
        SoundPlayer endSound = new SoundPlayer(Properties.Resources.Computer_Error_Song);

        SoundPlayer ingameSound = new SoundPlayer(Properties.Resources.Batman_Theme);

        private bool inSound = false;


        public AstherRoids()
        {
            InitializeComponent();
        }

        private void AstherRoids_KeyDown(object sender, KeyEventArgs e)
        {
            //start the input thread
            //if it not start yet
            if (!inputThread.IsAlive)
                inputThread.Start();

            input.getKey((Microsoft.Xna.Framework.Input.Keys)e.KeyCode);
        }

        private void AstherRoids_KeyUp(object sender, KeyEventArgs e)
        {
            input.KeyUp((Microsoft.Xna.Framework.Input.Keys)e.KeyData);
        }

        private void AstherRoids_Load(object sender, EventArgs e)
        {
            s = ClientSize;
            StartTheGame();

            foo = new Font(FontFamily.GenericSansSerif, 20);
            //initialize the thread but not START yet          
            inputThread = new Thread(input.UpdateInput);
            inputThread.IsBackground = true;
            inputThread.Start();
        }

        private void StartTheGame()
        {
            shapeLIST.Clear();
            regionLIST.Clear();
            listBullet.Clear();
            centerPoint = new PointF();
            MovePoint = new PointF();
            Direction = new PointF();

            Move = false;
                      
            rot = 0;
            input.ResetData(); 
            shipModel = new Ship();

            //initialize the info of game
            info.Life = 3;
            info.Level = 0;
            //ProtectShip();

            //start music for in game
            if (!inSound)
            {
                endSound.Stop();
                ingameSound.PlayLooping();
                inSound = true;
            }       
        }


        //Timer only for adding bullet
        private void BulletTimer_Tick(object sender, EventArgs e)
        {
            if (!shipModel.IsMarkedForDeath)
            {
                if (input.Space)
                {
                    if (listBullet.Count < 9)
                    {
                        Bullet b = new Bullet();
                        b.Rotation = shipModel.Rotation;
                        b.Translation = new PointF(MovePoint.X + (float)Math.Sin(rot * Math.PI / 180) * 10
                            , MovePoint.Y - (float)Math.Cos(rot * Math.PI / 180) * 10);
                        listBullet.Add(b);
                    }
                }
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {      
            //get the center point of watever size of form 1
            centerPoint = new PointF(ClientRectangle.Width / 2, ClientRectangle.Height / 2);

            //get the first Move point by the first location in form 1
            if (MovePoint.Equals(new PointF()))
                MovePoint = new PointF(centerPoint.X, centerPoint.Y);

            shipModel.getSize(ClientSize);

            //double buffer
            using (BufferedGraphicsContext bgc = new BufferedGraphicsContext())
            {
                using (BufferedGraphics bg = bgc.Allocate(CreateGraphics(), ClientRectangle))
                {
                    if (info.Life > 0)
                    {
                        bg.Graphics.Clear(System.Drawing.Color.Black);
                        //render smoothly
                        bg.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        info.Display(bg, shapeLIST);
                        if (!input.Pause)
                        {
                            //control from keyboard
                            //
                            if (!shipModel.IsMarkedForDeath)
                                checkInput();
                            else
                            {
                                TakeABreak();
                                input.ShipDead = true;
                            }

                            //move ship around
                            if (!Move)
                                shipModel.Translation = centerPoint;
                            else
                            {
                                if (MovePoint.X > ClientSize.Width + 1)
                                    MovePoint = new PointF(0, MovePoint.Y);
                                else if (MovePoint.X < -1)
                                    MovePoint = new PointF(ClientSize.Width, MovePoint.Y);
                                else if (MovePoint.Y > ClientSize.Height + 1)
                                    MovePoint = new PointF(MovePoint.X, 0);
                                else if (MovePoint.Y < 0)
                                    MovePoint = new PointF(MovePoint.X, ClientSize.Height);


                                shipModel.Translation = MovePoint;
                            }


                            //draw bullet
                            //move bullet
                            //check intersect
                            if (listBullet.Count > 0)
                            {
                                foreach (Bullet item in listBullet)
                                {
                                    bg.Graphics.FillPath(new SolidBrush(System.Drawing.Color.Red), item.GetPath());

                                    item.MoveBullet();

                                    Intersection(bg, item);
                                    
                                    if (!checkBullet(item))
                                    {
                                        item.IsMarkedForDeath = true;
                                        break;
                                    }

                                }
                            }

                            //create amount of rock
                            if (shapeLIST.Count == 0)
                            {
                                Init(info.Level);
                                info.Level++;
                                ProtectShip();
                            }
                                
                            //draw the rock
                            //shapeLIST.ForEach(o => o.Render(bg, ClientSize));

                            shipModel.Rotation = rot;

                            //bg.Graphics.FillPath(new SolidBrush(System.Drawing.Color.Aqua), shipModel.GetPath());
                            shipModel.DrawShip(bg, shipModel);

                            listBullet.RemoveAll(o => o.IsMarkedForDeath);

                            //calculate score and delete rock
                            for (int i = 0; i < shapeLIST.Count; i++)
                            {
                                if (shapeLIST[i].IsMarkedForDeath)
                                {
                                    if (shapeLIST[i].small == 1)
                                        info.addScore(100);
                                    else if (shapeLIST[i].small == 2)
                                        info.addScore(200);
                                    else if (shapeLIST[i].small == 3)
                                        info.addScore(300);

                                    if (info.Life < 6)
                                    {
                                        if (info.Score == 10000)
                                        {
                                            info.Life++;
                                            shipModel.scale += 0.1f;
                                        }
                                    }

                                    shapeLIST.RemoveAt(i);
                                }
                                //check intersect rock with ship//////
                                else
                                {

                                    //Console.WriteLine(sw4Protect.Elapsed.ToString());
                                    if (sw4Protect.ElapsedMilliseconds > 5000)
                                    {
                                        sw4Protect.Stop();
                                        sw4Protect.Reset();
                                        shipModel.Protect = false;
                                    }

                                    if (!shipModel.Protect)
                                    {
                                        Intersection(bg, shapeLIST[i]);
                                        shapeLIST[i].Render(bg, ClientSize);
                                    }
                                    else
                                    {
                                        shapeLIST[i].RenderSafe(bg, ClientSize);
                                    }
                                }
                                   
                            }                    
                        }
                        else
                        {
                            bg.Graphics.DrawString("Pause", foo, new SolidBrush(System.Drawing.Color.Red), ClientRectangle.Width / 2, ClientRectangle.Height / 2);
                        }

                        bg.Render();
                    }
                    else
                    {
                        info.GameOver(bg,ClientSize);
                        bg.Render();

                        input.ShipDead = true;
                        input.Pause = true;

                        if (inSound)
                        {
                            inSound = false;
                            ingameSound.Stop();
                            endSound.PlayLooping();
                        }

                        if (input.Restart)
                            StartTheGame();
                    }

                }
            }
        }

        private void ProtectShip()
        {
            if (!sw4Protect.IsRunning)
            {
                sw4Protect.Start();
                shipModel.Protect = true;
            }          
        }


        //new method for ship dead
        private void TakeABreak()
        {
            if (!sw.IsRunning)
            {
                sw.Start();
                info.Life--;
            }
            else
            {
                if (sw.ElapsedMilliseconds > 5000)
                {
                    sw.Stop();
                    sw.Reset();
                    shipModel.IsMarkedForDeath = false;
                    input.ShipDead = false;
                }

            }

        }

        private void checkInput()
        {
            if (input.Left)
                rot -= 6;
            if (input.Right)
                rot += 6;

            if (input.Up)
            {
                //formula to go the direction its head pointing to
                MovePoint = new PointF(MovePoint.X + (float)Math.Sin(rot * Math.PI / 180) * 5, MovePoint.Y - (float)Math.Cos(rot * Math.PI / 180) * 5);
                Console.WriteLine(MovePoint);
                //Direction = new PointF((float)Math.Sin(rot * Math.PI / 180) * 2, (float)Math.Cos(rot * Math.PI / 180) * 2);
                //Console.WriteLine(Direction);
                //MovePoint = new PointF(MovePoint.X + (float)(Math.Sin(rot)), MovePoint.Y - (float)(Math.Cos(rot)) );
                Move = true;
            }

            if (input.Down)
            {
                MovePoint = new PointF(MovePoint.X - (float)Math.Sin(rot * Math.PI / 180) * 2, MovePoint.Y + (float)Math.Cos(rot * Math.PI / 180) * 2);
                Console.WriteLine(MovePoint);
                Move = true;
            }
        }

        //check if bullet out of bound
        private bool checkBullet(Bullet b)
        {
            bool result = true;

            if (b.Translation.X < 0 || b.Translation.Y < 0)
                result = false;
            else if (b.Translation.X > ClientRectangle.Width || b.Translation.Y > ClientRectangle.Height)
                result = false;

            return result;
        }

        //create rock
        private void Init(int lvl)
        {
            for (int i = 0; i < lvl + 1; i++)
                shapeLIST.Add(new Rock(new PointF(rand.Next(ClientRectangle.Width), rand.Next(ClientRectangle.Height)), 1));
            //if (shapeLIST.Count > 0 && shapeLIST.Count < 11)
            //shapeLIST.Add(new Rock(new PointF(rand.Next(ClientRectangle.Width), rand.Next(ClientRectangle.Height))));
        }

        private double DistanceBetweenPoints(PointF A, PointF B)
        {
            double result = Math.Sqrt(Math.Pow((A.X - B.X), 2) + Math.Pow((A.Y - B.Y), 2));

            return result;
        }

        //check intersect of rock and bullet
        private void Intersection(BufferedGraphics bg, object o)
        {
            //if passing data is bullet
            if (o is Bullet)
            {
                Bullet b = (Bullet)o;
                for (int i = 0; i < shapeLIST.Count; i++)
                {
                    if (DistanceBetweenPoints(shapeLIST[i].Pos, b.Translation) < Shape.maxPolySize + 20
                                        && !shapeLIST[i].Hit)
                    {
                        Region region1 = new Region(shapeLIST[i].GetPath());
                        Region region2 = new Region(b.GetPath());

                        region1.Intersect(region2);

                        if (!region1.IsEmpty(bg.Graphics))
                        {
                            shapeLIST[i].Hit = true;


                            //break rock into smaller pieces
                            if (shapeLIST[i].small < 3)
                                for (int r = 0; r < shapeLIST[i].small + 1; r++)
                                    shapeLIST.Add(new Rock(shapeLIST[i].Pos, shapeLIST[i].small + 1));

                            shapeLIST[i].IsMarkedForDeath = true;
                            b.IsMarkedForDeath = true;
                            break;
                        }

                    }//////end if

                }////end for
            }//end of bullet Object
            else if (o is Shape)
            {
                Shape r = (Shape)o;

                Region region1 = new Region(r.GetPath());
                Region region2 = new Region(shipModel.GetPath());

                region1.Intersect(region2);

                if (!region1.IsEmpty(bg.Graphics))
                {
                    shipModel.IsMarkedForDeath = true;
                    
                }

            }

        }


    }
}
