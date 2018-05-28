﻿using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using PbixTools.FileSystem;
using Serilog;

namespace PbixTools.ProjectSystem
{
    public class PbixProject
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<PbixProject>();

        public static readonly string Filename = ".pbixproj.json";
        public static readonly Version CurrentVersion = new Version(0, 2);

        /*
         * PBIXPROJ Change Log
         * ===================
         * 0.0 - Initial version (Mashup, Model, Report, CustomVisuals, StaticResources)
         * 0.1 - Model/dataSources: use location (query name) as folder name (rather than datasource guid); always write 'dataSource.json'
         *     - FIX: use static name inside dataSource.json
         * 0.2 - "dataSources" renamed to "queries"
         *     - '/Mashup/Package/Formulas/Section1.m' rather than '/Mashup/Section1.m' (package fully extracted)
         */

        /* Entries to add later: */
        // Settings
        // Deployments

        [JsonProperty("version")]
        public string VersionString { get; set; }
        [JsonProperty("queries")]
        public IDictionary<string, string> Queries { get; set; }


        [JsonIgnore]
        public Version Version
        {
            get => Version.TryParse(VersionString, out var version) ? version : CurrentVersion;
            set => this.VersionString = value.ToString();
        }

        public PbixProject()
        {
            this.Version = CurrentVersion;
            this.Queries = new Dictionary<string, string>();
        }


        public static PbixProject FromFolder(IProjectRootFolder folder)
        {
            var file = folder.GetFile(Filename);
            if (file.TryGetFile(out Stream stream))
            {
                using (var reader = new StreamReader(stream))
                {
                    try
                    {
                        return JsonConvert.DeserializeObject<PbixProject>(reader.ReadToEnd());
                        // at this stage we could perform version compatibility checks
                    }
                    catch (JsonReaderException e)
                    {
                        Log.Error(e, "Failed to read PBIXPROJ file from {Path}", file.Path);
                    }
                }
            }

            return new PbixProject();
        }

        public void Save(IProjectRootFolder folder)
        {
            var json = JsonConvert.SerializeObject(this, Formatting.Indented); // don't use CamelCaseContractResolver as it will modify query names

            folder.GetFile(Filename).Write(json);
        }
    }
}