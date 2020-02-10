using System;
using System.Collections;
using System.Collections.Generic;
using Harmony;
using Verse;
using System.Linq;

namespace Madeline.RKTM
{
    public static class DefInjectionPatch
    {
        public static void Patch(HarmonyInstance HMinstance)
        {
            var original = AccessTools.Method(typeof(LoadedLanguage), "InjectIntoData_AfterImpliedDefs");
            var postfix = AccessTools.Method(typeof(DefInjectionPatch), nameof(LoadSecondAfterLoadingImpliedDefsLangugaeData));
            HMinstance.Patch(original, null, new HarmonyMethod(postfix));
        }

        static void LoadSecondAfterLoadingImpliedDefsLangugaeData()
        {
            if(SecondTranslatePackDB.secondTranslatePack == null)
            {
                Log.Warning("no matched languagePack!");
                return;
            }

            Log.Message($"Injecting second LanguagePack {SecondTranslatePackDB.secondTranslatePack.folderName}");
            CustomDefInjection.InjectLanguageData(SecondTranslatePackDB.secondTranslatePack);
        }
    }
}