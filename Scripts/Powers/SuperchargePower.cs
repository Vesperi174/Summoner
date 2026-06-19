using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Scaffolding.Content;

namespace Summoner.Scripts.Powers;

public class SuperchargePower : SummonerPower
{
    public override PowerAssetProfile AssetProfile => new(
        IconPath: "res://Summoner/Images/Powers/SuperchargePower.png",
        BigIconPath: "res://Summoner/Images/Powers/SuperchargePower.png"
    );
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterEnergySpent(CardModel card, int amount)
    {
        
        for (int i = 0; i < amount; i++)
        {
            var hittableEnemies = base.Owner.CombatState.HittableEnemies;
            if (card.Owner.Creature == base.Owner && amount > 0)
            {
                var randomEnemy = base.Owner.Player.RunState.Rng.CombatTargets.NextItem(hittableEnemies);
                await PowerCmd.Apply<PlasmaPower>(new ThrowingPlayerChoiceContext(), randomEnemy, base.Amount, base.Owner, null);
                
            }
        }
    }
    
    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (participants.Contains(base.Owner) && side == CombatSide.Player)
        {
            // 玩家回合结束后移除这个Power
            await PowerCmd.Remove(this);
        }
    }
}