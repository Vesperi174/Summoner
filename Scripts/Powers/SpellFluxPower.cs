using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Scaffolding.Content;
using Summoner.Scripts.Cards.Common;

namespace Summoner.Scripts.Powers;

public class SpellFluxPower : SummonerPower
{
    public override PowerAssetProfile AssetProfile => new(
        IconPath: "res://Summoner/Images/Powers/SpellFluxPower.png",
        BigIconPath: "res://Summoner/Images/Powers/SpellFluxPower.png"
    );
    
    private static bool _isOverloadChain = false;
    private static int _chainDepth = 0;
    
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target == base.Owner && result.TotalDamage > 0 && base.Amount > 0)
        {
            // 判断是否是第一次触发伤害链（来自Overload卡）
            bool isInitialOverload = !_isOverloadChain && cardSource != null && cardSource is Overload;
            
            // 如果是首次由Overload触发，则设置标志并增加深度计数
            if (isInitialOverload)
            {
                _isOverloadChain = true;
                _chainDepth = 1;
            }
            else if (_isOverloadChain)
            {
                // 如果已经在Overload伤害链中，则增加深度
                _chainDepth++;
            }
            
            try
            {
                // 判断当前伤害链是否应该加倍
                bool shouldDoubleDamage = _isOverloadChain;
                
                // 输出日志：哪个角色受到了伤害
                // Log.Info($"法术涌动: {base.Owner.Name} 受到了 {result.TotalDamage} 点伤害，当前法术涌动层数: {base.Amount}，伤害链深度: {_chainDepth}，伤害链是否加倍: {shouldDoubleDamage}");
                
                // 获取所有拥有"法术涌动"的其他角色
                var creaturesWithSpellFlux = base.Owner.CombatState.Creatures
                    .Where(c => c != base.Owner && c.HasPower<SpellFluxPower>())
                    .ToList();

                // 输出日志：有多少其他角色拥有法术涌动
                // Log.Info($"法术涌动: 发现 {creaturesWithSpellFlux.Count} 个其他拥有法术涌动的角色");

                // 计算传递的伤害值（如果在Overload伤害链中则加倍）
                int damageToPass = shouldDoubleDamage ? (int)(result.TotalDamage * 2) : (int)result.TotalDamage;

                // 减少一层法术涌动
                // Log.Info($"法术涌动: {base.Owner.Name} 的法术涌动层数减少1");
                await PowerCmd.Decrement(this);
                
                // 对每个拥有"法术涌动"的角色造成等量伤害
                for (int i = 0; i < creaturesWithSpellFlux.Count; i++)
                {
                    var creature = creaturesWithSpellFlux[i];
                    
                    if (i < creaturesWithSpellFlux.Count - 1) // 最后一次不需要等待
                    {
                        await Cmd.Wait(0.3f);
                    }
                    
                    await CreatureCmd.Damage(choiceContext, creature, damageToPass, ValueProp.Unpowered, base.Owner, null);
                    

                }
                
            }
            finally
            {
                // 减少深度计数
                if (_chainDepth > 0)
                {
                    _chainDepth--;
                }
                
                // 如果深度为0且原本是由Overload触发的，则重置标志
                if (_chainDepth == 0 && _isOverloadChain)
                {
                    _isOverloadChain = false;
                }
            }
        }
    }
    
}