using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E06 RID: 7686
	public class Instruction_BuildNearRoom : Instruction_BuildAtRoom
	{
		// Token: 0x170024E0 RID: 9440
		// (get) Token: 0x0600C7BE RID: 51134 RVA: 0x004950E1 File Offset: 0x004932E1
		protected override CellRect BuildableRect
		{
			get
			{
				return Find.TutorialState.roomRect.ExpandedBy(10);
			}
		}

		// Token: 0x0600C7BF RID: 51135 RVA: 0x004950F4 File Offset: 0x004932F4
		protected override bool AllowBuildAt(IntVec3 c)
		{
			return base.AllowBuildAt(c) && !Find.TutorialState.roomRect.Contains(c);
		}
	}
}
