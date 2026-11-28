using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200202B RID: 8235
	public static class FleckMaker
	{
		// Token: 0x0600D088 RID: 53384 RVA: 0x004CB300 File Offset: 0x004C9500
		public static FleckCreationData GetDataStatic(Vector3 loc, Map map, FleckDef fleckDef, float scale = 1f)
		{
			return new FleckCreationData
			{
				def = fleckDef,
				spawnPosition = loc,
				scale = scale,
				ageTicksOverride = -1
			};
		}

		// Token: 0x0600D089 RID: 53385 RVA: 0x004CB336 File Offset: 0x004C9536
		public static void Static(IntVec3 cell, Map map, FleckDef fleckDef, float scale = 1f)
		{
			FleckMaker.Static(cell.ToVector3Shifted(), map, fleckDef, scale);
		}

		// Token: 0x0600D08A RID: 53386 RVA: 0x004CB347 File Offset: 0x004C9547
		public static void Static(Vector3 loc, Map map, FleckDef fleckDef, float scale = 1f)
		{
			map.flecks.CreateFleck(FleckMaker.GetDataStatic(loc, map, fleckDef, scale));
		}

		// Token: 0x0600D08B RID: 53387 RVA: 0x004CB360 File Offset: 0x004C9560
		public static FleckCreationData GetDataThrowMetaIcon(IntVec3 cell, Map map, FleckDef fleckDef, float velocitySpeed = 0.42f)
		{
			return new FleckCreationData
			{
				def = fleckDef,
				spawnPosition = cell.ToVector3Shifted() + new Vector3(0.35f, 0f, 0.35f) + new Vector3(Rand.Value, 0f, Rand.Value) * 0.1f,
				velocityAngle = (float)Rand.Range(30, 60),
				velocitySpeed = velocitySpeed,
				rotationRate = Rand.Range(-3f, 3f),
				scale = 0.7f,
				ageTicksOverride = -1
			};
		}

		// Token: 0x0600D08C RID: 53388 RVA: 0x004CB40B File Offset: 0x004C960B
		public static void ThrowMetaIcon(IntVec3 cell, Map map, FleckDef fleckDef, float velocitySpeed = 0.42f)
		{
			if (!cell.ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			map.flecks.CreateFleck(FleckMaker.GetDataThrowMetaIcon(cell, map, fleckDef, velocitySpeed));
		}

		// Token: 0x0600D08D RID: 53389 RVA: 0x004CB42C File Offset: 0x004C962C
		public static FleckCreationData GetDataAttachedOverlay(Thing thing, FleckDef fleckDef, Vector3 offset, float scale = 1f, float solidTimeOverride = -1f)
		{
			return new FleckCreationData
			{
				def = fleckDef,
				spawnPosition = thing.DrawPos + offset,
				solidTimeOverride = new float?(solidTimeOverride),
				scale = scale,
				ageTicksOverride = -1
			};
		}

		// Token: 0x0600D08E RID: 53390 RVA: 0x004CB47B File Offset: 0x004C967B
		public static void AttachedOverlay(Thing thing, FleckDef fleckDef, Vector3 offset, float scale = 1f, float solidTimeOverride = -1f)
		{
			if (thing.Destroyed)
			{
				return;
			}
			thing.MapHeld.flecks.CreateFleck(FleckMaker.GetDataAttachedOverlay(thing, fleckDef, offset, scale, solidTimeOverride));
		}

		// Token: 0x0600D08F RID: 53391 RVA: 0x004CB4A4 File Offset: 0x004C96A4
		public static void ThrowShamblerParticles(Thing thing)
		{
			if (!thing.Spawned)
			{
				return;
			}
			FleckCreationData dataAttachedOverlay = FleckMaker.GetDataAttachedOverlay(thing, FleckDefOf.FleckShamblerDecay, Vector3.zero, Rand.Range(0.8f, 1.2f), -1f);
			dataAttachedOverlay.spawnPosition += new Vector3(Rand.Range(-0.1f, 0.1f), 0f, Rand.Range(0f, 0.5f));
			dataAttachedOverlay.rotationRate = Rand.Range(-12f, 12f);
			dataAttachedOverlay.velocityAngle = Rand.Range(-35f, 35f);
			dataAttachedOverlay.velocitySpeed = Rand.Range(0.1f, 0.3f);
			dataAttachedOverlay.solidTimeOverride = new float?(Rand.Range(0.7f, 1f));
			thing.MapHeld.flecks.CreateFleck(dataAttachedOverlay);
		}

		// Token: 0x0600D090 RID: 53392 RVA: 0x004CB58C File Offset: 0x004C978C
		public static void ThrowMetaPuffs(CellRect rect, Map map)
		{
			if (!Find.TickManager.Paused)
			{
				for (int i = rect.minX; i <= rect.maxX; i++)
				{
					for (int j = rect.minZ; j <= rect.maxZ; j++)
					{
						FleckMaker.ThrowMetaPuffs(new TargetInfo(new IntVec3(i, 0, j), map, false));
					}
				}
			}
		}

		// Token: 0x0600D091 RID: 53393 RVA: 0x004CB5E8 File Offset: 0x004C97E8
		public static void ThrowMetaPuffs(TargetInfo targ)
		{
			Vector3 a = targ.HasThing ? targ.Thing.TrueCenter() : targ.Cell.ToVector3Shifted();
			int num = Rand.RangeInclusive(4, 6);
			for (int i = 0; i < num; i++)
			{
				FleckMaker.ThrowMetaPuff(a + new Vector3(Rand.Range(-0.5f, 0.5f), 0f, Rand.Range(-0.5f, 0.5f)), targ.Map);
			}
		}

		// Token: 0x0600D092 RID: 53394 RVA: 0x004CB66C File Offset: 0x004C986C
		public static void ThrowMetaPuff(Vector3 loc, Map map)
		{
			if (!loc.ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc, map, FleckDefOf.MetaPuff, 1.9f);
			dataStatic.rotationRate = (float)Rand.Range(-60, 60);
			dataStatic.velocityAngle = (float)Rand.Range(0, 360);
			dataStatic.velocitySpeed = Rand.Range(0.6f, 0.78f);
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x0600D093 RID: 53395 RVA: 0x004CB6DC File Offset: 0x004C98DC
		public static void ThrowAirPuffUp(Vector3 loc, Map map)
		{
			if (!loc.ToIntVec3().ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc + new Vector3(Rand.Range(-0.02f, 0.02f), 0f, Rand.Range(-0.02f, 0.02f)), map, FleckDefOf.AirPuff, 1.5f);
			dataStatic.rotationRate = (float)Rand.RangeInclusive(-240, 240);
			dataStatic.velocityAngle = (float)Rand.Range(-45, 45);
			dataStatic.velocitySpeed = Rand.Range(1.2f, 1.5f);
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x0600D094 RID: 53396 RVA: 0x004CB784 File Offset: 0x004C9984
		public static void ThrowBreathPuff(Vector3 loc, Map map, float throwAngle, Vector3 inheritVelocity)
		{
			if (!loc.ToIntVec3().ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc + new Vector3(Rand.Range(-0.005f, 0.005f), 0f, Rand.Range(-0.005f, 0.005f)), map, FleckDefOf.AirPuff, Rand.Range(0.6f, 0.7f));
			dataStatic.rotationRate = (float)Rand.RangeInclusive(-240, 240);
			dataStatic.velocityAngle = throwAngle + (float)Rand.Range(-10, 10);
			dataStatic.velocitySpeed = Rand.Range(0.1f, 0.8f);
			dataStatic.velocity = new Vector3?(inheritVelocity * 0.5f);
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x0600D095 RID: 53397 RVA: 0x004CB84D File Offset: 0x004C9A4D
		public static void ThrowDustPuff(IntVec3 cell, Map map, float scale)
		{
			FleckMaker.ThrowDustPuff(cell.ToVector3() + new Vector3(Rand.Value, 0f, Rand.Value), map, scale);
		}

		// Token: 0x0600D096 RID: 53398 RVA: 0x004CB878 File Offset: 0x004C9A78
		public static void ThrowDustPuff(Vector3 loc, Map map, float scale)
		{
			if (!loc.ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc, map, FleckDefOf.DustPuff, 1.9f * scale);
			dataStatic.rotationRate = (float)Rand.Range(-60, 60);
			dataStatic.velocityAngle = (float)Rand.Range(0, 360);
			dataStatic.velocitySpeed = Rand.Range(0.6f, 0.75f);
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x0600D097 RID: 53399 RVA: 0x004CB8EC File Offset: 0x004C9AEC
		public static void ThrowDustPuffThick(Vector3 loc, Map map, float scale, Color color)
		{
			if (!loc.ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc, map, FleckDefOf.DustPuffThick, scale);
			dataStatic.rotationRate = (float)Rand.Range(-60, 60);
			dataStatic.velocityAngle = (float)Rand.Range(0, 360);
			dataStatic.velocitySpeed = Rand.Range(0.6f, 0.75f);
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x0600D098 RID: 53400 RVA: 0x004CB958 File Offset: 0x004C9B58
		public static void ThrowTornadoDustPuff(Vector3 loc, Map map, float scale, Color color)
		{
			if (!loc.ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc, map, FleckDefOf.TornadoDustPuff, 1.9f * scale);
			dataStatic.rotationRate = (float)Rand.Range(-60, 60);
			dataStatic.velocityAngle = (float)Rand.Range(0, 360);
			dataStatic.velocitySpeed = Rand.Range(0.6f, 0.75f);
			dataStatic.instanceColor = new Color?(color);
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x0600D099 RID: 53401 RVA: 0x004CB9D8 File Offset: 0x004C9BD8
		public static void ThrowSmoke(Vector3 loc, Map map, float size)
		{
			if (!loc.ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc, map, FleckDefOf.Smoke, Rand.Range(1.5f, 2.5f) * size);
			dataStatic.rotationRate = Rand.Range(-30f, 30f);
			dataStatic.velocityAngle = (float)Rand.Range(30, 40);
			dataStatic.velocitySpeed = Rand.Range(0.5f, 0.7f);
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x0600D09A RID: 53402 RVA: 0x004CBA58 File Offset: 0x004C9C58
		public static void ThrowFireGlow(Vector3 c, Map map, float size)
		{
			if (!c.ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			Vector3 vector = c + size * new Vector3(Rand.Value - 0.5f, 0f, Rand.Value - 0.5f);
			if (!vector.InBounds(map))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(vector, map, FleckDefOf.FireGlow, Rand.Range(4f, 6f) * size);
			dataStatic.rotationRate = Rand.Range(-3f, 3f);
			dataStatic.velocityAngle = (float)Rand.Range(0, 360);
			dataStatic.velocitySpeed = 0.12f;
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x0600D09B RID: 53403 RVA: 0x004CBB08 File Offset: 0x004C9D08
		public static void ThrowHeatGlow(IntVec3 c, Map map, float size)
		{
			Vector3 vector = c.ToVector3Shifted();
			if (!vector.ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			vector += size * new Vector3(Rand.Value - 0.5f, 0f, Rand.Value - 0.5f);
			if (!vector.InBounds(map))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(vector, map, FleckDefOf.HeatGlow, Rand.Range(4f, 6f) * size);
			dataStatic.rotationRate = Rand.Range(-3f, 3f);
			dataStatic.velocityAngle = (float)Rand.Range(0, 360);
			dataStatic.velocitySpeed = 0.12f;
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x0600D09C RID: 53404 RVA: 0x004CBBC0 File Offset: 0x004C9DC0
		public static void ThrowMicroSparks(Vector3 loc, Map map)
		{
			if (!loc.ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			loc -= new Vector3(0.5f, 0f, 0.5f);
			loc += new Vector3(Rand.Value, 0f, Rand.Value);
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc, map, FleckDefOf.MicroSparks, Rand.Range(0.8f, 1.2f));
			dataStatic.rotationRate = Rand.Range(-12f, 12f);
			dataStatic.velocityAngle = (float)Rand.Range(35, 45);
			dataStatic.velocitySpeed = 1.2f;
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x0600D09D RID: 53405 RVA: 0x004CBC6C File Offset: 0x004C9E6C
		public static void ThrowLightningGlow(Vector3 loc, Map map, float size)
		{
			if (!loc.ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc + size * new Vector3(Rand.Value - 0.5f, 0f, Rand.Value - 0.5f), map, FleckDefOf.LightningGlow, Rand.Range(4f, 6f) * size);
			dataStatic.rotationRate = Rand.Range(-3f, 3f);
			dataStatic.velocityAngle = (float)Rand.Range(0, 360);
			dataStatic.velocitySpeed = 1.2f;
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x0600D09E RID: 53406 RVA: 0x004CBD10 File Offset: 0x004C9F10
		public static void PlaceFootprint(Vector3 loc, Map map, float rot)
		{
			if (!loc.ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc, map, FleckDefOf.Footprint, 0.5f);
			dataStatic.rotation = rot;
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x0600D09F RID: 53407 RVA: 0x004CBD4E File Offset: 0x004C9F4E
		public static void ThrowHorseshoe(Pawn thrower, IntVec3 targetCell)
		{
			FleckMaker.ThrowObjectAt(thrower, targetCell, FleckDefOf.Horseshoe);
		}

		// Token: 0x0600D0A0 RID: 53408 RVA: 0x004CBD5C File Offset: 0x004C9F5C
		public static void ThrowStone(Pawn thrower, IntVec3 targetCell)
		{
			FleckMaker.ThrowObjectAt(thrower, targetCell, FleckDefOf.Stone);
		}

		// Token: 0x0600D0A1 RID: 53409 RVA: 0x004CBD6C File Offset: 0x004C9F6C
		private static void ThrowObjectAt(Pawn thrower, IntVec3 targetCell, FleckDef fleck)
		{
			if (!thrower.Position.ShouldSpawnMotesAt(thrower.Map, true))
			{
				return;
			}
			float num = Rand.Range(3.8f, 5.6f);
			Vector3 vector = targetCell.ToVector3Shifted() + Vector3Utility.RandomHorizontalOffset((1f - (float)thrower.skills.GetSkill(SkillDefOf.Shooting).Level / 20f) * 1.8f);
			vector.y = thrower.DrawPos.y;
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(thrower.DrawPos, thrower.Map, fleck, 1f);
			dataStatic.rotationRate = (float)Rand.Range(-300, 300);
			dataStatic.velocityAngle = (vector - dataStatic.spawnPosition).AngleFlat();
			dataStatic.velocitySpeed = num;
			dataStatic.airTimeLeft = new float?((float)Mathf.RoundToInt((dataStatic.spawnPosition - vector).MagnitudeHorizontal() / num));
			thrower.Map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x0600D0A2 RID: 53410 RVA: 0x004CBE70 File Offset: 0x004CA070
		public static void ThrowExplosionCell(IntVec3 cell, Map map, FleckDef fleckDef, Color color)
		{
			if (!cell.ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(cell.ToVector3Shifted(), map, fleckDef, 1f);
			dataStatic.rotation = (float)(90 * Rand.RangeInclusive(0, 3));
			dataStatic.instanceColor = new Color?(color);
			map.flecks.CreateFleck(dataStatic);
			if (Rand.Value < 0.7f)
			{
				FleckMaker.ThrowDustPuff(cell, map, 1.2f);
			}
		}

		// Token: 0x0600D0A3 RID: 53411 RVA: 0x004CBEE0 File Offset: 0x004CA0E0
		public static void ThrowExplosionInterior(Vector3 loc, Map map, FleckDef fleckDef)
		{
			if (!loc.ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc, map, fleckDef, Rand.Range(3f, 4.5f));
			dataStatic.rotationRate = Rand.Range(-30f, 30f);
			dataStatic.velocityAngle = (float)Rand.Range(0, 360);
			dataStatic.velocitySpeed = Rand.Range(0.48f, 0.72f);
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x0600D0A4 RID: 53412 RVA: 0x004CBF5C File Offset: 0x004CA15C
		public static void WaterSplash(Vector3 loc, Map map, float size, float velocity)
		{
			if (!loc.ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			map.flecks.CreateFleck(new FleckCreationData
			{
				def = FleckDefOf.WaterSplash,
				targetSize = size,
				velocitySpeed = velocity,
				spawnPosition = loc
			});
		}

		// Token: 0x0600D0A5 RID: 53413 RVA: 0x004CBFAC File Offset: 0x004CA1AC
		public static void WaterRipple(Vector3 loc, Map map, float size)
		{
			if (!loc.ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			map.flecks.CreateFleck(new FleckCreationData
			{
				def = FleckDefOf.WaterRipple,
				spawnPosition = loc,
				scale = size,
				ageTicksOverride = -1
			});
		}

		// Token: 0x0600D0A6 RID: 53414 RVA: 0x004CBFFC File Offset: 0x004CA1FC
		public static void ConnectingLine(Vector3 start, Vector3 end, FleckDef fleckDef, Map map, float width = 1f)
		{
			Vector3 vector = end - start;
			float x = vector.MagnitudeHorizontal();
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(start + vector * 0.5f, map, fleckDef, 1f);
			dataStatic.exactScale = new Vector3?(new Vector3(x, 1f, width));
			dataStatic.rotation = Mathf.Atan2(-vector.z, vector.x) * 57.29578f;
			map.flecks.CreateFleck(dataStatic);
		}
	}
}
