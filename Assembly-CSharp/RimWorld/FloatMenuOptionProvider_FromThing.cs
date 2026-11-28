using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D2C RID: 7468
	public class FloatMenuOptionProvider_FromThing : FloatMenuOptionProvider
	{
		// Token: 0x17002368 RID: 9064
		// (get) Token: 0x0600C145 RID: 49477 RVA: 0x00467223 File Offset: 0x00465423
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002369 RID: 9065
		// (get) Token: 0x0600C146 RID: 49478 RVA: 0x00467226 File Offset: 0x00465426
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700236A RID: 9066
		// (get) Token: 0x0600C147 RID: 49479 RVA: 0x00467229 File Offset: 0x00465429
		protected override bool Multiselect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700236B RID: 9067
		// (get) Token: 0x0600C148 RID: 49480 RVA: 0x0046722C File Offset: 0x0046542C
		protected override bool CanSelfTarget
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C149 RID: 49481 RVA: 0x0046722F File Offset: 0x0046542F
		public override IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
		{
			FloatMenuOptionProvider_FromThing.<GetOptionsFor>d__8 <GetOptionsFor>d__ = new FloatMenuOptionProvider_FromThing.<GetOptionsFor>d__8(-2);
			<GetOptionsFor>d__.<>3__clickedThing = clickedThing;
			<GetOptionsFor>d__.<>3__context = context;
			return <GetOptionsFor>d__;
		}
	}
}
