using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BD9 RID: 7129
	public class Alert_SlavesUnsuppressed : Alert
	{
		// Token: 0x170020FC RID: 8444
		// (get) Token: 0x0600B68C RID: 46732 RVA: 0x00404F54 File Offset: 0x00403154
		public List<Pawn> Targets
		{
			get
			{
				this.targetsResult.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					foreach (Pawn pawn in maps[i].mapPawns.FreeColonistsSpawned)
					{
						Need_Suppression need_Suppression;
						if (!pawn.Suspended && pawn.IsSlave && pawn.needs.TryGetNeed<Need_Suppression>(out need_Suppression) && need_Suppression.IsHigh)
						{
							this.targetsResult.Add(pawn);
						}
					}
				}
				return this.targetsResult;
			}
		}

		// Token: 0x0600B68D RID: 46733 RVA: 0x00405008 File Offset: 0x00403208
		public Alert_SlavesUnsuppressed()
		{
			this.defaultLabel = "SlavesUnsuppressedLabel".Translate();
			this.defaultExplanation = "SlavesUnsuppressedDesc".Translate();
			this.defaultPriority = AlertPriority.High;
			this.requireIdeology = true;
		}

		// Token: 0x0600B68E RID: 46734 RVA: 0x0040505E File Offset: 0x0040325E
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x0600B68F RID: 46735 RVA: 0x0040506B File Offset: 0x0040326B
		public override TaggedString GetExplanation()
		{
			return "SlavesUnsuppressedDesc".Translate(this.Targets[0]);
		}

		// Token: 0x040068D5 RID: 26837
		private List<Pawn> targetsResult = new List<Pawn>();
	}
}
