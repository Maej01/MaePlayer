using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.IO;

namespace MaeMediaPlayer
{
    public partial class MainForm : Form
    {
        private SpeechRecognitionEngine Engine; //DEFINING A SPEECH RECOGNITION OBJECT
        private SpeechSynthesizer jarvis; //DEFINING A SPEECH RECOGNITION SYNTHESIZER OBJECT CALLED jarvis
        WMPLib.WindowsMediaPlayer windowsMediaPlayer = new WMPLib.WindowsMediaPlayer();
        String[] files, paths; //THESE ARRAY VARIABLES WILL HOLD THE FILE NAMES OF THE SELECTED MEDIA FILE


        public MainForm()
        {
            InitializeComponent();
        }

        //FORM LOAD
        private void MainForm_Load(object sender, EventArgs e)
        {
            loadSpeech(); //CALLING THE SPEECH RECOGNITION PROCEDURE
            bunifuFormFadeTransition1.ShowAsyc(this); //APPLYING FADING TRANSITION TO THE FORM

            volume.Value = 100;

            btnUnMute.Visible = false;
        }

        //THIS IS THE SPEECH RECOGNITION PROCEDURE
        private void loadSpeech()
        {
            Engine = new SpeechRecognitionEngine(); //CREATING THE INSTANCE OF THE SPEECH RECOGNITION
            jarvis = new SpeechSynthesizer(); //CREATING THE INSTANCE OF THE SPEECH SYNTHESIZER           
            Engine.SetInputToDefaultAudioDevice();  //ALLOWING THE INSTANCE TO RECEIVE INPUT FROM THE MICROPHONE
            grammarAndCommands(); //CALLING THE LOAD GRAMMAR PROCEDURE
            Engine.LoadGrammar(new DictationGrammar());
            Engine.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Voice); //CREATING AN EVENT HANDLER CALLED VOICE
            Engine.RecognizeAsync(RecognizeMode.Multiple); //STARTING THE SPEECH RECOGNITION

        }

