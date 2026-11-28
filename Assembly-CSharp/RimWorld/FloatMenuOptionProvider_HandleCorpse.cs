using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D30 RID: 7472
	public class FloatMenuOptionProvider_HandleCorpse : FloatMenuOptionProvider
	{
		// Token: 0x17002376 RID: 9078
		// (get) Token: 0x0600C15C RID: 49500 RVA: 0x004673D4 File Offset: 0x004655D4
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002377 RID: 9079
		// (get) Token: 0x0600C15D RID: 49501 RVA: 0x004673D7 File Offset: 0x004655D7
		protected override bool Undrafted
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002378 RID: 9080
		// (get) Token: 0x0600C15E RID: 49502 RVA: 0x004673DA File Offset: 0x004655DA
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002379 RID: 9081
		// (get) Token: 0x0600C15F RID: 49503 RVA: 0x004673DD File Offset: 0x004655DD
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C160 RID: 49504 RVA: 0x004673E0 File Offset: 0x004655E0
		public override IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
		{
			FloatMenuOptionProvider_HandleCorpse.<GetOptionsFor>d__8 <GetOptionsFor>d__ = new FloatMenuOptionProvider_HandleCorpse.<GetOptionsFor>d__8(-2);
			<GetOptionsFor>d__.<>3__clickedThing = clickedThing;
			<GetOptionsFor>d__.<>3__context = context;
			return <GetOptionsFor>d__;
		}
	}
}
