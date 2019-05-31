using System;
using System.Collections.Generic;

namespace YummyGame.Framework
{
    public class Version : IEquatable<Version>, IFormattable
    {
        private readonly uint _data;
        public static readonly Version zero = new Version(0);
        public Version(uint version)
        {
            _data = version;
        }

        public uint Major
        {
            get
            {
                return (_data >> 24) & 0xff;
            }
        }

        public uint Minor
        {
            get
            {
                return (_data >> 16) & 0xff;
            }
        }

        public uint Revision
        {
            get
            {
                return (_data >> 8) & 0xff;
            }
        }

        public uint Build
        {
            get
            {
                return _data & 0xff;
            }
        }

        public static bool IsHotUpdateVersion(Version baseVersion,Version updateVersion)
        {
            return baseVersion.Major == updateVersion.Major && baseVersion.Minor == updateVersion.Minor
                && baseVersion.Revision == updateVersion.Revision && baseVersion.Build < updateVersion.Build;
        }

        public static bool IsFullUpdateVersion(Version baseVersion, Version updateVersion)
        {
            return baseVersion.Major != updateVersion.Major || baseVersion.Minor != updateVersion.Minor
                || baseVersion.Revision != updateVersion.Revision;
        }

        public Version AddRevision(uint add)
        {
            uint re = Revision + 1;
            return new Version(_data + (add << 8));
        }

        public Version GetBaseVersion()
        {
            return new Version(_data & 0xffffff00);
        }

        public static bool IsBaseVersion(string str, char split = '.')
        {
            if (!IsValidVersionString(str, split)) return false;
            return FromString(str, split).Build == 0;
        }

        public static bool IsValidVersionString(string str, char split = '.')
        {
            string[] splits = str.Split(split);
            if (splits.Length != 4) return false;
            foreach (var value in splits)
            {
                int res = 0;
                if (!int.TryParse(value, out res)) return false;
                if (res > 255 || res < 0) return false;
            }
            return true;
        }

        public static Version FromString(string str, char split = '.')
        {
            if (!IsValidVersionString(str,split))
            {
                throw new System.Exception($"版本号错误：{str}");
            }
            string[] splits = str.Split(split);
            uint version = 0;
            for (int i = 0; i < 4; i++)
            {
                uint tmp;
                if (!uint.TryParse(splits[i], out tmp))
                {
                    throw new System.Exception($"版本号错误：{str}");
                }
                version |= (tmp << (24 - 8 * i));
            }
            return new Version(version);
        }

        public static Version operator+(Version v, int add)
        {
            return new Version((uint)(v._data + add));
        }

        public static int operator -(Version v1, Version v2)
        {
            return (int)v1._data - (int)v2._data;
        }

        public static bool operator ==(Version v1, Version v2)
        {
            return v1._data == v2._data;
        }

        public static bool operator !=(Version v1, Version v2)
        {
            return v1._data != v2._data;
        }

        public static bool operator >(Version v1, Version v2)
        {
            return v1._data > v2._data;
        }

        public static bool operator <(Version v1, Version v2)
        {
            return v1._data < v2._data;
        }

        public static bool operator >=(Version v1, Version v2)
        {
            return v1._data >= v2._data;
        }

        public static bool operator <=(Version v1, Version v2)
        {
            return v1._data <= v2._data;
        }

        public bool Equals(Version other)
        {
            return _data == other._data;
        }

        public override string ToString()
        {
            return $"{Major}.{Minor}.{Revision}.{Build}";
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is Version)
            {
                return (Version)obj == this;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return (int)_data;
        }
    }
}
