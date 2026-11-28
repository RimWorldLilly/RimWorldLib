using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001716 RID: 5910
	public class RitualRoleBehavior : IExposable
	{
		// Token: 0x17001955 RID: 6485
		// (get) Token: 0x0600939F RID: 37791 RVA: 0x0035E0C6 File Offset: 0x0035C2C6
		public List<RitualPosition> CustomPositionsForReading
		{
			get
			{
				return this.customPositions;
			}
		}

		// Token: 0x060093A0 RID: 37792 RVA: 0x0035E0D0 File Offset: 0x0035C2D0
		public RitualPosition GetPosition(IntVec3 spot, Pawn p, LordJob_Ritual ritual)
		{
			if (!this.customPositions.NullOrEmpty<RitualPosition>())
			{
				foreach (RitualPosition ritualPosition in this.customPositions)
				{
					if (ritualPosition.CanUse(spot, p, ritual))
					{
						return ritualPosition;
					}
				}
			}
			return null;
		}

		// Token: 0x060093A1 RID: 37793 RVA: 0x0035E13C File Offset: 0x0035C33C
		public virtual string GetJobReportOverride(Pawn pawn)
		{
			string text = this.jobReportOverride;
			TaggedString? taggedString = (text != null) ? new TaggedString?(text.Formatted(pawn.Named("PAWN")).CapitalizeFirst()) : null;
			if (taggedString == null)
			{
				return null;
			}
			return taggedString.GetValueOrDefault();
		}

		// Token: 0x060093A2 RID: 37794 RVA: 0x0035E194 File Offset: 0x0035C394
		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.roleId, "roleId", null, false);
			Scribe_Defs.Look<DutyDef>(ref this.dutyDef, "dutyDef");
			Scribe_Collections.Look<RitualPosition>(ref this.customPositions, "customPositions", LookMode.Deep, Array.Empty<object>());
			Scribe_Defs.Look<InteractionDef>(ref this.speakerInteraction, "speakerInteraction");
		}

		// Token: 0x04005940 RID: 22848
		[NoTranslate]
		public string roleId;

		// Token: 0x04005941 RID: 22849
		public DutyDef dutyDef;

		// Token: 0x04005942 RID: 22850
		[MustTranslate]
		public string jobReportOverride;

		// Token: 0x04005943 RID: 22851
		protected List<RitualPosition> customPositions;

		// Token: 0x04005944 RID: 22852
		public InteractionDef speakerInteraction;
	}
}
