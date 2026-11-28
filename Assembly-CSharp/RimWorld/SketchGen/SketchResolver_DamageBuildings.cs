using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020020CD RID: 8397
	public class SketchResolver_DamageBuildings : SketchResolver
	{
		// Token: 0x170025E7 RID: 9703
		// (get) Token: 0x0600D35F RID: 54111 RVA: 0x004DE126 File Offset: 0x004DC326
		protected virtual float MaxPctOfTotalDestroyed
		{
			get
			{
				return 0.65f;
			}
		}

		// Token: 0x170025E8 RID: 9704
		// (get) Token: 0x0600D360 RID: 54112 RVA: 0x004DE12D File Offset: 0x004DC32D
		protected virtual float HpRandomFactor
		{
			get
			{
				return 1.2f;
			}
		}

		// Token: 0x170025E9 RID: 9705
		// (get) Token: 0x0600D361 RID: 54113 RVA: 0x004DE134 File Offset: 0x004DC334
		protected virtual float DestroyChanceExp
		{
			get
			{
				return 1.32f;
			}
		}

		// Token: 0x0600D362 RID: 54114 RVA: 0x004DE13B File Offset: 0x004DC33B
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return true;
		}

		// Token: 0x0600D363 RID: 54115 RVA: 0x004DE140 File Offset: 0x004DC340
		protected override void ResolveInt(SketchResolveParams parms)
		{
			CellRect occupiedRect = parms.sketch.OccupiedRect;
			Rot4 random = Rot4.Random;
			int num = 0;
			int count = parms.sketch.Buildables.Count;
			foreach (SketchBuildable sketchBuildable in parms.sketch.Buildables.InRandomOrder(null).ToList<SketchBuildable>())
			{
				bool flag;
				this.Damage(sketchBuildable, occupiedRect, random, parms.sketch, out flag, parms.destroyChanceExp);
				if (flag)
				{
					num++;
					this.ClearDisconnectedDoors(parms, sketchBuildable.pos);
					if ((float)num > (float)count * this.MaxPctOfTotalDestroyed)
					{
						break;
					}
				}
			}
		}

		// Token: 0x0600D364 RID: 54116 RVA: 0x004DE200 File Offset: 0x004DC400
		private void ClearDisconnectedDoors(SketchResolveParams parms, IntVec3 position)
		{
			IntVec3[] cardinalDirectionsAround = GenAdj.CardinalDirectionsAround;
			for (int i = 0; i < cardinalDirectionsAround.Length; i++)
			{
				IntVec3 position2 = cardinalDirectionsAround[i] + position;
				SketchThing door = parms.sketch.GetDoor(position2);
				if (door != null && !this.AdjacentToWall(parms.sketch, door))
				{
					parms.sketch.Remove(door);
				}
			}
		}

		// Token: 0x0600D365 RID: 54117 RVA: 0x004DE25C File Offset: 0x004DC45C
		private bool AdjacentToWall(Sketch sketch, SketchEntity entity)
		{
			foreach (IntVec3 b in GenAdj.CardinalDirections)
			{
				if (sketch.ThingsAt(entity.pos + b).Any((SketchThing t) => t.def == ThingDefOf.Wall))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600D366 RID: 54118 RVA: 0x004DE2C0 File Offset: 0x004DC4C0
		private void Damage(SketchBuildable buildable, CellRect rect, Rot4 dir, Sketch sketch, out bool destroyed, float? destroyChanceExp = null)
		{
			float num;
			if (dir.IsHorizontal)
			{
				num = (float)(buildable.pos.x - rect.minX) / (float)rect.Width;
			}
			else
			{
				num = (float)(buildable.pos.z - rect.minZ) / (float)rect.Height;
			}
			if (dir == Rot4.East || dir == Rot4.South)
			{
				num = 1f - num;
			}
			float num2 = num;
			num2 = Mathf.Pow(num2, destroyChanceExp ?? this.DestroyChanceExp);
			bool flag = false;
			LayoutSketch layoutSketch = sketch as LayoutSketch;
			if (layoutSketch != null)
			{
				foreach (IntVec3 pos in buildable.OccupiedRect.ExpandedBy(1).EdgeCells)
				{
					LayoutRoom layoutRoom;
					if (layoutSketch.structureLayout.TryGetRoom(pos, out layoutRoom) && layoutRoom.DontDestroyWallsDoors)
					{
						flag = true;
						break;
					}
				}
			}
			if (Rand.Chance(num2) && !flag)
			{
				sketch.Remove(buildable);
				destroyed = true;
				SketchTerrain sketchTerrain = buildable as SketchTerrain;
				if (sketchTerrain != null && sketchTerrain.def.burnedDef != null)
				{
					sketch.AddTerrain(sketchTerrain.def.burnedDef, sketchTerrain.pos, true);
					return;
				}
			}
			else
			{
				destroyed = false;
				SketchThing sketchThing = buildable as SketchThing;
				if (sketchThing != null)
				{
					sketchThing.hitPoints = new int?(Mathf.Clamp(Mathf.RoundToInt((float)sketchThing.MaxHitPoints * (1f - num) * Rand.Range(1f, this.HpRandomFactor)), 1, sketchThing.MaxHitPoints));
				}
			}
		}
	}
}
