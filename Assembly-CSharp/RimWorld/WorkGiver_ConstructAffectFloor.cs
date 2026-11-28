using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BC0 RID: 3008
	public abstract class WorkGiver_ConstructAffectFloor : WorkGiver_Scanner
	{
		// Token: 0x17000F23 RID: 3875
		// (get) Token: 0x060056A6 RID: 22182
		protected abstract DesignationDef DesDef { get; }

		// Token: 0x17000F24 RID: 3876
		// (get) Token: 0x060056A7 RID: 22183 RVA: 0x00203204 File Offset: 0x00201404
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060056A8 RID: 22184 RVA: 0x00203207 File Offset: 0x00201407
		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			WorkGiver_ConstructAffectFloor.<PotentialWorkCellsGlobal>d__4 <PotentialWorkCellsGlobal>d__ = new WorkGiver_ConstructAffectFloor.<PotentialWorkCellsGlobal>d__4(-2);
			<PotentialWorkCellsGlobal>d__.<>4__this = this;
			<PotentialWorkCellsGlobal>d__.<>3__pawn = pawn;
			return <PotentialWorkCellsGlobal>d__;
		}

		// Token: 0x060056A9 RID: 22185 RVA: 0x0020321E File Offset: 0x0020141E
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(this.DesDef);
		}

		// Token: 0x060056AA RID: 22186 RVA: 0x00203239 File Offset: 0x00201439
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return pawn.Map.designationManager.DesignationAt(c, this.DesDef) != null && pawn.CanReserve(c, 1, -1, ReservationLayerDefOf.Floor, forced);
		}
	}
}
