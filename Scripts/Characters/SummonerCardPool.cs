using Godot;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Utils;
using Summoner.Scripts.Extensions;

namespace Summoner.Scripts.Characters;

public class SummonerCardPool : TypeListCardPoolModel
{
    public override string Title => Summoner.CharacterId;
    public override string EnergyColorName => Summoner.CharacterId;
    
    // 描述中使用的能量图标。大小为24x24。
    public override string? TextEnergyIconPath => "res://Summoner/Images/Charui/text_energy.png";
    // tooltip和卡牌左上角的能量图标。大小为74x74。
    public override string? BigEnergyIconPath => "res://Summoner/Images/Charui/big_energy.png";
    
    /*
    public override string BigEnergyIconPath => "Charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "Charui/text_energy.png".ImagePath();
    */
    
    // 卡池的主题色。
    public override Color DeckEntryCardColor => new(0.1f, 0.24f, 0.44f);
    // 能量表盘文字轮廓颜色
    public override Color EnergyOutlineColor => new(0.1f, 0.24f, 0.44f);

    // 根据你使用的卡框决定使用哪个Material
    private static readonly Material? _poolFrameMaterial = MaterialUtils.CreateReplaceHueShaderMaterial(0.1f, 0.24f, 0.44f); // 如果你使用原版卡框，使用这个直接替换色调。
    // private static readonly Material? _poolFrameMaterial = MaterialUtils.CreateRgbShaderMaterial(0.5f, 0.5f, 1f); // 使用原版卡框替换色调。除非你的版本没有CreateReplaceHueShaderMaterial函数，否则应使用上面那种
    // private static readonly Material? _poolFrameMaterial = MaterialUtils.CreateUnmodulatedHsvShaderMaterial(); // 如果你是自定义卡框，使用这个
    public override Material? PoolFrameMaterial => _poolFrameMaterial;
    
    /*
    public override float H => 0.69f;
    public override float S => 1.062f;
    public override float V => 0.895f;
    
    public override Color DeckEntryCardColor => new("4a9eff");
    public override Color EnergyOutlineColor => new("1a3b7a");
    */
    public override bool IsColorless => false;
}