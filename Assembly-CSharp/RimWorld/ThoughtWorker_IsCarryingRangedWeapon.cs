using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E3F RID: 3647
	public class ThoughtWorker_IsCarryingRangedWeapon : ThoughtWorker
	{
		// Token: 0x06006174 RID: 24948 RVA: 0x00232DDB File Offset: 0x00230FDB
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.equipment.Primary != null && p.equipment.Primary.def.IsRangedWeapon;
		}
	}
}
