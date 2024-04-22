using BepInEx;
using System;
using HarmonyLib;

namespace Decryptor
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            Harmony.CreateAndPatchAll(typeof(Patches));
            Logger.LogInfo("Patched encrypted song loading");
            System.IO.Directory.CreateDirectory("charts");
            Logger.LogInfo("Created dump directory");
        }
    }
}

class Patches {
    [HarmonyPatch(typeof(SongEntry), "iniPath", MethodType.Getter)]
    [HarmonyWrapSafe]
    [HarmonyPostfix]
    static void iniPath(ref SongEntry __instance, ref string __result) {
        var entry = __instance;
        var name = entry.Name.ˁʴʿˁʾʹʶʷʵʷʵ;
        var artist = entry.Artist.ˁʴʿˁʾʹʶʷʵʷʵ;
        var charter = entry.Charter.ˁʴʿˁʾʹʶʷʵʷʵ;
        var data = entry.songEnc;
        var chart = data.ʶˁʾʲʹʳʸʻʽʶʺ;

        var dump_dir = $"charts/{name}";

        __result = $"{dump_dir}/song.ini";
    }
    
    [HarmonyPatch(typeof(SongEntry), "ˁʿʹʶʽʲʺʻˁʼʸ")]
    [HarmonyWrapSafe]
    [HarmonyPostfix]
    static void songLoaded(ref SongEntry __instance) {
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

		logger.LogInfo($"  Dumping album art");
		var album_jpg = ImageConversion.EncodeToJPG(data.ˁʻʳʴˁʵʶʶʶʼʻ(ʼˀʼʼʽʳˀʶʺʵˀ.ALBUM_ART));
        
        logger.LogInfo($"  Chart fully decrypted/dumped");
    }
}
