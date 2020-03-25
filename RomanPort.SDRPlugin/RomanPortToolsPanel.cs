using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SDRSharp.Common;
using System.IO;
using RomanPort.BetterSDRRecorder.Framework;
using RomanPort.BetterSDRRecorder.Recorders;

namespace RomanPort.BetterSDRRecorder
{
    public partial class RomanPortToolsPanel : UserControl
    {
        public ISharpControl control;
        public RewindWriter rewind;

        public const string START_RECORDING_TEXT = "Start";
        public const string END_RECORDING_TEXT = "Stop";
        
        public RomanPortToolsPanel()
        {
            InitializeComponent();
            recordAudioFormat.SelectedIndex = 1; //pcm-16
            recordAudioOutput.SelectedIndex = 1; //wav
            recordBroadbandFormat.SelectedIndex = 1; //pcm-16
            recordBroadbandOutput.SelectedIndex = 1; //wav
            rewindTime.Value = GetDefaultRewindTime();
        }

        public void InitParts(ISharpControl control)
        {
            this.control = control;
            InitRewind();
        }

        public void InitRewind()
        {
            rewind = new RewindWriter(control, Framework.RecordingMode.Audio, Framework.WavSampleFormat.PCM16, GetDefaultRewindTime());
            rewind.OnUpdateEvent += UpdateRewindStatus;
            UpdateRewindStatus();
            rewind.StartRecording();
        }

        public void UpdateRewindStatus()
        {
            rewindStatus.Text = $"s/{Math.Ceiling((double)rewind.GetBufferSizeBytes() / 1000)} KB max - {Math.Round(rewind.GetUsableSeconds())}s";
            rewindUsageBar.Value = (int)Math.Round(rewind.GetBufferFullness() * 100);
        }

        private void rewindSaveBtn_Click(object sender, EventArgs e)
        {
            //Get usable buffer and read bytes
            byte[] buffer = new byte[rewind.GetUsableBytes()];
            rewind.ReadBuffer(buffer, 0);

            //Open a file prompt
            SaveFileDialog fd = new SaveFileDialog();
            fd.Title = "Save Audio Buffer";
            fd.Filter = "Audio File (*.wav)|*.wav";
            DialogResult ar = fd.ShowDialog();

            //Save
            if(ar == DialogResult.OK)
            {
                using(FileStream fs = new FileStream(fd.FileName, FileMode.Create))
                using(WavEncoder wav = new WavEncoder(fs, rewind))
                {
                    wav.Write(buffer, 0, buffer.Length);
                    wav.Flush();
                }
                rewind.ResetBuffer();
            }
        }

        private void rewindSaveAndRecordBtn_Click(object sender, EventArgs e)
        {
            //Save the rewind buffer and begin recording audio
            //Get usable buffer and read bytes
            byte[] buffer = new byte[rewind.GetUsableBytes()];
            rewind.ReadBuffer(buffer, 0);

            //Reset rewind
            rewind.ResetBuffer();

            //Start the audio recorder and copy this into the buffer
            OpenAudioRecorder();
            recorderAudio.OnGetSamples(buffer, buffer.Length / (int)rewind.samplesPerSecond / rewind.channels);
            recorderAudio.StartRecording();
        }

        private void rewindTime_ValueChanged(object sender, EventArgs e)
        {
            if (rewind == null)
                return;
            rewind.bufferSeconds = (int)rewindTime.Value;
            rewind.ResetBuffer();
        }

        private int GetDefaultRewindTime()
        {
            return 60;
        }

        /* AUDIO RECORDING PART */

        public BasicRecorder recorderAudio;

        private void recordAudioBtn_Click(object sender, EventArgs e)
        {
            if(recorderAudio == null)
            {
                //Start
                OpenAudioRecorder();
                recorderAudio.StartRecording();
            } else
            {
                //Stop
                recorderAudio.StopRecording();
                recordAudioFormat.Enabled = true;
                recordAudioOutput.Enabled = true;
                rewindSaveAndRecordBtn.Enabled = true; //Make sure we can't save over this. Audio only
                recorderAudio.Save();
                recorderAudio = null;
                recordAudioBtn.Text = START_RECORDING_TEXT;
            }
        }

        private void OpenAudioRecorder()
        {
            recorderAudio = new BasicRecorder(control, RecordingMode.Audio, (WavSampleFormat)recordAudioFormat.SelectedIndex, (BasicRecorderOutput)recordAudioOutput.SelectedIndex);
            recordAudioFormat.Enabled = false;
            recordAudioOutput.Enabled = false;
            rewindSaveAndRecordBtn.Enabled = false; //Make sure we can't save over this. Audio only
            recorderAudio.OnUpdateEvent += UpdateRecordAudioStatus;
            recordAudioBtn.Text = END_RECORDING_TEXT;
            UpdateRecordAudioStatus();
        }

        private void UpdateRecordAudioStatus()
        {
            if(recorderAudio != null)
            {
                double seconds = recorderAudio.GetRecordedSeconds();
                long ticks = (long)(seconds * 10000000);
                TimeSpan time = new TimeSpan(ticks);
                recordAudioStatus.Text = $"{time.Hours.ToString("00")}:{time.Minutes.ToString("00")}:{time.Seconds.ToString("00")} - {Math.Round((double)recorderAudio.GetBytesOnDisk() / 1024 / 1024)} MB";
            }
        }

        /* BROADBAND RECORDING PART */

        public BasicRecorder recorderBroadband;

        private void recordBroadbandBtn_Click(object sender, EventArgs e)
        {
            if (recorderBroadband == null)
            {
                //Start
                OpenBroadbandRecorder();
                recorderBroadband.StartRecording();
            }
            else
            {
                //Stop
                recorderBroadband.StopRecording();
                recordBroadbandOutput.Enabled = true;
                recordBroadbandFormat.Enabled = true;
                recorderBroadband.Save();
                recorderBroadband = null;
                recordBroadbandBtn.Text = START_RECORDING_TEXT;
            }
        }

        private void OpenBroadbandRecorder()
        {
            recorderBroadband = new BasicRecorder(control, RecordingMode.Baseband, (WavSampleFormat)recordBroadbandFormat.SelectedIndex, (BasicRecorderOutput)recordBroadbandOutput.SelectedIndex);
            recordBroadbandOutput.Enabled = false;
            recordBroadbandFormat.Enabled = false;
            recorderBroadband.OnUpdateEvent += UpdateRecordBroadbandStatus;
            recordBroadbandBtn.Text = END_RECORDING_TEXT;
            UpdateRecordBroadbandStatus();
        }

        private void UpdateRecordBroadbandStatus()
        {
            if (recorderBroadband != null)
            {
                double seconds = recorderBroadband.GetRecordedSeconds();
                long ticks = (long)(seconds * 10000000);
                TimeSpan time = new TimeSpan(ticks);
                recordBroadbandStatus.Text = $"{time.Hours.ToString("00")}:{time.Minutes.ToString("00")}:{time.Seconds.ToString("00")} - {Math.Round((double)recorderBroadband.GetBytesOnDisk() / 1024 / 1024)} MB";
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Roman-Port/RomanPort.BetterSDRRecorder");
        }
    }
}
