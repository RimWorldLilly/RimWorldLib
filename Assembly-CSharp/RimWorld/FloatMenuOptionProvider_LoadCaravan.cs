using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D33 RID: 7475
	public class FloatMenuOptionProvider_LoadCaravan : FloatMenuOptionProvider
	{
		// Token: 0x17002381 RID: 9089
		// (get) Token: 0x0600C16E RID: 49518 RVA: 0x00467B17 File Offset: 0x00465D17
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002382 RID: 9090
		// (get) Token: 0x0600C16F RID: 49519 RVA: 0x00467B1A File Offset: 0x00465D1A
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002383 RID: 9091
		// (get) Token: 0x0600C170 RID: 49520 RVA: 0x00467B1D File Offset: 0x00465D1D
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600C171 RID: 49521 RVA: 0x00467B20 File Offset: 0x00465D20
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return context.FirstSelectedPawn.IsFormingCaravan();
		}

		// Token: 0x0600C172 RID: 49522 RVA: 0x00467B2D File Offset: 0x00465D2D
		public override IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
		{
			FloatMenuOptionProvider_LoadCaravan.<GetOptionsFor>d__7 <GetOptionsFor>d__ = new FloatMenuOptionProvider_LoadCaravan.<GetOptionsFor>d__7(-2);
			<GetOptionsFor>d__.<>3__clickedThing = clickedThing;
			<GetOptionsFor>d__.<>3__context = context;
			return <GetOptionsFor>d__;
		}
	}
}
