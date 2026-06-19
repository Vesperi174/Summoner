using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace Summoner.Scripts.Cards.Common;

public class SiphoningStrike() :
    SummonerCard(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    private const string _increaseKey = "Increase";

    private const int _baseDamage = 1;

    private int _currentDamage = 1;

    private int _increasedDamage;

    [SavedProperty]
    public int CurrentDamage
    {
        get
        {
            return _currentDamage;
        }
        set
        {
            AssertMutable();
            _currentDamage = value;
            base.DynamicVars.Damage.BaseValue = _currentDamage;
        }
    }

    [SavedProperty]
    public int IncreasedDamage
    {
        get
        {
            return _increasedDamage;
        }
        set
        {
            AssertMutable();
            _increasedDamage = value;
        }
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(CurrentDamage, ValueProp.Move),
        new IntVar("Increase", 2m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        int intValue = base.DynamicVars["Increase"].IntValue;
        BuffFromPlay(intValue);
        (base.DeckVersion as SiphoningStrike)?.BuffFromPlay(intValue);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["Increase"].UpgradeValueBy(1m);
    }

    protected override void AfterDowngraded()
    {
        UpdateDamage();
    }

    private void BuffFromPlay(int extraDamage)
    {
        IncreasedDamage += extraDamage;
        UpdateDamage();
    }

    private void UpdateDamage()
    {
        CurrentDamage = 1 + IncreasedDamage;
    }
}