using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using Verse;

namespace Madeline.RKTM
{
    public class ExternalDataSaver
    {
        public static ExternalDataSaver externalDataSaver;
        XDocument xdoc;
        string settingsSaveFolderPath;
        string filePath;
        
        public static void Initialize(string ModAssemblyFolderPath)
        {
            Log.Message("Initializing ExternalDataSaver...");
            if(externalDataSaver != null)
                throw new Exception();

            var instance = new ExternalDataSaver(ModAssemblyFolderPath);
            externalDataSaver = instance;
        }
        private ExternalDataSaver(string SettingSaveFolderPath)
        {
            this.settingsSaveFolderPath = SettingSaveFolderPath;
            Initialize();
        }

        void Initialize()
        {
            setPath();
            xdoc = LoadOrCreateXDoc();
        }

        void setPath()
        {
            var filename = "RKTMDataSave.xml";
            string path = Path.Combine(settingsSaveFolderPath, filename);
            filePath = path;
        }
        
        XDocument LoadOrCreateXDoc()
        {
            var path = filePath;
            if(File.Exists(path))
            {
                return XDocument.Load(path);
            }
            else
            {
                return CreateXDoc();
            }
        }

        XDocument CreateXDoc()
        {
            var xdoc = new XDocument(new XElement("Data"));
            return xdoc;
        }

        public string GetData(string key)
        {
            var element = xdoc.Root?.Element(key)?.Value ?? "None";
            return element;
        }

        public void WriteData(string key, string value)
        {
            var element = xdoc.Root.Element(key);
            if(element == null)
            {
                element = new XElement(key);
                xdoc.Root.Add(element);
            }

            element.Value = value;
        }

        public void SaveDataToFile()
        {
            xdoc.Save(filePath);
        }
    }
}