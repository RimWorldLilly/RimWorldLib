using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D38 RID: 7480
	public class FloatMenuOptionProvider_PickUpItem : FloatMenuOptionProvider
	{
		// Token: 0x17002391 RID: 9105
		// (get) Token: 0x0600C18B RID: 49547 RVA: 0x00467EA3 File Offset: 0x004660A3
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002392 RID: 9106
		// (get) Token: 0x0600C18C RID: 49548 RVA: 0x00467EA6 File Offset: 0x004660A6
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002393 RID: 9107
		// (get) Token: 0x0600C18D RID: 49549 RVA: 0x00467EA9 File Offset: 0x004660A9
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600C18E RID: 49550 RVA: 0x00467EAC File Offset: 0x004660AC
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return !context.FirstSelectedPawn.IsFormingCaravan();
		}

		// Token: 0x0600C18F RID: 49551 RVA: 0x00467EBC File Offset: 0x004660BC
		public override IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
		{
			FloatMenuOptionProvider_PickUpItem.<GetOptionsFor>d__7 <GetOptionsFor>d__ = new FloatMenuOptionProvider_PickUpItem.<GetOptionsFor>d__7(-2);
			<GetOptionsFor>d__.<>3__clickedThing = clickedThing;
			<GetOptionsFor>d__.<>3__context = context;
			return <GetOptionsFor>d__;
		}
	}
}
