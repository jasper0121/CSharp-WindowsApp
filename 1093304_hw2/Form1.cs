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

namespace _1093304_hw2
{
    public partial class Form1 : Form
    {
        int[,] nineSquare = new int[3, 3]; //設定9宮格存雙方O和X的位置
        int myRound = 0; //我方的回合數，myRound == 5時棋盤上9格皆已佔滿，此時我方若未贏則平局
        bool I_win = false, You_win = false; //贏的那方設為true
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e) //畫棋盤和當下O和X的位置
        {
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Black, 3), x * 60, (y + 1) * 60, 60, 60);
                    switch (nineSquare[x, y]) //看格內的數字是多少
                    {
                        case 1: //1 == 我方
                            e.Graphics.DrawEllipse(new Pen(Color.Blue, 3), x * 60 + 5, (y + 1) * 60 + 5, 50, 50); //則在棋盤上的相應位置上畫O
                            if (I_win) //若當下我方贏(視窗縮小再展開時重畫用)
                            {
                                Check_Win(1, e.Graphics); //找出贏的位置並畫紅色連線
                            }

                            break;
                        case -1: //-1 == 對方
                            Draw_X(x, y, e.Graphics); //則在棋盤上的相應位置上畫X
                            if (You_win) //若當下對方贏(視窗縮小再展開時重畫用)
                            {
                                Check_Win(-1, e.Graphics); //找出贏的位置並畫紅色連線
                            }

                            break;
                    }
                }
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            //若點擊在範圍內 && 該位置沒有O或X && 雙方勝利狀態相同(即還未分出勝負，不可能雙方同時皆贏)
            if (e.X < 180 && 60 <= e.Y && e.Y < 240 && nineSquare[e.X / 60, e.Y / 60 - 1] == 0 && I_win == You_win)
            {
                Graphics G = CreateGraphics(); //繪圖物件
                G.DrawEllipse(new Pen(Color.Blue, 3), e.X / 60 * 60 + 5, e.Y / 60 * 60 + 5, 50, 50); //畫圓，直徑50，起始座標為棋盤方格左上角 + 5
                if (Check_Win(nineSquare[e.X / 60, e.Y / 60 - 1] = 1, G)) //紀錄畫O位置，若我方贏
                {
                    I_win = true; //我方勝利
                    label1.Text = "You win!";
                    return; //遊戲結束
                }

                if ((++myRound) == 5) //每次畫完O後，我方回合數 + 1，若到了第5回合，代表9格棋盤已畫滿且未分勝負，故平局
                {
                    label1.Text = "Draw!";
                    return;
                }

                Opponent(G); //對手的回合
            }
        }

        private bool Check_Win(int val, Graphics G) //檢查是否獲勝，若有則畫上紅線
        {
            Point[] p = new Point[2]; //存線段的2點座標
            for (int n = 0; n < 3; n++) //檢查橫列和直列是否已連線
            {
                if (nineSquare[n, 0] == val && nineSquare[n, 0] == nineSquare[n, 1] && nineSquare[n, 1] == nineSquare[n, 2]) //直列連線
                {
                    p[0].X = p[1].X = n * 60 + 30;
                    p[0].Y = 75;
                    p[1].Y = 225;
                    G.DrawPolygon(new Pen(Color.Red, 3), p);
                    return true;
                }

                if (nineSquare[0, n] == val && nineSquare[0, n] == nineSquare[1, n] && nineSquare[1, n] == nineSquare[2, n]) //橫列連線
                {
                    p[0].X = 15;
                    p[0].Y = p[1].Y = (n + 1) * 60 + 30;
                    p[1].X = 165;
                    G.DrawPolygon(new Pen(Color.Red, 3), p);
                    return true;
                }
            }

            if (nineSquare[0, 0] == val && nineSquare[0, 0] == nineSquare[1, 1] && nineSquare[1, 1] == nineSquare[2, 2]) //檢查對角線是否連線
            {
                p[0].X = 15;
                p[0].Y = 75;
                p[1].X = 165;
                p[1].Y = 225;
                G.DrawPolygon(new Pen(Color.Red, 3), p);
                return true;
            }

            if (nineSquare[0, 2] == val && nineSquare[0, 2] == nineSquare[1, 1] && nineSquare[1, 1] == nineSquare[2, 0]) //檢查對角線是否連線
            {
                p[0].X = 165;
                p[0].Y = 75;
                p[1].X = 15;
                p[1].Y = 225;
                G.DrawPolygon(new Pen(Color.Red, 3), p);
                return true;
            }

            return false; //若以上都沒有連線則回傳false
        }

        private void Opponent(Graphics G) //對手的回合
        {
            int pos = 0; //9宮格位置，從左上0號 ~ 右下8號
            Random rd = new Random(); //設定隨機亂數       
            for (pos = rd.Next(9); nineSquare[pos / 3, pos % 3] != 0; pos = rd.Next(9)) ; //找0 ~ 8號的亂數，若該位置已有O或X則重新找亂數到找到為止
            Draw_X(pos / 3, pos % 3, G); //畫X(pos / 3 == x，pos % 3 == y)
            if (Check_Win(nineSquare[pos / 3, pos % 3] = -1, G)) //紀錄畫X位置，若對手贏
            {
                You_win = true; //對方勝利
                label1.Text = "You lose!";
            }
        }

        private void Draw_X(int x, int y, Graphics G) //畫X
        {
            Point[] p1 = new Point[2];
            Point[] p2 = new Point[2];
            p1[1].X = p1[1].Y = 50;
            for (int n = 0; n < 2; n++) //制定X的4個頂點座標
            {
                p2[n].X = p1[n].X += x * 60 + 5;
                p2[n].Y = p1[(n + 1) % 2].Y += (y + 1) * 60 + 5;
            }

            G.DrawPolygon(new Pen(Color.Blue, 3), p1);
            G.DrawPolygon(new Pen(Color.Blue, 3), p2);
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e) //所有參數重置
        {
            Invalidate(); //重置使當前介面失效，並重新跑Form1_Paint
            myRound = 0;
            I_win = You_win = false;
            label1.Text = "";
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    nineSquare[x, y] = 0;
                }
            }
        }
    }
}