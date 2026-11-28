using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D43 RID: 7491
	public class FloatMenuOptionProvider_Trade : FloatMenuOptionProvider
	{
		// Token: 0x170023B7 RID: 9143
		// (get) Token: 0x0600C1D3 RID: 49619 RVA: 0x00468DF4 File Offset: 0x00466FF4
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170023B8 RID: 9144
		// (get) Token: 0x0600C1D4 RID: 49620 RVA: 0x00468DF7 File Offset: 0x00466FF7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170023B9 RID: 9145
		// (get) Token: 0x0600C1D5 RID: 49621 RVA: 0x00468DFA File Offset: 0x00466FFA
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600C1D6 RID: 49622 RVA: 0x00468DFD File Offset: 0x00466FFD
		public override IEnumerable<FloatMenuOption> GetOptionsFor(Pawn clickedPawn, FloatMenuContext context)
		{
			FloatMenuOptionProvider_Trade.<GetOptionsFor>d__6 <GetOptionsFor>d__ = new FloatMenuOptionProvider_Trade.<GetOptionsFor>d__6(-2);
			<GetOptionsFor>d__.<>3__clickedPawn = clickedPawn;
			<GetOptionsFor>d__.<>3__context = context;
			return <GetOptionsFor>d__;
		}
	}
}
