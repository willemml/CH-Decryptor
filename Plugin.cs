﻿using BepInEx;
using HarmonyLib;

using System;
using System.IO;
using System.IO.MemoryMappedFiles;

using System.Runtime.CompilerServices;

using System.Reflection;


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
            log(entry.isEnc.ToString());
            dump_path = "lol";
            dumpData();
            log(entry.Name.ˁʴʿˁʾʹʶʷʵʷʵ);
            dump_path = $"{dump_dir}/{entry.Name.ˁʴʿˁʾʹʶʷʵʷʵ} - {entry.Artist.ˁʴʿˁʾʹʶʷʵʷʵ} ({entry.Charter.ˁʴʿˁʾʹʶʷʵʷʵ}))";
            System.IO.Directory.CreateDirectory(dump_path);
            log($"Song loaded, dump directory ready.");
        }

        public void dump()
        {
            log($"Beginning dump of '{entry.Name.ˁʴʿˁʾʹʶʷʵʷʵ}'...");

            log("  Dumping song.ini...");
            dumpIni();
            log("  Dumping chart...");
            dumpChart();
            log("  Dumping data...");
            dumpData();
            log("  Dumping album art...");
            dumpAlbumArt();
            log("  Dumping background...");
            dumpBackgroundArt();

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
            entry.ʷʲʿʾʷʿʽʽʷʴʷ(entry.videoBackground);
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


        logger.LogInfo($"  Dumping file");
        var file = MemoryMappedFile.CreateFromFile(File.OpenRead($"Clone Hero.app/Contents/Resources/Data/StreamingAssets/songs/{name}.srb"), null, 0L, MemoryMappedFileAccess.Read, HandleInheritability.None, leaveOpen: false);
        using MemoryMappedViewAccessor memoryMappedViewAccessor = file.CreateViewAccessor(0L, 0L, MemoryMappedFileAccess.Read);
        long num = 0L;
        ulong ʹʼʼˀʷʽʵʹʶʶʼ = ˁʾʴʳʴʿʸˁʶʼʾ.ʾʴʻʻˁʽʻʲʶʼʾ(memoryMappedViewAccessor.ReadUInt64(num));
        num += Unsafe.SizeOf<ulong>();
        ulong num2 = ˀʴʽʾʸʻʻˁʶʺʶ.ʷʹʴʳʾʹʳʼˁʴʵ(memoryMappedViewAccessor.ReadUInt64(num), ʹʼʼˀʷʽʵʹʶʶʼ);
        logger.LogInfo($"  num2 = {num2}");
        num += Unsafe.SizeOf<ulong>();
        var ʿˀʺʷʼʾʳˀʹʳˁ = new ʽʼʲʴʲˀʸʻʸʲʹ();
        byte[] array = new byte[num2];
        data.ʿʷʽʻʸʾʲʼʾʲʼ(memoryMappedViewAccessor, array, (uint)num2, ref num);
        array = ʲʲʹʻʿʳʿʻʼʴʾ.ʺʼʳʵʻʺʻʿʹʵʻ(array);
        int ʻʴʲʳʴʽʹʸʵʸʼ = 0;
        var ˁʳʵʻʺʴʹʲʵʴʾ = array.weirdM1(ref ʻʴʲʳʴʽʹʸʵʸʼ);
        if (ˁʳʵʻʺʴʹʲʵʴʾ == 20210228)
        {
            ʿˀʺʷʼʾʳˀʹʳˁ.ʼˀʸʳʵˁʵʽˀʿʴ(array, ref ʻʴʲʳʴʽʹʸʵʸʼ);
            long num3 = array.weirdM2(ref ʻʴʲʳʴʽʹʸʵʸʼ);
            logger.LogInfo($"  num3 = {num3}");
            int num4 = array.weirdM4(ref ʻʴʲʳʴʽʹʸʵʸʼ);
            logger.LogInfo($"  num4 = {num4}");
            var ʹʹʻˁʽʻʹʼʹʹʷ = (ʼˀʼʼʽʳˀʶʺʵˀ)array.weirdM5(ref ʻʴʲʳʴʽʹʸʵʸʼ);
            var ʽʶʵˁʼʲʳʲʴʻʳ = array.weirdM4(ref ʻʴʲʳʴʽʹʸʵʸʼ);

            int ʻʴʷʹʷˁʸʵʵʿʴ;
            int ʳʲʹˀʳʼʴʽʳˁʼ;

            if ((ʹʹʻˁʽʻʹʼʹʹʷ & ʼˀʼʼʽʳˀʶʺʵˀ.ALBUM_ART) != 0)
            {
                ʻʴʷʹʷˁʸʵʵʿʴ = array.weirdM4(ref ʻʴʲʳʴʽʹʸʵʸʼ);
            }
            if ((ʹʹʻˁʽʻʹʼʹʹʷ & ʼˀʼʼʽʳˀʶʺʵˀ.BACKGROUND) != 0)
            {
                ʳʲʹˀʳʼʴʽʳˁʼ = array.weirdM4(ref ʻʴʲʳʴʽʹʸʵʸʼ);
            }
            var ʾʵʻʾʶʼʸʻʳʺʾ = new ulong[ʽʶʵˁʼʲʳʲʴʻʳ];
            if (ʽʶʵˁʼʲʳʲʴʻʳ > 0)
            {
                array.weirdM6(ref ʻʴʲʳʴʽʹʸʵʸʼ, ʾʵʻʾʶʼʸʻʳʺʾ, (uint)ʽʶʵˁʼʲʳʲʴʻʳ);
            }
            var ʵʼʺˁʹʵʽʻʳʽʽ = new ulong[num4];
            array.weirdM6(ref ʻʴʲʳʴʽʹʸʵʸʼ, ʵʼʺˁʹʵʽʻʳʽʽ, (uint)num4);
            var ʴˁʾʻʴʳʻʷʾʿʹ = new ulong[num4];
            array.weirdM6(ref ʻʴʲʳʴʽʹʸʵʸʼ, ʴˁʾʻʴʳʻʷʾʿʹ, (uint)num4);
            var ʶˁʾʲʹʳʸʻʽʶʺ = new byte[num3];
            data.ʿʷʽʻʸʾʲʼʾʲʼ(memoryMappedViewAccessor, ʶˁʾʲʹʳʸʻʽʶʺ, (uint)num3, ref num);
            ʶˁʾʲʹʳʸʻʽʶʺ = ʲʲʹʻʿʳʿʻʼʴʾ.ʺʼʳʵʻʺʻʿʹʵʻ(ʶˁʾʲʹʳʸʻʽʶʺ);
            var ʹʶˁʷʺʸʴʹʼʸʸ = (uint)num;
            for (int i = 0; i < ʽʶʵˁʼʲʳʲʴʻʳ; i++)
            {
                num += (long)ʾʵʻʾʶʼʸʻʳʺʾ[i];
            }
            var ʾˀʵʿʴʶʾʼʲˁˀ = new ʳˁʿʶʵʺʻʺʿʽʴ[num4];
            for (int j = 0; j < num4; j++)
            {
                ulong num5 = ʴˁʾʻʴʳʻʷʾʿʹ[j];
                logger.LogInfo($"  num5 = {num5}");
                checked
                {
                    ʾˀʵʿʴʶʾʼʲˁˀ[j] = new ʿʳʶʳʾˀʺʸʹʳʽ(file, num, (long)num5);
                    num += (long)num5;
                }
            }
            logger.LogInfo("  block!");
        }



        System.IO.File.WriteAllBytes($"{dump_dir}/data.srb", array);


        logger.LogInfo($"  Chart fully decrypted/dumped");
    }

}


