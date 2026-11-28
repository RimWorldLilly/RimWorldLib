using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001275 RID: 4725
	public class RoomPart_Gore : RoomPartWorker
	{
		// Token: 0x060074FC RID: 29948 RVA: 0x0029109C File Offset: 0x0028F29C
		public RoomPart_Gore(RoomPartDef def) : base(def)
		{
		}

		// Token: 0x060074FD RID: 29949 RVA: 0x002910A8 File Offset: 0x0028F2A8
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float threatPoints)
		{
			float num = (float)room.Area / 100f;
			int num2 = Mathf.Max(Mathf.RoundToInt(RoomPart_Gore.BloodGroupsPer100Cells.RandomInRange * num), 1);
			int num3 = 0;
			IntVec3 cell;
			while (num3 < num2 && room.TryGetRandomCellInRoom(map, out cell, 1, 0, null, false))
			{
				this.SpawnBloodPool(map, cell, room);
				num3++;
			}
			IntVec3 cell2;
			if (room.TryGetRandomCellInRoom(map, out cell2, 3, 0, (IntVec3 c) => RoomPart_Gore.ValidCell(map, c, room), false))
			{
				this.SpawnCorpseSmear(map, cell2, room, faction);
			}
		}

		// Token: 0x060074FE RID: 29950 RVA: 0x00291170 File Offset: 0x0028F370
		private void SpawnBloodPool(Map map, IntVec3 cell, LayoutRoom room)
		{
			GenSpawn.SpawnIrregularLump(ThingDefOf.Filth_Blood, cell, map, RoomPart_Gore.BloodCountRange, RoomPart_Gore.BloodDistanceRange, WipeMode.Vanish, (IntVec3 c) => RoomPart_Gore.ValidCell(map, c, room), null, null, null, null);
		}

		// Token: 0x060074FF RID: 29951 RVA: 0x002911C0 File Offset: 0x0028F3C0
		private void SpawnCorpseSmear(Map map, IntVec3 cell, LayoutRoom room, Faction faction)
		{
			int num = RoomPart_Gore.SmearLengthRange.RandomInRange;
			IntVec3 intVec = cell;
			HashSet<IntVec3> hashSet = new HashSet<IntVec3>();
			RoomPart_Gore.SpawnCorpse(map, cell, faction);
			while (num > 0 && RoomPart_Gore.ValidCell(map, intVec, room))
			{
				IntVec3 intVec2 = IntVec3.Invalid;
				int num2 = Rand.Range(0, 8);
				for (int i = 0; i < 8; i++)
				{
					IntVec3 intVec3 = intVec + GenAdj.AdjacentCellsAround[GenMath.PositiveMod(i + num2, 8)];
					if (!hashSet.Contains(intVec3) && RoomPart_Gore.ValidCell(map, intVec3, room))
					{
						intVec2 = intVec3;
						break;
					}
				}
				if (!intVec2.IsValid)
				{
					break;
				}
				Filth filth;
				FilthMaker.TryMakeFilth(intVec, map, ThingDefOf.Filth_BloodSmear, out filth, 1, FilthSourceFlags.None, false);
				Filth filth2;
				FilthMaker.TryMakeFilth(intVec, map, ThingDefOf.Filth_BloodSmear, out filth2, 1, FilthSourceFlags.None, false);
				if (filth != null)
				{
					filth.SetOverrideDrawPositionAndRotation(intVec.ToVector3().WithY(ThingDefOf.Filth_BloodSmear.Altitude), (intVec2 - intVec).AngleFlat);
					int num3 = Rand.Range(3, 5);
					for (int j = 0; j < num3; j++)
					{
						filth.ThickenFilth();
					}
				}
				if (filth2 != null)
				{
					Vector3 vector = (intVec2 - intVec).ToVector3();
					Vector3 v = intVec.ToVector3() + vector * 0.5f;
					filth2.SetOverrideDrawPositionAndRotation(v.WithY(ThingDefOf.Filth_BloodSmear.Altitude), vector.AngleFlat());
					int num4 = Rand.Range(3, 5);
					for (int k = 0; k < num4; k++)
					{
						filth2.ThickenFilth();
					}
				}
				hashSet.Add(intVec);
				intVec = intVec2;
				num--;
			}
		}

		// Token: 0x06007500 RID: 29952 RVA: 0x00291360 File Offset: 0x0028F560
		private static void SpawnCorpse(Map map, IntVec3 cell, Faction faction)
		{
			PawnKindDef pawnKindDef = PawnKindDefOf.Villager;
			if (faction != null)
			{
				pawnKindDef = faction.RandomPawnKind();
			}
			PawnKindDef kind = pawnKindDef;
			int randomInRange = RoomPart_Gore.DeadTicksRange.RandomInRange;
			DamageDef executionCut = DamageDefOf.ExecutionCut;
			Tool toolUsed = ThingDefOf.MeleeWeapon_Knife.tools[1];
			BodyPartDef neck = BodyPartDefOf.Neck;
			RoomGenUtility.SpawnCorpse(cell, kind, randomInRange, map, executionCut, null, false, null, toolUsed, null, neck);
		}

		// Token: 0x06007501 RID: 29953 RVA: 0x002913C7 File Offset: 0x0028F5C7
		private static bool ValidCell(Map map, IntVec3 cell, LayoutRoom room)
		{
			return room.Contains(cell, 0) && cell.GetEdifice(map) == null && cell.GetRoof(map) != null && cell.GetAffordances(map).Contains(TerrainAffordanceDefOf.Light);
		}

		// Token: 0x0400495B RID: 18779
		private static readonly FloatRange BloodGroupsPer100Cells = new FloatRange(1f, 2f);

		// Token: 0x0400495C RID: 18780
		private static readonly IntRange BloodCountRange = new IntRange(6, 10);

		// Token: 0x0400495D RID: 18781
		private static readonly IntRange BloodDistanceRange = new IntRange(5, 9);

		// Token: 0x0400495E RID: 18782
		private static readonly IntRange SmearLengthRange = new IntRange(6, 10);

		// Token: 0x0400495F RID: 18783
		private static readonly IntRange DeadTicksRange = new IntRange(180000, 36000000);
	}
}
