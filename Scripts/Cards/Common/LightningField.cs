using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Summoner.Scripts.Powers;

namespace Summoner.Scripts.Cards.Common;

public class LightningField() :
    SummonerCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new IntVar("Mana", 8m),
        new IntVar("ManaPro", 2m)
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<ManaPower>()
    ];
    

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int manaAmount = base.DynamicVars["Mana"].IntValue;
        int manaProAmount = base.DynamicVars["ManaPro"].IntValue;
        
        // 先给予法力值
        await PowerCmd.Apply<ManaPower>(
            choiceContext,
            base.Owner.Creature,
            manaAmount,
            base.Owner.Creature,
            this);
        
        // 再给予闪电领域Power
        await PowerCmd.Apply<LightningFieldPower>(
            choiceContext,
            base.Owner.Creature,
            manaProAmount,
            base.Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["Mana"].UpgradeValueBy(2m);
        base.DynamicVars["ManaPro"].UpgradeValueBy(1m);
    }
}