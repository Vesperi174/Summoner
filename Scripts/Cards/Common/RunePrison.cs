using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Summoner.Scripts.Powers;

namespace Summoner.Scripts.Cards.Common;

public class RunePrison() :
    SummonerCard(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<VulnerablePower>(),
        HoverTipFactory.FromCard<Overload>()
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<VulnerablePower>(2m),
        new PowerVar<VulnerablePower>("_VulnerablePower", 3m)  // 默认是易伤层数+1
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var target = cardPlay.Target;
        if (target != null)
        {
            // 检查目标是否带有法术涌动
            if (target.HasPower<SpellFluxPower>())
            {
                // 如果带有法术涌动，给予_VulnerablePower层易伤（通常是易伤层数+1）
                await PowerCmd.Apply<VulnerablePower>(choiceContext, target, base.DynamicVars["_VulnerablePower"].BaseValue, base.Owner.Creature, this);
            }
            else
            {
                // 如果不带法术涌动，给予VulnerablePower层易伤
                await PowerCmd.Apply<VulnerablePower>(choiceContext, target, base.DynamicVars.Vulnerable.BaseValue, base.Owner.Creature, this);
            }
            
            // 添加一张超负荷到手牌
            await Overload.CreateInHand(base.Owner, base.CombatState);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Vulnerable.UpgradeValueBy(1m);
        base.DynamicVars["_VulnerablePower"].UpgradeValueBy(1m);
    }
}