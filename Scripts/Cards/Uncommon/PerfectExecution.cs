using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Summoner.Scripts.Cards.Uncommon;

public class PerfectExecution() :
    SummonerCard(2, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(12m, ValueProp.Move)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 对所有敌人造成伤害
        foreach (var enemy in base.CombatState.HittableEnemies)
        {
            // 计算敌人损失的生命值百分比
            int maxHp = enemy.MaxHp;
            int currentHp = enemy.CurrentHp;
            int lostHp = maxHp - currentHp;
            
            // 计算损失百分比（0-100）
            decimal lostPercent = (lostHp * 100m) / maxHp;
            
            // 每损失1%生命值，伤害提升2%
            decimal damageMultiplier = 1m + (lostPercent * 0.02m);
            
            // 计算最终伤害
            decimal baseDamage = base.DynamicVars.Damage.BaseValue;
            decimal finalDamage = baseDamage * damageMultiplier;
            
            // 对敌人造成伤害
            await CreatureCmd.Damage(choiceContext, enemy, (int)finalDamage, ValueProp.Unpowered, base.Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(3m);
    }
}