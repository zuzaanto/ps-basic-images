using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Media;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;

namespace psModuleImageEdition
{
    public static class ImageFunctions
    {
        public static Bitmap Load(string source)
        {
            StreamReader streamReader = new StreamReader(source);
            var result = (Bitmap)Bitmap.FromStream(streamReader.BaseStream);
            streamReader.Close();
            return result;
        }
        public static byte Calculate(byte color1, byte color2,
                 ColorCalculationType calculationType)
        {
            byte resultValue = 0;
            int intResult = 0;

            if (calculationType == ColorCalculationType.Add)
            {
                intResult = color1 + color2;
            }
            else if (calculationType == ColorCalculationType.Average)
            {
                intResult = (color1 + color2) / 2;
            }
            else if (calculationType == ColorCalculationType.SubtractLeft)
            {
                intResult = color1 - color2;
            }
            else if (calculationType == ColorCalculationType.SubtractRight)
            {
                intResult = color2 - color1;
            }
            else if (calculationType == ColorCalculationType.Difference)
            {
                intResult = Math.Abs(color1 - color2);
            }
            else if (calculationType == ColorCalculationType.Multiply)
            {
                intResult = (int)((color1 / 255.0 * color2 / 255.0) * 255.0);
            }
            else if (calculationType == ColorCalculationType.Min)
            {
                intResult = (color1 < color2 ? color1 : color2);
            }
            else if (calculationType == ColorCalculationType.Max)
            {
                intResult = (color1 > color2 ? color1 : color2);
            }
            else if (calculationType == ColorCalculationType.Amplitude)
            {
                intResult = (int)(Math.Sqrt(color1 * color1 + color2 * color2)
                                                             / Math.Sqrt(2.0));
            }

            if (intResult < 0)
            {
                resultValue = 0;
            }
            else if (intResult > 255)
            {
                resultValue = 255;
            }
            else
            {
                resultValue = (byte)intResult;
            }

            return resultValue;
        }

        public enum ColorCalculationType
        {
            Average,
            Add,
            SubtractLeft,
            SubtractRight,
            Difference,
            Multiply,
            Min,
            Max,
            Amplitude
        }
        public static Bitmap ArithmeticBlend(this Bitmap sourceBitmap, Bitmap blendBitmap,
                                     ColorCalculationType calculationType)
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                    sourceBitmap.Width, sourceBitmap.Height),
                                    ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);

            BitmapData blendData = blendBitmap.LockBits(new Rectangle(0, 0,
                                    blendBitmap.Width, blendBitmap.Height),
                                    ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            byte[] blendBuffer = new byte[blendData.Stride * blendData.Height];
            Marshal.Copy(blendData.Scan0, blendBuffer, 0, blendBuffer.Length);
            blendBitmap.UnlockBits(blendData);

            for (int k = 0; (k + 4 < pixelBuffer.Length) &&
                            (k + 4 < blendBuffer.Length); k += 4)
            {
                pixelBuffer[k] = Calculate(pixelBuffer[k],
                                 blendBuffer[k], calculationType);

                pixelBuffer[k + 1] = Calculate(pixelBuffer[k + 1],
                                     blendBuffer[k + 1], calculationType);

                pixelBuffer[k + 2] = Calculate(pixelBuffer[k + 2],
                                     blendBuffer[k + 2], calculationType);
            }

            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);

            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                    resultBitmap.Width, resultBitmap.Height),
                                    ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            Marshal.Copy(pixelBuffer, 0, resultData.Scan0, pixelBuffer.Length);
            resultBitmap.UnlockBits(resultData);

            return resultBitmap;
        }
        public static Bitmap CopyToSquareCanvas(this Bitmap sourceBitmap, int canvasWidthLenght)
        {
            float ratio = 1.0f;
            int maxSide = sourceBitmap.Width > sourceBitmap.Height ?
                          sourceBitmap.Width : sourceBitmap.Height;

            ratio = (float)maxSide / (float)canvasWidthLenght;

            Bitmap bitmapResult = (sourceBitmap.Width > sourceBitmap.Height ?
                                    new Bitmap(canvasWidthLenght, (int)(sourceBitmap.Height / ratio))
                                    : new Bitmap((int)(sourceBitmap.Width / ratio), canvasWidthLenght));

            using (Graphics graphicsResult = Graphics.FromImage(bitmapResult))
            {
                graphicsResult.CompositingQuality = CompositingQuality.HighQuality;
                graphicsResult.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphicsResult.PixelOffsetMode = PixelOffsetMode.HighQuality;

                graphicsResult.DrawImage(sourceBitmap,
                                        new Rectangle(0, 0,
                                            bitmapResult.Width, bitmapResult.Height),
                                        new Rectangle(0, 0,
                                            sourceBitmap.Width, sourceBitmap.Height),
                                            GraphicsUnit.Pixel);
                graphicsResult.Flush();
            }

            return bitmapResult;
        }
    }
}
