using System.Collections.Generic;

namespace Pathfinding.Locations
{
    public abstract class LocationType<T>
    {
        public abstract string Name { get; set; }
        public abstract List<T> Locations { get; set; }

        public virtual void OnSelected()
        {
            
        }

        public virtual void OnUnselected()
        {
            
        }
    }
}