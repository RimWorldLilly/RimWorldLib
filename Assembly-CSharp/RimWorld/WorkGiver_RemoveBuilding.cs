using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BCC RID: 3020
	public abstract class WorkGiver_RemoveBuilding : WorkGiver_Scanner
	{
		// Token: 0x17000F30 RID: 3888
		// (get) Token: 0x060056EE RID: 22254
		protected abstract DesignationDef Designation { get; }

		// Token: 0x17000F31 RID: 3889
		// (get) Token: 0x060056EF RID: 22255
		protected abstract JobDef RemoveBuildingJob { get; }

		// Token: 0x17000F32 RID: 3890
		// (get) Token: 0x060056F0 RID: 22256 RVA: 0x00204925 File Offset: 0x00202B25
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060056F1 RID: 22257 RVA: 0x00204928 File Offset: 0x00202B28
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			WorkGiver_RemoveBuilding.<PotentialWorkThingsGlobal>d__6 <PotentialWorkThingsGlobal>d__ = new WorkGiver_RemoveBuilding.<PotentialWorkThingsGlobal>d__6(-2);
			<PotentialWorkThingsGlobal>d__.<>4__this = this;
			<PotentialWorkThingsGlobal>d__.<>3__pawn = pawn;
			return <PotentialWorkThingsGlobal>d__;
		}

		// Token: 0x060056F2 RID: 22258 RVA: 0x0020493F File Offset: 0x00202B3F
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(this.Designation);
		}

		// Token: 0x060056F3 RID: 22259 RVA: 0x0020495C File Offset: 0x00202B5C
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			if (pawn.Map.designationManager.DesignationOn(t, this.Designation) == null)
			{
				return false;
			}
			CompExplosive compExplosive;
			if (t.TryGetComp(out compExplosive) && compExplosive.wickStarted)
			{
				JobFailReason.Is("AboutToExplode".Translate(), null);
				return false;
			}
			return true;
		}

		// Token: 0x060056F4 RID: 22260 RVA: 0x002049C2 File Offset: 0x00202BC2
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(this.RemoveBuildingJob, t);
		}
	}
}
