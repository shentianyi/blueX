using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpeechLib;
using System.Threading;

namespace ScmClient.Helper
{
    public class VoiceText {
        public string Text { get; set; }
        public int Times { get; set; }

        public VoiceText() {
            this.Times = 1;
        }
    }

    public class VoiceHelper
    {
        public   List<VoiceText> Text { get; set; }
       // public int Times { get; set; }
        public VoiceHelper() { 
        
        }
        public VoiceHelper(string text, int times = 1)
        {
            this.Text = new List<VoiceText>() { new VoiceText() { Text = text, Times = times } };
          
        }

        public   void Speak()
        {
            Thread inventoryThread = new Thread(speakT);//盘点线程
            inventoryThread.Start();
         // speakT();
        }
        private void speakT()
        {
            try
            {
                SpeechVoiceSpeakFlags SpFlags = SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak;
                SpVoice Voice = new SpVoice();
                foreach (VoiceText vt in this.Text)
                {
                    if (!string.IsNullOrEmpty(vt.Text))
                    {
                        while (vt.Times > 0)
                        {
                            Voice.Speak(vt.Text, SpFlags);
                            vt.Times -= 1;
                        }
                    }
                }
                //if (!string.IsNullOrEmpty(this.Text))
                //{
                //    
                //    while (this.Times > 0)
                //    {
                //        Voice.Speak(this.Text, SpFlags);
                //        this.Times -= 1;
                //    }
                //}
            }
            catch { }
        }
    }
   
}
