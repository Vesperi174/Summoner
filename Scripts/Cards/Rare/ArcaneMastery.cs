using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using Summoner.Scripts.Cards.Common;
using Summoner.Scripts.Powers;

namespace Summoner.Scripts.Cards.Rare;

public class ArcaneMastery() :
    SummonerCard(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override List<CardKeyword> CanonicalKeywords => [
        CardKeyword.Exhaust
    ];
    
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<StrengthPower>(),
        HoverTipFactory.FromCard<Overload>()
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new IntVar("Mana", 3m),
        new PowerVar<StrengthPower>(2m),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int manaCost = base.DynamicVars["Mana"].IntValue; 
        int strengthPerConversion = base.DynamicVars["StrengthPower"].IntValue;
        
        var manaPower = base.Owner.Creature.GetPower<ManaPower>(); 
        
        if (manaPower != null) 
        { 
            // 计算最多能进行多少次转换
            int maxConversions = manaPower.Amount / manaCost;
            
            if (maxConversions > 0)
            {
                // 应用总的力量增益
                int totalStrength = maxConversions * strengthPerConversion;
                await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner.Creature, totalStrength, base.Owner.Creature, this); 

                // 一次性消耗所有需要的法力
                await PowerCmd.ModifyAmount(choiceContext, manaPower, -maxConversions * manaCost, base.Owner.Creature, this); 
            }
        } 
        
        await Overload.CreateInHand(base.Owner, base.CombatState);
        await Overload.CreateInHand(base.Owner, base.CombatState);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["Mana"].UpgradeValueBy(-1m);
    }
}