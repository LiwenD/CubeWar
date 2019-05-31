using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace YummyGame.Framework
{
    public class VersionManager:UnitySingleton<VersionManager>
    {
        public List<Version> versions { get; private set; }
        private Version latestVersion;
        private Version initialVersion;
        private bool initialed;

        private void Awake()
        {
            versions = new List<Version>();
            latestVersion = GetInitialVersion();
            initialed = false;
        }

        public Version GetInitialVersion()
        {
            if(initialed == false)
            {
                TextAsset ta = Resources.Load<TextAsset>("Version");
                initialVersion = Version.FromString(ta.text);
                initialed = true;
            }
            return initialVersion;
        }

        public void SetLatestVersion(Version version)
        {
            latestVersion = version;
        }

        public Version GetLatestVersion()
        {
            return latestVersion;
        }

        public void AddVersion(string version)
        {
            versions.Add(Version.FromString(version));
        }

        public void AddVersion(Version version)
        {
            versions.Add(version);
        }

        public string GetDataVersionPath(Version version)
        {
            return Utility.PathCombile(Config.AssetRoot, version.ToString());
        }

        public string GetInitialVersionPath()
        {
            return GetDataVersionPath(GetInitialVersion());
        }
    }
}
