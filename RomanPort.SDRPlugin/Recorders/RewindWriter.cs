using RomanPort.BetterSDRRecorder.Framework;
using RomanPort.BetterSDRRecorder.Framework.BinaryPart.Processors;
using SDRSharp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomanPort.BetterSDRRecorder.Recorders
{
    public class RewindWriter : AudioWriter
    {
        public byte[] rewindBuffer;
        public int bufferSeconds;
        private int bufferPosition;
        private bool full;

        public event RecorderUpdateArgs OnUpdateEvent;

        public RewindWriter(ISharpControl control, RecordingMode mode, WavSampleFormat format, int bufferSeconds) : base(control, mode, format)
        {
            this.bufferSeconds = bufferSeconds;
            Init();
        }

        private void Init()
        {
            rewindBuffer = new byte[bufferSeconds * samplesPerSecond * bytesPerSample * channels];
            full = false;
        }

        public void ResetBuffer()
        {
            Init();
            bufferPosition = 0;
            OnUpdateEvent?.Invoke();
        }

        public int GetUsableBytes()
        {
            if (full)
                return rewindBuffer.Length;
            return bufferPosition;
        }

        public float GetUsableSeconds()
        {
            return (float)GetUsableBytes() / samplesPerSecond / bytesPerSample / channels;
        }

        public float GetBufferFullness()
        {
            return GetUsableSeconds() / bufferSeconds;
        }

        public int GetBufferSizeBytes()
        {
            return rewindBuffer.Length;
        }

        public override void OnGetSamples(byte[] buffer, int count)
        {
            //Copy bytes, and overlap if needed
            for(int i = 0; i<buffer.Length; i+=1)
            {
                int index = (i + bufferPosition) % rewindBuffer.Length;
                rewindBuffer[index] = buffer[i];
            }
            if (buffer.Length + bufferPosition >= rewindBuffer.Length)
                full = true;
            bufferPosition = (buffer.Length + bufferPosition) % rewindBuffer.Length;

            //Send events
            OnUpdateEvent?.Invoke();
        }

        public int ReadBuffer(byte[] output, int offset)
        {
            //Get the starting position
            int usable = GetUsableBytes();
            int start = bufferPosition - usable;
            if (start < 0)
                start = bufferPosition; //This will only ever happen if the whole buffer is used. When the full buffer is used, we can safely use the whole buffer, and wrap around

            //Ensure we will fit
            if (output.Length - offset > usable)
                throw new Exception("Output buffer is not large enough to hold all samples.");

            //Copy
            for(int i = 0;i<usable; i++)
            {
                int index = (i + start) % rewindBuffer.Length;
                output[i] = rewindBuffer[index];
            }

            return usable;
        }

        public void ReadBufferIntoWav(WavEncoder wav)
        {
            //Get usable buffer and read bytes
            byte[] buffer = new byte[GetUsableBytes()];
            ReadBuffer(buffer, 0);

            //Write
            wav.Write(buffer, 0, buffer.Length);
        }
    }
}
