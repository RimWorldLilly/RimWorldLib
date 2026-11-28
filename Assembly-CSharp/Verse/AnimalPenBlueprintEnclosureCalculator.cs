using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000231 RID: 561
	public class AnimalPenBlueprintEnclosureCalculator
	{
		// Token: 0x06001051 RID: 4177 RVA: 0x0005DB3E File Offset: 0x0005BD3E
		public AnimalPenBlueprintEnclosureCalculator()
		{
			this.passCheck = new Predicate<IntVec3>(this.PassCheck);
			this.cellProcessor = new Func<IntVec3, bool>(this.CellProcessor);
		}

		// Token: 0x06001052 RID: 4178 RVA: 0x0005DB78 File Offset: 0x0005BD78
		public void VisitPen(IntVec3 position, Map map)
		{
			int num = Gen.HashCombineInt(map.listerThings.StateHashOfGroup(ThingRequestGroup.Blueprint), map.listerThings.StateHashOfGroup(ThingRequestGroup.BuildingFrame), map.listerThings.StateHashOfGroup(ThingRequestGroup.BuildingArtificial), 42);
			if (this.map == null || this.map != map || !this.last_position.Equals(position) || this.last_stateHash != num)
			{
				this.map = map;
				this.last_position = position;
				this.last_stateHash = num;
				this.isEnclosed = true;
				this.cellsFound.Clear();
				this.FloodFill(position);
			}
		}

		// Token: 0x06001053 RID: 4179 RVA: 0x0005DC0F File Offset: 0x0005BE0F
		private void FloodFill(IntVec3 position)
		{
			this.map.floodFiller.FloodFill(position, this.passCheck, this.cellProcessor, int.MaxValue, false, null);
		}

		// Token: 0x06001054 RID: 4180 RVA: 0x0005DC35 File Offset: 0x0005BE35
		private bool CellProcessor(IntVec3 c)
		{
			this.cellsFound.Add(c);
			if (c.OnEdge(this.map))
			{
				this.isEnclosed = false;
				return true;
			}
			return false;
		}

		// Token: 0x06001055 RID: 4181 RVA: 0x0005DC5C File Offset: 0x0005BE5C
		private bool PassCheck(IntVec3 c)
		{
			if (!c.WalkableByFenceBlocked(this.map))
			{
				return false;
			}
			foreach (Thing thing in c.GetThingList(this.map))
			{
				ThingDef def = thing.def;
				if (def.passability == Traversability.Impassable)
				{
					return false;
				}
				Building_Door building_Door = thing as Building_Door;
				if (building_Door != null)
				{
					if (AnimalPenEnclosureCalculator.RoamerCanPass(building_Door))
					{
						return true;
					}
					return false;
				}
				else if (def.IsBlueprint || def.IsFrame)
				{
					ThingDef thingDef = def.entityDefToBuild as ThingDef;
					if (thingDef != null)
					{
						if (thingDef.IsFence || thingDef.passability == Traversability.Impassable)
						{
							return false;
						}
						if (thingDef.IsDoor)
						{
							if (AnimalPenEnclosureCalculator.RoamerCanPass(thingDef))
							{
								return true;
							}
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x04000F41 RID: 3905
		private readonly Predicate<IntVec3> passCheck;

		// Token: 0x04000F42 RID: 3906
		private readonly Func<IntVec3, bool> cellProcessor;

		// Token: 0x04000F43 RID: 3907
		public bool isEnclosed;

		// Token: 0x04000F44 RID: 3908
		public List<IntVec3> cellsFound = new List<IntVec3>();

		// Token: 0x04000F45 RID: 3909
		private Map map;

		// Token: 0x04000F46 RID: 3910
		private IntVec3 last_position;

		// Token: 0x04000F47 RID: 3911
		private int last_stateHash;
	}
}
