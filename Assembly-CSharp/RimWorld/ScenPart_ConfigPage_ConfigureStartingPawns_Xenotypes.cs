using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020017A0 RID: 6048
	public class ScenPart_ConfigPage_ConfigureStartingPawns_Xenotypes : ScenPart_ConfigPage_ConfigureStartingPawnsBase
	{
		// Token: 0x170019A8 RID: 6568
		// (get) Token: 0x060096C2 RID: 38594 RVA: 0x0036ACFD File Offset: 0x00368EFD
		private float ElementHeight
		{
			get
			{
				return ScenPart.RowHeight * 4f;
			}
		}

		// Token: 0x170019A9 RID: 6569
		// (get) Token: 0x060096C3 RID: 38595 RVA: 0x0036AD0A File Offset: 0x00368F0A
		protected override int TotalPawnCount
		{
			get
			{
				return this.xenotypeCounts.Sum((XenotypeCount x) => x.count);
			}
		}

		// Token: 0x060096C4 RID: 38596 RVA: 0x0036AD38 File Offset: 0x00368F38
		public override string Summary(Scenario scen)
		{
			string result;
			if ((result = this.customSummary) == null)
			{
				result = "ScenPart_StartWithSpecificColonists".Translate((from x in this.xenotypeCounts
				select x.Summary).ToCommaList(true, false), this.pawnChoiceCount);
			}
			return result;
		}

		// Token: 0x060096C5 RID: 38597 RVA: 0x0036ADA0 File Offset: 0x00368FA0
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, this.ElementHeight * (float)this.xenotypeCounts.Count + ScenPart.RowHeight);
			scenPartRect.height = ScenPart.RowHeight;
			for (int i = 0; i < this.xenotypeCounts.Count; i++)
			{
				XenotypeCount xenotypeCount = this.xenotypeCounts[i];
				Widgets.TextFieldNumeric<int>(scenPartRect, ref xenotypeCount.count, ref xenotypeCount.countBuffer, 1f, 10f);
				scenPartRect.y += ScenPart.RowHeight;
				Rect rect = scenPartRect;
				rect.xMax -= ScenPart.RowHeight;
				if (Widgets.ButtonText(rect, xenotypeCount.xenotype.LabelCap, true, true, true, null))
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (XenotypeDef localXen2 in DefDatabase<XenotypeDef>.AllDefs)
					{
						XenotypeDef localXen = localXen2;
						list.Add(new FloatMenuOption(localXen.LabelCap, delegate()
						{
							xenotypeCount.xenotype = localXen;
						}, localXen.Icon, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
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
					this.xenotypeCounts.RemoveAt(i);
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
				Widgets.Checkbox(scenPartRect.xMin + vector.x + 4f, scenPartRect.y, ref xenotypeCount.requiredAtStart, 24f, false, false, null, null);
				scenPartRect.y += ScenPart.RowHeight * 2f;
			}
			if (Widgets.ButtonText(scenPartRect, "Add".Translate().CapitalizeFirst(), true, true, true, null))
			{
				this.xenotypeCounts.Add(new XenotypeCount
				{
					xenotype = XenotypeDefOf.Baseliner,
					count = 1
				});
				return;
			}
			scenPartRect.y += ScenPart.RowHeight;
		}

		// Token: 0x060096C6 RID: 38598 RVA: 0x0036B07C File Offset: 0x0036927C
		protected override ScenPart CopyForEditingInner()
		{
			ScenPart_ConfigPage_ConfigureStartingPawns_Xenotypes scenPart_ConfigPage_ConfigureStartingPawns_Xenotypes = new ScenPart_ConfigPage_ConfigureStartingPawns_Xenotypes
			{
				def = this.def,
				visible = this.visible,
				summarized = this.summarized,
				pawnChoiceCount = this.pawnChoiceCount,
				customSummary = this.customSummary
			};
			foreach (XenotypeCount xenotypeCount in this.xenotypeCounts)
			{
				scenPart_ConfigPage_ConfigureStartingPawns_Xenotypes.xenotypeCounts.Add(new XenotypeCount
				{
					xenotype = xenotypeCount.xenotype,
					count = xenotypeCount.count,
					allowedDevelopmentalStages = xenotypeCount.allowedDevelopmentalStages,
					description = xenotypeCount.description,
					requiredAtStart = xenotypeCount.requiredAtStart
				});
			}
			foreach (XenotypePawnKind xenotypePawnKind in this.overrideKinds)
			{
				scenPart_ConfigPage_ConfigureStartingPawns_Xenotypes.overrideKinds.Add(new XenotypePawnKind
				{
					xenotype = xenotypePawnKind.xenotype,
					pawnKind = xenotypePawnKind.pawnKind
				});
			}
			return scenPart_ConfigPage_ConfigureStartingPawns_Xenotypes;
		}

		// Token: 0x060096C7 RID: 38599 RVA: 0x0036B1C0 File Offset: 0x003693C0
		protected override void GenerateStartingPawns()
		{
			int num = 0;
			do
			{
				StartingPawnUtility.ClearAllStartingPawns();
				int num2 = 0;
				foreach (XenotypeCount xenotypeCount in this.xenotypeCounts)
				{
					for (int i = 0; i < xenotypeCount.count; i++)
					{
						PawnGenerationRequest generationRequest = StartingPawnUtility.GetGenerationRequest(num2);
						generationRequest.ForcedXenotype = xenotypeCount.xenotype;
						if (xenotypeCount.xenotype != null)
						{
							generationRequest.PawnKindDefGetter = delegate(XenotypeDef t)
							{
								foreach (XenotypePawnKind xenotypePawnKind in this.overrideKinds)
								{
									if (t == xenotypePawnKind.xenotype)
									{
										return xenotypePawnKind.pawnKind;
									}
								}
								return Find.GameInitData.startingPawnKind ?? Faction.OfPlayer.def.basicMemberKind;
							};
						}
						else
						{
							generationRequest.PawnKindDefGetter = null;
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

		// Token: 0x060096C8 RID: 38600 RVA: 0x0036B28C File Offset: 0x0036948C
		public override void PostIdeoChosen()
		{
			Find.GameInitData.startingPawnsRequired = null;
			Find.GameInitData.startingPawnKind = null;
			Find.GameInitData.startingXenotypesRequired = (from c in this.xenotypeCounts
			where c.requiredAtStart
			select c).ToList<XenotypeCount>();
			base.PostIdeoChosen();
		}

		// Token: 0x060096C9 RID: 38601 RVA: 0x0036B2F0 File Offset: 0x003694F0
		public override bool HasNullDefs()
		{
			if (base.HasNullDefs())
			{
				return true;
			}
			using (List<XenotypeCount>.Enumerator enumerator = this.xenotypeCounts.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.xenotype == null)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060096CA RID: 38602 RVA: 0x0036B354 File Offset: 0x00369554
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<XenotypeCount>(ref this.xenotypeCounts, "xenotypeCounts", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x060096CB RID: 38603 RVA: 0x0036B374 File Offset: 0x00369574
		public override int GetHashCode()
		{
			int num = base.GetHashCode();
			foreach (XenotypeCount xenotypeCount in this.xenotypeCounts)
			{
				num ^= xenotypeCount.GetHashCode();
			}
			return num;
		}

		// Token: 0x04005A63 RID: 23139
		public List<XenotypeCount> xenotypeCounts = new List<XenotypeCount>();

		// Token: 0x04005A64 RID: 23140
		public List<XenotypePawnKind> overrideKinds = new List<XenotypePawnKind>();

		// Token: 0x04005A65 RID: 23141
		[MustTranslate]
		public string customSummary;
	}
}
