using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Summoner.Scripts.Powers;

namespace Summoner.Scripts.Cards.Rare;

public class PitGrit() :
    SummonerCard(2, CardType.Power, CardRarity.Rare, TargetType.Self)
{

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 给予玩家一层沙场豪情Power
        await PowerCmd.Apply<PitGritPower>(
            choiceContext, 
            base.Owner.Creature, 
            1, // 一层
            base.Owner.Creature, 
            this);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }
}