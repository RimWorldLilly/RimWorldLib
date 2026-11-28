using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200202E RID: 8238
	public class GenStep_ScatterLayout : GenStep_Scatterer
	{
		// Token: 0x1700257E RID: 9598
		// (get) Token: 0x0600D0AD RID: 53421 RVA: 0x004CC336 File Offset: 0x004CA536
		public override int SeedPart
		{
			get
			{
				return 947153967;
			}
		}

		// Token: 0x0600D0AE RID: 53422 RVA: 0x004CC340 File Offset: 0x004CA540
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			for (int i = 0; i < this.layout.Count; i++)
			{
				Thing t = GenSpawn.Spawn(ThingMaker.MakeThing(this.layout[i].thing, null), loc + this.layout[i].offset, map, this.layout[i].rotation, WipeMode.Vanish, false, false);
				if (this.layout[i].filthDef != null)
				{
					foreach (IntVec3 c in t.OccupiedRect().ExpandedBy(this.layout[i].filthExpandBy))
					{
						if (Rand.Chance(this.layout[i].filthChance) && c.InBounds(map))
						{
							FilthMaker.TryMakeFilth(c, map, this.layout[i].filthDef, 1, FilthSourceFlags.None, true);
						}
					}
				}
			}
		}

		// Token: 0x0600D0AF RID: 53423 RVA: 0x004CC464 File Offset: 0x004CA664
		protected override bool CanScatterAt(IntVec3 loc, Map map)
		{
			if (!base.CanScatterAt(loc, map))
			{
				return false;
			}
			for (int i = 0; i < this.layout.Count; i++)
			{
				if (!this.CanSpawn(this.layout[i].thing, map, loc + this.layout[i].offset, this.layout[i].rotation))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600D0B0 RID: 53424 RVA: 0x004CC4D8 File Offset: 0x004CA6D8
		private bool CanSpawn(ThingDef def, Map map, IntVec3 cell, Rot4 rot)
		{
			CellRect cellRect = GenAdj.OccupiedRect(cell, rot, def.size);
			if (!cellRect.InBounds(map))
			{
				return false;
			}
			foreach (IntVec3 intVec in cellRect)
			{
				if (this.checkEveryLayoutCell && !base.CanScatterAt(intVec, map))
				{
					return false;
				}
				List<Thing> thingList = intVec.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					if (!thingList[i].def.destroyable || !thingList[i].def.canScatterOver)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x040084D3 RID: 34003
		public List<LayoutThing> layout;

		// Token: 0x040084D4 RID: 34004
		public bool checkEveryLayoutCell = true;
	}
}
