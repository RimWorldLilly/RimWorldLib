using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CB0 RID: 3248
	public class LordJob_Joinable_Concert : LordJob_Joinable_Party
	{
		// Token: 0x17001007 RID: 4103
		// (get) Token: 0x06005C2D RID: 23597 RVA: 0x0021E5A6 File Offset: 0x0021C7A6
		protected override ThoughtDef AttendeeThought
		{
			get
			{
				return ThoughtDefOf.AttendedConcert;
			}
		}

		// Token: 0x17001008 RID: 4104
		// (get) Token: 0x06005C2E RID: 23598 RVA: 0x0021E5AD File Offset: 0x0021C7AD
		protected override TaleDef AttendeeTale
		{
			get
			{
				return TaleDefOf.AttendedConcert;
			}
		}

		// Token: 0x17001009 RID: 4105
		// (get) Token: 0x06005C2F RID: 23599 RVA: 0x0021E5B4 File Offset: 0x0021C7B4
		protected override ThoughtDef OrganizerThought
		{
			get
			{
				return ThoughtDefOf.HeldConcert;
			}
		}

		// Token: 0x1700100A RID: 4106
		// (get) Token: 0x06005C30 RID: 23600 RVA: 0x0021E5BB File Offset: 0x0021C7BB
		protected override TaleDef OrganizerTale
		{
			get
			{
				return TaleDefOf.HeldConcert;
			}
		}

		// Token: 0x06005C31 RID: 23601 RVA: 0x0021E5C2 File Offset: 0x0021C7C2
		public LordJob_Joinable_Concert()
		{
		}

		// Token: 0x06005C32 RID: 23602 RVA: 0x0021E5CA File Offset: 0x0021C7CA
		public LordJob_Joinable_Concert(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef) : base(spot, organizer, gatheringDef)
		{
		}

		// Token: 0x06005C33 RID: 23603 RVA: 0x0021E5D5 File Offset: 0x0021C7D5
		public override string GetReport(Pawn pawn)
		{
			if (pawn != this.organizer)
			{
				return "LordReportAttendingConcert".Translate();
			}
			return "LordReportHoldingConcert".Translate();
		}

		// Token: 0x06005C34 RID: 23604 RVA: 0x0021E5FF File Offset: 0x0021C7FF
		protected override LordToil CreateGatheringToil(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef)
		{
			return new LordToil_Concert(spot, organizer, gatheringDef, 3.5E-05f);
		}

		// Token: 0x06005C35 RID: 23605 RVA: 0x0021E60E File Offset: 0x0021C80E
		protected override Trigger_TicksPassed GetTimeoutTrigger()
		{
			return new Trigger_TicksPassedAfterConditionMet(base.DurationTicks, () => GatheringsUtility.InGatheringArea(this.organizer.Position, this.spot, this.organizer.Map), 60);
		}
	}
}
