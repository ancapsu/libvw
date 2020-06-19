using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibVisWeb.Models
{

    public class LinkModel
    {

        public LinkModel()
        {
                        
            Link = "";
            Lang = 0;

        }

        public string Link { get; set; }
        public int Lang { get; set; }
                
    }

    public class LinkResultModel
    {
        
        public int Result { get; set; }
        public string ResultComplement { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }
        public string Link { get; set; }
        public List<string> Image { get; set; }

        public LinkResultModel()
        {
            
            Result = 0;
            ResultComplement = "";

            Link = "";
            Title = "";
            Text = "";
            Image = new List<string>();

        }

    }

}
