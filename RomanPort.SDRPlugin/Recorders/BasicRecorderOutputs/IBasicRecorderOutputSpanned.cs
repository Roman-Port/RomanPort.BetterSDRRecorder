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
    public abstract class IBasicRecorderOutputSpanned : IBasicRecorderOutput
    {
        private List<string> tempPaths;
        private Stream activeStream;

        public AudioWriter audio;
        
        public override void Close()
        {
            //Close active file
            CloseActiveFile();
            
            //Prompt for output path
            SaveFileDialog fd = new SaveFileDialog();
            fd.Title = "Save Output";
            fd.Filter = $"Output Files (*.{GetFileExtension()})|*.{GetFileExtension()}";
            
            //Open
            var result = fd.ShowDialog();
            if(result == DialogResult.OK)
            {
                //Get base path
                string basePath = fd.FileName.Substring(0, fd.FileName.Length - GetFileExtension().Length - 1);

                //If there is only one part, don't rename it
                if(tempPaths.Count == 1)
                {
                    File.Move(tempPaths[0], basePath + "." + GetFileExtension());
                } else
                {
                    for(int i = 0; i<tempPaths.Count; i+=1)
                    {
                        File.Move(tempPaths[i], basePath + "_PT" + (i+1).ToString() + "." + GetFileExtension());
                    }
                }
            } else
            {
                //Delete all temporary files
                for (int i = 0; i < tempPaths.Count; i += 1)
                {
                    File.Delete(tempPaths[i]);
                }
            }
        }

        public override long GetBytesOnDisk()
        {
            return ((tempPaths.Count - 1) * GetMaxFileSize()) + activeStream.Length;
        }

        private void InternalOpenNewActiveFile()
        {
            string path = GetNextTempOutputFile();
            tempPaths.Add(path);
            activeStream = OpenActiveFile(path);
        }

        public override void Open(AudioWriter audio)
        {
            //Set
            this.audio = audio;
            tempPaths = new List<string>();
            
            //Open first file
            InternalOpenNewActiveFile();
        }

        public override void Write(byte[] data)
        {
            //Check if our length would go over the max
            if(GetCurrentFileSize() + data.Length > GetMaxFileSize())
            {
                //We'll need to create a new file
                CloseActiveFile();
                InternalOpenNewActiveFile();
            }

            //Write to file
            activeStream.Write(data, 0, data.Length);
        }

        public abstract string GetFileExtension();
        public abstract long GetMaxFileSize();
        public abstract long GetCurrentFileSize();
        public abstract Stream OpenActiveFile(string path);
        public abstract void CloseActiveFile();
    }
}
