using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Scaffolding.Content;

namespace Summoner.Scripts.Powers;

public class LightningFieldPower : SummonerPower
{
    public override PowerAssetProfile AssetProfile => new(
        IconPath: "res://Summoner/Images/Powers/LightningFieldPower.png",
        BigIconPath: "res://Summoner/Images/Powers/LightningFieldPower.png"
    );
    
    
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<ManaPower>()
    ];
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props, Creature target, CardModel? cardSource)
    {
        // 检查是否是玩家造成的伤害
        if (dealer == base.Owner)
        {
            // 造成伤害后获得法力
            // Log.Info("LightningFieldPower获得法力值");
            await PowerCmd.Apply<ManaPower>(
                choiceContext,
                base.Owner,
                base.Amount, // 获得的法力值等于Power的层数
                base.Owner,
                null);
        }
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        // 回合结束时移除这个Power
        if (participants.Contains(base.Owner))
        {
            await PowerCmd.Remove(this);
        }
    }
}