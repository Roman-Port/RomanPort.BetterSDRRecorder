using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDRSharp.Radio;
using System.Threading;
using RomanPort.BetterSDRRecorder.Framework.BinaryPart.Processors;
using System.Runtime.InteropServices;
using SDRSharp.Common;

namespace RomanPort.BetterSDRRecorder.Framework.BinaryPart
{
    /// <summary>
    /// Actually gets the binary audio data, the sends it out in a reasonable way
    /// </summary>
    public abstract class BinaryDataReceiver : IDisposable
    {
        private static readonly int _bufferCount = Utils.GetIntSetting("RecordingBufferCount", 8);
        private readonly float _audioGain = (float)Math.Pow(3.0, 10.0);
        private readonly SharpEvent _bufferEvent = new SharpEvent(false);
        private readonly UnsafeBuffer[] _circularBuffers = new UnsafeBuffer[BinaryDataReceiver._bufferCount];
        private readonly unsafe Complex*[] _complexCircularBufferPtrs = new Complex*[BinaryDataReceiver._bufferCount];
        private readonly unsafe float*[] _floatCircularBufferPtrs = new float*[BinaryDataReceiver._bufferCount];
        private const int DefaultAudioGain = 30;
        private int _circularBufferTail;
        private int _circularBufferHead;
        private int _circularBufferLength;
        private volatile int _circularBufferUsedCount;
        private long _skippedBuffersCount;
        private bool _diskWriterRunning;
        private bool _unityGain;
        private string _fileName;
        public double _sampleRate;
        private double _frequencyOffset;
        public WavSampleFormat _wavSampleFormat;
        private Thread _diskWriter;
        private FrequencyTranslator _iqTranslator;
        private readonly RecordingMode _recordingMode;
        private readonly RecordingIQProcessor _iqProcessor;
        private readonly RecordingAudioProcessor _audioProcessor;
        internal ISharpControl control;

        public bool IsRecording
        {
            get
            {
                return this._diskWriterRunning;
            }
        }

        public bool IsStreamFull { get; }

        public long BytesWritten { get; }

        public long SkippedBuffers { get; }

        public RecordingMode Mode
        {
            get
            {
                return this._recordingMode;
            }
        }

        public WavSampleFormat Format
        {
            get
            {
                return this._wavSampleFormat;
            }
            set
            {
                if (this._diskWriterRunning)
                    throw new ArgumentException("Format cannot be set while recording");
                this._wavSampleFormat = value;
            }
        }

        public double SampleRate
        {
            get
            {
                return this._sampleRate;
            }
            set
            {
                if (this._diskWriterRunning)
                    throw new ArgumentException("SampleRate cannot be set while recording");
                this._sampleRate = value;
            }
        }

        public int FrequencyOffset
        {
            get
            {
                return (int)this._frequencyOffset;
            }
            set
            {
                if (this._diskWriterRunning)
                    throw new ArgumentException("SampleRate cannot be set while recording");
                this._frequencyOffset = (double)value;
            }
        }

        public string FileName
        {
            get
            {
                return this._fileName;
            }
            set
            {
                if (this._diskWriterRunning)
                    throw new ArgumentException("FileName cannot be set while recording");
                this._fileName = value;
            }
        }

        public bool UnityGain
        {
            get
            {
                return this._unityGain;
            }
            set
            {
                this._unityGain = value;
            }
        }

        public BinaryDataReceiver(ISharpControl control, RecordingMode mode, WavSampleFormat format)
        {
            this.control = control;
            if (mode == RecordingMode.Audio)
            {
                _audioProcessor = new RecordingAudioProcessor();
                _audioProcessor.Enabled = true;
                control.RegisterStreamHook((object)this._audioProcessor, ProcessorType.FilteredAudioOutput);
                SampleRate = (double)control.AudioSampleRate;
                UnityGain = false;
            }
            else
            {
                _iqProcessor = new RecordingIQProcessor();
                _iqProcessor.Enabled = true;
                control.RegisterStreamHook((object)this._iqProcessor, ProcessorType.RawIQ);
                SampleRate = control.RFBandwidth;
                FrequencyOffset = control.IFOffset;
            }
            this._recordingMode = mode;
            this._wavSampleFormat = format;
        }

        public void Dispose()
        {
            this.FreeBuffers();
            if(_audioProcessor != null)
                control.UnregisterStreamHook(_audioProcessor);
            if (_iqProcessor != null)
                control.UnregisterStreamHook(_iqProcessor);
        }

