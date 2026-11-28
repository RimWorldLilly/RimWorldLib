using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D59 RID: 7513
	public class ChoiceLetter_RansomDemand : ChoiceLetter
	{
		// Token: 0x170023EF RID: 9199
		// (get) Token: 0x0600C27F RID: 49791 RVA: 0x0046E950 File Offset: 0x0046CB50
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				ChoiceLetter_RansomDemand.<get_Choices>d__5 <get_Choices>d__ = new ChoiceLetter_RansomDemand.<get_Choices>d__5(-2);
				<get_Choices>d__.<>4__this = this;
				return <get_Choices>d__;
			}
		}

		// Token: 0x170023F0 RID: 9200
		// (get) Token: 0x0600C280 RID: 49792 RVA: 0x0046E960 File Offset: 0x0046CB60
		public override bool CanShowInLetterStack
		{
			get
			{
				return base.CanShowInLetterStack && Find.Maps.Contains(this.map) && this.faction.kidnapped.KidnappedPawnsListForReading.Contains(this.kidnapped);
			}
		}

		// Token: 0x0600C281 RID: 49793 RVA: 0x0046E99C File Offset: 0x0046CB9C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Map>(ref this.map, "map", false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_References.Look<Pawn>(ref this.kidnapped, "kidnapped", false);
			Scribe_Values.Look<int>(ref this.fee, "fee", 0, false);
		}

		// Token: 0x04006F3B RID: 28475
		public Map map;

		// Token: 0x04006F3C RID: 28476
		public Faction faction;

		// Token: 0x04006F3D RID: 28477
		public Pawn kidnapped;

		// Token: 0x04006F3E RID: 28478
		public int fee;
	}
}
