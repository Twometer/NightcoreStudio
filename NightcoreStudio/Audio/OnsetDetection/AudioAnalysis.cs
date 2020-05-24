using CSCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightcoreStudio.Audio.OnsetDetection
{
    class AudioAnalysis
    {
        const int SAMPLE_SIZE = 1024;

        // Audio stream fed into the sound playback device
        //BlockAlignReductionStream stream;
        // Instance of sound playback device
        // public WaveOutEvent outputDevice;

        // Fast Fourier Transform library
        FFT fft;

        /// <summary>
        /// Raw audio data
        /// </summary>
        public ISampleSource PCMStream { get; set; }

        // Onset Detection
        OnsetDetection onsetDetection;
        public float[] OnsetsFound { get; set; }
        public float TimePerSample { get; set; }

        // Constructor
        public AudioAnalysis()
        {
            SetUpFFT();
        }

        ~AudioAnalysis()
        {
            DisposeOutputDevice();
        }

        // Used to set up the sound device
        private void InitialiseOutputDevice()
        {
            DisposeOutputDevice();

            //outputDevice = new WaveOutEvent();
            //outputDevice.Init(new WaveChannel32(stream));
            //outputDevice.Init(PCMStream);
        }

        public void LoadAudioFromFile(string filePath)
        {
            // MP3
           /* if (filePath.EndsWith(".mp3"))
            {
                PCMStream = new AudioFileReader(filePath);
            }
            // WAV
            else if (filePath.EndsWith(".wav"))
            {
                PCMStream = new AudioFileReader(filePath);
            }

            if (PCMStream != null)
            {
                // Throw an error is the audio has more channels than stereo
                if (PCMStream.WaveFormat.Channels > 2)
                {
                    throw new FormatException("Only Mono and Stereo are supported");
                }

                InitialiseOutputDevice();
                OnsetsFound = null;
            }
            else
            {
                throw new FormatException("Invalid audio file");
            }*/
        }

        // Play out the loaded audio file
        // Returns whether function was successful
        public bool PlayAudio()
        {
            if (PCMStream != null)
            {
                // If audio was previously stopped
                // Or audio has reached the end of the track
                // Reset the playback position to the beginning
                if (PCMStream.Position == PCMStream.Length)
                {
                    PCMStream.Position = 0;
                }

                

                return true;
            }

            return false;
        }

        // Pause the audio file
        public bool PauseAudio()
        {
            if (PCMStream != null)
            {
                

                return true;
            }

            return false;
        }

        // Stop the audio file
        public bool StopAudio()
        {
            if (PCMStream != null)
            {
                
                PCMStream.Position = 0;

                return true;
            }

            return false;
        }

        // Track Position getter/setter
        public long GetTrackPosition()
        {
            return PCMStream.Position;
        }
        public void SetTrackPosition(long position)
        {
            PCMStream.Position = position;
        }

        public void DetectOnsets(float sensitivity = 1.5f)
        {
            onsetDetection = new OnsetDetection(PCMStream.WaveFormat.SampleRate, 1024);
            // Has finished reading in the audio file
            bool finished = false;
            // Set the pcm data back to the beginning
            SetTrackPosition(0);


            float[] buf = new float[SAMPLE_SIZE];
            var readtotal = 0;
            do
            {
                var read = PCMStream.Read(buf, 0, buf.Length);
                readtotal += read;

                if (read == 0) break;
                // Read in audio data and find the flux values until end of audio file
                onsetDetection.AddFlux(buf);
            }
            while (readtotal < PCMStream.Length);
            Console.WriteLine("Finished adding flux!");

            // Find peaks
            onsetDetection.FindOnsets(sensitivity);
        }

        public void NormalizeOnsets(int type)
        {
            onsetDetection.NormalizeOnsets(type);
        }

        public float[] GetOnsets()
        {
            return onsetDetection.Onsets;
        }

        public float GetTimePerSample()
        {
            return onsetDetection.TimePerSample();
        }

        #region Internals
        // Starts up the Fast Fourier Transform class
        void SetUpFFT()
        {
            fft = new FFT();

            //Determine how phase works on the forward and inverse transforms. 
            // (0, 1) default
            // (1, -1) for signal processing
            fft.A = 0;
            fft.B = 1;
        }

        // Properly clean up sound output device
        public void DisposeOutputDevice()
        {

        }

        public void DisposeAudioAnalysis()
        {
            
        }

        #endregion
    }
}