        public unsafe void IQSamplesIn(Complex* buffer, int length)
        {
            if (this._circularBufferLength != length)
            {
                this.FreeBuffers();
                this.CreateBuffers(length);
                this._circularBufferTail = 0;
                this._circularBufferHead = 0;
            }
            if (this._circularBufferUsedCount == BinaryDataReceiver._bufferCount)
            {
                ++this._skippedBuffersCount;
            }
            else
            {
                Complex* circularBufferPtr = this._complexCircularBufferPtrs[this._circularBufferHead];
                Utils.Memcpy((void*)circularBufferPtr, (void*)buffer, length * sizeof(Complex));
                if (this._iqTranslator == null || this._iqTranslator.SampleRate != this._sampleRate || this._iqTranslator.Frequency != this._frequencyOffset)
                {
                    this._iqTranslator = new FrequencyTranslator(this._sampleRate);
                    this._iqTranslator.Frequency = this._frequencyOffset;
                }
                this._iqTranslator.Process(circularBufferPtr, length);
                ++this._circularBufferHead;
                this._circularBufferHead &= BinaryDataReceiver._bufferCount - 1;
                ++this._circularBufferUsedCount;
                this._bufferEvent.Set();
            }
        }

        public unsafe void AudioSamplesIn(float* audio, int length)
        {
            int size = length / 2;
            if (this._circularBufferLength != size)
            {
                this.FreeBuffers();
                this.CreateBuffers(size);
                this._circularBufferTail = 0;
                this._circularBufferHead = 0;
            }
            if (this._circularBufferUsedCount == BinaryDataReceiver._bufferCount)
            {
                ++this._skippedBuffersCount;
            }
            else
            {
                Utils.Memcpy((void*)this._floatCircularBufferPtrs[this._circularBufferHead], (void*)audio, length * 4);
                ++this._circularBufferHead;
                this._circularBufferHead &= BinaryDataReceiver._bufferCount - 1;
                ++this._circularBufferUsedCount;
                this._bufferEvent.Set();
            }
        }

        public unsafe void ScaleAudio(float* audio, int length)
        {
            if (this._unityGain)
                return;
            for (int index = 0; index < length; ++index)
            {
                float* numPtr = audio + index;
                *numPtr = *numPtr * this._audioGain;
            }
        }

        private unsafe void DiskWriterThread()
        {
            if (this._recordingMode == RecordingMode.Baseband)
            {
                this._iqProcessor.IQReady += new RecordingIQProcessor.IQReadyDelegate(this.IQSamplesIn);
                this._iqProcessor.Enabled = true;
            }
            else
            {
                this._audioProcessor.AudioReady += new RecordingAudioProcessor.AudioReadyDelegate(this.AudioSamplesIn);
                this._audioProcessor.Enabled = true;
            }
            while (this._diskWriterRunning && !this.IsStreamFull)
            {
                if (this._circularBufferTail == this._circularBufferHead)
                    this._bufferEvent.WaitOne();
                if (this._diskWriterRunning && this._circularBufferTail != this._circularBufferHead)
                {
                    if (this._recordingMode == RecordingMode.Audio)
                        this.ScaleAudio(this._floatCircularBufferPtrs[this._circularBufferTail], this._circularBuffers[this._circularBufferTail].Length * 2);
                    OnWriteUnsafeBinary(this._floatCircularBufferPtrs[this._circularBufferTail], this._circularBuffers[this._circularBufferTail].Length);
                    --this._circularBufferUsedCount;
                    ++this._circularBufferTail;
                    this._circularBufferTail &= BinaryDataReceiver._bufferCount - 1;
                }
            }
            for (; this._circularBufferTail != this._circularBufferHead; this._circularBufferTail &= BinaryDataReceiver._bufferCount - 1)
            {
                if ((IntPtr)this._floatCircularBufferPtrs[this._circularBufferTail] != IntPtr.Zero)
                {
                    if (this._recordingMode == RecordingMode.Audio)
                        this.ScaleAudio(this._floatCircularBufferPtrs[this._circularBufferTail], this._circularBuffers[this._circularBufferTail].Length * 2);
                    OnWriteUnsafeBinary(this._floatCircularBufferPtrs[this._circularBufferTail], this._circularBuffers[this._circularBufferTail].Length);
                }
                ++this._circularBufferTail;
            }
            if (this._recordingMode == RecordingMode.Baseband)
            {
                this._iqProcessor.Enabled = false;
                this._iqProcessor.IQReady -= new RecordingIQProcessor.IQReadyDelegate(this.IQSamplesIn);
            }
            else
            {
                this._audioProcessor.Enabled = false;
                this._audioProcessor.AudioReady -= new RecordingAudioProcessor.AudioReadyDelegate(this.AudioSamplesIn);
            }
            this._diskWriterRunning = false;
        }

        private void Flush()
        {
            //TODO: Close
        }

