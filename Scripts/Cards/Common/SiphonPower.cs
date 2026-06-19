using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.ValueProps;
using Summoner.Scripts.Powers;

namespace Summoner.Scripts.Cards.Common;

public class SiphonPower() :
    SummonerCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    public override bool GainsBlock => true;

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<ManaPower>()
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var target = cardPlay.Target;
        ArgumentNullException.ThrowIfNull(target, "cardPlay.Target");
        
        // 获取玩家的法力Power
        var manaPower = base.Owner.Creature.GetPower<ManaPower>();
        
        if (manaPower != null)
        {
            // 获取当前法力值
            int manaAmount = manaPower.Amount;
            
            if (manaAmount > 0)
            {
                // 消耗所有法力
                await PowerCmd.Remove(manaPower);
                
                // 造成等量伤害
                await DamageCmd.Attack(manaAmount)
                    .FromCard(this)
                    .Targeting(target)
                    .WithHitFx("vfx/vfx_attack_slash")
                    .Execute(choiceContext);
                
                // 获得等量格挡
                await CreatureCmd.GainBlock(base.Owner.Creature, (decimal)manaAmount, ValueProp.Unpowered, cardPlay);
            }
        }
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}