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

			Button buttonRec = FindViewById<Button> (Resource.Id.myButton);
			Button buttonPlay = FindViewById<Button> (Resource.Id.btnPlay);
            ToggleButton buttonRecT = FindViewById<ToggleButton> (Resource.Id.toggleButton1);
            EditText editTextBox = FindViewById<EditText>(Resource.Id.editText1);
            RecordAudio recAudio = new RecordAudio();
            PlayAudio playAudio = new PlayAudio ();

            buttonRecT.Click += (sender, e) => {

                if (buttonRecT.Checked) {
                    Toast.MakeText(this, "Recording", ToastLength.Short).Show();
                    Logger.LogThis("Started recording at " + DateTime.Now.ToString(), "Recording button", "MainActivity");
                    //ThreadPool.QueueUserWorkItem(o=>recAudio.StartAsync());
                    recAudio.StartAsync();
                }
                else
                {
                    RunOnUiThread(() => recAudio.Stop());
                    Toast.MakeText(this, "Stopped", ToastLength.Short).Show();

                    Log.Info("Stopped","Stopped recording at " + DateTime.Now.ToString());
                    //OutputEachByte(editTextBox,false);
                    /*
                    var anyDiffZero = RecordAudio.fullAudioBuffer.AsEnumerable().Where(b=>b != 0).Any();
                    if(anyDiffZero)
                    {
                        editTextBox.Text = "I have something != 0";
                        Thread.Sleep(500);
                        OutputEachByte(editTextBox,true);
                    }
                    else
                    {
                        editTextBox.Text = "There all zeros";
                    }
                    */
                }   
                
            };

            buttonPlay.Click += (sender, e) => 
            {   
                editTextBox.Text = "Playing.. ";
                playAudio.StartAsync().Start();
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


