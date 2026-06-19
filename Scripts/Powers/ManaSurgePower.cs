using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Scaffolding.Content;

namespace Summoner.Scripts.Powers;

public class ManaSurgePower : SummonerPower
{
    public override PowerAssetProfile AssetProfile => new(
        IconPath: "res://Summoner/Images/Powers/ManaSurgePower.png",
        BigIconPath: "res://Summoner/Images/Powers/ManaSurgePower.png"
    );
    
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<ManaPower>()
    ];
    
    private class Data
    {
        public int damageCountThisTurn = 0;
    }

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override object InitInternalData()
    {
        return new Data();
    }

    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props, Creature target, CardModel? cardSource)
    {
        // 检查是否是玩家造成的伤害，并且是本回合前N次造成伤害（N由Amount决定）
        if (dealer == base.Owner && GetInternalData<Data>().damageCountThisTurn < base.Amount)
        {
            // 增加计数
            GetInternalData<Data>().damageCountThisTurn++;
            
            // 获得与造成伤害量相等的法力
            Log.Info("ManaSurgePower获得法力值");
            await PowerCmd.Apply<ManaPower>(
                choiceContext,
                base.Owner,
                result.UnblockedDamage, // 法力值等于实际造成的伤害量
                base.Owner,
                null);
        }
    }

    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        // 新回合开始时重置计数
        if (participants.Contains(base.Owner))
        {
            GetInternalData<Data>().damageCountThisTurn = 0;
        }
    }
}