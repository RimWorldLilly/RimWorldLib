using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020002D8 RID: 728
	public class ScattererValidator_AvoidThingsOfDef : ScattererValidator
	{
		// Token: 0x06001631 RID: 5681 RVA: 0x000850A0 File Offset: 0x000832A0
		public override bool Allows(IntVec3 c, Map map)
		{
			foreach (ThingDef def in this.thingsToAvoid)
			{
				foreach (Thing thing in map.listerThings.ThingsOfDef(def))
				{
					if (c.InHorDistOf(thing.Position, (float)this.radius))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x04001334 RID: 4916
		public int radius = 1;

		// Token: 0x04001335 RID: 4917
		public List<ThingDef> thingsToAvoid = new List<ThingDef>();
	}
}
