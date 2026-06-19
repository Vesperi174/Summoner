using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Summoner.Scripts.Powers;

namespace Summoner.Scripts.Cards.Uncommon;

public class IcathianRain() :
    SummonerCard(2, CardType.Attack, CardRarity.Uncommon, TargetType.RandomEnemy)
{
    public override List<CardKeyword> CanonicalKeywords => [
        CardKeyword.Ethereal
    ];
    
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<PlasmaPower>()
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(2m, ValueProp.Move),
        new RepeatVar(6)
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int hitCount = base.DynamicVars.Repeat.IntValue;

        for (int i = 0; i < hitCount; i++)
        {
            var hittableEnemies = base.CombatState.HittableEnemies;

            if (hittableEnemies.Count > 0)
            {
                // 随机选择
                var randomEnemy = base.Owner.RunState.Rng.CombatTargets.NextItem(hittableEnemies);
                
                await Cmd.Wait(0.1f);

                await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this)
                    .Targeting(randomEnemy)
                    .WithHitFx("vfx/vfx_attack_slash")
                    .Execute(choiceContext);
                

                await PowerCmd.Apply<PlasmaPower>(choiceContext, randomEnemy, 1m, base.Owner.Creature, this);
            }
        }
        
    }
    
    protected override void OnUpgrade()
    {
        base.DynamicVars.Repeat.UpgradeValueBy(2m);
    }
}