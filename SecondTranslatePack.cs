using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Madeline.RKTM
{
    public static class SecondTranslatePackDB
    {
        public static LoadedLanguage secondTranslatePack { get; set; }

        public static void UpdateSecondTranslatePackField()
        {
            secondTranslatePack = LanguageDatabase.AllLoadedLanguages.FirstOrDefault(la => la.folderName == RKTM.SecondLanguagePackName);
        }

        public static bool TryGetTextFromKey(string key, ref string result)
        {
            if (secondTranslatePack == null)
            {
                result = key;
                return false;
            }
            else
            {
                return secondTranslatePack.TryGetTextFromKey(key, out result);
            }
        }
    }
}
