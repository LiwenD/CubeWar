using System;
using System.Collections.Generic;
using UnityEngine;

namespace YummyGame.Framework
{
    public class DataManager:UnitySingleton<DataManager>
    {
        static string secret = "f&a%rtw@zg56&k-";

        private string ParseString(string key)
        {
            return secret + key;
        }

        public int GetInt(string key,int def = 0)
        {
            if (!HasKey(key)) return def;
            return PlayerPrefs.GetInt(ParseString(key));
        }

        public float GetFloat(string key,float def = 0.0f)
        {
            if (!HasKey(key)) return def;
            return PlayerPrefs.GetFloat(ParseString(key));
        }

        public bool GetBool(string key,bool def = false)
        {
            if (!HasKey(key)) return def;
            return PlayerPrefs.GetInt(ParseString(key))>0;
        }

        public string GetString(string key,string def = "")
        {
            if (!HasKey(key)) return def;
            return PlayerPrefs.GetString(ParseString(key));
        }

        public bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(ParseString(key));
        }

        public void SetInt(string key, int val)
        {
            PlayerPrefs.SetInt(ParseString(key),val);
        }

        public void SetFloat(string key, float val)
        {
            PlayerPrefs.SetFloat(ParseString(key), val);
        }

        public void SetBool(string key, bool val)
        {
            PlayerPrefs.SetInt(ParseString(key), val ? 1 : 0);
        }

        public void SetString(string key, string val)
        {
            PlayerPrefs.SetString(ParseString(key), val);
        }

        public void DeleteKey(string key)
        {
            if (HasKey(key))
            {
                PlayerPrefs.DeleteKey(ParseString(key));
            }
        }
    }
}
