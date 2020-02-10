using System;
using System.Linq;
using Harmony;
using System.Reflection;
using System.Collections.Generic;
using Verse;
using System.Reflection.Emit;

namespace Madeline.RKTM
{
    public static class CustomDefInjection
    {
        #region transpiler harmony patch
        public static void Patch(HarmonyInstance HMinstance)
        {
            var original = AccessTools.Method(typeof(CustomDefInjection), nameof(InjectSecondsToData));
            var transpiler = AccessTools.Method(typeof(CustomDefInjection), nameof(DontMake10ParameterMethodFuckYouTynan));
            HMinstance.Patch(original, null, null, new HarmonyMethod(transpiler));

            var InjectIntoDefs = AccessTools.Method(typeof(DefInjectionPackage), "InjectIntoDefs");
            var postfix = AccessTools.Method(typeof(CustomDefInjection), nameof(FetchUsedTranslateDataPaths));
            HMinstance.Patch(InjectIntoDefs, null, new HarmonyMethod(postfix));
        }

        static IEnumerable<CodeInstruction> DontMake10ParameterMethodFuckYouTynan(IEnumerable<CodeInstruction> instructions)
        {
            var replacedString = AccessTools.Field(typeof(DefInjectionPackage.DefInjection), "replacedString");
            var replacedList = AccessTools.Field(typeof(DefInjectionPackage.DefInjection), "replacedList");
            var setDefFieldAtPath = AccessTools.Method(typeof(DefInjectionPackage), "SetDefFieldAtPath");
            var insts = instructions.ToList();
            for(int i = 0; i < insts.Count; i++)
            {
                var inst = insts[i];
                if(inst.opcode == OpCodes.Ldstr && inst.operand != null)
                {
                    if(inst.operand.ToString().Contains("Patch here1"))
                    {
                        // defType, pairKey, fullListInjection, typeof(List<string>), errorOnDefNotFOund(false), fileSource, isPlaceHolder, &normalizedPath, &suggestedPath, &replacedString, &replacedList
                        yield return new CodeInstruction(OpCodes.Ldloc_2); // instance
                        yield return new CodeInstruction(OpCodes.Ldloc, 7); // defType
                        yield return new CodeInstruction(OpCodes.Ldloc, 8); // pairKey
                        yield return new CodeInstruction(OpCodes.Ldloc, 14); // fullListInjection
                        yield return new CodeInstruction(OpCodes.Ldloc, 7); // typeof(List<string>)
                        yield return new CodeInstruction(OpCodes.Ldc_I4_0); // errorOnDefNotFound(false)
                        yield return new CodeInstruction(OpCodes.Ldloc, 9); // fileSource
                        yield return new CodeInstruction(OpCodes.Ldloc, 10); // isPlaceHolder
                        yield return new CodeInstruction(OpCodes.Ldloca, 5); // normalizedPath
                        yield return new CodeInstruction(OpCodes.Ldloca, 6); // suggestedPath
                        yield return new CodeInstruction(OpCodes.Ldloc, 11);
                        yield return new CodeInstruction(OpCodes.Ldflda, replacedString); // replacedString
                        yield return new CodeInstruction(OpCodes.Ldloc, 11);
                        yield return new CodeInstruction(OpCodes.Ldflda, replacedList); // replacedList
                        yield return new CodeInstruction(OpCodes.Call, setDefFieldAtPath);
                        //yield return new CodeInstruction(OpCodes.Ldloc, 

                        //yield return new CodeInstruction(OpCodes.
                    }
                    else if(inst.operand.ToString().Contains("Patch here2"))
                    {
                        //defType, pairKey, Injection, typeof(string), errorOnDefNotFound, fileSource, isPlaceholder, &normalizedPath, &suggestedPath, &replacedString, &replacedList
                        yield return new CodeInstruction(OpCodes.Ldloc_2); // instance
                        yield return new CodeInstruction(OpCodes.Ldloc, 7); // defType
                        yield return new CodeInstruction(OpCodes.Ldloc, 8); // pairKey
                        yield return new CodeInstruction(OpCodes.Ldloc, 17); // Injection
                        yield return new CodeInstruction(OpCodes.Ldloc, 18); // typeof(string)
                        yield return new CodeInstruction(OpCodes.Ldc_I4_0); // errorOnDefNotFound(false)
                        yield return new CodeInstruction(OpCodes.Ldloc, 9); // fileSource
                        yield return new CodeInstruction(OpCodes.Ldloc, 10); // isPlaceHolder
                        yield return new CodeInstruction(OpCodes.Ldloca, 5); // normalizedPath
                        yield return new CodeInstruction(OpCodes.Ldloca, 6); // suggestedPath
                        yield return new CodeInstruction(OpCodes.Ldloc, 11);
                        yield return new CodeInstruction(OpCodes.Ldflda, replacedString); // replacedString
                        yield return new CodeInstruction(OpCodes.Ldloc, 11);
                        yield return new CodeInstruction(OpCodes.Ldflda, replacedList); // replacedList
                        yield return new CodeInstruction(OpCodes.Call, setDefFieldAtPath);
                    }
                    i += 1;
                    continue;
                }

                yield return inst;
            }

        }
        static void FetchUsedTranslateDataPaths(Dictionary<string, DefInjectionPackage.DefInjection> ___injections)
        {
            RegisterUsedPath(___injections.Keys.ToList());
        }

