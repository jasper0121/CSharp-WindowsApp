using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging; //ColorMatrix

namespace _1093304_hw7
{
    public partial class Form1 : Form
    {
        int w, h, x0, y0, pen_width = 1, tools = 0;
        Bitmap img1, img2, BackupImg, BackupImg2;
        Pen p;
        Color c = Color.Black, fillColor = Color.Black;

        public Form1()
        {
            InitializeComponent();
            p = new Pen(c, pen_width);
            toolStripStatusLabel1.Text = "Width: " + w.ToString() + ", Height: " + h.ToString();
            toolStripStatusLabel3.Text = "Pen: " + c.ToString();
            toolStripStatusLabel4.Text = "Brush: " + fillColor.ToString();
            復原ToolStripMenuItem.Enabled = 取消復原ToolStripMenuItem.Enabled = false;
        }

        public Bitmap ConvertCM(Image image, ColorMatrix cm)
        {
            Bitmap dest = new Bitmap(image.Width, image.Height);
            Graphics g = Graphics.FromImage(dest); //從點陣圖建立新的畫布
            // cm 定義含有 RGBA 空間座標的 5 x 5 矩陣
            // (R, G, B, A, 1) 乘上 此矩陣      
            ImageAttributes ia = new ImageAttributes(); //ImageAttributes 類別的多個方法會使用色彩矩陣來調整影像色彩
            ia.SetColorMatrix(cm); //設定預設分類的色彩調整矩陣
            g.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, ia);
            g.Dispose(); //清掉畫布與點陣圖變數的連結
            BackupImg = (Bitmap)pictureBox1.Image.Clone();
            復原ToolStripMenuItem.Enabled = true;
            return dest;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                x0 = e.X;
                y0 = e.Y;
                img2 = (Bitmap)pictureBox1.Image.Clone();
                BackupImg = (Bitmap)pictureBox1.Image.Clone();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            復原ToolStripMenuItem.Enabled = true;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                toolStripStatusLabel2.Text = (e.X < w && e.Y < h && e.X > 0 && e.Y > 0) ? e.Location.ToString() : "{,}";
                if (e.Button == MouseButtons.Left)
                {
                    switch (tools)
                    {
                        case 0:
                            Graphics g = Graphics.FromImage(pictureBox1.Image);
                            if (e.X < w && e.Y < h && e.X > 0 && e.Y > 0) //直接改pictureBox1的圖
                            {
                                g.DrawLine(p, x0, y0, e.X, e.Y);
                                x0 = e.X;
                                y0 = e.Y;
                                pictureBox1.Refresh();
                            }

                            break;
                        case 1:
                            if (e.X < w && e.Y < h && e.X > 0 && e.Y > 0)
                            {
                                Bitmap tempImg = (Bitmap)img2.Clone();
                                Graphics gg = Graphics.FromImage(tempImg); //複製一份點陣圖，最後再放回pictureBox1
                                gg.DrawLine(p, x0, y0, e.X, e.Y);
                                pictureBox1.Image = tempImg;
                                pictureBox1.Refresh();
                            }

                            break;
                        default:
                            if (e.X < w && e.Y < h && e.X > 0 && e.Y > 0)
                            {
                                Bitmap tempImg = (Bitmap)img2.Clone();
                                Graphics gg = Graphics.FromImage(tempImg); //複製一份點陣圖，最後再放回pictureBox1
                                if (tools == 2) //畫矩形
                                {
                                    if (填滿ToolStripMenuItem.Checked) //要求填滿
                                    {
                                        gg.FillRectangle(new SolidBrush(fillColor), Math.Min(x0, e.X), Math.Min(y0, e.Y), Math.Abs(e.X - x0), Math.Abs(e.Y - y0));
                                    }

                                    gg.DrawRectangle(p, Math.Min(x0, e.X), Math.Min(y0, e.Y), Math.Abs(e.X - x0), Math.Abs(e.Y - y0));
                                }
                                else //畫圓形
                                {
                                    if (填滿ToolStripMenuItem.Checked) //要求填滿
                                    {
                                        gg.FillEllipse(new SolidBrush(fillColor), Math.Min(x0, e.X), Math.Min(y0, e.Y), Math.Abs(e.X - x0), Math.Abs(e.Y - y0));
                                    }

                                    gg.DrawEllipse(p, Math.Min(x0, e.X), Math.Min(y0, e.Y), Math.Abs(e.X - x0), Math.Abs(e.Y - y0));
                                }

                                pictureBox1.Image = tempImg;
                                pictureBox1.Refresh();
                            }

                            break;
                    }
                }
            }
        }

        private void 開啟舊檔ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK) //開啟影像檔
            {
                string input = openFileDialog1.FileName;
                img1 = (Bitmap)Image.FromFile(input); //產生一個Image物件
                w = img1.Width;
                h = img1.Height;
                if (ClientSize.Width < w || ClientSize.Height < h)
                {
                    ClientSize = new Size(w, h + 56);
                }

                pictureBox1.Image = img1;
                pictureBox1.Refresh(); //要求重畫
                toolStripStatusLabel1.Text = "Width: " + w.ToString() + ", Height: " + h.ToString();
            }
        }

        private void 開啟新檔ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 x = new Form2();
            x.TopMost = true;  //移到最上層
            x.Text = "設定畫布的寬與高"; //Form2 的標題
            x.ShowDialog(); //Show Form2
            w = x.getWidth();
            h = x.getHeight();
            if (w != -1)
            {
                img1 = new Bitmap(w, h);
                pictureBox1.Image = img1;
                Graphics g = Graphics.FromImage(pictureBox1.Image);
                g.Clear(Color.White);
                pictureBox1.Refresh(); // 要求重畫
                if (ClientSize.Width < w || ClientSize.Height < (h + 56))
                {
                    ClientSize = new Size(w, h + 56);
                }

                toolStripStatusLabel1.Text = "Width: " + w.ToString() + ", Height: " + h.ToString();
            }
        }

        private void 儲存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null && saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string output = saveFileDialog1.FileName;
                pictureBox1.Image.Save(output);
                img1 = (Bitmap)pictureBox1.Image;
            }
        }

        private void 灰階ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                ColorMatrix cm = new ColorMatrix(new float[][] { new float[] { 0.33f, 0.33f, 0.33f, 0, 0 }, new float[] { 0.33f, 0.33f, 0.33f, 0, 0 },
                    new float[] { 0.33f, 0.33f, 0.33f, 0, 0 }, new float[] { 0, 0, 0, 1, 0 }, new float[] { 0, 0, 0, 0, 1 } });
                pictureBox1.Image = ConvertCM(pictureBox1.Image, cm);
            }
        }

        private void 負片ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                ColorMatrix cm = new ColorMatrix(new float[][] { new float[] { -1f, 0, 0, 0, 0 }, new float[] { 0, -1f, 0, 0, 0 },
                    new float[] { 0, 0, -1f, 0, 0 }, new float[] { 0, 0, 0, 1, 0 }, new float[] { 1, 1, 1, 0, 1 } });
                pictureBox1.Image = ConvertCM(pictureBox1.Image, cm);
            }
        }

        private void 自訂ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Form3 x = new Form3();
                x.TopMost = true; //移到最上層
                x.Text = "調整畫布的顏色"; //Form3的標題
                x.ShowDialog(); //Show Form3
                ColorMatrix cm = new ColorMatrix(new float[][] { new float[] { x.getR(), 0, 0, 0, 0 }, new float[] { 0, x.getG(), 0, 0, 0 },
                    new float[] { 0, 0, x.getB(), 0, 0 }, new float[] { 0, 0, 0, x.getA(), 0 }, new float[] { 0, 0, 0, 0, 1 } });
                pictureBox1.Image = ConvertCM(pictureBox1.Image, cm);
            }
        }

        private void 增亮ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                ColorMatrix cm = new ColorMatrix(new float[][] { new float[] { 1.1f, 0, 0, 0, 0 }, new float[] { 0, 1.1f, 0, 0, 0 },
                    new float[] { 0, 0, 1.1f, 0, 0 }, new float[] { 0, 0, 0, 1, 0 }, new float[] { 0, 0, 0, 0, 1 } });
                pictureBox1.Image = ConvertCM(pictureBox1.Image, cm);
            }
        }

        private void 調暗ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                ColorMatrix cm = new ColorMatrix(new float[][] { new float[] { 0.9f, 0, 0, 0, 0 }, new float[] { 0, 0.9f, 0, 0, 0 },
                       new float[] { 0, 0, 0.9f, 0, 0 }, new float[] { 0, 0, 0, 1, 0 }, new float[] { 0, 0, 0, 0, 1 } });
                pictureBox1.Image = ConvertCM(pictureBox1.Image, cm);
            }
        }

        private void 一半ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BackupImg = (Bitmap)pictureBox1.Image.Clone();
            復原ToolStripMenuItem.Enabled = true;
            w = pictureBox1.Image.Width;
            h = pictureBox1.Image.Height;
            img1 = (Bitmap)pictureBox1.Image;
            Bitmap p = new Bitmap(w / 2, h / 2);
            for (int i = 0; i < w && (i + 2 < w); i += 2)
            {
                for (int j = 0; j < h && (j + 2 < h); j += 2)
                {
                    Color c = img1.GetPixel(i, j);
                    p.SetPixel(i / 2, j / 2, c);
                }
            }

            pictureBox1.Image = img1 = p;
            w = p.Width;
            h = p.Height;
            pictureBox1.Refresh();
            toolStripStatusLabel1.Text = "Width: " + w.ToString() + ", Height: " + h.ToString();
        }

        private void 二倍ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BackupImg = (Bitmap)pictureBox1.Image.Clone();
            復原ToolStripMenuItem.Enabled = true;
            w = pictureBox1.Image.Width;
            h = pictureBox1.Image.Height;
            Bitmap p = new Bitmap(w * 2, h * 2);
            img1 = (Bitmap)pictureBox1.Image;
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    Color c = img1.GetPixel(i, j);
                    for (int ii = 0; ii < 2; ii++)
                    {
                        for (int jj = 0; jj < 2; jj++)
                        {
                            p.SetPixel(i * 2 + ii, j * 2 + jj, c); //垂直與水平方向都重複畫兩遍
                        }
                    }
                }
            }

            pictureBox1.Image = img1 = p;
            w = p.Width;
            h = p.Height;
            pictureBox1.Refresh();
            toolStripStatusLabel1.Text = "Width: " + w.ToString() + ", Height: " + h.ToString();
            if (ClientSize.Width < w || ClientSize.Height < (h + 56))
            {
                ClientSize = new Size(w, h + 56);
            }
        }

        private void 點ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            點ToolStripMenuItem.Checked = true;
            直線ToolStripMenuItem.Checked = 矩形ToolStripMenuItem.Checked = 圓ToolStripMenuItem.Checked = false;
            tools = 0;
        }

        private void 直線ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            直線ToolStripMenuItem.Checked = true;
            點ToolStripMenuItem.Checked = 矩形ToolStripMenuItem.Checked = 圓ToolStripMenuItem.Checked = false;
            tools = 1;
        }

        private void 矩形ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            矩形ToolStripMenuItem.Checked = true;
            點ToolStripMenuItem.Checked = 直線ToolStripMenuItem.Checked = 圓ToolStripMenuItem.Checked = false;
            tools = 2;
        }

        private void 圓ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            圓ToolStripMenuItem.Checked = true;
            點ToolStripMenuItem.Checked = 直線ToolStripMenuItem.Checked = 矩形ToolStripMenuItem.Checked = false;
            tools = 3;
        }

        private void 顏色ToolStripMenuItem_Click(object sender, EventArgs e) //筆刷顏色
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                c = colorDialog1.Color;
                p = new Pen(c, pen_width);
                toolStripStatusLabel3.Text = "Pen: " + c.ToString();
            }
        }


        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            pen_width = 1;
            toolStripMenuItem2.Checked = true;
            toolStripMenuItem3.Checked = toolStripMenuItem4.Checked = toolStripMenuItem5.Checked = toolStripMenuItem6.Checked = false;
            p = new Pen(c, pen_width);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            pen_width = 2;
            toolStripMenuItem3.Checked = true;
            toolStripMenuItem2.Checked = toolStripMenuItem4.Checked = toolStripMenuItem5.Checked = toolStripMenuItem6.Checked = false;
            p = new Pen(c, pen_width);
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            pen_width = 3;
            toolStripMenuItem4.Checked = true;
            toolStripMenuItem2.Checked = toolStripMenuItem3.Checked = toolStripMenuItem5.Checked = toolStripMenuItem6.Checked = false;
            p = new Pen(c, pen_width);
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            pen_width = 4;
            toolStripMenuItem5.Checked = true;
            toolStripMenuItem2.Checked = toolStripMenuItem3.Checked = toolStripMenuItem4.Checked = toolStripMenuItem6.Checked = false;
            p = new Pen(c, pen_width);
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            pen_width = 5;
            toolStripMenuItem6.Checked = true;
            toolStripMenuItem2.Checked = toolStripMenuItem3.Checked = toolStripMenuItem4.Checked = toolStripMenuItem5.Checked = false;
            p = new Pen(c, pen_width);
        }

        private void 顏色ToolStripMenuItem1_Click(object sender, EventArgs e) //畫刷顏色
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                fillColor = colorDialog1.Color;
                toolStripStatusLabel4.Text = "Brush: " + fillColor.ToString();
            }
        }

        private void 填滿ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            填滿ToolStripMenuItem.Checked = true;
            無填滿ToolStripMenuItem.Checked = false;
        }

        private void 無填滿ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            無填滿ToolStripMenuItem.Checked = true;
            填滿ToolStripMenuItem.Checked = false;
        }

        private void 復原ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            復原ToolStripMenuItem.Enabled = false;
            取消復原ToolStripMenuItem.Enabled = true;
            BackupImg2 = (Bitmap)pictureBox1.Image;
            pictureBox1.Image = BackupImg;
            w = BackupImg.Width;
            h = BackupImg.Height;
            pictureBox1.Refresh();
        }

        private void 取消復原ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            復原ToolStripMenuItem.Enabled = true;
            取消復原ToolStripMenuItem.Enabled = false;
            pictureBox1.Image = BackupImg2;
            w = BackupImg.Width;
            h = BackupImg.Height;
            pictureBox1.Refresh();
        }
    }
}