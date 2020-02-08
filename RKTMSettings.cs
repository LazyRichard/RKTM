using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace Madeline.RKTM
{
    public class RKTMSettings : ModSettings
    {
        public string secondLanguagePack;
        public RKTMSettings()
        {
            base.ExposeData();
            Scribe_Values.Look<string>(ref secondLanguagePack, "secondLanguagePack", "None", false);
        }
    }
}
