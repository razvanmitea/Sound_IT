using System;
using System.Threading;
using System.Threading.Tasks;
using Android.Media;
using Android.Util;
using Android.Widget;
using System.Collections.Generic;

namespace SoundIt
{
    public class RecordAudio  : INotification
    {
        AudioRecord audioRecord = null;
        short[] audioBuffer = new short[512];
        public static List<short> fullAudioBuffer = new List<short>();
        bool isRecording = false;

        public Boolean IsRecording
        {
            get { return (isRecording); }
            set{ this.isRecording = value;}
        }

        protected Task ReadAudioAsync()
        {
            return new Task(()=>{
               while(isRecording)
                {
                    try
                    {
                        // Keep reading the buffer while there is audio input.
                        audioRecord.Read(audioBuffer, 0, audioBuffer.Length);
                        fullAudioBuffer.AddRange(audioBuffer);
                        audioBuffer = new short[512];
                        //if(fullAudioBuffer.Count > 100000)
                        //    break;
                    }
                    catch (Exception ex)
                    {
                        Log.Error("ReadAudioSync", ex.Message);
                        Console.Out.WriteLine(ex.Message);
                        break;
                    }
                }
            });
        }

        protected Task StartRecorderAsync()
        {
            isRecording = true;

            audioRecord = openAudio();
            if (audioRecord != null)
            {
                audioRecord.StartRecording();

                // Off line this so that we do not block the UI thread.
                return ReadAudioAsync();
            }
            return null;
        }


        public Task StartAsync()
        {
            fullAudioBuffer.Clear();
            return StartRecorderAsync();
        }

        public void Stop()
        {
            audioRecord.Stop ();
            isRecording = false;
            Thread.Sleep(500); // Give it time to drop out.
            audioRecord.Release ();
        }

        private AudioRecord openAudio()
        {

            // 44100, 22050, 16000, 11025, 8000.
            int[] samplingRates = {44100, 22050, 16000, 11025, 8000};

            for (int i = 0; i < samplingRates.Length; ++i)
            {
                try
                {
                    int min = AudioRecord.GetMinBufferSize(samplingRates[i], 
                                                           ChannelIn.Mono, 
                                                           Encoding.Pcm16bit);
                    if (min < 4096)
                        min = 4096;
                    AudioRecord record = new AudioRecord(AudioSource.Mic, samplingRates[i],
                                                         ChannelIn.Mono, Encoding.Pcm16bit,    min);
                    if (record.State == State.Initialized)
                    {   
                        Logger.LogThis("Audio recorder initialised at " + record.SampleRate, "openAudio", "RecordAudio");
                        Toast.MakeText(Android.App.Application.Context,"Audio recorder initialised at " + record.SampleRate, ToastLength.Short);
                        return record;
                    }
                    record.Release();
                    record = null;
                }
                catch (Exception e)
                {
                    // Meh. Try the next one.
                }
            }
            // None worked.
            return null;
        }

    }
}

