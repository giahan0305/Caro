using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Caro
{
    public partial class Form1 : Form
    {
        ChessBoardManager chessboard;
        public Form1()
        {
           InitializeComponent();
           chessboard = new ChessBoardManager(panelChessBoard, textBoxPlayer, pictureBox);
           chessboard.Endgame += chessboard_Endgame;
           chessboard.Playermark += chessboard_Playermark;
            chessboard.DrawChessBoard();
            panelChessBoard.Enabled = false;
        }
        void Endgame()
        {
            timer.Stop();
            panelChessBoard.Enabled = false;    
            MessageBox.Show("End game");
        }
        void chessboard_Playermark(object sender, EventArgs e)
        {
            timer.Start();
            progressBarPlayer.Value = 0;
        }
        void chessboard_Endgame(object sender, EventArgs e)
        {
            Endgame();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBarPlayer.PerformStep();
            if (progressBarPlayer.Value >= progressBarPlayer.Maximum)
            {
                Endgame();
            }    
        }

        private void button_start_Click(object sender, EventArgs e)
        {
            panelChessBoard.Enabled = true;
            pictureBox.Image = Image.FromFile(Application.StartupPath + "\\Resources\\image1.png");
            textBoxPlayer.Text = ("Player1");
            progressBarPlayer.Step = Constant.step;
            progressBarPlayer.Maximum = Constant.time;
            progressBarPlayer.Value = 0;
            timer.Interval = Constant.interval;
            timer.Start();
        }
    }
}
