
using Emgu.CV;
using Emgu.CV.Stitching;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Forms;

namespace test_k
{
    public partial class Form1 : Form
    {

        List<Bitmap> sol_image = new List<Bitmap>();//记录所有图片
        Rectangle MouseRect = Rectangle.Empty;//选框rect
        Bitmap btm = new Bitmap("D:\\3.jpg");//复制源
        Mat picout = new Mat();

        public Form1()
        {
            Image image1 = Image.FromFile("D:\\3.jpg");
            this.BackgroundImage = image1;
            this.MouseMove += new MouseEventHandler(frmMain_MouseMove);
            //timer
            System.Windows.Forms.Timer time1 = new System.Windows.Forms.Timer();
            time1.Enabled = false;
            time1.Interval = 2000;

            time1.Tick += new EventHandler(timer1_Tick);
            InitializeComponent();
        }
        //timer间隔记录btm2
        private void timer1_Tick(object sender, EventArgs e)
        {
            //if (btm2 != null) { sol_image.Add(btm2); }
            label3.Text = sol_image.Count.ToString();
        }
        int XX = 0;
        int i = 0;//记录截图
        int k = 0;//记录失败事件
        //mouseMove事件*******************************************************
        void frmMain_MouseMove(object sender, MouseEventArgs e)
        {
            int X_tmp = e.X;
            ResizeToRectangle(e.Location);
            //pictureBox1.Invalidate();
            //this.Invalidate();
            draw(X_tmp);
            if (e.X - XX > 30) { //jietu
                jietu(X_tmp);
                i++;
                this.label3.Text = i + "";
            }
            //sol_image.Add(btm2);
        }
        private void ResizeToRectangle(Point p)
        {
            DrawRectangle();
            MouseRect.Width = MouseRect.Height = 200;
            MouseRect.X = p.X - 100;
            MouseRect.Y = p.Y - 100;
            label1.Text = p.X.ToString();
            label2.Text = p.Y.ToString();
            DrawRectangle();
        }

        private void DrawRectangle()
        {
            Rectangle rect2 = this.RectangleToScreen(MouseRect);
            ControlPaint.DrawReversibleFrame(rect2, Color.Red, FrameStyle.Dashed);
        }
        //mouseMove事件*******************************************************
        
        /*
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
                Bitmap btm2 = null;
                Rectangle rect = Rectangle.Empty;//选框rect
                rect.Width = MouseRect.Width;
                rect.Height = MouseRect.Height;
                rect.X = 0;
                rect.Y = 0;
                //MouseRect.Y = MouseRect.Y;
                btm2 = btm.Clone(MouseRect, curBitmap.PixelFormat);
                g.DrawImage(btm2, rect);
                pictureBox2.Image = btm2;
        }
        */
        Bitmap btm2 = null;
        Bitmap btm3 = null;
        System.Drawing.Imaging.PixelFormat format;
        public void draw(int X_tmp)
        {
            format = btm.PixelFormat;
            //btm2 = null;
            Rectangle rect = Rectangle.Empty;//选框rect
            rect.Width = MouseRect.Width;
            rect.Height = MouseRect.Height;
            rect.X = 0;
            rect.Y = 0;
            //MouseRect.Y = MouseRect.Y;

            //if (MouseRect.X < 0 || MouseRect.Y < 0) { }
            //else
            //{
            //    btm2 = btm.Clone(MouseRect, format);
            //    pictureBox2.Image = btm2;
            //    XX = X_tmp;
            //}
        }
        int j = 0;
        public void jietu(int X_tmp) {
            if (MouseRect.X < 0 || MouseRect.Y < 0) { }
            else
            {
                btm2 = btm.Clone(MouseRect, format);
                pictureBox2.Image = btm2;
                XX = X_tmp;
                hebing(btm2);
            }
        }
        //缩略图
        public bool ThumbnailCallback()
        {
            return false;
        }

        private void buttonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Image.GetThumbnailImageAbort myCallback =
            new Image.GetThumbnailImageAbort(ThumbnailCallback);
            Bitmap myBitmap = new Bitmap("D:\\1.jpg");
            Image myThumbnail = myBitmap.GetThumbnailImage(
            40, 40, myCallback, IntPtr.Zero);
            //e.Graphics.DrawImage(myThumbnail, 150, 75);
            pictureBox1.Image = myThumbnail;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void 合成ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            // Image<Gray, Byte> tmp = new Image<Gray, byte>(size.Width, size.Height, mat.Step, mat.DataPointer)
            // Image<Bgr, byte> c = new Image<Bgr, byte>(picout.ToString());
            Image<Bgr, byte> a = new Image<Bgr, byte>("D:\\c.jpg");
            Image<Bgr, byte> src = new Image<Bgr, byte>("D:\\c.jpg");
            //Image<Bgr, byte> c = new Image<Bgr, byte>("D:\\c.jpg");
            Stitcher stitcher = new Stitcher(false);
            Mat outimg = new Mat();
            try
            {
               // MessageBox.Show("d");
                imageBox2.Image = a.Mat;
                imageBox3.Image = src.Mat;
                stitcher.Stitch(new VectorOfMat(new Mat[] { a.Mat, src.Mat }), outimg);
                //MessageBox.Show("s");
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            imageBox1.Image = outimg;
            picout = outimg;
        }

        public void hebing(Bitmap btm2) {
            sol_image.Add(btm2);
            Mat outimg = new Mat();
            Image<Bgr, Byte> src = new Image<Bgr, Byte>(btm2);
            if (btm3 == null) { btm3 = btm2; }
            else {
                btm3 = outimg.Bitmap;
            }
            if (btm3 == null) { btm3 = btm2; }
            Image<Bgr, Byte> src2 = new Image<Bgr, Byte>(btm3);
            Stitcher stitcher = new Stitcher(false);
            try
            {
                imageBox2.Image = src2.Mat;
                imageBox3.Image = src.Mat;
                stitcher.Stitch(new VectorOfMat(new Mat[] { src2.Mat, src.Mat }), outimg);
                j++;
                label4.Text = j + "";
                if (outimg.Bitmap == null) {
                    k++;
                    label5.Text = k + "";
                }
                //MessageBox.Show("s");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            imageBox1.Image = outimg;
        }

        private void 合成测试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Image<Bgr, Byte>> tmp = new List<Image<Bgr, byte>>();
            Mat outimg = new Mat();
            for (int i = 0; i < sol_image.Count; i++) {
                Image<Bgr, Byte> src = new Image<Bgr, Byte>(sol_image[i]);
                tmp.Add(src);
            }
            MessageBox.Show(tmp.Count.ToString());
            Stitcher stitcher = new Stitcher(false);
            try
            {
                stitcher.Stitch(new VectorOfMat(new Mat[] { tmp[1].Mat, tmp[2].Mat,tmp[3].Mat, tmp[4].Mat, tmp[5].Mat, tmp[6].Mat, tmp[7].Mat, tmp[8].Mat, tmp[9].Mat, tmp[10].Mat }), outimg);
                //MessageBox.Show("s");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            imageBox1.Image = outimg;
        }



        //Point picPoint = pictureBox1.PointToClient(Control.MousePosition); //鼠标相对于button1左上角的坐标
        //e.Graphics.DrawRectangle(new Pen(Color.Red, 3),rect);//重新绘制颜色为红色
    }
}
