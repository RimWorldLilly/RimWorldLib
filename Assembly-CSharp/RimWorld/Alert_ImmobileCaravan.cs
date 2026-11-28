using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BA6 RID: 7078
	public class Alert_ImmobileCaravan : Alert_Critical
	{
		// Token: 0x170020DA RID: 8410
		// (get) Token: 0x0600B5B2 RID: 46514 RVA: 0x003FFEF4 File Offset: 0x003FE0F4
		private List<Caravan> ImmobileCaravans
		{
			get
			{
				this.immobileCaravansResult.Clear();
				foreach (Caravan caravan in Find.WorldObjects.Caravans)
				{
					if (caravan.Shuttle == null && caravan.IsPlayerControlled && caravan.ImmobilizedByMass)
					{
						this.immobileCaravansResult.Add(caravan);
					}
				}
				return this.immobileCaravansResult;
			}
		}

		// Token: 0x0600B5B3 RID: 46515 RVA: 0x003FFF7C File Offset: 0x003FE17C
		public Alert_ImmobileCaravan()
		{
			this.defaultLabel = "ImmobileCaravan".Translate();
			this.defaultExplanation = "ImmobileCaravanDesc".Translate();
		}

		// Token: 0x0600B5B4 RID: 46516 RVA: 0x003FFFB9 File Offset: 0x003FE1B9
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ImmobileCaravans);
		}

		// Token: 0x04006885 RID: 26757
		private List<Caravan> immobileCaravansResult = new List<Caravan>();
	}
}
