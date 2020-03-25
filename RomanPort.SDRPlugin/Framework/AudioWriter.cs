using RomanPort.BetterSDRRecorder.Framework.BinaryPart;
using RomanPort.BetterSDRRecorder.Framework.BinaryPart.Processors;
using SDRSharp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RomanPort.BetterSDRRecorder.Framework
{
    public abstract class AudioWriter : BinaryDataReceiver
    {
        public uint samplesPerSecond { get { return (uint)_sampleRate; } }
        public ushort channels;
        public ushort formatTag;
        public ushort bitsPerSample;
        public ushort bytesPerSample;
        
        public AudioWriter(ISharpControl control, RecordingMode mode, WavSampleFormat format) : base(control, mode, format)
        {
            channels = 2;
            Init();
        }

        private void Init()
        {
            switch (_wavSampleFormat)
            {
                case WavSampleFormat.PCM8:
                    formatTag = (ushort)1;
                    bitsPerSample = (ushort)8;
                    break;
                case WavSampleFormat.PCM16:
                    formatTag = (ushort)1;
                    bitsPerSample = (ushort)16;
                    break;
                case WavSampleFormat.Float32:
                    formatTag = (ushort)3;
                    bitsPerSample = (ushort)32;
                    break;
            }
            bytesPerSample = (ushort)(bitsPerSample / 8);
        }
    }
}
