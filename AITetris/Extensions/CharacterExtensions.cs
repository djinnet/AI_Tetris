using AITetris.Classes;

namespace AITetris.Extensions;
public static class CharacterExtensions
{
    public static bool IsPlayer(this Character character)
    {
        return character.GetType() == typeof(Player);
    }
}
