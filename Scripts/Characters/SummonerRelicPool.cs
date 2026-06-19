
using Godot;
using STS2RitsuLib.Scaffolding.Content;

namespace Summoner.Scripts.Characters;

public class SummonerRelicPool : TypeListRelicPoolModel
{
    public override string EnergyColorName => Summoner.CharacterId;

    public override Color LabOutlineColor => Summoner.Color;
}