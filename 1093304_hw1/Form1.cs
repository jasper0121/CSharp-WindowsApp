using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1093304_hw1
{
    public partial class Form1 : Form
    {
        Brush[,] brush = new Brush[3, 3]; //建立九宮格陣列儲存顏色
        Random rd = new Random(); //隨機亂數
        int r, g, b; //存r,g,b值
        public Form1()
        {
            InitializeComponent();
            for (int y = 0; y < 3; y++) //算出9種顏色並儲存
            {
                for (int x = 0; x < 3; x++)
                {
                    r = rd.Next(256);
                    g = rd.Next(256);
                    b = rd.Next(256);
                    brush[x, y] = new SolidBrush(Color.FromArgb(r, g, b));
                }
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    e.Graphics.FillRectangle(brush[x, y], x * 50, y * 50, 50, 50); //將陣列中的顏色畫出來
                    e.Graphics.DrawRectangle(Pens.Black, new Rectangle(x * 50, y * 50, 50, 50)); //正方形描黑邊
                }
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.X < 150 && e.Y < 150) //若點擊位置在範圍內，則重算該位置的顏色並重新上色
            {
                r = rd.Next(256);
                g = rd.Next(256);
                b = rd.Next(256);
                brush[e.X / 50, e.Y / 50] = new SolidBrush(Color.FromArgb(r, g, b));
                Graphics G = CreateGraphics();               
                G.FillRectangle(brush[e.X / 50, e.Y / 50], e.X / 50 * 50, e.Y / 50 * 50, 50, 50); //e.X / 50 * 50 == 當前點擊正方形的左上角X座標，Y座標同理
                G.DrawRectangle(Pens.Black, new Rectangle(e.X / 50 * 50, e.Y / 50 * 50, 50, 50)); //正方形描黑邊
            }
        }
    }
}
