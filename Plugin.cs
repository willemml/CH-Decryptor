using BepInEx;
using HarmonyLib;

using System;
using System.IO;
using System.IO.Compression;
using System.IO.MemoryMappedFiles;

using System.Reflection.Emit;

using System.Collections.Generic;

namespace Decryptor
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            DecryptableSong.decrypt_all();
        }
    }

    public class DecryptableSong
    {
        private SongEntry entry;
        private string source_path;
        private string dump_path;
        private static BepInEx.Logging.ManualLogSource logger = BepInEx.Logging.Logger.CreateLogSource("Decryptor");

        public static void decrypt_all()
        {
            System.IO.Directory.CreateDirectory("charts_st");

            var toDump = System.IO.Directory.GetFiles("Clone Hero.app/Contents/Resources/Data/StreamingAssets/songs");
            foreach (string file in toDump)
            {
                try
                {
                    var chart = new DecryptableSong(file, "charts_st");
                    chart.dump();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.ToString());
                }
            }
        }

        public static void log(string text)
        {
            logger.LogInfo(text);
        }

        public DecryptableSong(string path, string dump_dir)
        {
            log($"Loading song '{path}'...");
            source_path = path;
            entry = new SongEntry(path);
            entry.ʹʼˁʼʸʵʽʾʵʴʽ();
            dump_path = $"{dump_dir}/{entry.Name.ˁʴʿˁʾʹʶʷʵʷʵ} - {entry.Artist.ˁʴʿˁʾʹʶʷʵʷʵ}";
            System.IO.Directory.CreateDirectory(dump_path);
            log($"Song loaded, dump directory ready.");
        }

        public void dump()
        {
            log($"Beginning dump of '{entry.Name.ˁʴʿˁʾʹʶʷʵʷʵ}'...");

            log("  Dumping chart...");
            dumpChart();
            log("  Dumping album art...");
            dumpAlbumArt();
            log("  Dumping background...");
            dumpBackgroundArt();
            log("  Dumping song.ini...");
            dumpIni();
            log("  Dumping tracks...");
            dumpTracks();

            log($"  Dump complete.");
        }

        public void dumpAlbumArt()
        {
            var texture = entry.songEnc.ʵʼʴˀʾˀʼʿʲʶʴ(ʼˀʼʼʽʳˀʶʺʵˀ.ALBUM_ART);
            var album_jpg = UnityEngine.ImageConversion.EncodeToJPG(texture);
            System.IO.File.WriteAllBytes($"{dump_path}/album.jpg", album_jpg);
        }

        public void dumpBackgroundArt()
        {
            var texture = entry.songEnc.ʵʼʴˀʾˀʼʿʲʶʴ(ʼˀʼʼʽʳˀʶʺʵˀ.BACKGROUND);
            var album_jpg = UnityEngine.ImageConversion.EncodeToJPG(texture);
            System.IO.File.WriteAllBytes($"{dump_path}/background.jpg", album_jpg);
        }

        public void dumpIni()
        {
            entry.folderPath = $"{dump_path}/song.ini";
            entry.ʷʲʿʾʷʿʽʽʷʴʷ(false);
        }

        public void dumpChart()
        {
            System.IO.File.WriteAllBytes($"{dump_path}/{entry.chartName}", entry.songEnc.ʶˁʾʲʹʳʸʻʽʶʺ);
        }

        private static byte[] ReadMMFAllBytes(ref MemoryMappedFile mmf, long offset, long size)
        {
            using MemoryMappedViewStream stream = mmf.CreateViewStream(offset, size, MemoryMappedFileAccess.Read);
            MemoryStream memoryStream = new MemoryStream();
            using (DeflateStream deflateStream = new DeflateStream(stream, CompressionMode.Compress))
            {
                deflateStream.CopyTo(memoryStream);
            }

            using (BinaryReader binReader = new BinaryReader(memoryStream))
            {
                return binReader.ReadBytes((int)size);
            }
        }

        private static void dumpTrack(MemoryMappedViewAccessor stream, int length, string path)
        {
            var bytes = new byte[length];
            stream.ReadArray<byte>(0, bytes, 0, length);

            System.IO.File.WriteAllBytes(path, bytes);

        }

        public void dumpTracks()
        {
            var track_lengths = entry.songEnc.ʴˁʾʻʴʳʻʷʾʿʹ;

            for (int i = 0; i < track_lengths.Length; i++)
            {
                log($"    Dumping track {i}...");
                dumpTrack(((ʿʳʶʳʾˀʺʸʹʳʽ)entry.songEnc.ʾˀʵʿʴʶʾʼʲˁˀ[0]).ʻʶʳʶʲʾʿʹˀˁʻ, (int)track_lengths[i], $"{dump_path}/track{i.ToString()}");
            }
        }

    }
}
