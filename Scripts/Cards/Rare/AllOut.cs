using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Summoner.Scripts.Cards.Rare;

public class AllOut() :
    SummonerCard(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<VulnerablePower>(),
        HoverTipFactory.FromPower<FrailPower>(),
        HoverTipFactory.FromPower<StrengthPower>()
    ];
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("MaxHpReduce", 20m),
        new PowerVar<VulnerablePower>(5m),
        new PowerVar<FrailPower>(5m),
        new EnergyVar(3),
        new CardsVar(3),
        new PowerVar<StrengthPower>(8m),
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 失去最大生命值
        await CreatureCmd.LoseMaxHp(choiceContext, base.Owner.Creature, base.DynamicVars["MaxHpReduce"].BaseValue, isFromCard: true);
        
        // 获得易伤
        await PowerCmd.Apply<VulnerablePower>(choiceContext, base.Owner.Creature, base.DynamicVars.Vulnerable.BaseValue, base.Owner.Creature, this);
        
        // 获得脆弱
        await PowerCmd.Apply<FrailPower>(choiceContext, base.Owner.Creature, base.DynamicVars["FrailPower"].BaseValue, base.Owner.Creature, this);
        
        // 获得能量
        await PlayerCmd.GainEnergy(base.DynamicVars.Energy.IntValue, base.Owner);
        
        // 抽牌
        await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner);
        
        // 获得力量
        await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Strength.BaseValue, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Energy.UpgradeValueBy(1m);
        base.DynamicVars.Cards.UpgradeValueBy(2m);
        base.DynamicVars.Strength.UpgradeValueBy(2m);
    }
}