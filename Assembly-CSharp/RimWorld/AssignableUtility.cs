using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200181E RID: 6174
	public static class AssignableUtility
	{
		// Token: 0x06009A7B RID: 39547 RVA: 0x0037BC24 File Offset: 0x00379E24
		public static Pawn GetAssignedPawn(this Building building)
		{
			CompAssignableToPawn compAssignableToPawn = building.TryGetComp<CompAssignableToPawn>();
			if (compAssignableToPawn == null || !compAssignableToPawn.AssignedPawnsForReading.Any<Pawn>())
			{
				return null;
			}
			return compAssignableToPawn.AssignedPawnsForReading[0];
		}

		// Token: 0x06009A7C RID: 39548 RVA: 0x0037BC58 File Offset: 0x00379E58
		public static IEnumerable<Pawn> GetAssignedPawns(this Building building)
		{
			CompAssignableToPawn compAssignableToPawn = building.TryGetComp<CompAssignableToPawn>();
			if (compAssignableToPawn == null || !compAssignableToPawn.AssignedPawnsForReading.Any<Pawn>())
			{
				return null;
			}
			return compAssignableToPawn.AssignedPawns;
		}
	}
}
