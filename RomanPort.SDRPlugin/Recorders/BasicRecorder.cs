using RomanPort.BetterSDRRecorder.Framework;
using RomanPort.BetterSDRRecorder.Recorders.BasicRecorderOutputs;
using SDRSharp.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomanPort.BetterSDRRecorder.Recorders
{
    public class BasicRecorder : AudioWriter
    {
        public BasicRecorderOutput outputType;
        private IBasicRecorderOutput output;
        private long bytesWritten;

        public event RecorderUpdateArgs OnUpdateEvent;

        public BasicRecorder(ISharpControl control, RecordingMode mode, WavSampleFormat format, BasicRecorderOutput outputType) : base(control, mode, format)
        {
            //Set
            this.outputType = outputType;

            //Create output
            CreateNewOutput();

            //Create
            output.Open(this);
        }

        private void CreateNewOutput()
        {
            switch (outputType)
            {
                case BasicRecorderOutput.Raw:
                    output = new BasicRecorderOutputRaw();
                    break;
                case BasicRecorderOutput.Wav:
                    output = new BasicRecorderOutputWav();
                    break;
                case BasicRecorderOutput.Mp3:
                    output = new BasicRecorderOutputMp3();
                    break;
                default:
                    throw new Exception("Unsupported output type!");
            }
        }

        public override void OnGetSamples(byte[] buffer, int count)
        {
            output.Write(buffer);
            bytesWritten += buffer.Length;
            OnUpdateEvent?.Invoke();
        }

        public void Save()
        {
            //Save
            output.Close();

            //Create new
            CreateNewOutput();
        }

        public double GetRecordedSeconds()
        {
            double bytes = bytesWritten;
            return bytes / samplesPerSecond / bytesPerSample / channels;
        }

        public long GetBytesOnDisk()
        {
            return output.GetBytesOnDisk();
        }
    }

    public enum BasicRecorderOutput
    {
        Raw = 0,
        Wav = 1,
        Mp3 = 2
    }
}
