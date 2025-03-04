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
using System.IO;

namespace _1093304_hw3
{
    public partial class Form1 : Form
    {
        Point point; //當前起點位置
        List<Point> startPt = new List<Point>(), endPt = new List<Point>(); //存起點和終點的點座標
        Color color = Color.Red; //當前顏色
        List<Color> colorPt = new List<Color>(); //存各種線段的顏色
        DashStyle dashStyle = DashStyle.Solid; //當前線段種類，預設實線
        List<DashStyle> dashStylePt = new List<DashStyle>(); //存各種線段的種類
        int width = 1; //當前線段的寬度，預設為1
        List<int> widthPt = new List<int>(); //存各種線段的寬度
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "線段檔(*.pnt)|*.pnt";
            saveFileDialog1.Filter = "線段檔(*.pnt)|*.pnt";
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            point = e.Location; //紀錄當前起點位置
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            startPt.Add(point); //存當前起點位置
            endPt.Add(e.Location); //存當前終點位置
            colorPt.Add(color); //存當前顏色
            dashStylePt.Add(dashStyle); //存當前線段種類
            widthPt.Add(width); //存當前線段寬度
            Invalidate(); //進入Form1_Paint並將所有線段重畫
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < endPt.Count(); i++) //將每條線重新畫
            {
                Pen pen = new Pen(colorPt[i], widthPt[i]); //建立pen並設定顏色和寬度
                pen.DashStyle = dashStylePt[i]; //設定線的種類
                e.Graphics.DrawLine(pen, startPt[i].X, startPt[i].Y, endPt[i].X, endPt[i].Y); //畫線
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK) //寫檔
            {
                string s = saveFileDialog1.FileName;
                BinaryWriter outFile = new BinaryWriter(File.Open(s, FileMode.Create));
                outFile.Write(startPt.Count()); //先存共有幾組線段
                for (int i = 0; i < endPt.Count(); i++) //每一條線段有起點和終點座標、顏色、線段種類和寬度
                {
                    outFile.Write(startPt[i].X);
                    outFile.Write(startPt[i].Y);
                    outFile.Write(endPt[i].X);
                    outFile.Write(endPt[i].Y);
                    outFile.Write(colorPt[i] == Color.Red ? 0 : (colorPt[i] == Color.Green ? 1 : 2)); //若顏色為紅則存0、綠為1、藍為2
                    outFile.Write(dashStylePt[i] == DashStyle.Solid ? 0 : 1); //若線段為實線則存0；否則虛線存1
                    outFile.Write(widthPt[i]); //存線段寬度
                }

                outFile.Close(); //寫檔關閉
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK) //讀檔
            {
                string s = openFileDialog1.FileName;
                if (!File.Exists(s))
                {
                    return;
                }

                BinaryReader inFile = new BinaryReader(File.Open(s, FileMode.Open));
                startPt.Clear(); //先將各種資料的內容清空，以便存讀檔進來的資料
                endPt.Clear();
                colorPt.Clear();
                dashStylePt.Clear();
                widthPt.Clear();
                int n = inFile.ReadInt32(); //檔案中第1筆資料代表有幾條線段
                for (int color, i = 0; i < n; i++)
                {
                    startPt.Add(new Point(inFile.ReadInt32(), inFile.ReadInt32()));
                    endPt.Add(new Point(inFile.ReadInt32(), inFile.ReadInt32()));
                    colorPt.Add((color = inFile.ReadInt32()) == 0 ? Color.Red : (color == 1 ? Color.Green : Color.Blue)); //0 == 紅、1 == 綠、2 == 藍
                    dashStylePt.Add(inFile.ReadInt32() == 0 ? DashStyle.Solid : DashStyle.Dash);
                    widthPt.Add(inFile.ReadInt32());
                }

                Invalidate(); //進入Form1_Paint並將所有線段重畫
                inFile.Close(); //讀檔關閉
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            startPt.Clear(); //所有資料內容清空
            endPt.Clear();
            colorPt.Clear();
            dashStylePt.Clear();
            widthPt.Clear();
            redToolStripMenuItem_Click(sender, e); //所有勾選回到預設
            solidToolStripMenuItem_Click(sender, e);
            toolStripMenuItem1_Click(sender, e);
            Invalidate(); //使畫面上的線段消失
        }

        private void redToolStripMenuItem_Click(object sender, EventArgs e)
        {
            color = Color.Red;
            redToolStripMenuItem.Checked = true;
            greenToolStripMenuItem.Checked = blueToolStripMenuItem.Checked = false;
        }

        private void greenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            color = Color.Green;
            greenToolStripMenuItem.Checked = true;
            redToolStripMenuItem.Checked = blueToolStripMenuItem.Checked = false;
        }

        private void blueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            color = Color.Blue;
            blueToolStripMenuItem.Checked = true;
            redToolStripMenuItem.Checked = greenToolStripMenuItem.Checked = false;
        }

        private void solidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dashStyle = DashStyle.Solid;
            solidToolStripMenuItem.Checked = true;
            dashToolStripMenuItem.Checked = false;
        }

        private void dashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dashStyle = DashStyle.Dash;
            dashToolStripMenuItem.Checked = true;
            solidToolStripMenuItem.Checked = false;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            width = 1;
            toolStripMenuItem1.Checked = true;
            toolStripMenuItem2.Checked = toolStripMenuItem3.Checked = toolStripMenuItem4.Checked = toolStripMenuItem5.Checked = false;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            width = 2;
            toolStripMenuItem2.Checked = true;
            toolStripMenuItem1.Checked = toolStripMenuItem3.Checked = toolStripMenuItem4.Checked = toolStripMenuItem5.Checked = false;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            width = 3;
            toolStripMenuItem3.Checked = true;
            toolStripMenuItem1.Checked = toolStripMenuItem2.Checked = toolStripMenuItem4.Checked = toolStripMenuItem5.Checked = false;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            width = 4;
            toolStripMenuItem4.Checked = true;
            toolStripMenuItem1.Checked = toolStripMenuItem2.Checked = toolStripMenuItem3.Checked = toolStripMenuItem5.Checked = false;
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            width = 5;
            toolStripMenuItem5.Checked = true;
            toolStripMenuItem1.Checked = toolStripMenuItem2.Checked = toolStripMenuItem3.Checked = toolStripMenuItem4.Checked = false;
        }
    }
}