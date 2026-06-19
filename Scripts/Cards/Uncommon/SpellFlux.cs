using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Summoner.Scripts.Cards.Common;
using Summoner.Scripts.Powers;

namespace Summoner.Scripts.Cards.Uncommon;

public class SpellFlux() :
    SummonerCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies)
{
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<SpellFluxPower>(),
        HoverTipFactory.FromCard<Overload>()
    ];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new IntVar("SpellFlux", 1m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 给所有敌人施加法术涌动
        int fluxAmount = base.DynamicVars["SpellFlux"].IntValue;
        foreach (var enemy in base.CombatState.HittableEnemies)
        {
            await PowerCmd.Apply<SpellFluxPower>(choiceContext, enemy, fluxAmount, base.Owner.Creature, this);
        }
        
        // 添加一张超负荷到手牌
        await Overload.CreateInHand(base.Owner, base.CombatState);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["SpellFlux"].UpgradeValueBy(1m);
    }
}