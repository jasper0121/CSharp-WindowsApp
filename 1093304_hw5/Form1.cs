using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1093304_hw5
{
    public partial class Form1 : Form
    {
        //載入9張圖片
        Image[] img = new Image[9] { Properties.Resources._8_0, Properties.Resources._8_1, Properties.Resources._8_2, Properties.Resources._8_3,
            Properties.Resources._8_4, Properties.Resources._8_5, Properties.Resources._8_6, Properties.Resources._8_7, Properties.Resources._8_8 };
        Image[,] sixteenSquare = new Image[4, 4]; //建立Image的4 * 4二維陣列
        bool[,] squareState = new bool[4, 4]; //每一格的狀態
        Point[] pos = new Point[2]; //點擊的2圖片位置
        int second, fruit, time; //秒數(0 ~ ∞)、以翻開的水果種類數(0 ~ 8)、當回合已翻開的次數(0 ~ 2)
        bool playing; //遊戲狀態
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            playing = true; //初始遊玩狀態設為true
            label1.Text = (second = fruit = time = 0).ToString() + " Seconds"; //將second、fruit和time設為0，並顯示初始秒數
            timer1.Start(); //遊戲時間開始
            int pos = 0; //紀錄以下建立16宮格時的座標位置(0 ~ 16)
            Random rd = new Random(); //建立隨機亂數
            for (; pos < 16; squareState[pos / 4, pos++ % 4] = true) ; //初始將所有狀態設為true
            for (int num = 0, i = 1; i < 9; i++, num = 0) //將8種圖片隨機放入16格中
            {
                for (pos = rd.Next(16); num < 2; pos = rd.Next(16)) //每次產生0 ~ 15的亂數，轉成相應格子位置後檢查該格是否能放。每種圖放2次
                {
                    if (squareState[pos / 4, pos % 4]) //若為true則代表還沒將圖片放入
                    {
                        sixteenSquare[pos / 4, pos % 4] = img[i]; //放入圖片
                        squareState[pos / 4, pos % 4] = false; //放入圖片後設為false
                        num++;
                    }
                }
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    //畫圖，若squareState[y, x] == true，代表當下該圖是已翻開的狀態，故繪出該格的水果；反之則繪出水果籃(未翻開)
                    e.Graphics.DrawImage(squareState[y, x] ? sixteenSquare[y, x] : img[0], x * 70, 25 + y * 70, 70, 70);
                    e.Graphics.DrawRectangle(new Pen(Color.Black, 3), x * 70, 25 + y * 70, 70, 70); //畫黑色邊框，長寬各70，高度預留25以免被上方工具列擋住
                }
            }

            if (time == 2) //若當回合已翻開2張圖片
            {
                time = 0; //先將次數重置
                if (sixteenSquare[pos[0].Y, pos[0].X] == sixteenSquare[pos[1].Y, pos[1].X]) //檢查若翻開的2張圖片相同
                {
                    if (++fruit == 8) //已翻開的水果種類數 + 1，若水果種類數 == 8，代表每一格都已被翻開，遊戲結束
                    {
                        playing = false;
                        timer1.Stop();
                    }
                }
                else //反之則將翻開的2個圖片蓋回去，且過程中將playing暫時設為false，以timer2計時1秒，使得在1秒內除restart外無法進行任何操作
                {
                    playing = squareState[pos[0].Y, pos[0].X] = squareState[pos[1].Y, pos[1].X] = false;
                    timer2.Start(); //計時開始
                }
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            //若正處於遊玩狀態&&滑鼠點擊時的位置在範圍內&&所點的格子圖片還未被翻開
            if (playing && e.X < 280 && 25 <= e.Y && e.Y < 305 && squareState[(e.Y - 25) / 70, e.X / 70] == false)
            {
                pos[time++] = new Point(e.X / 70, (e.Y - 25) / 70); //將當前所點的格子位置存起來
                squareState[(e.Y - 25) / 70, e.X / 70] = true; //翻開格子內的圖片
                Invalidate(); //介面重畫
            }
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e) //重新開始
        {
            timer1.Stop(); //遊戲時間結束
            Form1_Load(sender, e); //所有資料重置
            Invalidate(); //介面重畫
        }

        private void timer1_Tick(object sender, EventArgs e) //每隔1秒會執行此函式
        {
            label1.Text = (++second).ToString() + " Seconds"; //遊戲時間計時
        }

        private void timer2_Tick(object sender, EventArgs e) //每隔1秒會執行此函式
        {
            timer2.Stop(); //1秒後計時結束
            playing = true; //恢復遊玩狀態
            Invalidate(); //介面重畫
        }
    }
}