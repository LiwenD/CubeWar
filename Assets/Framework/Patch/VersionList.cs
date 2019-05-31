using System;
using System.Collections.Generic;
using System.Text;

namespace YummyGame.Framework
{
    public class VersionList
    {
        public List<Version> m_versions = new List<Version>();
        
        public void AddVersion(Version version)
        {
            if (!m_versions.Contains(version))
            {
                m_versions.Add(version);
            }
        }

        public void AddVersion(string version)
        {
            AddVersion(Version.FromString(version));
        }

        public static VersionList FromString(string str)
        {
            VersionList list = new VersionList();
            string[] splits = str.Split('-');
            foreach (var s in splits)
            {
                if (string.IsNullOrEmpty(s)) continue;
                Version version = Version.FromString(s);
                list.AddVersion(version);
            }
            list.m_versions.Sort((a, b) => a - b);
            return list;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < m_versions.Count; i++)
            {
                if (i != m_versions.Count - 1)
                {
                    sb.Append(m_versions[i].ToString()).Append("-");
                }
                else
                {
                    sb.Append(m_versions[i].ToString());
                }
            }
            return sb.ToString();
        }
    }
}
