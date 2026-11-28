using System;

namespace Verse
{
	// Token: 0x020002DC RID: 732
	public class ScattererValidator_Roof : ScattererValidator
	{
		// Token: 0x0600163A RID: 5690 RVA: 0x000854C8 File Offset: 0x000836C8
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
					if (map.roofGrid.RoofAt(c2) != this.roofDef)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0400133A RID: 4922
		public int radius = 1;

		// Token: 0x0400133B RID: 4923
		public RoofDef roofDef;
	}
}
