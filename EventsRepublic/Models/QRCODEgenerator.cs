using QRCoder;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace EventsRepublic.Models
{
    public class QRCODEgenerator
    {
        public string sQRCODEgenerator()
        {
           
            
            //create raw qr code
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qRCodeData = qrGenerator.CreateQrCode("what", QRCodeGenerator.ECCLevel.M);
            //create byte/raw bitmap qr code
            BitmapByteQRCode qrCodeBmp = new BitmapByteQRCode(qRCodeData);
            byte[] qrCodeImageBmp = qrCodeBmp.GetGraphic(4);
            var h = Convert.ToBase64String(qrCodeImageBmp);
            return h;
        }
        
    }
}
