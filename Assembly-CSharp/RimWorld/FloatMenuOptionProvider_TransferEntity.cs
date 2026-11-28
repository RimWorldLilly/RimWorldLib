using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D44 RID: 7492
	public class FloatMenuOptionProvider_TransferEntity : FloatMenuOptionProvider
	{
		// Token: 0x170023BA RID: 9146
		// (get) Token: 0x0600C1D8 RID: 49624 RVA: 0x00468E1C File Offset: 0x0046701C
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170023BB RID: 9147
		// (get) Token: 0x0600C1D9 RID: 49625 RVA: 0x00468E1F File Offset: 0x0046701F
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170023BC RID: 9148
		// (get) Token: 0x0600C1DA RID: 49626 RVA: 0x00468E22 File Offset: 0x00467022
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170023BD RID: 9149
		// (get) Token: 0x0600C1DB RID: 49627 RVA: 0x00468E25 File Offset: 0x00467025
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C1DC RID: 49628 RVA: 0x00468E28 File Offset: 0x00467028
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return ModsConfig.AnomalyActive;
		}

		// Token: 0x0600C1DD RID: 49629 RVA: 0x00468E2F File Offset: 0x0046702F
		public override IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
		{
			FloatMenuOptionProvider_TransferEntity.<GetOptionsFor>d__9 <GetOptionsFor>d__ = new FloatMenuOptionProvider_TransferEntity.<GetOptionsFor>d__9(-2);
			<GetOptionsFor>d__.<>3__clickedThing = clickedThing;
			<GetOptionsFor>d__.<>3__context = context;
			return <GetOptionsFor>d__;
		}
	}
}
