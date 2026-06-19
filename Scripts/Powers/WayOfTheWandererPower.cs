using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Scaffolding.Content;

namespace Summoner.Scripts.Powers;

public class WayOfTheWandererPower : SummonerPower
{
    public override PowerAssetProfile AssetProfile => new(
        IconPath: "res://Summoner/Images/Powers/WayOfTheWandererPower.png",
        BigIconPath: "res://Summoner/Images/Powers/WayOfTheWandererPower.png"
    );
    
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<PlatingPower>()
    ];
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 检查是否打出了技能牌
        if (cardPlay.Card.Type == CardType.Skill && cardPlay.Card.Owner.Creature == base.Owner)
        {
            // 获得覆甲
            await PowerCmd.Apply<PlatingPower>(choiceContext, base.Owner, base.Amount, base.Owner, null);
        }
    }
}