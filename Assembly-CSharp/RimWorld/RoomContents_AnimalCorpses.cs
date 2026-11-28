using System;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001242 RID: 4674
	public class RoomContents_AnimalCorpses : RoomContentsWorker
	{
		// Token: 0x060073F4 RID: 29684 RVA: 0x00289EE8 File Offset: 0x002880E8
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.FillRoom(map, room, faction, threatPoints);
			PawnKindDef kind = (from k in DefDatabase<PawnKindDef>.AllDefsListForReading
			where k.RaceProps.Animal && k.RaceProps.CanDoHerdMigration
			select k).RandomElementByWeight((PawnKindDef x) => Mathf.Lerp(0.2f, 1f, x.race.GetStatValueAbstract(StatDefOf.Wildness, null)));
			int randomInRange = RoomContents_AnimalCorpses.CorpseCountRange.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				RoomContents_AnimalCorpses.SpawnCorpse(map, room, kind);
			}
		}

		// Token: 0x060073F5 RID: 29685 RVA: 0x00289F74 File Offset: 0x00288174
		private static Corpse SpawnCorpse(Map map, LayoutRoom room, PawnKindDef kind)
		{
			IntVec3 loc;
			if (!room.TryGetRandomCellInRoom(map, out loc, 2, 0, null, false))
			{
				return null;
			}
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kind, null, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
			pawn.health.SetDead();
			Corpse corpse = pawn.MakeCorpse(null, null);
			corpse.Age = Mathf.RoundToInt(RoomContents_AnimalCorpses.CorpseAgeDaysRange.RandomInRange * 60000f);
			corpse.GetComp<CompRottable>().RotProgress += (float)corpse.Age;
			Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
			return (Corpse)GenSpawn.Spawn(corpse, loc, map, WipeMode.Vanish);
		}

		// Token: 0x0400490E RID: 18702
		private static readonly FloatRange CorpseAgeDaysRange = new FloatRange(3f, 30f);

		// Token: 0x0400490F RID: 18703
		private static readonly IntRange CorpseCountRange = new IntRange(1, 3);
	}
}
