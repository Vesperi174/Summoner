using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using Summoner.Scripts.Cards.Common;

namespace Summoner.Scripts.Cards.Rare;

public class RealmWarp() :
    SummonerCard(3, CardType.Skill, CardRarity.Rare, TargetType.None)
{
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromCard<Overload>()
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 查找所有超负荷卡片（在手牌、抽牌堆、弃牌堆中）
        var allOverloadCards = base.Owner.PlayerCombatState.AllCards
            .Where(card => card is Overload && card.Pile.Type != PileType.Exhaust)
            .ToList();

        // 对每张超负荷执行双倍伤害的攻击
        foreach (var overloadCard in allOverloadCards)
        {
            // 获取原始伤害值并乘以2
            var originalDamage = overloadCard is Overload originalOverload ? 
                originalOverload.DynamicVars.Damage.BaseValue : 2m;
            var doubleDamage = originalDamage * 2;
            
            // 对随机敌人造成双倍伤害
            await DamageCmd.Attack(doubleDamage)
                .FromCard(this)
                .TargetingRandomOpponents(base.CombatState)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
        }

        // 移除所有找到的超负荷卡牌
        if (allOverloadCards.Count > 0)
        {
            foreach (var card in allOverloadCards)
            {
                await CardPileCmd.Add(card, PileType.Exhaust, CardPilePosition.Bottom);
            }
        }
    }
    
    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
    
}