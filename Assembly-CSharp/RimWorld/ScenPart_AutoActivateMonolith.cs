using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017AE RID: 6062
	public class ScenPart_AutoActivateMonolith : ScenPart
	{
		// Token: 0x0600970F RID: 38671 RVA: 0x0036CBD3 File Offset: 0x0036ADD3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.delayTicks, "delayTicks", 0, false);
		}

		// Token: 0x06009710 RID: 38672 RVA: 0x0036CBF0 File Offset: 0x0036ADF0
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			float num = (float)this.delayTicks / 60000f;
			Widgets.TextFieldNumericLabeled<float>(listing.GetScenPartRect(this, ScenPart.RowHeight), "delayDays".Translate().CapitalizeFirst(), ref num, ref this.delayTicksBuf, 0f, 1E+09f);
			this.delayTicks = (int)(num * 60000f);
		}

		// Token: 0x06009711 RID: 38673 RVA: 0x0036CC54 File Offset: 0x0036AE54
		public override void PostGameStart()
		{
			if (!ModsConfig.AnomalyActive)
			{
				return;
			}
			if (!Find.Anomaly.GenerateMonolith)
			{
				return;
			}
			if (Find.Anomaly.monolith == null)
			{
				return;
			}
			Find.Anomaly.monolith.AutoActivate(Find.TickManager.TicksGame + this.delayTicks);
		}

		// Token: 0x04005A84 RID: 23172
		public int delayTicks;

		// Token: 0x04005A85 RID: 23173
		private string delayTicksBuf;
	}
}
