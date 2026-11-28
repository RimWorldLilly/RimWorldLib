using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001AF6 RID: 6902
	public class InsectAmbush : Thing
	{
		// Token: 0x0600B064 RID: 45156 RVA: 0x003E21C7 File Offset: 0x003E03C7
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.radius, "radius", 0f, false);
		}

		// Token: 0x0600B065 RID: 45157 RVA: 0x003E21E8 File Offset: 0x003E03E8
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.radius = InsectAmbush.RadiusRange.RandomInRange;
		}

		// Token: 0x0600B066 RID: 45158 RVA: 0x003E2210 File Offset: 0x003E0410
		protected override void Tick()
		{
			if (!this.IsHashIntervalTick(60))
			{
				return;
			}
			Map map = base.Map;
			int num = GenRadial.NumCellsInRadius(this.radius);
			for (int i = 0; i < num; i++)
			{
				IntVec3 c = base.Position + GenRadial.RadialPattern[i];
				if (c.InBounds(map))
				{
					foreach (Thing thing in c.GetThingList(map))
					{
						Pawn pawn = thing as Pawn;
						if (pawn != null && pawn.IsColonistPlayerControlled && GenSight.LineOfSightToThing(pawn.Position, this, base.Map, false, null))
						{
							this.Activate();
							return;
						}
					}
				}
			}
		}

		// Token: 0x0600B067 RID: 45159 RVA: 0x003E22E0 File Offset: 0x003E04E0
		private void Activate()
		{
			float points = InsectAmbush.AmbushThreatPointsCurve.Evaluate(StorytellerUtility.DefaultThreatPointsNow(Find.AnyPlayerHomeMap));
			List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
			{
				groupKind = PawnGroupKindDefOf.Combat,
				points = points,
				faction = Faction.OfInsects
			}, true).ToList<Pawn>();
			List<Thing> list2 = new List<Thing>();
			foreach (Pawn pawnToSpawn in list)
			{
				IntVec3 loc;
				if (CellFinder.TryFindRandomReachableNearbyCell(base.Position, base.Map, 5f, TraverseParms.For(TraverseMode.ByPawn, Danger.Deadly, false, false, false, true, false), (IntVec3 cell) => cell.Standable(base.Map), null, out loc, 999999))
				{
					PawnGroundSpawner pawnGroundSpawner = (PawnGroundSpawner)ThingMaker.MakeThing(ThingDefOf.PawnGroundSpawner, null);
					pawnGroundSpawner.Init(pawnToSpawn, InsectAmbush.TunnelDelayTicks);
					GenSpawn.Spawn(pawnGroundSpawner, loc, base.Map, WipeMode.Vanish);
					list2.Add(pawnGroundSpawner);
				}
			}
			Find.LetterStack.ReceiveLetter("InsectAmbushLetter".Translate(), "InsectAmbushLetterText".Translate(), LetterDefOf.ThreatBig, list2, null, null, null, null, 0, true);
			LordMaker.MakeNewLord(Faction.OfInsects, new LordJob_AssaultColony(), base.Map, list);
			this.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x040065D0 RID: 26064
		private static readonly IntRange TunnelDelayTicks = new IntRange(240, 480);

		// Token: 0x040065D1 RID: 26065
		private static readonly FloatRange RadiusRange = new FloatRange(5f, 8f);

		// Token: 0x040065D2 RID: 26066
		private static readonly SimpleCurve AmbushThreatPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(100f, 100f),
				true
			},
			{
				new CurvePoint(1000f, 300f),
				true
			},
			{
				new CurvePoint(5000f, 1500f),
				true
			}
		};

		// Token: 0x040065D3 RID: 26067
		private float radius;
	}
}
