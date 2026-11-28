using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02001DEB RID: 7659
	[StaticConstructorOnStartup]
	public abstract class PawnRoleSelectionWidgetBase<RoleType> : IPawnRoleSelectionWidget where RoleType : class, ILordJobRole
	{
		// Token: 0x0600C719 RID: 50969 RVA: 0x0048EDDD File Offset: 0x0048CFDD
		public virtual string SpectatorsLabel()
		{
			return "Spectators".Translate();
		}

		// Token: 0x0600C71A RID: 50970 RVA: 0x0048EDEE File Offset: 0x0048CFEE
		public virtual string NotParticipatingLabel()
		{
			return "NotParticipating".Translate();
		}

		// Token: 0x0600C71B RID: 50971 RVA: 0x0048EDFF File Offset: 0x0048CFFF
		public virtual bool ShouldDrawHighlight(RoleType role, Pawn pawn)
		{
			return false;
		}

		// Token: 0x0600C71C RID: 50972 RVA: 0x0048EE02 File Offset: 0x0048D002
		public virtual string SpectatorFilterReason(Pawn pawn)
		{
			return null;
		}

		// Token: 0x0600C71D RID: 50973 RVA: 0x0048EE05 File Offset: 0x0048D005
		public virtual string ExtraInfoForRole(RoleType role, Pawn pawnToBeAssigned, IEnumerable<Pawn> currentlyAssigned)
		{
			return null;
		}

		// Token: 0x0600C71E RID: 50974 RVA: 0x0048EE08 File Offset: 0x0048D008
		public virtual bool ShouldGrayOut(Pawn pawn, out TaggedString reason)
		{
			return !this.assignments.CanParticipate(pawn, out reason);
		}

		// Token: 0x0600C71F RID: 50975 RVA: 0x0048EE1A File Offset: 0x0048D01A
		public PawnRoleSelectionWidgetBase(ILordJobCandidatePool candidatePool, ILordJobAssignmentsManager<RoleType> assignments)
		{
			this.candidatePool = candidatePool;
			this.assignments = assignments;
		}

		// Token: 0x0600C720 RID: 50976 RVA: 0x0048EE44 File Offset: 0x0048D044
		public void DrawPawnList(Rect listRectPawns)
		{
			float num = (this.listScrollViewHeight > listRectPawns.height) ? 16f : 0f;
			Rect viewRect = new Rect(0f, 0f, listRectPawns.width - num, this.listScrollViewHeight);
			Widgets.BeginScrollView(listRectPawns, ref this.scrollPositionPawns, viewRect, true);
			try
			{
				this.DrawPawnListInternal(viewRect, listRectPawns);
			}
			finally
			{
				Widgets.EndScrollView();
			}
		}

		// Token: 0x0600C721 RID: 50977 RVA: 0x0048EEBC File Offset: 0x0048D0BC
		public void WindowUpdate()
		{
			this.scrollPositionPawns.y = this.scrollPositionPawns.y + (float)(this.pawnsListEdgeScrollDirection * 1000) * Time.deltaTime;
		}

		// Token: 0x0600C722 RID: 50978 RVA: 0x0048EEE0 File Offset: 0x0048D0E0
		public virtual void Notify_AssignmentsChanged()
		{
		}

		// Token: 0x0600C723 RID: 50979 RVA: 0x0048EEE4 File Offset: 0x0048D0E4
		private void DrawPawnListInternal(Rect viewRect, Rect listRect)
		{
			PawnRoleSelectionWidgetBase<RoleType>.rolesGroupedTmp.Clear();
			PawnRoleSelectionWidgetBase<RoleType>.rolesGroupedTmp.AddRange(this.assignments.RoleGroups());
			try
			{
				int num = DragAndDropWidget.NewGroup(null);
				this.dragAndDropGroup = ((num == -1) ? this.dragAndDropGroup : num);
				int maxPawnsPerRow = Mathf.FloorToInt((viewRect.width - 8f) / 54f);
				float num2 = 0f;
				float num3 = 0f;
				float num4 = 0f;
				int num5 = 0;
				foreach (IGrouping<string, RoleType> grouping in PawnRoleSelectionWidgetBase<RoleType>.rolesGroupedTmp)
				{
					IGrouping<string, RoleType> localRoleGroup = grouping;
					RoleType roleType = grouping.First<RoleType>();
					int num6 = 0;
					foreach (RoleType roleType2 in grouping)
					{
						num6 += roleType2.MaxCount;
						num5 += roleType2.MaxCount;
					}
					string extraInfo = this.ExtraPawnAssignmentInfo(localRoleGroup, null);
					IEnumerable<Pawn> enumerable = grouping.SelectMany((RoleType r) => this.assignments.AssignedPawns(r));
					TaggedString taggedString = Find.ActiveLanguageWorker.Pluralize(roleType.CategoryLabelCap, num6);
					Vector2 mp = Event.current.mousePosition;
					this.DrawRoleGroup(viewRect, enumerable, taggedString, num6, maxPawnsPerRow, ref num4, ref num3, ref num2, delegate(Pawn p, Vector2 dropPos)
					{
						Pawn pawn2 = (Pawn)DragAndDropWidget.DraggableAt(this.dragAndDropGroup, mp);
						if (pawn2 != null)
						{
							this.TryAssignReplace(p, localRoleGroup, pawn2);
							return;
						}
						this.TryAssign(p, localRoleGroup, true, (Pawn)DragAndDropWidget.GetDraggableAfter(this.dragAndDropGroup, dropPos), true, true);
					}, grouping, extraInfo, enumerable.Any<Pawn>() && enumerable.All((Pawn p) => this.assignments.Required(p)), PawnRoleSelectionWidgetBase<RoleType>.WarningIcon, null, delegate(Pawn p)
					{
						if (this.assignments.Required(p))
						{
							return;
						}
						if (!this.assignments.TryAssignSpectate(p, null))
						{
							this.assignments.RemoveParticipant(p);
						}
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					}, new TryGetPredicate<Pawn, TaggedString>(this.ShouldGrayOut));
				}
				List<Pawn> allCandidatePawns = this.candidatePool.AllCandidatePawns;
				string spectatorLabel = this.SpectatorsLabel();
				if (this.assignments.SpectatorsAllowed)
				{
					List<Pawn> spectatorsForReading = this.assignments.SpectatorsForReading;
					this.DrawRoleGroup(viewRect, spectatorsForReading, spectatorLabel, allCandidatePawns.Count, maxPawnsPerRow, ref num4, ref num3, ref num2, delegate(Pawn p, Vector2 dropPos)
					{
						if (this.SendTryAssignMessages(p, null, false))
						{
							return;
						}
						this.assignments.TryAssignSpectate(p, (Pawn)DragAndDropWidget.GetDraggableAfter(this.dragAndDropGroup, dropPos));
						SoundDefOf.DropElement.PlayOneShotOnCamera(null);
					}, PawnRoleSelectionWidgetBase<RoleType>.DropContextSpectator, null, false, null, delegate(Pawn p)
					{
						this.TryAssignAnyRole(p);
					}, delegate(Pawn p)
					{
						if (this.assignments.Required(p))
						{
							return;
						}
						this.assignments.RemoveParticipant(p);
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					}, new TryGetPredicate<Pawn, TaggedString>(this.ShouldGrayOut));
				}
				PawnRoleSelectionWidgetBase<RoleType>.nonParticipatingPawnCandidatesTmp.Clear();
				PawnRoleSelectionWidgetBase<RoleType>.nonParticipatingPawnCandidatesTmp.AddRange(allCandidatePawns);
				PawnRoleSelectionWidgetBase<RoleType>.nonParticipatingPawnCandidatesTmp.AddRange(this.candidatePool.NonAssignablePawns);
				PawnRoleSelectionWidgetBase<RoleType>.nonParticipatingPawnCandidatesTmp.RemoveDuplicates(null);
				IEnumerable<Pawn> selectedPawns = from p in PawnRoleSelectionWidgetBase<RoleType>.nonParticipatingPawnCandidatesTmp
				where !this.assignments.PawnParticipating(p)
				select p;
				string text3;
				this.DrawRoleGroup(viewRect, selectedPawns, this.NotParticipatingLabel(), PawnRoleSelectionWidgetBase<RoleType>.nonParticipatingPawnCandidatesTmp.Count, maxPawnsPerRow, ref num4, ref num3, ref num2, delegate(Pawn p, Vector2 dropPos)
				{
					this.assignments.RemoveParticipant(p);
					SoundDefOf.DropElement.PlayOneShotOnCamera(null);
				}, PawnRoleSelectionWidgetBase<RoleType>.DropContextNotParticipating, null, false, null, delegate(Pawn p)
				{
					RoleType roleType4;
					bool flag2;
					string text3 = this.CannotAssignReason(p, null, out roleType4, out flag2, false);
					if (text3 != null)
					{
						if (!this.TryAssignAnyRole(p))
						{
							Messages.Message(text3, LookTargets.Invalid, MessageTypeDefOf.RejectInput, false);
						}
						return;
					}
					this.assignments.TryAssignSpectate(p, null);
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}, delegate(Pawn p)
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					RoleType roleType4;
					bool flag2;
					string text3 = this.CannotAssignReason(p, null, out roleType4, out flag2, false);
					if (this.assignments.SpectatorsAllowed)
					{
						Action action2 = (text3 != null) ? null : new Action(delegate()
						{
							this.assignments.TryAssignSpectate(p, null);
						});
						list.Add(new FloatMenuOption(PawnRoleSelectionWidgetBase<RoleType>.PostProcessFloatLabel(spectatorLabel, text3, null), action2, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					foreach (IGrouping<string, RoleType> grouping3 in PawnRoleSelectionWidgetBase<RoleType>.rolesGroupedTmp)
					{
						IGrouping<string, RoleType> localRoleGroup = grouping3;
						RoleType roleType5 = grouping3.First<RoleType>();
						bool mustReplace;
						text3 = this.CannotAssignReason(p, localRoleGroup, out roleType4, out mustReplace, true);
						Pawn replacing = mustReplace ? localRoleGroup.SelectMany((RoleType role) => this.assignments.AssignedPawns(role)).Last<Pawn>() : null;
						Action action3 = (text3 != null) ? null : new Action(delegate()
						{
							if (mustReplace)
							{
								this.TryAssignReplace(p, localRoleGroup, replacing);
								return;
							}
							this.TryAssign(p, localRoleGroup, true, null, true, false);
						});
						list.Add(new FloatMenuOption(PawnRoleSelectionWidgetBase<RoleType>.PostProcessFloatLabel(roleType5.LabelCap, text3, replacing), action3, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					Find.WindowStack.Add(new FloatMenu(list));
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}, new TryGetPredicate<Pawn, TaggedString>(this.ShouldGrayOut));
				this.highlightedRole = default(RoleType);
				num3 += num2 + 4f;
				using (new TextBlock(new GameFont?(GameFont.Tiny), new TextAnchor?(TextAnchor.MiddleLeft), new Color?(ColorLibrary.Grey)))
				{
					Widgets.Label(new Rect(viewRect.x, num3, viewRect.width, 24f), SteamDeck.IsSteamDeckInNonKeyboardMode ? "DragPawnsToRolesInfoController".Translate() : "DragPawnsToRolesInfo".Translate());
				}
				num3 += 20f;
				if (Event.current.type == EventType.layout)
				{
					this.listScrollViewHeight = num3;
				}
				foreach (Action action in PawnRoleSelectionWidgetBase<RoleType>.tmpDelayedGuiCalls)
				{
					action();
				}
				object obj = DragAndDropWidget.CurrentlyDraggedDraggable();
				Pawn pawn = (Pawn)DragAndDropWidget.DraggableAt(this.dragAndDropGroup, Event.current.mousePosition);
				if (obj != null)
				{
					object obj2 = DragAndDropWidget.HoveringDropArea(this.dragAndDropGroup);
					if (obj2 != null)
					{
						Rect? rect = DragAndDropWidget.HoveringDropAreaRect(this.dragAndDropGroup, null);
						if (this.lastHoveredDropArea != null && rect != null && rect != this.lastHoveredDropArea)
						{
							SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
						}
						this.lastHoveredDropArea = rect;
					}
					if (obj2 != null && obj2 != PawnRoleSelectionWidgetBase<RoleType>.DropContextNotParticipating)
					{
						IGrouping<string, RoleType> grouping2 = obj2 as IGrouping<string, RoleType>;
						RoleType roleType3;
						bool flag;
						text3 = this.CannotAssignReason((Pawn)obj, grouping2, out roleType3, out flag, grouping2 != null && pawn != null);
						string text2 = (roleType3 == null) ? null : this.ExtraPawnAssignmentInfo(grouping2, (Pawn)obj);
						if (!string.IsNullOrWhiteSpace(text3) || !string.IsNullOrWhiteSpace(text2))
						{
							string text = string.IsNullOrWhiteSpace(text3) ? text2 : text3;
							Color color = string.IsNullOrWhiteSpace(text3) ? ColorLibrary.Yellow : ColorLibrary.RedReadable;
							Text.Font = GameFont.Small;
							Vector2 vector = Text.CalcSize(text);
							Rect r = new Rect(UI.MousePositionOnUI.x - vector.x / 2f, (float)UI.screenHeight - UI.MousePositionOnUI.y - vector.y - 10f, vector.x, vector.y).ExpandedBy(5f);
							Find.WindowStack.ImmediateWindow(47839543, r, WindowLayer.Super, delegate
							{
								Text.Font = GameFont.Small;
								GUI.color = color;
								Widgets.Label(r.AtZero().ContractedBy(5f), text);
								GUI.color = Color.white;
							}, true, false, 1f, null, false);
						}
					}
				}
			}
			finally
			{
				PawnRoleSelectionWidgetBase<RoleType>.tmpDelayedGuiCalls.Clear();
			}
			this.pawnsListEdgeScrollDirection = 0;
			if (DragAndDropWidget.CurrentlyDraggedDraggable() != null)
			{
				Rect rect2 = new Rect(viewRect.x, this.scrollPositionPawns.y, viewRect.width, 30f);
				Rect rect3 = new Rect(viewRect.x, this.scrollPositionPawns.y + (listRect.height - 30f), viewRect.width, 30f);
				if (Mouse.IsOver(rect2))
				{
					this.pawnsListEdgeScrollDirection = -1;
					return;
				}
				if (Mouse.IsOver(rect3))
				{
					this.pawnsListEdgeScrollDirection = 1;
				}
			}
		}

		// Token: 0x0600C724 RID: 50980 RVA: 0x0048F5E8 File Offset: 0x0048D7E8
		private void DrawRoleGroup(Rect viewRect, IEnumerable<Pawn> selectedPawns, string headline, int maxPawns, int maxPawnsPerRow, ref float curX, ref float curY, ref float rowHeight, Action<Pawn, Vector2> assignAction, object dropAreaContext, string extraInfo = null, bool locked = false, Texture2D extraInfoIcon = null, Action<Pawn> clickHandler = null, Action<Pawn> rightClickHandler = null, TryGetPredicate<Pawn, TaggedString> isGrayedOutPredicate = null)
		{
			using (new ProfilerBlock("DrawRoleGroup"))
			{
				PawnRoleSelectionWidgetBase<RoleType>.tmpSelectedPawns.AddRange(selectedPawns);
				try
				{
					int num = Mathf.Min(maxPawns, PawnRoleSelectionWidgetBase<RoleType>.tmpSelectedPawns.Count + 1);
					if (num == 0)
					{
						num = 1;
					}
					int num2 = Mathf.CeilToInt((float)num / (float)maxPawnsPerRow);
					int num3 = Mathf.Min(maxPawnsPerRow, num);
					int num4 = num3 * 50 + (num3 - 1) * 4;
					int num5 = num2 * 70 + (num2 - 1) * 4;
					Vector2 vector = Text.CalcSize(headline);
					int num6 = 60;
					int num7 = Mathf.Max(num4, (int)vector.x + num6 + ((num6 > 0) ? 10 : 0));
					int num8 = Mathf.FloorToInt((viewRect.width - (curX + 26f)) / 50f);
					bool flag = (num8 > 0 && num8 < maxPawns) || maxPawns + 2 >= maxPawnsPerRow;
					if (flag)
					{
						num4 = maxPawnsPerRow * 50 + (maxPawnsPerRow - 1) * 4;
					}
					if (curX + (float)num7 + 26f > viewRect.width || flag)
					{
						curY += rowHeight;
						rowHeight = 0f;
						curX = 0f;
					}
					float num9 = 0f;
					Rect rect = new Rect(viewRect.x + curX, viewRect.y + num9 + curY, vector.x, vector.y);
					Rect rect2 = new Rect(rect.xMax + 10f, rect.y + (vector.y - 20f) / 2f, (float)num6, 20f);
					num9 += vector.y + 4f;
					Rect rect3 = new Rect(viewRect.x + curX, viewRect.y + num9 + curY, (float)(num4 + 8), (float)(num5 + 8));
					num9 += rect3.height + 10f;
					rowHeight = Mathf.Max(rowHeight, num9);
					curX += (float)(num7 + 26);
					GUI.color = (locked ? ColorLibrary.Grey : Color.white);
					Widgets.Label(rect, headline);
					GUI.color = Color.white;
					Widgets.DrawRectFast(rect3, Widgets.MenuSectionBGFillColor, null);
					IGrouping<string, RoleType> grouping = dropAreaContext as IGrouping<string, RoleType>;
					if (grouping != null && Mouse.IsOver(rect3))
					{
						this.highlightedRole = grouping.First<RoleType>();
					}
					if (locked)
					{
						Rect rect4 = new Rect(rect2.x, rect2.y, 20f, 20f);
						rect2.x += rect4.width;
						rect2.width -= rect4.height;
						Widgets.DrawTextureFitted(rect4, IdeoUIUtility.LockedTex, 1f, 1f);
						TooltipHandler.TipRegion(rect4, () => "Required".Translate(), 93457856);
					}
					if (extraInfo != null)
					{
						Rect rect5 = new Rect(rect2.x, rect2.y, 20f, 20f);
						rect2.x += rect5.width;
						rect2.width -= rect5.height;
						GUI.color = (Mouse.IsOver(rect5) ? Color.white : new Color(0.8f, 0.8f, 0.8f, 1f));
						Widgets.DrawTextureFitted(rect5, extraInfoIcon ?? PawnRoleSelectionWidgetBase<RoleType>.WarningIcon, 1f, 1f);
						GUI.color = Color.white;
						TooltipHandler.TipRegion(rect5, () => extraInfo, 34899345);
					}
					Rect rect6 = rect3.ContractedBy(4f);
					DragAndDropWidget.DropArea(this.dragAndDropGroup, rect6, delegate(object pawn)
					{
						assignAction((Pawn)pawn, Event.current.mousePosition);
					}, dropAreaContext);
					if (Mouse.IsOver(rect6))
					{
						Widgets.DrawBoxSolidWithOutline(rect3, new Color(0.3f, 0.3f, 0.3f, 1f), new Color(0.5f, 0.5f, 0.5f, 1f), 3);
					}
					GenUI.DrawElementStack<Pawn>(rect6, 70f, PawnRoleSelectionWidgetBase<RoleType>.tmpSelectedPawns, delegate(Rect r, Pawn p)
					{
						this.DrawPawnPortrait(r, p, isGrayedOutPredicate, delegate
						{
							Action<Pawn> clickHandler2 = clickHandler;
							if (clickHandler2 == null)
							{
								return;
							}
							clickHandler2(p);
						}, delegate
						{
							Action<Pawn> rightClickHandler2 = rightClickHandler;
							if (rightClickHandler2 == null)
							{
								return;
							}
							rightClickHandler2(p);
						});
					}, (Pawn p) => 50f, 4f, 4f, false);
				}
				finally
				{
					PawnRoleSelectionWidgetBase<RoleType>.tmpSelectedPawns.Clear();
				}
			}
		}

		// Token: 0x0600C725 RID: 50981 RVA: 0x0048FAB4 File Offset: 0x0048DCB4
		private void DrawPawnPortraitInternal(Rect r, Pawn pawn, bool dragging, float scale, TryGetPredicate<Pawn, TaggedString> isGrayedOutPredicate = null)
		{
			using (new ProfilerBlock("DrawPawnPortraitInternal"))
			{
				Rect rect = new Rect(r.x, r.y, r.width * scale, 50f * scale);
				Rect rect2 = new Rect(r.x, r.y + 50f * scale, r.width * scale, 20f * scale);
				TaggedString empty = TaggedString.Empty;
				bool flag = isGrayedOutPredicate != null && isGrayedOutPredicate(pawn, out empty);
				Material material = flag ? TexUI.GrayscaleGUI : null;
				GenUI.DrawTextureWithMaterial(rect, ColonistBar.BGTex, material, default(Rect));
				if (this.ShouldDrawHighlight(this.highlightedRole, pawn))
				{
					Widgets.DrawHighlight(rect.ContractedBy(3f));
				}
				RenderTexture texture = PortraitsCache.Get(pawn, new Vector2(100f, 100f), Rot4.South, new Vector3(0f, 0f, 0.1f), 1.5f, true, true, true, true, null, null, false, null);
				GenUI.DrawTextureWithMaterial(rect, texture, material, default(Rect));
				Widgets.DrawRectFast(rect2, Widgets.WindowBGFillColor, null);
				float yMax = rect.yMax;
				float xMax = rect.xMax;
				float iconSize = 20f * scale;
				bool flag2;
				PawnPortraitIconsDrawer.DrawPawnPortraitIcons(rect, pawn, this.assignments.Required(pawn), flag, ref xMax, ref yMax, iconSize, this.showIdeoIcon, out flag2);
				using (new TextBlock(new GameFont?(GameFont.Tiny), new TextAnchor?(TextAnchor.MiddleCenter), new Color?(flag ? Color.gray : Color.white)))
				{
					Widgets.LabelFit(rect2, pawn.LabelShortCap);
					if (!flag2 && !dragging)
					{
						TooltipHandler.TipRegion(new Rect(r.x, r.y, r.width * scale, 70f * scale), this.PawnTooltip(pawn, empty));
					}
				}
			}
		}

		// Token: 0x0600C726 RID: 50982 RVA: 0x0048FCF4 File Offset: 0x0048DEF4
		private string PawnTooltip(Pawn pawn, TaggedString cannotAssignReason)
		{
			this.tipSB.Clear();
			this.tipSB.AppendLineTagged(pawn.LabelShortCap.AsTipTitle());
			string text = this.ExtraTipContents(pawn);
			if (!text.NullOrEmpty())
			{
				this.tipSB.AppendLine(text);
			}
			if (!cannotAssignReason.NullOrEmpty())
			{
				this.tipSB.AppendLine();
				this.tipSB.AppendLineTagged(cannotAssignReason.Colorize(ColorLibrary.RedReadable));
			}
			return this.tipSB.ToString();
		}

		// Token: 0x0600C727 RID: 50983 RVA: 0x0048FD82 File Offset: 0x0048DF82
		protected virtual string ExtraTipContents(Pawn pawn)
		{
			return null;
		}

		// Token: 0x0600C728 RID: 50984 RVA: 0x0048FD88 File Offset: 0x0048DF88
		private void DrawPawnPortrait(Rect rect, Pawn pawn, TryGetPredicate<Pawn, TaggedString> isGrayedOutPredicate = null, Action clickHandler = null, Action rightClickHandler = null)
		{
			if (Mouse.IsOver(rect) && Event.current.type == EventType.mouseDown && Event.current.button == 1 && rightClickHandler != null)
			{
				rightClickHandler();
			}
			Vector2 mp = Event.current.mousePosition;
			if (!this.assignments.Required(pawn) && DragAndDropWidget.Draggable(this.dragAndDropGroup, rect, pawn, clickHandler, delegate
			{
				this.lastHoveredDropArea = DragAndDropWidget.HoveringDropAreaRect(this.dragAndDropGroup, new Vector3?(mp));
			}))
			{
				rect.position = Event.current.mousePosition;
				PawnRoleSelectionWidgetBase<RoleType>.tmpDelayedGuiCalls.Add(delegate
				{
					this.DrawPawnPortraitInternal(rect, pawn, true, 0.9f, isGrayedOutPredicate);
				});
				return;
			}
			this.DrawPawnPortraitInternal(rect, pawn, false, 1f, isGrayedOutPredicate);
			Widgets.DrawHighlightIfMouseover(rect);
		}

		// Token: 0x0600C729 RID: 50985 RVA: 0x0048FE88 File Offset: 0x0048E088
		private bool SendTryAssignMessages(Pawn pawn, IEnumerable<RoleType> roleGroup, bool isReplacing = false)
		{
			RoleType roleType;
			bool flag;
			string text = this.CannotAssignReason(pawn, roleGroup, out roleType, out flag, isReplacing);
			if (text != null)
			{
				Messages.Message(text, LookTargets.Invalid, MessageTypeDefOf.RejectInput, false);
				return true;
			}
			return false;
		}

		// Token: 0x0600C72A RID: 50986 RVA: 0x0048FEBC File Offset: 0x0048E0BC
		private bool DoTryAssign(Pawn pawn, IEnumerable<RoleType> roleGroup, bool sendMessage = true, Pawn insertBefore = null, bool doSound = true)
		{
			if (sendMessage && this.SendTryAssignMessages(pawn, roleGroup, false))
			{
				return false;
			}
			RoleType roleType;
			bool flag;
			if (!sendMessage && this.CannotAssignReason(pawn, roleGroup, out roleType, out flag, false) != null)
			{
				return false;
			}
			foreach (RoleType role in roleGroup)
			{
				PsychicRitualRoleDef.Reason reason;
				if (this.assignments.TryAssign(pawn, role, out reason, PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual, insertBefore))
				{
					this.Notify_AssignmentsChanged();
					if (doSound)
					{
						SoundDefOf.DropElement.PlayOneShotOnCamera(null);
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600C72B RID: 50987 RVA: 0x0048FF54 File Offset: 0x0048E154
		private bool TryAssign(Pawn pawn, IEnumerable<RoleType> roleGroup, bool sendMessage = true, Pawn insertBefore = null, bool doSound = true, bool insertLast = false)
		{
			bool result;
			try
			{
				int num = 0;
				foreach (RoleType roleType in roleGroup)
				{
					if (roleType.MaxCount == 0)
					{
						num = -1;
					}
					else if (roleType.MaxCount > 0)
					{
						num += roleType.MaxCount;
					}
					PawnRoleSelectionWidgetBase<RoleType>.tmpAssignedPawns.AddRange(this.assignments.AssignedPawns(roleType));
				}
				if ((num > 0 && PawnRoleSelectionWidgetBase<RoleType>.tmpAssignedPawns.Count == num) || (insertBefore != null && !PawnRoleSelectionWidgetBase<RoleType>.tmpAssignedPawns.Contains(insertBefore)))
				{
					result = this.DoTryAssign(pawn, roleGroup, sendMessage, null, doSound);
				}
				else
				{
					foreach (Pawn pawn2 in PawnRoleSelectionWidgetBase<RoleType>.tmpAssignedPawns)
					{
						this.assignments.TryUnassignAnyRole(pawn2);
					}
					if (insertBefore == null)
					{
						if (insertLast)
						{
							PawnRoleSelectionWidgetBase<RoleType>.tmpAssignedPawns.Add(pawn);
						}
						else
						{
							PawnRoleSelectionWidgetBase<RoleType>.tmpAssignedPawns.Insert(0, pawn);
						}
					}
					else
					{
						PawnRoleSelectionWidgetBase<RoleType>.tmpAssignedPawns.Insert(PawnRoleSelectionWidgetBase<RoleType>.tmpAssignedPawns.IndexOf(insertBefore), pawn);
					}
					bool flag = false;
					foreach (Pawn pawn3 in PawnRoleSelectionWidgetBase<RoleType>.tmpAssignedPawns)
					{
						bool flag2 = this.DoTryAssign(pawn3, roleGroup, sendMessage && pawn3 == pawn, null, pawn3 == pawn);
						if (pawn3 == pawn)
						{
							flag = flag2;
						}
					}
					result = flag;
				}
			}
			finally
			{
				PawnRoleSelectionWidgetBase<RoleType>.tmpAssignedPawns.Clear();
			}
			return result;
		}

		// Token: 0x0600C72C RID: 50988 RVA: 0x00490144 File Offset: 0x0048E344
		private bool TryAssignReplace(Pawn pawn, IEnumerable<RoleType> roleGroup, Pawn replacing)
		{
			if (!this.SendTryAssignMessages(pawn, roleGroup, true))
			{
				bool flag = this.assignments.PawnSpectating(pawn);
				RoleType roleType = this.assignments.RoleForPawn(pawn, true);
				Pawn insertBefore = roleGroup.SelectMany((RoleType r) => this.assignments.AssignedPawns(r)).SkipWhile((Pawn p) => p != replacing).FirstOrDefault<Pawn>();
				this.assignments.RemoveParticipant(replacing);
				this.TryAssign(pawn, roleGroup, true, insertBefore, true, true);
				PsychicRitualRoleDef.Reason reason;
				if (flag)
				{
					this.assignments.TryAssignSpectate(replacing, null);
				}
				else if (roleType != null && this.assignments.TryAssign(replacing, roleType, out reason, PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual, null))
				{
					this.Notify_AssignmentsChanged();
				}
			}
			return roleGroup.Contains(this.assignments.RoleForPawn(pawn, true));
		}

		// Token: 0x0600C72D RID: 50989 RVA: 0x00490224 File Offset: 0x0048E424
		private bool TryAssignAnyRole(Pawn p)
		{
			string text = null;
			bool flag = PawnRoleSelectionWidgetBase<RoleType>.rolesGroupedTmp.Count == 1;
			foreach (IGrouping<string, RoleType> grouping in PawnRoleSelectionWidgetBase<RoleType>.rolesGroupedTmp)
			{
				RoleType roleType;
				bool flag2;
				text = this.CannotAssignReason(p, grouping, out roleType, out flag2, true);
				if (text == null && this.TryAssign(p, grouping, false, null, true, false))
				{
					return true;
				}
			}
			foreach (IGrouping<string, RoleType> grouping2 in PawnRoleSelectionWidgetBase<RoleType>.rolesGroupedTmp)
			{
				RoleType roleType;
				bool flag3;
				text = this.CannotAssignReason(p, grouping2, out roleType, out flag3, true);
				if (text == null)
				{
					Pawn replacing = flag3 ? grouping2.SelectMany((RoleType role) => this.assignments.AssignedPawns(role)).Last<Pawn>() : null;
					if (this.TryAssignReplace(p, grouping2, replacing))
					{
						return true;
					}
				}
			}
			if (flag && text != null)
			{
				Messages.Message(text, LookTargets.Invalid, MessageTypeDefOf.RejectInput, false);
			}
			SoundDefOf.ClickReject.PlayOneShotOnCamera(null);
			return false;
		}

		// Token: 0x0600C72E RID: 50990 RVA: 0x0049034C File Offset: 0x0048E54C
		private string ExtraPawnAssignmentInfo(IEnumerable<RoleType> roleGroup, Pawn pawnToBeAssigned = null)
		{
			RoleType roleType = (roleGroup != null) ? roleGroup.First<RoleType>() : default(RoleType);
			IEnumerable<Pawn> enumerable = this.assignments.AssignedPawns(roleType);
			if (pawnToBeAssigned != null)
			{
				enumerable = enumerable.Concat(new Pawn[]
				{
					pawnToBeAssigned
				}).Distinct<Pawn>();
			}
			string text = this.ExtraInfoForRole(roleType, pawnToBeAssigned, enumerable);
			if (roleType.MinCount > 0 && pawnToBeAssigned == null && this.assignments.FirstAssignedPawn(roleType) == null)
			{
				int num = 0;
				foreach (RoleType roleType2 in roleGroup)
				{
					num += roleType2.MinCount;
				}
				if (num > 1)
				{
					text += "MessageLordJobNeedsAtLeastNumRolePawn".Translate(Find.ActiveLanguageWorker.Pluralize(roleType.Label, -1), num);
				}
				else
				{
					text += "MessageLordJobNeedsAtLeastOneRolePawn".Translate(roleType.Label.Resolve());
				}
			}
			return text;
		}

		// Token: 0x0600C72F RID: 50991 RVA: 0x00490488 File Offset: 0x0048E688
		private static string PostProcessFloatLabel(string label, string unavailableReason, Pawn replacing = null)
		{
			string text = label;
			if (unavailableReason != null)
			{
				text += " (" + "DisabledLower".Translate().CapitalizeFirst() + ": " + unavailableReason.CapitalizeFirst() + ")";
			}
			if (replacing != null)
			{
				text += " (" + "RitualRoleReplaces".Translate(replacing.Named("PAWN")) + ")";
			}
			return "AssignToRole".Translate(text);
		}

		// Token: 0x0600C730 RID: 50992 RVA: 0x00490530 File Offset: 0x0048E730
		private string CannotAssignReason(Pawn draggable, IEnumerable<RoleType> roles, out RoleType firstRole, out bool mustReplace, bool isReplacing = false)
		{
			int num = 0;
			int num2 = 0;
			firstRole = ((roles != null) ? roles.FirstOrDefault<RoleType>() : default(RoleType));
			mustReplace = false;
			string text = this.assignments.PawnNotAssignableReason(draggable, firstRole).CapitalizeFirst();
			if (text == null && firstRole == null)
			{
				text = this.SpectatorFilterReason(draggable);
			}
			if (text == null && firstRole != null)
			{
				bool flag = true;
				foreach (RoleType roleType in roles)
				{
					if (!this.assignments.AssignedPawns(roleType).Any<Pawn>())
					{
						flag = false;
						break;
					}
					foreach (Pawn pawn in this.assignments.AssignedPawns(roleType))
					{
						if (this.assignments.ForcedRole(pawn) != roleType)
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					text = "RoleIsLocked".Translate(firstRole.Label);
				}
			}
			if (text == null && roles != null && !roles.Any((RoleType r) => this.assignments.RoleForPawn(draggable, true) == r))
			{
				foreach (RoleType roleType2 in roles)
				{
					if (roleType2.MaxCount <= 0)
					{
						num = -1;
					}
					if (num != -1)
					{
						num += roleType2.MaxCount;
					}
					num2 += this.assignments.AssignedPawns(roleType2).Count<Pawn>();
				}
				if (num >= 0 && num <= num2)
				{
					mustReplace = true;
					if (!isReplacing)
					{
						text = "MaxPawnsPerRole".Translate(firstRole.Label, num);
					}
				}
			}
			return text;
		}

		// Token: 0x0400713D RID: 28989
		protected ILordJobCandidatePool candidatePool;

		// Token: 0x0400713E RID: 28990
		protected ILordJobAssignmentsManager<RoleType> assignments;

		// Token: 0x0400713F RID: 28991
		public bool showIdeoIcon = true;

		// Token: 0x04007140 RID: 28992
		private int dragAndDropGroup;

		// Token: 0x04007141 RID: 28993
		private RoleType highlightedRole;

		// Token: 0x04007142 RID: 28994
		private static readonly object DropContextSpectator = new object();

		// Token: 0x04007143 RID: 28995
		private static readonly object DropContextNotParticipating = new object();

		// Token: 0x04007144 RID: 28996
		private static List<Action> tmpDelayedGuiCalls = new List<Action>();

		// Token: 0x04007145 RID: 28997
		private Rect? lastHoveredDropArea;

		// Token: 0x04007146 RID: 28998
		protected float listScrollViewHeight;

		// Token: 0x04007147 RID: 28999
		protected Vector2 scrollPositionPawns;

		// Token: 0x04007148 RID: 29000
		private int pawnsListEdgeScrollDirection;

		// Token: 0x04007149 RID: 29001
		private static List<IGrouping<string, RoleType>> rolesGroupedTmp = new List<IGrouping<string, RoleType>>();

		// Token: 0x0400714A RID: 29002
		private static readonly Texture2D WarningIcon = Resources.Load<Texture2D>("Textures/UI/Widgets/Warning");

		// Token: 0x0400714B RID: 29003
		public const int PawnsListPadding = 4;

		// Token: 0x0400714C RID: 29004
		public const int PawnsListHorizontalGap = 26;

		// Token: 0x0400714D RID: 29005
		public const int PawnPortraitHeight = 50;

		// Token: 0x0400714E RID: 29006
		public const int PawnPortraitWidth = 50;

		// Token: 0x0400714F RID: 29007
		public const int PawnPortraitLabelHeight = 20;

		// Token: 0x04007150 RID: 29008
		public const int PawnPortraitHeightTotal = 70;

		// Token: 0x04007151 RID: 29009
		public const int PawnPortraitMargin = 4;

		// Token: 0x04007152 RID: 29010
		public const int PawnPortraitIconSize = 20;

		// Token: 0x04007153 RID: 29011
		public const int HeadlineIconSize = 20;

		// Token: 0x04007154 RID: 29012
		public const int EdgeScrollSpeedWhileDragging = 1000;

		// Token: 0x04007155 RID: 29013
		private static List<Pawn> nonParticipatingPawnCandidatesTmp = new List<Pawn>();

		// Token: 0x04007156 RID: 29014
		private static readonly List<Pawn> tmpSelectedPawns = new List<Pawn>();

		// Token: 0x04007157 RID: 29015
		private StringBuilder tipSB = new StringBuilder();

		// Token: 0x04007158 RID: 29016
		private static List<Pawn> tmpAssignedPawns = new List<Pawn>();
	}
}
