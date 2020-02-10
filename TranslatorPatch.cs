using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using Verse;
using System.Reflection;
using System.Reflection.Emit;

namespace Madeline.RKTM
{
    static class TranslatorPatch
    {
        public static void Patch(HarmonyInstance HMinstance)
        {
            var original = AccessTools.Method(typeof(Translator), nameof(Translator.Translate), new Type[] { typeof(string) });
            var transpiler = AccessTools.Method(typeof(TranslatorPatch), nameof(TranslatorPatch.Transpiler_Translate));
            HMinstance.Patch(original, null, null, new HarmonyMethod(transpiler));

            var original2 = AccessTools.Method(typeof(LanguageDatabase), nameof(LanguageDatabase.LoadAllMetadata));
            var postfix = AccessTools.Method(typeof(TranslatorPatch), nameof(FetchSecondData));
            HMinstance.Patch(original2, null, new HarmonyMethod(postfix));
        }

        static IEnumerable<CodeInstruction> Transpiler_Translate(IEnumerable<CodeInstruction> instructions, ILGenerator ILG)
        {
            var GetSecondTranslate = AccessTools.Method(typeof(SecondTranslatePackDB), nameof(SecondTranslatePackDB.TryGetTextFromKey));
            var insts = instructions.ToList();
            var IL_000F_Label = insts.Find(instruction => instruction.operand != null && instruction.operand.ToString().Contains("defaultLanguage")).labels.First();
            var goToSecond = ILG.DefineLabel();

            for(int i = 0; i < instructions.Count(); i++)
            {
                var inst = insts[i];
                if(i > 0 && insts[i-1].opcode == OpCodes.Call && inst.opcode == OpCodes.Brfalse && insts[i-1].operand.ToString().Contains("TryTranslate"))
                { // entering IL_0008, brfalse, line 143
                    inst.operand = goToSecond;
                    yield return inst;
                    continue;
                }
               

                if(inst.opcode == OpCodes.Ldsfld)
                {
                    var IL_000F = inst.labels.ListFullCopy();
                    yield return new CodeInstruction(OpCodes.Ldarg_0) { labels = new List<Label>() { goToSecond } };
                    yield return new CodeInstruction(OpCodes.Ldloca_S, 0);
                    yield return new CodeInstruction(OpCodes.Call, GetSecondTranslate); // true or false
                    yield return new CodeInstruction(OpCodes.Brfalse, IL_000F.First());
                    yield return new CodeInstruction(OpCodes.Ldloc_0);
                    yield return new CodeInstruction(OpCodes.Ret);
                    yield return inst;
                    continue;
                }

                yield return inst;
            }
        }

        static void FetchSecondData()
        {
            //Log.Message($"Fetching languagePack {RKTM.SecondLanguagePackName}");
            SecondTranslatePackDB.secondTranslatePack = LanguageDatabase.AllLoadedLanguages.FirstOrDefault(la => la.folderName == RKTM.SecondLanguagePackName);
        }
    }
}
