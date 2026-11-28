using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C0E RID: 7182
	public class Alert_LowHemogen : Alert
	{
		// Token: 0x0600B765 RID: 46949 RVA: 0x0040A5DA File Offset: 0x004087DA
		public Alert_LowHemogen()
		{
			this.requireBiotech = true;
			this.defaultLabel = "AlertLowHemogen".Translate();
		}

		// Token: 0x0600B766 RID: 46950 RVA: 0x0040A614 File Offset: 0x00408814
		public override string GetLabel()
		{
			string text = this.defaultLabel;
			if (this.targets.Count == 1)
			{
				text = text + ": " + this.targetLabels[0];
			}
			return text;
		}

		// Token: 0x0600B767 RID: 46951 RVA: 0x0040A650 File Offset: 0x00408850
		private void CalculateTargets()
		{
			this.targets.Clear();
			this.targetLabels.Clear();
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_AliveSpawned)
			{
				if (pawn.genes != null && pawn.RaceProps.Humanlike && pawn.Faction == Faction.OfPlayer)
				{
					Gene_Hemogen firstGeneOfType = pawn.genes.GetFirstGeneOfType<Gene_Hemogen>();
					if (firstGeneOfType != null && firstGeneOfType.Value < firstGeneOfType.MinLevelForAlert)
					{
						this.targets.Add(pawn);
						this.targetLabels.Add(pawn.NameShortColored.Resolve());
					}
				}
			}
		}

		// Token: 0x0600B768 RID: 46952 RVA: 0x0040A714 File Offset: 0x00408914
		public override TaggedString GetExplanation()
		{
			return "AlertLowHemogenDesc".Translate() + ":\n" + this.targetLabels.ToLineList("  - ");
		}

		// Token: 0x0600B769 RID: 46953 RVA: 0x0040A73F File Offset: 0x0040893F
		public override AlertReport GetReport()
		{
			this.CalculateTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x04006931 RID: 26929
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		// Token: 0x04006932 RID: 26930
		private List<string> targetLabels = new List<string>();
	}
}
