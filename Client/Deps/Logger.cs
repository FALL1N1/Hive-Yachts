using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Deps
{
    public class Logger
    {
        public Logger() { }
        public void outInfo(string message)
        {
            Debug.WriteLine($"{DateTime.Now.ToLongTimeString()} [INFO] Hive-Yachts: {message}");
        }

        public void outError(Exception exception)
        {
            Debug.WriteLine($"{DateTime.Now.ToLongTimeString()} [ERROR] Hive-Yachts: {exception.Message}\n{exception.StackTrace}");
        }
        public void outDebug(string message)
        {
            Debug.WriteLine($"{DateTime.Now.ToLongTimeString()} [DEBUG] Hive-Yachts: {message}");
        }
    }
}
