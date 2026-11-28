using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015B2 RID: 5554
	public class ApparelPolicy : Policy
	{
		// Token: 0x0600899E RID: 35230 RVA: 0x0031D833 File Offset: 0x0031BA33
		public ApparelPolicy()
		{
		}

		// Token: 0x0600899F RID: 35231 RVA: 0x0031D846 File Offset: 0x0031BA46
		public ApparelPolicy(int id, string label) : base(id, label)
		{
		}

		// Token: 0x170017B8 RID: 6072
		// (get) Token: 0x060089A0 RID: 35232 RVA: 0x0031D85B File Offset: 0x0031BA5B
		protected override string LoadKey
		{
			get
			{
				return "ApparelPolicy";
			}
		}

		// Token: 0x060089A1 RID: 35233 RVA: 0x0031D864 File Offset: 0x0031BA64
		public override void CopyFrom(Policy other)
		{
			ApparelPolicy apparelPolicy = other as ApparelPolicy;
			if (apparelPolicy == null)
			{
				return;
			}
			this.filter.CopyAllowancesFrom(apparelPolicy.filter);
		}

		// Token: 0x060089A2 RID: 35234 RVA: 0x0031D88D File Offset: 0x0031BA8D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThingFilter>(ref this.filter, "filter", Array.Empty<object>());
		}

		// Token: 0x040054C5 RID: 21701
		public ThingFilter filter = new ThingFilter();
	}
}
