using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C16 RID: 3094
	public class WorkGiver_HaulGeneral : WorkGiver_Haul
	{
		// Token: 0x0600584A RID: 22602 RVA: 0x0020A971 File Offset: 0x00208B71
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t is Corpse)
			{
				return null;
			}
			return base.JobOnThing(pawn, t, forced);
		}
	}
}
