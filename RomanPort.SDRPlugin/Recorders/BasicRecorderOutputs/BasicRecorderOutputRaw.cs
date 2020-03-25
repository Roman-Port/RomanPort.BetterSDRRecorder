using RomanPort.BetterSDRRecorder.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RomanPort.BetterSDRRecorder.Recorders.BasicRecorderOutputs
{
    public class BasicRecorderOutputRaw : IBasicRecorderOutput
    {
        public FileStream fs;
        public string path;
        
        public override void Close()
        {
            //Close
            fs.Close();

            //Prompt
            SaveFileDialog fd = new SaveFileDialog();
            fd.Title = "Save Output";
            fd.Filter = "Binary Files (*.bin)|*.bin";

            //Write
            if(fd.ShowDialog() == DialogResult.OK)
            {
                File.Move(path, fd.FileName);
            } else
            {
                File.Delete(path);
            }
        }

        public override long GetBytesOnDisk()
        {
            return fs.Length;
        }

        public override void Open(AudioWriter audio)
        {
            path = GetNextTempOutputFile();
            fs = new FileStream(path, FileMode.Create);
        }

        public override void Write(byte[] data)
        {
            fs.Write(data, 0, data.Length);
        }
    }
}
