using System;
using System.Collections.Generic;
using System.Text;

namespace Albums.Model
{
    public class FotoModel
    {
        public int Id { get; set; }
        public String Nome { get; set; }
        public byte[] Arquivo { get; set; }
    }
}
