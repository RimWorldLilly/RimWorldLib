using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BFB RID: 7163
	public class Alert_UnusableMeditationFocus : Alert
	{
		// Token: 0x1700211A RID: 8474
		// (get) Token: 0x0600B711 RID: 46865 RVA: 0x00408170 File Offset: 0x00406370
		private List<GlobalTargetInfo> Targets
		{
			get
			{
				this.targets.Clear();
				this.pawnEntries.Clear();
				foreach (Pawn pawn in PawnsFinder.HomeMaps_FreeColonistsSpawned)
				{
					if (pawn.timetable != null && pawn.timetable.CurrentAssignment == TimeAssignmentDefOf.Meditate && pawn.psychicEntropy.IsCurrentlyMeditating && !MeditationFocusDefOf.Natural.CanPawnUse(pawn))
					{
						JobDriver_Meditate jobDriver_Meditate = pawn.jobs.curDriver as JobDriver_Meditate;
						if (jobDriver_Meditate != null && !(jobDriver_Meditate.Focus != null) && !(jobDriver_Meditate is JobDriver_Reign))
						{
							foreach (Thing thing in GenRadial.RadialDistinctThingsAround(pawn.Position, pawn.Map, MeditationUtility.FocusObjectSearchRadius, false))
							{
								if (thing.def == ThingDefOf.Plant_TreeAnima || thing.def == ThingDefOf.AnimusStone || thing.def == ThingDefOf.NatureShrine_Small || thing.def == ThingDefOf.NatureShrine_Large)
								{
									this.targets.Add(pawn);
									this.pawnEntries.Add(pawn.NameShortColored.Resolve() + " (" + thing.LabelShort + ")");
									break;
								}
							}
						}
					}
				}
				return this.targets;
			}
		}

		// Token: 0x0600B712 RID: 46866 RVA: 0x00408338 File Offset: 0x00406538
		public Alert_UnusableMeditationFocus()
		{
			this.defaultLabel = "UnusableMeditationFocusAlert".Translate();
			this.requireRoyalty = true;
		}

		// Token: 0x0600B713 RID: 46867 RVA: 0x00408372 File Offset: 0x00406572
		public override TaggedString GetExplanation()
		{
			return "UnusableMeditationFocusAlertDesc".Translate(this.pawnEntries.ToLineList("  - "));
		}

		// Token: 0x0600B714 RID: 46868 RVA: 0x00408393 File Offset: 0x00406593
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x04006905 RID: 26885
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		// Token: 0x04006906 RID: 26886
		private List<string> pawnEntries = new List<string>();

		// Token: 0x020038BC RID: 14524
		public class Alert_PermitAvailable : Alert
		{
			// Token: 0x06014B1C RID: 84764 RVA: 0x006A09A6 File Offset: 0x0069EBA6
			public Alert_PermitAvailable()
			{
				this.defaultLabel = "PermitChoiceReadyAlert".Translate();
				this.requireRoyalty = true;
			}

			// Token: 0x06014B1D RID: 84765 RVA: 0x006A09D8 File Offset: 0x0069EBD8
			private void CalculateTargets()
			{
				this.targets.Clear();
				this.target = null;
				foreach (Pawn pawn in PawnsFinder.HomeMaps_FreeColonistsSpawned)
				{
					if (pawn.royalty != null && pawn.royalty.PermitPointsAvailable)
					{
						if (this.target == null)
						{
							this.target = pawn;
						}
						this.targets.Add(pawn);
					}
				}
			}

			// Token: 0x06014B1E RID: 84766 RVA: 0x006A0A6C File Offset: 0x0069EC6C
			public override AlertReport GetReport()
			{
				this.CalculateTargets();
				return AlertReport.CulpritsAre(this.targets);
			}

			// Token: 0x06014B1F RID: 84767 RVA: 0x006A0A7F File Offset: 0x0069EC7F
			public override TaggedString GetExplanation()
			{
				return "PermitChoiceReadyAlertDesc".Translate(this.target.Named("PAWN"));
			}

			// Token: 0x06014B20 RID: 84768 RVA: 0x006A0A9B File Offset: 0x0069EC9B
			protected override void OnClick()
			{
				base.OnClick();
				Pawn pawn = this.target;
				if (pawn == null)
				{
					return;
				}
				Pawn_RoyaltyTracker royalty = pawn.royalty;
				if (royalty == null)
				{
					return;
				}
				royalty.OpenPermitWindow();
			}

			// Token: 0x0400E2A8 RID: 58024
			private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

			// Token: 0x0400E2A9 RID: 58025
			private Pawn target;
		}
	}
}
