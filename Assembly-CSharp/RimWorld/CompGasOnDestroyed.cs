using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200189C RID: 6300
	public class CompGasOnDestroyed : ThingComp
	{
		// Token: 0x17001B04 RID: 6916
		// (get) Token: 0x06009DCB RID: 40395 RVA: 0x0038B004 File Offset: 0x00389204
		private CompProperties_GasOnDestroyed Props
		{
			get
			{
				return (CompProperties_GasOnDestroyed)this.props;
			}
		}

		// Token: 0x06009DCC RID: 40396 RVA: 0x0038B011 File Offset: 0x00389211
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (mode != DestroyMode.Vanish)
			{
				GasUtility.AddGas(this.parent.PositionHeld, previousMap, this.Props.gasType, this.Props.amount);
			}
		}
	}
}
