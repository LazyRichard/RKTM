using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using UnityEngine;

namespace Madeline.RKTM
{
    public class RKTM : Mod
    {
        public static RKTM singleton;
        RKTMSettings RKTMSettings;
        public static string SecondLanguagePackName { get { return singleton.RKTMSettings.secondLanguagePack; } }
        public RKTM(ModContentPack pack) : base(pack)
        {
            this.RKTMSettings = GetSettings<RKTMSettings>();
            singleton = this;
            TranslatorPatch.Initialize();
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
            SecondTranslatePackDB.UpdateSecondTranslatePackField();
        }

        public override string SettingsCategory()
        {
            return "RKTMSettingsLabel".ToString();
        }
    }
}
