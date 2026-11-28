using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200129E RID: 4766
	public class GenStep_Dreadmeld : GenStep
	{
		// Token: 0x17001409 RID: 5129
		// (get) Token: 0x0600763B RID: 30267 RVA: 0x00299807 File Offset: 0x00297A07
		public override int SeedPart
		{
			get
			{
				return 21341517;
			}
		}

		// Token: 0x0600763C RID: 30268 RVA: 0x00299810 File Offset: 0x00297A10
		public override void Generate(Map map, GenStepParams parms)
		{
			PocketMapExit pitGateExit = (PocketMapExit)map.listerThings.ThingsOfDef(ThingDefOf.CaveExit).First<Thing>();
			IntVec3 intVec;
			CellFinder.TryFindRandomCell(map, (IntVec3 c) => c.Standable(map) && !c.InHorDistOf(pitGateExit.Position, 20f) && c.DistanceToEdge(map) > 10, out intVec);
			List<IntVec3> list = GridShapeMaker.IrregularLump(intVec, map, 100, null);
			List<IntVec3> list2 = GridShapeMaker.IrregularLump(intVec, map, 20, null);
			foreach (IntVec3 intVec2 in list)
			{
				GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Fleshmass, null), intVec2, map, Rot4.Random, WipeMode.Vanish, false, false).SetFaction(Faction.OfEntities, null);
				map.terrainGrid.SetTerrain(intVec2, TerrainDefOf.Flesh);
			}
			foreach (IntVec3 c2 in list2)
			{
				foreach (Thing thing in from t in c2.GetThingList(map).ToList<Thing>()
				where t.def.destroyable
				select t)
				{
					thing.Destroy(DestroyMode.Vanish);
				}
			}
			int randomInRange = GenStep_Dreadmeld.NumFleshBulbsRange.RandomInRange;
			Predicate<IntVec3> <>9__2;
			for (int i = 0; i < randomInRange; i++)
			{
				IntVec3 root = intVec;
				Map map2 = map;
				int squareRadius = 5;
				Predicate<IntVec3> validator;
				if ((validator = <>9__2) == null)
				{
					validator = (<>9__2 = ((IntVec3 c) => c.GetEdifice(map) == null));
				}
				IntVec3 loc;
				if (CellFinder.TryFindRandomCellNear(root, map2, squareRadius, validator, out loc, -1))
				{
					GenSpawn.Spawn(ThingDefOf.Fleshbulb, loc, map, WipeMode.Vanish).SetFaction(Faction.OfEntities, null);
				}
			}
			GenSpawn.Spawn(PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.Dreadmeld, Faction.OfEntities, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false)), intVec, map, Rot4.Random, WipeMode.Vanish, false, false);
			string signalTag = "dreadmeldApproached-" + Find.UniqueIDsManager.GetNextSignalTagID().ToString();
			CellRect rect = CellRect.FromCellList(list).ExpandedBy(2).ClipInsideMap(map);
			RectTrigger rectTrigger = (RectTrigger)ThingMaker.MakeThing(ThingDefOf.RectTrigger, null);
			rectTrigger.signalTag = signalTag;
			rectTrigger.Rect = rect;
			rectTrigger.destroyIfUnfogged = true;
			GenSpawn.Spawn(rectTrigger, rect.CenterCell, map, WipeMode.Vanish);
			SignalAction_Letter signalAction_Letter = (SignalAction_Letter)ThingMaker.MakeThing(ThingDefOf.SignalAction_Letter, null);
			signalAction_Letter.signalTag = signalTag;
			signalAction_Letter.letterDef = LetterDefOf.ThreatBig;
			signalAction_Letter.letterLabelKey = "LetterLabelDreadmeldWarning";
			signalAction_Letter.letterMessageKey = "LetterDreadmeldWarning";
			GenSpawn.Spawn(signalAction_Letter, rect.CenterCell, map, WipeMode.Vanish);
		}

		// Token: 0x04004A14 RID: 18964
		private const int HiveSize = 100;

		// Token: 0x04004A15 RID: 18965
		private const int HiveInteriorSize = 20;

		// Token: 0x04004A16 RID: 18966
		private const float MinSpawnDistFromGateExit = 20f;

		// Token: 0x04004A17 RID: 18967
		private static readonly IntRange NumFleshBulbsRange = new IntRange(2, 4);
	}
}
