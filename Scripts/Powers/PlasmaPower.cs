using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Scaffolding.Content;

namespace Summoner.Scripts.Powers;

public class PlasmaPower : SummonerPower
{
    public override PowerAssetProfile AssetProfile => new(
        IconPath: "res://Summoner/Images/Powers/PlasmaPower.png",
        BigIconPath: "res://Summoner/Images/Powers/PlasmaPower.png"
    );
    
    private const int _threshold = 5;
    private const double DAMAGE_PERCENTAGE = 0.2;
    
    public override PowerType Type => PowerType.Debuff;
    
    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override bool AllowNegative => false;

    protected override List<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.Static(StaticHoverTip.Block)
    ];

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power == this && amount > 0)
        {
            if (base.Amount >= _threshold)
            {
                
                var owner = base.Owner;
                int lostHp = owner.MaxHp - owner.CurrentHp;

                int damage = (int)(lostHp * DAMAGE_PERCENTAGE);
                
                if (damage > 0)
                {
                    await CreatureCmd.Damage(choiceContext, base.Owner, damage, ValueProp.Unpowered, base.Owner, null);
                }
                
                PowerCmd.Remove(this);
            }            
        }

    }
}