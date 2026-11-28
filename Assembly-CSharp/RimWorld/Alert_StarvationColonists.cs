using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BD4 RID: 7124
	public class Alert_StarvationColonists : Alert
	{
		// Token: 0x0600B678 RID: 46712 RVA: 0x00404809 File Offset: 0x00402A09
		public Alert_StarvationColonists()
		{
			this.defaultLabel = "Starvation".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170020F7 RID: 8439
		// (get) Token: 0x0600B679 RID: 46713 RVA: 0x00404844 File Offset: 0x00402A44
		private List<Pawn> StarvingColonists
		{
			get
			{
				this.starvingColonistsResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists_NoSuspended)
				{
					if (pawn.needs.food != null && pawn.needs.food.Starving)
					{
						this.starvingColonistsResult.Add(pawn);
					}
				}
				return this.starvingColonistsResult;
			}
		}

		// Token: 0x0600B67A RID: 46714 RVA: 0x004048CC File Offset: 0x00402ACC
		public override TaggedString GetExplanation()
		{
			this.sb.Length = 0;
			foreach (Pawn pawn in this.starvingColonistsResult)
			{
				this.sb.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "StarvationDesc".Translate(this.sb.ToString().TrimEndNewlines());
		}

		// Token: 0x0600B67B RID: 46715 RVA: 0x00404968 File Offset: 0x00402B68
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.StarvingColonists);
		}

		// Token: 0x040068CD RID: 26829
		private List<Pawn> starvingColonistsResult = new List<Pawn>();

		// Token: 0x040068CE RID: 26830
		private StringBuilder sb = new StringBuilder();
	}
}
