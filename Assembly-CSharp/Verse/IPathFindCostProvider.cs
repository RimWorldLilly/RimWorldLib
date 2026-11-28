using System;

namespace Verse
{
	// Token: 0x02000368 RID: 872
	public interface IPathFindCostProvider
	{
		// Token: 0x06001AC2 RID: 6850
		ushort PathFindCostFor(Pawn pawn);

		// Token: 0x06001AC3 RID: 6851
		CellRect GetOccupiedRect();
	}
}
