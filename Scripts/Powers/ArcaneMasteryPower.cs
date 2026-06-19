using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using STS2RitsuLib.Scaffolding.Content;

namespace Summoner.Scripts.Powers;

public class ArcaneMasteryPower : SummonerPower
{
    public override PowerAssetProfile AssetProfile => new(
        IconPath: "res://Summoner/Images/Powers/ArcaneMasteryPower.png",
        BigIconPath: "res://Summoner/Images/Powers/ArcaneMasteryPower.png"
    );
    
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (participants.Contains(base.Owner))
        {
            // 将生命值回复到层数指定的水平
            int targetHp = (int)base.Amount;
            int currentHp = base.Owner.CurrentHp;
            
            if (targetHp > currentHp)
            {
                // 计算需要回复的生命值
                int healAmount = targetHp - currentHp;
                await CreatureCmd.Heal(base.Owner, healAmount);
            }
            // 如果当前生命值已经高于目标值，则不做任何操作
            
            // 回合结束时移除这个Power
            await PowerCmd.Remove(this);
        }
    }
    
    public override async Task AfterCombatEnd(CombatRoom room)
    {
        // 战斗结束时也将生命值回复到层数指定的水平 
        int targetHp = (int)base.Amount; 
        int currentHp = base.Owner.CurrentHp; 
        
        if (targetHp > currentHp) 
        { 
            // 计算需要回复的生命值 
            int healAmount = targetHp - currentHp; 
            await CreatureCmd.Heal(base.Owner, healAmount); 
        }
        // 如果当前生命值已经高于目标值，则不做任何操作 
    }
}