using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E05 RID: 7685
	public class Instruction_BuildInRoom : Instruction_BuildAtRoom
	{
		// Token: 0x170024DF RID: 9439
		// (get) Token: 0x0600C7BC RID: 51132 RVA: 0x004950C7 File Offset: 0x004932C7
		protected override CellRect BuildableRect
		{
			get
			{
				return Find.TutorialState.roomRect.ContractedBy(1);
			}
		}
	}
}
