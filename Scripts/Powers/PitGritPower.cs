using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Scaffolding.Content;

namespace Summoner.Scripts.Powers;

public class PitGritPower : SummonerPower
{
    public override PowerAssetProfile AssetProfile => new(
        IconPath: "res://Summoner/Images/Powers/PitGritPower.png",
        BigIconPath: "res://Summoner/Images/Powers/PitGritPower.png"
    );
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("Percent", 0.9m)
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<StrengthPower>()
    ];
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter; // 改为可叠加

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power == this)
        {
            // 更新动态变量的值为层数乘以基础百分比
            decimal percentToSubtract = base.Amount * 0.1m; // 每层减去10%
            decimal finalValue = 1m - percentToSubtract; // 最终值
            base.DynamicVars["Percent"].BaseValue = finalValue;
        }
    }

    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (participants.Contains(base.Owner))
        {
            // 直接使用动态变量中的百分比值
            int currentHp = base.Owner.CurrentHp;
            decimal actualPercent = 1m - base.DynamicVars["Percent"].BaseValue; // 实际失去百分比
            decimal damageDecimal = currentHp * actualPercent;
            int damageAmount = (int)decimal.Round(damageDecimal, MidpointRounding.AwayFromZero); // 四舍五入到整数
            
            // 造成伤害（失去一定百分比当前生命值）
            if (damageAmount > 0)
            {
                await CreatureCmd.Damage(choiceContext, base.Owner, damageAmount, ValueProp.Unblockable | ValueProp.Unpowered, null, null);
            }
            
            int strengthGain = System.Math.Max(1, (int)System.Math.Round(damageAmount * 0.5m, System.MidpointRounding.AwayFromZero));
            await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner, strengthGain, base.Owner, null);
        }
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (participants.Contains(base.Owner))
        {
            // 计算已损生命值的10%（最大生命值 - 当前生命值）的10%
            int maxHp = base.Owner.MaxHp;
            int currentHp = base.Owner.CurrentHp;
            int lostHp = maxHp - currentHp;
            decimal tenPercentOfLostDecimal = lostHp * (1m - base.DynamicVars["Percent"].BaseValue); // 固定10%，不随层数变化
            int healAmount = (int)decimal.Round(tenPercentOfLostDecimal, MidpointRounding.AwayFromZero); // 四舍五入到整数
            
            // 回复生命值
            if (healAmount > 0)
            {
                await CreatureCmd.Heal(base.Owner, healAmount);
            }
        }
    }
    
    public override async Task AfterCombatEnd(CombatRoom room)
    {
        // 战斗结束时也回复生命值
        // 计算已损生命值的10%（最大生命值 - 当前生命值）的10% 
        int maxHp = base.Owner.MaxHp;
        int currentHp = base.Owner.CurrentHp;
        int lostHp = maxHp - currentHp;
        decimal tenPercentOfLostDecimal = lostHp * (1m - base.DynamicVars["Percent"].BaseValue); // 固定10%，不随层数变化 
        int healAmount = (int)decimal.Round(tenPercentOfLostDecimal, MidpointRounding.AwayFromZero); // 四舍五入到整数 
        
        // 回复生命值 
        if (healAmount > 0) 
        {
            await CreatureCmd.Heal(base.Owner, healAmount); 
        }
    }
    
}