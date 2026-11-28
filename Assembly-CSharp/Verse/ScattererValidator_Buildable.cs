using System;

namespace Verse
{
	// Token: 0x020002D6 RID: 726
	public class ScattererValidator_Buildable : ScattererValidator
	{
		// Token: 0x0600162D RID: 5677 RVA: 0x00084FA4 File Offset: 0x000831A4
		public override bool Allows(IntVec3 c, Map map)
		{
			CellRect cellRect = CellRect.CenteredOn(c, this.radius);
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 c2 = new IntVec3(j, 0, i);
					if (!c2.InBounds(map))
					{
						return false;
					}
					if (c2.InNoBuildEdgeArea(map))
					{
						return false;
					}
					if (this.affordance != null && !c2.GetAffordances(map).Contains(this.affordance))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x04001331 RID: 4913
		public int radius = 1;

		// Token: 0x04001332 RID: 4914
		public TerrainAffordanceDef affordance;
	}
}
