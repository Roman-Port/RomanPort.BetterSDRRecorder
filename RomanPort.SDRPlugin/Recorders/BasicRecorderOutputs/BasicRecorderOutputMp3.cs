using RomanPort.BetterSDRRecorder.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;
using Telerik.WinControls.Zip;
using System.Net.Http;
using System.Diagnostics;

namespace RomanPort.BetterSDRRecorder.Recorders.BasicRecorderOutputs
{
    public class BasicRecorderOutputMp3 : IBasicRecorderOutput
    {
        public const string FFMPEG_URL = "https://ffmpeg.zeranoe.com/builds/win32/static/ffmpeg-4.2.2-win32-static.zip";

        public Process ffmpeg;
        public string filename;
        public MemoryStream prewriteBuffer = new MemoryStream();

        public override void Close()
        {
            //Send close command to FFMPEG
            ffmpeg.Kill();

            //Wait for FFMPEG to end
            ffmpeg.WaitForExit();

            //Prompt for final location
            SaveFileDialog fd = new SaveFileDialog();
            fd.Title = "Save Output";
            fd.Filter = $"MP3 Files (*.mp3)|*.mp3";

            //Open
            var result = fd.ShowDialog();
            if (result == DialogResult.OK)
            {
                //Copy to this
                File.Move(filename, fd.FileName);
            }
            else
            {
                //Delete temporary file
                File.Delete(filename);
            }
        }

        public override long GetBytesOnDisk()
        {
            FileInfo fi = new FileInfo(filename);
            if (!fi.Exists)
                return 0;
            return fi.Length;
        }

        public override void Open(AudioWriter audio)
        {
            //Download FFMPEG if we haven't
            DownloadFFMPEG();

            //Obtain our next path
            filename = GetNextTempOutputFile();

            //Get the format
            string format;
            switch(audio.Format)
            {
                case WavSampleFormat.PCM8: format = "u8"; break;
                case WavSampleFormat.PCM16: format = "s16le"; break;
                case WavSampleFormat.Float32: format = "f32le"; break;
                default: throw new Exception("Unknown format!");
            }

            //Start FFMPEG
            ffmpeg = Process.Start(new ProcessStartInfo
            {
                FileName = "rp_lib_ffmpeg.exe",
                Arguments = $"-f {format} -ar {audio.SampleRate} -ac {audio.channels} -i pipe: -f mp3 " + filename,
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            });
        }

        public override void Write(byte[] data)
        {
            //If ffmpeg isn't running yet, write to the prewrite buffer
            if(ffmpeg == null)
            {
                prewriteBuffer.Write(data, 0, data.Length);
                return;
            }

            //If FFMPEG has just started, copy the prewrite buffer
            if(ffmpeg != null && prewriteBuffer != null)
            {
                prewriteBuffer.Position = 0;
                prewriteBuffer.CopyTo(ffmpeg.StandardInput.BaseStream);
                prewriteBuffer.Dispose();
                prewriteBuffer = null;
            }

            //Write data
            ffmpeg.StandardInput.BaseStream.Write(data, 0, data.Length);
        }

        public void DownloadFFMPEG()
        {
            //Check if we already have it
            if (File.Exists("rp_lib_ffmpeg.exe"))
                return;
            
            //Prompt if it's OK
            DialogResult dialogResult = MessageBox.Show("To record to mp3, FFMPEG will automatically be downloaded. Is this OK?\n\nYou will only need to do this once.", "MP3 Encoder First-Time Setup", MessageBoxButtons.YesNo);
            if (dialogResult != DialogResult.Yes)
            {
                throw new Exception("User stopped FFMPEG download");
            }

            //Download and unzip
            using(MemoryStream raw = new MemoryStream())
            {
                using (HttpClient wc = new HttpClient())
                    wc.GetStreamAsync(FFMPEG_URL).GetAwaiter().GetResult().CopyTo(raw);

                //Unzip
                raw.Position = 0;
                using (ZipArchive z = new ZipArchive(raw, ZipArchiveMode.Read, false, Encoding.UTF8))
                using (Stream s = z.GetEntry("ffmpeg-4.2.2-win32-static/bin/ffmpeg.exe").Open())
                using (FileStream fs = new FileStream("rp_lib_ffmpeg.exe", FileMode.Create))
                    s.CopyTo(fs);
            }
        }
    }
}
