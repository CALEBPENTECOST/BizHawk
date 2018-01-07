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

        DisplayModes dispMode = DisplayModes.RGB;

        public UserView(Backend attachedBackend)
        {    
            InitializeComponent();

            mBackendModel = attachedBackend;

            // Set up the combo box and choose the first item
            comboBox_imageMode.Items.Clear();
            foreach (var item in Enum.GetValues(typeof(DisplayModes)))
            {
                comboBox_imageMode.Items.Add(item);
            }
            comboBox_imageMode.SelectedIndex = 0;
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

            int monoKernel_w = 1;
            int monoKernel_h = 1;
            switch (dispMode)
            {
                case DisplayModes.Monochrome_Quarter:
                    monoKernel_w = 4;
                    monoKernel_h = 4;
                    goto case DisplayModes.Monochrome;
                case DisplayModes.Monochrome_Half:
                    monoKernel_w = 2;
                    monoKernel_h = 2;
                    goto case DisplayModes.Monochrome;
                case DisplayModes.Monochrome:
                    // We need to extract the monochrome image and assign the output image to the result
                    var grayData = mBackendModel.capuredImage.ToGrayscaleImage(monoKernel_w, monoKernel_h);
                    var grayBmp = CopyDataToBitmap(grayData);
                    image = grayBmp;
                    break;
                case DisplayModes.RGB:
                    // Do nothing special
                    break;
                default:
                    // Should never get here. Assume color image
                    break;
            }

            if (dispMode != DisplayModes.RGB)
            {
               
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

            // Update the RAM watch as well
            uint watchVal = mBackendModel.GetWatchVal(textBox_watchName.Text);
            textBox_watchValUint.Text = watchVal.ToString();
            textBox_watchValFloat.Text = ((float)watchVal).ToString();
        }

        private void UpdateFrameCounter()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Enumerates all of the ways the input image is handled
        /// </summary>
        private enum DisplayModes
        {
            RGB,
            Monochrome,
            Monochrome_Half,
            Monochrome_Quarter
        }

        private void comboBox_imageMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            // When the color mode changes, we need to set our enum appropriately
            dispMode = (DisplayModes)comboBox_imageMode.SelectedItem;
        }

        private void button_setWatch_Click(object sender, EventArgs e)
        {
            // The user wants to set a watch! Call the appropriate function
            SetUserWatch();
        }

        private void SetUserWatch()
        {
            // Get the data from the text boxes
            string wName = textBox_watchName.Text;

            int addressVal = Convert.ToInt32(textBox_watchAddr.Text, 16);
            long wAddr = (long)addressVal;

            string wNotes = textBox_watchNotes.Text;

            // Tell the backend what we want to do
            mBackendModel.ClearRamWatches();
            mBackendModel.AddRamWatch(wName, wAddr, wNotes);
        }
    }
}
