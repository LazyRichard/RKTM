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
        RKTMSettings RKTMSettings;
        ExternalDataSaver dataSaver;
        public ModContentPack pack;
        public static string SecondLanguagePackName;
        public RKTM(ModContentPack pack) : base(pack)
        {
            this.RKTMSettings = GetSettings<RKTMSettings>();
            singleton = this;
            HarmonyInstance HMinstance = HarmonyInstance.Create("Madeline.RKTM");
            HarmonyInstance.DEBUG = true;
            TranslatorPatch.Patch(HMinstance);
            DefInjectionPatch.Patch(HMinstance);
            ExternalDataSaver.Initialize(pack.AssembliesFolder);
            dataSaver = ExternalDataSaver.externalDataSaver;
            SecondLanguagePackName = dataSaver.GetData("AlternativeLanguageName");
            Log.Message(pack.AssembliesFolder);
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);

            Listing_Standard listing_Standard = new Listing_Standard();
            listing_Standard.Begin(inRect);

            string description = "SecondLanguagePackDescription".Translate();
            string result = listing_Standard.TextEntryLabeled("SecondLanguagePack", RKTMSettings.secondLanguagePack, 1);
            RKTMSettings.secondLanguagePack = result;

            listing_Standard.End();
        }

        public override void WriteSettings()
        {
            base.WriteSettings();
            dataSaver.WriteData("AlternativeLanguageName", RKTMSettings.secondLanguagePack);
            dataSaver.SaveDataToFile();
            //SecondTranslatePackDB.UpdateSecondTranslatePackField();
        }

        public override string SettingsCategory()
        {
            return "RKTMSettingsLabel".ToString();
        }
    }
}
