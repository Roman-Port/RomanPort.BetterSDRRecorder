using RomanPort.BetterSDRRecorder.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomanPort.BetterSDRRecorder.Recorders.BasicRecorderOutputs
{
    public abstract class IBasicRecorderOutput
    {
        public abstract void Open(AudioWriter audio);
        
        public abstract void Write(byte[] data);

        public abstract void Close();

        public abstract long GetBytesOnDisk();
        
        public string GetNextTempOutputFile()
        {
            int i = 0;
            while (File.Exists("OUTPUT_TEMP_" + i))
                i++;
            return "OUTPUT_TEMP_" + i;
        }
    }
}
