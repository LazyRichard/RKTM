using System;
using System.Linq;
using Harmony;
using System.Reflection;
using System.Collections.Generic;
using Verse;

namespace Madeline.RKTM
{
    public static class CustomDefInjection
    {
        static List<string> alreadyUsedPaths = new List<string>();
        public static void InjectLanguageData(LoadedLanguage alternativeLanguage)
        {
            var alternativeInjections = new List<Pair<DefInjectionPackage, KeyValuePair<string, DefInjectionPackage.DefInjection>>>();
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
                }
            }

            InjectSecondsToData(alternativeInjections);
        }

        static void InjectSecondsToData(IEnumerable<Pair<DefInjectionPackage, KeyValuePair<string, DefInjectionPackage.DefInjection>>> InstanceValuePairs)
        {
            var method = AccessTools.Method(typeof(DefInjectionPackage), "InjectIntoDefs");
            bool errorOnDefNotFound = false;
            foreach(var InstancePair in InstanceValuePairs)
            {
                var instance = InstancePair.First;
                var pair = InstancePair.Second;
                if (!pair.Value.injected)
                {
                    string normalizedPath = string.Empty;
                    string suggestedPath = string.Empty;
                    if (pair.Value.IsFullListInjection)
                    {
                        string str1 = "Patch here";
                        method.Invoke(instance, null);
                        //pair.Value.injected = this.SetDefFieldAtPath(this.defType, pair.Key, pair.Value.fullListInjection, typeof(List<string>), errorOnDefNotFound, pair.Value.fileSource, pair.Value.isPlaceholder, out normalizedPath, out suggestedPath, out pair.Value.replacedString, out pair.Value.replacedList);
                    }
                    else
                    {
                        string str2 = "Patch here2";
                        method.Invoke(instance, null);
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
    }
}