using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BDD RID: 7133
	public class Alert_PollutedTerrain : Alert
	{
		// Token: 0x17002100 RID: 8448
		// (get) Token: 0x0600B69C RID: 46748 RVA: 0x004053BC File Offset: 0x004035BC
		public List<Pawn> Targets
		{
			get
			{
				this.targets.Clear();
				foreach (Map map in Find.Maps)
				{
					foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
					{
						Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.ToxicBuildup, false);
						if (firstHediffOfDef != null && firstHediffOfDef.Severity >= 0.3f && pawn.Position.IsPolluted(map))
						{
							this.targets.Add(pawn);
						}
					}
				}
				return this.targets;
			}
		}

		// Token: 0x0600B69D RID: 46749 RVA: 0x004054A4 File Offset: 0x004036A4
		public Alert_PollutedTerrain()
		{
			this.defaultLabel = "AlertPollutedTerrain".Translate();
			this.defaultPriority = AlertPriority.High;
			this.requireBiotech = true;
		}

		// Token: 0x0600B69E RID: 46750 RVA: 0x004054DA File Offset: 0x004036DA
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x0600B69F RID: 46751 RVA: 0x004054E8 File Offset: 0x004036E8
		public override TaggedString GetExplanation()
		{
			string value = (from p in this.targets
			select p.NameShortColored.Resolve()).ToLineList("  - ", false);
			return "AlertPollutedTerrainDesc".Translate(value);
		}

		// Token: 0x040068DB RID: 26843
		private const float ToxicBuildupSeverityThreshold = 0.3f;

		// Token: 0x040068DC RID: 26844
		private List<Pawn> targets = new List<Pawn>();
	}
}
