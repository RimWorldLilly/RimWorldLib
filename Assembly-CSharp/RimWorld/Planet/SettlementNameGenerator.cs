using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023B6 RID: 9142
	public static class SettlementNameGenerator
	{
		// Token: 0x0600E3B2 RID: 58290 RVA: 0x0054A978 File Offset: 0x00548B78
		public static string GenerateSettlementName(WorldObject worldObject, RulePackDef rulePack = null)
		{
			if (rulePack == null)
			{
				Faction faction = worldObject.Faction;
				if (((faction != null) ? faction.def.settlementNameMaker : null) == null)
				{
					return worldObject.def.label;
				}
				rulePack = worldObject.Faction.def.settlementNameMaker;
			}
			SettlementNameGenerator.usedNames.Clear();
			List<Settlement> settlements = Find.WorldObjects.Settlements;
			for (int i = 0; i < settlements.Count; i++)
			{
				Settlement settlement = settlements[i];
				if (settlement.Name != null)
				{
					SettlementNameGenerator.usedNames.Add(settlement.Name);
				}
			}
			return NameGenerator.GenerateName(rulePack, SettlementNameGenerator.usedNames, true, null);
		}

		// Token: 0x04009115 RID: 37141
		private static readonly List<string> usedNames = new List<string>();
	}
}
