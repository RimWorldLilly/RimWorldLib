using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C45 RID: 3141
	public abstract class WorkGiver_TakeToBed : WorkGiver_Scanner
	{
		// Token: 0x06005950 RID: 22864 RVA: 0x0020E70A File Offset: 0x0020C90A
		protected Building_Bed FindBed(Pawn pawn, Pawn patient)
		{
			return RestUtility.FindBedFor(patient, pawn, false, false, patient.GuestStatus);
		}
	}
}
