using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;

namespace Verse.Grammar
{
	// Token: 0x020007AC RID: 1964
	public static class GrammarUtility
	{
		// Token: 0x06003F51 RID: 16209 RVA: 0x001959A0 File Offset: 0x00193BA0
		public static IEnumerable<Rule> RulesForPawn(string pawnSymbol, Pawn pawn, Dictionary<string, string> constants = null, bool addRelationInfoSymbol = true, bool addTags = true)
		{
			if (pawn == null)
			{
				Log.ErrorOnce("Tried to insert rule " + pawnSymbol + " for null pawn", 16015097);
				return Enumerable.Empty<Rule>();
			}
			TaggedString taggedString = "";
			if (addRelationInfoSymbol)
			{
				PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref taggedString, pawn);
			}
			Name name = pawn.Name;
			Pawn_StoryTracker story = pawn.story;
			string title = (story != null) ? story.Title : null;
			PawnKindDef kindDef = pawn.kindDef;
			Gender gender = pawn.gender;
			Faction faction = pawn.Faction;
			int ageBiologicalYears = pawn.ageTracker.AgeBiologicalYears;
			int ageChronologicalYears = pawn.ageTracker.AgeChronologicalYears;
			string relationInfo = taggedString;
			bool everBeenColonistOrTameAnimal = PawnUtility.EverBeenColonistOrTameAnimal(pawn);
			bool everBeenQuestLodger = PawnUtility.EverBeenQuestLodger(pawn);
			bool isFactionLeader = pawn.Faction != null && pawn.Faction.leader == pawn;
			Pawn_RoyaltyTracker royalty = pawn.royalty;
			return GrammarUtility.RulesForPawn(pawnSymbol, name, title, kindDef, gender, faction, ageBiologicalYears, ageChronologicalYears, relationInfo, everBeenColonistOrTameAnimal, everBeenQuestLodger, isFactionLeader, (royalty != null) ? royalty.AllTitlesForReading : null, ModsConfig.AnomalyActive && pawn.health.hediffSet.HasHediff(HediffDefOf.CubeInterest, false), pawn.LabelNoParenthesis, constants, addTags);
		}

		// Token: 0x06003F52 RID: 16210 RVA: 0x00195A94 File Offset: 0x00193C94
		public static IEnumerable<Rule> RulesForPawn(string pawnSymbol, Name name, string title, PawnKindDef kind, Gender gender, Faction faction, int age, int chronologicalAge, string relationInfo, bool everBeenColonistOrTameAnimal, bool everBeenQuestLodger, bool isFactionLeader, List<RoyalTitle> royalTitles, bool cubeInterest, string labelNoParenthesis, Dictionary<string, string> constants = null, bool addTags = true)
		{
			GrammarUtility.<RulesForPawn>d__1 <RulesForPawn>d__ = new GrammarUtility.<RulesForPawn>d__1(-2);
			<RulesForPawn>d__.<>3__pawnSymbol = pawnSymbol;
			<RulesForPawn>d__.<>3__name = name;
			<RulesForPawn>d__.<>3__title = title;
			<RulesForPawn>d__.<>3__kind = kind;
			<RulesForPawn>d__.<>3__gender = gender;
			<RulesForPawn>d__.<>3__faction = faction;
			<RulesForPawn>d__.<>3__age = age;
			<RulesForPawn>d__.<>3__chronologicalAge = chronologicalAge;
			<RulesForPawn>d__.<>3__relationInfo = relationInfo;
			<RulesForPawn>d__.<>3__everBeenColonistOrTameAnimal = everBeenColonistOrTameAnimal;
			<RulesForPawn>d__.<>3__everBeenQuestLodger = everBeenQuestLodger;
			<RulesForPawn>d__.<>3__isFactionLeader = isFactionLeader;
			<RulesForPawn>d__.<>3__royalTitles = royalTitles;
			<RulesForPawn>d__.<>3__cubeInterest = cubeInterest;
			<RulesForPawn>d__.<>3__labelNoParenthesis = labelNoParenthesis;
			<RulesForPawn>d__.<>3__constants = constants;
			<RulesForPawn>d__.<>3__addTags = addTags;
			return <RulesForPawn>d__;
		}

		// Token: 0x06003F53 RID: 16211 RVA: 0x00195B2C File Offset: 0x00193D2C
		public static IEnumerable<Rule> RulesForThing(string prefix, Thing thing)
		{
			GrammarUtility.<RulesForThing>d__2 <RulesForThing>d__ = new GrammarUtility.<RulesForThing>d__2(-2);
			<RulesForThing>d__.<>3__prefix = prefix;
			<RulesForThing>d__.<>3__thing = thing;
			return <RulesForThing>d__;
		}

