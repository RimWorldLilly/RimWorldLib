using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D35 RID: 7477
	public class FloatMenuOptionProvider_Mechanitor : FloatMenuOptionProvider
	{
		// Token: 0x17002387 RID: 9095
		// (get) Token: 0x0600C17A RID: 49530 RVA: 0x00467B93 File Offset: 0x00465D93
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002388 RID: 9096
		// (get) Token: 0x0600C17B RID: 49531 RVA: 0x00467B96 File Offset: 0x00465D96
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002389 RID: 9097
		// (get) Token: 0x0600C17C RID: 49532 RVA: 0x00467B99 File Offset: 0x00465D99
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700238A RID: 9098
		// (get) Token: 0x0600C17D RID: 49533 RVA: 0x00467B9C File Offset: 0x00465D9C
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C17E RID: 49534 RVA: 0x00467B9F File Offset: 0x00465D9F
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return ModsConfig.BiotechActive && MechanitorUtility.IsMechanitor(context.FirstSelectedPawn);
		}

		// Token: 0x0600C17F RID: 49535 RVA: 0x00467BB5 File Offset: 0x00465DB5
		public override IEnumerable<FloatMenuOption> GetOptionsFor(Pawn clickedPawn, FloatMenuContext context)
		{
			FloatMenuOptionProvider_Mechanitor.<GetOptionsFor>d__9 <GetOptionsFor>d__ = new FloatMenuOptionProvider_Mechanitor.<GetOptionsFor>d__9(-2);
			<GetOptionsFor>d__.<>3__clickedPawn = clickedPawn;
			<GetOptionsFor>d__.<>3__context = context;
			return <GetOptionsFor>d__;
		}
	}
}
