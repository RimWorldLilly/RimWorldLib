using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020017D1 RID: 6097
	public class TaleData_Pawn : TaleData
	{
		// Token: 0x06009822 RID: 38946 RVA: 0x00372764 File Offset: 0x00370964
		public override void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", true);
			Scribe_Defs.Look<PawnKindDef>(ref this.kind, "kind");
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
			Scribe_Values.Look<int>(ref this.chronologicalAge, "chronologicalAge", 0, false);
			Scribe_Values.Look<string>(ref this.relationInfo, "relationInfo", null, false);
			Scribe_Values.Look<bool>(ref this.everBeenColonistOrTameAnimal, "everBeenColonistOrTameAnimal", false, false);
			Scribe_Values.Look<bool>(ref this.everBeenQuestLodger, "everBeenQuestLodger", false, false);
			Scribe_Values.Look<bool>(ref this.isFactionLeader, "isFactionLeader", false, false);
			Scribe_Collections.Look<RoyalTitle>(ref this.royalTitles, "royalTitles", LookMode.Deep, Array.Empty<object>());
			Scribe_Deep.Look<Name>(ref this.name, "name", Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_Defs.Look<ThingDef>(ref this.primaryEquipment, "peq");
			Scribe_Defs.Look<ThingDef>(ref this.notableApparel, "app");
		}

		// Token: 0x06009823 RID: 38947 RVA: 0x00372880 File Offset: 0x00370A80
		public override IEnumerable<Rule> GetRules(string prefix, Dictionary<string, string> constants = null)
		{
			return GrammarUtility.RulesForPawn(prefix, this.name, this.title, this.kind, this.gender, this.faction, this.age, this.chronologicalAge, this.relationInfo, this.everBeenColonistOrTameAnimal, this.everBeenQuestLodger, this.isFactionLeader, this.royalTitles, false, string.Empty, constants, true);
		}

		// Token: 0x06009824 RID: 38948 RVA: 0x003728E4 File Offset: 0x00370AE4
		public static TaleData_Pawn GenerateFrom(Pawn pawn)
		{
			TaleData_Pawn taleData_Pawn = new TaleData_Pawn();
			taleData_Pawn.pawn = pawn;
			taleData_Pawn.kind = pawn.kindDef;
			taleData_Pawn.faction = pawn.Faction;
			taleData_Pawn.gender = (pawn.RaceProps.hasGenders ? pawn.gender : Gender.None);
			taleData_Pawn.age = pawn.ageTracker.AgeBiologicalYears;
			taleData_Pawn.chronologicalAge = pawn.ageTracker.AgeChronologicalYears;
			taleData_Pawn.everBeenColonistOrTameAnimal = PawnUtility.EverBeenColonistOrTameAnimal(pawn);
			taleData_Pawn.everBeenQuestLodger = PawnUtility.EverBeenQuestLodger(pawn);
			taleData_Pawn.isFactionLeader = (pawn.Faction != null && pawn.Faction.leader == pawn);
			if (pawn.royalty != null)
			{
				taleData_Pawn.royalTitles = new List<RoyalTitle>();
				foreach (RoyalTitle other in pawn.royalty.AllTitlesForReading)
				{
					taleData_Pawn.royalTitles.Add(new RoyalTitle(other));
				}
			}
			TaggedString taggedString = "";
			PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref taggedString, pawn);
			taleData_Pawn.relationInfo = taggedString.Resolve();
			if (pawn.story != null)
			{
				taleData_Pawn.title = pawn.story.title;
			}
			if (pawn.RaceProps.Humanlike)
			{
				taleData_Pawn.name = pawn.Name;
				if (pawn.equipment.Primary != null)
				{
					taleData_Pawn.primaryEquipment = pawn.equipment.Primary.def;
				}
				Apparel apparel;
				if (pawn.apparel.WornApparel.TryRandomElement(out apparel))
				{
					taleData_Pawn.notableApparel = apparel.def;
				}
			}
			return taleData_Pawn;
		}

		// Token: 0x06009825 RID: 38949 RVA: 0x00372A8C File Offset: 0x00370C8C
		public static TaleData_Pawn GenerateRandom(bool humanLike = false)
		{
			object obj;
			if (!humanLike)
			{
				obj = (from x in DefDatabase<PawnKindDef>.AllDefsListForReading
				where x.Isnt<CreepJoinerFormKindDef>()
				select x).RandomElement<PawnKindDef>();
			}
			else
			{
				obj = PawnKindDefOf.Drifter;
			}
			object obj2 = obj;
			Faction faction = FactionUtility.DefaultFactionFrom(obj2.defaultFactionDef);
			return TaleData_Pawn.GenerateFrom(PawnGenerator.GeneratePawn(obj2, faction, null));
		}

		// Token: 0x04005AFC RID: 23292
		public Pawn pawn;

		// Token: 0x04005AFD RID: 23293
		public PawnKindDef kind;

		// Token: 0x04005AFE RID: 23294
		public Faction faction;

		// Token: 0x04005AFF RID: 23295
		public Gender gender;

		// Token: 0x04005B00 RID: 23296
		public int age;

		// Token: 0x04005B01 RID: 23297
		public int chronologicalAge;

		// Token: 0x04005B02 RID: 23298
		public string relationInfo;

		// Token: 0x04005B03 RID: 23299
		public bool everBeenColonistOrTameAnimal;

		// Token: 0x04005B04 RID: 23300
		public bool everBeenQuestLodger;

		// Token: 0x04005B05 RID: 23301
		public bool isFactionLeader;

		// Token: 0x04005B06 RID: 23302
		public List<RoyalTitle> royalTitles;

		// Token: 0x04005B07 RID: 23303
		public Name name;

		// Token: 0x04005B08 RID: 23304
		public string title;

		// Token: 0x04005B09 RID: 23305
		public ThingDef primaryEquipment;

		// Token: 0x04005B0A RID: 23306
		public ThingDef notableApparel;

		// Token: 0x04005B0B RID: 23307
		private List<Faction> tmpFactions;

		// Token: 0x04005B0C RID: 23308
		private List<RoyalTitleDef> tmpRoyalTitles;
	}
}