        private unsafe void CreateBuffers(int size)
        {
            for (int index = 0; index < BinaryDataReceiver._bufferCount; ++index)
            {
                this._circularBuffers[index] = UnsafeBuffer.Create(size, sizeof(Complex));
                this._complexCircularBufferPtrs[index] = (Complex*)(void*)this._circularBuffers[index];
                this._floatCircularBufferPtrs[index] = (float*)(void*)this._circularBuffers[index];
            }
            this._circularBufferLength = size;
        }

        private unsafe void FreeBuffers()
        {
            this._circularBufferLength = 0;
            for (int index = 0; index < BinaryDataReceiver._bufferCount; ++index)
            {
                if (this._circularBuffers[index] != null)
                {
                    this._circularBuffers[index].Dispose();
                    this._circularBuffers[index] = (UnsafeBuffer)null;
                    this._complexCircularBufferPtrs[index] = (Complex*)null;
                    this._floatCircularBufferPtrs[index] = (float*)null;
                }
            }
        }

        public void StartRecording()
        {
            if (this._diskWriter != null)
                return;
            this._circularBufferHead = 0;
            this._circularBufferTail = 0;
            this._skippedBuffersCount = 0L;
            this._bufferEvent.Reset();
            this._diskWriter = new Thread(new ThreadStart(this.DiskWriterThread));
            this._diskWriterRunning = true;
            this._diskWriter.Start();
        }

        public void StopRecording()
        {
            this._diskWriterRunning = false;
            if (this._diskWriter != null)
            {
                this._bufferEvent.Set();
                this._diskWriter.Join();
            }
            this.Flush();
            this.FreeBuffers();
            this._diskWriter = (Thread)null;
        }

        //BINARY BIT
        public unsafe void OnWriteUnsafeBinary(float* data, int length)
        {
            //Get samples in their binary form
            byte[] buffer;
            switch (this._wavSampleFormat)
            {
                case WavSampleFormat.PCM8:
                    buffer = this.GetPCM8(data, length);
                    break;
                case WavSampleFormat.PCM16:
                    buffer = this.GetPCM16(data, length);
                    break;
                case WavSampleFormat.Float32:
                    buffer = this.GetFloat(data, length);
                    break;
                default:
                    return;
            }
            OnGetSamples(buffer, length);
        }

        public abstract void OnGetSamples(byte[] buffer, int count);

        private unsafe byte[] GetPCM8(float* data, int length)
        {
            byte[] outputBuffer = new byte[length * 2];
            float* numPtr1 = data;
            for (int index1 = 0; index1 < length; ++index1)
            {
                byte[] outputBuffer1 = outputBuffer;
                int index2 = index1 * 2;
                float* numPtr2 = numPtr1;
                float* numPtr3 = (float*)((IntPtr)numPtr2 + 4);
                int num1 = (int)(byte)((double)*numPtr2 * (double)sbyte.MaxValue + 128.0);
                outputBuffer1[index2] = (byte)num1;
                byte[] outputBuffer2 = outputBuffer;
                int index3 = index1 * 2 + 1;
                float* numPtr4 = numPtr3;
                numPtr1 = (float*)((IntPtr)numPtr4 + 4);
                int num2 = (int)(byte)((double)*numPtr4 * (double)sbyte.MaxValue + 128.0);
                outputBuffer2[index3] = (byte)num2;
            }
            return outputBuffer;
        }

        private unsafe byte[] GetPCM16(float* data, int length)
        {
            byte[] outputBuffer = new byte[length * 2 * 2];
            float* numPtr1 = data;
            for (int index = 0; index < length; ++index)
            {
                float* numPtr2 = numPtr1;
                float* numPtr3 = (float*)((IntPtr)numPtr2 + 4);
                short num1 = (short)((double)*numPtr2 * (double)short.MaxValue);
                float* numPtr4 = numPtr3;
                numPtr1 = (float*)((IntPtr)numPtr4 + 4);
                short num2 = (short)((double)*numPtr4 * (double)short.MaxValue);
                outputBuffer[index * 4] = (byte)((uint)num1 & (uint)byte.MaxValue);
                outputBuffer[index * 4 + 1] = (byte)((uint)num1 >> 8);
                outputBuffer[index * 4 + 2] = (byte)((uint)num2 & (uint)byte.MaxValue);
                outputBuffer[index * 4 + 3] = (byte)((uint)num2 >> 8);
            }
            return outputBuffer;
        }

        private unsafe byte[] GetFloat(float* data, int length)
        {
            byte[] outputBuffer = new byte[length * 4 * 2];
            Marshal.Copy((IntPtr)(void*)data, outputBuffer, 0, outputBuffer.Length);
            return outputBuffer;
        }
    }
}
