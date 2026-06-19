using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Summoner.Scripts.Cards.Rare;

public class Haymaker() :
    SummonerCard(2, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
{
    public override List<CardKeyword> CanonicalKeywords => [
        CardKeyword.Exhaust
    ];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("Percent", 1.05m)
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 计算已损生命值（最大生命值 - 当前生命值）
        int maxHp = base.Owner.Creature.MaxHp;
        int currentHp = base.Owner.Creature.CurrentHp;
        int lostHp = maxHp - currentHp;
        
        if (lostHp > 0)
        {
            // 获取玩家的力量层数
            int strengthAmount = base.Owner.Creature.GetPowerAmount<StrengthPower>();
            
            // 计算最终伤害：基础伤害 + 每点力量的加成
            decimal totalMultiplier = 1m + (strengthAmount * (base.DynamicVars["Percent"].BaseValue - 1m));
            int finalDamage = (int)(lostHp * totalMultiplier);
            
            await DamageCmd.Attack(finalDamage)
                .FromCard(this)
                .TargetingAllOpponents(base.CombatState)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
        }
    }
    
    protected override void OnUpgrade()
    {
        base.DynamicVars["Percent"].UpgradeValueBy(0.05m);
    }
}