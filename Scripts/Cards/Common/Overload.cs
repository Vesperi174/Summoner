
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using Summoner.Scripts.Powers;

namespace Summoner.Scripts.Cards.Common;

public class Overload() :
    SummonerCard(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(2m, ValueProp.Move)
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var target = cardPlay.Target;
        if (target != null)
        {
            // 检查目标是否具有SpellFluxPower
            decimal damage = base.DynamicVars.Damage.BaseValue;
            if (target.HasPower<SpellFluxPower>())
            {
                // 对具有SpellFluxPower的单位造成双倍伤害
                damage *= 2;
            }
            
            await DamageCmd.Attack(damage).FromCard(this)
                .Targeting(target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(1m);
    }
    public static async Task<CardModel?> CreateInHand(Player owner, ICombatState combatState)
    {
        return (await CreateInHand(owner, 1, combatState)).FirstOrDefault();
    }

    public static async Task<IEnumerable<CardModel>> CreateInHand(Player owner, int count, ICombatState combatState)
    {
        if (count == 0)
        {
            return Array.Empty<CardModel>();
        }
        if (CombatManager.Instance.IsOverOrEnding)
        {
            return Array.Empty<CardModel>();
        }
        List<CardModel> overloads = new List<CardModel>();
        for (int i = 0; i < count; i++)
        {
            overloads.Add(combatState.CreateCard<Overload>(owner));
        }
        await CardPileCmd.AddGeneratedCardsToCombat(overloads, PileType.Hand, owner);
        return overloads;
    }
    
}