using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BE2 RID: 7138
	public class Alert_CaravanIdle : Alert
	{
		// Token: 0x17002105 RID: 8453
		// (get) Token: 0x0600B6B3 RID: 46771 RVA: 0x00405CC8 File Offset: 0x00403EC8
		private List<Caravan> IdleCaravans
		{
			get
			{
				this.idleCaravansResult.Clear();
				foreach (Caravan caravan in Find.WorldObjects.Caravans)
				{
					if (caravan.Spawned && caravan.IsPlayerControlled && !caravan.pather.MovingNow && !caravan.CantMove)
					{
						this.idleCaravansResult.Add(caravan);
					}
				}
				return this.idleCaravansResult;
			}
		}

		// Token: 0x0600B6B4 RID: 46772 RVA: 0x00405D5C File Offset: 0x00403F5C
		public Alert_CaravanIdle()
		{
			this.defaultLabel = "CaravanIdle".Translate();
		}

		// Token: 0x0600B6B5 RID: 46773 RVA: 0x00405D90 File Offset: 0x00403F90
		public override TaggedString GetExplanation()
		{
			this.sb.Length = 0;
			foreach (Caravan caravan in this.idleCaravansResult)
			{
				this.sb.AppendLine("  - " + caravan.Label);
			}
			return "CaravanIdleDesc".Translate(this.sb.ToString().TrimEndNewlines());
		}

		// Token: 0x0600B6B6 RID: 46774 RVA: 0x00405E24 File Offset: 0x00404024
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.IdleCaravans);
		}

		// Token: 0x040068E6 RID: 26854
		private List<Caravan> idleCaravansResult = new List<Caravan>();

		// Token: 0x040068E7 RID: 26855
		private StringBuilder sb = new StringBuilder();
	}
}
