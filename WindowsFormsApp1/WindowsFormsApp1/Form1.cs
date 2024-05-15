using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        bool GoUp, GoDown, GoLeft, GoRight, GameOver;
        string ViewPlayer = "up";
        int PlayerHealth = 100;
        int SpeedGame = 7; // передвижение игрока
        Double Patron = 10;
        int BotSpeed = 1;
        int Score;
        int shift;
        bool pause = true;
        int kolvo_Bot = 5; // количество ботов
        Random rand = new Random();
        int width_form = 0;

        List<PictureBox> BotList = new List<PictureBox>(); // создание ботов, объектов
        public Form1()
        {
            InitializeComponent();
            RestartGame();
           this.TopMost = true;
            this.Focus();
           this.FormBorderStyle = FormBorderStyle.None; // кнопки вверху формы

           this.WindowState = FormWindowState.Maximized; // во весь экран
            pictureBox1.Focus();

            width_form = this.Width;
           
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (GameOver == true) 
            {
                return;
            }

            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A)
            {
                GoLeft = true;
                ViewPlayer = "Left";
                pictureBox1.Image = Properties.Resources.left;
            }

            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D)
            {
                GoRight = true;
                ViewPlayer = "Right";
                pictureBox1.Image = Properties.Resources.right;
            }

            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W)
            {
                GoUp = true;
                ViewPlayer = "Up";
                pictureBox1.Image = Properties.Resources.up;
            }

            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S)
            {
                GoDown = true;
                ViewPlayer = "Down";
                pictureBox1.Image = Properties.Resources.down;
            } 
            
            if (e.KeyCode == Keys.ShiftKey)
            {
                shift = 1;
            }

            if (shift == 1 && e.KeyCode == Keys.Space)
            {
                Shoot(ViewPlayer);
            }

            

        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A)
            {
                GoLeft = false;
            }

            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D)
            {
                GoRight = false;
            }

            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W)
            {
                GoUp = false;
            }
            
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S)
            {
                GoDown = false;
            }

            if (e.KeyCode == Keys.Escape)
            {

                 if (pause == true)
                 {
                     timer1.Stop();
                     pause = false;

                     Form2 settingsForm = new Form2();
                     // Show the settings form
                     settingsForm.Show();
                     settingsForm.TopMost = true;

                 }
                 else
                 {
                     timer1.Start();
                     pause = true;
                 }

  
                
            }

            if ((shift == 0 && e.KeyCode == Keys.Space) && Patron > 0 && GameOver == false)
            {
                Patron--;
                Shoot(ViewPlayer);

                if (Patron < 1)
                {
                    DropPatron();
                    
                }
                if (Score%15==0)
                {
                    DropHealth();
                }
            }

            if (e.KeyCode == Keys.Enter && GameOver == true) // перезапуск по клавише Enter
            {
                RestartGame();
            }

            if (e.KeyCode == Keys.ShiftKey)
            {
                shift = 0;
            }
            
        }

        private void MainTamerEvent(object sender, EventArgs e)
        {
            if (PlayerHealth > 0)
            {
                if (PlayerHealth>100) 
                { 
                    progressBar1.Value = 100; 
                }
                else
                {
                    progressBar1.Value = PlayerHealth;
                }
                progressBar2.Value = Score;

            }
            else
            {
                //Pass the filepath and filename to the StreamWriter Constructor
                    StreamWriter sw = new StreamWriter("file_records.txt");
                    //Write a line of text
                    sw.WriteLine(Score);
                    //Write a second line of text
                    //Close the file
                    sw.Close();

                GameOver = true;
                pictureBox1.Image = Properties.Resources.dead;
                timer1.Stop();

                
                    
                
           

            }

            label1.Text = "Патроны: " + Patron;
            label2.Text = "Убийства: " + Score;

            if (GoLeft == true && pictureBox1.Left > 0)
            {
                pictureBox1.Left -= SpeedGame;
            }
            if (GoRight == true && pictureBox1.Left + pictureBox1.Width < this.ClientSize.Width)
            {
                pictureBox1.Left += SpeedGame;
            }
            if (GoUp == true && pictureBox1.Top > 40) // ограничение 40, чтобы не залезало на labelы
            {
                pictureBox1.Top -= SpeedGame;
            }
            if (GoDown == true && pictureBox1.Top + pictureBox1.Height < this.ClientSize.Height)
            {
                pictureBox1.Top += SpeedGame;
            }


            foreach (Control x in this.Controls) // проверка полуячения дополнительных патронов
            {
                if (x is PictureBox && (string)x.Tag == "Patron")
                {
                    if (pictureBox1.Bounds.IntersectsWith(x.Bounds)) 
                    {
                        this.Controls.Remove(x);
                        ((PictureBox)x).Dispose();
                        Patron += 10; // бонус патрон

                    }

                }

                if (PlayerHealth<100) 
                {
                    if (x is PictureBox && (string)x.Tag == "Health") // проверка получения дополнительного здоровья
                    {
                        if (pictureBox1.Bounds.IntersectsWith(x.Bounds))
                        {
                            this.Controls.Remove(x);
                            ((PictureBox)x).Dispose();
                        
                            PlayerHealth += 5; // бонус здоровье
                        
                        }

                    }
                }

                if (x is PictureBox && (string)x.Tag == "Bot") { // управление перемещением ботов

                    if (pictureBox1.Bounds.IntersectsWith(x.Bounds)) 
                    {
                        PlayerHealth -= 1; // уменьшение здоровья игрока при пересечении с ботом
                    }

                    
                    
                    if (x.Left > pictureBox1.Left) //левее игрока
                    {
                        x.Left -= BotSpeed;
                        ((PictureBox)x).Image = Properties.Resources.zleft;
                    }

                    if (x.Left < pictureBox1.Left) // правее игрока
                    {
                        x.Left += BotSpeed;
                        ((PictureBox)x).Image = Properties.Resources.zright;
                    }

                    if (x.Top > pictureBox1.Top) // выше игрока
                    {
                        x.Top -= BotSpeed;
                        ((PictureBox)x).Image = Properties.Resources.zup;
                    }

                    if (x.Top < pictureBox1.Top) // ниже игрока
                    {
                        x.Top += BotSpeed;
                        ((PictureBox)x).Image = Properties.Resources.zdown;
                    }

                }


                foreach (Control q in this.Controls)
                {
                    if (q is PictureBox && (string)q.Tag == "bullet" && x is PictureBox && (string)x.Tag == "Bot")
                    {
                        if (x.Bounds.IntersectsWith(q.Bounds)) // проверка попадания пули в бота
                        {
                            Score++;

                            if (PlayerHealth < 100)
                            {
                                PlayerHealth += 1;
                            }
                            Patron += 0.5;

                            this.Controls.Remove(q); // удаление старых ботов
                            ((PictureBox)q).Dispose();
                            this.Controls.Remove(x);
                            ((PictureBox)x).Dispose();
                            BotList.Remove(((PictureBox)x));
                            MakeBot(); // создание новых

                        }
                    }
                }
            }


        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Shoot(string direction)
        {
            Bullet shoot = new Bullet();
            shoot.direction = direction;
            shoot.bulletLeft= pictureBox1.Left + (pictureBox1.Width / 2);
            shoot.bulletTop = pictureBox1.Top + (pictureBox1.Height / 2);
            
            shoot.MakeBullet(this);
        }

        private void MakeBot() // создание ботов
        {
            PictureBox bot = new PictureBox();
            bot.Tag = "Bot";
            bot.Image = Properties.Resources.zdown;
            bot.Left = rand.Next(0, this.Height-100);
            bot.Top = rand.Next(0, this.Width-100);
            bot.SizeMode = PictureBoxSizeMode.AutoSize;
            BotList.Add(bot);
            this.Controls.Add(bot);
            pictureBox1.BringToFront();

        }

        private void DropPatron() // создание бонуса патронов
        {
            PictureBox Patron = new PictureBox();
            Patron.Image = Properties.Resources.ammo_Image;
            Patron.SizeMode = PictureBoxSizeMode.AutoSize; 
            Patron.Left = rand.Next(10, this.ClientSize.Width - Patron.Width);
            Patron.Top = rand.Next(60, this.ClientSize.Height - Patron.Height); // ограничение надписей
            Patron.Tag = "Patron";
            this.Controls.Add(Patron);
            Patron.BringToFront() ; 

            pictureBox1.BringToFront();

        }
        private void DropHealth()
        {
            PictureBox Health = new PictureBox();
            Health.Image = Properties.Resources.serdce;
            Health.SizeMode = PictureBoxSizeMode.AutoSize;
            Health.Left = rand.Next(10, this.ClientSize.Width - Health.Width);
            Health.Top = rand.Next(60, this.ClientSize.Height - Health.Height); // ограничение надписей
            Health.Tag = "Health";
            this.Controls.Add(Health);
            Health.BringToFront();

            pictureBox1.BringToFront();

        }

        private void RestartGame() // перезапуск игры
        {
            pictureBox1.Image = Properties.Resources.up;

            foreach (PictureBox i in BotList)
            {
                this.Controls.Remove(i);
            }

            BotList.Clear();
            
            


            for (int i = 0; i < kolvo_Bot; i++) // спавн ботов
           {
               MakeBot();
           }
            
            GoUp = false;
            GoDown = false;
            GoLeft = false;
            GoRight = false;
            GameOver = false;

            PlayerHealth = 100;
            Score = 0;
            Patron = 10;

            timer1.Start();

        }
    }
}
