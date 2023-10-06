using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arbook_tools.Models
{
    class FileXlsxModel
    {
        public int Id { get; set; }
        public bool IsSelected { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public string Count { get; set; }
    }
}
