using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Data
{
    public class GenerateTimeData
    {
       
        public int id { get; set; }//关卡
        public int wave { get; set; }//波数、
        public int time { get; set; }//波间隔
        public float rate { get; set; }//刷怪间隔
        public int gold { get; set; }
        public int count { get; set; }//个数
      
       
    }
}
