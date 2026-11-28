using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FBD RID: 8125
	public class StatWorker_ContainmentStrength : StatWorker
	{
		// Token: 0x0600CE61 RID: 52833 RVA: 0x004BEDD4 File Offset: 0x004BCFD4
		public override bool ShouldShowFor(StatRequest req)
		{
			ThingDef thingDef = req.Def as ThingDef;
			return (thingDef != null && thingDef.HasComp<CompEntityHolder>()) || req.StatBases.GetStatValueFromList(StatDefOf.ContainmentStrength, 0f) != 0f;
		}

		// Token: 0x0600CE62 RID: 52834 RVA: 0x004BEE1C File Offset: 0x004BD01C
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			return base.GetValueUnfinalized(req, applyPostProcess) + this.CalculateValues(req).Sum;
		}

		// Token: 0x0600CE63 RID: 52835 RVA: 0x004BEE44 File Offset: 0x004BD044
		public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			StatWorker_ContainmentStrength.explanationSB.Clear();
			if (req.HasThing)
			{
				StatWorker_ContainmentStrength.ContainmentStrengthValues containmentStrengthValues = this.CalculateValues(req);
				if (containmentStrengthValues.lighting > 0f)
				{
					StringBuilder stringBuilder = StatWorker_ContainmentStrength.explanationSB;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
					defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("Stat_ContainmentStrength_AverageLighting".Translate());
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted<float>(containmentStrengthValues.lighting, "F2");
					stringBuilder.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				if (containmentStrengthValues.fullyRoofed < 0f)
				{
					StringBuilder stringBuilder2 = StatWorker_ContainmentStrength.explanationSB;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(2, 2);
					defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("Stat_ContainmentStrength_NotFullyRoofed".Translate());
					defaultInterpolatedStringHandler2.AppendLiteral(": ");
					defaultInterpolatedStringHandler2.AppendFormatted<float>(containmentStrengthValues.fullyRoofed, "F2");
					stringBuilder2.AppendLine(defaultInterpolatedStringHandler2.ToStringAndClear());
				}
				if (containmentStrengthValues.wallHp > 0f)
				{
					StringBuilder stringBuilder3 = StatWorker_ContainmentStrength.explanationSB;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(2, 2);
					defaultInterpolatedStringHandler3.AppendFormatted<TaggedString>("Stat_ContainmentStrength_AverageWallHp".Translate());
					defaultInterpolatedStringHandler3.AppendLiteral(": ");
					defaultInterpolatedStringHandler3.AppendFormatted<float>(containmentStrengthValues.wallHp, "F2");
					stringBuilder3.AppendLine(defaultInterpolatedStringHandler3.ToStringAndClear());
				}
				if (containmentStrengthValues.doorHp > 0f || containmentStrengthValues.doorBreach)
				{
					StringBuilder stringBuilder4 = StatWorker_ContainmentStrength.explanationSB;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(2, 2);
					defaultInterpolatedStringHandler4.AppendFormatted<TaggedString>("Stat_ContainmentStrength_AverageDoorHp".Translate());
					defaultInterpolatedStringHandler4.AppendLiteral(": ");
					defaultInterpolatedStringHandler4.AppendFormatted<float>(containmentStrengthValues.doorHp, "F2");
					string str = defaultInterpolatedStringHandler4.ToStringAndClear();
					string str2;
					if (!containmentStrengthValues.doorBreach)
					{
						str2 = string.Empty;
					}
					else
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler5 = new DefaultInterpolatedStringHandler(3, 1);
						defaultInterpolatedStringHandler5.AppendLiteral(" (");
						defaultInterpolatedStringHandler5.AppendFormatted<TaggedString>("Stat_ContainmentStrength_DoorForcedOpen".Translate());
						defaultInterpolatedStringHandler5.AppendLiteral(")");
						str2 = defaultInterpolatedStringHandler5.ToStringAndClear();
					}
					stringBuilder4.AppendLine(str + str2);
				}
				if (containmentStrengthValues.floor > 0f)
				{
					StringBuilder stringBuilder5 = StatWorker_ContainmentStrength.explanationSB;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler6 = new DefaultInterpolatedStringHandler(2, 2);
					defaultInterpolatedStringHandler6.AppendFormatted<TaggedString>("Stat_ContainmentStrength_Flooring".Translate());
					defaultInterpolatedStringHandler6.AppendLiteral(": ");
					defaultInterpolatedStringHandler6.AppendFormatted<float>(containmentStrengthValues.floor, "F2");
					stringBuilder5.AppendLine(defaultInterpolatedStringHandler6.ToStringAndClear());
				}
				if (containmentStrengthValues.otherHoldingPlatformsCount > 0)
				{
					StringBuilder stringBuilder6 = StatWorker_ContainmentStrength.explanationSB;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler7 = new DefaultInterpolatedStringHandler(2, 2);
					defaultInterpolatedStringHandler7.AppendFormatted<TaggedString>("Stat_ContainmentStrength_OtherHoldingPlatforms".Translate(containmentStrengthValues.otherHoldingPlatformsCount + 1));
					defaultInterpolatedStringHandler7.AppendLiteral(": ");
					defaultInterpolatedStringHandler7.AppendFormatted<float>(containmentStrengthValues.otherHoldingPlatformsOffset, "F2");
					stringBuilder6.AppendLine(defaultInterpolatedStringHandler7.ToStringAndClear());
				}
				if (!Mathf.Approximately(containmentStrengthValues.globalOffset, 0f))
				{
					StringBuilder stringBuilder7 = StatWorker_ContainmentStrength.explanationSB;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler8 = new DefaultInterpolatedStringHandler(2, 2);
					defaultInterpolatedStringHandler8.AppendFormatted<TaggedString>("Stat_ContainmentStrength_MapwideOffset".Translate());
					defaultInterpolatedStringHandler8.AppendLiteral(": ");
					defaultInterpolatedStringHandler8.AppendFormatted<float>(containmentStrengthValues.globalOffset, "F2");
					stringBuilder7.AppendLine(defaultInterpolatedStringHandler8.ToStringAndClear());
				}
				if (!Mathf.Approximately(containmentStrengthValues.holderFactor, 1f))
				{
					StringBuilder stringBuilder8 = StatWorker_ContainmentStrength.explanationSB;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler9 = new DefaultInterpolatedStringHandler(3, 2);
					defaultInterpolatedStringHandler9.AppendFormatted<TaggedString>("Stat_ContainmentStrength_Platform".Translate());
					defaultInterpolatedStringHandler9.AppendLiteral(": x");
					defaultInterpolatedStringHandler9.AppendFormatted<float>(containmentStrengthValues.holderFactor, "F2");
					stringBuilder8.AppendLine(defaultInterpolatedStringHandler9.ToStringAndClear());
				}
			}
			StatWorker_ContainmentStrength.explanationSB.Append(base.GetExplanationUnfinalized(req, numberSense));
			return StatWorker_ContainmentStrength.explanationSB.ToString();
		}

		// Token: 0x0600CE64 RID: 52836 RVA: 0x004BF1B0 File Offset: 0x004BD3B0
		private StatWorker_ContainmentStrength.ContainmentStrengthValues CalculateValues(StatRequest req)
		{
			StatWorker_ContainmentStrength.ContainmentStrengthValues result;
			if (!req.HasThing)
			{
				result = default(StatWorker_ContainmentStrength.ContainmentStrengthValues);
				return result;
			}
			Map mapHeld = req.Thing.MapHeld;
			Room room = req.Thing.GetRoom(RegionType.Set_All);
			if (room == null || mapHeld == null || room.TouchesMapEdge)
			{
				result = default(StatWorker_ContainmentStrength.ContainmentStrengthValues);
				return result;
			}
			bool psychologicallyOutdoors = room.PsychologicallyOutdoors;
			float num = 0f;
			foreach (IntVec3 c in room.Cells)
			{
				if (!psychologicallyOutdoors)
				{
					num += mapHeld.glowGrid.GroundGlowAt(c, false, false);
				}
			}
			num = num / (float)room.CellCount * 10f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			int num5 = 0;
			int num6 = 0;
			float globalOffset = 0f;
			bool doorBreach = false;
			float num7 = 0f;
			int num8 = 0;
			float holderFactor = 1f;
			if (!psychologicallyOutdoors)
			{
				int num9;
				try
				{
					this.CalculateDoorStats(room, out num9, out num3, out doorBreach);
				}
				finally
				{
					StatWorker_ContainmentStrength.tmpRegions.Clear();
					StatWorker_ContainmentStrength.tmpDoors.Clear();
				}
				foreach (Thing thing in room.ContainedAndAdjacentThings)
				{
					if (ThingRequestGroup.EntityHolder.Includes(thing.def) && thing != req.Thing)
					{
						num6++;
					}
				}
				CompEntityHolder compEntityHolder;
				if (req.Thing.TryGetComp(out compEntityHolder))
				{
					holderFactor = compEntityHolder.Props.containmentFactor;
				}
				foreach (IntVec3 c2 in room.Cells)
				{
					if (c2.InBounds(mapHeld) && !c2.Filled(mapHeld))
					{
						num7 += c2.GetTerrain(mapHeld).statBases.GetStatValueFromList(StatDefOf.ContainmentStrength, 0f);
						num8++;
					}
				}
				foreach (IntVec3 c3 in room.BorderCellsCached.Distinct<IntVec3>())
				{
					if (c3.InBounds(mapHeld))
					{
						Building edifice = c3.GetEdifice(mapHeld);
						if (edifice != null && !(edifice is Building_Door))
						{
							num2 += (float)edifice.HitPoints;
							num5++;
						}
					}
				}
				num4 = ((num8 == 0) ? 0f : (num7 / (float)num8));
				num2 = ((num5 == 0) ? 0f : StatWorker_ContainmentStrength.WallContainmentStrengthFromHPCurve.Evaluate(num2 / (float)num5));
				num3 = ((num9 == 0) ? 0f : (num3 / (float)num9 / 5f));
			}
			float num10 = num + num2 + num3 + num4;
			float num11 = num10;
			for (int i = 0; i < num6; i++)
			{
				num10 *= 0.9f;
			}
			num11 = num10 - num11;
			result = new StatWorker_ContainmentStrength.ContainmentStrengthValues
			{
				lighting = num,
				wallHp = num2,
				doorHp = num3,
				otherHoldingPlatformsCount = num6,
				otherHoldingPlatformsOffset = num11,
				globalOffset = globalOffset,
				floor = num4,
				fullyRoofed = ((!psychologicallyOutdoors && room.OpenRoofCount > 0) ? -30f : 0f),
				holderFactor = holderFactor,
				doorBreach = doorBreach
			};
			return result;
		}

		// Token: 0x0600CE65 RID: 52837 RVA: 0x004BF540 File Offset: 0x004BD740
		private void CalculateDoorStats(Room room, out int doorCount, out float avgDoorHp, out bool doorBreach)
		{
			foreach (Region region in room.Regions)
			{
				foreach (RegionLink regionLink in region.links)
				{
					Region otherRegion = regionLink.GetOtherRegion(region);
					if (otherRegion.type == RegionType.Portal && StatWorker_ContainmentStrength.tmpRegions.Add(otherRegion))
					{
						Building_Door door = otherRegion.door;
						for (int i = 0; i < otherRegion.links.Count; i++)
						{
							Region regionA = otherRegion.links[i].RegionA;
							Region regionB = otherRegion.links[i].RegionB;
							if ((regionA.Room != room && regionA != otherRegion && regionA.door != door) || (regionB.Room != room && regionB != otherRegion && regionB.door != door))
							{
								StatWorker_ContainmentStrength.tmpDoors.Add(door);
								break;
							}
						}
					}
				}
			}
			doorCount = StatWorker_ContainmentStrength.tmpDoors.Count;
			avgDoorHp = 0f;
			doorBreach = false;
			foreach (Building_Door building_Door in StatWorker_ContainmentStrength.tmpDoors)
			{
				if (building_Door.ContainmentBreached)
				{
					doorBreach = true;
					avgDoorHp = 0f;
				}
				else if (!doorBreach)
				{
					avgDoorHp += (float)building_Door.HitPoints;
				}
			}
		}

		// Token: 0x0600CE66 RID: 52838 RVA: 0x004BF700 File Offset: 0x004BD900
		public static bool AnyDoorForcedOpen(Room room)
		{
			if (room.PsychologicallyOutdoors)
			{
				return false;
			}
			foreach (Thing thing in room.ContainedAndAdjacentThings)
			{
				Building_Door building_Door = thing as Building_Door;
				if (building_Door != null && building_Door.ContainmentBreached)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04008358 RID: 33624
		private static HashSet<Region> tmpRegions = new HashSet<Region>();

		// Token: 0x04008359 RID: 33625
		private static HashSet<Building_Door> tmpDoors = new HashSet<Building_Door>();

		// Token: 0x0400835A RID: 33626
		private const float LightingMultiplier = 10f;

		// Token: 0x0400835B RID: 33627
		private const float DoorHpDivisor = 5f;

		// Token: 0x0400835C RID: 33628
		private const float OtherHoldingPlatformsFactor = 0.9f;

		// Token: 0x0400835D RID: 33629
		private const float NotFullyRoofed = -30f;

		// Token: 0x0400835E RID: 33630
		private static readonly SimpleCurve WallContainmentStrengthFromHPCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(1000f, 100f),
				true
			},
			{
				new CurvePoint(10000f, 150f),
				true
			}
		};

		// Token: 0x0400835F RID: 33631
		private static StringBuilder explanationSB = new StringBuilder();

		// Token: 0x02003C12 RID: 15378
		private struct ContainmentStrengthValues
		{
			// Token: 0x170036C7 RID: 14023
			// (get) Token: 0x0601584B RID: 88139 RVA: 0x006C96F3 File Offset: 0x006C78F3
			public float Sum
			{
				get
				{
					return (this.lighting + this.wallHp + this.doorHp + this.otherHoldingPlatformsOffset + this.globalOffset + this.fullyRoofed + this.floor) * this.holderFactor;
				}
			}

			// Token: 0x0400EE17 RID: 60951
			public float lighting;

			// Token: 0x0400EE18 RID: 60952
			public float wallHp;

			// Token: 0x0400EE19 RID: 60953
			public float doorHp;

			// Token: 0x0400EE1A RID: 60954
			public float floor;

			// Token: 0x0400EE1B RID: 60955
			public int otherHoldingPlatformsCount;

			// Token: 0x0400EE1C RID: 60956
			public float otherHoldingPlatformsOffset;

			// Token: 0x0400EE1D RID: 60957
			public float globalOffset;

			// Token: 0x0400EE1E RID: 60958
			public float fullyRoofed;

			// Token: 0x0400EE1F RID: 60959
			public float holderFactor;

			// Token: 0x0400EE20 RID: 60960
			public bool doorBreach;
		}
	}
}
