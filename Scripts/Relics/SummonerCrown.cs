using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Summoner.Scripts.Relics;

[RegisterTouchOfOrobasRefinement(typeof(AllianceCrown))]
public class SummonerCrown : SummonerRelic
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(1)
    ];
    
    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (participants.Contains(base.Owner.Creature))
        { 
            Flash();
            List<CardModel> cards = PileType.Hand.GetPile(base.Owner).Cards
                .Where((CardModel c) => c.IsUpgradable)
                .ToList().StableShuffle(base.Owner.RunState.Rng.CombatCardSelection)
                .Take(base.DynamicVars.Cards.IntValue).ToList();
            CardCmd.Upgrade(cards,CardPreviewStyle.HorizontalLayout);
            CardCmd.Preview(cards);
            await Cmd.CustomScaledWait(0.5f,1f);
        }
    }
}