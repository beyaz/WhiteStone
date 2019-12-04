﻿using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using BOA.TfsAccess;

namespace BOA.EntityGeneration.UI.Container.Bootstrapper
{
    class Launcher
    {
        #region Fields
        readonly Config config;
        readonly string ZipFileTempPath;
        #endregion

        #region Constructors
        public Launcher()
        {
            config          = Config.CreateFromFile();
            ZipFileTempPath = config.ZipFilePath + ".tmp";
        }
        #endregion

        #region Public Methods
        public void Start()
        {
            TryToExtractZipFile();

            if (!File.Exists(config.ProcessPath))
            {
                Console.WriteLine("Yükleniyor...");

                DownloadFile();

                Console.WriteLine("Yüklendi.");

                File.Move(ZipFileTempPath, config.ZipFilePath);

                TryToExtractZipFile();
            }

            Console.WriteLine("Starting process...");
            Process.Start(config.ProcessPath);

            Console.WriteLine("Process is started.");
        }
        #endregion

        #region Methods
        void DownloadFile()
        {
            TFSAccessForBOA.DownloadFile(config.ZipFileUrl, ZipFileTempPath);
        }

        void TryToExtractZipFile()
        {
            if (File.Exists(config.ZipFilePath))
            {
                if (Directory.Exists(config.ExportDir))
                {
                    Console.WriteLine("Deleting folder: " + config.ExportDir);
                    Directory.Delete(config.ExportDir, true);
                    Console.WriteLine("Deleted folder: " + config.ExportDir);
                }

                Console.WriteLine("Exporting zip file: " + config.ZipFilePath);
                ZipFile.ExtractToDirectory(config.ZipFilePath, config.ExportDir);

                Console.WriteLine("Deleting zip file: " + config.ZipFilePath);
                File.Delete(config.ZipFilePath);
                Console.WriteLine("Deleted zip file: " + config.ZipFilePath);
            }
        }
        #endregion
    }
}