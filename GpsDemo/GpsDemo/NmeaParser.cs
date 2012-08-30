using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace zedIT_GpsDemo
{
    public class NmeaParser
    {
        StringBuilder _buffer;
        object _lock = new object();

        public NmeaParser()
        {
            _buffer = new StringBuilder();
        }

        public void AppendData(string data)
        {
            lock (_lock)
            {
                _buffer.Append(data);
            }
        }
    }
}
