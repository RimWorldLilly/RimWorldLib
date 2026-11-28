using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E0F RID: 7695
	public class Instruction_ExpandAreaBuildRoof : Instruction_ExpandArea
	{
		// Token: 0x170024EB RID: 9451
		// (get) Token: 0x0600C7F2 RID: 51186 RVA: 0x00495CFF File Offset: 0x00493EFF
		protected override Area MyArea
		{
			get
			{
				return base.Map.areaManager.BuildRoof;
			}
		}
	}
}
