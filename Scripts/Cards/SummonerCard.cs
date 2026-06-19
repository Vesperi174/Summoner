using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Logging;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using Summoner.Scripts.Characters;
using Summoner.Scripts.Extensions;

namespace Summoner.Scripts.Cards;

[RegisterCard(typeof(SummonerCardPool), Inherit = true)]
public abstract class SummonerCard(int cost, CardType type, CardRarity rarity, TargetType target) :
    ModCardTemplate(cost, type, rarity, target)
{
    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"res://Summoner/Images/Cards/{GetType().Name}.png"
        // 卡框等，有需求自己添加。需要自行判断卡牌类型（攻击、技能、能力等）设置，建议写在基类里。
        // 如果使用自定义卡池，需要改下material，看添加人物章节的添加卡池部分
        // FramePath: "", // 卡牌背景
        // PortraitBorderPath: "", // 边框（状态牌感染使用的）
        // BannerTexturePath: "" // 横幅（不同类型）
    );
    
    
    /*
    //Image size:
    //Normal art: 1000x760 (Using 500x380 should also work, it will simply be scaled.)
    //Full art: 606x852
    public override string CustomPortraitPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
            Log.Info(">>>[SummonerMod]CardPath=" + path, 2);
            return ResourceLoader.Exists(path) ? path : "card.png".CardImagePath();
        }
    }

    //Smaller variants of card images for efficiency:
    //Smaller variant of fullart: 250x350
    //Smaller variant of normalart: 250x190

    //Uses card_portraits/card_name.png as image path. These should be smaller images.
    public override string PortraitPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
            Log.Info(">>>[SummonerMod]CardPath=" + path, 2);
            return ResourceLoader.Exists(path) ? path : "card.png".CardImagePath();
        }
    }

    //Optional and I'm not sure it's functional yet.
    public override string BetaPortraitPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
            Log.Info(">>>[SummonerMod]CardPath=" + path, 2);
            return ResourceLoader.Exists(path) ? path : "card.png".CardImagePath();
        }
    }
    */
}