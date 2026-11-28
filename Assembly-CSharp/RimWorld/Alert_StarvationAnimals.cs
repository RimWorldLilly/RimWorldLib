using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BF1 RID: 7153
	public class Alert_StarvationAnimals : Alert
	{
		// Token: 0x0600B6EF RID: 46831 RVA: 0x0040765A File Offset: 0x0040585A
		public Alert_StarvationAnimals()
		{
			this.defaultLabel = "StarvationAnimals".Translate();
		}

		// Token: 0x1700210F RID: 8463
		// (get) Token: 0x0600B6F0 RID: 46832 RVA: 0x00407690 File Offset: 0x00405890
		private List<Pawn> StarvingAnimals
		{
			get
			{
				this.starvingAnimalsResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction_NoCryptosleep)
				{
					if (pawn.HostFaction == null && !pawn.RaceProps.Humanlike && pawn.needs.food != null && (pawn.needs.food.TicksStarving > 30000 || (pawn.health.hediffSet.HasHediff(HediffDefOf.Pregnant, true) && pawn.needs.food.TicksStarving > 5000)))
					{
						this.starvingAnimalsResult.Add(pawn);
					}
				}
				return this.starvingAnimalsResult;
			}
		}

		// Token: 0x0600B6F1 RID: 46833 RVA: 0x00407764 File Offset: 0x00405964
		public override TaggedString GetExplanation()
		{
			this.sb.Length = 0;
			foreach (Pawn pawn in from a in this.starvingAnimalsResult
			orderby a.def.label
			select a)
			{
				this.sb.Append("  - " + pawn.NameShortColored.Resolve());
				if (pawn.Name.IsValid && !pawn.Name.Numerical)
				{
					this.sb.Append(" (" + pawn.def.label + ")");
				}
				this.sb.AppendLine();
			}
			return "StarvationAnimalsDesc".Translate(this.sb.ToString().TrimEndNewlines());
		}

		// Token: 0x0600B6F2 RID: 46834 RVA: 0x0040786C File Offset: 0x00405A6C
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.StarvingAnimals);
		}

		// Token: 0x040068F9 RID: 26873
		private List<Pawn> starvingAnimalsResult = new List<Pawn>();

		// Token: 0x040068FA RID: 26874
		private StringBuilder sb = new StringBuilder();
	}
}
