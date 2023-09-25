using System.Drawing;

namespace ImageEditor
{
    public static class Globals
    {

    
    public static async Task<byte[]> ToByteArray(Stream instream)
    {
        if (instream is MemoryStream)
            return ((MemoryStream)instream).ToArray();

        using (var memoryStream = new MemoryStream())
        {
            await instream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
    public async static Task<byte[]> MakeTransparent(Bitmap bitmap, Color color, int tolerance)
        {
            Bitmap transparentImage = new Bitmap(bitmap);

            for (int i = transparentImage.Size.Width - 1; i >= 0; i--)
            {
                for (int j = transparentImage.Size.Height - 1; j >= 0; j--)
                {
                    var currentColor = transparentImage.GetPixel(i, j);
                    if (Math.Abs(color.R - currentColor.R) < tolerance &&
                         Math.Abs(color.G - currentColor.G) < tolerance &&
                         Math.Abs(color.B - currentColor.B) < tolerance)
                        transparentImage.SetPixel(i, j, color);
                }
            }
            transparentImage.MakeTransparent(color);

            var imgStream = ToMemoryStream(transparentImage);


            return await ToByteArray(imgStream);
        }

        private static MemoryStream ToMemoryStream(Bitmap b)
        {
            MemoryStream ms = new MemoryStream();
            b.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms;
        }

    }
}
