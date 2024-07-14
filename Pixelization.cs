using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixelization
{
    class Pixelization
    {
        public static Bitmap PixelateAverageColor(Bitmap inputImage, int maxWidth, int maxHeight)
        {
            float aspectRatio = (float)inputImage.Width / inputImage.Height;
            int targetWidth = Math.Min(inputImage.Width, maxWidth);
            int targetHeight = (int)(targetWidth / aspectRatio);

            if (targetHeight > maxHeight)
            {
                targetHeight = maxHeight;
                targetWidth = (int)(targetHeight * aspectRatio);
            }

            Bitmap pixelatedImage = new Bitmap(targetWidth, targetHeight);

            using (Graphics graphics = Graphics.FromImage(pixelatedImage))
            {
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                graphics.DrawImage(inputImage, new Rectangle(0, 0, targetWidth, targetHeight));
            }

            return pixelatedImage;
        }

        public static Bitmap PixelateNearestNeighbor(Bitmap inputImage, int maxWidth, int maxHeight)
        {
            float aspectRatio = (float)inputImage.Width / inputImage.Height;
            int targetWidth = Math.Min(inputImage.Width, maxWidth);
            int targetHeight = (int)(targetWidth / aspectRatio);

            if (targetHeight > maxHeight)
            {
                targetHeight = maxHeight;
                targetWidth = (int)(targetHeight * aspectRatio);
            }

            Bitmap pixelatedImage = new Bitmap(inputImage, new Size(targetWidth, targetHeight));

            using (Graphics graphics = Graphics.FromImage(pixelatedImage))
            {
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                graphics.DrawImage(inputImage, new Rectangle(0, 0, targetWidth, targetHeight));
            }

            return pixelatedImage;
        }



        public static Bitmap PixelateGaussianBlur(Bitmap inputImage, int maxWidth, int maxHeight, int blurRadius)
        {
            float aspectRatio = (float)inputImage.Width / inputImage.Height;
            int targetWidth = Math.Min(inputImage.Width, maxWidth);
            int targetHeight = (int)(targetWidth / aspectRatio);

            if (targetHeight > maxHeight)
            {
                targetHeight = maxHeight;
                targetWidth = (int)(targetHeight * aspectRatio);
            }

            Bitmap blurredImage = ApplyGaussianBlur(inputImage, blurRadius);
            Bitmap pixelatedImage = new Bitmap(blurredImage, new Size(targetWidth, targetHeight));

            return pixelatedImage;
        }

        private static Bitmap ApplyGaussianBlur(Bitmap image, int radius)
        {
            if (radius < 1)
                return (Bitmap)image.Clone();

            int size = radius * 2 + 1;
            int[,] kernel = new int[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    kernel[i, j] = 1;
                }
            }

            float factor = 1.0f / (size * size);
            return ConvolutionFilter(image, kernel, factor);
        }

        private static Bitmap ConvolutionFilter(Bitmap image, int[,] kernel, float factor)
        {
            int width = image.Width;
            int height = image.Height;

            Bitmap result = new Bitmap(width, height);

            int kernelSize = kernel.GetLength(0);
            int offset = kernelSize / 2;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float red = 0.0f, green = 0.0f, blue = 0.0f;

                    for (int i = 0; i < kernelSize; i++)
                    {
                        for (int j = 0; j < kernelSize; j++)
                        {
                            int pixelX = x + i - offset;
                            int pixelY = y + j - offset;

                            if (pixelX >= 0 && pixelX < width && pixelY >= 0 && pixelY < height)
                            {
                                Color pixelColor = image.GetPixel(pixelX, pixelY);
                                red += pixelColor.R * kernel[i, j];
                                green += pixelColor.G * kernel[i, j];
                                blue += pixelColor.B * kernel[i, j];
                            }
                        }
                    }

                    int newR = Math.Min(Math.Max((int)(factor * red), 0), 255);
                    int newG = Math.Min(Math.Max((int)(factor * green), 0), 255);
                    int newB = Math.Min(Math.Max((int)(factor * blue), 0), 255);

                    Color newColor = Color.FromArgb(newR, newG, newB);
                    result.SetPixel(x, y, newColor);
                }
            }

            return result;
        }

        public static Bitmap PixelateCross(Bitmap inputImage, int maxWidth, int maxHeight, int crossSize)
        {
            float aspectRatio = (float)inputImage.Width / inputImage.Height;
            int targetWidth = Math.Min(inputImage.Width, maxWidth);
            int targetHeight = (int)(targetWidth / aspectRatio);

            if (targetHeight > maxHeight)
            {
                targetHeight = maxHeight;
                targetWidth = (int)(targetHeight * aspectRatio);
            }

            Bitmap pixelatedImage = new Bitmap(targetWidth, targetHeight);


            using (Graphics graphics = Graphics.FromImage(pixelatedImage))
            {
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

                for (int x = 0; x < targetWidth; x += crossSize)
                {
                    for (int y = 0; y < targetHeight; y += crossSize)
                    {
                        Color averageColor = GetAverageColor(inputImage, x, y, crossSize, crossSize);
                        DrawCross(pixelatedImage, averageColor, x, y, crossSize);
                    }
                }
            }

            return pixelatedImage;
        }

        private static void DrawCross(Bitmap image, Color color, int x, int y, int crossSize)
        {
            using (Graphics graphics = Graphics.FromImage(image))
            {
                using (SolidBrush brush = new SolidBrush(color))
                {
                    int halfSize = crossSize / 2;
                    graphics.FillRectangle(brush, new Rectangle(x, y + halfSize / 2, crossSize, halfSize));
                    graphics.FillRectangle(brush, new Rectangle(x + halfSize / 2, y, halfSize, crossSize));
                }
            }
        }

        public static Bitmap PixelateTriangle(Bitmap inputImage, int maxWidth, int maxHeight, int triangleSize)
        {
            float aspectRatio = (float)inputImage.Width / inputImage.Height;
            int targetWidth = Math.Min(inputImage.Width, maxWidth);
            int targetHeight = (int)(targetWidth / aspectRatio);

            if (targetHeight > maxHeight)
            {
                targetHeight = maxHeight;
                targetWidth = (int)(targetHeight * aspectRatio);
            }

            Bitmap pixelatedImage = new Bitmap(targetWidth, targetHeight);


            using (Graphics graphics = Graphics.FromImage(pixelatedImage))
            {
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

                for (int x = 0; x < targetWidth; x += triangleSize)
                {
                    for (int y = 0; y < targetHeight; y += triangleSize)
                    {
                        Color averageColor = GetAverageColor(inputImage, x, y, triangleSize, triangleSize);
                        DrawTriangle(pixelatedImage, averageColor, x, y, triangleSize);
                    }
                }
            }

            return pixelatedImage;
        }

        private static void DrawTriangle(Bitmap image, Color color, int x, int y, int triangleSize)
        {
            using (Graphics graphics = Graphics.FromImage(image))
            {
                using (SolidBrush brush = new SolidBrush(color))
                {
                    Point[] points = new Point[]
                    {
                new Point(x, y),
                new Point(x + triangleSize, y),
                new Point(x + triangleSize, y + triangleSize)
                    };
                    graphics.FillPolygon(brush, points);
                }
            }
        }

        private static Color GetAverageColor(Bitmap image, int startX, int startY, int width, int height)
        {
            int r = 0, g = 0, b = 0;
            int totalPixels = 0;

            for (int x = startX; x < startX + width; x++)
            {
                for (int y = startY; y < startY + height; y++)
                {
                    if (x < image.Width && y < image.Height)
                    {
                        Color pixelColor = image.GetPixel(x, y);
                        r += pixelColor.R;
                        g += pixelColor.G;
                        b += pixelColor.B;
                        totalPixels++;
                    }
                }
            }

            r /= totalPixels;
            g /= totalPixels;
            b /= totalPixels;

            return Color.FromArgb(r, g, b);
        }






        public static Bitmap Resize(Bitmap inputBitmap, int scale)
        {
            int originalWidth = inputBitmap.Width;
            int originalHeight = inputBitmap.Height;
            int newWidth = originalWidth * scale;
            int newHeight = originalHeight * scale;

            Bitmap enlargedBitmap = new Bitmap(newWidth, newHeight);

            for (int x = 0; x < originalWidth; x++)
            {
                for (int y = 0; y < originalHeight; y++)
                {
                    Color pixelColor = inputBitmap.GetPixel(x, y);

                    for (int i = 0; i < scale; i++)
                    {
                        for (int j = 0; j < scale; j++)
                        {
                            int newX = x * scale + i;
                            int newY = y * scale + j;
                            enlargedBitmap.SetPixel(newX, newY, pixelColor);
                        }
                    }
                }
            }

            return enlargedBitmap;
        }
    }
}
