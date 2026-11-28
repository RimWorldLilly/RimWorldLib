using System;
using System.Collections.Generic;
using RimWorld.Utility;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D3C RID: 7484
	public class FloatMenuOptionProvider_Reload : FloatMenuOptionProvider
	{
		// Token: 0x1700239E RID: 9118
		// (get) Token: 0x0600C1A7 RID: 49575 RVA: 0x0046870F File Offset: 0x0046690F
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700239F RID: 9119
		// (get) Token: 0x0600C1A8 RID: 49576 RVA: 0x00468712 File Offset: 0x00466912
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170023A0 RID: 9120
		// (get) Token: 0x0600C1A9 RID: 49577 RVA: 0x00468715 File Offset: 0x00466915
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600C1AA RID: 49578 RVA: 0x00468718 File Offset: 0x00466918
		public override IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
		{
			FloatMenuOptionProvider_Reload.<GetOptionsFor>d__6 <GetOptionsFor>d__ = new FloatMenuOptionProvider_Reload.<GetOptionsFor>d__6(-2);
			<GetOptionsFor>d__.<>4__this = this;
			<GetOptionsFor>d__.<>3__clickedThing = clickedThing;
			<GetOptionsFor>d__.<>3__context = context;
			return <GetOptionsFor>d__;
		}

		// Token: 0x0600C1AB RID: 49579 RVA: 0x00468736 File Offset: 0x00466936
		private IEnumerable<IReloadableComp> GetReloadablesUsingAmmo(Pawn pawn, Thing clickedThing)
		{
			FloatMenuOptionProvider_Reload.<GetReloadablesUsingAmmo>d__7 <GetReloadablesUsingAmmo>d__ = new FloatMenuOptionProvider_Reload.<GetReloadablesUsingAmmo>d__7(-2);
			<GetReloadablesUsingAmmo>d__.<>3__pawn = pawn;
			<GetReloadablesUsingAmmo>d__.<>3__clickedThing = clickedThing;
			return <GetReloadablesUsingAmmo>d__;
		}
	}
}
