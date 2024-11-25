using HarmonyLib;
using Kingmaker;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Persistence;

namespace MoreInformativeSaveNames;

/// <summary>
/// Patches for SaveManager that replace value written into SaveInfo
/// Normally PlayerCharacterName contains only main character name,
/// but here we write more information
/// </summary>
[HarmonyPatch]
internal static class SaveManagerPatches
{
    [HarmonyPatch(typeof(SaveManager), nameof(SaveManager.CreateNewSave))]
    [HarmonyPostfix]
    internal static void PostCreateNewSave(ref SaveInfo __result)
    {
        UnitEntityData value = Game.Instance.Player.MainCharacter.Value;
        if (value != null)
        {
            __result.PlayerCharacterName = CharacterInfoFormatter.GetExpandedName(value);
        }
    }

    [HarmonyPatch(typeof(SaveManager), nameof(SaveManager.PrepareSave))]
    [HarmonyPostfix]
    internal static void PostPrepareSave(SaveInfo save)
    {
        UnitEntityData value = Game.Instance.Player.MainCharacter.Value;
        if (value != null)
        {
            save.PlayerCharacterName = CharacterInfoFormatter.GetExpandedName(value);
        }
    }
}
