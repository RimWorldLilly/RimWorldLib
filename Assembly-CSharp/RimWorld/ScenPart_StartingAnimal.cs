using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020017B8 RID: 6072
	public class ScenPart_StartingAnimal : ScenPart
	{
		// Token: 0x06009762 RID: 38754 RVA: 0x0036EE05 File Offset: 0x0036D005
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PawnKindDef>(ref this.animalKind, "animalKind");
			Scribe_Values.Look<int>(ref this.count, "count", 0, false);
			Scribe_Values.Look<float>(ref this.bondToRandomPlayerPawnChance, "bondToRandomPlayerPawnChance", 0f, false);
		}

		// Token: 0x06009763 RID: 38755 RVA: 0x0036EE48 File Offset: 0x0036D048
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 2f + 1f);
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.Begin(scenPartRect.TopHalf());
			listing_Standard.ColumnWidth = scenPartRect.width;
			listing_Standard.TextFieldNumeric<int>(ref this.count, ref this.countBuf, 1f, 1E+09f);
			listing_Standard.End();
			if (Widgets.ButtonText(scenPartRect.BottomHalf(), this.CurrentAnimalLabel().CapitalizeFirst(), true, true, true, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				list.Add(new FloatMenuOption("RandomPet".Translate().CapitalizeFirst(), delegate()
				{
					this.animalKind = null;
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				foreach (PawnKindDef localKind2 in this.PossibleAnimals(false))
				{
					PawnKindDef localKind = localKind2;
					list.Add(new FloatMenuOption(localKind.LabelCap, delegate()
					{
						this.animalKind = localKind;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x06009764 RID: 38756 RVA: 0x0036EFB4 File Offset: 0x0036D1B4
		private IEnumerable<PawnKindDef> PossibleAnimals(bool checkForTamer = true)
		{
			return from td in DefDatabase<PawnKindDef>.AllDefs
			where td.RaceProps.Animal && (!checkForTamer || ScenPart_StartingAnimal.CanKeepPetTame(td))
			select td;
		}

		// Token: 0x06009765 RID: 38757 RVA: 0x0036EFE4 File Offset: 0x0036D1E4
		private static bool CanKeepPetTame(PawnKindDef def)
		{
			float level = (float)Find.GameInitData.startingAndOptionalPawns.Take(Find.GameInitData.startingPawnCount).MaxBy((Pawn c) => c.skills.GetSkill(SkillDefOf.Animals).Level).skills.GetSkill(SkillDefOf.Animals).Level;
			float statValueAbstract = def.race.GetStatValueAbstract(StatDefOf.MinimumHandlingSkill, null);
			return level >= statValueAbstract;
		}

		// Token: 0x06009766 RID: 38758 RVA: 0x0036F05C File Offset: 0x0036D25C
		private IEnumerable<PawnKindDef> RandomPets()
		{
			IEnumerable<PawnKindDef> enumerable = from td in this.PossibleAnimals(true)
			where td.RaceProps.petness > 0f
			select td;
			if (enumerable.EnumerableNullOrEmpty<PawnKindDef>())
			{
				enumerable = from td in this.PossibleAnimals(false)
				where td.RaceProps.petness > 0f
				select td;
			}
			return enumerable;
		}

		// Token: 0x06009767 RID: 38759 RVA: 0x0036F0CA File Offset: 0x0036D2CA
		private string CurrentAnimalLabel()
		{
			if (this.animalKind == null)
			{
				return "RandomPet".TranslateSimple();
			}
			return this.animalKind.label;
		}

		// Token: 0x06009768 RID: 38760 RVA: 0x0036F0EA File Offset: 0x0036D2EA
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "PlayerStartsWith", ScenPart_StartingThing_Defined.PlayerStartWithIntro);
		}

		// Token: 0x06009769 RID: 38761 RVA: 0x0036F0FC File Offset: 0x0036D2FC
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			ScenPart_StartingAnimal.<GetSummaryListEntries>d__13 <GetSummaryListEntries>d__ = new ScenPart_StartingAnimal.<GetSummaryListEntries>d__13(-2);
			<GetSummaryListEntries>d__.<>4__this = this;
			<GetSummaryListEntries>d__.<>3__tag = tag;
			return <GetSummaryListEntries>d__;
		}

		// Token: 0x0600976A RID: 38762 RVA: 0x0036F114 File Offset: 0x0036D314
		public override void Randomize()
		{
			if (Rand.Value < 0.5f)
			{
				this.animalKind = null;
			}
			else
			{
				this.animalKind = this.PossibleAnimals(false).RandomElement<PawnKindDef>();
			}
			this.count = ScenPart_StartingAnimal.PetCountChances.RandomElementByWeight((Pair<int, float> pa) => pa.Second).First;
			this.bondToRandomPlayerPawnChance = 0f;
		}

		// Token: 0x0600976B RID: 38763 RVA: 0x0036F18C File Offset: 0x0036D38C
		public override bool TryMerge(ScenPart other)
		{
			ScenPart_StartingAnimal scenPart_StartingAnimal = other as ScenPart_StartingAnimal;
			if (scenPart_StartingAnimal != null && scenPart_StartingAnimal.animalKind == this.animalKind)
			{
				this.count += scenPart_StartingAnimal.count;
				return true;
			}
			return false;
		}

		// Token: 0x0600976C RID: 38764 RVA: 0x0036F1C8 File Offset: 0x0036D3C8
		private float PetWeight(PawnKindDef animal)
		{
			FactionIdeosTracker ideos = Find.GameInitData.playerFaction.ideos;
			Ideo ideo = (ideos != null) ? ideos.PrimaryIdeo : null;
			if (ideo != null)
			{
				using (List<ThingDef>.Enumerator enumerator = ideo.VeneratedAnimals.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current == animal.race)
						{
							return 8f;
						}
					}
				}
			}
			return animal.RaceProps.petness;
		}

		// Token: 0x0600976D RID: 38765 RVA: 0x0036F250 File Offset: 0x0036D450
		public override IEnumerable<Thing> PlayerStartingThings()
		{
			ScenPart_StartingAnimal.<PlayerStartingThings>d__17 <PlayerStartingThings>d__ = new ScenPart_StartingAnimal.<PlayerStartingThings>d__17(-2);
			<PlayerStartingThings>d__.<>4__this = this;
			return <PlayerStartingThings>d__;
		}

		// Token: 0x0600976E RID: 38766 RVA: 0x0036F260 File Offset: 0x0036D460
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ ((this.animalKind != null) ? this.animalKind.GetHashCode() : 0) ^ this.count ^ this.bondToRandomPlayerPawnChance.GetHashCode();
		}

		// Token: 0x04005AB4 RID: 23220
		private PawnKindDef animalKind;

		// Token: 0x04005AB5 RID: 23221
		private int count = 1;

		// Token: 0x04005AB6 RID: 23222
		private float bondToRandomPlayerPawnChance = 0.5f;

		// Token: 0x04005AB7 RID: 23223
		private string countBuf;

		// Token: 0x04005AB8 RID: 23224
		private static readonly List<Pair<int, float>> PetCountChances = new List<Pair<int, float>>
		{
			new Pair<int, float>(1, 20f),
			new Pair<int, float>(2, 10f),
			new Pair<int, float>(3, 5f),
			new Pair<int, float>(4, 3f),
			new Pair<int, float>(5, 1f),
			new Pair<int, float>(6, 1f),
			new Pair<int, float>(7, 1f),
			new Pair<int, float>(8, 1f),
			new Pair<int, float>(9, 1f),
			new Pair<int, float>(10, 0.1f),
			new Pair<int, float>(11, 0.1f),
			new Pair<int, float>(12, 0.1f),
			new Pair<int, float>(13, 0.1f),
			new Pair<int, float>(14, 0.1f)
		};

		// Token: 0x04005AB9 RID: 23225
		public const float VeneratedAnimalWeight = 8f;
	}
}
