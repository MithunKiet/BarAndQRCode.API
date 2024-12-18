﻿using Microsoft.AspNetCore.Mvc;

namespace BarAndQRCodeScanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScannerController(BarcodeReaderService barcodeReaderService, QRCodeReaderService qrCodeReaderService, GenrateBarQRCode genrateBarQRCode) : ControllerBase
    {
        private readonly BarcodeReaderService _barcodeReaderService = barcodeReaderService;
        private readonly QRCodeReaderService _qrCodeReaderService = qrCodeReaderService;
        private readonly GenrateBarQRCode _genrateBarQRCode = genrateBarQRCode;

        [HttpPost("scan-barcode")]
        public IActionResult ScanBarcode(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            using var stream = file.OpenReadStream();
            var result = _barcodeReaderService.ReadBarcode(stream);

            if (string.IsNullOrEmpty(result))
                return NotFound("No barcode detected.");

            return Ok(new { BarcodeText = result });
        }

        [HttpPost("scan-qrcode")]
        public IActionResult ScanQRcode(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            using var stream = file.OpenReadStream();
            var result = _qrCodeReaderService.ReadQRCode(stream);

            if (string.IsNullOrEmpty(result))
                return NotFound("No barcode detected.");

            return Ok(new { BarcodeText = result });
        }

        [HttpGet("generate-qrcode")]
        public IActionResult GenerateQRCode([FromQuery] string text)
        {
            if (string.IsNullOrEmpty(text))
                return BadRequest("Text parameter is required.");

            var qrCodeImage = _genrateBarQRCode.GenerateQRCode(text);
            return File(qrCodeImage, "image/png");
        }

        [HttpGet("generate-qrcode-using-zxing")]
        public IActionResult GenerateQRCodeUsingZXing([FromQuery] string text)
        {
            if (string.IsNullOrEmpty(text))
                return BadRequest("Text parameter is required.");

            var qrCodeImage = _genrateBarQRCode.GenerateQRCodeUsingZXing(text);
            return File(qrCodeImage, "image/png");
        }

        [HttpGet("generate-barcode")]
        public IActionResult GenerateBarCode([FromQuery] string text)
        {
            if (string.IsNullOrEmpty(text))
                return BadRequest("Text parameter is required.");

            var qrCodeImage = _genrateBarQRCode.GenerateBarcode(text);
            return File(qrCodeImage, "image/png");
        }
    }
}
