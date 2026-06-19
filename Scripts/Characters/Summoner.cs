using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.PotionPools;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Characters;
using Summoner.Scripts.Cards.Basic;
using Summoner.Scripts.Extensions;
using Summoner.Scripts.Relics;

namespace Summoner.Scripts.Characters;
[RegisterCharacter]
public class Summoner: ModCharacterTemplate<SummonerCardPool, SummonerRelicPool, SharedPotionPool>
{
    public static readonly Color Color = new Color("0f4280");
    
    public override Color NameColor => Color;
    
    public override Color EnergyLabelOutlineColor => Color;
    
    public override Color MapDrawingColor => Color;
    
    public override CharacterGender Gender => CharacterGender.Masculine;
    
    public override int StartingHp => 75;
    
    public override int StartingGold => 99;
    
    public const string CharacterId = "Summoner";
    
    // public override string PlaceholderID => "necrobinder";
    
    
    public override CharacterAssetProfile AssetProfile => CharacterAssetProfiles.Merge(
        CharacterAssetProfiles.Ironclad(),
        new(
            Scenes: new(
                // 人物模型tscn路径。
                VisualsPath: "res://Summoner/Scenes/summoner_character.tscn",
                // 能量表盘tscn路径。
                EnergyCounterPath: "res://Summoner/Scenes/summoner_energy_counter.tscn",
                // 商店人物场景。
                MerchantAnimPath: "res://Summoner/Scenes/summoner_character_merchant.tscn",
                // 篝火休息场景。
                RestSiteAnimPath: "res://Summoner/Scenes/summoner_rest_site.tscn"
            ),
            Ui: new(
                // 人物头像路径。
                IconTexturePath: "res://Summoner/Images/Charui/character_icon_summoner.png",
                // 人物头像2号。
                IconPath: "res://Summoner/Scenes/ui/summoner_icon.tscn",
                // 人物选择背景。
                CharacterSelectBgPath: "res://Summoner/Scenes/summoner_bg.tscn",
                // 人物选择图标。
                CharacterSelectIconPath: "res://Summoner/Images/Charui/char_select_summoner.png",
                // 人物选择图标-锁定状态。
                CharacterSelectLockedIconPath: "res://Summoner/Images/Charui/char_select_summoner_locked.png",
                // 人物选择过渡动画。
                // CharacterSelectTransitionPath: "res://materials/transitions/ironclad_transition_mat.tres",
                // 地图上的角色标记图标、表情轮盘上的角色头像
                MapMarkerPath: "res://Summoner/Images/Charui/map_marker_summoner.png"
            ),
            Vfx: new(
                // 卡牌拖尾场景。
                // TrailPath: "res://scenes/vfx/card_trail_ironclad.tscn"
            ),
            Audio: new(
                // 攻击音效
                // AttackSfx: null,
                // 施法音效
                // CastSfx: null,
                // 死亡音效
                // DeathSfx: null,
                // 角色选择音效
                // CharacterSelectSfx: null,
                // 过渡音效
                // CharacterTransitionSfx: "event:/sfx/ui/wipe_ironclad"
            ),
            Multiplayer: new(
                // 多人模式-手指。
                ArmPointingTexturePath: "res://Summoner/Images/hands/multiplayer_hand_summoner_point.png",
                // 多人模式剪刀石头布-石头。
                ArmRockTexturePath: "res://Summoner/Images/hands/multiplayer_hand_summoner_rock.png",
                // 多人模式剪刀石头布-布。
                ArmPaperTexturePath: "res://Summoner/Images/hands/multiplayer_hand_summoner_paper.png",
                // 多人模式剪刀石头布-剪刀。
                ArmScissorsTexturePath: "res://Summoner/Images/hands/multiplayer_hand_summoner_scissors.png"
            )
            // 其余如果有需要自行填写
            // Spine: null,
            // VisualCues: null, // 帧动画静态图人物使用，查看角色动画一章
            // WorldProceduralVisuals: null,
            // VanillaCardVisualOverrides: [],
            // VanillaRelicVisualOverrides: [
            //     new (CharacterOwnedVanillaRelicModelId.YummyCookie, new("res://icon.svg")) // 美味饼干覆盖
            // ],
            // VanillaPotionVisualOverrides: []
        ));
    
    // 攻击和施法动画延迟，以对齐动画
    public override float AttackAnimDelay => 0f;
    public override float CastAnimDelay => 0f;
    
    
    
    
    protected override IEnumerable<StartingDeckEntry> StartingDeckEntries => 
    [
        new(typeof(DefendSummoner), 5),
        new(typeof(StrikeSummoner),5),
        new(typeof(PiercingArrow)),
        new(typeof(Inspiration)),
        /*
        ModelDb.Card<DefendSummoner>(),
        ModelDb.Card<DefendSummoner>(),
        ModelDb.Card<DefendSummoner>(),
        ModelDb.Card<DefendSummoner>(),
        ModelDb.Card<DefendSummoner>(),
        
        ModelDb.Card<StrikeSummoner>(),
        ModelDb.Card<StrikeSummoner>(),
        ModelDb.Card<StrikeSummoner>(),
        ModelDb.Card<StrikeSummoner>(),
        ModelDb.Card<StrikeSummoner>(),
        
        ModelDb.Card<PiercingArrow>(),
        ModelDb.Card<Inspiration>()
        */
        
    ];
    
    protected override IEnumerable<Type> StartingRelicTypes => 
    [
        typeof(SummonerCrown)
        // ModelDb.Relic<SummonerCrown>()
    ];
    
    public override List<string> GetArchitectAttackVfx() => [
        "vfx/vfx_attack_blunt",
        "vfx/vfx_heavy_blunt",
        "vfx/vfx_attack_slash",
        "vfx/vfx_bloody_impact",
        "vfx/vfx_rock_shatter"
    ];
    
    public override bool RequiresEpochAndTimeline => false;
    /*
    public override CardPoolModel CardPool => ModelDb.CardPool<SummonerCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<SummonerRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<SharedPotionPool>();
    
    
    public override string CustomVisualPath => "";
    public override string CustomIconTexturePath => "character_icon_summoner.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_summoner.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_summoner_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_summoner.png".CharacterUiPath();
    
    public override string CustomCharacterSelectBg => "";
    public override string CustomEnergyCounterPath => "";
    public override string CustomMerchantAnimPath => "";
    public override string CustomRestSiteAnimPath => "";
    
    // 多人模式-手指。
    public override string CustomArmPointingTexturePath => "res://Summoner/Images/hands/multiplayer_hand_summoner_point.png";
    // 多人模式剪刀石头布-石头。
    public override string CustomArmRockTexturePath => "res://Summoner/Images/hands/multiplayer_hand_summoner_rock.png";
    // 多人模式剪刀石头布-布。
    public override string CustomArmPaperTexturePath => "res://Summoner/Images/hands/multiplayer_hand_summoner_paper.png";
    // 多人模式剪刀石头布-剪刀。
    public override string CustomArmScissorsTexturePath => "res://Summoner/Images/hands/multiplayer_hand_summoner_scissors.png";
    */
    
    // public override string CustomTrailPath => "res://Summoner/Scenes/vfx/card_trail_summoner.tscn";
    
    public override string CustomIconPath => "res://Summoner/Scenes/ui/summoner_icon.tscn";
}