using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BB3 RID: 2995
	public abstract class WorkGiver_Scanner : WorkGiver
	{
		// Token: 0x17000F15 RID: 3861
		// (get) Token: 0x0600565D RID: 22109 RVA: 0x00202386 File Offset: 0x00200586
		public virtual ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Undefined);
			}
		}

		// Token: 0x17000F16 RID: 3862
		// (get) Token: 0x0600565E RID: 22110 RVA: 0x0020238E File Offset: 0x0020058E
		public virtual int MaxRegionsToScanBeforeGlobalSearch
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x17000F17 RID: 3863
		// (get) Token: 0x0600565F RID: 22111 RVA: 0x00202391 File Offset: 0x00200591
		public virtual bool Prioritized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005660 RID: 22112 RVA: 0x00202394 File Offset: 0x00200594
		public virtual IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			return Enumerable.Empty<IntVec3>();
		}

		// Token: 0x06005661 RID: 22113 RVA: 0x0020239B File Offset: 0x0020059B
		public virtual IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return null;
		}

		// Token: 0x17000F18 RID: 3864
		// (get) Token: 0x06005662 RID: 22114 RVA: 0x0020239E File Offset: 0x0020059E
		public virtual bool AllowUnreachable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000F19 RID: 3865
		// (get) Token: 0x06005663 RID: 22115 RVA: 0x002023A1 File Offset: 0x002005A1
		public virtual PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06005664 RID: 22116 RVA: 0x002023A4 File Offset: 0x002005A4
		public virtual Danger MaxPathDanger(Pawn pawn)
		{
			return pawn.NormalMaxDanger();
		}

		// Token: 0x06005665 RID: 22117 RVA: 0x002023AC File Offset: 0x002005AC
		public virtual bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return this.JobOnThing(pawn, t, forced) != null;
		}

		// Token: 0x06005666 RID: 22118 RVA: 0x002023BA File Offset: 0x002005BA
		public virtual string JobInfo(Pawn pawn, Job job)
		{
			return string.Empty;
		}

		// Token: 0x06005667 RID: 22119 RVA: 0x002023C1 File Offset: 0x002005C1
		public virtual Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return null;
		}

		// Token: 0x06005668 RID: 22120 RVA: 0x002023C4 File Offset: 0x002005C4
		public virtual bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return this.JobOnCell(pawn, c, forced) != null;
		}

		// Token: 0x06005669 RID: 22121 RVA: 0x002023D2 File Offset: 0x002005D2
		public virtual Job JobOnCell(Pawn pawn, IntVec3 cell, bool forced = false)
		{
			return null;
		}

		// Token: 0x0600566A RID: 22122 RVA: 0x002023D5 File Offset: 0x002005D5
		public virtual float GetPriority(Pawn pawn, TargetInfo t)
		{
			return 0f;
		}

		// Token: 0x0600566B RID: 22123 RVA: 0x002023DC File Offset: 0x002005DC
		public virtual string PostProcessedGerund(Job job)
		{
			return this.def.gerund;
		}

		// Token: 0x0600566C RID: 22124 RVA: 0x002023E9 File Offset: 0x002005E9
		public float GetPriority(Pawn pawn, IntVec3 cell)
		{
			return this.GetPriority(pawn, new TargetInfo(cell, pawn.Map, false));
		}

		// Token: 0x0600566D RID: 22125 RVA: 0x002023FF File Offset: 0x002005FF
		public virtual ReservationLayerDef GetReservationLayer(Pawn pawn, LocalTargetInfo target)
		{
			return null;
		}
	}
}
