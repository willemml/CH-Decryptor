using BepInEx;

using System;
using System.Linq;
using System.Collections.Generic;

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
        private static string[] track_names = BassAudioManager.ʷʼˁˀʾʸʴʳʺʵˁ;

        private static BepInEx.Logging.ManualLogSource logger = BepInEx.Logging.Logger.CreateLogSource("Decryptor");

        public static void decrypt_all()
        {
            System.IO.Directory.CreateDirectory("charts");

            var toDump = System.IO.Directory.GetFiles("songs");
            foreach (string file in toDump)
            {
                try
                {
                    try
                    {
                        var chart = new DecryptableSong(file, "charts");
                        chart.dump();
                    }
                    catch (System.NullReferenceException e)
                    {
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
                dump_path = $"{dump_dir}/{entry.Artist.ˁʴʿˁʾʹʶʷʵʷʵ} - {entry.Name.ˁʴʿˁʾʹʶʷʵʷʵ}";
                System.IO.Directory.CreateDirectory(dump_path);
                log($"Song loaded, dump directory ready.");
            }
            catch (Exception e)
            {
                log("Loading song failed.");
                log(e.ToString());
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
            log("  Dumping tracks...");
            dumpTracks();
            log("  Dumping song.ini...");
            dumpIni();

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
                log("    Dumping album art failed.");
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
                log("    Dumping background failed.");
                log(e.ToString());
            }
        }

        public void dumpIni()
        {
            try
            {
                if (entry.songLength <= 0) {
                    entry.songLength = 1;
                }
                entry.folderPath = $"{dump_path}/song.ini";
                entry.ʷʲʿʾʷʿʽʽʷʴʷ(false);
            }
            catch (Exception e)
            {
                log($"    Song length: {entry.songLength.ToString()}");
                log("    Dumping song.ini failed.");
                log(e.ToString());
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
                log($"    Dumping {entry.chartName} faild.");
            }
        }
        public void dumpTracks()
        {
            var track_lengths = new int[14];
            var track_streams = new ʳˁʿʶʵʺʻʺʿʽʴ[14];

            IEnumerable<Tuple<ʳˁʿʶʵʺʻʺʿʽʴ, ulong>> encrypted_streams = entry.songEnc.ʾˀʵʿʴʶʾʼʲˁˀ.Zip(entry.songEnc.ʴˁʾʻʴʳʻʷʾʿʹ, Tuple.Create);

            foreach ((ʳˁʿʶʵʺʻʺʿʽʴ track_stream, ulong length) in encrypted_streams)
            {
                var index = track_stream.ʾʷʽʼʴˁˁʹʳʴʴ;
                track_lengths[index] = (int)length;

                track_stream.ʾʴʵʵʴʿʾʷʴʲˀ();
                track_streams[index] = track_stream;
            }

            for (int i = 0; i < track_streams.Length; i++)
            {
                if (track_streams[i] != null)
                {
                    log($"    Dumping track '{track_names[i]}'...");
                    var bytes = new byte[track_lengths[i]];
                    unsafe
                    {
                        fixed (byte* ptr = &bytes[0])
                            ((ʿʳʶʳʾˀʺʸʹʳʽ)track_streams[i]).ʺʳˀʾʴʵʷʶʼʶʹ(ptr, bytes.Length);
                    }
                    System.IO.File.WriteAllBytes($"{dump_path}/{track_names[i]}.opus", bytes);
                }
            }
        }
    }
}
