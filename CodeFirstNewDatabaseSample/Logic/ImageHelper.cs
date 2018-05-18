using System.Drawing;
using System.IO;

namespace Data.Logic
{
    public static class ImageHelper
    {
        public static double[,] ImageByteArray2pixelArray(byte[] imageByteArray)
        {
            double[,] result;

            using (var ms = new MemoryStream(imageByteArray))
            {
                Bitmap trainPhotoImage = new Bitmap(System.Drawing.Image.FromStream(ms));
                result = new double[trainPhotoImage.Height, trainPhotoImage.Width];
                for (int x = 0; x < trainPhotoImage.Width; x++)
                {
                    for (int y = 0; y < trainPhotoImage.Height; y++)
                    {
                        Color pixelColor = trainPhotoImage.GetPixel(x, y);
                        result[y, x] = pixelColor.R;
                    }
                }
            }

            return result;
        }
    }
}