public static class WeirdM1
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint weirdM1(this byte[] ʳʼʾʺˀʽʿʳʹʷʽ, int ʻʴʲʳʴʽʹʸʵʸʼ)
    {
        return Unsafe.As<byte, uint>(ref ʳʼʾʺˀʽʿʳʹʷʽ[ʻʴʲʳʴʽʹʸʵʸʼ]);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static uint weirdM1(this ReadOnlySpan<byte> ʳʼʾʺˀʽʿʳʹʷʽ, int ʻʴʲʳʴʽʹʸʵʸʼ)
    {
        fixed (byte* ʳʼʾʺˀʽʿʳʹʷʽ2 = ʳʼʾʺˀʽʿʳʹʷʽ)
        {
            return weirdM1(ʳʼʾʺˀʽʿʳʹʷʽ2, ʻʴʲʳʴʽʹʸʵʸʼ);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint weirdM1(this ReadOnlySpan<byte> ʳʼʾʺˀʽʿʳʹʷʽ, ref int ʻʴʲʳʴʽʹʸʵʸʼ)
    {
        uint result = ʳʼʾʺˀʽʿʳʹʷʽ.weirdM1(ʻʴʲʳʴʽʹʸʵʸʼ);
        ʻʴʲʳʴʽʹʸʵʸʼ += 4;
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint weirdM1(this byte[] ʳʼʾʺˀʽʿʳʹʷʽ, ref int ʻʴʲʳʴʽʹʸʵʸʼ)
    {
        uint result = ʳʼʾʺˀʽʿʳʹʷʽ.weirdM1(ʻʴʲʳʴʽʹʸʵʸʼ);
        ʻʴʲʳʴʽʹʸʵʸʼ += 4;
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static uint weirdM1(this Span<byte> ʳʼʾʺˀʽʿʳʹʷʽ, int ʻʴʲʳʴʽʹʸʵʸʼ)
    {
        fixed (byte* ʳʼʾʺˀʽʿʳʹʷʽ2 = ʳʼʾʺˀʽʿʳʹʷʽ)
        {
            return weirdM1(ʳʼʾʺˀʽʿʳʹʷʽ2, ʻʴʲʳʴʽʹʸʵʸʼ);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint weirdM1(this Span<byte> ʳʼʾʺˀʽʿʳʹʷʽ, ref int ʻʴʲʳʴʽʹʸʵʸʼ)
    {
        uint result = ʳʼʾʺˀʽʿʳʹʷʽ.weirdM1(ʻʴʲʳʴʽʹʸʵʸʼ);
        ʻʴʲʳʴʽʹʸʵʸʼ += 4;
        return result;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static uint weirdM1(byte* ʳʼʾʺˀʽʿʳʹʷʽ, int ʻʴʲʳʴʽʹʸʵʸʼ)
    {
        return Unsafe.As<byte, uint>(ref ʳʼʾʺˀʽʿʳʹʷʽ[ʻʴʲʳʴʽʹʸʵʸʼ]);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static uint weirdM1(byte* ʳʼʾʺˀʽʿʳʹʷʽ, ref int ʻʴʲʳʴʽʹʸʵʸʼ)
    {
        uint result = weirdM1(ʳʼʾʺˀʽʿʳʹʷʽ, ʻʴʲʳʴʽʹʸʵʸʼ);
        ʻʴʲʳʴʽʹʸʵʸʼ += 4;
        return result;
    }
}

public static class WeirdM2
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long weirdM2(this byte[] ʳʼʾʺˀʽʿʳʹʷʽ, int ʻʴʲʳʴʽʹʸʵʸʼ)
    {
        return (long)ʳʼʾʺˀʽʿʳʹʷʽ.weirdM3(ʻʴʲʳʴʽʹʸʵʸʼ);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static long weirdM2(this ReadOnlySpan<byte> ʳʼʾʺˀʽʿʳʹʷʽ, int ʻʴʲʳʴʽʹʸʵʸʼ)
    {
        fixed (byte* ʳʼʾʺˀʽʿʳʹʷʽ2 = ʳʼʾʺˀʽʿʳʹʷʽ)
        {
            return weirdM2(ʳʼʾʺˀʽʿʳʹʷʽ2, ʻʴʲʳʴʽʹʸʵʸʼ);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long weirdM2(this byte[] ʳʼʾʺˀʽʿʳʹʷʽ, ref int ʻʴʲʳʴʽʹʸʵʸʼ)
    {
        long result = ʳʼʾʺˀʽʿʳʹʷʽ.weirdM2(ʻʴʲʳʴʽʹʸʵʸʼ);
        ʻʴʲʳʴʽʹʸʵʸʼ += 8;
        return result;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static long weirdM2(this Span<byte> ʳʼʾʺˀʽʿʳʹʷʽ, int ʻʴʲʳʴʽʹʸʵʸʼ)
    {
        fixed (byte* ʳʼʾʺˀʽʿʳʹʷʽ2 = ʳʼʾʺˀʽʿʳʹʷʽ)
        {
            return weirdM2(ʳʼʾʺˀʽʿʳʹʷʽ2, ʻʴʲʳʴʽʹʸʵʸʼ);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long weirdM2(this Span<byte> ʳʼʾʺˀʽʿʳʹʷʽ, ref int ʻʴʲʳʴʽʹʸʵʸʼ)
    {
        long result = ʳʼʾʺˀʽʿʳʹʷʽ.weirdM2(ʻʴʲʳʴʽʹʸʵʸʼ);
        ʻʴʲʳʴʽʹʸʵʸʼ += 8;
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static long weirdM2(byte* ʳʼʾʺˀʽʿʳʹʷʽ, int ʻʴʲʳʴʽʹʸʵʸʼ)
    {
        return (long)WeirdM3.weirdM3(ʳʼʾʺˀʽʿʳʹʷʽ, ʻʴʲʳʴʽʹʸʵʸʼ);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static long weirdM2(byte* ʳʼʾʺˀʽʿʳʹʷʽ, ref int ʻʴʲʳʴʽʹʸʵʸʼ)
    {
        long result = weirdM2(ʳʼʾʺˀʽʿʳʹʷʽ, ʻʴʲʳʴʽʹʸʵʸʼ);
        ʻʴʲʳʴʽʹʸʵʸʼ += 8;
        return result;
    }
}

public static class WeirdM3
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong weirdM3(this byte[] ʳʼʾʺˀʽʿʳʹʷʽ, int ʻʴʲʳʴʽʹʸʵʸʼ)
    {
        return Unsafe.As<byte, ulong>(ref ʳʼʾʺˀʽʿʳʹʷʽ[ʻʴʲʳʴʽʹʸʵʸʼ]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static ulong weirdM3(this ReadOnlySpan<byte> ʳʼʾʺˀʽʿʳʹʷʽ, int ʻʴʲʳʴʽʹʸʵʸʼ)
    {
        fixed (byte* ʳʼʾʺˀʽʿʳʹʷʽ2 = ʳʼʾʺˀʽʿʳʹʷʽ)
        {
            return weirdM3(ʳʼʾʺˀʽʿʳʹʷʽ2, ʻʴʲʳʴʽʹʸʵʸʼ);
        }
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong weirdM3(this ReadOnlySpan<byte> ʳʼʾʺˀʽʿʳʹʷʽ, ref int ʻʴʲʳʴʽʹʸʵʸʼ)
    {
        ulong result = ʳʼʾʺˀʽʿʳʹʷʽ.weirdM3(ʻʴʲʳʴʽʹʸʵʸʼ);
        ʻʴʲʳʴʽʹʸʵʸʼ += 8;
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong weirdM3(this byte[] ʳʼʾʺˀʽʿʳʹʷʽ, ref int ʻʴʲʳʴʽʹʸʵʸʼ)
    {
        ulong result = ʳʼʾʺˀʽʿʳʹʷʽ.weirdM3(ʻʴʲʳʴʽʹʸʵʸʼ);
        ʻʴʲʳʴʽʹʸʵʸʼ += 8;
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static ulong weirdM3(this Span<byte> ʳʼʾʺˀʽʿʳʹʷʽ, int ʻʴʲʳʴʽʹʸʵʸʼ)
    {
        fixed (byte* ʳʼʾʺˀʽʿʳʹʷʽ2 = ʳʼʾʺˀʽʿʳʹʷʽ)
        {
            return weirdM3(ʳʼʾʺˀʽʿʳʹʷʽ2, ʻʴʲʳʴʽʹʸʵʸʼ);
        }
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong weirdM3(this Span<byte> ʳʼʾʺˀʽʿʳʹʷʽ, ref int ʻʴʲʳʴʽʹʸʵʸʼ)
    {
        ulong result = ʳʼʾʺˀʽʿʳʹʷʽ.weirdM3(ʻʴʲʳʴʽʹʸʵʸʼ);
        ʻʴʲʳʴʽʹʸʵʸʼ += 8;
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static ulong weirdM3(byte* ʳʼʾʺˀʽʿʳʹʷʽ, int ʻʴʲʳʴʽʹʸʵʸʼ)
    {
        return Unsafe.As<byte, ulong>(ref ʳʼʾʺˀʽʿʳʹʷʽ[ʻʴʲʳʴʽʹʸʵʸʼ]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static ulong weirdM3(byte* ʳʼʾʺˀʽʿʳʹʷʽ, ref int ʻʴʲʳʴʽʹʸʵʸʼ)
    {
        ulong result = weirdM3(ʳʼʾʺˀʽʿʳʹʷʽ, ʻʴʲʳʴʽʹʸʵʸʼ);
        ʻʴʲʳʴʽʹʸʵʸʼ += 8;
        return result;
    }
}

public static class WeirdM4
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int weirdM4(this byte[] ʳʼʾʺˀʽʿʳʹʷʽ, int ʻʴʲʳʴʽʹʸʵʸʼ)
    {
        return (int)ʳʼʾʺˀʽʿʳʹʷʽ.weirdM1(ʻʴʲʳʴʽʹʸʵʸʼ);
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static int weirdM4(this ReadOnlySpan<byte> ʳʼʾʺˀʽʿʳʹʷʽ, int ʻʴʲʳʴʽʹʸʵʸʼ)
    {
        fixed (byte* ʳʼʾʺˀʽʿʳʹʷʽ2 = ʳʼʾʺˀʽʿʳʹʷʽ)
        {
            return weirdM4(ʳʼʾʺˀʽʿʳʹʷʽ2, ʻʴʲʳʴʽʹʸʵʸʼ);
        }
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int weirdM4(this ReadOnlySpan<byte> ʳʼʾʺˀʽʿʳʹʷʽ, ref int ʻʴʲʳʴʽʹʸʵʸʼ)
    {
        int result = ʳʼʾʺˀʽʿʳʹʷʽ.weirdM4(ʻʴʲʳʴʽʹʸʵʸʼ);
        ʻʴʲʳʴʽʹʸʵʸʼ += 4;
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int weirdM4(this byte[] ʳʼʾʺˀʽʿʳʹʷʽ, ref int ʻʴʲʳʴʽʹʸʵʸʼ)
    {
        int result = ʳʼʾʺˀʽʿʳʹʷʽ.weirdM4(ʻʴʲʳʴʽʹʸʵʸʼ);
        ʻʴʲʳʴʽʹʸʵʸʼ += 4;
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static int weirdM4(this Span<byte> ʳʼʾʺˀʽʿʳʹʷʽ, int ʻʴʲʳʴʽʹʸʵʸʼ)
    {
        fixed (byte* ʳʼʾʺˀʽʿʳʹʷʽ2 = ʳʼʾʺˀʽʿʳʹʷʽ)
        {
            return weirdM4(ʳʼʾʺˀʽʿʳʹʷʽ2, ʻʴʲʳʴʽʹʸʵʸʼ);
        }
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int weirdM4(this Span<byte> ʳʼʾʺˀʽʿʳʹʷʽ, ref int ʻʴʲʳʴʽʹʸʵʸʼ)
    {
        int result = ʳʼʾʺˀʽʿʳʹʷʽ.weirdM4(ʻʴʲʳʴʽʹʸʵʸʼ);
        ʻʴʲʳʴʽʹʸʵʸʼ += 4;
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static int weirdM4(byte* ʳʼʾʺˀʽʿʳʹʷʽ, int ʻʴʲʳʴʽʹʸʵʸʼ)
    {
        return (int)WeirdM1.weirdM1(ʳʼʾʺˀʽʿʳʹʷʽ, ʻʴʲʳʴʽʹʸʵʸʼ);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static int weirdM4(byte* ʳʼʾʺˀʽʿʳʹʷʽ, ref int ʻʴʲʳʴʽʹʸʵʸʼ)
    {
        int result = weirdM4(ʳʼʾʺˀʽʿʳʹʷʽ, ʻʴʲʳʴʽʹʸʵʸʼ);
        ʻʴʲʳʴʽʹʸʵʸʼ += 4;
        return result;
    }
}

public static class WeirdM5
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte weirdM5(this byte[] ʳʼʾʺˀʽʿʳʹʷʽ, int ʻʴʲʳʴʽʹʸʵʸʼ)
    {
        return ʳʼʾʺˀʽʿʳʹʷʽ[ʻʴʲʳʴʽʹʸʵʸʼ];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static byte weirdM5(this ReadOnlySpan<byte> ʳʼʾʺˀʽʿʳʹʷʽ, int ʻʴʲʳʴʽʹʸʵʸʼ)
    {
        fixed (byte* ʳʼʾʺˀʽʿʳʹʷʽ2 = ʳʼʾʺˀʽʿʳʹʷʽ)
        {
            return weirdM5(ʳʼʾʺˀʽʿʳʹʷʽ2, ʻʴʲʳʴʽʹʸʵʸʼ);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte weirdM5(this ReadOnlySpan<byte> ʳʼʾʺˀʽʿʳʹʷʽ, ref int ʻʴʲʳʴʽʹʸʵʸʼ)
    {
        return ʳʼʾʺˀʽʿʳʹʷʽ[ʻʴʲʳʴʽʹʸʵʸʼ++];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte weirdM5(this byte[] ʳʼʾʺˀʽʿʳʹʷʽ, ref int ʻʴʲʳʴʽʹʸʵʸʼ)
    {
        return ʳʼʾʺˀʽʿʳʹʷʽ[ʻʴʲʳʴʽʹʸʵʸʼ++];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static byte weirdM5(this Span<byte> ʳʼʾʺˀʽʿʳʹʷʽ, int ʻʴʲʳʴʽʹʸʵʸʼ)
    {
        fixed (byte* ʳʼʾʺˀʽʿʳʹʷʽ2 = ʳʼʾʺˀʽʿʳʹʷʽ)
        {
            return weirdM5(ʳʼʾʺˀʽʿʳʹʷʽ2, ʻʴʲʳʴʽʹʸʵʸʼ);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte weirdM5(this Span<byte> ʳʼʾʺˀʽʿʳʹʷʽ, ref int ʻʴʲʳʴʽʹʸʵʸʼ)
    {
        return ʳʼʾʺˀʽʿʳʹʷʽ[ʻʴʲʳʴʽʹʸʵʸʼ++];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static byte weirdM5(byte* ʳʼʾʺˀʽʿʳʹʷʽ, int ʻʴʲʳʴʽʹʸʵʸʼ)
    {
        return ʳʼʾʺˀʽʿʳʹʷʽ[ʻʴʲʳʴʽʹʸʵʸʼ];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static byte weirdM5(byte* ʳʼʾʺˀʽʿʳʹʷʽ, ref int ʻʴʲʳʴʽʹʸʵʸʼ)
    {
        return ʳʼʾʺˀʽʿʳʹʷʽ[ʻʴʲʳʴʽʹʸʵʸʼ++];
    }
}

public static class WeirdM6
{
    public unsafe static int weirdM6<T>(this byte[] ʳʼʾʺˀʽʿʳʹʷʽ, int ʻʴʲʳʴʽʹʸʵʸʼ, T[] ˀʳʹʷʵˀʲʿʽʺʹ, uint ʶˀʿʷʴʻʶˁʶʿʿ) where T : unmanaged
    {
        int num = Unsafe.SizeOf<T>() * (int)ʶˀʿʷʴʻʶˁʶʿʿ;
        byte* source = (byte*)Unsafe.AsPointer(ref ˀʳʹʷʵˀʲʿʽʺʹ[0]);
        Unsafe.CopyBlockUnaligned(ref Unsafe.AsRef<byte>(source), ref ʳʼʾʺˀʽʿʳʹʷʽ[ʻʴʲʳʴʽʹʸʵʸʼ], (uint)num);
        return num;
    }

    public unsafe static int weirdM6<T>(this ReadOnlySpan<byte> ʳʼʾʺˀʽʿʳʹʷʽ, int ʻʴʲʳʴʽʹʸʵʸʼ, T[] ˀʳʹʷʵˀʲʿʽʺʹ, uint ʶˀʿʷʴʻʶˁʶʿʿ) where T : unmanaged
    {
        fixed (byte* ʳʼʾʺˀʽʿʳʹʷʽ2 = ʳʼʾʺˀʽʿʳʹʷʽ)
        {
            return weirdM6(ʳʼʾʺˀʽʿʳʹʷʽ2, ʻʴʲʳʴʽʹʸʵʸʼ, ˀʳʹʷʵˀʲʿʽʺʹ, ʶˀʿʷʴʻʶˁʶʿʿ);
        }
    }

    public static void weirdM6<T>(this ReadOnlySpan<byte> ʳʼʾʺˀʽʿʳʹʷʽ, ref int ʻʴʲʳʴʽʹʸʵʸʼ, T[] ˀʳʹʷʵˀʲʿʽʺʹ, uint ʶˀʿʷʴʻʶˁʶʿʿ) where T : unmanaged
    {
        ʻʴʲʳʴʽʹʸʵʸʼ += ʳʼʾʺˀʽʿʳʹʷʽ.weirdM6(ʻʴʲʳʴʽʹʸʵʸʼ, ˀʳʹʷʵˀʲʿʽʺʹ, ʶˀʿʷʴʻʶˁʶʿʿ);
    }


    public static void weirdM6<T>(this byte[] ʽʽʿʿʾʲʵʽʼʷˀ, ref int ʻʴʲʳʴʽʹʸʵʸʼ, T[] ˀʳʹʷʵˀʲʿʽʺʹ, uint ʶˀʿʷʴʻʶˁʶʿʿ) where T : unmanaged
    {
        ʻʴʲʳʴʽʹʸʵʸʼ += ʽʽʿʿʾʲʵʽʼʷˀ.weirdM6(ʻʴʲʳʴʽʹʸʵʸʼ, ˀʳʹʷʵˀʲʿʽʺʹ, ʶˀʿʷʴʻʶˁʶʿʿ);
    }


    public unsafe static int weirdM6<T>(this Span<byte> ʳʼʾʺˀʽʿʳʹʷʽ, int ʻʴʲʳʴʽʹʸʵʸʼ, T[] ˀʳʹʷʵˀʲʿʽʺʹ, uint ʶˀʿʷʴʻʶˁʶʿʿ) where T : unmanaged
    {
        fixed (byte* ʳʼʾʺˀʽʿʳʹʷʽ2 = ʳʼʾʺˀʽʿʳʹʷʽ)
        {
            return weirdM6(ʳʼʾʺˀʽʿʳʹʷʽ2, ʻʴʲʳʴʽʹʸʵʸʼ, ˀʳʹʷʵˀʲʿʽʺʹ, ʶˀʿʷʴʻʶˁʶʿʿ);
        }
    }

    public static void weirdM6<T>(this Span<byte> ʳʼʾʺˀʽʿʳʹʷʽ, ref int ʻʴʲʳʴʽʹʸʵʸʼ, T[] ˀʳʹʷʵˀʲʿʽʺʹ, uint ʶˀʿʷʴʻʶˁʶʿʿ) where T : unmanaged
    {
        ʻʴʲʳʴʽʹʸʵʸʼ += ʳʼʾʺˀʽʿʳʹʷʽ.weirdM6(ʻʴʲʳʴʽʹʸʵʸʼ, ˀʳʹʷʵˀʲʿʽʺʹ, ʶˀʿʷʴʻʶˁʶʿʿ);
    }


    public unsafe static int weirdM6<T>(byte* ʳʼʾʺˀʽʿʳʹʷʽ, int ʻʴʲʳʴʽʹʸʵʸʼ, T[] ˀʳʹʷʵˀʲʿʽʺʹ, uint ʶˀʿʷʴʻʶˁʶʿʿ) where T : unmanaged
    {
        uint num = (uint)Unsafe.SizeOf<T>() * ʶˀʿʷʴʻʶˁʶʿʿ;
        byte* source = (byte*)Unsafe.AsPointer(ref ˀʳʹʷʵˀʲʿʽʺʹ[0]);
        Unsafe.CopyBlockUnaligned(ref Unsafe.AsRef<byte>(source), ref ʳʼʾʺˀʽʿʳʹʷʽ[ʻʴʲʳʴʽʹʸʵʸʼ], num);
        return (int)num;
    }

    public unsafe static void weirdM6<T>(byte* ʳʼʾʺˀʽʿʳʹʷʽ, ref int ʻʴʲʳʴʽʹʸʵʸʼ, T[] ˀʳʹʷʵˀʲʿʽʺʹ, uint ʶˀʿʷʴʻʶˁʶʿʿ) where T : unmanaged
    {
        ʻʴʲʳʴʽʹʸʵʸʼ += weirdM6(ʳʼʾʺˀʽʿʳʹʷʽ, ʻʴʲʳʴʽʹʸʵʸʼ, ˀʳʹʷʵˀʲʿʽʺʹ, ʶˀʿʷʴʻʶˁʶʿʿ);
    }
}
