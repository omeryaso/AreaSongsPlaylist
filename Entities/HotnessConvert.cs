using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayList.Entities
{
    class HotnessConvert
    {
        /// <summary>
        /// converts from float number to stars as strings.
        /// </summary>
        /// <param name="hot"></param>
        public string convert(float hot)
        {
            if (hot == -1)
            {
                return "No Information";
            }
            else if (hot >= 0 && hot < 0.2)
            {
                return "*";
            }
            else if (hot > 0.2 && hot < 0.4)
            {
                return "**";
            }
            else if (hot > 0.4 && hot < 0.6)
            {
                return "***";
            }
            else if (hot > 0.6 && hot < 0.8)
            {
                return "****";
            }
            else
            {
                return "*****";
            }
        }
    }
}
