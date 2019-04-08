using System;
using System.Collections.Generic;
using System.Text;

namespace IDO_Client.Controls
{

    public interface IMessage
    {
        void LongAlert(string message);
        void ShortAlert(string message);
    }

}
