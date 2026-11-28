using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200144B RID: 5195
	public class CompAbilityEffect_SprayLiquid : CompAbilityEffect
	{
		// Token: 0x170015E2 RID: 5602
		// (get) Token: 0x06007FD6 RID: 32726 RVA: 0x002D9A87 File Offset: 0x002D7C87
		private new CompProperties_AbilitySprayLiquid Props
		{
			get
			{
				return (CompProperties_AbilitySprayLiquid)this.props;
			}
		}

		// Token: 0x170015E3 RID: 5603
		// (get) Token: 0x06007FD7 RID: 32727 RVA: 0x002D9A94 File Offset: 0x002D7C94
		private Pawn Pawn
		{
			get
			{
				return this.parent.pawn;
			}
		}

		// Token: 0x06007FD8 RID: 32728 RVA: 0x002D9AA4 File Offset: 0x002D7CA4
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			foreach (IntVec3 c in this.AffectedCells(target))
			{
				((Projectile)GenSpawn.Spawn(this.Props.projectileDef, this.Pawn.Position, this.Pawn.Map, WipeMode.Vanish)).Launch(this.Pawn, this.Pawn.DrawPos, c, c, ProjectileHitFlags.IntendedTarget, this.parent.verb.preventFriendlyFire, null, null);
			}
			if (this.Props.sprayEffecter != null)
			{
				this.Props.sprayEffecter.Spawn(this.parent.pawn.Position, target.Cell, this.parent.pawn.Map, 1f).Cleanup();
			}
			base.Apply(target, dest);
		}

		// Token: 0x06007FD9 RID: 32729 RVA: 0x002D9BA8 File Offset: 0x002D7DA8
		public override void DrawEffectPreview(LocalTargetInfo target)
		{
			GenDraw.DrawFieldEdges(this.AffectedCells(target), 2900);
		}

		// Token: 0x06007FDA RID: 32730 RVA: 0x002D9BBC File Offset: 0x002D7DBC
		public override bool AICanTargetNow(LocalTargetInfo target)
		{
			if (this.Pawn.Faction != null)
			{
				foreach (IntVec3 c in this.AffectedCells(target))
				{
					List<Thing> thingList = c.GetThingList(this.Pawn.Map);
					for (int i = 0; i < thingList.Count; i++)
					{
						if (thingList[i].Faction == this.Pawn.Faction)
						{
							return false;
						}
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x06007FDB RID: 32731 RVA: 0x002D9C58 File Offset: 0x002D7E58
		private List<IntVec3> AffectedCells(LocalTargetInfo target)
		{
			this.tmpCellDots.Clear();
			this.tmpCells.Clear();
			this.tmpCellDots.Add(new Pair<IntVec3, float>(target.Cell, 999f));
			if (this.Props.numCellsToHit > 1)
			{
				Vector3 b = this.Pawn.Position.ToVector3Shifted().Yto0();
				Vector3 b2 = target.Cell.ToVector3Shifted().Yto0();
				IntVec3[] array;
				int num;
				if (this.Props.numCellsToHit < 10)
				{
					array = GenAdj.AdjacentCells;
					num = 8;
				}
				else
				{
					array = GenRadial.RadialPattern;
					num = this.Props.numCellsToHit + 5;
				}
				for (int i = 0; i < num; i++)
				{
					IntVec3 first = target.Cell + array[i];
					Vector3 a = first.ToVector3Shifted().Yto0();
					float second = Vector3.Dot((a - b).normalized, (a - b2).normalized);
					this.tmpCellDots.Add(new Pair<IntVec3, float>(first, second));
				}
				this.tmpCellDots.SortBy((Pair<IntVec3, float> x) => -x.Second);
			}
			Map map = this.Pawn.Map;
			int num2 = Mathf.Min(this.tmpCellDots.Count, this.Props.numCellsToHit);
			for (int j = 0; j < num2; j++)
			{
				IntVec3 first2 = this.tmpCellDots[j].First;
				if (first2.InBounds(map))
				{
					if (first2.Filled(map))
					{
						Building_Door door = first2.GetDoor(map);
						if (door == null || !door.Open)
						{
							goto IL_1E5;
						}
					}
					ShootLine shootLine;
					if (this.parent.verb.TryFindShootLineFromTo(this.Pawn.Position, first2, out shootLine, true))
					{
						this.tmpCells.Add(first2);
					}
				}
				IL_1E5:;
			}
			return this.tmpCells;
		}

		// Token: 0x04004FF6 RID: 20470
		private List<Pair<IntVec3, float>> tmpCellDots = new List<Pair<IntVec3, float>>();

		// Token: 0x04004FF7 RID: 20471
		private List<IntVec3> tmpCells = new List<IntVec3>();
	}
}
