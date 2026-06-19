using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Summoner.Scripts.Cards.Common;

public class Condemn() :
    SummonerCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DynamicVar("PercentDamage", 10m)
    ];
    
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var target = cardPlay.Target;
        if (target != null)
        {
            // 获取基础百分比
            decimal basePercent = base.DynamicVars["PercentDamage"].BaseValue;
            
            // 获取力量加成（每1点力量增加1%）
            int strengthBonus = base.Owner.Creature.GetPowerAmount<StrengthPower>();
            
            // 计算总百分比
            decimal totalPercent = basePercent + strengthBonus;
            
            // 计算伤害值（敌人最大生命值的百分比）
            int damage = (int)(target.MaxHp * (totalPercent / 100m));
            
            await DamageCmd.Attack(damage).FromCard(this)
                .Targeting(target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["PercentDamage"].UpgradeValueBy(5m);
    }
}