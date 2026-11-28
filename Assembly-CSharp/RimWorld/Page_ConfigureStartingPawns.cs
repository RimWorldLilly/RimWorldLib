using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001D01 RID: 7425
	public class Page_ConfigureStartingPawns : Page
	{
		// Token: 0x170022DB RID: 8923
		// (get) Token: 0x0600BFB1 RID: 49073 RVA: 0x0045A3FC File Offset: 0x004585FC
		public override string PageTitle
		{
			get
			{
				return "CreateCharacters".Translate();
			}
		}

		// Token: 0x170022DC RID: 8924
		// (get) Token: 0x0600BFB2 RID: 49074 RVA: 0x0045A410 File Offset: 0x00458610
		private bool StartingPawnsAllBabies
		{
			get
			{
				List<Pawn> startingAndOptionalPawns = Find.GameInitData.startingAndOptionalPawns;
				int num = 0;
				for (int i = 0; i < Find.GameInitData.startingPawnCount; i++)
				{
					if (startingAndOptionalPawns[i].DevelopmentalStage.Baby())
					{
						num++;
					}
				}
				return num >= Find.GameInitData.startingPawnCount;
			}
		}

		// Token: 0x170022DD RID: 8925
		// (get) Token: 0x0600BFB3 RID: 49075 RVA: 0x0045A468 File Offset: 0x00458668
		private AcceptanceReport ExtraCanDoNextReport
		{
			get
			{
				if (ModsConfig.BiotechActive && this.StartingPawnsAllBabies)
				{
					return "ChooseChildOrAdult".Translate();
				}
				IEnumerable<Pawn> source = Find.GameInitData.startingAndOptionalPawns.Take(Find.GameInitData.startingPawnCount);
				if (source.Any((Pawn p) => (p.DevelopmentalStage & Find.GameInitData.allowedDevelopmentalStages) == DevelopmentalStage.None))
				{
					return "SelectedCharactersMustBeOfAllowedDevelopmentalStages".Translate(Find.GameInitData.allowedDevelopmentalStages.ToCommaListOr());
				}
				if (!Find.GameInitData.startingSkillsRequired.NullOrEmpty<SkillDef>())
				{
					using (List<SkillDef>.Enumerator enumerator = Find.GameInitData.startingSkillsRequired.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							SkillDef skill = enumerator.Current;
							if (!source.Any((Pawn p) => !p.skills.GetSkill(skill).TotallyDisabled))
							{
								return "SelectedCharacterMustBeCapableOf".Translate(skill.skillLabel);
							}
						}
					}
				}
				if (!Find.GameInitData.startingPawnsRequired.NullOrEmpty<PawnKindCount>())
				{
					for (int i = 0; i < Find.GameInitData.startingPawnsRequired.Count; i++)
					{
						PawnKindCount required = Find.GameInitData.startingPawnsRequired[i];
						int num = source.Count((Pawn p) => p.kindDef == required.kindDef);
						if (required.count > num)
						{
							if (required.count <= 1 || required.kindDef.labelPlural.NullOrEmpty())
							{
								string label = required.kindDef.label;
							}
							else
							{
								string labelPlural = required.kindDef.labelPlural;
							}
							return "SelectedCharactersMustInclude".Translate(required.Summary.Named("SUMMARY"));
						}
					}
				}
				if (!Find.GameInitData.startingXenotypesRequired.NullOrEmpty<XenotypeCount>())
				{
					for (int j = 0; j < Find.GameInitData.startingXenotypesRequired.Count; j++)
					{
						XenotypeCount required = Find.GameInitData.startingXenotypesRequired[j];
						if (source.Count((Pawn p) => p.genes.Xenotype == required.xenotype && required.allowedDevelopmentalStages.Has(p.DevelopmentalStage)) != required.count)
						{
							return "SelectedCharactersMustInclude".Translate(required.Summary.Named("SUMMARY"));
						}
					}
				}
				if (!Find.GameInitData.startingMutantsRequired.NullOrEmpty<MutantCount>())
				{
					for (int k = 0; k < Find.GameInitData.startingMutantsRequired.Count; k++)
					{
						MutantCount required = Find.GameInitData.startingMutantsRequired[k];
						if (source.Count((Pawn p) => p.IsMutant && p.mutant.Def == required.mutant && required.allowedDevelopmentalStages.Has(p.DevelopmentalStage)) != required.count)
						{
							return "SelectedCharactersMustInclude".Translate(required.Summary.Named("SUMMARY"));
						}
					}
				}
				return true;
			}
		}

		// Token: 0x0600BFB4 RID: 49076 RVA: 0x0045A7B8 File Offset: 0x004589B8
		public override void PreOpen()
		{
			base.PreOpen();
			if (Find.GameInitData.startingAndOptionalPawns.Count > 0)
			{
				this.curPawnIndex = 0;
			}
			this.renderClothes = true;
			this.renderHeadgear = false;
		}

		// Token: 0x0600BFB5 RID: 49077 RVA: 0x0045A7E7 File Offset: 0x004589E7
		public override void PostOpen()
		{
			base.PostOpen();
			TutorSystem.Notify_Event("PageStart-ConfigureStartingPawns");
		}

		// Token: 0x0600BFB6 RID: 49078 RVA: 0x0045A800 File Offset: 0x00458A00
		public override void DoWindowContents(Rect rect)
		{
			base.DrawPageTitle(rect);
			this.DrawApparelOptions(rect);
			rect.yMin += 45f;
			base.DoBottomButtons(rect, "Start".Translate(), null, null, true, false);
			this.DrawXenotypeEditorButton(rect);
			AcceptanceReport extraCanDoNextReport = this.ExtraCanDoNextReport;
			if (!extraCanDoNextReport.Accepted && !extraCanDoNextReport.Reason.NullOrEmpty())
			{
				Rect rect2 = new Rect(rect.center.x + Page.BottomButSize.x / 2f + 4f, rect.y + rect.height - Page.BottomButSize.y, Page.BottomButSize.x, Page.BottomButSize.y);
				rect2.xMax = rect.xMax - Page.BottomButSize.x - 4f;
				string text = this.ExtraCanDoNextReport.Reason.TruncateHeight(rect2.width, rect2.height, null);
				using (new TextBlock(GameFont.Tiny, Color.red))
				{
					Widgets.Label(rect2, text);
				}
				if (this.ExtraCanDoNextReport.Reason != text && Mouse.IsOver(rect2))
				{
					Widgets.DrawHighlight(rect2);
					TooltipHandler.TipRegion(rect2, this.ExtraCanDoNextReport.Reason);
				}
			}
			rect.yMax -= 48f;
			Rect rect3 = rect;
			rect3.width = 140f;
			this.DrawPawnList(rect3);
			UIHighlighter.HighlightOpportunity(rect3, "ReorderPawn");
			Rect rect4 = rect;
			rect4.xMin += 140f;
			Rect rect5 = rect4.BottomPartPixels(127f);
			rect4.yMax = rect5.yMin;
			rect4 = rect4.ContractedBy(4f);
			rect5 = rect5.ContractedBy(4f);
			StartingPawnUtility.DrawPortraitArea(rect4, this.curPawnIndex, this.renderClothes, this.renderHeadgear);
			StartingPawnUtility.DrawSkillSummaries(rect5);
		}

		// Token: 0x0600BFB7 RID: 49079 RVA: 0x0045AA20 File Offset: 0x00458C20
		private void DrawPawnList(Rect rect)
		{
			Rect position = rect;
			position.yMax -= 22f;
			float num = 0f;
			if (Find.GameInitData.startingPawnCount < Find.GameInitData.startingAndOptionalPawns.Count)
			{
				num = 22f;
			}
			float num2 = (float)Find.GameInitData.startingAndOptionalPawns.Count * 60f + 22f + num;
			float num3 = (num2 > position.height) ? 16f : 0f;
			Rect rect2 = new Rect(0f, 0f, position.width - num3, num2);
			Rect rect3 = rect2;
			rect3.height = 60f;
			rect3 = rect3.ContractedBy(4f);
			this.scroll = GUI.BeginScrollView(position, this.scroll, rect2);
			if (Event.current.type == EventType.repaint)
			{
				this.reorderableGroupID = ReorderableWidget.NewGroup(delegate(int from, int to)
				{
					if (!TutorSystem.AllowAction("ReorderPawn"))
					{
						return;
					}
					Pawn item = Find.GameInitData.startingAndOptionalPawns[from];
					Find.GameInitData.startingAndOptionalPawns.Insert(to, item);
					Find.GameInitData.startingAndOptionalPawns.RemoveAt((from < to) ? from : (from + 1));
					StartingPawnUtility.ReorderRequests(from, to);
					TutorSystem.Notify_Event("ReorderPawn");
					if (to < Find.GameInitData.startingPawnCount && from >= Find.GameInitData.startingPawnCount)
					{
						TutorSystem.Notify_Event("ReorderPawnOptionalToStarting");
					}
					int num4;
					if (from >= to)
					{
						this.curPawnIndex = to;
						num4 = to;
					}
					else
					{
						num4 = to - 1;
					}
					this.curPawnIndex = num4;
				}, ReorderableDirection.Vertical, rect, -1f, null, false);
			}
			this.DrawPawnListLabel(ref rect3, "StartingPawnsSelected".Translate(), false);
			for (int i = 0; i < Find.GameInitData.startingAndOptionalPawns.Count; i++)
			{
				if (i == Find.GameInitData.startingPawnCount)
				{
					this.DrawPawnListLabel(ref rect3, "StartingPawnsLeftBehind".Translate(), false);
				}
				Pawn pawn = Find.GameInitData.startingAndOptionalPawns[i];
				Widgets.BeginGroup(rect3.ExpandedBy(4f));
				Rect rect4 = new Rect(new Vector2(4f, 4f), rect3.size);
				Widgets.DrawOptionBackground(rect4, this.curPawnIndex == i);
				MouseoverSounds.DoRegion(rect4);
				Widgets.BeginGroup(rect4);
				GUI.color = new Color(1f, 1f, 1f, 0.2f);
				Rect position2 = new Rect(110f - Page_ConfigureStartingPawns.PawnSelectorPortraitSize.x / 2f, 40f - Page_ConfigureStartingPawns.PawnSelectorPortraitSize.y / 2f, Page_ConfigureStartingPawns.PawnSelectorPortraitSize.x, Page_ConfigureStartingPawns.PawnSelectorPortraitSize.y);
				Pawn pawn2 = pawn;
				Vector2 pawnSelectorPortraitSize = Page_ConfigureStartingPawns.PawnSelectorPortraitSize;
				Rot4 south = Rot4.South;
				bool flag = this.renderClothes;
				bool flag2 = this.renderHeadgear;
				GUI.DrawTexture(position2, PortraitsCache.Get(pawn2, pawnSelectorPortraitSize, south, default(Vector3), 1f, true, true, flag2, flag, null, null, false, null));
				GUI.color = Color.white;
				NameTriple nameTriple = pawn.Name as NameTriple;
				string label;
				if (nameTriple != null)
				{
					label = (string.IsNullOrEmpty(nameTriple.Nick) ? nameTriple.First : nameTriple.Nick);
				}
				else
				{
					label = pawn.LabelShort;
				}
				Widgets.Label(rect4.TopPart(0.5f).Rounded(), label);
				if (Text.CalcSize(pawn.story.TitleCap).x > rect4.width)
				{
					Widgets.Label(rect4.BottomPart(0.5f).Rounded(), pawn.story.TitleShortCap);
				}
				else
				{
					Widgets.Label(rect4.BottomPart(0.5f).Rounded(), pawn.story.TitleCap);
				}
				if (Event.current.type == EventType.mouseDown && Mouse.IsOver(rect4))
				{
					this.curPawnIndex = i;
					SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
				}
				Widgets.EndGroup();
				Widgets.EndGroup();
				if (ReorderableWidget.Reorderable(this.reorderableGroupID, rect3.ExpandedBy(4f), false, true))
				{
					Widgets.DrawRectFast(rect3, Widgets.WindowBGFillColor * new Color(1f, 1f, 1f, 0.5f), null);
				}
				if (Mouse.IsOver(rect3))
				{
					TooltipHandler.TipRegion(rect3, new TipSignal("DragToReorder".Translate(), pawn.GetHashCode() * 3499));
				}
				rect3.y += 60f;
			}
			GUI.EndScrollView();
			Rect rect5 = rect;
			rect5.yMin = position.yMax;
			using (new TextBlock(Color.gray))
			{
				Widgets.Label(rect5, "DragToReorder".Translate());
			}
		}

		// Token: 0x0600BFB8 RID: 49080 RVA: 0x0045AE64 File Offset: 0x00459064
		private void DrawPawnListLabel(ref Rect rect, string label, bool isGray = false)
		{
			Rect rect2 = rect;
			rect2.xMin -= 4f;
			rect2.height = 22f;
			rect.y += 22f;
			string text = label.Truncate(rect2.width, null);
			using (new TextBlock(isGray ? Color.gray : Color.white))
			{
				Widgets.Label(rect2, text);
			}
			if (label != text)
			{
				TooltipHandler.TipRegion(rect2, label);
			}
		}

		// Token: 0x0600BFB9 RID: 49081 RVA: 0x0045AF0C File Offset: 0x0045910C
		private void DrawApparelOptions(Rect rect)
		{
			if (ModsConfig.IdeologyActive)
			{
				string text = "ShowHeadgear".Translate();
				string text2 = "ShowApparel".Translate();
				float num = Mathf.Max(Text.CalcSize(text).x, Text.CalcSize(text2).x) + 4f + 24f;
				Rect rect2 = new Rect(rect.xMax - num, rect.y, num, Text.LineHeight * 2f);
				Widgets.CheckboxLabeled(new Rect(rect2.x, rect2.y, rect2.width, rect2.height / 2f), text, ref this.renderHeadgear, false, null, null, false, false);
				Widgets.CheckboxLabeled(new Rect(rect2.x, rect2.y + rect2.height / 2f, rect2.width, rect2.height / 2f), text2, ref this.renderClothes, false, null, null, false, false);
			}
		}

		// Token: 0x0600BFBA RID: 49082 RVA: 0x0045B00C File Offset: 0x0045920C
		private void DrawXenotypeEditorButton(Rect rect)
		{
			if (!ModsConfig.BiotechActive)
			{
				return;
			}
			Text.Font = GameFont.Small;
			float x = (rect.width - Page.BottomButSize.x) / 2f;
			float y = rect.y + rect.height - 38f;
			if (Widgets.ButtonText(new Rect(x, y, Page.BottomButSize.x, Page.BottomButSize.y), "XenotypeEditor".Translate(), true, true, true, null))
			{
				Find.WindowStack.Add(new Dialog_CreateXenotype(this.curPawnIndex, delegate()
				{
					CharacterCardUtility.cachedCustomXenotypes = null;
					StartingPawnUtility.RandomizePawn(this.curPawnIndex);
				}));
			}
		}

		// Token: 0x0600BFBB RID: 49083 RVA: 0x0045B0B4 File Offset: 0x004592B4
		protected override bool CanDoNext()
		{
			if (!base.CanDoNext())
			{
				return false;
			}
			if (TutorSystem.TutorialMode)
			{
				WorkTypeDef workTypeDef = StartingPawnUtility.RequiredWorkTypesDisabledForEveryone().FirstOrDefault<WorkTypeDef>();
				if (workTypeDef != null)
				{
					Messages.Message("RequiredWorkTypeDisabledForEveryone".Translate() + ": " + workTypeDef.gerundLabel.CapitalizeFirst() + ".", MessageTypeDefOf.RejectInput, false);
					return false;
				}
			}
			using (List<Pawn>.Enumerator enumerator = Find.GameInitData.startingAndOptionalPawns.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.Name.IsValid)
					{
						Messages.Message("EveryoneNeedsValidName".Translate(), MessageTypeDefOf.RejectInput, false);
						return false;
					}
				}
			}
			AcceptanceReport extraCanDoNextReport = this.ExtraCanDoNextReport;
			if (!extraCanDoNextReport.Reason.NullOrEmpty())
			{
				Messages.Message(extraCanDoNextReport.Reason, MessageTypeDefOf.RejectInput, false);
				return false;
			}
			PortraitsCache.Clear();
			return true;
		}

		// Token: 0x0600BFBC RID: 49084 RVA: 0x0045B1C0 File Offset: 0x004593C0
		protected override void DoNext()
		{
			this.CheckWarnRequiredWorkTypesDisabledForEveryone(delegate
			{
				foreach (Pawn pawn in Find.GameInitData.startingAndOptionalPawns)
				{
					NameTriple nameTriple = pawn.Name as NameTriple;
					if (nameTriple != null && string.IsNullOrEmpty(nameTriple.Nick))
					{
						pawn.Name = new NameTriple(nameTriple.First, nameTriple.First, nameTriple.Last);
					}
				}
				base.DoNext();
			});
		}

		// Token: 0x0600BFBD RID: 49085 RVA: 0x0045B1D4 File Offset: 0x004593D4
		private void CheckWarnRequiredWorkTypesDisabledForEveryone(Action nextAction)
		{
			IEnumerable<WorkTypeDef> enumerable = StartingPawnUtility.RequiredWorkTypesDisabledForEveryone();
			if (enumerable.Any<WorkTypeDef>())
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (WorkTypeDef workTypeDef in enumerable)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append("  - " + workTypeDef.gerundLabel.CapitalizeFirst());
				}
				TaggedString text = "ConfirmRequiredWorkTypeDisabledForEveryone".Translate(stringBuilder.ToString());
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(text, nextAction, false, null, WindowLayer.Dialog));
				return;
			}
			nextAction();
		}

		// Token: 0x0600BFBE RID: 49086 RVA: 0x0045B28C File Offset: 0x0045948C
		public void SelectPawn(Pawn c)
		{
			int num = StartingPawnUtility.PawnIndex(c);
			if (num != -1)
			{
				this.curPawnIndex = num;
			}
		}

		// Token: 0x04006E39 RID: 28217
		private int curPawnIndex;

		// Token: 0x04006E3A RID: 28218
		private bool renderClothes;

		// Token: 0x04006E3B RID: 28219
		private bool renderHeadgear;

		// Token: 0x04006E3C RID: 28220
		private int reorderableGroupID;

		// Token: 0x04006E3D RID: 28221
		private Vector2 scroll;

		// Token: 0x04006E3E RID: 28222
		private const float TabAreaWidth = 140f;

		// Token: 0x04006E3F RID: 28223
		private const float RightRectLeftPadding = 5f;

		// Token: 0x04006E40 RID: 28224
		private const float PawnEntryHeight = 60f;

		// Token: 0x04006E41 RID: 28225
		private const float SkillSummaryHeight = 127f;

		// Token: 0x04006E42 RID: 28226
		public static readonly Vector2 PawnPortraitSize = new Vector2(92f, 128f);

		// Token: 0x04006E43 RID: 28227
		private static readonly Vector2 PawnSelectorPortraitSize = new Vector2(70f, 110f);
	}
}
