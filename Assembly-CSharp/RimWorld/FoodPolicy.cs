using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015B5 RID: 5557
	public class FoodPolicy : Policy
	{
		// Token: 0x060089B0 RID: 35248 RVA: 0x0031DCB2 File Offset: 0x0031BEB2
		public FoodPolicy()
		{
		}

		// Token: 0x060089B1 RID: 35249 RVA: 0x0031DCC5 File Offset: 0x0031BEC5
		public FoodPolicy(int id, string label) : base(id, label)
		{
		}

		// Token: 0x170017BD RID: 6077
		// (get) Token: 0x060089B2 RID: 35250 RVA: 0x0031DCDA File Offset: 0x0031BEDA
		protected override string LoadKey
		{
			get
			{
				return "FoodPolicy";
			}
		}

		// Token: 0x060089B3 RID: 35251 RVA: 0x0031DCE1 File Offset: 0x0031BEE1
		public bool Allows(ThingDef def)
		{
			return this.filter.Allows(def);
		}

		// Token: 0x060089B4 RID: 35252 RVA: 0x0031DCEF File Offset: 0x0031BEEF
		public bool Allows(Thing thing)
		{
			return this.filter.Allows(thing);
		}

		// Token: 0x060089B5 RID: 35253 RVA: 0x0031DD00 File Offset: 0x0031BF00
		public override void CopyFrom(Policy other)
		{
			FoodPolicy foodPolicy = other as FoodPolicy;
			if (foodPolicy == null)
			{
				return;
			}
			this.filter.CopyAllowancesFrom(foodPolicy.filter);
		}

		// Token: 0x060089B6 RID: 35254 RVA: 0x0031DD29 File Offset: 0x0031BF29
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThingFilter>(ref this.filter, "filter", Array.Empty<object>());
		}

		// Token: 0x040054D1 RID: 21713
		public ThingFilter filter = new ThingFilter();
	}
}
