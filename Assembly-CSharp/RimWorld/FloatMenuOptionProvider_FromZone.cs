using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D2D RID: 7469
	public class FloatMenuOptionProvider_FromZone : FloatMenuOptionProvider
	{
		// Token: 0x1700236C RID: 9068
		// (get) Token: 0x0600C14B RID: 49483 RVA: 0x0046724E File Offset: 0x0046544E
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700236D RID: 9069
		// (get) Token: 0x0600C14C RID: 49484 RVA: 0x00467251 File Offset: 0x00465451
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700236E RID: 9070
		// (get) Token: 0x0600C14D RID: 49485 RVA: 0x00467254 File Offset: 0x00465454
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600C14E RID: 49486 RVA: 0x00467257 File Offset: 0x00465457
		public override IEnumerable<FloatMenuOption> GetOptions(FloatMenuContext context)
		{
			if (context.ClickedZone == null)
			{
				return Enumerable.Empty<FloatMenuOption>();
			}
			return context.ClickedZone.GetFloatMenuOptions(context.FirstSelectedPawn);
		}
	}
}
