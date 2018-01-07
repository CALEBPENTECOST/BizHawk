using BizHawk.Bizware.BizwareGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CPENTECOST.BizHawk.DeepLearning
{
    public partial class UserView : Form
    {
        Backend mBackendModel = null;

        bool grayscale = false;

        public UserView(Backend attachedBackend)
        {    
            InitializeComponent();

            mBackendModel = attachedBackend;
        }

        
        byte[] testData = null;
        public byte[] getTestData()
        {
            if(testData == null)
            {
                // Create test data
                int w = 320;
                int h = 240;
                testData = new byte[w * h];

                // For each row
                for(int row = 0; row < h; ++row)
                {
                    for (int col = 0; col < w; ++col)
                    {
                        testData[(row * w) + col] = (byte)(col % 0xFF);
                    }
                }
            }

            return testData;
        }
        

        public ColorPalette GetPalette(ColorPalette palette)
        {
           var grayScaleColorPalette = palette;
            for (int i = 0; i <= 255; i++)
            {
                // create greyscale color table
                grayScaleColorPalette.Entries[i] = Color.FromArgb(i, i, i);
            }
             
            return grayScaleColorPalette;
        }

        public Bitmap CopyDataToBitmap(BitmapBuffer.QuickGrayImage grayscale)
        {
            //Here create the Bitmap to the know height, width and format
            IntPtr unmanagedPointer = Marshal.AllocHGlobal(grayscale.Data.Length);
            Marshal.Copy(grayscale.Data, 0, unmanagedPointer, grayscale.Data.Length);

            // Call unmanaged code
            Bitmap bmp = new Bitmap(grayscale.Width, grayscale.Height, grayscale.Stride, PixelFormat.Format8bppIndexed, unmanagedPointer);
            bmp.Palette = GetPalette(bmp.Palette);
            Marshal.FreeHGlobal(unmanagedPointer);

            ////Create a BitmapData and Lock all pixels to be written 
            //BitmapData bmpData = bmp.LockBits(
            //                     new Rectangle(0, 0, bmp.Width, bmp.Height),
            //                     ImageLockMode.WriteOnly, bmp.PixelFormat);
            //
            ////Copy the data from the byte array into BitmapData.Scan0
            //Marshal.Copy(grayscale.Data, 0, bmpData.Scan0, grayscale.Data.Length);
            //
            ////Unlock the pixels
            //bmp.UnlockBits(bmpData);

            //Return the bitmap 
            return bmp;
        }

        private void UpdatePictureBox()
        {
            // We now have an image prolly!! Just show it
            var image = mBackendModel.capuredImage.ToSysdrawingBitmap();
            if (grayscale)
            {
                var grayData = mBackendModel.capuredImage.ToGrayscaleImage();
                //grayData.Data = getTestData();
                var grayBmp = CopyDataToBitmap(grayData);
                image = grayBmp;
            }


            pictureBox_inputImage.Image = image;
            pictureBox_inputImage.Invalidate();
        }

        internal void ForceViewUpdate()
        {
            // Get the frame counter
            UpdateFrameCounter();

            // Also update the picture box
            UpdatePictureBox();
        }

        private void UpdateFrameCounter()
        {
            //throw new NotImplementedException();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            grayscale = checkBox1.Checked;
        }
    }
}
