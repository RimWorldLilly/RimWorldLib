using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000980 RID: 2432
	public class Trigger_BecameNonHostileToPlayer : Trigger
	{
		// Token: 0x060049AC RID: 18860 RVA: 0x001CB10C File Offset: 0x001C930C
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.FactionRelationsChanged)
			{
				FactionRelationKind? previousRelationKind = signal.previousRelationKind;
				FactionRelationKind factionRelationKind = FactionRelationKind.Hostile;
				return (previousRelationKind.GetValueOrDefault() == factionRelationKind & previousRelationKind != null) && lord.faction != null && !lord.faction.HostileTo(Faction.OfPlayer);
			}
			return false;
		}
	}
}
