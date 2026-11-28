using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld.QuestGen
{
	// Token: 0x02002271 RID: 8817
	public class QuestPart_DistressCallAmbush : QuestPart
	{
		// Token: 0x0600D9C8 RID: 55752 RVA: 0x0050CD59 File Offset: 0x0050AF59
		public QuestPart_DistressCallAmbush()
		{
		}

		// Token: 0x0600D9C9 RID: 55753 RVA: 0x0050CD61 File Offset: 0x0050AF61
		public QuestPart_DistressCallAmbush(string inSignal, Site site, float points)
		{
			this.inSignal = inSignal;
			this.site = site;
			this.points = points;
		}

		// Token: 0x0600D9CA RID: 55754 RVA: 0x0050CD80 File Offset: 0x0050AF80
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag != this.inSignal)
			{
				return;
			}
			List<Thing> list = (from b in this.site.Map.listerThings.ThingsOfDef(ThingDefOf.PitBurrow)
			where !b.Fogged()
			select b).ToList<Thing>();
			if (list.NullOrEmpty<Thing>())
			{
				return;
			}
			List<Pawn> source = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
			{
				groupKind = PawnGroupKindDefOf.Fleshbeasts,
				points = this.points,
				faction = Faction.OfEntities,
				raidStrategy = RaidStrategyDefOf.ImmediateAttack
			}, true).ToList<Pawn>();
			Lord lord = LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_FleshbeastAssault(), this.site.Map, null);
			Thing burrow = list.RandomElement<Thing>();
			Predicate<IntVec3> <>9__2;
			List<IntVec3> spawnPositions = source.Select(delegate(Pawn p)
			{
				IntVec3 position = burrow.Position;
				Map map = this.site.Map;
				int squareRadius = 2;
				Predicate<IntVec3> validator;
				if ((validator = <>9__2) == null)
				{
					validator = (<>9__2 = ((IntVec3 c) => c.Walkable(this.site.Map) && !c.Fogged(this.site.Map)));
				}
				IntVec3 result;
				if (!CellFinder.TryFindRandomCellNear(position, map, squareRadius, validator, out result, -1))
				{
					return burrow.Position;
				}
				return result;
			}).ToList<IntVec3>();
			SpawnRequest request = new SpawnRequest(source.Cast<Thing>().ToList<Thing>(), spawnPositions, 1, 0.5f, lord);
			this.site.Map.deferredSpawner.AddRequest(request, true);
			Find.LetterStack.ReceiveLetter("DistressSignalAmbushLabel".Translate(), "DistressSignalAmbushText".Translate(), LetterDefOf.ThreatBig, burrow, null, null, null, null, 0, true);
		}

		// Token: 0x0600D9CB RID: 55755 RVA: 0x0050CEE0 File Offset: 0x0050B0E0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Site>(ref this.site, "site", false);
			Scribe_Values.Look<float>(ref this.points, "points", 0f, false);
		}

		// Token: 0x04008B39 RID: 35641
		private string inSignal;

		// Token: 0x04008B3A RID: 35642
		private Site site;

		// Token: 0x04008B3B RID: 35643
		private float points;
	}
}
