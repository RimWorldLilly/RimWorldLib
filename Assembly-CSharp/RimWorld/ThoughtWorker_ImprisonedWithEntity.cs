using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E94 RID: 3732
	public class ThoughtWorker_ImprisonedWithEntity : ThoughtWorker_EntityInRoomBase
	{
		// Token: 0x06006274 RID: 25204 RVA: 0x00236025 File Offset: 0x00234225
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.IsPrisoner)
			{
				return ThoughtState.Inactive;
			}
			if (!p.guest.PrisonerIsSecure)
			{
				return ThoughtState.Inactive;
			}
			return base.CurrentStateInternal(p);
		}
	}
}
