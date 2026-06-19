using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Summoner.Scripts.Powers;

namespace Summoner.Scripts.Cards.Uncommon;

public class Eclipse() :
    SummonerCard(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DynamicVar("DamageAdd", 5m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<EclipsePower>(choiceContext, base.Owner.Creature, base.DynamicVars["DamageAdd"].BaseValue, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["DamageAdd"].UpgradeValueBy(2m);
    }
}