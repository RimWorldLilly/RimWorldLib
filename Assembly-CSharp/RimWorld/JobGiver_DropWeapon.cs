using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000AFF RID: 2815
	public class JobGiver_DropWeapon : ThinkNode_JobGiver
	{
		// Token: 0x060053B6 RID: 21430 RVA: 0x001F29BC File Offset: 0x001F0BBC
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.equipment != null)
			{
				foreach (ThingWithComps thingWithComps in pawn.equipment.AllEquipmentListForReading)
				{
					if (thingWithComps.def.IsWeapon)
					{
						return JobMaker.MakeJob(JobDefOf.DropEquipment, thingWithComps);
					}
				}
			}
			return null;
		}
	}
}
