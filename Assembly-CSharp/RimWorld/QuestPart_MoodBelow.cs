using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001087 RID: 4231
	public class QuestPart_MoodBelow : QuestPartActivable
	{
		// Token: 0x17001225 RID: 4645
		// (get) Token: 0x060069FF RID: 27135 RVA: 0x002553C0 File Offset: 0x002535C0
		public override AlertReport AlertReport
		{
			get
			{
				if (!this.showAlert || this.minTicksBelowThreshold < 60)
				{
					return AlertReport.Inactive;
				}
				this.culpritsResult.Clear();
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.MoodBelowThreshold(this.pawns[i]))
					{
						this.culpritsResult.Add(this.pawns[i]);
					}
				}
				return AlertReport.CulpritsAre(this.culpritsResult);
			}
		}

		// Token: 0x17001226 RID: 4646
		// (get) Token: 0x06006A00 RID: 27136 RVA: 0x0025543C File Offset: 0x0025363C
		public override bool AlertCritical
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001227 RID: 4647
		// (get) Token: 0x06006A01 RID: 27137 RVA: 0x0025543F File Offset: 0x0025363F
		public override string AlertLabel
		{
			get
			{
				return "QuestPartMoodBelowThreshold".Translate();
			}
		}

		// Token: 0x17001228 RID: 4648
		// (get) Token: 0x06006A02 RID: 27138 RVA: 0x00255450 File Offset: 0x00253650
		public override string AlertExplanation
		{
			get
			{
				return "QuestPartMoodBelowThresholdDesc".Translate(this.quest.name, GenLabel.ThingsLabel(this.pawns.Where(new Func<Pawn, bool>(this.MoodBelowThreshold)).Cast<Thing>(), "  - "));
			}
		}

		// Token: 0x06006A03 RID: 27139 RVA: 0x002554A8 File Offset: 0x002536A8
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			while (this.moodBelowThresholdTicks.Count < this.pawns.Count)
			{
				this.moodBelowThresholdTicks.Add(0);
			}
			for (int i = 0; i < this.pawns.Count; i++)
			{
				if (this.MoodBelowThreshold(this.pawns[i]))
				{
					List<int> list = this.moodBelowThresholdTicks;
					int index = i;
					int num = list[index];
					list[index] = num + 1;
					if (this.moodBelowThresholdTicks[i] >= this.minTicksBelowThreshold)
					{
						base.Complete(this.pawns[i].Named("SUBJECT"));
						return;
					}
				}
				else
				{
					this.moodBelowThresholdTicks[i] = 0;
				}
			}
		}

		// Token: 0x06006A04 RID: 27140 RVA: 0x00255564 File Offset: 0x00253764
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<float>(ref this.threshold, "threshold", 0f, false);
			Scribe_Values.Look<int>(ref this.minTicksBelowThreshold, "minTicksBelowThreshold", 0, false);
			Scribe_Values.Look<bool>(ref this.showAlert, "showAlert", true, false);
			Scribe_Collections.Look<int>(ref this.moodBelowThresholdTicks, "moodBelowThresholdTicks", LookMode.Value, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06006A05 RID: 27141 RVA: 0x00255610 File Offset: 0x00253810
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			if (Find.AnyPlayerHomeMap != null)
			{
				Map randomPlayerHomeMap = Find.RandomPlayerHomeMap;
				this.pawns.Add(randomPlayerHomeMap.mapPawns.FreeColonists.FirstOrDefault<Pawn>());
				this.threshold = 0.5f;
				this.minTicksBelowThreshold = 2500;
			}
		}

		// Token: 0x06006A06 RID: 27142 RVA: 0x00255661 File Offset: 0x00253861
		private bool MoodBelowThreshold(Pawn pawn)
		{
			return pawn.needs != null && pawn.needs.mood != null && pawn.needs.mood.CurLevelPercentage < this.threshold;
		}

		// Token: 0x06006A07 RID: 27143 RVA: 0x00255692 File Offset: 0x00253892
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x04004361 RID: 17249
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04004362 RID: 17250
		public float threshold;

		// Token: 0x04004363 RID: 17251
		public int minTicksBelowThreshold;

		// Token: 0x04004364 RID: 17252
		public bool showAlert = true;

		// Token: 0x04004365 RID: 17253
		private List<int> moodBelowThresholdTicks = new List<int>();

		// Token: 0x04004366 RID: 17254
		private List<Pawn> culpritsResult = new List<Pawn>();
	}
}
