using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200115A RID: 4442
	public class IncidentParms : IExposable
	{
		// Token: 0x17001322 RID: 4898
		// (get) Token: 0x06006F93 RID: 28563 RVA: 0x002700E0 File Offset: 0x0026E2E0
		public int PawnGroupCount
		{
			get
			{
				Dictionary<Pawn, int> dictionary = this.pawnGroups;
				if (dictionary == null)
				{
					return 0;
				}
				return dictionary.Last<KeyValuePair<Pawn, int>>().Value + 1;
			}
		}

		// Token: 0x06006F94 RID: 28564 RVA: 0x00270108 File Offset: 0x0026E308
		public IncidentParms ShallowCopy()
		{
			return (IncidentParms)base.MemberwiseClone();
		}

		// Token: 0x06006F95 RID: 28565 RVA: 0x00270118 File Offset: 0x0026E318
		public void ExposeData()
		{
			Scribe_References.Look<IIncidentTarget>(ref this.target, "target", false);
			Scribe_Values.Look<float>(ref this.points, "threatPoints", 0f, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<bool>(ref this.forced, "forced", false, false);
			Scribe_Values.Look<string>(ref this.customLetterLabel, "customLetterLabel", null, false);
			Scribe_Values.Look<string>(ref this.customLetterText, "customLetterText", null, false);
			Scribe_Defs.Look<LetterDef>(ref this.customLetterDef, "customLetterDef");
			Scribe_Values.Look<bool>(ref this.sendLetter, "sendLetter", false, false);
			Scribe_Collections.Look<ThingDef>(ref this.letterHyperlinkThingDefs, "letterHyperlinkThingDefs", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<HediffDef>(ref this.letterHyperlinkHediffDefs, "letterHyperlinkHediffDefs", LookMode.Def, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.inSignalEnd, "inSignalEnd", null, false);
			Scribe_Values.Look<bool>(ref this.silent, "silent", false, false);
			if (Scribe.mode == LoadSaveMode.Saving && this.pawnGroups != null)
			{
				this.pawnGroups.RemoveAll((KeyValuePair<Pawn, int> x) => x.Key == null || x.Key.Destroyed);
			}
			Scribe_Values.Look<IntVec3>(ref this.spawnCenter, "spawnCenter", IntVec3.Invalid, false);
			Scribe_Values.Look<Rot4>(ref this.spawnRotation, "spawnRotation", default(Rot4), false);
			Scribe_Values.Look<bool>(ref this.generateFightersOnly, "generateFightersOnly", false, false);
			Scribe_Values.Look<bool>(ref this.dontUseSingleUseRocketLaunchers, "dontUseSingleUseRocketLaunchers", false, false);
			Scribe_Defs.Look<RaidStrategyDef>(ref this.raidStrategy, "raidStrategy");
			Scribe_Defs.Look<PawnsArrivalModeDef>(ref this.raidArrivalMode, "raidArrivalMode");
			Scribe_Values.Look<bool>(ref this.raidForceOneDowned, "raidForceOneDowned", false, false);
			Scribe_Values.Look<bool>(ref this.raidNeverFleeIndividual, "raidNeverFleeIndividual", false, false);
			Scribe_Values.Look<bool>(ref this.raidArrivalModeForQuickMilitaryAid, "raidArrivalModeForQuickMilitaryAid", false, false);
			Scribe_Defs.Look<RaidAgeRestrictionDef>(ref this.raidAgeRestriction, "raidAgeRestriction");
			Scribe_Values.Look<float>(ref this.biocodeWeaponsChance, "biocodeWeaponsChance", 0f, false);
			Scribe_Values.Look<float>(ref this.biocodeApparelChance, "biocodeApparelChance", 0f, false);
			Scribe_Collections.Look<Pawn, int>(ref this.pawnGroups, "pawnGroups", LookMode.Reference, LookMode.Value, ref this.tmpPawns, ref this.tmpGroups, true, false, false);
			Scribe_Values.Look<int?>(ref this.pawnGroupMakerSeed, "pawnGroupMakerSeed", null, false);
			Scribe_References.Look<Ideo>(ref this.pawnIdeo, "pawnIdeo", false);
			Scribe_References.Look<Lord>(ref this.lord, "lord", false);
			Scribe_Defs.Look<PawnKindDef>(ref this.pawnKind, "pawnKind");
			Scribe_Values.Look<int>(ref this.pawnCount, "pawnCount", 0, false);
			Scribe_Defs.Look<PawnGroupKindDef>(ref this.pawnGroupKind, "pawnGroupKind");
			Scribe_Defs.Look<TraderKindDef>(ref this.traderKind, "traderKind");
			Scribe_Values.Look<int>(ref this.podOpenDelay, "podOpenDelay", 140, false);
			Scribe_References.Look<Quest>(ref this.quest, "quest", false);
			Scribe_Defs.Look<QuestScriptDef>(ref this.questScriptDef, "questScriptDef");
			Scribe_Values.Look<string>(ref this.questTag, "questTag", null, false);
			Scribe_Deep.Look<MechClusterSketch>(ref this.mechClusterSketch, "mechClusterSketch", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.canTimeoutOrFlee, "canTimeoutOrFlee", true, false);
			Scribe_Values.Look<bool>(ref this.canSteal, "canSteal", true, false);
			Scribe_Values.Look<bool>(ref this.canKidnap, "canKidnap", true, false);
			Scribe_References.Look<Pawn>(ref this.controllerPawn, "controllerPawn", false);
			Scribe_Values.Look<IntVec3?>(ref this.infestationLocOverride, "infestationLocOverride", null, false);
			Scribe_Collections.Look<Thing>(ref this.attackTargets, "attackTargets", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.gifts, "gifts", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<float>(ref this.totalBodySize, "totalBodySize", 0f, false);
			Scribe_Defs.Look<PsychicRitualDef>(ref this.psychicRitualDef, "psychicRitualDef");
			Scribe_Values.Look<float>(ref this.pointMultiplier, "pointMultiplier", 1f, false);
			Scribe_Values.Look<bool>(ref this.bypassStorytellerSettings, "bypassStorytellerSettings", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				List<Thing> list = this.gifts;
				if (list != null)
				{
					list.RemoveAll((Thing x) => x == null);
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.attackTargets != null)
			{
				this.attackTargets.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x06006F96 RID: 28566 RVA: 0x00270574 File Offset: 0x0026E774
		public override string ToString()
		{
			string text = "";
			if (this.target != null)
			{
				string str = text;
				string str2 = "target=";
				IIncidentTarget incidentTarget = this.target;
				text = str + str2 + ((incidentTarget != null) ? incidentTarget.ToString() : null);
			}
			if (this.points >= 0f)
			{
				text = text + ", points=" + this.points.ToString();
			}
			if (this.faction != null)
			{
				text = string.Concat(new string[]
				{
					text,
					", faction=",
					this.faction.GetUniqueLoadID(),
					" (",
					this.faction.def.defName,
					")"
				});
			}
			if (this.generateFightersOnly)
			{
				text = text + ", generateFightersOnly=" + this.generateFightersOnly.ToString();
			}
			if (this.raidStrategy != null)
			{
				text = text + ", raidStrategy=" + this.raidStrategy.defName;
			}
			if (this.questScriptDef != null)
			{
				string str3 = text;
				string str4 = ", questScriptDef=";
				QuestScriptDef questScriptDef = this.questScriptDef;
				text = str3 + str4 + ((questScriptDef != null) ? questScriptDef.ToString() : null);
			}
			if (this.raidAgeRestriction != null)
			{
				string str5 = text;
				string str6 = ", raidAgeRestriction=";
				RaidAgeRestrictionDef raidAgeRestrictionDef = this.raidAgeRestriction;
				text = str5 + str6 + ((raidAgeRestrictionDef != null) ? raidAgeRestrictionDef.ToString() : null);
			}
			return text;
		}

		// Token: 0x0400473B RID: 18235
		public IIncidentTarget target;

		// Token: 0x0400473C RID: 18236
		public float points = -1f;

		// Token: 0x0400473D RID: 18237
		public Faction faction;

		// Token: 0x0400473E RID: 18238
		public bool forced;

		// Token: 0x0400473F RID: 18239
		public string customLetterLabel;

		// Token: 0x04004740 RID: 18240
		public string customLetterText;

		// Token: 0x04004741 RID: 18241
		public LetterDef customLetterDef;

		// Token: 0x04004742 RID: 18242
		public bool sendLetter = true;

		// Token: 0x04004743 RID: 18243
		public List<ThingDef> letterHyperlinkThingDefs;

		// Token: 0x04004744 RID: 18244
		public List<HediffDef> letterHyperlinkHediffDefs;

		// Token: 0x04004745 RID: 18245
		public string inSignalEnd;

		// Token: 0x04004746 RID: 18246
		public bool silent;

		// Token: 0x04004747 RID: 18247
		public IntVec3 spawnCenter = IntVec3.Invalid;

		// Token: 0x04004748 RID: 18248
		public Rot4 spawnRotation = Rot4.South;

		// Token: 0x04004749 RID: 18249
		public bool generateFightersOnly;

		// Token: 0x0400474A RID: 18250
		public bool dontUseSingleUseRocketLaunchers;

		// Token: 0x0400474B RID: 18251
		public RaidStrategyDef raidStrategy;

		// Token: 0x0400474C RID: 18252
		public PawnsArrivalModeDef raidArrivalMode;

		// Token: 0x0400474D RID: 18253
		[LoadAlias("raidForceOneIncap")]
		public bool raidForceOneDowned;

		// Token: 0x0400474E RID: 18254
		public bool raidNeverFleeIndividual;

		// Token: 0x0400474F RID: 18255
		public bool raidArrivalModeForQuickMilitaryAid;

		// Token: 0x04004750 RID: 18256
		public RaidAgeRestrictionDef raidAgeRestriction;

		// Token: 0x04004751 RID: 18257
		public float biocodeWeaponsChance;

		// Token: 0x04004752 RID: 18258
		public float biocodeApparelChance;

		// Token: 0x04004753 RID: 18259
		public Dictionary<Pawn, int> pawnGroups;

		// Token: 0x04004754 RID: 18260
		public int? pawnGroupMakerSeed;

		// Token: 0x04004755 RID: 18261
		public Ideo pawnIdeo;

		// Token: 0x04004756 RID: 18262
		public Lord lord;

		// Token: 0x04004757 RID: 18263
		public PawnKindDef pawnKind;

		// Token: 0x04004758 RID: 18264
		public int pawnCount;

		// Token: 0x04004759 RID: 18265
		public PawnGroupKindDef pawnGroupKind;

		// Token: 0x0400475A RID: 18266
		public TraderKindDef traderKind;

		// Token: 0x0400475B RID: 18267
		public int podOpenDelay = 140;

		// Token: 0x0400475C RID: 18268
		public Quest quest;

		// Token: 0x0400475D RID: 18269
		public QuestScriptDef questScriptDef;

		// Token: 0x0400475E RID: 18270
		public string questTag;

		// Token: 0x0400475F RID: 18271
		public MechClusterSketch mechClusterSketch;

		// Token: 0x04004760 RID: 18272
		public bool canTimeoutOrFlee = true;

		// Token: 0x04004761 RID: 18273
		public bool canSteal = true;

		// Token: 0x04004762 RID: 18274
		public bool canKidnap = true;

		// Token: 0x04004763 RID: 18275
		public Pawn controllerPawn;

		// Token: 0x04004764 RID: 18276
		public IntVec3? infestationLocOverride;

		// Token: 0x04004765 RID: 18277
		public List<Thing> attackTargets;

		// Token: 0x04004766 RID: 18278
		public List<Thing> gifts;

		// Token: 0x04004767 RID: 18279
		public float totalBodySize;

		// Token: 0x04004768 RID: 18280
		public PsychicRitualDef psychicRitualDef;

		// Token: 0x04004769 RID: 18281
		public float pointMultiplier = 1f;

		// Token: 0x0400476A RID: 18282
		public bool bypassStorytellerSettings;

		// Token: 0x0400476B RID: 18283
		public bool? canRoofPunch;

		// Token: 0x0400476C RID: 18284
		public int dropInRadius = -1;

		// Token: 0x0400476D RID: 18285
		public List<Pawn> storeGeneratedNeutralPawns;

		// Token: 0x0400476E RID: 18286
		private List<Pawn> tmpPawns;

		// Token: 0x0400476F RID: 18287
		private List<int> tmpGroups;
	}
}
