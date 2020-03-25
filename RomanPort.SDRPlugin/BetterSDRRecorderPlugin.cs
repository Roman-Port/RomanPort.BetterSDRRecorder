using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SDRSharp.Common;

namespace RomanPort.BetterSDRRecorder
{
    public class BetterSDRRecorderPlugin : ISharpPlugin
    {
        private const string _displayName = "Better SDR Recorder";
        private ISharpControl _control;
        private RomanPortToolsPanel _guiControl;
        private bool isPlaying;

        public UserControl Gui
        {
            get { return _guiControl; }
        }

        public string DisplayName
        {
            get { return _displayName; }
        }

        public void Close()
        {

        }

        public void Initialize(ISharpControl control)
        {
            _control = control;
            _guiControl = new RomanPortToolsPanel();
            _control.PropertyChanged += _control_PropertyChanged;
            
        }

        private void _control_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(_control.IsPlaying != isPlaying)
            {
                if(_control.IsPlaying)
                {
                    //Just started playing
                    _guiControl.InitParts(_control);
                } else
                {
                    //Just finished playing
                }
                isPlaying = _control.IsPlaying;
            }
        }
    }
}