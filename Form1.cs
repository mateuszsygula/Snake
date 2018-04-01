using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace Snake
{
    public partial class Form1 : Form
    {
        List<SnakePart> snake = new List<SnakePart>();
        const int tile_width = 16;
        const int tile_height = 16;
        bool gameover = false;
        bool gameT = false;
        int score = 0;
        int direction = 0; // Down = 0, Left = 1, Right = 2, Up = 3
        SnakePart food_piece = new SnakePart();

        public Form1()
        {
            InitializeComponent();
            
            gameTimer.Interval = 1000 / 4;
            gameTimer.Tick += new EventHandler(Update);
            gameTimer.Start();
            gameT = true;
          //  conectgamepad();
            string test = "testowoo";
            test = Snake.Properties.Resources.checktest;

            //  StartGame();
        }

        private void StartGame()
        {
            gameTimer.Start();
            gameT = true;
            gameover = false;
            score = 0;
            direction = 0;
            snake.Clear();
            SnakePart head = new SnakePart();
            head.X = 10;
            head.Y = 5;
            snake.Add(head);
            GenerateFood();
        }
        private void PauseGame()
        {
            if (gameT==true)
            {
                gameTimer.Stop();
                gameT = false;
            }
            else
            {
                gameTimer.Start();
            }
            

        }

        private void GenerateFood()
        {
            int max_tile_w = pbCanvas.Size.Width / tile_width;
            int max_tile_h = pbCanvas.Size.Height / tile_height;
            Random random = new Random();
            food_piece = new SnakePart();
            food_piece.X = random.Next(0, max_tile_w);
            food_piece.Y = random.Next(0, max_tile_h);
        }

        private void Update(object sender, EventArgs e)
        {
            if (gameover)
            {
                if (Input.Pressed(Keys.Enter))
                    StartGame();
            }
            else
            {
                if (Input.Pressed(Keys.Right))
                {
                    if (snake.Count < 2 || snake[0].X == snake[1].X)
                        direction = 2;
                }
                else if (Input.Pressed(Keys.Left))
                {
                    if (snake.Count < 2 || snake[0].X == snake[1].X)
                        direction = 1;
                }
                else if (Input.Pressed(Keys.Up))
                {
                    if (snake.Count < 2 || snake[0].Y == snake[1].Y)
                        direction = 3;
                }
                else if (Input.Pressed(Keys.Down))
                {
                    if (snake.Count < 2 || snake[0].Y == snake[1].Y)
                        direction = 0;
                }
                UpdateSnake();
            }
            pbCanvas.Invalidate();
        }

        private void UpdateSnake()
        {
            for (int i = snake.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    switch (direction)
                    {
                        case 0: // Down
                            snake[i].Y++;
                            break;
                        case 1: // Left
                            snake[i].X--;
                            break;
                        case 2: // Right
                            snake[i].X++;
                            break;
                        case 3: // Up
                            snake[i].Y--;
                            break;
                    }
                    int max_tile_w = pbCanvas.Width / tile_width;
                    int max_tile_h = pbCanvas.Height / tile_height;
                    if (snake[i].X < 0 || snake[i].X >= max_tile_w || snake[i].Y < 0 || snake[i].Y >= max_tile_h)
                        gameover = true;
                    for (int j = 1; j < snake.Count; j++)
                        if (snake[i].X == snake[j].X && snake[i].Y == snake[j].Y)
                            gameover = true;
                    if (snake[i].X == food_piece.X && snake[i].Y == food_piece.Y)
                    {
                        SnakePart part = new SnakePart();
                        part.X = snake[snake.Count - 1].X;
                        part.Y = snake[snake.Count - 1].Y;
                        snake.Add(part);
                        GenerateFood();
                        score++;
                    }
                }
                else
                {
                    snake[i].X = snake[i - 1].X;
                    snake[i].Y = snake[i - 1].Y;
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, false);
        }

        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            if (gameover)
            {
                Font font = this.Font;
                string gameover_msg = "Gameover";
                string score_msg = "Punkty: " + score.ToString();
                string newgame_msg = "Press Enter to Start Over";
                int center_width = pbCanvas.Width / 2;
                SizeF msg_size = canvas.MeasureString(gameover_msg, font);
                PointF msg_point = new PointF(center_width - msg_size.Width / 2, 16);
                canvas.DrawString(gameover_msg, font, Brushes.White, msg_point);
                msg_size = canvas.MeasureString(score_msg, font);
                msg_point = new PointF(center_width - msg_size.Width / 2, 32);
                canvas.DrawString(score_msg, font, Brushes.White, msg_point);
                msg_size = canvas.MeasureString(newgame_msg, font);
                msg_point = new PointF(center_width - msg_size.Width / 2, 48);
                canvas.DrawString(newgame_msg, font, Brushes.White, msg_point);
            }
            else
            {
                for (int i = 0; i < snake.Count; i++)
                {
                    Brush snake_color = i == 0 ? Brushes.Red : Brushes.Black;
                    canvas.FillRectangle(snake_color, new Rectangle(snake[i].X * tile_width, snake[i].Y * tile_height, tile_width, tile_height));
                }
                canvas.FillRectangle(Brushes.Orange, new Rectangle(food_piece.X * tile_width, food_piece.Y * tile_height, tile_width, tile_height));
                canvas.DrawString("Score: " + score.ToString(), this.Font, Brushes.White, new PointF(4, 4));
            }
        }


        private static string RxString = "";
        private void connectGamePadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            conectgamepad();
        }
        private void conectgamepad()
        {
            try
            {
                if (!serialPortRead.IsOpen)
                {// set status
                    ReadTextBox.Text = "Uruchomiono Port: " + serialPortRead.PortName + " !\n Polacz sie za pomoca aplikacji Arduino Bluetooth\n";
                    //      ReadTextBox.AppendText(serialPortWrite.PortName + "Ready!");
                    //      serialPortWrite.Open();
                    serialPortRead.Open();
                    //prevent reinitalion
                    initButton.Enabled = false;
                }
                else
                {
                    ReadTextBox.Text = "Port nie jest otwarty";
                }

            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            serialPortRead.Close();
        }

        private void serialPortRead_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                RxString = serialPortRead.ReadExisting();
                this.Invoke(new EventHandler(DisplayText));
            }
            catch (System.TimeoutException) { }
        }
        private void DisplayText(object s, EventArgs e)
        {
            switch (RxString)
            {
                case "left":
                    ReadTextBox.AppendText("W lewo \n");
                    if (snake.Count < 2 || snake[0].X == snake[1].X)
                        direction = 1;
                    break;
                case "up":
                    ReadTextBox.AppendText("W gore \n");
                    if (snake.Count < 2 || snake[0].Y == snake[1].Y)
                        direction = 3;
                    break;
                case "right":
                    ReadTextBox.AppendText("W prawo \n");
                    if (snake.Count < 2 || snake[0].X == snake[1].X)
                        direction = 2;
                    break;
                case "down":
                    ReadTextBox.AppendText("W dol \n");
                    if (snake.Count < 2 || snake[0].Y == snake[1].Y)
                        direction = 0;
                    break;
                case "start":
                    ReadTextBox.AppendText("start \n");
                    StartGame();
                    break;
                case "select":
                    ReadTextBox.AppendText("select \n");
                    PauseGame();
                    break;
                case "1":
                    ReadTextBox.AppendText("Kwadrat \n");
                    break;
                case "2":
                    ReadTextBox.AppendText("Trojkat \n");
                    break;
                case "3":
                    ReadTextBox.AppendText("Krzyzyk \n");
                    open();
                    break;
                case "4":
                    ReadTextBox.AppendText("Kolo \n");
                    close();
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }
            //  ReadTextBox.AppendText(RxString);
        }
        About newAboutWindow = new About();
        private void open()
        {

            
            if (newAboutWindow.ShowDialog() == DialogResult.Cancel)
            {

                newAboutWindow.Close();
            }
            
        }
        private void close()
        {
            newAboutWindow.Close();
        }
       
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked==true)
            {
                
                ReadTextBox.AppendText(Snake.Properties.Resources.checktest);
                ReadTextBox.AppendText("Check Działa \n");
            }
            else
            {
                ReadTextBox.AppendText("A może jednak nie \n");
                
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new About()).ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            serialPortRead.Close();
            Close();
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PauseGame();
        }
    }
}
