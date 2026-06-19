using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Summoner.Scripts.Powers;

namespace Summoner.Scripts.Cards.Rare;

public class DeathMark() :
    SummonerCard(1, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var target = cardPlay.Target;
        if (target != null)
        {
            // 计算目标最大生命值的60%作为Power层数
            int maxHp = target.MaxHp;
            int powerAmount = maxHp * 3 / 5;  // 最大生命值的60%
            
            // 对目标施加DeathMarkPower，层数为目标最大生命值的60%
            await PowerCmd.Apply<DeathMarkPower>(
                choiceContext, 
                target, 
                powerAmount, 
                base.Owner.Creature, 
                this);
        }
    }
    
    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    } 
}