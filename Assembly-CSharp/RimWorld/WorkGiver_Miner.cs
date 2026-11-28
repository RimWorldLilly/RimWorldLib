using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C27 RID: 3111
	public class WorkGiver_Miner : WorkGiver_Scanner
	{
		// Token: 0x17000F80 RID: 3968
		// (get) Token: 0x060058A2 RID: 22690 RVA: 0x0020C220 File Offset: 0x0020A420
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060058A3 RID: 22691 RVA: 0x0020C223 File Offset: 0x0020A423
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x060058A4 RID: 22692 RVA: 0x0020C226 File Offset: 0x0020A426
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return MineAIUtility.PotentialMineables(pawn);
		}

		// Token: 0x060058A5 RID: 22693 RVA: 0x0020C22E File Offset: 0x0020A42E
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Mine) && !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.MineVein);
		}

		// Token: 0x060058A6 RID: 22694 RVA: 0x0020C261 File Offset: 0x0020A461
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return MineAIUtility.JobOnThing(pawn, t, forced);
		}
	}
}
