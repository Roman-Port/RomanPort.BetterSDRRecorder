using RomanPort.BetterSDRRecorder.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomanPort.BetterSDRRecorder.Recorders.BasicRecorderOutputs
{
    public class BasicRecorderOutputWav : IBasicRecorderOutputSpanned
    {
        public WavEncoder wavStream;
        public FileStream fileStream;

        public override long GetCurrentFileSize()
        {
            return fileStream.Length;
        }

        public override string GetFileExtension()
        {
            return "wav";
        }

        public override long GetMaxFileSize()
        {
            return int.MaxValue;
        }

        public override Stream OpenActiveFile(string path)
        {
            fileStream = new FileStream(path, FileMode.Create);
            wavStream = new WavEncoder(fileStream, audio);
            return wavStream;
        }

        public override void CloseActiveFile()
        {
            wavStream.Flush();
            wavStream.Close();
            fileStream.Flush();
            fileStream.Close();
        }
    }
}
