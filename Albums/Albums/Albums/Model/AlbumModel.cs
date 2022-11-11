using System;
using System.Collections.Generic;
using System.Text;

namespace Albums.Model
{
    public class AlbumModel
    {
        public int Id { get; set; }
        public String Nome { get; set; }
        public List<FotoModel> Fotos { get; set; }
    }
}
