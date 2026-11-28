using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D41 RID: 7489
	public class FloatMenuOptionProvider_StartRitual : FloatMenuOptionProvider
	{
		// Token: 0x170023B0 RID: 9136
		// (get) Token: 0x0600C1C6 RID: 49606 RVA: 0x00468C1E File Offset: 0x00466E1E
		protected override bool Drafted
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170023B1 RID: 9137
		// (get) Token: 0x0600C1C7 RID: 49607 RVA: 0x00468C21 File Offset: 0x00466E21
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170023B2 RID: 9138
		// (get) Token: 0x0600C1C8 RID: 49608 RVA: 0x00468C24 File Offset: 0x00466E24
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600C1C9 RID: 49609 RVA: 0x00468C27 File Offset: 0x00466E27
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return ModsConfig.IdeologyActive;
		}

		// Token: 0x0600C1CA RID: 49610 RVA: 0x00468C2E File Offset: 0x00466E2E
		public override IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
		{
			FloatMenuOptionProvider_StartRitual.<GetOptionsFor>d__8 <GetOptionsFor>d__ = new FloatMenuOptionProvider_StartRitual.<GetOptionsFor>d__8(-2);
			<GetOptionsFor>d__.<>3__clickedThing = clickedThing;
			<GetOptionsFor>d__.<>3__context = context;
			return <GetOptionsFor>d__;
		}

		// Token: 0x04006EDA RID: 28378
		private static HashSet<RitualPatternDef> showingGizmosForRitualsTmp = new HashSet<RitualPatternDef>();
	}
}
