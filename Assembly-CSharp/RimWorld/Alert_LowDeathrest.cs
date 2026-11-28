using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C0F RID: 7183
	public class Alert_LowDeathrest : Alert
	{
		// Token: 0x0600B76A RID: 46954 RVA: 0x0040A752 File Offset: 0x00408952
		public Alert_LowDeathrest()
		{
			this.requireBiotech = true;
		}

		// Token: 0x0600B76B RID: 46955 RVA: 0x0040A778 File Offset: 0x00408978
		public override string GetLabel()
		{
			if (this.targets.Count == 1)
			{
				return "AlertLowDeathrestPawn".Translate(this.targetLabels[0].Named("PAWN"));
			}
			return "AlertLowDeathrestPawns".Translate(this.targetLabels.Count.ToStringCached().Named("NUMCULPRITS"));
		}

		// Token: 0x0600B76C RID: 46956 RVA: 0x0040A7E4 File Offset: 0x004089E4
		private void CalculateTargets()
		{
			this.targets.Clear();
			this.targetLabels.Clear();
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_AliveSpawned)
			{
				Need_Deathrest need_Deathrest;
				if (pawn.RaceProps.Humanlike && pawn.Faction == Faction.OfPlayer && pawn.needs != null && pawn.needs.TryGetNeed<Need_Deathrest>(out need_Deathrest) && need_Deathrest.CurLevel <= 0.1f && !pawn.Deathresting)
				{
					this.targets.Add(pawn);
					this.targetLabels.Add(pawn.NameShortColored.Resolve());
				}
			}
		}

		// Token: 0x0600B76D RID: 46957 RVA: 0x0040A8B0 File Offset: 0x00408AB0
		public override TaggedString GetExplanation()
		{
			return "AlertLowDeathrestDesc".Translate(this.targetLabels.ToLineList("  - ").Named("CULPRITS"));
		}

		// Token: 0x0600B76E RID: 46958 RVA: 0x0040A8D6 File Offset: 0x00408AD6
		public override AlertReport GetReport()
		{
			this.CalculateTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x04006933 RID: 26931
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		// Token: 0x04006934 RID: 26932
		private List<string> targetLabels = new List<string>();
	}
}
