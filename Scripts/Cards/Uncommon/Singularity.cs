using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using Summoner.Scripts.Powers;

namespace Summoner.Scripts.Cards.Uncommon;

public class Singularity() :
    SummonerCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override List<CardKeyword> CanonicalKeywords => [
        CardKeyword.Exhaust
    ];
    
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<ManaPower>(),
        HoverTipFactory.FromPower<CosmicCreatorPower>()
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 获取玩家的法力Power
        var manaPower = base.Owner.Creature.GetPower<ManaPower>();
        
        if (manaPower != null)
        {
            // 获取当前法力值
            int currentMana = manaPower.Amount;
            
            if (currentMana > 0)
            {
                // 移除法力Power
                await PowerCmd.Remove(manaPower);
                
                // 将所有法力转化为星尘（即应用相同数量的CosmicCreatorPower层数）
                await PowerCmd.Apply<CosmicCreatorPower>(
                    choiceContext, 
                    base.Owner.Creature, 
                    currentMana, 
                    base.Owner.Creature, 
                    this);
            }
        }
    }
    
    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }
}