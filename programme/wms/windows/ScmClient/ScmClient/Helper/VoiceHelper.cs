using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpeechLib;
using System.Threading;

namespace ScmClient.Helper
{
    public class VoiceHelper
    {
        public string Text { get; set; }
        public int Times { get; set; }
        public VoiceHelper() { Times = 1; }

        public   void Speak()
        {
            Thread inventoryThread = new Thread(speakT);//盘点线程
            inventoryThread.Start();

        }
        private void speakT()
        {
            try
            {
                if (!string.IsNullOrEmpty(this.Text))
                {
                    SpeechVoiceSpeakFlags SpFlags = SpeechVoiceSpeakFlags.SVSFDefault;
                    SpVoice Voice = new SpVoice();
                    while (this.Times > 0)
                    {
                        Voice.Speak(this.Text, SpFlags);
                        this.Times -= 1;
                    }
                }
            }
            catch { }
        }
    }
   
}
