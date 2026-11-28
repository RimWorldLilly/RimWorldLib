using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D1C RID: 7452
	public class FloatMenuOptionProvider_CarryToBiosculpterPod : FloatMenuOptionProvider
	{
		// Token: 0x17002328 RID: 9000
		// (get) Token: 0x0600C0C8 RID: 49352 RVA: 0x00464F68 File Offset: 0x00463168
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002329 RID: 9001
		// (get) Token: 0x0600C0C9 RID: 49353 RVA: 0x00464F6B File Offset: 0x0046316B
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700232A RID: 9002
		// (get) Token: 0x0600C0CA RID: 49354 RVA: 0x00464F6E File Offset: 0x0046316E
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700232B RID: 9003
		// (get) Token: 0x0600C0CB RID: 49355 RVA: 0x00464F71 File Offset: 0x00463171
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C0CC RID: 49356 RVA: 0x00464F74 File Offset: 0x00463174
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return ModsConfig.IdeologyActive;
		}

		// Token: 0x0600C0CD RID: 49357 RVA: 0x00464F7B File Offset: 0x0046317B
		public override IEnumerable<FloatMenuOption> GetOptionsFor(Pawn clickedPawn, FloatMenuContext context)
		{
			FloatMenuOptionProvider_CarryToBiosculpterPod.<GetOptionsFor>d__9 <GetOptionsFor>d__ = new FloatMenuOptionProvider_CarryToBiosculpterPod.<GetOptionsFor>d__9(-2);
			<GetOptionsFor>d__.<>3__clickedPawn = clickedPawn;
			<GetOptionsFor>d__.<>3__context = context;
			return <GetOptionsFor>d__;
		}
	}
}
