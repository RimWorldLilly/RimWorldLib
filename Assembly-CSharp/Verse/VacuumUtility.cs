using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200028C RID: 652
	public static class VacuumUtility
	{
		// Token: 0x060013A6 RID: 5030 RVA: 0x0006F5D8 File Offset: 0x0006D7D8
		public static void PawnVacuumTickInterval(Pawn pawn, int delta)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			if (!pawn.IsHashIntervalTick(60, delta) || !pawn.Spawned || !pawn.Map.Biome.inVacuum)
			{
				return;
			}
			if (!pawn.HarmedByVacuum)
			{
				return;
			}
			float vacuum = pawn.Position.GetVacuum(pawn.Map);
			if (vacuum < 0.5f)
			{
				return;
			}
			float num = 0.02f * vacuum * Mathf.Max(1f - pawn.GetStatValue(StatDefOf.VacuumResistance, true, -1), 0f);
			if (num > 0f)
			{
				HealthUtility.AdjustSeverity(pawn, HediffDefOf.VacuumExposure, num);
			}
		}

		// Token: 0x060013A7 RID: 5031 RVA: 0x0006F674 File Offset: 0x0006D874
		public static bool CanBeVacuumBurnt(Pawn pawn)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return false;
			}
			if (!pawn.def.race.canBeVacuumBurnt)
			{
				return false;
			}
			if (pawn.health.hediffSet.PreventVacuumBurns)
			{
				return false;
			}
			if (pawn.genes != null)
			{
				foreach (Gene gene in pawn.genes.GenesListForReading)
				{
					if (gene.Active && gene.def.immuneToVacuumBurns)
					{
						return false;
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x060013A8 RID: 5032 RVA: 0x0006F71C File Offset: 0x0006D91C
		public static bool TryGetVacuumBurnablePart(Pawn pawn, out BodyPartRecord p)
		{
			if (!VacuumUtility.CanBeVacuumBurnt(pawn))
			{
				p = null;
				return false;
			}
			VacuumUtility.tmpParts.AddRange(pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Outside, null, null));
			for (int i = VacuumUtility.tmpParts.Count - 1; i >= 0; i--)
			{
				BodyPartRecord bodyPartRecord = VacuumUtility.tmpParts[i];
				bool flag = false;
				if (!bodyPartRecord.def.canBeVacuumBurnt || !bodyPartRecord.def.IsSkinCovered(bodyPartRecord, pawn.health.hediffSet))
				{
					VacuumUtility.tmpParts.RemoveAt(i);
				}
				else
				{
					if (pawn.apparel != null)
					{
						for (int j = 0; j < bodyPartRecord.groups.Count; j++)
						{
							if (pawn.apparel.BodyPartGroupIsCovered(bodyPartRecord.groups[j], (Apparel ap) => VacuumUtility.IsProtectiveApparel(ap.def)))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							Apparel apparel;
							flag = pawn.apparel.TryGetFirstApparelOnBodyPart(bodyPartRecord, out apparel, (Apparel ap) => VacuumUtility.IsProtectiveApparel(ap.def));
						}
					}
					if (flag)
					{
						VacuumUtility.tmpParts.RemoveAt(i);
					}
				}
			}
			if (VacuumUtility.tmpParts.Empty<BodyPartRecord>())
			{
				p = null;
				return false;
			}
			VacuumUtility.tmpParts.TryRandomElementByWeight((BodyPartRecord w) => w.coverage, out p);
			VacuumUtility.tmpParts.Clear();
			return p != null;
		}

		// Token: 0x060013A9 RID: 5033 RVA: 0x0006F89C File Offset: 0x0006DA9C
		public static bool IsProtectiveApparel(ThingDef thing)
		{
			return thing.equippedStatOffsets.GetStatOffsetFromList(StatDefOf.VacuumResistance) >= 0.1f;
		}

		// Token: 0x060013AA RID: 5034 RVA: 0x0006F8B8 File Offset: 0x0006DAB8
		private static bool EverInVacuum(IntVec3 cell, Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return false;
			}
			if (map == null)
			{
				return false;
			}
			if (!cell.InBounds(map) || !map.Biome.inVacuum)
			{
				return false;
			}
			Building edifice = cell.GetEdifice(map);
			return edifice == null || ((edifice.def.passability != Traversability.Impassable || edifice.def.Fillage != FillCategory.Full) && (edifice.def.building.canExchangeVacuum || edifice.def.building.alwaysExchangeVacuum || edifice.ExchangeVacuum || edifice.def.Fillage != FillCategory.Full));
		}

		// Token: 0x060013AB RID: 5035 RVA: 0x0006F957 File Offset: 0x0006DB57
		public static bool VacuumConcernTo(this Thing thing, Pawn pawn)
		{
			return pawn.ConcernedByVacuum && thing.PositionHeld.GetVacuum(pawn.MapHeld) >= 0.5f;
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x0006F97E File Offset: 0x0006DB7E
		public static bool VacuumConcernTo(this IntVec3 cell, Pawn pawn)
		{
			return pawn.ConcernedByVacuum && cell.GetVacuum(pawn.MapHeld) >= 0.5f;
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x0006F9A0 File Offset: 0x0006DBA0
		public static float GetVacuum(this IntVec3 cell, Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return 0f;
			}
			if (!VacuumUtility.EverInVacuum(cell, map))
			{
				return 0f;
			}
			Room room = cell.GetRoom(map);
			if (room == null)
			{
				return 1f;
			}
			return room.Vacuum;
		}

		// Token: 0x060013AE RID: 5038 RVA: 0x0006F9E0 File Offset: 0x0006DBE0
		public static bool IsRoomAirtight(Room room)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return false;
			}
			VacuumUtility.tmpRoomOpenToOutside.Clear();
			if (VacuumUtility.<IsRoomAirtight>g__IsRoomDirectlyOpenToOutside|13_0(room))
			{
				return false;
			}
			foreach (IntVec3 c in room.BorderCells)
			{
				Building edifice = c.GetEdifice(room.Map);
				if (edifice != null && (edifice == null || !edifice.IsAirtight || edifice.def.building.alwaysExchangeVacuum))
				{
					CellRect cellRect = edifice.OccupiedRect();
					foreach (IntVec3 a in cellRect)
					{
						for (int i = 0; i < 4; i++)
						{
							IntVec3 intVec = a + GenAdj.CardinalDirections[i];
							if (!cellRect.Contains(intVec))
							{
								Room room2 = intVec.GetRoom(room.Map);
								if (room2 != null && room2 != room && VacuumUtility.<IsRoomAirtight>g__IsRoomDirectlyOpenToOutside|13_0(room2))
								{
									return false;
								}
							}
						}
					}
				}
			}
			return true;
		}

		// Token: 0x060013B0 RID: 5040 RVA: 0x0006FB2C File Offset: 0x0006DD2C
		[CompilerGenerated]
		internal static bool <IsRoomAirtight>g__IsRoomDirectlyOpenToOutside|13_0(Room room)
		{
			bool result;
			if (VacuumUtility.tmpRoomOpenToOutside.TryGetValue(room, out result))
			{
				return result;
			}
			if (room.TouchesMapEdge || room.OpenRoofCount > 0)
			{
				VacuumUtility.tmpRoomOpenToOutside[room] = true;
				return true;
			}
			foreach (IntVec3 c in room.Cells)
			{
				TerrainDef terrainDef = room.Map.terrainGrid.FoundationAt(c);
				if (terrainDef == null || !terrainDef.IsSubstructure)
				{
					VacuumUtility.tmpRoomOpenToOutside[room] = true;
					return true;
				}
			}
			VacuumUtility.tmpRoomOpenToOutside[room] = false;
			return false;
		}

		// Token: 0x04001105 RID: 4357
		private const float SeverityPerSecond = 0.02f;

		// Token: 0x04001106 RID: 4358
		public const float MinVacuumForDamage = 0.5f;

		// Token: 0x04001107 RID: 4359
		public const float ApparelMinResistAvoidDamage = 0.1f;

		// Token: 0x04001108 RID: 4360
		private static readonly List<BodyPartRecord> tmpParts = new List<BodyPartRecord>();

		// Token: 0x04001109 RID: 4361
		private static Dictionary<Room, bool> tmpRoomOpenToOutside = new Dictionary<Room, bool>();
	}
}
