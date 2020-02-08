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
            Log.Message("Patch Invoked!");
            //var testoriginal = AccessTools.Method(typeof(DefInjectionPackage), "SetDefFieldAtPath");
            //var testpostfix = AccessTools.Method(typeof(DefInjectionPatch), nameof(DefInjectionPatch.SetDefFieldTest));
            //HMinstance.Patch(testoriginal, null, new HarmonyMethod(testpostfix));

            var original = AccessTools.Method(typeof(LoadedLanguage), "InjectIntoData_AfterImpliedDefs");
            var postfix = AccessTools.Method(typeof(DefInjectionPatch), nameof(LoadSecondAfterLoadingImpliedDefsLangugaeData));
            HMinstance.Patch(original, null, new HarmonyMethod(postfix));
        }

        static void LoadSecondAfterLoadingImpliedDefsLangugaeData()
        {
            CustomDefInjection.InjectLanguageData(SecondTranslatePackDB.secondTranslatePack);
        }

        static void SetDefFieldTest(string path, object value, string normalizedPath, string suggestedPath, string replacedString)
        {
            string message = $"path : {path}, value : {value}, normalizedPath : {normalizedPath}, suggestedPath : {suggestedPath}, replacedString {replacedString}";
            Log.Message(message);
        }
    }
}