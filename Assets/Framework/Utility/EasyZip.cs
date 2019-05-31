using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace YummyGame.Framework
{
    public delegate bool ProgressDelegate(float progeess);
    public delegate bool ErrorDelegate(string name, string msg);

    public class EasyZip
    {

        private ProgressDelegate _progress { get; set; }
        private ErrorDelegate _error { get; set; }

        public void SetProgress(ProgressDelegate p)
        {
            _progress = p;
        }

        public void SetErrorHandler(ErrorDelegate err)
        {
            _error = err;
        }

        public void Zip(string file, string source)
        {
            try
            {
                if (!Directory.Exists(source)) return;
                if (File.Exists(file))
                {
                    File.Delete(file);
                }

                ZipConstants.DefaultCodePage = 0;
                FastZipEvents events = new FastZipEvents();
                events.Progress = new ProgressHandler(ShowProgress);
                events.ProgressInterval = TimeSpan.FromSeconds(0.017);

                events.DirectoryFailure = this.DirectoryFailure;
                events.FileFailure = this.FileFailure;

                var fastZip = new FastZip(events);
                fastZip.CreateEmptyDirectories = true;
                fastZip.RestoreAttributesOnExtract = false;
                fastZip.RestoreDateTimeOnExtract = false;

                fastZip.CreateZip(file, source, true,null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public void Unzip(string file, string outpath)
        {
            try
            {
                if (!Directory.Exists(outpath))
                {
                    Directory.CreateDirectory(outpath);
                    Debug.Log("创建目录:" + outpath);
                }

                ZipConstants.DefaultCodePage = 0;
                FastZipEvents events = new FastZipEvents();
                events.Progress = new ProgressHandler(ShowProgress);
                events.ProgressInterval = TimeSpan.FromSeconds(0.017);

                events.DirectoryFailure = this.DirectoryFailure;
                events.FileFailure = this.FileFailure;

                var fastZip = new FastZip(events);
                fastZip.CreateEmptyDirectories = true;
                fastZip.RestoreAttributesOnExtract = false;
                fastZip.RestoreDateTimeOnExtract = false;

                string fileFilter = null;
                fastZip.ExtractZip(file, outpath, fileFilter);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        void ShowProgress(object sender, ProgressEventArgs e)
        {
            // Very ugly but this is a sample!
            //			Debug.LogFormat("{0}%", e.PercentComplete);
            if (_progress != null)
            {
                _progress.Invoke(e.PercentComplete);
            }
        }

        void DirectoryFailure(object sender, ScanFailureEventArgs e)
        {
            if (_error != null)
            {
                _error.Invoke(e.Name, e.Exception.Message);
            }
        }

        void FileFailure(object sender, ScanFailureEventArgs e)
        {
            if (_error != null)
            {
                _error.Invoke(e.Name, e.Exception.Message);
            }
        }
    }
}
