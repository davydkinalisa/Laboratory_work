using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_linq_laba
{
    class ClassEstimate
    {
        public int code_stud { get; set; }
        public int code_subject { get; set; }
        public int code_lector { get; set; }
        public System.DateTime date_exam { get; set; }
        public Nullable<int> estimate { get; set; }
        public int idpro { get; set; }
    }
}
