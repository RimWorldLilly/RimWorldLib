using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C00 RID: 7168
	public class Alert_AnimalRoaming : Alert
	{
		// Token: 0x0600B725 RID: 46885 RVA: 0x00408BAA File Offset: 0x00406DAA
		public Alert_AnimalRoaming()
		{
			this.defaultLabel = "AlertAnimalIsRoaming".Translate();
		}

		// Token: 0x0600B726 RID: 46886 RVA: 0x00408BE0 File Offset: 0x00406DE0
		private void CalculateTargets()
		{
			this.targets.Clear();
			this.pawnNames.Clear();
			foreach (Pawn pawn in PawnsFinder.AllMaps_SpawnedPawnsInFaction(Faction.OfPlayer))
			{
				if (pawn.MentalStateDef == MentalStateDefOf.Roaming)
				{
					this.targets.Add(pawn);
					this.pawnNames.Add(pawn.NameShortColored.Resolve());
				}
			}
		}

		// Token: 0x0600B727 RID: 46887 RVA: 0x00408C80 File Offset: 0x00406E80
		public override TaggedString GetExplanation()
		{
			return "AlertAnimalIsRoamingDesc".Translate(this.pawnNames.ToLineList("  - "));
		}

		// Token: 0x0600B728 RID: 46888 RVA: 0x00408CA1 File Offset: 0x00406EA1
		public override AlertReport GetReport()
		{
			this.CalculateTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x04006910 RID: 26896
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		// Token: 0x04006911 RID: 26897
		private List<string> pawnNames = new List<string>();
	}
}
