using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Summoner.Scripts.Powers;

namespace Summoner.Scripts.Cards.Common;

public class Sorcery():
    SummonerCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override List<CardKeyword> CanonicalKeywords => [
        CardKeyword.Exhaust
    ];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new IntVar("Mana", 5m),
        new CardsVar(2)
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<ManaPower>()
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 获得法力
        int manaAmount = base.DynamicVars["Mana"].IntValue;
        await PowerCmd.Apply<ManaPower>(choiceContext, base.Owner.Creature, manaAmount, base.Owner.Creature, this);
        
        await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Cards.UpgradeValueBy(1m);
        base.DynamicVars["Mana"].UpgradeValueBy(3m);
    }
}