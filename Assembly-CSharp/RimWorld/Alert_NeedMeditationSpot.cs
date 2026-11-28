using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BFA RID: 7162
	public class Alert_NeedMeditationSpot : Alert
	{
		// Token: 0x17002119 RID: 8473
		// (get) Token: 0x0600B70B RID: 46859 RVA: 0x00407F8C File Offset: 0x0040618C
		private List<GlobalTargetInfo> Targets
		{
			get
			{
				this.targets.Clear();
				this.pawnNames.Clear();
				foreach (Pawn pawn in PawnsFinder.HomeMaps_FreeColonistsSpawned)
				{
					bool flag = false;
					for (int i = 0; i < pawn.timetable.times.Count; i++)
					{
						if (pawn.timetable.times[i] == TimeAssignmentDefOf.Meditate)
						{
							flag = true;
							break;
						}
					}
					if ((pawn.HasPsylink || flag) && pawn.psychicEntropy.IsPsychicallySensitive)
					{
						Building spot;
						if (Alert_NeedMeditationSpot.CachedSpots.TryGetValue(pawn, out spot))
						{
							if (MeditationUtility.IsValidMeditationBuildingForPawn(spot, pawn))
							{
								continue;
							}
							Alert_NeedMeditationSpot.CachedSpots.Remove(pawn);
						}
						LocalTargetInfo localTargetInfo = MeditationUtility.AllMeditationSpotCandidates(pawn, false).FirstOrFallback(LocalTargetInfo.Invalid);
						if (!localTargetInfo.IsValid)
						{
							this.targets.Add(pawn);
							this.pawnNames.Add(pawn.NameShortColored.Resolve());
						}
						else
						{
							Building building = localTargetInfo.Thing as Building;
							if (building != null)
							{
								Alert_NeedMeditationSpot.CachedSpots[pawn] = building;
							}
						}
					}
				}
				return this.targets;
			}
		}

		// Token: 0x0600B70C RID: 46860 RVA: 0x004080F0 File Offset: 0x004062F0
		public Alert_NeedMeditationSpot()
		{
			this.defaultLabel = "NeedMeditationSpotAlert".Translate();
			this.requireRoyalty = true;
		}

		// Token: 0x0600B70D RID: 46861 RVA: 0x0040812A File Offset: 0x0040632A
		public override TaggedString GetExplanation()
		{
			return "NeedMeditationSpotAlertDesc".Translate(this.pawnNames.ToLineList("  - "));
		}

		// Token: 0x0600B70E RID: 46862 RVA: 0x0040814B File Offset: 0x0040634B
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x0600B70F RID: 46863 RVA: 0x00408158 File Offset: 0x00406358
		public static void ClearCache()
		{
			Alert_NeedMeditationSpot.CachedSpots.Clear();
		}

		// Token: 0x04006902 RID: 26882
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		// Token: 0x04006903 RID: 26883
		private List<string> pawnNames = new List<string>();

		// Token: 0x04006904 RID: 26884
		private static Dictionary<Pawn, Building> CachedSpots = new Dictionary<Pawn, Building>();
	}
}
