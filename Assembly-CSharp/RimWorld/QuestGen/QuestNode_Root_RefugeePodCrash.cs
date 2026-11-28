using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200228C RID: 8844
	public class QuestNode_Root_RefugeePodCrash : QuestNode_Root_WandererJoin
	{
		// Token: 0x0600DA64 RID: 55908 RVA: 0x00512773 File Offset: 0x00510973
		public override Pawn GeneratePawn()
		{
			return this.GeneratePawn_NewTemp(null);
		}

		// Token: 0x0600DA65 RID: 55909 RVA: 0x0051277C File Offset: 0x0051097C
		public override Pawn GeneratePawn_NewTemp(Map map)
		{
			Pawn pawn = ThingUtility.FindPawn(ThingSetMakerDefOf.RefugeePod.root.Generate(new ThingSetMakerParams
			{
				tile = ((map != null) ? new PlanetTile?(map.Tile) : null)
			}));
			pawn.guest.Recruitable = true;
			return pawn;
		}

		// Token: 0x0600DA66 RID: 55910 RVA: 0x005127D2 File Offset: 0x005109D2
		[Obsolete]
		public override void SendLetter(Quest quest, Pawn pawn)
		{
			this.SendLetter_NewTemp(quest, pawn, Find.AnyPlayerHomeMap);
		}

		// Token: 0x0600DA67 RID: 55911 RVA: 0x005127E4 File Offset: 0x005109E4
		public override void SendLetter_NewTemp(Quest quest, Pawn pawn, Map map)
		{
			TaggedString label = "LetterLabelRefugeePodCrash".Translate();
			TaggedString taggedString = "RefugeePodCrash".Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
			taggedString += "\n\n";
			if (pawn.Faction == null)
			{
				taggedString += "RefugeePodCrash_Factionless".Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
			}
			else if (pawn.Faction.HostileTo(Faction.OfPlayer))
			{
				taggedString += "RefugeePodCrash_Hostile".Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
			}
			else
			{
				taggedString += "RefugeePodCrash_NonHostile".Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
			}
			if (pawn.DevelopmentalStage.Juvenile())
			{
				string arg = (pawn.ageTracker.AgeBiologicalYears * 3600000).ToStringTicksToPeriod(true, false, true, true, false);
				taggedString += "\n\n" + "RefugeePodCrash_Child".Translate(pawn.Named("PAWN"), arg.Named("AGE"));
			}
			QuestNode_Root_WandererJoin_WalkIn.AppendCharityInfoToLetter("JoinerCharityInfo".Translate(pawn), ref taggedString);
			PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref taggedString, ref label, pawn);
			Find.LetterStack.ReceiveLetter(label, taggedString, LetterDefOf.NeutralEvent, new TargetInfo(pawn), null, null, null, null, 0, true);
		}
	}
}
