using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012C1 RID: 4801
	public class GenStep_ScatterAncientTurret : GenStep_Scatterer
	{
		// Token: 0x17001440 RID: 5184
		// (get) Token: 0x06007723 RID: 30499 RVA: 0x002A0484 File Offset: 0x0029E684
		public override int SeedPart
		{
			get
			{
				return 344678634;
			}
		}

		// Token: 0x06007724 RID: 30500 RVA: 0x002A048B File Offset: 0x0029E68B
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckIdeology("Scatter ancient turret"))
			{
				return;
			}
			if (!Find.World.HasCaves(map.Tile))
			{
				return;
			}
			this.count = 1;
			this.warnOnFail = false;
			base.Generate(map, parms);
		}

		// Token: 0x06007725 RID: 30501 RVA: 0x002A04C4 File Offset: 0x0029E6C4
		protected override bool CanScatterAt(IntVec3 loc, Map map)
		{
			if (!base.CanScatterAt(loc, map) || !loc.InBounds(map))
			{
				return false;
			}
			if (loc.GetTerrain(map).IsWater)
			{
				return false;
			}
			RoofDef roof = loc.GetRoof(map);
			if (roof == null || !roof.isNatural)
			{
				return false;
			}
			if (loc.GetEdifice(map) != null)
			{
				return false;
			}
			if (!map.reachability.CanReachMapEdge(loc, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false, true, false)))
			{
				return false;
			}
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(loc, 2f, false))
			{
				if (intVec.InBounds(map) && !intVec.Roofed(map) && intVec.GetEdifice(map) == null && GenSight.LineOfSight(loc, intVec, map))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007726 RID: 30502 RVA: 0x002A05A0 File Offset: 0x0029E7A0
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			ScatterDebrisUtility.ScatterFilthAroundThing(GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.AncientSecurityTurret, null), loc, map, Rot4.North, WipeMode.Vanish, false, false), map, ThingDefOf.Filth_DriedBlood, 0.5f, 1, 3, null);
		}

		// Token: 0x04004AC3 RID: 19139
		private const int ShellScatterRadius = 10;

		// Token: 0x04004AC4 RID: 19140
		private static readonly IntRange MaxShellsRange = new IntRange(1, 2);
	}
}
