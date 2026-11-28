using System;
using System.Collections.Generic;
using System.Text;
using Verse;
using Verse.AI;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02001BA3 RID: 7075
	public class Alert_ColonistNeedsRescuing : Alert_Critical
	{
		// Token: 0x170020D7 RID: 8407
		// (get) Token: 0x0600B5A4 RID: 46500 RVA: 0x003FF984 File Offset: 0x003FDB84
		private List<Pawn> ColonistsNeedingRescue
		{
			get
			{
				this.colonistsNeedingRescueResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsSpawned)
				{
					if (Alert_ColonistNeedsRescuing.NeedsRescue(pawn))
					{
						this.colonistsNeedingRescueResult.Add(pawn);
					}
				}
				foreach (Pawn pawn2 in PawnsFinder.AllMaps_ColonySubhumansSpawnedPlayerControlled)
				{
					if (Alert_ColonistNeedsRescuing.NeedsRescue(pawn2))
					{
						this.colonistsNeedingRescueResult.Add(pawn2);
					}
				}
				return this.colonistsNeedingRescueResult;
			}
		}

		// Token: 0x0600B5A5 RID: 46501 RVA: 0x003FFA44 File Offset: 0x003FDC44
		public static bool NeedsRescue(Pawn p)
		{
			if (p.Downed && HealthAIUtility.WantsToBeRescued(p) && !p.InBed() && !(p.ParentHolder is Pawn_CarryTracker))
			{
				Pawn_JobTracker jobs = p.jobs;
				return (((jobs != null) ? jobs.jobQueue : null) == null || p.jobs.jobQueue.Count <= 0 || !p.jobs.jobQueue.Peek().job.CanBeginNow(p, false)) && !ChildcareUtility.BabyBeingPlayedWith(p);
			}
			return false;
		}

		// Token: 0x0600B5A6 RID: 46502 RVA: 0x003FFACA File Offset: 0x003FDCCA
		public override string GetLabel()
		{
			if (this.colonistsNeedingRescueResult.Count == 1)
			{
				return "ColonistNeedsRescue".Translate();
			}
			return "ColonistsNeedRescue".Translate();
		}

		// Token: 0x0600B5A7 RID: 46503 RVA: 0x003FFAFC File Offset: 0x003FDCFC
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.colonistsNeedingRescueResult)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			if (SteamDeck.IsSteamDeckInNonKeyboardMode)
			{
				return "ColonistsNeedRescueDescController".Translate(stringBuilder.ToString());
			}
			return "ColonistsNeedRescueDesc".Translate(stringBuilder.ToString());
		}

		// Token: 0x0600B5A8 RID: 46504 RVA: 0x003FFBA0 File Offset: 0x003FDDA0
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ColonistsNeedingRescue);
		}

		// Token: 0x04006883 RID: 26755
		private List<Pawn> colonistsNeedingRescueResult = new List<Pawn>();
	}
}
