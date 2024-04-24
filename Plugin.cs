using BepInEx;

using System;
using System.IO.MemoryMappedFiles;

#pragma warning disable 0168

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
                    try {
                    var chart = new DecryptableSong(file, "charts_st");
                    chart.dump();
                    } catch (System.NullReferenceException e) {
                        continue;
                    }
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

            try
            {
                source_path = path;
                entry = new SongEntry(path);
                entry.ʹʼˁʼʸʵʽʾʵʴʽ();
                dump_path = $"{dump_dir}/{entry.Name.ˁʴʿˁʾʹʶʷʵʷʵ} - {entry.Artist.ˁʴʿˁʾʹʶʷʵʷʵ}";
                System.IO.Directory.CreateDirectory(dump_path);
                log($"Song loaded, dump directory ready.");
            }
            catch (Exception e)
            {
                log("Loading song failed.");
            }
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
            try
            {
                var texture = entry.songEnc.ʵʼʴˀʾˀʼʿʲʶʴ(ʼˀʼʼʽʳˀʶʺʵˀ.ALBUM_ART);
                var album_jpg = UnityEngine.ImageConversion.EncodeToJPG(texture);
                System.IO.File.WriteAllBytes($"{dump_path}/album.jpg", album_jpg);
            }
            catch (Exception e)
            {
                log("  Dumping album art failed.");
            }
        }

        public void dumpBackgroundArt()
        {
            try
            {
                var texture = entry.songEnc.ʵʼʴˀʾˀʼʿʲʶʴ(ʼˀʼʼʽʳˀʶʺʵˀ.BACKGROUND);
                var album_jpg = UnityEngine.ImageConversion.EncodeToJPG(texture);
                System.IO.File.WriteAllBytes($"{dump_path}/background.jpg", album_jpg);
            }
            catch (Exception e)
            {
                log("  Dumping background failed.");
            }
        }

        public void dumpIni()
        {
            try
            {
                entry.folderPath = $"{dump_path}/song.ini";
                entry.ʷʲʿʾʷʿʽʽʷʴʷ(false);
            }
            catch (Exception e)
            {
                log("  Dumping song.ini failed.");
            }
        }

        public void dumpChart()
        {
            try
            {
                System.IO.File.WriteAllBytes($"{dump_path}/{entry.chartName}", entry.songEnc.ʶˁʾʲʹʳʸʻʽʶʺ);
            }
            catch (Exception e)
            {
                log($"  Dumping {entry.chartName} faild.");
            }
        }

        private static void dumpTrack(MemoryMappedViewAccessor stream, int length, string path)
        {
            try
            {
                var bytes = new byte[length];
                stream.ReadArray<byte>(0, bytes, 0, length);

                System.IO.File.WriteAllBytes(path, bytes);
            }
            catch (Exception e)
            {
                log($"    Failed.");
            }
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