        //LOADING THE GRAMMAR
        private void grammarAndCommands()
        {
            try
            {
                Choices texts = new Choices();
                String[] lines = File.ReadAllLines(Environment.CurrentDirectory + "\\MaePlayerCommands.txt"); //COLLECTING ALL COMMANDS FROM THE FILE

                texts.Add(lines);
                Grammar wordsList = new Grammar(new GrammarBuilder(texts));
                Engine.LoadGrammar(wordsList);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);
            }
        }

        //CODE FOR THE EVENT HANDLER CALLED VOICE
        private void Voice(object sender, SpeechRecognizedEventArgs e)
        {
            String speech = (e.Result.Text);

            switch (speech)
            {
                case "add play list":
                    jarvis.Speak("please choose files from your drive");
                    btnAddMediaFiles.PerformClick();
                    break;
                case "maej minimize player":
                    WindowState = FormWindowState.Minimized;
                    TopMost = false;
                    break;
                case "maej show player":
                    WindowState = FormWindowState.Normal;
                    TopMost = true;
                    break;
                case "maej play music":
                    btnPlay.PerformClick();
                    break;
                case "maej play video":
                    btnPlay.PerformClick();
                    break;
                case "maej play movie":
                    btnPlay.PerformClick();
                    break;
                case "maej pause music":
                    btnPause.PerformClick();
                    break;
                case "maej pause video":
                    btnPause.PerformClick();
                    break;
                case "maej pause movie":
                    btnPause.PerformClick();
                    break;
                case "ok maej resume":
                    btnPause.PerformClick();
                    break;
                case "maej stop playing":
                    btnStop.PerformClick();
                    break;
                case "maej play previous":
                    btnPrevious.PerformClick();
                    break;
                case "maej play next":
                    btnNext.PerformClick();
                    break;
                case "maej mute volume":
                    btnMute.PerformClick();
                    break;
                case "maej unmute volume":
                    btnUnMute.PerformClick();
                    break;
                case "maej play full screen":
                    btnFullScreen.PerformClick();
                    break;
                case "maej exit full screen":
                    MaePlayer.Focus();
                    MaePlayer.fullScreen = false;
                    break;
                case "maej close player":
                    jarvis.Speak("Ok Dr. Muhammad, Closing in 3 2 1");
                    this.Close();
                    break;
                case "show about":
                    btnAbout.PerformClick();
                    break;
                case "who created you":
                    jarvis.Speak("Muhammad Mustapha Sesay");
                    break;
                case "what is your name":
                    jarvis.Speak("Mae player");
                    break;
                case "maej what's the date":
                    jarvis.Speak(DateTime.Today.ToString("dd-MM-yyyy"));
                    break;
                case "maej what's the time":
                    jarvis.Speak(DateTime.Now.ToString());
                    break;
            }
        }

        //CODE FOR THE MINIMIZE BUTTON
        private void btnMin_Click_1(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        //CODE FOR THE MAXIMIZE BUTTON
        private void btnMax_Click_1(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
        }

        //CODE FOR THE CLOSE BUTTON
        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        //CODE FOR THE PAUSE BUTTON
        private void btnPause_Click(object sender, EventArgs e)
        {
            if (MaePlayer.playState == WMPLib.WMPPlayState.wmppsPaused)
            {
                MaePlayer.Ctlcontrols.play();
            }
            else
            {
                MaePlayer.Ctlcontrols.pause();
            }
        }

        //CODE FOR THE PLAY BUTTON
        private void btnPlay_Click_2(object sender, EventArgs e)
        {
            lstMediaFiles.SelectedIndex = 0;
            MaePlayer.Ctlcontrols.play();
        }

        //CODE FOR THE STOP BUTTON
        private void btnStop_Click_1(object sender, EventArgs e)
        {
            MaePlayer.Ctlcontrols.stop();
        }

        //CODE FOR THE PREVIOUS BUTTON
        private void btnPrevious_Click(object sender, EventArgs e)
        {

            if (MaePlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                if (lstMediaFiles.SelectedIndex == 0)
                {
                    lstMediaFiles.SelectedIndex = 0;
                    lstMediaFiles.Update();
                }
                else
                {
                    MaePlayer.Ctlcontrols.previous();
                    lstMediaFiles.SelectedIndex -= 1;
                    lstMediaFiles.Update();
                }
            }
        }

        //CODE FOR THE NEXT BUTTON
        private void btnNext_Click_1(object sender, EventArgs e)
        {

            if (MaePlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                if (lstMediaFiles.SelectedIndex < (lstMediaFiles.Items.Count - 1))
                {
                    MaePlayer.Ctlcontrols.next();   //SETTTING THE CONTROL TO NEXT
                    lstMediaFiles.SelectedIndex += 1;   //IF THE LISTBOX HAS MORE THAN ONE ITEMS INCREMENT
                    lstMediaFiles.Update();
                }
                else
                {
                    lstMediaFiles.SelectedIndex = 0;
                    lstMediaFiles.Update();
                }
            }

        }

        //CODE IF A FILE IS SELECTED
        private void lstMediaFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            MaePlayer.URL = paths[lstMediaFiles.SelectedIndex];
        }

        //CODE FOR THE VOLUMN BUTTON 
        private void volumn_ValueChanged(object sender, EventArgs e)
        {
            int rate = 100 * (volume.Value - 10);
            MaePlayer.settings.volume = volume.Value;
        }

        //CODE FOR THE FULL SCREEN MODE
        private void btnFullScreen_Click(object sender, EventArgs e)
        {
            if (MaePlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                MaePlayer.fullScreen = true; //SETTING THE MEDIA PLAYER TO FULL SCREEN MODE
            }
            else
            {
                MaePlayer.fullScreen = false; //SETTING THE MEDIA PLAYER TO NO FULL SCREEN MODE
            }
        }

        //CODE FOR THE MUTE BUTTON
        private void btnMute_Click_1(object sender, EventArgs e)
        {
            if (MaePlayer.settings.volume == 100)
            {
                MaePlayer.settings.volume = 0; //SETTING THE VOLUMN TO 0
                btnMute.Visible = false;  //SETTING THE UNMUTE TO INVISIBLE AND INACTIVE
                btnUnMute.Visible = true; //SETTING THE MUTE BUTTON TO VISIBLE AND ACTIVE
            }
            else
            {
                MaePlayer.settings.volume = 100;
            }
        }

        //CODE FOR THE UNMUTE BUTTON
        private void btnUnMute_Click_1(object sender, EventArgs e)
        {
            if (MaePlayer.settings.volume == 0)
            {
                MaePlayer.settings.volume = 100;
                btnMute.Visible = true;  //SETTING THE UNMUTE TO VISIBLE AND ACTIVE
                btnUnMute.Visible = false;
            }
            else
            {
                MaePlayer.settings.volume = 0;
            }
        }

        //CODE FOR THE ABOUT BUTTON
        private void btnAbout_Click(object sender, EventArgs e)
        {
            var aboutPlayer = new About(); //CREATED A INSTACE OF THE ABOUT OBJECT
            //this.Hide();
            aboutPlayer.ShowDialog(); //CALLING THE ABOUT TO DISPLAY
        }

        //CODE FOR THE VOLUMN MOUSE CLICK EVENT
        private void btnAddMediaFiles_Click(object sender, EventArgs e)
        {
            String userName = System.Environment.UserName; //DEFINING AN OBJECT OT HOLD THE NAME OF THE PC OWNER

            OpenFileDialog opf = new OpenFileDialog(); //CREATING AN OPEN FILE DIALOG OBJECT
            opf.InitialDirectory = @"C:\Users\" + userName + "\\Documents\\MyMusic";
            opf.Filter = "(mp3, wav, mp4, ogg, flv, avi, 3gp, mpg, mov, wmv)|*.mp3; *.wav; *.mp4; *.ogg; *.flv; *.avi; *.3gp; *.mpg; *.mov; *.wmv; |all files|*.*";
            opf.Multiselect = true;

            if (opf.ShowDialog() == DialogResult.OK)
            {
                files = opf.SafeFileNames;
                paths = opf.FileNames;

                for (int i = 0; i < files.Length; i++)
                {
                    lstMediaFiles.Items.Add(files[i]);
                }
            }
        }


        //CODE FOR THE VOLUMN MOUSE CLICK EVENT
        private void volumn_MouseClick(object sender, MouseEventArgs e)
        {
            btnMute.Visible = true;  //SETTING THE UNMUTE TO VISIBLE AND ACTIVE
            btnUnMute.Visible = false; //SETTING THE MUTE BUTTON TO INVISIBLE
        }

        //CODE FOR THE MEDIA LIST BOX DRAG DROP 
        private void lstMediaFiles_DragDrop(object sender, DragEventArgs e)
        {
            for (int i = 0; i < files.Length; i++)
            {
                lstMediaFiles.Items.Add(files[i]);
            }
        }

        //CODE FOR THE FULL SCREEN MODE
        private void btnFullScreen_Click_1(object sender, EventArgs e)
        {
            if (MaePlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                MaePlayer.fullScreen = true; //SETTING THE MEDIA PLAYER TO FULL SCREEN MODE
            }
            else
            {
                MaePlayer.fullScreen = false; //SETTING THE MEDIA PLAYER TO NO FULL SCREEN MODE
            }
        }

        //CODE FOR THE VOLUMN BUTTON 
        private void volume_ValueChanged(object sender, EventArgs e)
        {
            int rate = 100 * (volume.Value - 10);
            MaePlayer.settings.volume = volume.Value;
        }

        //CODE FOR THE VOLUMN MOUSE CLICK EVENT
        private void volume_MouseClick_1(object sender, MouseEventArgs e)
        {
            btnMute.Visible = true;  //SETTING THE UNMUTE TO VISIBLE AND ACTIVE
            btnUnMute.Visible = false; //SETTING THE MUTE BUTTON TO INVISIBLE
        }


        //CODE IF A FILE IS SELECTED
        private void lstMediaFiles_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            MaePlayer.URL = paths[lstMediaFiles.SelectedIndex];
        }
    }
}
