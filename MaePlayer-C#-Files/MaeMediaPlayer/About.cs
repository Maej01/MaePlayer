using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Recognition; //IMPORT THE SPEECH RECGONITION FRAMEWORK
using System.Speech.Synthesis;  //IMPORT THE SPEECH SYNTHESIS FRAMEWORK
using System.IO;

namespace MaeMediaPlayer
{
    public partial class About : Form
    {
        private SpeechRecognitionEngine Engine; //DEFINING A SPEECH RECOGNITION OBJECT
        private SpeechSynthesizer jarvis; //DEFINING A SPEECH RECOGNITION SYNTHESIZER OBJECT CALLED jarvis
        WMPLib.WindowsMediaPlayer windowsMediaPlayer = new WMPLib.WindowsMediaPlayer();


        private int childFormNumber = 0;

        public About()
        {
            InitializeComponent();
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }

        //FORM LOAD
        private void About_Load_1(object sender, EventArgs e)
        {
            loadSpeech();
            bunifuFormFadeTransition1.ShowAsyc(this); //APPLYING FADING TRANSITION TO THE FORM

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
                case "maej read about":
                    jarvis.Speak("MaePlayer is a free open source media player that support majority video and audio formats and it also has a speech recognition capability.");
                    break;
                case "who is the developer":
                    jarvis.Speak("Muhammad Mustapha Sesay");
                    break;
                case "who developed mae payer":
                    jarvis.Speak("Muhammad Mustapha Sesay");
                    break;
                case "give me his contact":
                    jarvis.Speak("do you want the Orange number or the Africel number");
                    break;
                case "give me the Orange number":
                    jarvis.Speak("+23279712365");
                    break;
                case "the orange number":
                    jarvis.Speak("+23279712365");
                    break;
                case "give me the Africell number":
                    jarvis.Speak("+23299662105");
                    break;
                case "the africell number":
                    jarvis.Speak("+23299662105");
                    break;
                case "give me the email address":
                    jarvis.Speak("muhammadsesay8@gmail.com");
                    break;
                case "minimize about":
                    WindowState = FormWindowState.Minimized;
                    TopMost = false;
                    break;
                case "restore about":
                    WindowState = FormWindowState.Normal;
                    TopMost = true;
                    break;
                case "close about":
                    jarvis.Speak("Ok Dr. Muhammad, Closing in 3 2 1");
                    this.Close();
                    break;
                case "maej close about":
                    jarvis.Speak("Ok Dr. Muhammad, Closing in 3 2 1");
                    this.Close();
                    break;
                case "who created you":
                    jarvis.Speak("Muhammad Mustapha Sesay");
                    break;
                case "what is your name":
                    jarvis.Speak("Mae player");
                    break;
                case "maej what's the date":
                    jarvis.Speak(DateTime.Today.ToString());
                    break;
                case "maej what's the time":
                    jarvis.Speak(DateTime.Now.ToString());
                    break;
            }
        }


        //CODE FOR THE MINIMIZE BUTTON
        private void btnMin_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        //CODE FOR THE CLOSE BUTTON
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

     
    }
}
