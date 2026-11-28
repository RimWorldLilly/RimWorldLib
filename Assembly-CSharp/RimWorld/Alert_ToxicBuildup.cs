using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BDC RID: 7132
	public class Alert_ToxicBuildup : Alert
	{
		// Token: 0x0600B698 RID: 46744 RVA: 0x00405293 File Offset: 0x00403493
		public Alert_ToxicBuildup()
		{
			this.defaultLabel = "AlertToxicBuildup".Translate();
			this.defaultPriority = AlertPriority.High;
			this.requireBiotech = true;
		}

		// Token: 0x170020FF RID: 8447
		// (get) Token: 0x0600B699 RID: 46745 RVA: 0x004052CC File Offset: 0x004034CC
		private List<Pawn> ToxicBuildupColonists
		{
			get
			{
				this.toxicBuildupColonists.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists_NoSuspended)
				{
					Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.ToxicBuildup, false);
					if (firstHediffOfDef != null && firstHediffOfDef.Severity >= 0.6f)
					{
						this.toxicBuildupColonists.Add(pawn);
					}
				}
				return this.toxicBuildupColonists;
			}
		}

		// Token: 0x0600B69A RID: 46746 RVA: 0x0040535C File Offset: 0x0040355C
		public override TaggedString GetExplanation()
		{
			string value = (from p in this.toxicBuildupColonists
			select p.NameShortColored.Resolve()).ToLineList(" - ", false);
			return "AlertToxicBuildupDesc".Translate(value);
		}

		// Token: 0x0600B69B RID: 46747 RVA: 0x004053AF File Offset: 0x004035AF
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ToxicBuildupColonists);
		}

		// Token: 0x040068D9 RID: 26841
		private const float ToxicBuildupSeverityThreshold = 0.6f;

		// Token: 0x040068DA RID: 26842
		private List<Pawn> toxicBuildupColonists = new List<Pawn>();
	}
}
