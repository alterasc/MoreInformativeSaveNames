using HarmonyLib;
using Kingmaker.Blueprints.Root;
using Kingmaker.UI.MVVM._VM.SaveLoad;
using Kingmaker.UI.MVVM._VM.ServiceWindows.CharacterInfo.Sections.Abilities;
using Owlcat.Runtime.UI.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace MoreInformativeSaveNames;

/// <summary>
/// Abbreviates campaign name on loading screen
/// I.e. instead of "Test - Inevitable Excess" 
/// it becomes "Test - IE"
/// </summary>
[HarmonyPatch]
internal static class SaveSlotGroupRenamer
{
    [HarmonyPatch(typeof(SaveSlotGroupVM), MethodType.Constructor, [typeof(SaveSlotVM)])]
    [HarmonyTranspiler]
    internal static IEnumerable<CodeInstruction> InjectLootClaim(IEnumerable<CodeInstruction> instructions)
    {
        var method = AccessTools.Method(typeof(BaseDisposable), nameof(BaseDisposable.AddDisposable), [typeof(IDisposable)]);
        foreach (var instruction in instructions)
        {
            yield return instruction;
            if (instruction.Calls(method))
            {
                break;
            }
        }
        yield return new CodeInstruction(opcode: OpCodes.Ret);
    }

    [HarmonyPatch(typeof(SaveSlotGroupVM), MethodType.Constructor, [typeof(SaveSlotVM)])]
    [HarmonyPostfix]
    internal static void PostConstructor(SaveSlotGroupVM __instance, SaveSlotVM slot)
    {
        BlueprintCampaign campaign = slot.Reference.Campaign;
        string title = campaign != null && !campaign.IsMainGameContent
            ? string.Format("{0} - {1}", __instance.CharacterName, GetCampaignAbbrev(slot.Reference.Campaign.Title))
            : __instance.CharacterName;
        __instance.AddDisposable(__instance.ExpandableTitleVM = new ExpandableTitleVM(title, new Action<bool>(__instance.SwitchExpand)));
    }

    private static string GetCampaignAbbrev(string title)
    {
        return title.Split([' '], StringSplitOptions.RemoveEmptyEntries)
            .Select(word => word[0])
            .Join(null, "");
    }
}
