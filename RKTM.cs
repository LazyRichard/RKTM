using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using UnityEngine;
using Harmony;

namespace Madeline.RKTM
{
    public class RKTM : Mod
    {
        public static RKTM singleton;
        ExternalDataSaver dataSaver;
        public ModContentPack pack;
        string _SecondLanguagePackName;
        public static string SecondLanguagePackName { get { return singleton._SecondLanguagePackName; } set { singleton._SecondLanguagePackName = value; } }
        public RKTM(ModContentPack pack) : base(pack)
        {
            Log.Message("Initializing RKTM language injector by madeline...");

            ExternalDataSaver.Initialize(pack.AssembliesFolder);

            singleton = this;

            HarmonyInstance HMinstance = HarmonyInstance.Create("Madeline.RKTM");
            HarmonyInstance.DEBUG = true;

            TranslatorPatch.Patch(HMinstance);
            DefInjectionPatch.Patch(HMinstance);
            CustomDefInjection.Patch(HMinstance);

            dataSaver = ExternalDataSaver.externalDataSaver;
            SecondLanguagePackName = dataSaver.GetData("AlternativeLanguageName");
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);

            Listing_Standard listing_Standard = new Listing_Standard();
            listing_Standard.Begin(inRect);

            string description = "SecondLanguagePackDescription".Translate();
            listing_Standard.Label("SecondLanguagePack".Translate(), -1, description);
            string result = listing_Standard.TextEntry(SecondLanguagePackName, 1);
            SecondLanguagePackName = result;

            listing_Standard.End();
        }

        public override void WriteSettings()
        {
            dataSaver.WriteData("AlternativeLanguageName", SecondLanguagePackName);
            dataSaver.SaveDataToFile();
            //SecondTranslatePackDB.UpdateSecondTranslatePackField();
        }

        public override string SettingsCategory()
        {
            return "RKTMSettingsLabel".Translate();
        }
    }
}
