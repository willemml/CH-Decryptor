using BepInEx;
using HarmonyLib;

using System;
using System.IO;
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
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            Harmony.CreateAndPatchAll(typeof(SongEntryWriteIniPatch));
            Logger.LogInfo("Patched encrypted song loading");
            System.IO.Directory.CreateDirectory("charts");
            Logger.LogInfo("Created dump directory");

            DecryptableSong.decrypt_all();
        }
    }

    public class DecryptableSong
    {
        private SongEntry entry;
        private string dump_path;
        private static BepInEx.Logging.ManualLogSource logger = BepInEx.Logging.Logger.CreateLogSource("Decryptor");

        public static void decrypt_all()
        {
            System.IO.Directory.CreateDirectory("charts_st");

			//(new GlobalVariables()).Awake();

			//GlobalVariables.ʷʲʺʷʻʳʾʶˁˀʷ.ʸʲʹʴʲʷʴʳʹʶʿ = new ʵʴʺˁʵʽʾʹʸʹˀ();

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

        private void log(string text)
        {
            logger.LogInfo(text);
        }

        public DecryptableSong(string path, string dump_dir)
        {
            log($"Loading song '{path}'...");
            entry = new SongEntry(path);
			entry.ʹʼˁʼʸʵʽʾʵʴʽ();
			//GlobalVariables.ʷʲʺʷʻʳʾʶˁˀʷ.ʸʲʹʴʲʷʴʳʹʶʿ.ˁʽʳʹʾʹʺʳʳʿˀ(entry);
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
            //log("  Dumping data...");
            //dumpData();

            log($"Dump complete.");
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

        private static byte[] ReadMMFAllBytes(ref MemoryMappedFile mmf)
        {
            using (var stream = mmf.CreateViewStream())
            {
                using (BinaryReader binReader = new BinaryReader(stream))
                {
                    return binReader.ReadBytes((int)stream.Length);
                }
            }
        }

        public void dumpData()
        {
            var bytes = ReadMMFAllBytes(ref entry.songEnc.ʻʵʳʸʴʺˁˀʵʿʴ);
            System.IO.File.WriteAllBytes($"{dump_path}/dumped", bytes);
        }

    }
}


class Patches
{
    //[HarmonyPatch(typeof(SongEntry), "iniPath", MethodType.Getter)]
    //[HarmonyWrapSafe]
    //[HarmonyPostfix]
    static void iniPath(ref SongEntry __instance, ref string __result)
    {
        var entry = __instance;
        var name = entry.Name.ˁʴʿˁʾʹʶʷʵʷʵ;
        var artist = entry.Artist.ˁʴʿˁʾʹʶʷʵʷʵ;
        var charter = entry.Charter.ˁʴʿˁʾʹʶʷʵʷʵ;
        var data = entry.songEnc;
        var chart = data.ʶˁʾʲʹʳʸʻʽʶʺ;

        var dump_dir = $"charts/{name}";

        __result = $"{dump_dir}/song.ini";
    }

    //[HarmonyPatch(typeof(SongEntry), "ˁʿʹʶʽʲʺʻˁʼʸ")]
    //[HarmonyWrapSafe]
    //[HarmonyPostfix]
    static void songLoaded(ref SongEntry __instance)
    {
        
        var logger = BepInEx.Logging.Logger.CreateLogSource("Decryptor");
        var entry = __instance;
        var name = entry.Name.ˁʴʿˁʾʹʶʷʵʷʵ;
        var artist = entry.Artist.ˁʴʿˁʾʹʶʷʵʷʵ;
        var charter = entry.Charter.ˁʴʿˁʾʹʶʷʵʷʵ;
        var data = entry.songEnc;
        var chart = data.ʶˁʾʲʹʳʸʻʽʶʺ;

        var dump_dir = $"charts/{name}";
        System.IO.Directory.CreateDirectory(dump_dir);


        logger.LogInfo($"Loaded encrypted song '{name}'");
        logger.LogInfo($"  Dumping chart data to {dump_dir}");

        logger.LogInfo($"  Writing metadata to ini file");
        entry.ʷʲʿʾʷʿʽʽʷʴʷ(false);

        logger.LogInfo($"  Dumping chart MIDI data");
        System.IO.File.WriteAllBytes($"{dump_dir}/notes.mid", chart);

        //var has_art = data.ʴʲʼʹʴʶʷʾʺʴʹ(ʼˀʼʼʽʳˀʶʺʵˀ.ALBUM_ART);
        //logger.LogInfo($"  Dumping album art ({has_art})");
        //var a_texture = new Texture2D(512, 512, GraphicsFormat.R8G8B8A8_SRGB, TextureCreationFlags.None);
        //a_texture.wrapMode = TextureWrapMode.Clamp;
        //NativeArray<Color32> nativeTextureData = a_texture.GetRawTextureData<Color32>();
        //data.ʵʼʴˀʾˀʼʿʲʶʴ(ʼˀʼʼʽʳˀʶʺʵˀ.ALBUM_ART, nativeTextureData, 512, 512);
        //var album_jpg = UnityEngine.ImageConversion.EncodeToJPG(a_texture);
        //System.IO.File.WriteAllBytes($"{dump_dir}/album.jpg", album_jpg);

        /* logger.LogInfo("  Recreating EncryptedSong");
         var songEnc = new ʺʻʻˁˀʹʿʲʻʴʿ($"Clone Hero.app/Contents/Resources/Data/StreamingAssets/songs/{name}.srb");
         var texture = songEnc.ʵʼʴˀʾˀʼʿʲʶʴ(ʼˀʼʼʽʳˀʶʺʵˀ.ALBUM_ART);
         var album_jpg = UnityEngine.ImageConversion.EncodeToJPG(texture);
         System.IO.File.WriteAllBytes($"{dump_dir}/album.jpg", album_jpg);
 */

   
        logger.LogInfo($"  Chart fully decrypted/dumped");
    }

}

[HarmonyPatch(typeof(SongEntry))]
[HarmonyPatch("ʷʲʿʾʷʿʽʽʷʴʷ")]
public static class SongEntryWriteIniPatch {
	static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
	{
		var codes = new List<CodeInstruction>(instructions);
		for (int i = 0; i < codes.Count; i++)
		{
			if (i >= 0x02a2 && i <= 0x02bc)
			{
			codes[i].opcode = OpCodes.Nop;
			}
			}
		return codes;
	}
}
