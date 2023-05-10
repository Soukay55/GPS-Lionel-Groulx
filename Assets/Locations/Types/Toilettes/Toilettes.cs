using System.Collections.Generic;

namespace Pathfinding.Locations.Types
{
    public class Toilettes : LocationType<ToiletteLocation>
    {
        public override string Name { get; set; } = "Toilettes";

        public override List<ToiletteLocation> Locations { get; set; } = new()
        {
            new() { Identifier = "1", Location = new(3, 5, 7) },
            new() { Identifier = "2", Location = new(3, 5, 7) },
            new() { Identifier = "3", Location = new(3, 5, 7) },
        };

        public override void OnSelected()
        {
            base.OnSelected();
        }
    }
}