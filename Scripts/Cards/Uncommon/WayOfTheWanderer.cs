using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Summoner.Scripts.Powers;

namespace Summoner.Scripts.Cards.Uncommon;

public class WayOfTheWanderer() :
    SummonerCard(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<PlatingPower>()
    ];
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("PlatingPower", 1m)
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 应用一层浪客之道Power
        await PowerCmd.Apply<WayOfTheWandererPower>(
            choiceContext, 
            base.Owner.Creature, 
            base.DynamicVars["PlatingPower"].IntValue,
            base.Owner.Creature, 
            this);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["PlatingPower"].UpgradeValueBy(1m);
    }
}