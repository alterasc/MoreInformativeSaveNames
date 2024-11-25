using Kingmaker.EntitySystem.Entities;
using System.Linq;

namespace MoreInformativeSaveNames;

/// <summary>
/// Util class for generating names
/// </summary>
public static class CharacterInfoFormatter
{
    /// <summary>
    /// Generates informative string about main character
    /// </summary>
    /// <param name="value">main character unit</param>
    /// <returns></returns>
    public static string GetExpandedName(UnitEntityData value)
    {
        if (value == null)
        {
            return "Unnamed";
        }

        var charName = value.CharacterName ?? "Unnamed";
        try
        {
            var mainClass = value.Progression.Classes.OrderByDescending(x => x.Level).First();
            var charClass = string.Empty;
            if (mainClass.Archetypes.Count > 0)
            {
                charClass = mainClass.Archetypes[0].Name;
            }
            else
            {
                charClass = mainClass.CharacterClass.Name;
            }
            var mythicClass = value.Progression.GetCurrentMythicClass();

            if (mythicClass != null)
            {
                var mythicClassName = mythicClass.CharacterClass.Name;
                return $"{charName} {charClass}/{mythicClassName}";
            }
            else
            {
                return $"{charName} {charClass}";
            }
        }
        catch (System.Exception ex)
        {
            Main.log.Log($"Error when generating informative name: {ex.Message}");
            return charName;
        }
    }
}
