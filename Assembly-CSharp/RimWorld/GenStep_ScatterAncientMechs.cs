using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020012BF RID: 4799
	public class GenStep_ScatterAncientMechs : GenStep_Scatterer
	{
		// Token: 0x1700143E RID: 5182
		// (get) Token: 0x06007716 RID: 30486 RVA: 0x0029FE39 File Offset: 0x0029E039
		private static IEnumerable<ThingDef> MechThingDefs
		{
			get
			{
				return new GenStep_ScatterAncientMechs.<get_MechThingDefs>d__4(-2);
			}
		}

		// Token: 0x1700143F RID: 5183
		// (get) Token: 0x06007717 RID: 30487 RVA: 0x0029FE42 File Offset: 0x0029E042
		public override int SeedPart
		{
			get
			{
				return 1034745625;
			}
		}

		// Token: 0x06007718 RID: 30488 RVA: 0x0029FE4C File Offset: 0x0029E04C
		protected override float GetPlacementFactor(Map map)
		{
			float num = 1f;
			foreach (TileMutatorDef tileMutatorDef in map.TileInfo.Mutators)
			{
				num *= tileMutatorDef.junkDensityFactor;
			}
			return num;
		}

		// Token: 0x06007719 RID: 30489 RVA: 0x0029FEA8 File Offset: 0x0029E0A8
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckIdeology("Scatter ancient outdoor building"))
			{
				return;
			}
			this.count = 1;
			this.allowInWaterBiome = false;
			this.thingToPlace = GenStep_ScatterAncientMechs.MechThingDefs.RandomElement<ThingDef>();
			base.Generate(map, parms);
		}

		// Token: 0x0600771A RID: 30490 RVA: 0x0029FEE0 File Offset: 0x0029E0E0
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (!base.CanScatterAt(c, map))
			{
				return false;
			}
			if (this.thingToPlace.rotatable)
			{
				int num = Rand.RangeInclusive(1, 4);
				for (int i = 0; i < 4; i++)
				{
					this.rotation = new Rot4((i + num) % 4);
					if (this.CanPlaceThingAt(c, this.rotation, map, this.thingToPlace))
					{
						return true;
					}
				}
			}
			this.rotation = this.thingToPlace.defaultPlacingRot;
			return this.CanPlaceThingAt(c, this.rotation, map, this.thingToPlace);
		}

		// Token: 0x0600771B RID: 30491 RVA: 0x0029FF67 File Offset: 0x0029E167
		private bool CanPlaceThingAt(IntVec3 c, Rot4 rot, Map map, ThingDef thingDef)
		{
			return ScatterDebrisUtility.CanPlaceThingAt(c, rot, map, thingDef);
		}

		// Token: 0x0600771C RID: 30492 RVA: 0x0029FF74 File Offset: 0x0029E174
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			Thing thing = GenSpawn.Spawn(ThingMaker.MakeThing(this.thingToPlace, null), loc, map, this.rotation, WipeMode.Vanish, false, false);
			ScatterDebrisUtility.ScatterFilthAroundThing(thing, map, ThingDefOf.Filth_MachineBits, 0.5f, 1, int.MaxValue, null);
			ScatterDebrisUtility.ScatterFilthAroundThing(thing, map, ThingDefOf.Filth_OilSmear, 0.2f, 0, int.MaxValue, null);
			if (thing.def == ThingDefOf.AncientWarwalkerTorso)
			{
				if (Rand.Chance(0.75f))
				{
					int num = ThingDefOf.AncientWarwalkerTorso.size.z / 2 + ThingDefOf.AncientWarwalkerLeg.size.z / 2 + Rand.Range(1, 3);
					IntVec3 intVec = thing.Position + new IntVec3(Rand.Range(-3, 3), 0, -num).RotatedBy(this.rotation);
					if (this.CanPlaceThingAt(intVec, this.rotation, map, ThingDefOf.AncientWarwalkerLeg))
					{
						ScatterDebrisUtility.ScatterFilthAroundThing(GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.AncientWarwalkerLeg, null), intVec, map, this.rotation, WipeMode.Vanish, false, false), map, ThingDefOf.Filth_MachineBits, 0.5f, 1, int.MaxValue, null);
					}
				}
				if (Rand.Chance(0.75f))
				{
					int newX = (ThingDefOf.AncientWarwalkerTorso.size.x / 2 + ThingDefOf.AncientWarwalkerClaw.size.x / 2 + Rand.Range(3, 5)) * (Rand.Bool ? -1 : 1);
					IntVec3 intVec2 = thing.Position + new IntVec3(newX, 0, Rand.Range(-1, 1)).RotatedBy(this.rotation);
					if (this.CanPlaceThingAt(intVec2, this.rotation, map, ThingDefOf.AncientWarwalkerClaw))
					{
						Thing thing2 = GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.AncientWarwalkerClaw, null), intVec2, map, this.rotation, WipeMode.Vanish, false, false);
						ScatterDebrisUtility.ScatterFilthAroundThing(thing2, map, ThingDefOf.Filth_MachineBits, 0.5f, 1, int.MaxValue, null);
						ScatterDebrisUtility.ScatterFilthAroundThing(thing2, map, ThingDefOf.Filth_OilSmear, 0.2f, 0, int.MaxValue, null);
					}
				}
			}
		}

		// Token: 0x04004ABE RID: 19134
		private const float MechClawChance = 0.75f;

		// Token: 0x04004ABF RID: 19135
		private const float MechLegChance = 0.75f;

		// Token: 0x04004AC0 RID: 19136
		private const float OilSmearChance = 0.2f;

		// Token: 0x04004AC1 RID: 19137
		private ThingDef thingToPlace;

		// Token: 0x04004AC2 RID: 19138
		private Rot4 rotation;
	}
}
