using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Summoner.Scripts.Cards.Rare;

public class WorldEnder() :
    SummonerCard(3, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    private const string _enemyStrengthKey = "EnemyStrength";

    public override List<CardKeyword> CanonicalKeywords => [
        CardKeyword.Exhaust
    ];
    
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<StrengthPower>()
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<StrengthPower>(2m),
        new DynamicVar("EnemyStrength", 2m)
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 获得力量
        await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner.Creature, base.DynamicVars["StrengthPower"].BaseValue, base.Owner.Creature, this);
        
        // 翻倍你的力量加成 - 这里是指将当前的力量值翻倍
        int currentStrength = base.Owner.Creature.GetPowerAmount<StrengthPower>();
        if (currentStrength > 0)
        {
            await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner.Creature, currentStrength, base.Owner.Creature, this);
        }
        
        // 所有敌人获得力量
        foreach (Creature enemy in base.CombatState.HittableEnemies)
        {
            await PowerCmd.Apply<StrengthPower>(choiceContext, enemy, base.DynamicVars["EnemyStrength"].BaseValue, base.Owner.Creature, this);
        }
    }
    
    
    protected override void OnUpgrade()
    {
        base.DynamicVars.Strength.UpgradeValueBy(1m);
        base.DynamicVars["EnemyStrength"].UpgradeValueBy(1m);
    }
}