using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000975 RID: 2421
	public class Trigger_ChanceOnPlayerHarmNPCBuilding : Trigger
	{
		// Token: 0x06004994 RID: 18836 RVA: 0x001CABCE File Offset: 0x001C8DCE
		public Trigger_ChanceOnPlayerHarmNPCBuilding(float chance)
		{
			this.chance = chance;
		}

		// Token: 0x06004995 RID: 18837 RVA: 0x001CABE8 File Offset: 0x001C8DE8
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.BuildingDamaged && signal.dinfo.Def.ExternalViolenceFor(signal.thing) && signal.thing.def.category == ThingCategory.Building && signal.dinfo.Instigator != null && signal.dinfo.Instigator.Faction == Faction.OfPlayer && signal.thing.Faction != Faction.OfPlayer && Rand.Value < this.chance;
		}

		// Token: 0x04002F3C RID: 12092
		private float chance = 1f;
	}
}
