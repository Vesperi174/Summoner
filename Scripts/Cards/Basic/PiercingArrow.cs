using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using Summoner.Scripts.Cards.Ancient;

namespace Summoner.Scripts.Cards.Basic;

[RegisterArchaicToothTranscendence(typeof(DarkinArrow))]

public class PiercingArrow() :
    SummonerCard(3, CardType.Attack, CardRarity.Basic, TargetType.AllEnemies)
{
    private const string _increaseKey = "Increase";

    private decimal _extraDamageFromPlays;
    
    
    private decimal ExtraDamageFromPlays
    {
        get
        {
            return _extraDamageFromPlays;
        }
        set
        {
            AssertMutable();
            _extraDamageFromPlays = value;
        }
    }
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(10m, ValueProp.Move),
        new DynamicVar("Increase", 10m)
    ];
    
    public override bool HasTurnEndInHandEffect => true;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .TargetingAllOpponents(base.CombatState)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
    }

    protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
    { 
        base.DynamicVars.Damage.BaseValue += base.DynamicVars["Increase"].BaseValue;
        ExtraDamageFromPlays += base.DynamicVars["Increase"].BaseValue;
    }
    
    protected override void AfterDowngraded()
    {
        base.AfterDowngraded();
        base.DynamicVars.Damage.BaseValue += ExtraDamageFromPlays;
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(2m);
        base.DynamicVars["Increase"].UpgradeValueBy(2m);
    }
}