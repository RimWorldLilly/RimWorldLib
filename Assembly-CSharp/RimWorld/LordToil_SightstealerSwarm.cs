using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CF7 RID: 3319
	public class LordToil_SightstealerSwarm : LordToil_EntitySwarm
	{
		// Token: 0x06005DA6 RID: 23974 RVA: 0x002250AD File Offset: 0x002232AD
		public LordToil_SightstealerSwarm(IntVec3 start, IntVec3 dest) : base(start, dest)
		{
		}

		// Token: 0x06005DA7 RID: 23975 RVA: 0x002250B7 File Offset: 0x002232B7
		protected override DutyDef GetDutyDef()
		{
			return DutyDefOf.SightstealerSwarm;
		}

		// Token: 0x06005DA8 RID: 23976 RVA: 0x002250BE File Offset: 0x002232BE
		public override bool CanAddPawn(Pawn p)
		{
			return p.kindDef == PawnKindDefOf.Sightstealer;
		}
	}
}
