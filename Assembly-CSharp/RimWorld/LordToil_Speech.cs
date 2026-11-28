using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D13 RID: 3347
	public class LordToil_Speech : LordToil_Ritual
	{
		// Token: 0x17001078 RID: 4216
		// (get) Token: 0x06005E0F RID: 24079 RVA: 0x00226E4E File Offset: 0x0022504E
		public new LordToilData_Speech Data
		{
			get
			{
				return (LordToilData_Speech)this.data;
			}
		}

		// Token: 0x06005E10 RID: 24080 RVA: 0x00226E5B File Offset: 0x0022505B
		public LordToil_Speech(IntVec3 spot, Precept_Ritual ritual, LordJob_Ritual lordJob, Pawn organizer) : base(spot, lordJob, null, organizer)
		{
			this.organizer = organizer;
			this.data = new LordToilData_Speech();
		}

		// Token: 0x06005E11 RID: 24081 RVA: 0x00226E7C File Offset: 0x0022507C
		public override void Init()
		{
			base.Init();
			this.Data.spectateRect = CellRect.CenteredOn(this.spot, 0);
			Rot4 rotation = this.spot.GetFirstThing(this.organizer.MapHeld).Rotation;
			SpectateRectSide asSpectateSide = rotation.Opposite.AsSpectateSide;
			this.Data.spectateRectAllowedSides = (SpectateRectSide.All & ~asSpectateSide);
			this.Data.spectateRectPreferredSide = rotation.AsSpectateSide;
		}

		// Token: 0x06005E12 RID: 24082 RVA: 0x00226EF4 File Offset: 0x002250F4
		public override ThinkTreeDutyHook VoluntaryJoinDutyHookFor(Pawn p)
		{
			if (p == this.organizer)
			{
				return DutyDefOf.GiveSpeech.hook;
			}
			return DutyDefOf.Spectate.hook;
		}

		// Token: 0x06005E13 RID: 24083 RVA: 0x00226F14 File Offset: 0x00225114
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (pawn == this.organizer)
				{
					Building_Throne firstThing = this.spot.GetFirstThing(base.Map);
					pawn.mindState.duty = new PawnDuty(DutyDefOf.GiveSpeech, this.spot, firstThing, -1f);
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
				else
				{
					PawnDuty pawnDuty = new PawnDuty(DutyDefOf.Spectate);
					pawnDuty.spectateRect = this.Data.spectateRect;
					pawnDuty.spectateRectAllowedSides = this.Data.spectateRectAllowedSides;
					pawnDuty.spectateRectPreferredSide = this.Data.spectateRectPreferredSide;
					pawn.mindState.duty = pawnDuty;
				}
			}
		}
	}
}
