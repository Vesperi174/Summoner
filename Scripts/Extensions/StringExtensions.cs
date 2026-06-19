using MainFile = Summoner.Scripts.Entry;
using MegaCrit.Sts2.Core.Logging;

namespace Summoner.Scripts.Extensions;

public static class StringExtensions
{
    public static string ImagePath(this string path)
    {
        return Path.Join(MainFile.ModId, "Images", path);
    }

    public static string CardImagePath(this string path)
    {   
        return Path.Join(MainFile.ModId, "Images", "Cards", path);
    }

    public static string BigCardImagePath(this string path)
    {
        return Path.Join(MainFile.ModId, "Images", "Cards", "Big", path);
    }

    public static string PowerImagePath(this string path)
    {
        return Path.Join(MainFile.ModId, "Images", "Powers", path);
    }
    public static string BigPowerImagePath(this string path)
    {
        return Path.Join(MainFile.ModId, "Images", "Powers", "Big", path);
    }

    public static string RelicImagePath(this string path)
    {
        return Path.Join(MainFile.ModId, "Images", "Relics", path);
    }

    public static string BigRelicImagePath(this string path)
    {
        return Path.Join(MainFile.ModId, "Images", "Relics", "Big", path);
    }

    public static string CharacterUiPath(this string path)
    {
        return Path.Join(MainFile.ModId, "Images", "Charui", path);
    }
}