		// Token: 0x06003F54 RID: 16212 RVA: 0x00195B43 File Offset: 0x00193D43
		public static IEnumerable<Rule> RulesForDef(string prefix, Def def)
		{
			GrammarUtility.<RulesForDef>d__3 <RulesForDef>d__ = new GrammarUtility.<RulesForDef>d__3(-2);
			<RulesForDef>d__.<>3__prefix = prefix;
			<RulesForDef>d__.<>3__def = def;
			return <RulesForDef>d__;
		}

		// Token: 0x06003F55 RID: 16213 RVA: 0x00195B5A File Offset: 0x00193D5A
		public static IEnumerable<Rule> RulesForBodyPartRecord(string prefix, BodyPartRecord part)
		{
			GrammarUtility.<RulesForBodyPartRecord>d__4 <RulesForBodyPartRecord>d__ = new GrammarUtility.<RulesForBodyPartRecord>d__4(-2);
			<RulesForBodyPartRecord>d__.<>3__prefix = prefix;
			<RulesForBodyPartRecord>d__.<>3__part = part;
			return <RulesForBodyPartRecord>d__;
		}

		// Token: 0x06003F56 RID: 16214 RVA: 0x00195B71 File Offset: 0x00193D71
		public static IEnumerable<Rule> RulesForHediffDef(string prefix, HediffDef def, BodyPartRecord part)
		{
			GrammarUtility.<RulesForHediffDef>d__5 <RulesForHediffDef>d__ = new GrammarUtility.<RulesForHediffDef>d__5(-2);
			<RulesForHediffDef>d__.<>3__prefix = prefix;
			<RulesForHediffDef>d__.<>3__def = def;
			<RulesForHediffDef>d__.<>3__part = part;
			return <RulesForHediffDef>d__;
		}

		// Token: 0x06003F57 RID: 16215 RVA: 0x00195B8F File Offset: 0x00193D8F
		public static IEnumerable<Rule> RulesForFaction(string prefix, Faction faction, Dictionary<string, string> constants = null, bool addTags = true)
		{
			GrammarUtility.<RulesForFaction>d__6 <RulesForFaction>d__ = new GrammarUtility.<RulesForFaction>d__6(-2);
			<RulesForFaction>d__.<>3__prefix = prefix;
			<RulesForFaction>d__.<>3__faction = faction;
			<RulesForFaction>d__.<>3__constants = constants;
			<RulesForFaction>d__.<>3__addTags = addTags;
			return <RulesForFaction>d__;
		}

		// Token: 0x06003F58 RID: 16216 RVA: 0x00195BB4 File Offset: 0x00193DB4
		public static IEnumerable<Rule> RulesForWorldObject(string prefix, WorldObject worldObject, bool addTags = true)
		{
			GrammarUtility.<RulesForWorldObject>d__7 <RulesForWorldObject>d__ = new GrammarUtility.<RulesForWorldObject>d__7(-2);
			<RulesForWorldObject>d__.<>3__prefix = prefix;
			<RulesForWorldObject>d__.<>3__worldObject = worldObject;
			<RulesForWorldObject>d__.<>3__addTags = addTags;
			return <RulesForWorldObject>d__;
		}

		// Token: 0x06003F59 RID: 16217 RVA: 0x00195BD2 File Offset: 0x00193DD2
		public static IEnumerable<Rule> RulesForIdeo(string prefix, Ideo ideo)
		{
			GrammarUtility.<RulesForIdeo>d__8 <RulesForIdeo>d__ = new GrammarUtility.<RulesForIdeo>d__8(-2);
			<RulesForIdeo>d__.<>3__prefix = prefix;
			<RulesForIdeo>d__.<>3__ideo = ideo;
			return <RulesForIdeo>d__;
		}

		// Token: 0x06003F5A RID: 16218 RVA: 0x00195BE9 File Offset: 0x00193DE9
		public static IEnumerable<Rule> RulesForPrecept(string prefix, Precept precept)
		{
			GrammarUtility.<RulesForPrecept>d__9 <RulesForPrecept>d__ = new GrammarUtility.<RulesForPrecept>d__9(-2);
			<RulesForPrecept>d__.<>3__prefix = prefix;
			<RulesForPrecept>d__.<>3__precept = precept;
			return <RulesForPrecept>d__;
		}

		// Token: 0x06003F5B RID: 16219 RVA: 0x00195C00 File Offset: 0x00193E00
		[CompilerGenerated]
		internal static string <RulesForWorldObject>g__PossiblyWithTag|7_0(string str, ref GrammarUtility.<>c__DisplayClass7_0 A_1)
		{
			if (!(A_1.worldObject.Faction != null & A_1.addTags))
			{
				return str;
			}
			return str.ApplyTag(TagType.Settlement, A_1.worldObject.Faction.GetUniqueLoadID()).Resolve();
		}
	}
}
