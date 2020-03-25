using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomanPort.BetterSDRRecorder.Framework
{
    public class WavEncoder : Stream
    {
        public Stream baseStream;
        public long wavLength;
        private long fileSizeOffs;
        private long dataSizeOffs;

        private ushort formatTag;
        private ushort channels;
        private uint samplesPerSec;
        private uint avgBytesPerSec;
        private ushort blockAlign;
        private ushort bitsPerSample;


        public override bool CanRead => false;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => baseStream.Length;

        public override long Position { get => baseStream.Position; set => baseStream.Position = value; }

        public override void Flush()
        {
            baseStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return baseStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            baseStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            baseStream.Write(buffer, offset, count);
            wavLength += count;
            UpdateLength();
        }

        public WavEncoder(Stream baseStream, AudioWriter audio)
        {
            //Set stream
            this.baseStream = baseStream;
            
            //Set metadatas
            formatTag = audio.formatTag;
            channels = audio.channels;
            samplesPerSec = audio.samplesPerSecond;
            blockAlign = (ushort)((uint)channels * ((uint)audio.bitsPerSample / 8U));
            avgBytesPerSec = samplesPerSec * (uint)blockAlign;
            bitsPerSample = audio.bitsPerSample;

            //Write header info
            WriteHeader();
        }

        public void WriteHeader()
        {
            WriteTag("RIFF");
            fileSizeOffs = this.baseStream.Position;
            WriteUnsignedInt(0U);
            WriteTag("WAVE");
            WriteTag("fmt ");
            WriteUnsignedInt(16U);
            WriteUnsignedShort(formatTag);
            WriteUnsignedShort(channels);
            WriteUnsignedInt(samplesPerSec);
            WriteUnsignedInt(avgBytesPerSec);
            WriteUnsignedShort(blockAlign);
            WriteUnsignedShort(bitsPerSample);
            WriteTag("data");
            dataSizeOffs = this.baseStream.Position;
            WriteUnsignedInt(0U);
        }

        private void UpdateLength()
        {
            this.baseStream.Seek((int)this.fileSizeOffs, SeekOrigin.Begin);
            WriteUnsignedInt((uint)((ulong)this.baseStream.Length - 8UL));
            this.baseStream.Seek((int)this.dataSizeOffs, SeekOrigin.Begin);
            WriteUnsignedInt((uint)this.Length);
            this.baseStream.Seek(0L, SeekOrigin.End);
        }

        private void WriteTag(string tag)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(tag);
            baseStream.Write(bytes, 0, bytes.Length);
        }

        private void WriteUnsignedInt(uint value)
        {
            WriteEndianBytes(BitConverter.GetBytes(value));
        }

        private void WriteUnsignedShort(ushort value)
        {
            WriteEndianBytes(BitConverter.GetBytes(value));
        }

        private void WriteEndianBytes(byte[] data)
        {
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(data);
            baseStream.Write(data, 0, data.Length);
        }
    }
}
