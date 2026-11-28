using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D9A RID: 3482
	public class ThoughtWorker_Precept_HasAutomatedTurrets : ThoughtWorker_Precept
	{
		// Token: 0x06005FDF RID: 24543 RVA: 0x0022D9CC File Offset: 0x0022BBCC
		public static void ResetStaticData()
		{
			ThoughtWorker_Precept_HasAutomatedTurrets.automatedTurretDefs.Clear();
			List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (allDefsListForReading[i].building != null && allDefsListForReading[i].building.IsTurret && !allDefsListForReading[i].HasComp(typeof(CompMannable)))
				{
					ThoughtWorker_Precept_HasAutomatedTurrets.automatedTurretDefs.Add(allDefsListForReading[i]);
				}
			}
		}

		// Token: 0x06005FE0 RID: 24544 RVA: 0x0022DA44 File Offset: 0x0022BC44
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (p.Faction == null || p.IsSlave)
			{
				return false;
			}
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				for (int j = 0; j < ThoughtWorker_Precept_HasAutomatedTurrets.automatedTurretDefs.Count; j++)
				{
					List<Thing> list = maps[i].listerThings.ThingsOfDef(ThoughtWorker_Precept_HasAutomatedTurrets.automatedTurretDefs[j]);
					for (int k = 0; k < list.Count; k++)
					{
						if (list[k].Faction == p.Faction)
						{
							return true;
						}
					}
				}
				if (ModsConfig.BiotechActive)
				{
					using (List<Pawn>.Enumerator enumerator = maps[i].mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.IsColonyMechPlayerControlled)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x040035E0 RID: 13792
		private static List<ThingDef> automatedTurretDefs = new List<ThingDef>();
	}
}
