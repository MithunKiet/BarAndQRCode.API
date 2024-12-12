using SkiaSharp;
using ZXing.Common;
using ZXing;
using ZXing.SkiaSharp;

namespace BarAndQRCodeScanner.Controllers
{
    public class BarcodeReaderService
    {
        public string ReadBarcode(Stream imageStream)
        {
            using var skBitmap = SKBitmap.Decode(imageStream);
            if (skBitmap == null)
                return null;

            var barcodeReader = new BarcodeReader<SKBitmap>(bitmap =>
                new SKBitmapLuminanceSource(bitmap))
            {
                AutoRotate = true,
                Options = new DecodingOptions
                {
                    TryInverted = true
                }
            };

            var result = barcodeReader.Decode(skBitmap);
            return result?.Text;
        }
    }
}
