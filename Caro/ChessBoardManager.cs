using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Caro
{
    public class ChessBoardManager
    {
        #region Properties
        private Panel chessboard;
        public Panel Chessboard
        {
            get { return chessboard; }
            set { chessboard = value; }
        }
        private TextBox playername;
        public TextBox Playername
        {
            get => playername;
            set => playername = value;
        }
        private List<Player> player;
        public List<Player> Player 
        { 
            get => player; 
            set => player = value; 
        }
        private int currentplayer;
        public int Currentplayer
        {
            get => currentplayer;
            set => currentplayer = value;
        }

        private PictureBox mark;
        public PictureBox Mark 
        { 
            get => mark; 
            set => mark = value; 
        }
        private List<List<Button>> matrix;
        public List<List<Button>> Matrix 
        { 
            get => matrix; 
            set => matrix = value; 
        }

        private event EventHandler playermark;
        public event EventHandler Playermark
        {
            add
            {
                playermark += value;
            }
            remove
            {
                playermark -= value;
            }
        }
        private event EventHandler endgame;
        public event EventHandler Endgame
        {
            add
            {
                endgame += value;
            }
            remove
            {
                endgame -= value;
            }
        }
        #endregion
        #region Initialize
        public ChessBoardManager(Panel chessboard, TextBox playername, PictureBox mark)
        {
            this.Chessboard = chessboard;
            this.Playername = playername;
            this.Mark = mark;
            this.Player = new List<Player>()
            {
                new Player ("Player1", Image.FromFile(Application.StartupPath+"\\Resources\\image1.png")),
                new Player ("Player2", Image.FromFile(Application.StartupPath+"\\Resources\\image2.png"))
            };
            Currentplayer = 0;
        }    
        #endregion
        #region Methods
        public void DrawChessBoard()
        {
            chessboard.Enabled = true;
            chessboard.Controls.Clear();
            matrix = new List<List<Button>>();
            Button oldbutton = new Button()
            {
                Width = 0,
                Location = new Point(0, 0)
            };
            for (int i = 0; i < Constant.chessboard_height; i++)
            {
                Matrix.Add(new List<Button>());
                for (int j = 0; j < Constant.chessboard_width; j++)
                {
                    Button button = new Button()
                    {
                        Width = Constant.chess_width,
                        Height = Constant.chess_height,
                        Location = new Point(oldbutton.Location.X + oldbutton.Width, oldbutton.Location.Y),
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Tag = i.ToString()
                    };
                    button.Click += buttonclick;
                    Chessboard.Controls.Add(button);
                    Matrix[i].Add(button);
                    oldbutton = button;
                }
                oldbutton.Location = new Point(0, oldbutton.Location.Y + Constant.chess_height);
                oldbutton.Width = 0;
                oldbutton.Height = 0;
            }
        }
        void buttonclick (object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button.BackgroundImage != null)
                return;
            button.BackgroundImage = Player[Currentplayer].Mark;
            if (Currentplayer == 0)
            {
                Currentplayer = 1;
            }
            else Currentplayer = 0;
            Playername.Text = Player[Currentplayer].Name;
            Mark.Image = Player[Currentplayer].Mark;
            if (playermark != null)
            {
                playermark(this, new EventArgs());
            }
            if (IsEndGame(button))
            {
                EndGame();
                chessboard.Enabled = false;
                if (Currentplayer == 0)
                {
                    Currentplayer = 1;
                }
                else Currentplayer = 0;
                MessageBox.Show(Player[Currentplayer].Name + " win");
            }
        }
        #endregion
        public void EndGame()
        {
            if  (endgame!=null)
            {
                endgame(this, new EventArgs());
            }    
        }
        private bool IsEndGame(Button button)
        {
            return Horizontal(button) || Vertical(button) || MainDiagonal(button) || SecondaryDiagonal(button);
        }
        private Point Getchessbutton(Button button)
        {
            int vertical = Convert.ToInt32(button.Tag);
            int horizontal = Matrix[vertical].IndexOf(button);
            Point point = new Point(horizontal,vertical);
            return point;
        }
        private bool Horizontal(Button button)
        {
            Point point = Getchessbutton(button);
            int countleft=0;
            for (int i=point.X;i>=0;i--)
            {
                if (Matrix[point.Y][i].BackgroundImage == button.BackgroundImage)
                {
                    countleft++;
                }
                else break;
            }    
            int countright = 0;
            for (int i = point.X+1; i < Constant.chessboard_width; i++)
            {
                if (Matrix[point.Y][i].BackgroundImage == button.BackgroundImage)
                {
                    countright++;
                }
                else break;
            }
            return countleft+countright==5;
        }
        private bool Vertical(Button button)
        {
            Point point = Getchessbutton(button);
            int counttop = 0;
            for (int i = point.Y; i >= 0; i--)
            {
                if (Matrix[i][point.X].BackgroundImage == button.BackgroundImage)
                {
                    counttop++;
                }
                else break;
            }
            int countbottom = 0;
            for (int i = point.Y + 1; i < Constant.chessboard_height; i++)
            {
                if (Matrix[i][point.X].BackgroundImage == button.BackgroundImage)
                {
                    countbottom++;
                }
                else break;
            }
            return counttop + countbottom == 5;
        }
        private bool MainDiagonal(Button button)
        {
            Point point = Getchessbutton(button);
            int counttop = 0;
            for (int i = 0; i <= point.X; i++)
            {
                if (point.X - i < 0 || point.Y - i < 0)
                    break;
                if (Matrix[point.Y-i][point.X-i].BackgroundImage == button.BackgroundImage)
                {
                    counttop++;
                }
                else break;
            }
            int countbottom = 0;
            for (int i = 1; i <= Constant.chessboard_width - point.X; i++)
            {
                if (point.Y + i >= Constant.chessboard_height || point.X + i >= Constant.chessboard_width)
                    break;
                if (Matrix[point.Y+i][point.X+i].BackgroundImage == button.BackgroundImage)
                {
                    countbottom++;
                }
                else break;
            }
            return counttop + countbottom == 5;
        }
        private bool SecondaryDiagonal(Button button)
        {
            Point point = Getchessbutton(button);
            int counttop = 0;
            for (int i = 0; i <= point.X; i++)
            {
                if (point.X + i >= Constant.chessboard_width || point.Y - i < 0)
                    break;
                if (Matrix[point.Y - i][point.X + i].BackgroundImage == button.BackgroundImage)
                {
                    counttop++;
                }
                else break;
            }
            int countbottom = 0;
            for (int i = 1; i <= Constant.chessboard_width - point.X; i++)
            {
                if (point.X - i < 0 || point.Y + i >= Constant.chessboard_height)
                    break;
                if (Matrix[point.Y + i][point.X - i].BackgroundImage == button.BackgroundImage)
                {
                    countbottom++;
                }
                else break;
            }
            return counttop + countbottom == 5;
        }
    }
}
