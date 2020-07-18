using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FollowFocusInterface.Models
{
    public class SerialStatusModel
    {
        public bool USBSerialStatus { get; set; }
        public bool BTSerialStatus { get; set; }
        public string ComType { get; set; }
    }
}
