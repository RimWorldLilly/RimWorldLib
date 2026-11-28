using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FF8 RID: 8184
	public class Pawn_SurroundingsTracker : IExposable
	{
		// Token: 0x0600CF57 RID: 53079 RVA: 0x004C5A6E File Offset: 0x004C3C6E
		public Pawn_SurroundingsTracker()
		{
		}

		// Token: 0x0600CF58 RID: 53080 RVA: 0x004C5AA2 File Offset: 0x004C3CA2
		public Pawn_SurroundingsTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600CF59 RID: 53081 RVA: 0x004C5AE0 File Offset: 0x004C3CE0
		private int NumSightingsInRange(ref List<TreeSighting> sightings, int ticks)
		{
			int num = 0;
			for (int i = 0; i < sightings.Count; i++)
			{
				if (sightings[i].TicksSinceSighting <= ticks)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600CF5A RID: 53082 RVA: 0x004C5B18 File Offset: 0x004C3D18
		public int NumSightingsInRange(TreeCategory treeCategory, int ticks)
		{
			switch (treeCategory)
			{
			case TreeCategory.Mini:
				return this.NumSightingsInRange(ref this.miniTreeSightings, ticks);
			case TreeCategory.Full:
				return this.NumSightingsInRange(ref this.fullTreeSightings, ticks);
			case TreeCategory.Super:
				return this.NumSightingsInRange(ref this.superTreeSightings, ticks);
			default:
				return 0;
			}
		}

		// Token: 0x0600CF5B RID: 53083 RVA: 0x004C5B66 File Offset: 0x004C3D66
		public int NumSkullspikeSightings()
		{
			return this.skullspikeSightings.Count;
		}

		// Token: 0x0600CF5C RID: 53084 RVA: 0x004C5B74 File Offset: 0x004C3D74
		public void SurroundingsTrackerTickInterval(int delta)
		{
			if (this.pawn.IsHashIntervalTick(2500, delta) && ModsConfig.IdeologyActive && this.pawn.Ideo != null && this.pawn.Ideo.cachedPossibleSituationalThoughts.Contains(ThoughtDefOf.TreesDesired) && this.pawn.Awake())
			{
				if (this.pawn.Spawned)
				{
					this.GetSpawnedTreeSightings();
				}
				else
				{
					this.GetCaravanTreeSightings();
				}
			}
			if (this.pawn.IsHashIntervalTick(60, delta) && ModsConfig.IdeologyActive && this.pawn.Ideo != null && this.pawn.Spawned && this.pawn.Awake() && (this.pawn.Ideo.cachedPossibleSituationalThoughts.Contains(ThoughtDefOf.Skullspike_Desired) || this.pawn.Ideo.cachedPossibleSituationalThoughts.Contains(ThoughtDefOf.Skullspike_Disapproved)) && !PawnUtility.IsBiologicallyOrArtificiallyBlind(this.pawn))
			{
				for (int i = this.skullspikeSightings.Count - 1; i >= 0; i--)
				{
					if (this.skullspikeSightings[i].TicksSinceSighting > 1800)
					{
						this.skullspikeSightings.RemoveAt(i);
					}
				}
				if (this.pawn.Map.listerThings.ThingsOfDef(ThingDefOf.Skullspike).Count != 0)
				{
					IntVec3 positionHeld = this.pawn.PositionHeld;
					int num = GenRadial.NumCellsInRadius(10f);
					for (int j = 0; j < num; j++)
					{
						IntVec3 c = positionHeld + GenRadial.RadialPattern[j];
						if (c.InBounds(this.pawn.Map) && !c.Fogged(this.pawn.Map))
						{
							Thing firstThing = c.GetFirstThing(this.pawn.Map, ThingDefOf.Skullspike);
							if (firstThing != null && GenSight.LineOfSight(this.pawn.PositionHeld, firstThing.PositionHeld, firstThing.Map, true, null, 0, 0))
							{
								bool flag = false;
								for (int k = 0; k < this.skullspikeSightings.Count; k++)
								{
									if (this.skullspikeSightings[k].skullspike == firstThing)
									{
										SkullspikeSighting value = this.skullspikeSightings[k];
										value.tickSighted = Find.TickManager.TicksGame;
										this.skullspikeSightings[k] = value;
										flag = true;
										break;
									}
								}
								if (!flag)
								{
									this.skullspikeSightings.Add(new SkullspikeSighting
									{
										skullspike = firstThing,
										tickSighted = Find.TickManager.TicksGame
									});
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600CF5D RID: 53085 RVA: 0x004C5E38 File Offset: 0x004C4038
		private void GetSpawnedTreeSightings()
		{
			foreach (TreeSighting treeSighting in IdeoUtility.TreeSightingsNearPawn(this.pawn.Position, this.pawn.Map, this.pawn.Ideo))
			{
				if (treeSighting.Tree != null)
				{
					TreeCategory treeCategory = treeSighting.Tree.def.plant.treeCategory;
				}
				switch (treeSighting.Tree.def.plant.treeCategory)
				{
				case TreeCategory.Mini:
					this.AddSighting(ref this.miniTreeSightings, treeSighting);
					break;
				case TreeCategory.Full:
					this.AddSighting(ref this.fullTreeSightings, treeSighting);
					break;
				case TreeCategory.Super:
					this.AddSighting(ref this.superTreeSightings, treeSighting);
					break;
				}
			}
		}

		// Token: 0x0600CF5E RID: 53086 RVA: 0x004C5F1C File Offset: 0x004C411C
		private void GetCaravanTreeSightings()
		{
			Caravan caravan = this.pawn.GetCaravan();
			if (caravan == null)
			{
				return;
			}
			int treeSightingsPerHourFromCaravan = caravan.Biome.TreeSightingsPerHourFromCaravan;
			for (int i = 0; i < treeSightingsPerHourFromCaravan; i++)
			{
				this.AddSighting(ref this.fullTreeSightings, new TreeSighting(null, Find.TickManager.TicksGame));
			}
		}

		// Token: 0x0600CF5F RID: 53087 RVA: 0x004C5F70 File Offset: 0x004C4170
		private void AddSighting(ref List<TreeSighting> list, TreeSighting newSighting)
		{
			if (newSighting.tree != null)
			{
				for (int i = list.Count - 1; i >= 0; i--)
				{
					if (list[i].tree != null && list[i].tree == newSighting.tree)
					{
						list.RemoveAt(i);
					}
				}
			}
			list.Add(newSighting);
			for (int j = 0; j < list.Count - 15; j++)
			{
				list.RemoveAt(0);
			}
		}

		// Token: 0x0600CF60 RID: 53088 RVA: 0x004C5FEA File Offset: 0x004C41EA
		public void Clear()
		{
			this.miniTreeSightings.Clear();
			this.fullTreeSightings.Clear();
			this.superTreeSightings.Clear();
		}

		// Token: 0x0600CF61 RID: 53089 RVA: 0x004C6010 File Offset: 0x004C4210
		public void ExposeData()
		{
			Scribe_Collections.Look<TreeSighting>(ref this.miniTreeSightings, "miniTreeSightings", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<TreeSighting>(ref this.fullTreeSightings, "fullTreeSightings", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<TreeSighting>(ref this.superTreeSightings, "superTreeSightings", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x040083E1 RID: 33761
		public Pawn pawn;

		// Token: 0x040083E2 RID: 33762
		public List<TreeSighting> miniTreeSightings = new List<TreeSighting>();

		// Token: 0x040083E3 RID: 33763
		public List<TreeSighting> fullTreeSightings = new List<TreeSighting>();

		// Token: 0x040083E4 RID: 33764
		public List<TreeSighting> superTreeSightings = new List<TreeSighting>();

		// Token: 0x040083E5 RID: 33765
		public List<SkullspikeSighting> skullspikeSightings = new List<SkullspikeSighting>();

		// Token: 0x040083E6 RID: 33766
		private const int MaxSightingsPerCategory = 15;

		// Token: 0x040083E7 RID: 33767
		private const int TreeCheckInterval = 2500;
	}
}