        #endregion
        static HashSet<string> alreadyUsedPaths = new HashSet<string>();
        public static void InjectLanguageData(LoadedLanguage alternativeLanguage)
        {
            alternativeLanguage.LoadData();
            var alternativeInjections = new List<Pair<DefInjectionPackage, KeyValuePair<string, DefInjectionPackage.DefInjection>>>();
            int AllPairCount = 0;
            foreach(var defInjectionPackage in alternativeLanguage.defInjections)
            {
                foreach(var pair in defInjectionPackage.injections)
                {
                    var translatePath = pair.Key;
                    if(!alreadyUsedPaths.Contains(translatePath))
                    {
                        var item = new Pair<DefInjectionPackage, KeyValuePair<string, DefInjectionPackage.DefInjection>>(defInjectionPackage, pair);
                        alternativeInjections.Add(item);
                    }
                    AllPairCount += 1;
                }
            }
            Log.Message($"Injecting {alternativeInjections.Count} language datas from {AllPairCount} data, language : {alternativeLanguage.folderName}");
            InjectSecondsToData(alternativeInjections);
        }

        static void InjectSecondsToData(IEnumerable<Pair<DefInjectionPackage, KeyValuePair<string, DefInjectionPackage.DefInjection>>> InstanceValuePairs)
        { // 바꾸지 말것, transpiler로 고쳐져있음, 로컬 변수의 개수나 순서를 변경하면 transpiler가 깨질 가능성이 높음!
            foreach(var InstancePair in InstanceValuePairs)
            {
                var instance = InstancePair.First;
                var pair = InstancePair.Second;
                if (!pair.Value.injected)
                {
                    string normalizedPath = string.Empty;
                    string suggestedPath = string.Empty;

                    var defType = instance.defType;
                    var pairKey = pair.Key;
                    var fileSource = pair.Value.fileSource;
                    var isPlaceHolder = pair.Value.isPlaceholder;
                    var DefInjectionInstance = InstancePair.Second.Value;

                    if (pair.Value.IsFullListInjection)
                    {
                        var fullListInjection = pair.Value.fullListInjection;
                        var type = typeof(List<string>);
                        // transpiler 삽입 구간 //
                        string str1 = "Patch here1";
                        // trnaspiler 삽입 구간 //
                        //pair.Value.injected = this.SetDefFieldAtPath(this.defType, pair.Key, pair.Value.fullListInjection, typeof(List<string>), errorOnDefNotFound, pair.Value.fileSource, pair.Value.isPlaceholder, out normalizedPath, out suggestedPath, out pair.Value.replacedString, out pair.Value.replacedList);
                    }
                    else
                    {
                        var injection = pair.Value.injection;
                        var type = typeof(string);
                        // transpiler 삽입 구간 //
                        string str2 = "Patch here2";
                        // transpiler 삽입 구간 //
                        //pair.Value.injected = this.SetDefFieldAtPath(this.defType, pair.Key, pair.Value.injection, typeof(string), errorOnDefNotFound, pair.Value.fileSource, pair.Value.isPlaceholder, out normalizedPath, out suggestedPath, out pair.Value.replacedString, out pair.Value.replacedList);
                    }
                    pair.Value.normalizedPath = normalizedPath;
                    pair.Value.suggestedPath = suggestedPath;
                }
            }
        }

        static void RegisterUsedPath(string path)
        {
            alreadyUsedPaths.Add(path);
        }

        static void RegisterUsedPath(List<string> paths)
        {
            //예 : IceSheet.label
            //예 : IceSheet.description
            //예 : SeaIce.label
            //예 : SeaIce.description
            alreadyUsedPaths.AddRange(paths);
        }
    }
}