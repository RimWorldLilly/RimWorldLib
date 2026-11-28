using System;

namespace Verse
{
	// Token: 0x020002D7 RID: 727
	public class ScattererValidator_NoNonNaturalEdifices : ScattererValidator
	{
		// Token: 0x0600162F RID: 5679 RVA: 0x00085038 File Offset: 0x00083238
		public override bool Allows(IntVec3 c, Map map)
		{
			CellRect cellRect = CellRect.CenteredOn(c, this.radius);
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					if (new IntVec3(j, 0, i).GetEdifice(map) != null)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x04001333 RID: 4915
		public int radius = 1;
	}
}
