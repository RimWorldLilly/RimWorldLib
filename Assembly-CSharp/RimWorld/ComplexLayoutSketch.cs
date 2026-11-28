using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001221 RID: 4641
	public class ComplexLayoutSketch : LayoutSketch
	{
		// Token: 0x06007330 RID: 29488 RVA: 0x00284C78 File Offset: 0x00282E78
		protected override ThingDef GetWallStuff(int roomId)
		{
			if (roomId % 2 != 0)
			{
				return ThingDefOf.Steel;
			}
			return this.wallStuff;
		}
	}
}
