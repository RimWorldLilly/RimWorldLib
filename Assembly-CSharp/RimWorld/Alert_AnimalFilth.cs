using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BFC RID: 7164
	public class Alert_AnimalFilth : Alert
	{
		// Token: 0x0600B715 RID: 46869 RVA: 0x004083A0 File Offset: 0x004065A0
		public Alert_AnimalFilth()
		{
			this.defaultLabel = "AlertAnimalFilth".Translate();
		}

		// Token: 0x0600B716 RID: 46870 RVA: 0x004083D4 File Offset: 0x004065D4
		private void CalculateTargets()
		{
			this.targets.Clear();
			this.pawnEntries.Clear();
			foreach (Pawn pawn in PawnsFinder.AllMaps_SpawnedPawnsInFaction(Faction.OfPlayer))
			{
				if (pawn.HostFaction == null && !pawn.RaceProps.Humanlike && (!ModsConfig.AnomalyActive || !pawn.IsShambler) && pawn.GetStatValue(StatDefOf.FilthRate, true, -1) >= 4f)
				{
					IntVec3 position = pawn.Position;
					Map map = pawn.Map;
					TerrainDef terrain = position.GetTerrain(map);
					if (terrain.GetStatValueAbstract(StatDefOf.FilthMultiplier, null) > 0.5f && FilthMaker.TerrainAcceptsFilth(terrain, ThingDefOf.Filth_AnimalFilth, FilthSourceFlags.Pawn) && position.GetRoof(map) != null)
					{
						Room room = position.GetRoom(map);
						if (room != null && !room.Fogged && !room.TouchesMapEdge && !room.IsDoorway && pawn.Map.areaManager.Home[pawn.Position])
						{
							this.targets.Add(pawn);
							this.pawnEntries.Add(pawn.NameShortColored.Resolve());
						}
					}
				}
			}
		}

		// Token: 0x0600B717 RID: 46871 RVA: 0x00408548 File Offset: 0x00406748
		public override TaggedString GetExplanation()
		{
			return "AlertAnimalFilthDesc".Translate(this.pawnEntries.ToLineList("  - "));
		}

		// Token: 0x0600B718 RID: 46872 RVA: 0x00408569 File Offset: 0x00406769
		public override AlertReport GetReport()
		{
			this.CalculateTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x04006907 RID: 26887
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		// Token: 0x04006908 RID: 26888
		private List<string> pawnEntries = new List<string>();

		// Token: 0x04006909 RID: 26889
		private const float MinFilthRate = 4f;

		// Token: 0x0400690A RID: 26890
		private const float MinFilthMultiplier = 0.5f;
	}
}
