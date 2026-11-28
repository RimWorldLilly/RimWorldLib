using System;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B8B RID: 2955
	public class JoyGiver_InPrivateRoom : JoyGiver
	{
		// Token: 0x060055C4 RID: 21956 RVA: 0x001FF030 File Offset: 0x001FD230
		public override Job TryGiveJob(Pawn pawn)
		{
			JoyGiver_InPrivateRoom.<>c__DisplayClass0_0 CS$<>8__locals1 = new JoyGiver_InPrivateRoom.<>c__DisplayClass0_0();
			CS$<>8__locals1.pawn = pawn;
			Room room = null;
			if (ModsConfig.IdeologyActive)
			{
				room = MeditationUtility.UsableWorshipRooms(CS$<>8__locals1.pawn).RandomElementWithFallback(null);
			}
			if (room == null)
			{
				Pawn_Ownership ownership = CS$<>8__locals1.pawn.ownership;
				room = ((ownership != null) ? ownership.OwnedRoom : null);
			}
			if (room == null)
			{
				return null;
			}
			IntVec3 c;
			if (!room.Cells.Where(new Func<IntVec3, bool>(CS$<>8__locals1.<TryGiveJob>g__Validator|0)).TryRandomElement(out c))
			{
				return null;
			}
			return JobMaker.MakeJob(this.def.jobDef, c);
		}

		// Token: 0x060055C5 RID: 21957 RVA: 0x001FF0BD File Offset: 0x001FD2BD
		public override Job TryGiveJobWhileInBed(Pawn pawn)
		{
			return JobMaker.MakeJob(this.def.jobDef, pawn.CurrentBed());
		}
	}
}
