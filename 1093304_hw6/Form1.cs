using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace _1093304_hw6
{
    public partial class Form1 : Form
    {
        Image[] bg = new Image[3] { Properties.Resources.Hydrangeas, Properties.Resources.Penguins, Properties.Resources.Tulips }; //背景圖
        Image[] fruit = new Image[3] { Properties.Resources.Banana, Properties.Resources.StawBerry, Properties.Resources.Tomato }; //3種水果
        Image bowl = Properties.Resources.Bowl; //碗       
        Rectangle rectDest; //設定視窗上的矩形範圍
        Point[] startPos = new Point[3]; //3種水果開始掉落的點座標
        Point bgSize, bowlPos; //設定背景長寬、碗的正中心點座標
        Random rd = new Random(); //亂數
        int cur = 0, counter, second; //當前的背景圖、碗中水果數、秒數
        bool playing; //遊玩狀態
        float[][] cmArray = //色彩調整矩陣，調整為半透明
           { new float[] { 1, 0, 0, 0, 0 }, new float[] { 0, 1, 0, 0, 0 }, new float[] { 0, 0, 1, 0, 0 }, new float[] { 0, 0, 0, 0.5f, 0 }, new float[] { 0, 0, 0, 0, 1 } };

        public Form1()
        {
            InitializeComponent();
            bgSize = new Point(bg[0].Width / 2, 350); //背景固定長寬           
            rectDest = new Rectangle(0, 25, bgSize.X, bgSize.Y); //設定視窗上的矩形固定範圍
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Size = new Size(bgSize.X + 16, 550); //設定視窗大小
            for (int i = 0; i < 3; startPos[i++] = new Point(rd.Next(10, bgSize.X - 40), rd.Next(0, 20))) ; //亂數給定3種水果的起始掉落座標
            label2.Text = (second = 120).ToString() + " Seconds"; //初始遊戲時間倒數120秒
            label4.Text = (counter = 0).ToString(); //初始碗中水果數為0           
            bowlPos = new Point(bgSize.X / 2, bgSize.Y + 25); //碗的正中心
            playing = true; //true == 開始，false == 結束
            timer1.Start(); //每10秒換一次背景
            timer2.Start(); //每1毫秒刷新水果位置
            timer3.Start(); //倒數計時120秒
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Invalidate(); //介面失效
            timer1.Stop(); timer2.Stop(); timer3.Stop(); //所有計時暫停
            Form1_Load(sender, e); //所有參數初始化
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            ColorMatrix cm = new ColorMatrix(cmArray); //放入色彩調整矩陣
            ImageAttributes ia = new ImageAttributes();
            ia.SetColorMatrix(cm, ColorMatrixFlag.Default, ColorAdjustType.Bitmap); //將圖片調整為半透明
            e.Graphics.DrawImage(bg[cur], rectDest, 0, 0, bg[cur].Width, bg[cur].Height, GraphicsUnit.Pixel, ia); //繪出半透明背景
            e.Graphics.DrawImage(bowl, bowlPos.X - bowl.Width / 2, bowlPos.Y, bowl.Width, bowl.Height); //繪出碗
            for (int i = 0; i < 3; i++) //繪出3種水果
            {
                e.Graphics.DrawImage(fruit[i], startPos[i].X, startPos[i].Y, fruit[i].Width, fruit[i].Height);
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (playing) //若正在遊戲中
            {
                //設定碗可移動的左右邊界，一旦滑鼠移動，則更改碗的中心位置
                bowlPos.X = e.X < bowl.Width / 2 ? bowl.Width / 2 : (e.X > bgSize.X - bowl.Width / 2 ? bgSize.X - bowl.Width / 2 : e.X);
            }
        }

        private void timer1_Tick(object sender, EventArgs e) //每10秒換一次背景
        {
            ++cur; cur %= 3; //cur == 0 ~ 2循環(對應bg[0] ~ bg[2]中的3張背景)
            Invalidate();
        }

        private void timer2_Tick(object sender, EventArgs e) //每1毫秒刷新水果位置
        {
            for (int i = 0; i < 3; ++startPos[i++].Y) //每次將該水果Y座標 + 1
            {
                if (startPos[i].Y == bgSize.Y) //若水果到達底部
                {
                    //若水果的長度中心點在碗的範圍內
                    if (bowlPos.X - bowl.Width / 2 <= startPos[i].X + fruit[i].Width / 2 && startPos[i].X + fruit[i].Width / 2 <= bowlPos.X + bowl.Width / 2)
                    {
                        label4.Text = (++counter).ToString(); //則視為該水果有被碗接住，碗內水果數 + 1
                    }

                    startPos[i] = new Point(rd.Next(10, bgSize.X - 40), rd.Next(0, 20)); //重新設定起始掉落座標
                }
            }

            Invalidate();
        }

        private void timer3_Tick(object sender, EventArgs e) //倒數計時120秒
        {
            if (playing) //若正在遊戲中
            {
                label2.Text = (--second).ToString() + " Seconds";
                if (second == 0) //若倒數計時至0，則所有計時停止，遊玩狀態結束
                {
                    timer1.Stop(); timer2.Stop(); timer3.Stop(); //所有計時暫停
                    playing = false; //遊戲結束
                }
            }
        }
    }
}