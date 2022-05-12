using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Common.Helper;

public class ImageHelper
{
    public static BitmapImage? CreateBitmapImage(string imgFullPath, int decodePixelWidth = 300)
    {
        string imageFileName = Path.GetFileName(imgFullPath);
        if (File.Exists($"{PathHelper.GetLocalImagesDirectory()}{imageFileName}") == false)
            return null;

        BitmapImage bitmapImg = new BitmapImage();
        bitmapImg.BeginInit();
        bitmapImg.CacheOption = BitmapCacheOption.OnDemand;
        bitmapImg.CreateOptions = BitmapCreateOptions.DelayCreation;
        bitmapImg.DecodePixelWidth = decodePixelWidth;
        bitmapImg.UriSource = new Uri($"{PathHelper.GetLocalImagesDirectory()}{imageFileName}");
        bitmapImg.EndInit();

        return bitmapImg;
    }

    public static BitmapImage CreateBitmapImageByRecorce(string fileName, int decodePixelWidth = 300)
    {
        BitmapImage bitmapImg = new BitmapImage();
        bitmapImg.BeginInit();
        bitmapImg.CacheOption = BitmapCacheOption.OnDemand;
        bitmapImg.CreateOptions = BitmapCreateOptions.DelayCreation;
        bitmapImg.DecodePixelWidth = decodePixelWidth;
        bitmapImg.UriSource = new Uri($"/Images/{fileName}", UriKind.Relative);
        bitmapImg.EndInit();

        return bitmapImg;
    }

    public static bool SetImageForImage(Image imageControl, string imgFullPath, bool isLocalDownloadFile = true, int decodePixelWidth = 300)
    {
        BitmapImage bitmapImg = null;
        if (isLocalDownloadFile)
        {
            bitmapImg = CreateBitmapImage(imgFullPath, decodePixelWidth);
        }
        else
        {
            bitmapImg = new BitmapImage();
            bitmapImg.BeginInit();
            bitmapImg.CacheOption = BitmapCacheOption.OnDemand;
            bitmapImg.CreateOptions = BitmapCreateOptions.DelayCreation;
            bitmapImg.DecodePixelWidth = decodePixelWidth;
            bitmapImg.UriSource = new Uri(imgFullPath, UriKind.RelativeOrAbsolute);
            bitmapImg.EndInit();
        }
        if (bitmapImg == null)
        {
            if (string.IsNullOrWhiteSpace(imgFullPath))
                return false;

            imageControl.Source = new BitmapImage(new Uri(imgFullPath));
            return true;
        }
        else
        {
            imageControl.Source = bitmapImg;
            return true;
        }
    }

    public static BitmapImage? Base64ToBitmapImage(string base64Img)
    {
        if (string.IsNullOrWhiteSpace(base64Img))
            return null;

        byte[] binaryData = Convert.FromBase64String(base64Img);

        BitmapImage bitmapImg = new BitmapImage();
        bitmapImg.BeginInit();
        bitmapImg.StreamSource = new MemoryStream(binaryData);
        bitmapImg.EndInit();

        return bitmapImg;
    }
}
