using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009B2 RID: 2482
	public class JobDriver_Train : JobDriver_InteractAnimal
	{
		// Token: 0x17000DB2 RID: 3506
		// (get) Token: 0x06004B2D RID: 19245 RVA: 0x001D2C45 File Offset: 0x001D0E45
		protected override bool CanInteractNow
		{
			get
			{
				return !TrainableUtility.TrainedTooRecently(base.Animal);
			}
		}

		// Token: 0x06004B2E RID: 19246 RVA: 0x001D2C55 File Offset: 0x001D0E55
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Train.<MakeNewToils>d__2 <MakeNewToils>d__ = new JobDriver_Train.<MakeNewToils>d__2(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}
	}
}
