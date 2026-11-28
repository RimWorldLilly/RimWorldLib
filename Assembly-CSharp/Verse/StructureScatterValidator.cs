using System;

namespace Verse
{
	// Token: 0x020002D3 RID: 723
	public class StructureScatterValidator : ScattererValidator
	{
		// Token: 0x0600161D RID: 5661 RVA: 0x000846E0 File Offset: 0x000828E0
		public override bool Allows(IntVec3 c, Map map)
		{
			return c.InHorDistOf(this.structureRect.CenterCell, this.maxDistFromStructure);
		}

		// Token: 0x0400131B RID: 4891
		public CellRect structureRect;

		// Token: 0x0400131C RID: 4892
		public float maxDistFromStructure;
	}
}
