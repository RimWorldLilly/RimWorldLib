using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020015AA RID: 5546
	public class Pawn_TimetableTracker : IExposable
	{
		// Token: 0x170017AB RID: 6059
		// (get) Token: 0x06008953 RID: 35155 RVA: 0x0031BAE7 File Offset: 0x00319CE7
		public TimeAssignmentDef CurrentAssignment
		{
			get
			{
				if (!this.pawn.IsColonist || this.pawn.IsPrisonerOfColony)
				{
					return TimeAssignmentDefOf.Anything;
				}
				return this.times[GenLocalDate.HourOfDay(this.pawn)];
			}
		}

		// Token: 0x06008954 RID: 35156 RVA: 0x0031BB20 File Offset: 0x00319D20
		public Pawn_TimetableTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.times = new List<TimeAssignmentDef>(24);
			for (int i = 0; i < 24; i++)
			{
				TimeAssignmentDef item;
				if (i <= 5 || i > 21)
				{
					item = TimeAssignmentDefOf.Sleep;
				}
				else
				{
					item = TimeAssignmentDefOf.Anything;
				}
				this.times.Add(item);
			}
		}

		// Token: 0x06008955 RID: 35157 RVA: 0x0031BB78 File Offset: 0x00319D78
		public void ExposeData()
		{
			Scribe_Collections.Look<TimeAssignmentDef>(ref this.times, "times", LookMode.Undefined, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && !ModsConfig.RoyaltyActive)
			{
				for (int i = 0; i < this.times.Count; i++)
				{
					if (this.times[i] == TimeAssignmentDefOf.Meditate)
					{
						this.times[i] = TimeAssignmentDefOf.Anything;
					}
				}
			}
		}

		// Token: 0x06008956 RID: 35158 RVA: 0x0031BBE4 File Offset: 0x00319DE4
		public TimeAssignmentDef GetAssignment(int hour)
		{
			return this.times[hour];
		}

		// Token: 0x06008957 RID: 35159 RVA: 0x0031BBF2 File Offset: 0x00319DF2
		public void SetAssignment(int hour, TimeAssignmentDef ta)
		{
			this.times[hour] = ta;
		}

		// Token: 0x04005495 RID: 21653
		private Pawn pawn;

		// Token: 0x04005496 RID: 21654
		public List<TimeAssignmentDef> times;
	}
}
