using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace YummyGame.Framework.Editor
{
    [Serializable]
    [XmlRoot("Patches")]
    public class Patches
    {
        [XmlElement("platforms")]
        public List<PlatformPatch> patches = new List<PlatformPatch>();
    }

    [Serializable]
    public class PlatformPatch
    {
        public PlatformPatch() { }
        public PlatformPatch(string platform)
        {
            this.platform = platform;
        }
        [XmlElement("platform")]
        public string platform;

        [XmlElement("histories")]
        public List<Patch> patches = new List<Patch>();
    }

    [Serializable]
    public class Patch
    {
        public Patch() { }
        public Patch(string baseVersion)
        {
            this.baseVersion = baseVersion;
        }
        [XmlElement("base")]
        public string baseVersion;

        [XmlElement("hotfix")]
        public List<string> hotFix = new List<string>();
    }
}
