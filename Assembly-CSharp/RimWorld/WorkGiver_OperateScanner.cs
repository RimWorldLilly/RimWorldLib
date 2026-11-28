using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C29 RID: 3113
	public class WorkGiver_OperateScanner : WorkGiver_Scanner
	{
		// Token: 0x17000F82 RID: 3970
		// (get) Token: 0x060058AE RID: 22702 RVA: 0x0020C2EB File Offset: 0x0020A4EB
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(this.ScannerDef);
			}
		}

		// Token: 0x17000F83 RID: 3971
		// (get) Token: 0x060058AF RID: 22703 RVA: 0x0020C2F8 File Offset: 0x0020A4F8
		public ThingDef ScannerDef
		{
			get
			{
				return this.def.scannerDef;
			}
		}

		// Token: 0x17000F84 RID: 3972
		// (get) Token: 0x060058B0 RID: 22704 RVA: 0x0020C305 File Offset: 0x0020A505
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.InteractionCell;
			}
		}

		// Token: 0x060058B1 RID: 22705 RVA: 0x0020C308 File Offset: 0x0020A508
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x060058B2 RID: 22706 RVA: 0x0020C30C File Offset: 0x0020A50C
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.Faction != pawn.Faction)
			{
				return false;
			}
			Building building = t as Building;
			if (building == null)
			{
				return false;
			}
			if (!pawn.CanReserve(building, 1, -1, null, forced))
			{
				return false;
			}
			AcceptanceReport canUseNow = building.TryGetComp<CompScanner>().CanUseNow;
			if (!canUseNow)
			{
				if (!canUseNow.Reason.NullOrEmpty())
				{
					JobFailReason.Is(canUseNow.Reason, null);
				}
				return false;
			}
			return !building.IsBurning();
		}

		// Token: 0x060058B3 RID: 22707 RVA: 0x0020C385 File Offset: 0x0020A585
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.OperateScanner, t, 1500, true);
		}
	}
}
