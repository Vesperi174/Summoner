using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.ValueProps;
using Summoner.Scripts.Powers;

namespace Summoner.Scripts.Cards.Rare;

public class RiteOfTheAranes() :
    SummonerCard(0, CardType.Attack, CardRarity.Rare, TargetType.RandomEnemy)
{
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<ManaPower>()
    ];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new IntVar("Mana", 5m),
        new DamageVar(8m,ValueProp.Move)
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 获取法力消耗量和伤害量
        int manaCost = base.DynamicVars["Mana"].IntValue;
        int damage = base.DynamicVars.Damage.IntValue;
        
        var manaPower = base.Owner.Creature.GetPower<ManaPower>();
        // 循环直到没有足够的法力值
        while (manaPower != null && manaPower.Amount >= manaCost)
        {
            // 选择一个随机敌人
            var hittableEnemies = base.CombatState.HittableEnemies;
            if (hittableEnemies.Count > 0)
            {
                var randomEnemy = base.Owner.RunState.Rng.CombatTargets.NextItem(hittableEnemies);
                // Log.Info("RiteOfTheAranes打伤害");
                await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this)
                    .Targeting(randomEnemy)
                    .WithHitFx("vfx/vfx_attack_slash")
                    .Execute(choiceContext);
                
                await Cmd.Wait(0.1f);
            } else
            {
                break;
            }
            //Log.Info("消耗法力值");
            await PowerCmd.ModifyAmount(choiceContext, manaPower, -manaCost, base.Owner.Creature, this);
            
            manaPower = base.Owner.Creature.GetPower<ManaPower>();
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(2m); //升级后加3点伤害
    }
}