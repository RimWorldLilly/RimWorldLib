using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200179D RID: 6045
	public class ScenPart_ConfigPage_ConfigureStartingPawns_KindDefs : ScenPart_ConfigPage_ConfigureStartingPawnsBase
	{
		// Token: 0x170019A4 RID: 6564
		// (get) Token: 0x060096B1 RID: 38577 RVA: 0x0036A57C File Offset: 0x0036877C
		private float ElementHeight
		{
			get
			{
				return ScenPart.RowHeight * 4f;
			}
		}

		// Token: 0x170019A5 RID: 6565
		// (get) Token: 0x060096B2 RID: 38578 RVA: 0x0036A589 File Offset: 0x00368789
		private IEnumerable<PawnKindDef> AvailableKinds
		{
			get
			{
				return from x in DefDatabase<PawnKindDef>.AllDefs
				where x.RaceProps.Humanlike && x.defaultFactionDef != null && x.defaultFactionDef.isPlayer
				select x;
			}
		}

		// Token: 0x170019A6 RID: 6566
		// (get) Token: 0x060096B3 RID: 38579 RVA: 0x0036A5B4 File Offset: 0x003687B4
		protected override int TotalPawnCount
		{
			get
			{
				return this.kindCounts.Sum((PawnKindCount x) => x.count);
			}
		}

		// Token: 0x060096B4 RID: 38580 RVA: 0x0036A5E0 File Offset: 0x003687E0
		public override string Summary(Scenario scen)
		{
			return "ScenPart_StartWithSpecificColonists".Translate((from x in this.kindCounts
			select x.Summary).ToCommaList(true, false), this.pawnChoiceCount);
		}

		// Token: 0x060096B5 RID: 38581 RVA: 0x0036A640 File Offset: 0x00368840
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, this.ElementHeight * (float)this.kindCounts.Count + ScenPart.RowHeight);
			scenPartRect.height = ScenPart.RowHeight;
			for (int i = 0; i < this.kindCounts.Count; i++)
			{
				PawnKindCount kindCount = this.kindCounts[i];
				Widgets.TextFieldNumeric<int>(scenPartRect, ref kindCount.count, ref kindCount.countBuffer, 1f, 10f);
				scenPartRect.y += ScenPart.RowHeight;
				Rect rect = scenPartRect;
				rect.xMax -= ScenPart.RowHeight;
				if (Widgets.ButtonText(rect, kindCount.kindDef.LabelCap, true, true, true, null))
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (PawnKindDef localKind2 in this.AvailableKinds)
					{
						PawnKindDef localKind = localKind2;
						list.Add(new FloatMenuOption(localKind.LabelCap, delegate()
						{
							kindCount.kindDef = localKind;
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
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
					this.kindCounts.RemoveAt(i);
					return;
				}
				scenPartRect.y += ScenPart.RowHeight;
				Rect rect2 = scenPartRect;
				TaggedString taggedString = "Required".Translate();
				Vector2 vector = Text.CalcSize(taggedString);
				rect2.width = vector.x;
				Text.Anchor = TextAnchor.MiddleRight;
				Widgets.Label(rect2, taggedString);
				Text.Anchor = TextAnchor.UpperLeft;
				Widgets.Checkbox(scenPartRect.xMin + vector.x + 4f, scenPartRect.y, ref kindCount.requiredAtStart, 24f, false, false, null, null);
				scenPartRect.y += ScenPart.RowHeight * 2f;
			}
			if (Widgets.ButtonText(scenPartRect, "Add".Translate().CapitalizeFirst(), true, true, true, null))
			{
				this.kindCounts.Add(new PawnKindCount
				{
					kindDef = PawnKindDefOf.Colonist,
					count = 1
				});
				return;
			}
			scenPartRect.y += ScenPart.RowHeight;
		}

		// Token: 0x060096B6 RID: 38582 RVA: 0x0036A90C File Offset: 0x00368B0C
		protected override ScenPart CopyForEditingInner()
		{
			ScenPart_ConfigPage_ConfigureStartingPawns_KindDefs scenPart_ConfigPage_ConfigureStartingPawns_KindDefs = new ScenPart_ConfigPage_ConfigureStartingPawns_KindDefs
			{
				def = this.def,
				visible = this.visible,
				summarized = this.summarized,
				pawnChoiceCount = this.pawnChoiceCount,
				leftBehindPawnKind = this.leftBehindPawnKind
			};
			foreach (PawnKindCount pawnKindCount in this.kindCounts)
			{
				scenPart_ConfigPage_ConfigureStartingPawns_KindDefs.kindCounts.Add(new PawnKindCount
				{
					kindDef = pawnKindCount.kindDef,
					count = pawnKindCount.count,
					requiredAtStart = pawnKindCount.requiredAtStart
				});
			}
			return scenPart_ConfigPage_ConfigureStartingPawns_KindDefs;
		}

		// Token: 0x060096B7 RID: 38583 RVA: 0x0036A9D0 File Offset: 0x00368BD0
		protected override void GenerateStartingPawns()
		{
			int num = 0;
			do
			{
				StartingPawnUtility.ClearAllStartingPawns();
				int num2 = 0;
				foreach (PawnKindCount pawnKindCount in this.kindCounts)
				{
					for (int i = 0; i < pawnKindCount.count; i++)
					{
						PawnGenerationRequest generationRequest = StartingPawnUtility.GetGenerationRequest(num2);
						generationRequest.KindDef = pawnKindCount.kindDef;
						if (ModsConfig.BiotechActive)
						{
							if (pawnKindCount.kindDef.xenotypeSet != null)
							{
								generationRequest.ForcedXenotype = null;
							}
							else
							{
								generationRequest.ForcedXenotype = XenotypeDefOf.Baseliner;
							}
						}
						StartingPawnUtility.SetGenerationRequest(num2, generationRequest);
						StartingPawnUtility.AddNewPawn(num2);
						num2++;
					}
				}
				num++;
			}
			while (num <= 20 && !StartingPawnUtility.WorkTypeRequirementsSatisfied());
		}

		// Token: 0x060096B8 RID: 38584 RVA: 0x0036AAA0 File Offset: 0x00368CA0
		public override void PostIdeoChosen()
		{
			Find.GameInitData.startingPawnKind = this.leftBehindPawnKind;
			Find.GameInitData.startingPawnsRequired = (from c in this.kindCounts
			where c.requiredAtStart
			select c).ToList<PawnKindCount>();
			Find.GameInitData.startingXenotypesRequired = null;
			base.PostIdeoChosen();
		}

		// Token: 0x060096B9 RID: 38585 RVA: 0x0036AB08 File Offset: 0x00368D08
		public override bool HasNullDefs()
		{
			if (base.HasNullDefs())
			{
				return true;
			}
			using (List<PawnKindCount>.Enumerator enumerator = this.kindCounts.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.kindDef == null)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060096BA RID: 38586 RVA: 0x0036AB6C File Offset: 0x00368D6C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<PawnKindCount>(ref this.kindCounts, "kindCounts", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x060096BB RID: 38587 RVA: 0x0036AB8C File Offset: 0x00368D8C
		public override int GetHashCode()
		{
			int num = base.GetHashCode();
			foreach (PawnKindCount pawnKindCount in this.kindCounts)
			{
				num ^= pawnKindCount.GetHashCode();
			}
			return num;
		}

		// Token: 0x04005A5C RID: 23132
		public List<PawnKindCount> kindCounts = new List<PawnKindCount>();

		// Token: 0x04005A5D RID: 23133
		private PawnKindDef leftBehindPawnKind;
	}
}
