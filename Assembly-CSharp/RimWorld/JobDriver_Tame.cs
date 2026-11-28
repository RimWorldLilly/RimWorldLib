using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009B1 RID: 2481
	public class JobDriver_Tame : JobDriver_InteractAnimal
	{
		// Token: 0x17000DB1 RID: 3505
		// (get) Token: 0x06004B27 RID: 19239 RVA: 0x001D2B16 File Offset: 0x001D0D16
		protected override bool CanInteractNow
		{
			get
			{
				return !TameUtility.TriedToTameTooRecently(base.Animal);
			}
		}

		// Token: 0x06004B28 RID: 19240 RVA: 0x001D2B26 File Offset: 0x001D0D26
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Tame.<MakeNewToils>d__3 <MakeNewToils>d__ = new JobDriver_Tame.<MakeNewToils>d__3(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x04002FB7 RID: 12215
		private const TargetIndex FoodIndex = TargetIndex.C;
	}
}
