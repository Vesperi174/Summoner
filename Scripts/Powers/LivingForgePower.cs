using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using STS2RitsuLib.Scaffolding.Content;

namespace Summoner.Scripts.Powers;

public class LivingForgePower : SummonerPower
{
    public override PowerAssetProfile AssetProfile => new(
        IconPath: "res://Summoner/Images/Powers/LivingForgePower.png",
        BigIconPath: "res://Summoner/Images/Powers/LivingForgePower.png"
    );
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCombatEnd(CombatRoom room)
    {
        // 获取卡组中所有可升级的牌
        List<CardModel> upgradableCards = PileType.Deck.GetPile(base.Owner.Player).Cards
            .Where((CardModel c) => c.IsUpgradable).ToList();
        
        // 随机升级指定数量的牌
        for (int i = 0; i < base.Amount; i++)
        {
            if (upgradableCards.Count == 0)
            {
                break;
            }
            
            // 使用游戏内置的随机选择器随机选择一张牌
            CardModel cardToUpgrade = base.Owner.Player.RunState.Rng.CombatCardSelection.NextItem(upgradableCards);
            upgradableCards.Remove(cardToUpgrade);
            
            // 升级选中的牌
            CardCmd.Upgrade(cardToUpgrade);
        }
    }
}