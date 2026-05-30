using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Poliklininka.Helpers;

public static class PhotoCoverter
{
    public static ImageSource? ConvertToImageSourse(byte[]? photo)
    {
        if (photo != null)
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = new MemoryStream(photo);
            image.EndInit();
            return image;
        }
        return null;
    }
    public static byte[]? ConvertToByteArray(ImageSource? photo)
    {
        if (photo == null) return null;

        var encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create((BitmapSource)photo));
        using var stream = new MemoryStream();
        encoder.Save(stream);
        return stream.ToArray();
    }
}
