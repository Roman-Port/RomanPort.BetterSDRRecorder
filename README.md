# Better SDR Recorder
The better SDR recorder is a work in progress plugin to improve on the crappy built in recorder in SDR#

![Demo](example.gif)

**([FULL SIZE/SPEED EXAMPLE VIDEO HERE](https://youtu.be/ZSAUdbxg2H4))**

## Features
* **Rewind buffer** to save transmissions received if the record button wasn't pressed
* Ability to push audio in the replay buffer into the recording session to continue recording, without interruption
* **MP3** audio export option
* **Arbitrary recording save location**, prompted after the recording is finished
* **No more file size limit** of 2^31 bytes. Recordings are automatically split
* **Raw, WAV or MP3** export of audio samples
* Split audio/baseband recording sessions

## Known Bugs
* Saved IQ is not displayed at the correct frequency, but plays back fine

## Installation
**This is pre-release software**.

* Download the DLL from the releases tab in GitHub (note that it is likely to be outdated)
* Save the downloaded DLL to the SDRSharp folder
* In ``Plugins.xml``, in your SDRSharp folder, add the following line before ``</sharpPlugins>``:
```<add key="RomanPort Better SDR Recorder" value="RomanPort.BetterSDRRecorder.BetterSDRRecorderPlugin,RomanPort.BetterSDRRecorder" />```

## Finishing Tips
You should come join us in the [RTL-SDR Discord Server](https://discord.gg/JWTRfk3)