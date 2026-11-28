using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CB1 RID: 3249
	public abstract class LordJob_Joinable_Gathering : LordJob_VoluntarilyJoinable
	{
		// Token: 0x1700100B RID: 4107
		// (get) Token: 0x06005C37 RID: 23607 RVA: 0x0021E64C File Offset: 0x0021C84C
		public Pawn Organizer
		{
			get
			{
				return this.organizer;
			}
		}

		// Token: 0x1700100C RID: 4108
		// (get) Token: 0x06005C38 RID: 23608 RVA: 0x0021E654 File Offset: 0x0021C854
		public int DurationTicks
		{
			get
			{
				return this.durationTicks;
			}
		}

		// Token: 0x1700100D RID: 4109
		// (get) Token: 0x06005C39 RID: 23609 RVA: 0x0021E65C File Offset: 0x0021C85C
		public virtual int TicksLeft
		{
			get
			{
				return this.timeoutTrigger.TicksLeft;
			}
		}

		// Token: 0x1700100E RID: 4110
		// (get) Token: 0x06005C3A RID: 23610 RVA: 0x0021E669 File Offset: 0x0021C869
		public virtual IntVec3 Spot
		{
			get
			{
				return this.spot;
			}
		}

		// Token: 0x06005C3B RID: 23611 RVA: 0x0021E671 File Offset: 0x0021C871
		public LordJob_Joinable_Gathering()
		{
		}

		// Token: 0x06005C3C RID: 23612 RVA: 0x0021E679 File Offset: 0x0021C879
		public LordJob_Joinable_Gathering(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef)
		{
			this.spot = spot;
			this.organizer = organizer;
			this.gatheringDef = gatheringDef;
		}

		// Token: 0x06005C3D RID: 23613
		protected abstract LordToil CreateGatheringToil(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef);

		// Token: 0x06005C3E RID: 23614 RVA: 0x0021E696 File Offset: 0x0021C896
		protected virtual bool ShouldBeCalledOff()
		{
			return (this.organizer != null && !GatheringsUtility.PawnCanStartOrContinueGathering(this.organizer)) || !GatheringsUtility.AcceptableGameConditionsToContinueGathering(base.Map);
		}

		// Token: 0x06005C3F RID: 23615 RVA: 0x0021E6C0 File Offset: 0x0021C8C0
		public override float VoluntaryJoinPriorityFor(Pawn p)
		{
			if (!this.IsInvited(p))
			{
				return 0f;
			}
			if (!GatheringsUtility.ShouldPawnKeepGathering(p, this.gatheringDef))
			{
				return 0f;
			}
			if (this.spot.IsForbidden(p))
			{
				return 0f;
			}
			if (!this.lord.ownedPawns.Contains(p) && this.IsGatheringAboutToEnd())
			{
				return 0f;
			}
			List<Hediff> hediffs = p.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].def.blocksSocialInteraction)
				{
					return 0f;
				}
			}
			return VoluntarilyJoinableLordJobJoinPriorities.SocialGathering;
		}

		// Token: 0x06005C40 RID: 23616 RVA: 0x0021E768 File Offset: 0x0021C968
		protected virtual Trigger_TicksPassed GetTimeoutTrigger()
		{
			return new Trigger_TicksPassed(this.durationTicks);
		}

		// Token: 0x06005C41 RID: 23617 RVA: 0x0021E778 File Offset: 0x0021C978
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.spot, "spot", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.durationTicks, "durationTicks", 0, false);
			Scribe_References.Look<Pawn>(ref this.organizer, "organizer", false);
			Scribe_Defs.Look<GatheringDef>(ref this.gatheringDef, "gatheringDef");
		}

		// Token: 0x06005C42 RID: 23618 RVA: 0x0021E7D2 File Offset: 0x0021C9D2
		protected bool IsGatheringAboutToEnd()
		{
			return this.TicksLeft < 1200;
		}

		// Token: 0x06005C43 RID: 23619 RVA: 0x0021E7E4 File Offset: 0x0021C9E4
		protected virtual bool IsInvited(Pawn p)
		{
			return this.lord.faction != null && p.Faction == this.lord.faction;
		}

		// Token: 0x040034B9 RID: 13497
		protected IntVec3 spot;

		// Token: 0x040034BA RID: 13498
		protected Pawn organizer;

		// Token: 0x040034BB RID: 13499
		protected GatheringDef gatheringDef;

		// Token: 0x040034BC RID: 13500
		protected int durationTicks;

		// Token: 0x040034BD RID: 13501
		protected Trigger_TicksPassed timeoutTrigger;
	}
}
