using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Summoner.Scripts.Powers;

namespace Summoner.Scripts.Cards.Uncommon;

public class KillerInstinct() :
    SummonerCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
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
        new BlockVar(8m, ValueProp.Move),
        new IntVar("Plasma", 1m)
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 获得格挡
        await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);
        
        // 给予所有敌人电浆
        int plasmaAmount = base.DynamicVars["Plasma"].IntValue;
        foreach (var enemy in base.CombatState.HittableEnemies)
        {
            await PowerCmd.Apply<PlasmaPower>(choiceContext, enemy, plasmaAmount, base.Owner.Creature, this);
        }
    }
    
    protected override void OnUpgrade()
    {
        base.DynamicVars.Block.UpgradeValueBy(3m);
        base.DynamicVars["Plasma"].UpgradeValueBy(1m);
    }
}