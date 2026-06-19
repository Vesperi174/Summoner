using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Summoner.Scripts.Powers;

namespace Summoner.Scripts.Cards.Uncommon;

public class VoidSeeker() :
    SummonerCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    public override List<CardKeyword> CanonicalKeywords => [
        CardKeyword.Ethereal
    ];
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(8m, ValueProp.Move),
        new IntVar("Plasma", 2m)
    ];
    
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<PlasmaPower>()
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this) //攻击来源
            .Targeting(cardPlay.Target) //攻击目标
            .WithHitFx("vfx/vfx_attack_slash")  //攻击特效
            .Execute(choiceContext);   
        await PowerCmd.Apply<PlasmaPower>(choiceContext, cardPlay.Target, base.DynamicVars["Plasma"].IntValue, base.Owner.Creature, this);//执行攻击效果
    }
    
    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(3m); //升级后加3点伤害
        base.DynamicVars["Plasma"].UpgradeValueBy(1m);
    }
}