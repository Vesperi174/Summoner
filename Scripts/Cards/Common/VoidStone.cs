using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Summoner.Scripts.Powers;

namespace Summoner.Scripts.Cards.Common;

public class VoidStone() :
    SummonerCard(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override List<CardKeyword> CanonicalKeywords => [
        CardKeyword.Ethereal
    ];
    
    protected override bool HasEnergyCostX => true;

    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<ManaPower>()
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new IntVar("Mana", 10m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 获取X值（玩家的能量X值）
        int xValue = ResolveEnergyXValue();
        
        // 获取每份法力值
        int manaPerX = base.DynamicVars["Mana"].IntValue;
        
        // 计算总法力值
        int totalMana = xValue * manaPerX;
        
        // 获得法力
        if (totalMana > 0)
        {
            await PowerCmd.Apply<ManaPower>(
                choiceContext,
                base.Owner.Creature,
                totalMana,
                base.Owner.Creature,
                this);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["Mana"].UpgradeValueBy(2m);
    }
}