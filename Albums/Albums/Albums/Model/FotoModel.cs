using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace Albums.Model
{
    public class FotoModel
    {
        public int Id { get; set; }
        public String Nome { get; set; }
        public byte[] Arquivo { get; set; }
        public ImageSource ArquivoSource { get {
                //String base64Encode = "";
                //base64Encode = Convert.ToBase64String(Arquivo);
                ImageSource imageaData = ImageSource.FromStream(() => new MemoryStream(Arquivo));
                return imageaData;
            } 
        }
    }
}
