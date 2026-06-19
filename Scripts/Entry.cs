using System.Reflection;
using Godot.Bridge;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using STS2RitsuLib;
using STS2RitsuLib.Interop;

namespace Summoner.Scripts;

[ModInitializer(nameof(Init))]
public class Entry
{
    public const string ModId = "Summoner";
    public static readonly Logger Logger = RitsuLibFramework.CreateLogger(ModId);
    public static void Init()
    {
        /*
        var harmony = new Harmony("Vesperi.Summoner");
        harmony.PatchAll();
        ScriptManagerBridge.LookupScriptsInAssembly(typeof(Entry).Assembly);
        Log.Info("Summoner Mod initialized!");
        */
        
        var assembly = Assembly.GetExecutingAssembly();
        RitsuLibFramework.EnsureGodotScriptsRegistered(assembly, Logger);
        // 自动注册内容
        ModTypeDiscoveryHub.RegisterModAssembly(ModId, assembly);
    }
}