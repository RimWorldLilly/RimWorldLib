using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

// Token: 0x02000019 RID: 25
public static class ScenarioUtility
{
	// Token: 0x060000CD RID: 205 RVA: 0x00008200 File Offset: 0x00006400
	public static bool AllowsChildSelection(Scenario scenario)
	{
		List<ScenPart> parts = scenario.parts;
		for (int i = 0; i < parts.Count; i++)
		{
			ScenPart_ConfigPage_ConfigureStartingPawns_KindDefs scenPart_ConfigPage_ConfigureStartingPawns_KindDefs = parts[i] as ScenPart_ConfigPage_ConfigureStartingPawns_KindDefs;
			if (scenPart_ConfigPage_ConfigureStartingPawns_KindDefs != null)
			{
				using (List<PawnKindCount>.Enumerator enumerator = scenPart_ConfigPage_ConfigureStartingPawns_KindDefs.kindCounts.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!ScenarioUtility.CanBeChild(enumerator.Current.kindDef))
						{
							return false;
						}
					}
				}
			}
		}
		return true;
	}

	// Token: 0x060000CE RID: 206 RVA: 0x00008288 File Offset: 0x00006488
	private static bool CanBeChild(PawnKindDef kindDef)
	{
		if (!kindDef.apparelRequired.NullOrEmpty<ThingDef>())
		{
			for (int i = 0; i < kindDef.apparelRequired.Count; i++)
			{
				if (kindDef.apparelRequired[i].IsApparel && !kindDef.apparelRequired[i].apparel.developmentalStageFilter.Juvenile())
				{
					return false;
				}
			}
		}
		return true;
	}
}
