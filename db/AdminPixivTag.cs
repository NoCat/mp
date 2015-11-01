using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mp.DAL
{
    public class AdminPixivTag
    {
        public int ID { get; set; }
        public string PText { get; set; }
        public string MText { get; set; }
        public bool IsSkip { get; set; }
        public int CitationCount { get; set; }
    }
}
