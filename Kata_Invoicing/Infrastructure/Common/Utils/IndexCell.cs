using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kata_Invoicing.Infrastructure.Common.Utils
{
    public class IndexCell
    {
        private int _row;
        private short _col;
        public int Row
        {
            get
            {
                return _row;
            }
            set
            {
                _row = value;
                MaxRow = (MaxRow < value ? value : MaxRow);
            }
        }
        public short Col
        {
            get
            {
                return _col;
            }
            set
            {
                _col = value;
                MaxCol = (MaxCol < value ? value : MaxCol);
            }
        }
        public static int MaxRow
        {
            get;
            set;
        }
        public static short MaxCol
        {
            get;
            set;
        }

    }
}
