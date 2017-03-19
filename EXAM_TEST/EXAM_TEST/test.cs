using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXAM_TEST
{
   public class test
    {
          
       public string Qwet
       {
           get;
           set;
       }
       public List<string> Answers  { set; get; }
       public test(){}
       public test(string qwet, List<string> ans)
       {
           this.Qwet = qwet;
           this.Answers= ans;

       }
       
    }
}
