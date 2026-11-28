using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000981 RID: 2433
	public class Trigger_BecamePlayerEnemy : Trigger
	{
		// Token: 0x060049AE RID: 18862 RVA: 0x001CB166 File Offset: 0x001C9366
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.FactionRelationsChanged && lord.faction != null && lord.faction.HostileTo(Faction.OfPlayer);
		}
	}
}
