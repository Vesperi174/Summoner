using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Summoner.Scripts.Cards.Colorless;

[RegisterCard(typeof(ColorlessCardPool))]
public class AssassinMark : ModCardTemplate
{
    public override List<CardKeyword> CanonicalKeywords => [
        CardKeyword.Exhaust
    ];
    // 基础耗能
    private const int energyCost = 1;
    // 卡牌类型
    private const CardType type = CardType.Attack;
    // 卡牌稀有度
    private const CardRarity rarity = CardRarity.Common;
    // 目标类型（AnyEnemy表示任意敌人）
    private const TargetType targetType = TargetType.AnyEnemy;
    // 是否在卡牌图鉴中显示
    private const bool shouldShowInCardLibrary = true;

    // 卡图资源
    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"res://Summoner/Images/Cards/{GetType().Name}.png"
    );

    // 卡牌基础数值
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(3, ValueProp.Move)
    ];
    
    public AssassinMark() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var target = cardPlay.Target;
        if (target != null)
        {
            // 计算基础伤害
            decimal baseDamage = base.DynamicVars.Damage.BaseValue;
            
            // 计算目标当前生命值的5%
            decimal currentHpPercent = target.CurrentHp * 0.05m;
            
            // 计算总伤害
            decimal totalDamage = baseDamage + currentHpPercent;
            
            // 对目标造成伤害
            await DamageCmd.Attack(totalDamage)
                .FromCard(this)
                .Targeting(target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(2m);
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
        List<CardModel> assassinMarks = new List<CardModel>();
        for (int i = 0; i < count; i++)
        {
            assassinMarks.Add(combatState.CreateCard<AssassinMark>(owner));
        }
        await CardPileCmd.AddGeneratedCardsToCombat(assassinMarks, PileType.Hand, owner);
        return assassinMarks;
    }
}