using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200107E RID: 4222
	public class QuestPart_FactionGoodwillLocked : QuestPartActivable
	{
		// Token: 0x17001220 RID: 4640
		// (get) Token: 0x060069CE RID: 27086 RVA: 0x002547BD File Offset: 0x002529BD
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				QuestPart_FactionGoodwillLocked.<get_InvolvedFactions>d__3 <get_InvolvedFactions>d__ = new QuestPart_FactionGoodwillLocked.<get_InvolvedFactions>d__3(-2);
				<get_InvolvedFactions>d__.<>4__this = this;
				return <get_InvolvedFactions>d__;
			}
		}

		// Token: 0x060069CF RID: 27087 RVA: 0x002547CD File Offset: 0x002529CD
		public override void Notify_FactionRemoved(Faction faction)
		{
			if (faction == this.faction1)
			{
				this.faction1 = null;
			}
			if (faction == this.faction2)
			{
				this.faction2 = null;
			}
		}

		// Token: 0x060069D0 RID: 27088 RVA: 0x002547EF File Offset: 0x002529EF
		public bool AppliesTo(Faction a, Faction b)
		{
			return base.State == QuestPartState.Enabled && ((this.faction1 == a && this.faction2 == b) || (this.faction1 == b && this.faction2 == a));
		}

		// Token: 0x060069D1 RID: 27089 RVA: 0x00254824 File Offset: 0x00252A24
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Faction>(ref this.faction1, "faction1", false);
			Scribe_References.Look<Faction>(ref this.faction2, "faction2", false);
		}

		// Token: 0x04004344 RID: 17220
		public Faction faction1;

		// Token: 0x04004345 RID: 17221
		public Faction faction2;
	}
}
