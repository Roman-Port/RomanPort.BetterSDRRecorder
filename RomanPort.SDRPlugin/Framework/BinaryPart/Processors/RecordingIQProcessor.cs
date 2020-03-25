using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDRSharp.Radio;

namespace RomanPort.BetterSDRRecorder.Framework.BinaryPart.Processors
{
    public class RecordingIQProcessor : IIQProcessor, IStreamProcessor, IBaseProcessor
    {
        private volatile bool _enabled;
        private double _sampleRate;

        public event RecordingIQProcessor.IQReadyDelegate IQReady;

        public bool Enabled
        {
            get
            {
                return this._enabled;
            }
            set
            {
                this._enabled = value;
            }
        }

        public double SampleRate
        {
            set
            {
                this._sampleRate = value;
            }
            get
            {
                return this._sampleRate;
            }
        }

        public unsafe void Process(Complex* buffer, int length)
        {
            if (this.IQReady == null)
                return;
            this.IQReady(buffer, length);
        }

        public unsafe delegate void IQReadyDelegate(Complex* buffer, int length);
    }
}
