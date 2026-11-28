using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D1F RID: 7455
	public class FloatMenuOptionProvider_Childcare : FloatMenuOptionProvider
	{
		// Token: 0x17002334 RID: 9012
		// (get) Token: 0x0600C0DD RID: 49373 RVA: 0x00465334 File Offset: 0x00463534
		protected override bool Drafted
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002335 RID: 9013
		// (get) Token: 0x0600C0DE RID: 49374 RVA: 0x00465337 File Offset: 0x00463537
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002336 RID: 9014
		// (get) Token: 0x0600C0DF RID: 49375 RVA: 0x0046533A File Offset: 0x0046353A
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002337 RID: 9015
		// (get) Token: 0x0600C0E0 RID: 49376 RVA: 0x0046533D File Offset: 0x0046353D
		public override bool CanTargetDespawned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C0E1 RID: 49377 RVA: 0x00465340 File Offset: 0x00463540
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return ModsConfig.BiotechActive;
		}

		// Token: 0x0600C0E2 RID: 49378 RVA: 0x00465347 File Offset: 0x00463547
		public override IEnumerable<FloatMenuOption> GetOptionsFor(Pawn clickedPawn, FloatMenuContext context)
		{
			FloatMenuOptionProvider_Childcare.<GetOptionsFor>d__9 <GetOptionsFor>d__ = new FloatMenuOptionProvider_Childcare.<GetOptionsFor>d__9(-2);
			<GetOptionsFor>d__.<>3__clickedPawn = clickedPawn;
			<GetOptionsFor>d__.<>3__context = context;
			return <GetOptionsFor>d__;
		}
	}
}
