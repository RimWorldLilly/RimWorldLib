using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B17 RID: 6935
	public class FloorEtchingRambling : Building
	{
		// Token: 0x0600B159 RID: 45401 RVA: 0x003E7C9C File Offset: 0x003E5E9C
		public override void PostMake()
		{
			base.PostMake();
			this.comp = base.GetComp<CompFloorEtchingRambling>();
		}

		// Token: 0x17001FF4 RID: 8180
		// (get) Token: 0x0600B15A RID: 45402 RVA: 0x003E7CB0 File Offset: 0x003E5EB0
		public override string DescriptionFlavor
		{
			get
			{
				if (this.comp.deciphered)
				{
					return this.comp.message;
				}
				return base.DescriptionFlavor;
			}
		}

		// Token: 0x04006670 RID: 26224
		private CompFloorEtchingRambling comp;
	}
}
