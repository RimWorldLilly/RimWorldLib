using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E93 RID: 3731
	public class ThoughtWorker_EntityInRoom : ThoughtWorker_EntityInRoomBase
	{
		// Token: 0x06006272 RID: 25202 RVA: 0x00235FF8 File Offset: 0x002341F8
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.IsPrisoner)
			{
				return ThoughtState.Inactive;
			}
			if (!p.IsColonist)
			{
				return ThoughtState.Inactive;
			}
			return base.CurrentStateInternal(p);
		}
	}
}
