using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C18 RID: 7192
	public class Alert_DeathrestComplete : Alert
	{
		// Token: 0x0600B791 RID: 46993 RVA: 0x0040B494 File Offset: 0x00409694
		public Alert_DeathrestComplete()
		{
			this.defaultLabel = "AlertDeathrestComplete".Translate();
			this.defaultExplanation = "AlertDeathrestCompleteDesc".Translate();
			this.requireBiotech = true;
		}

		// Token: 0x0600B792 RID: 46994 RVA: 0x0040B4E3 File Offset: 0x004096E3
		public override AlertReport GetReport()
		{
			this.GetTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x0600B793 RID: 46995 RVA: 0x0040B4F8 File Offset: 0x004096F8
		public override TaggedString GetExplanation()
		{
			return this.defaultExplanation + ":\n" + (from p in this.targets
			select p.NameShortColored.Resolve()).ToLineList("  - ", false);
		}

		// Token: 0x0600B794 RID: 46996 RVA: 0x0040B550 File Offset: 0x00409750
		private void GetTargets()
		{
			this.targets.Clear();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				List<Pawn> list = maps[i].mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j].genes != null && list[j].Deathresting)
					{
						Gene_Deathrest firstGeneOfType = list[j].genes.GetFirstGeneOfType<Gene_Deathrest>();
						if (firstGeneOfType != null && firstGeneOfType.ShowWakeAlert)
						{
							this.targets.Add(list[j]);
						}
					}
				}
			}
		}

		// Token: 0x04006941 RID: 26945
		private List<Pawn> targets = new List<Pawn>();
	}
}
