using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.ValueProps;

namespace Summoner.Scripts.Cards.Uncommon;

public class DragonsRage() :
    SummonerCard(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(12m, ValueProp.Move),
        new DynamicVar("PercentDamage", 15m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        
        Log.Info($"[DragonsRage] 开始执行 - 目标: {cardPlay.Target.GetType().Name}");
        Log.Info($"[DragonsRage] 基础伤害: {base.DynamicVars.Damage.BaseValue}, 百分比伤害: {base.DynamicVars["PercentDamage"].BaseValue}%");
        
        Log.Info($"[DragonsRage] 对初始目标造成基础伤害: {base.DynamicVars.Damage.BaseValue}");
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        Log.Info($"[DragonsRage] 初始目标伤害执行完成");
        
        var otherEnemies = base.CombatState.Enemies.Where(e => e != cardPlay.Target);
        Log.Info($"[DragonsRage] 找到{otherEnemies.Count()}个其他敌人");
        
        await Cmd.Wait(0.1f);
        
        foreach (var enemy in otherEnemies)
        {
            Log.Info($"[DragonsRage] 处理敌人: {enemy.GetType().Name}, 最大生命值: {enemy.MaxHp}");
            
            // 计算百分比伤害
            decimal percentDamage = enemy.MaxHp * base.DynamicVars["PercentDamage"].BaseValue / 100m;
            decimal totalDamage = base.DynamicVars.Damage.BaseValue + percentDamage;
            
            Log.Info($"[DragonsRage] 计算伤害 - 百分比伤害: {percentDamage}, 总伤害: {totalDamage}");
            
            await DamageCmd.Attack(totalDamage)
                .FromCard(this)
                .Targeting(enemy)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
            
            Log.Info($"[DragonsRage] 对{enemy.GetType().Name}造成伤害执行完成");
        }
        
        Log.Info($"[DragonsRage] 所有伤害执行完成");
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(3m);
        base.DynamicVars["PercentDamage"].UpgradeValueBy(5m);
    }
}