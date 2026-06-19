using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Summoner.Scripts.Cards.Ancient;

public class Chronobreak() :
    SummonerCard(0, CardType.Skill, CardRarity.Ancient, TargetType.Self)
{
    public override List<CardKeyword> CanonicalKeywords => [
        CardKeyword.Exhaust
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(3),
        new EnergyVar(2),
        new HealVar(5)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 丢弃所有手牌
        var handCards = PileType.Hand.GetPile(base.Owner).Cards.ToArray();
        foreach (var card in handCards)
        {
            if (card != null)
            {
                await CardCmd.Discard(choiceContext, card);
            }
        }
        
        // 抽取指定数量的牌
        await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner);
        
        // 获得能量
        await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
        
        // 回复生命
        await CreatureCmd.Heal(base.Owner.Creature, base.DynamicVars.Heal.BaseValue);
    }
    
    
    protected override void OnUpgrade()
    {
        base.DynamicVars.Cards.UpgradeValueBy(2m);
        base.DynamicVars.Energy.UpgradeValueBy(1m);
        base.DynamicVars.Heal.UpgradeValueBy(3m);
    }

}