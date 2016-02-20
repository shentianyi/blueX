using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScmClient.Enum;
using System.Media;

namespace ScmClient.Helper
{
   public class SoundHelper
    {
       public static void PlaySound(SoundType type) {
           string file = "success.wav";
           switch (type) { 
               case SoundType.SUCCESS:
                   file = "success.wav";
                   break;
               case SoundType.FAIL:
                   file = "fail.wav";
                   break;
               default:
                   file = "success.wav";
                   break;
           }

           string path = PathHelper.GetSoundPath(file);
           if (path != null) {
               SoundPlayer sp = new SoundPlayer(path);
               
               sp.Play();
           }
       }
    }
}
