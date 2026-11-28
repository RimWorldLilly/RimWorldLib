using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020017A2 RID: 6050
	public class ScenPart_ConfigPage_ConfigureStartingPawns_Mutants : ScenPart_ConfigPage_ConfigureStartingPawnsBase
	{
		// Token: 0x170019AB RID: 6571
		// (get) Token: 0x060096D2 RID: 38610 RVA: 0x0036B589 File Offset: 0x00369789
		private float ElementHeight
		{
			get
			{
				return ScenPart.RowHeight * 4f;
			}
		}

		// Token: 0x170019AC RID: 6572
		// (get) Token: 0x060096D3 RID: 38611 RVA: 0x0036B596 File Offset: 0x00369796
		protected override int TotalPawnCount
		{
			get
			{
				return this.mutantCounts.Sum((MutantCount x) => x.count);
			}
		}

		// Token: 0x060096D4 RID: 38612 RVA: 0x0036B5C4 File Offset: 0x003697C4
		public override string Summary(Scenario scen)
		{
			string result;
			if ((result = this.customSummary) == null)
			{
				result = "ScenPart_StartWithSpecificColonists".Translate((from x in this.mutantCounts
				select x.Summary).ToCommaList(true, false), this.pawnChoiceCount);
			}
			return result;
		}

		// Token: 0x060096D5 RID: 38613 RVA: 0x0036B62C File Offset: 0x0036982C
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, this.ElementHeight * (float)this.mutantCounts.Count + ScenPart.RowHeight);
			scenPartRect.height = ScenPart.RowHeight;
			for (int i = 0; i < this.mutantCounts.Count; i++)
			{
				MutantCount mutantCount = this.mutantCounts[i];
				Widgets.TextFieldNumeric<int>(scenPartRect, ref mutantCount.count, ref mutantCount.countBuffer, 1f, 10f);
				scenPartRect.y += ScenPart.RowHeight;
				Rect rect = scenPartRect;
				rect.xMax -= ScenPart.RowHeight;
				Rect rect2 = rect;
				MutantDef mutant = mutantCount.mutant;
				if (Widgets.ButtonText(rect2, (mutant != null) ? mutant.LabelCap : "None".Translate(), true, true, true, null))
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					list.Add(new FloatMenuOption("None".Translate(), delegate()
					{
						mutantCount.mutant = null;
					}, null, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
					foreach (MutantDef localMut2 in DefDatabase<MutantDef>.AllDefs)
					{
						MutantDef localMut = localMut2;
						if (localMut.showInScenarioEditor)
						{
							list.Add(new FloatMenuOption(localMut.LabelCap, delegate()
							{
								mutantCount.mutant = localMut;
							}, null, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
						}
					}
					if (list.Any<FloatMenuOption>())
					{
						Find.WindowStack.Add(new FloatMenu(list));
					}
				}
				Rect butRect = scenPartRect;
				butRect.xMin = rect.xMax;
				if (Widgets.ButtonImage(butRect, TexButton.Delete, true, null))
				{
					this.mutantCounts.RemoveAt(i);
					return;
				}
				scenPartRect.y += ScenPart.RowHeight;
				Rect rect3 = scenPartRect;
				TaggedString taggedString = "Required".Translate();
				Vector2 vector = Text.CalcSize(taggedString);
				rect3.width = vector.x;
				Text.Anchor = TextAnchor.MiddleRight;
				Widgets.Label(rect3, taggedString);
				Text.Anchor = TextAnchor.UpperLeft;
				Widgets.Checkbox(scenPartRect.xMin + vector.x + 4f, scenPartRect.y, ref mutantCount.requiredAtStart, 24f, false, false, null, null);
				scenPartRect.y += ScenPart.RowHeight * 2f;
			}
			if (Widgets.ButtonText(scenPartRect, "Add".Translate().CapitalizeFirst(), true, true, true, null))
			{
				this.mutantCounts.Add(new MutantCount
				{
					mutant = null,
					count = 1
				});
				return;
			}
			scenPartRect.y += ScenPart.RowHeight;
		}

		// Token: 0x060096D6 RID: 38614 RVA: 0x0036B954 File Offset: 0x00369B54
		protected override ScenPart CopyForEditingInner()
		{
			ScenPart_ConfigPage_ConfigureStartingPawns_Mutants scenPart_ConfigPage_ConfigureStartingPawns_Mutants = new ScenPart_ConfigPage_ConfigureStartingPawns_Mutants
			{
				def = this.def,
				visible = this.visible,
				summarized = this.summarized,
				pawnChoiceCount = this.pawnChoiceCount,
				customSummary = this.customSummary
			};
			foreach (MutantCount mutantCount in this.mutantCounts)
			{
				scenPart_ConfigPage_ConfigureStartingPawns_Mutants.mutantCounts.Add(new MutantCount
				{
					mutant = mutantCount.mutant,
					count = mutantCount.count,
					allowedDevelopmentalStages = mutantCount.allowedDevelopmentalStages,
					description = mutantCount.description,
					requiredAtStart = mutantCount.requiredAtStart
				});
			}
			return scenPart_ConfigPage_ConfigureStartingPawns_Mutants;
		}

		// Token: 0x060096D7 RID: 38615 RVA: 0x0036BA30 File Offset: 0x00369C30
		protected override void GenerateStartingPawns()
		{
			int num = 0;
			do
			{
				StartingPawnUtility.ClearAllStartingPawns();
				int num2 = 0;
				foreach (MutantCount mutantCount in this.mutantCounts)
				{
					for (int i = 0; i < mutantCount.count; i++)
					{
						PawnGenerationRequest generationRequest = StartingPawnUtility.GetGenerationRequest(num2);
						generationRequest.ForcedMutant = mutantCount.mutant;
						StartingPawnUtility.SetGenerationRequest(num2, generationRequest);
						StartingPawnUtility.AddNewPawn(num2);
						num2++;
					}
				}
				num++;
			}
			while (num <= 20 && !StartingPawnUtility.WorkTypeRequirementsSatisfied());
		}

		// Token: 0x060096D8 RID: 38616 RVA: 0x0036BAD4 File Offset: 0x00369CD4
		public override void PostIdeoChosen()
		{
			Find.GameInitData.startingPawnsRequired = null;
			Find.GameInitData.startingPawnKind = null;
			Find.GameInitData.startingMutantsRequired = (from c in this.mutantCounts
			where c.requiredAtStart
			select c).ToList<MutantCount>();
			base.PostIdeoChosen();
		}

		// Token: 0x060096D9 RID: 38617 RVA: 0x0036BB36 File Offset: 0x00369D36
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<MutantCount>(ref this.mutantCounts, "mutantCounts", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x060096DA RID: 38618 RVA: 0x0036BB54 File Offset: 0x00369D54
		public override int GetHashCode()
		{
			int num = base.GetHashCode();
			foreach (MutantCount mutantCount in this.mutantCounts)
			{
				num ^= mutantCount.GetHashCode();
			}
			return num;
		}

		// Token: 0x04005A69 RID: 23145
		public List<MutantCount> mutantCounts = new List<MutantCount>();

		// Token: 0x04005A6A RID: 23146
		[MustTranslate]
		public string customSummary;
	}
}
