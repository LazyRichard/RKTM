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
            //var testoriginal = AccessTools.Method(typeof(DefInjectionPackage), "SetDefFieldAtPath");
            //var testpostfix = AccessTools.Method(typeof(DefInjectionPatch), nameof(DefInjectionPatch.SetDefFieldTest));
            //HMinstance.Patch(testoriginal, null, new HarmonyMethod(testpostfix));

            var original = AccessTools.Method(typeof(LoadedLanguage), "InjectIntoData_AfterImpliedDefs");
            var postfix = AccessTools.Method(typeof(DefInjectionPatch), nameof(LoadSecondAfterLoadingImpliedDefsLangugaeData));
            HMinstance.Patch(original, null, new HarmonyMethod(postfix));

            var original2 = AccessTools.Method(typeof(LoadedLanguage), nameof(LoadedLanguage.InjectIntoData_BeforeImpliedDefs));
            var prefix2 = AccessTools.Method(typeof(DefInjectionPatch), nameof(BeforeImpliedDefTest));
            HMinstance.Patch(original2, new HarmonyMethod(prefix2));

            var original3 = AccessTools.Method(typeof(LoadedLanguage), nameof(LoadedLanguage.InjectIntoData_AfterImpliedDefs));
            HMinstance.Patch(original3, new HarmonyMethod(prefix2));
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

        static void SetDefFieldTest(string path, object value, string normalizedPath, string suggestedPath, string replacedString)
        {
            string message = $"path : {path}, value : {value}, normalizedPath : {normalizedPath}, suggestedPath : {suggestedPath}, replacedString {replacedString}";
            Log.Message(message);
        }

        static bool BeforeImpliedDefTest()
        {
            Log.Message("preventing injecting language data to game");
            return false;
        }
    }
}