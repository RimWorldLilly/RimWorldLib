using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C21 RID: 7201
	public abstract class Alert_Thought : Alert
	{
		// Token: 0x17002123 RID: 8483
		// (get) Token: 0x0600B7AC RID: 47020
		protected abstract ThoughtDef Thought { get; }

		// Token: 0x0600B7AD RID: 47021 RVA: 0x0040BE10 File Offset: 0x0040A010
		public override string GetLabel()
		{
			int count = this.AffectedPawns.Count;
			string label = base.GetLabel();
			if (count > 1)
			{
				return label + " x" + count.ToString();
			}
			return label;
		}

		// Token: 0x17002124 RID: 8484
		// (get) Token: 0x0600B7AE RID: 47022 RVA: 0x0040BE48 File Offset: 0x0040A048
		private List<Pawn> AffectedPawns
		{
			get
			{
				this.affectedPawnsResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists_NoSuspended)
				{
					if (pawn.Dead)
					{
						string str = "Dead pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists:";
						Pawn pawn2 = pawn;
						Log.Error(str + ((pawn2 != null) ? pawn2.ToString() : null));
					}
					else if (pawn.needs.mood != null)
					{
						pawn.needs.mood.thoughts.GetMoodThoughtsFor(this.Thought, Alert_Thought.tmpThoughts);
						for (int i = 0; i < Alert_Thought.tmpThoughts.Count; i++)
						{
							if (!ThoughtUtility.ThoughtNullified(pawn, Alert_Thought.tmpThoughts[i].def))
							{
								this.affectedPawnsResult.Add(pawn);
								break;
							}
						}
						Alert_Thought.tmpThoughts.Clear();
					}
				}
				return this.affectedPawnsResult;
			}
		}

		// Token: 0x0600B7AF RID: 47023 RVA: 0x0040BF44 File Offset: 0x0040A144
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.AffectedPawns);
		}

		// Token: 0x0600B7B0 RID: 47024 RVA: 0x0040BF54 File Offset: 0x0040A154
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.AffectedPawns)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return this.explanationKey.Translate(stringBuilder.ToString());
		}

		// Token: 0x04006948 RID: 26952
		protected string explanationKey;

		// Token: 0x04006949 RID: 26953
		private static readonly List<Thought> tmpThoughts = new List<Thought>();

		// Token: 0x0400694A RID: 26954
		private readonly List<Pawn> affectedPawnsResult = new List<Pawn>();
	}
}
