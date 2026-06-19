using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Scaffolding.Content;
using Summoner.Scripts.Cards.Colorless;

namespace Summoner.Scripts.Powers;

public class AkaliPower: SummonerPower
{
    public override PowerAssetProfile AssetProfile => new(
        IconPath: "res://Summoner/Images/Powers/AkaliPower.png",
        BigIconPath: "res://Summoner/Images/Powers/AkaliPower.png"
    );
    private class Data
    {
        public Dictionary<CardModel, int> cardsThatTriggeredThisTurn = new Dictionary<CardModel, int>();
    }

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override object InitInternalData()
    {
        return new Data();
    }

    public override async Task BeforeCardPlayed(CardPlay cardPlay)
    {
        // Log.Info($"[AkaliPower] BeforeCardPlayed被调用 - 卡牌: {cardPlay.Card.GetType().Name}, 类型: {cardPlay.Card.Type}, 所有者: {cardPlay.Card.Owner?.GetType().Name}");
        
        // 检查是否是玩家打出的攻击牌，且不是潜龙印
        if (cardPlay.Card.Owner == base.Owner.Player && 
            cardPlay.Card.Type == CardType.Attack && 
            cardPlay.Card is not AssassinMark)
        {
            // Log.Info($"[AkaliPower] 记录攻击牌: {cardPlay.Card.GetType().Name}");
            // 记录这张卡牌，使用Power的层数作为触发次数
            GetInternalData<Data>().cardsThatTriggeredThisTurn[cardPlay.Card] = base.Amount;
            // Log.Info($"[AkaliPower] 当前记录的卡牌数量: {GetInternalData<Data>().cardsThatTriggeredThisTurn.Count}, 触发次数: {base.Amount}");
        }
        else
        {
            // Log.Info($"[AkaliPower] 不满足记录条件 - 是玩家卡牌: {cardPlay.Card.Owner == base.Owner.Player}, 是攻击牌: {cardPlay.Card.Type == CardType.Attack}, 不是潜龙印: {cardPlay.Card is not AssassinMark}");
        }
    }

    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props, Creature target, CardModel? cardSource)
    {
        // Log.Info($"[AkaliPower] AfterDamageGiven被调用 - dealer: {dealer?.GetType().Name}, cardSource: {cardSource?.GetType().Name ?? "null"}, 未格挡伤害: {result.UnblockedDamage}, 格挡伤害: {result.BlockedDamage}");
        // Log.Info($"[AkaliPower] dealer == base.Owner: {dealer == base.Owner}");
        // Log.Info($"[AkaliPower] cardSource != null: {cardSource != null}");
        
        if (cardSource != null)
        {
            bool contains = GetInternalData<Data>().cardsThatTriggeredThisTurn.ContainsKey(cardSource);
            int triggerCount = contains ? GetInternalData<Data>().cardsThatTriggeredThisTurn[cardSource] : 0;
            // Log.Info($"[AkaliPower] cardsThatTriggeredThisTurn包含cardSource: {contains}, 触发次数: {triggerCount}");
            // Log.Info($"[AkaliPower] cardsThatTriggeredThisTurn中的卡牌: {string.Join(", ", GetInternalData<Data>().cardsThatTriggeredThisTurn.Select(c => $"{c.Key.GetType().Name}({c.Value}次)"))}");
        }
        
        // 检查是否是玩家造成的伤害，且卡牌已记录且造成了伤害（包括被格挡的伤害）
        if (dealer == base.Owner && 
            cardSource != null && 
            GetInternalData<Data>().cardsThatTriggeredThisTurn.ContainsKey(cardSource) &&
            (result.UnblockedDamage + result.BlockedDamage) > 0)
        {
            int triggerCount = GetInternalData<Data>().cardsThatTriggeredThisTurn[cardSource];
            // Log.Info($"[AkaliPower] 造成伤害，触发潜龙印添加。伤害来源: {cardSource.GetType().Name}, 添加数量: {triggerCount}");
            
            // 移除记录
            GetInternalData<Data>().cardsThatTriggeredThisTurn.Remove(cardSource);
            
            // 根据触发次数添加潜龙印到手牌
            for (int i = 0; i < triggerCount; i++)
            {
                await AssassinMark.CreateInHand(base.Owner.Player, base.CombatState);
            }
            // Log.Info($"[AkaliPower] 已添加{triggerCount}张潜龙印到手牌");
        }
        else
        {
            // Log.Info($"[AkaliPower] 不满足触发条件");
        }
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        // 回合结束时清空记录并移除Power
        if (participants.Contains(base.Owner))
        {
            // Log.Info($"[AkaliPower] 回合结束，移除Power");
            GetInternalData<Data>().cardsThatTriggeredThisTurn.Clear();
            await PowerCmd.Remove(this);
        }
    }
}