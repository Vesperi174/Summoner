using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Summoner.Scripts.Cards.Common;

public class Mimic() :
    SummonerCard(2, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override List<CardKeyword> CanonicalKeywords => [
        CardKeyword.Exhaust
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 获取消耗牌堆中的所有卡牌
        var exhaustPileCards = base.Owner.PlayerCombatState?.ExhaustPile.Cards.ToList();
        
        if (exhaustPileCards != null && exhaustPileCards.Count > 0)
        {
            // 将消耗牌堆的所有卡牌移动到抽牌堆
            foreach (var card in exhaustPileCards.ToList()) // 使用ToList()避免迭代时修改集合
            {
                await CardPileCmd.Add(card, PileType.Draw, CardPilePosition.Bottom);
            }
        }
    }
    
    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    } 
}