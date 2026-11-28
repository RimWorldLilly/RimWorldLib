using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D15 RID: 7445
	public class FloatMenuOptionProvider_CaptureEntity : FloatMenuOptionProvider
	{
		// Token: 0x1700230D RID: 8973
		// (get) Token: 0x0600C094 RID: 49300 RVA: 0x00463A0B File Offset: 0x00461C0B
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700230E RID: 8974
		// (get) Token: 0x0600C095 RID: 49301 RVA: 0x00463A0E File Offset: 0x00461C0E
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700230F RID: 8975
		// (get) Token: 0x0600C096 RID: 49302 RVA: 0x00463A11 File Offset: 0x00461C11
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002310 RID: 8976
		// (get) Token: 0x0600C097 RID: 49303 RVA: 0x00463A14 File Offset: 0x00461C14
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C098 RID: 49304 RVA: 0x00463A17 File Offset: 0x00461C17
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return ModsConfig.AnomalyActive;
		}

		// Token: 0x0600C099 RID: 49305 RVA: 0x00463A1E File Offset: 0x00461C1E
		public override IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
		{
			FloatMenuOptionProvider_CaptureEntity.<GetOptionsFor>d__9 <GetOptionsFor>d__ = new FloatMenuOptionProvider_CaptureEntity.<GetOptionsFor>d__9(-2);
			<GetOptionsFor>d__.<>3__clickedThing = clickedThing;
			<GetOptionsFor>d__.<>3__context = context;
			return <GetOptionsFor>d__;
		}
	}
}
