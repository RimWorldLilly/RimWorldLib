using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002D4 RID: 724
	public class GenStep_ScatterThings : GenStep_Scatterer
	{
		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x0600161F RID: 5663 RVA: 0x00084707 File Offset: 0x00082907
		public override int SeedPart
		{
			get
			{
				return 1158116095;
			}
		}

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x06001620 RID: 5664 RVA: 0x00084710 File Offset: 0x00082910
		private List<Rot4> PossibleRotations
		{
			get
			{
				if (this.possibleRotationsInt == null)
				{
					this.possibleRotationsInt = new List<Rot4>();
					if (this.thingDef.rotatable)
					{
						this.possibleRotationsInt.Add(Rot4.North);
						this.possibleRotationsInt.Add(Rot4.East);
						this.possibleRotationsInt.Add(Rot4.South);
						this.possibleRotationsInt.Add(Rot4.West);
					}
					else
					{
						this.possibleRotationsInt.Add(Rot4.North);
					}
				}
				return this.possibleRotationsInt;
			}
		}

		// Token: 0x06001621 RID: 5665 RVA: 0x00084798 File Offset: 0x00082998
		public override void Generate(Map map, GenStepParams parms)
		{
			if (this.ShouldSkipMap(map))
			{
				return;
			}
			int count = this.CalculateFinalCount(map);
			IntRange one;
			if (this.thingDef.ingestible != null && this.thingDef.ingestible.IsMeal && this.thingDef.stackLimit <= 10)
			{
				one = IntRange.One;
			}
			else if (this.thingDef.stackLimit > 5)
			{
				one = new IntRange(Mathf.RoundToInt((float)this.thingDef.stackLimit * 0.5f), this.thingDef.stackLimit);
			}
			else
			{
				one = new IntRange(this.thingDef.stackLimit, this.thingDef.stackLimit);
			}
			List<int> list = GenStep_ScatterThings.CountDividedIntoStacks(count, one);
			for (int i = 0; i < list.Count; i++)
			{
				IntVec3 intVec;
				if (!this.TryFindScatterCell(map, out intVec))
				{
					return;
				}
				this.ScatterAt(intVec, map, parms, list[i]);
				this.usedSpots.Add(intVec);
			}
			this.usedSpots.Clear();
			this.clusterCenter = IntVec3.Invalid;
			this.leftInCluster = 0;
		}

		// Token: 0x06001622 RID: 5666 RVA: 0x000848A0 File Offset: 0x00082AA0
		protected override bool TryFindScatterCell(Map map, out IntVec3 result)
		{
			if (this.clusterSize > 1)
			{
				if (this.leftInCluster <= 0)
				{
					if (!base.TryFindScatterCell(map, out this.clusterCenter))
					{
						string str = "Could not find cluster center to scatter ";
						ThingDef thingDef = this.thingDef;
						Log.Error(str + ((thingDef != null) ? thingDef.ToString() : null));
					}
					this.leftInCluster = this.clusterSize;
				}
				this.leftInCluster--;
				result = CellFinder.RandomClosewalkCellNear(this.clusterCenter, map, 4, delegate(IntVec3 x)
				{
					Rot4 rot;
					return this.TryGetRandomValidRotation(x, map, out rot);
				});
				return result.IsValid;
			}
			return base.TryFindScatterCell(map, out result);
		}

		// Token: 0x06001623 RID: 5667 RVA: 0x00084960 File Offset: 0x00082B60
		protected virtual Thing GenerateThing()
		{
			Thing thing = ThingMaker.MakeThing(this.thingDef, this.stuff);
			CompQuality compQuality;
			if (thing.TryGetComp(out compQuality))
			{
				compQuality.SetQuality(this.quality.GetValueOrDefault(QualityCategory.Normal), new ArtGenerationContext?(ArtGenerationContext.Outsider));
			}
			if (this.thingDef.IsIngestible && this.thingDef.ingestible.IsMeal)
			{
				FoodUtility.GenerateGoodIngredients(thing, Faction.OfPlayer.ideos.PrimaryIdeo);
			}
			if (this.minify && this.thingDef.Minifiable)
			{
				thing = thing.MakeMinified(DestroyMode.Vanish);
			}
			return thing;
		}

		// Token: 0x06001624 RID: 5668 RVA: 0x000849F4 File Offset: 0x00082BF4
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int stackCount = 1)
		{
			Rot4 rot;
			if (!this.TryGetRandomValidRotation(loc, map, out rot))
			{
				string str = "Could not find any valid rotation for ";
				ThingDef thingDef = this.thingDef;
				Log.Warning(str + ((thingDef != null) ? thingDef.ToString() : null));
				return;
			}
			if (this.clearSpaceSize > 0)
			{
				foreach (IntVec3 c in GridShapeMaker.IrregularLump(loc, map, this.clearSpaceSize, null))
				{
					Building edifice = c.GetEdifice(map);
					if (edifice != null)
					{
						edifice.Destroy(DestroyMode.Vanish);
					}
				}
			}
			Thing thing = this.GenerateThing();
			if (thing.def.category == ThingCategory.Item)
			{
				thing.stackCount = stackCount;
				thing.SetForbidden(true, false);
				Thing thing2;
				GenPlace.TryPlaceThing(thing, loc, map, ThingPlaceMode.Near, out thing2, null, null, null, 1);
				if (this.nearPlayerStart && thing2 != null && thing2.def.category == ThingCategory.Item && TutorSystem.TutorialMode)
				{
					Find.TutorialState.AddStartingItem(thing2);
				}
			}
			else
			{
				GenSpawn.Spawn(thing, loc, map, rot, WipeMode.Vanish, false, false);
			}
			if (this.filthDef != null)
			{
				foreach (IntVec3 c2 in thing.OccupiedRect().ExpandedBy(this.filthExpandBy))
				{
					if (Rand.Chance(this.filthChance) && c2.InBounds(thing.Map))
					{
						FilthMaker.TryMakeFilth(c2, thing.Map, this.filthDef, 1, FilthSourceFlags.None, true);
					}
				}
			}
			if (this.createUsedRect)
			{
				MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects").Add(thing.OccupiedRect());
			}
		}

		// Token: 0x06001625 RID: 5669 RVA: 0x00084BB0 File Offset: 0x00082DB0
		protected override bool CanScatterAt(IntVec3 loc, Map map)
		{
			if (!base.CanScatterAt(loc, map))
			{
				return false;
			}
			if (!GenSpawn.CanSpawnAt(this.thingDef, loc, map, null, true))
			{
				return false;
			}
			Rot4 rot;
			if (!this.TryGetRandomValidRotation(loc, map, out rot))
			{
				return false;
			}
			if (this.terrainValidationRadius > 0f)
			{
				foreach (IntVec3 c in GenRadial.RadialCellsAround(loc, this.terrainValidationRadius, true))
				{
					if (c.InBounds(map))
					{
						TerrainDef terrain = c.GetTerrain(map);
						if (this.terrainValidationDisallowed != null)
						{
							foreach (string tag in this.terrainValidationDisallowed)
							{
								if (terrain.HasTag(tag))
								{
									return false;
								}
							}
						}
						if (this.terrainValidationAllowed != null)
						{
							foreach (string tag2 in this.terrainValidationAllowed)
							{
								if (!terrain.HasTag(tag2))
								{
									return false;
								}
							}
						}
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x06001626 RID: 5670 RVA: 0x00084D0C File Offset: 0x00082F0C
		private bool TryGetRandomValidRotation(IntVec3 loc, Map map, out Rot4 rot)
		{
			List<Rot4> possibleRotations = this.PossibleRotations;
			for (int i = 0; i < possibleRotations.Count; i++)
			{
				if (this.IsRotationValid(loc, possibleRotations[i], map))
				{
					GenStep_ScatterThings.tmpRotations.Add(possibleRotations[i]);
				}
			}
			if (GenStep_ScatterThings.tmpRotations.TryRandomElement(out rot))
			{
				GenStep_ScatterThings.tmpRotations.Clear();
				return true;
			}
			rot = Rot4.Invalid;
			return false;
		}

		// Token: 0x06001627 RID: 5671 RVA: 0x00084D78 File Offset: 0x00082F78
		private bool IsRotationValid(IntVec3 loc, Rot4 rot, Map map)
		{
			CellRect cellRect = GenAdj.OccupiedRect(loc, rot, this.thingDef.size);
			if (!cellRect.InBounds(map))
			{
				return false;
			}
			if (this.avoidUsedRects && MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects").Any((CellRect ur) => ur.Overlaps(cellRect)))
			{
				return false;
			}
			if (this.thingDef.hasInteractionCell)
			{
				IntVec3 c2 = loc + this.thingDef.interactionCellOffset.RotatedBy(rot);
				if (!c2.InBounds(map) || !c2.Walkable(map))
				{
					return false;
				}
			}
			return !GenSpawn.WouldWipeAnythingWith(loc, rot, this.thingDef, map, (Thing x) => x.def == this.thingDef || (x.def.category != ThingCategory.Plant && x.def.category != ThingCategory.Filth)) && (!this.skipRoofed || !cellRect.Any((IntVec3 c) => c.Roofed(map))) && this.thingDef.CanSpawnAt(loc, rot, map);
		}

		// Token: 0x06001628 RID: 5672 RVA: 0x00084E94 File Offset: 0x00083094
		public static List<int> CountDividedIntoStacks(int count, IntRange stackSizeRange)
		{
			List<int> list = new List<int>();
			while (count > 0)
			{
				int num = Mathf.Min(count, stackSizeRange.RandomInRange);
				count -= num;
				list.Add(num);
			}
			if (stackSizeRange.max > 2)
			{
				for (int i = 0; i < list.Count * 4; i++)
				{
					int num2 = Rand.RangeInclusive(0, list.Count - 1);
					int num3 = Rand.RangeInclusive(0, list.Count - 1);
					if (num2 != num3 && list[num2] > list[num3])
					{
						int num4 = (int)((float)(list[num2] - list[num3]) * Rand.Value);
						List<int> list2 = list;
						int index = num2;
						list2[index] -= num4;
						list2 = list;
						index = num3;
						list2[index] += num4;
					}
				}
			}
			return list;
		}

		// Token: 0x0400131D RID: 4893
		public ThingDef thingDef;

		// Token: 0x0400131E RID: 4894
		public ThingDef stuff;

		// Token: 0x0400131F RID: 4895
		public int clearSpaceSize;

		// Token: 0x04001320 RID: 4896
		public int clusterSize = 1;

		// Token: 0x04001321 RID: 4897
		public QualityCategory? quality;

		// Token: 0x04001322 RID: 4898
		public bool minify;

		// Token: 0x04001323 RID: 4899
		public bool avoidUsedRects;

		// Token: 0x04001324 RID: 4900
		public bool createUsedRect;

		// Token: 0x04001325 RID: 4901
		public bool skipRoofed;

		// Token: 0x04001326 RID: 4902
		public ThingDef filthDef;

		// Token: 0x04001327 RID: 4903
		public int filthExpandBy;

		// Token: 0x04001328 RID: 4904
		public float filthChance = 0.5f;

		// Token: 0x04001329 RID: 4905
		public float terrainValidationRadius;

		// Token: 0x0400132A RID: 4906
		[NoTranslate]
		private List<string> terrainValidationDisallowed;

		// Token: 0x0400132B RID: 4907
		[NoTranslate]
		private List<string> terrainValidationAllowed;

		// Token: 0x0400132C RID: 4908
		[Unsaved(false)]
		private IntVec3 clusterCenter;

		// Token: 0x0400132D RID: 4909
		[Unsaved(false)]
		private int leftInCluster;

		// Token: 0x0400132E RID: 4910
		private const int ClusterRadius = 4;

		// Token: 0x0400132F RID: 4911
		private List<Rot4> possibleRotationsInt;

		// Token: 0x04001330 RID: 4912
		private static List<Rot4> tmpRotations = new List<Rot4>();
	}
}
