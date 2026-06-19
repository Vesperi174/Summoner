using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Scaffolding.Content;

namespace Summoner.Scripts.Powers;

public class DeathMarkPower : SummonerPower
{
    public override PowerAssetProfile AssetProfile => new(
        IconPath: "res://Summoner/Images/Powers/DeathMarkPower.png",
        BigIconPath: "res://Summoner/Images/Powers/DeathMarkPower.png"
    );
    
    
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;
    
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        // 检查是否是有标记的敌人受到了伤害
        if (target == base.Owner)
        {
            // 根据伤害量减少层数
            int damageDone = result.UnblockedDamage + result.BlockedDamage;
            if (damageDone > 0)
            {
                // 减少Power的层数
                int reduceAmount = System.Math.Min(damageDone, base.Amount);
                await PowerCmd.ModifyAmount(choiceContext, this, -reduceAmount, dealer, cardSource);
                
                // 检查层数是否清零
                if (base.Amount <= 0)
                {
                    // 移除Power并造成伤害
                    await PowerCmd.Remove(this);
                    
                    // 造成目标最大生命值40%的伤害
                    int fortyPercentDamage = target.MaxHp * 2 / 5;
                    await CreatureCmd.Damage(choiceContext, target, fortyPercentDamage, ValueProp.Unpowered, dealer, cardSource);
                }
            }
        }
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        // 检查是否是玩家的回合结束
        if (side == CombatSide.Player)
        {
            // 玩家回合结束时如果还有层数，直接移除Power
            await PowerCmd.Remove(this);
        }
    }
    
    
}