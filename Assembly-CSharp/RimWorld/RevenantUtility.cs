using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001506 RID: 5382
	public static class RevenantUtility
	{
		// Token: 0x060083CA RID: 33738 RVA: 0x002F6110 File Offset: 0x002F4310
		public static int NumSpawnedUnhypnotizedColonists(Map map)
		{
			if (map == null)
			{
				return 0;
			}
			int num = 0;
			using (List<Pawn>.Enumerator enumerator = map.mapPawns.FreeColonistsSpawned.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.health.hediffSet.HasHediff(HediffDefOf.RevenantHypnosis, false))
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x060083CB RID: 33739 RVA: 0x002F6184 File Offset: 0x002F4384
		public static Pawn ScanForTarget(Pawn pawn, bool forced = false)
		{
			RevenantUtility.tmpTargets.Clear();
			TraverseParms traverseParms = TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false);
			RegionTraverser.BreadthFirstTraverse(pawn.Position, pawn.Map, (Region from, Region to) => to.Allows(traverseParms, true), delegate(Region x)
			{
				List<Thing> list = x.ListerThings.ThingsInGroup(ThingRequestGroup.Pawn);
				for (int i = 0; i < list.Count; i++)
				{
					Pawn pawn3 = (Pawn)list[i];
					if (RevenantUtility.ValidTarget(pawn3))
					{
						RevenantUtility.tmpTargets.Add(pawn3);
					}
				}
				return false;
			}, 999999, RegionType.Set_Passable);
			Pawn pawn2;
			if (RevenantUtility.tmpTargets.TryRandomElement(out pawn2) && (forced || RevenantUtility.NearbyHumanlikePawnCount(pawn2.Position, pawn2.Map, 12f) < 5))
			{
				return pawn2;
			}
			return null;
		}

		// Token: 0x060083CC RID: 33740 RVA: 0x002F6224 File Offset: 0x002F4424
		public static Pawn GetClosestTargetInRadius(Pawn pawn, float radius)
		{
			List<Thing> list = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Pawn);
			float num = float.MaxValue;
			Pawn result = null;
			foreach (Thing thing in list)
			{
				Pawn pawn2 = (Pawn)thing;
				if (RevenantUtility.ValidTarget(pawn2) && pawn.Position.InHorDistOf(pawn2.Position, radius) && (float)pawn2.Position.DistanceToSquared(pawn.Position) < num && GenSight.LineOfSightToThing(pawn.Position, pawn2, pawn.Map, false, null))
				{
					num = (float)pawn2.Position.DistanceToSquared(pawn.Position);
					result = pawn2;
				}
			}
			return result;
		}

		// Token: 0x060083CD RID: 33741 RVA: 0x002F62EC File Offset: 0x002F44EC
		public static IntVec3 FindEscapeCell(Pawn pawn)
		{
			IntVec3 invalid = IntVec3.Invalid;
			CellFinder.TryFindRandomCellNear(pawn.Position, pawn.Map, 200, (IntVec3 x) => x.Standable(pawn.Map) && RevenantUtility.NearbyHumanlikePawnCount(x, pawn.Map, 20f) == 0, out invalid, -1);
			return invalid;
		}

		// Token: 0x060083CE RID: 33742 RVA: 0x002F6340 File Offset: 0x002F4540
		public static int NearbyHumanlikePawnCount(IntVec3 pos, Map map, float radius)
		{
			int num = 0;
			foreach (Thing thing in map.listerThings.ThingsInGroup(ThingRequestGroup.Pawn))
			{
				Pawn pawn = (Pawn)thing;
				if (RevenantUtility.ValidTarget(pawn) && !pawn.Downed && pos.InHorDistOf(pawn.Position, radius))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060083CF RID: 33743 RVA: 0x002F63C0 File Offset: 0x002F45C0
		public static bool ValidTarget(Pawn pawn)
		{
			return pawn.RaceProps.Humanlike && pawn.Faction != Faction.OfEntities && !pawn.IsSubhuman && !pawn.health.hediffSet.HasHediff(HediffDefOf.RevenantHypnosis, false);
		}

		// Token: 0x060083D0 RID: 33744 RVA: 0x002F6400 File Offset: 0x002F4600
		public static void CreateRevenantSmear(Pawn pawn)
		{
			Filth filth;
			FilthMaker.TryMakeFilth(pawn.Position, pawn.Map, ThingDefOf.Filth_RevenantSmear, out filth, 1, FilthSourceFlags.None, true);
			if (filth == null)
			{
				return;
			}
			((FilthRevenantSmear)filth).revenant = pawn;
			pawn.TryGetComp<CompRevenant>().revenantLastLeftSmear = Find.TickManager.TicksGame;
		}

		// Token: 0x060083D1 RID: 33745 RVA: 0x002F6450 File Offset: 0x002F4650
		public static void OnRevenantDeath(Pawn pawn, Map map)
		{
			Find.LetterStack.ReceiveLetter("LetterLabelRevenantKilled".Translate(), "LetterRevenantKilled".Translate(), LetterDefOf.PositiveEvent, new LookTargets(pawn.PositionHeld, map), null, null, null, null, 0, true);
			((DyingRevenant)GenSpawn.Spawn(ThingDefOf.DyingRevenant, pawn.Position, map, WipeMode.Vanish)).InitWith(pawn);
		}

		// Token: 0x0400516E RID: 20846
		private const float SurroundingPawnsRadius = 12f;

		// Token: 0x0400516F RID: 20847
		public const float NearbyTargetRange = 10f;

		// Token: 0x04005170 RID: 20848
		private const int EscapeDist = 200;

		// Token: 0x04005171 RID: 20849
		public const float EscapedRadius = 20f;

		// Token: 0x04005172 RID: 20850
		public static readonly FloatRange SearchForTargetCooldownRangeDays = new FloatRange(1.8f, 3.2f);

		// Token: 0x04005173 RID: 20851
		public const int WaitBeforeBecomingInvisibleTicks = 140;

		// Token: 0x04005174 RID: 20852
		public const int CheckForAnyTargetMTB = 2500;

		// Token: 0x04005175 RID: 20853
		public const int CheckForNearbyTargetMTB = 900;

		// Token: 0x04005176 RID: 20854
		public static readonly SimpleCurve SpeedRangeFromBecameVisibleCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(30f, 2.8f),
				true
			}
		};

		// Token: 0x04005177 RID: 20855
		public static readonly SimpleCurve RevealRangeFromNumColonistsCurve = new SimpleCurve
		{
			{
				new CurvePoint(2f, 26f),
				true
			},
			{
				new CurvePoint(3f, 18f),
				true
			},
			{
				new CurvePoint(8f, 16f),
				true
			},
			{
				new CurvePoint(15f, 10f),
				true
			}
		};

		// Token: 0x04005178 RID: 20856
		public static readonly SimpleCurve HypnotizeDurationSecondsFromNumColonistsCurve = new SimpleCurve
		{
			{
				new CurvePoint(2f, 15f),
				true
			},
			{
				new CurvePoint(4f, 11f),
				true
			},
			{
				new CurvePoint(10f, 6f),
				true
			}
		};

		// Token: 0x04005179 RID: 20857
		private static HashSet<Pawn> tmpTargets = new HashSet<Pawn>();
	}
}
