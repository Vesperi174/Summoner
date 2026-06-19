using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Scaffolding.Content;

namespace Summoner.Scripts.Powers;

public class ScalePower : SummonerPower
{
    public override PowerAssetProfile AssetProfile => new(
        IconPath: "res://Summoner/Images/Powers/ScalePower.png",
        BigIconPath: "res://Summoner/Images/Powers/ScalePower.png"
    );
    
    public override PowerType Type => PowerType.Buff;
    
    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props, Creature target, CardModel? cardSource)
    {
        // 检查是否是玩家造成的伤害
        if (dealer == base.Owner)
        {
            // 获取未被格挡的伤害
            int unblockedDamage = result.UnblockedDamage;
            
            if (unblockedDamage > 0)
            {
                // 计算回复的生命值（未被格挡伤害的百分比）
                decimal healDecimal = unblockedDamage * base.Amount * 0.01m;
                int healAmount = (int)decimal.Round(healDecimal, MidpointRounding.AwayFromZero);
                
                if (healAmount > 0)
                {
                    await CreatureCmd.Heal(base.Owner, healAmount);
                }
            }
        }
    }
}