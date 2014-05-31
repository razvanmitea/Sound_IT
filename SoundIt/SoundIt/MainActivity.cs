using System;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Speech;
using Android.Media;
using Android.Util;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Android.Graphics;

namespace SoundIt
{
    [Activity (Label = "Welcome to SoundIt", MainLauncher = true)]
	public class MainActivity : Activity
	{

		protected override void OnCreate (Bundle bundle)
		{

			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			Button buttonRec = FindViewById<Button> (Resource.Id.btnRec);
			Button buttonPlay = FindViewById<Button> (Resource.Id.btnPlay);
            EditText editTextBox = FindViewById<EditText>(Resource.Id.editText1);
            RecordAudio recAudio = new RecordAudio();
            PlayAudio playAudio = new PlayAudio ();

            buttonRec.Click += (sender, e) => {

                if(!recAudio.IsRecording)
                {
                    buttonRec.SetTextColor(Color.Red);
                    editTextBox.Text = string.Empty;
                    Toast.MakeText(this, "Recording", ToastLength.Short).Show();
                    Logger.LogThis("Started recording at " + DateTime.Now.ToString(), "Recording button", "MainActivity");
                    //ThreadPool.QueueUserWorkItem(o=>recAudio.StartAsync());
                    RunOnUiThread(() =>recAudio.StartAsync().Start());
                }
                else
                {
                    buttonRec.SetTextColor(Color.White);
                    RunOnUiThread(() => recAudio.Stop());
                    Toast.MakeText(this, "Stopped recording!", ToastLength.Short).Show();
                    Log.Info("Stopped","Stopped recording at " + DateTime.Now.ToString() + System.Environment.NewLine + "Full audio buffer length is: " + RecordAudio.fullAudioBuffer.Count.ToString());                   
                    editTextBox.Text = (RecordAudio.fullAudioBuffer.Count / 1000).ToString() + " KB recorded!";
                }  

                buttonPlay.Enabled = !recAudio.IsRecording;

            };

            buttonPlay.Click += (sender, e) => 
            {   
                if(RecordAudio.fullAudioBuffer.Count == 0)
                {
                    Toast.MakeText(this, "Nothing to play yet!", ToastLength.Short).Show();
                    return;
                }

                editTextBox.Text = "Playing.. ";
                playAudio.StartAsync().Start();
                editTextBox.Text = string.Empty;
            };
		}

        private void OutputEachByte(EditText et, bool allDiffZero)
        {
            foreach (var bt in RecordAudio.fullAudioBuffer)
            {
                Logger.LogThis(bt.ToString(), "audioPlay", "mainacitivity");
                /*

                if (allDiffZero)
                {
                    if (bt != 0)
                    {
                        //et.Text = bt.ToString();


                    }
                }
                else
                {
                    et.Text = bt.ToString();
                }
                */
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            //Apparently preloading a menu from a xml doesn't work anymore

            //Log.Debug("SoundIt", "Menu created!");
            //MenuInflater.Inflate(Resource.Menu.optionsMenu, menu); 

            menu.Add(0, 1, 1, "Quit");
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case 1: //Quit button
                    Finish();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

    }



}


