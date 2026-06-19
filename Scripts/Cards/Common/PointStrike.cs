using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Summoner.Scripts.Cards.Common;

public class PointStrike() :
    SummonerCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
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

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(12m, ValueProp.Move)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var target = cardPlay.Target;
        if (target != null)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
        }

        // 减少此牌在本场战斗中的伤害
        base.DynamicVars.Damage.BaseValue -= 2m;
        ExtraDamageFromPlays -= 2m;

        // 将此牌放到抽牌堆顶
        await CardPileCmd.Add(this, PileType.Draw, CardPilePosition.Top);
    }

    protected override void AfterDowngraded()
    {
        base.AfterDowngraded();
        base.DynamicVars.Damage.BaseValue += ExtraDamageFromPlays;
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(4m);
    }
}