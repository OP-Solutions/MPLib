using System;
using System.Collections.Generic;
using System.Text;

namespace MPLib.Core.Game
{
    public class Game
    {
        public virtual string FullName { get; internal set; }
        public virtual Guid Guid { get; }
        public virtual Version Version { get; internal set; }
    }
}
