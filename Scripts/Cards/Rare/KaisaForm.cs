using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using Summoner.Scripts.Powers;

namespace Summoner.Scripts.Cards.Rare;

public class KaisaForm() :
    SummonerCard(3, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    public override List<CardKeyword> CanonicalKeywords => [
        // CardKeyword.Innate 固有
        CardKeyword.Ethereal
    ];
    
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<PlasmaPower>()
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<KaisaFormPower>(choiceContext, base.Owner.Creature, 1m, base.Owner.Creature, this);
    }
    
    
    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }
}