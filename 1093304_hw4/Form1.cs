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

namespace _1093304_hw4
{
    public partial class Form1 : Form
    {
        Point ball = new Point(10, 52); //球座標(左上角)
        Point move = new Point(1, 1); //球的X和Y軸行徑方向
        Point block = new Point(100, 260); //底下方塊座標(中心點)   
        Color color = Color.Red; //球的顏色(預設為紅)
        bool playing = true; //遊玩狀態
        int second, speed = 1; //經過時間、球的速度

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = (second = 0).ToString(); //計時秒數從0開始
            toolStripStatusLabel2.Text = "Playing!";
            timer1.Start(); //遊戲計時開始(間隔1秒)
            timer2.Start(); //球的移動開始(間隔20毫秒)        
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e) //所有變數和介面初始化
        {
            Invalidate();
            ball.X = 10;
            ball.Y = 52;
            move.X = move.Y = speed = 1;
            block.X = 100;
            playing = true;
            toolStripButton1_Click(sender, e);          
            Form1_Load(sender, e);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(Pens.Black, 0, 50, 200, block.Y - 50); //Y從50開始以避免被上方工具列蓋到，畫出黑色矩形邊框(Y軸長度可隨block.Y調整)
            e.Graphics.FillEllipse(new SolidBrush(color), ball.X, ball.Y, 10, 10); //實心球，直徑為10
            e.Graphics.FillRectangle(new SolidBrush(Color.Red), block.X - 20, block.Y, 40, 15); //實心方塊，長40寬15
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (playing) //若還在遊戲中
            {
                block.X = e.X < 20 ? 20 : (e.X > 180 ? 180 : e.X); //設定方塊可移動的左右邊界，一旦滑鼠移動，則更改方塊中心位置
                Invalidate(); //更改位置後重新畫圖
            }
        }

        private void timer1_Tick(object sender, EventArgs e) //遊戲計時(間隔1秒)
        {
            if (second % 5 == 0 && second != 0) //每隔5秒速度 + 1(不包括一開始)
            {
                speed++;
            }

            toolStripStatusLabel1.Text = (++second).ToString(); //每隔1秒秒數 + 1
        }

        private void timer2_Tick(object sender, EventArgs e) //球的時間，每隔20毫秒刷新球的位置
        {
            //speed值越高，代表在20毫秒內執行的for次數越多，使得球速越快、每次球的X和Y移動量為1或-1，刷新球的位置後介面重畫
            for (int t = 0; t++ < speed; ball.X += move.X, ball.Y += move.Y, Invalidate())
            {
                if (ball.X == 0 || ball.X + 10 == 200) //若碰到左或右壁則移動方向相反(ball.X == 球的左座標、10 == 球直徑)
                {
                    move.X *= -1;
                }

                if (ball.Y == 50 || ball.Y + 10 == block.Y) //若碰到上或下則移動方向相反(ball.Y == 球的上座標)
                {
                    if (ball.Y + 10 == block.Y && !(block.X - 20 <= ball.X + 5 && ball.X + 5 <= block.X + 20)) //其中若球在下時反彈的位置不在方塊內則遊戲結束
                    {
                        timer1.Stop();
                        timer2.Stop();
                        playing = false;
                        toolStripStatusLabel2.Text = "Game Over!";                      
                        return;
                    }

                    move.Y *= -1;
                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e) //紅
        {
            color = Color.Red;
            toolStripButton1.Checked = true;
            toolStripButton2.Checked = toolStripButton3.Checked = false;
            Invalidate();
        }

        private void toolStripButton2_Click(object sender, EventArgs e) //綠
        {
            color = Color.Green;
            toolStripButton2.Checked = true;
            toolStripButton1.Checked = toolStripButton3.Checked = false;
            Invalidate();
        }

        private void toolStripButton3_Click(object sender, EventArgs e) //藍
        {
            color = Color.Blue;
            toolStripButton3.Checked = true;
            toolStripButton1.Checked = toolStripButton2.Checked = false;
            Invalidate();
        }
    }
}