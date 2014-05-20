using System;
using System.Threading.Tasks;
using Android.Media;

namespace SoundIt
{
    public class PlayAudio
    {
        AudioTrack audioTrack = null;
               
        protected Task PlayAudioTrackAsync  ()
        {
            return new Task(() => {
            audioTrack = new AudioTrack (
                // Stream type
                Android.Media.Stream.Music,
                // Frequency
                44100,
                // Mono or stereo
                ChannelConfiguration.Mono,
                // Audio encoding
                Android.Media.Encoding.Pcm16bit,
                // Length of the audio clip.
                RecordAudio.fullAudioBuffer.Count,
                // Mode. Stream or static.
                AudioTrackMode.Stream);

            audioTrack.Play ();

                audioTrack.Write(RecordAudio.fullAudioBuffer.ToArray(), 0, RecordAudio.fullAudioBuffer.Count);
            });
        }

        public Task StartAsync ()
        {
            return PlayAudioTrackAsync();
        }

        public void Stop ()
        {
            if (audioTrack != null) {
                audioTrack.Stop ();
                audioTrack.Release ();
                audioTrack = null;
            }
        }

    }
}

