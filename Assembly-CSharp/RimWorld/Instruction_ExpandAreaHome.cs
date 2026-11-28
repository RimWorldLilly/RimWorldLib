using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E0E RID: 7694
	public class Instruction_ExpandAreaHome : Instruction_ExpandArea
	{
		// Token: 0x170024EA RID: 9450
		// (get) Token: 0x0600C7F0 RID: 51184 RVA: 0x00495CE5 File Offset: 0x00493EE5
		protected override Area MyArea
		{
			get
			{
				return base.Map.areaManager.Home;
			}
		}
	}
}
