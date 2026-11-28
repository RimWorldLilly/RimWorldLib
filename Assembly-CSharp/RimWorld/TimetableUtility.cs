using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015A9 RID: 5545
	public static class TimetableUtility
	{
		// Token: 0x06008952 RID: 35154 RVA: 0x0031BACC File Offset: 0x00319CCC
		public static TimeAssignmentDef GetTimeAssignment(this Pawn pawn)
		{
			if (pawn.timetable == null)
			{
				return TimeAssignmentDefOf.Anything;
			}
			return pawn.timetable.CurrentAssignment;
		}
	}
}
