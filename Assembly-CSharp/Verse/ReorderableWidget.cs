using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000653 RID: 1619
	public static class ReorderableWidget
	{
		// Token: 0x170009A1 RID: 2465
		// (get) Token: 0x060032B5 RID: 12981 RVA: 0x00143500 File Offset: 0x00141700
		public static bool Dragging
		{
			get
			{
				return ReorderableWidget.dragBegun;
			}
		}

		// Token: 0x170009A2 RID: 2466
		// (get) Token: 0x060032B6 RID: 12982 RVA: 0x00143507 File Offset: 0x00141707
		public static int GetDraggedIndex
		{
			get
			{
				return ReorderableWidget.GetIndexWithinGroup(ReorderableWidget.draggingReorderable);
			}
		}

		// Token: 0x170009A3 RID: 2467
		// (get) Token: 0x060032B7 RID: 12983 RVA: 0x00143513 File Offset: 0x00141713
		public static int GetDraggedFromGroupID
		{
			get
			{
				return ReorderableWidget.reorderables[ReorderableWidget.draggingReorderable].groupID;
			}
		}

		// Token: 0x060032B8 RID: 12984 RVA: 0x0014352C File Offset: 0x0014172C
		public static void ReorderableWidgetOnGUI_BeforeWindowStack()
		{
			if (ReorderableWidget.dragBegun && ReorderableWidget.draggingReorderable >= 0 && ReorderableWidget.draggingReorderable < ReorderableWidget.reorderables.Count)
			{
				int groupID = ReorderableWidget.reorderables[ReorderableWidget.draggingReorderable].groupID;
				if (groupID >= 0 && groupID < ReorderableWidget.groups.Count && ReorderableWidget.groups[groupID].extraDraggedItemOnGUI != null)
				{
					ReorderableWidget.groups[groupID].extraDraggedItemOnGUI(ReorderableWidget.GetIndexWithinGroup(ReorderableWidget.draggingReorderable), ReorderableWidget.dragStartPos);
				}
			}
		}

		// Token: 0x060032B9 RID: 12985 RVA: 0x001435B8 File Offset: 0x001417B8
		public static void ReorderableWidgetOnGUI_AfterWindowStack()
		{
			if (Event.current.rawType == EventType.mouseUp)
			{
				ReorderableWidget.released = true;
			}
			if (Event.current.type == EventType.repaint)
			{
				if (ReorderableWidget.clicked)
				{
					ReorderableWidget.StopDragging();
					for (int i = 0; i < ReorderableWidget.reorderables.Count; i++)
					{
						if (ReorderableWidget.reorderables[i].groupID == ReorderableWidget.groupClicked && ReorderableWidget.reorderables[i].rect == ReorderableWidget.clickedInRect)
						{
							ReorderableWidget.draggingReorderable = i;
							ReorderableWidget.dragStartPos = Event.current.mousePosition;
							break;
						}
					}
					ReorderableWidget.clicked = false;
				}
				if (ReorderableWidget.draggingReorderable >= ReorderableWidget.reorderables.Count)
				{
					ReorderableWidget.StopDragging();
				}
				if (ReorderableWidget.reorderables.Count != ReorderableWidget.lastFrameReorderableCount)
				{
					ReorderableWidget.StopDragging();
				}
				ReorderableWidget.lastInsertNear = ReorderableWidget.CurrentInsertNear(out ReorderableWidget.lastInsertNearLeft);
				ReorderableWidget.hoveredGroup = -1;
				for (int j = 0; j < ReorderableWidget.groups.Count; j++)
				{
					if (ReorderableWidget.groups[j].absRect.Contains(Event.current.mousePosition))
					{
						ReorderableWidget.hoveredGroup = j;
						if (ReorderableWidget.lastInsertNear >= 0 && ReorderableWidget.AreInMultiGroup(j, ReorderableWidget.reorderables[ReorderableWidget.lastInsertNear].groupID) && ReorderableWidget.reorderables[ReorderableWidget.lastInsertNear].groupID != j)
						{
							ReorderableWidget.lastInsertNear = ReorderableWidget.FindLastReorderableIndexWithinGroup(j);
							ReorderableWidget.lastInsertNearLeft = (ReorderableWidget.lastInsertNear < 0);
						}
					}
				}
				if (ReorderableWidget.released)
				{
					ReorderableWidget.released = false;
					if (ReorderableWidget.dragBegun && ReorderableWidget.draggingReorderable >= 0)
					{
						int indexWithinGroup = ReorderableWidget.GetIndexWithinGroup(ReorderableWidget.draggingReorderable);
						int groupID = ReorderableWidget.reorderables[ReorderableWidget.draggingReorderable].groupID;
						int num;
						if (ReorderableWidget.lastInsertNear == ReorderableWidget.draggingReorderable)
						{
							num = indexWithinGroup;
						}
						else if (ReorderableWidget.lastInsertNearLeft)
						{
							num = ReorderableWidget.GetIndexWithinGroup(ReorderableWidget.lastInsertNear);
						}
						else
						{
							num = ReorderableWidget.GetIndexWithinGroup(ReorderableWidget.lastInsertNear) + 1;
						}
						int num2 = -1;
						if (ReorderableWidget.lastInsertNear >= 0)
						{
							num2 = ReorderableWidget.reorderables[ReorderableWidget.lastInsertNear].groupID;
						}
						if (ReorderableWidget.AreInMultiGroup(groupID, ReorderableWidget.hoveredGroup) && ReorderableWidget.hoveredGroup >= 0 && ReorderableWidget.hoveredGroup != num2)
						{
							num2 = ReorderableWidget.hoveredGroup;
							num = ReorderableWidget.GetIndexWithinGroup(ReorderableWidget.FindLastReorderableIndexWithinGroup(num2)) + 1;
						}
						if (ReorderableWidget.AreInMultiGroup(groupID, num2))
						{
							ReorderableWidget.GetMultiGroupByGroupID(groupID).Value.reorderedAction(indexWithinGroup, groupID, num, num2);
							SoundDefOf.DropElement.PlayOneShotOnCamera(null);
						}
						else if (num >= 0 && num != indexWithinGroup && num != indexWithinGroup + 1)
						{
							SoundDefOf.DropElement.PlayOneShotOnCamera(null);
							try
							{
								ReorderableWidget.groups[ReorderableWidget.reorderables[ReorderableWidget.draggingReorderable].groupID].reorderedAction(indexWithinGroup, num);
							}
							catch (Exception ex)
							{
								string[] array = new string[6];
								array[0] = "Could not reorder elements (from ";
								array[1] = indexWithinGroup.ToString();
								array[2] = " to ";
								array[3] = num.ToString();
								array[4] = "): ";
								int num3 = 5;
								Exception ex2 = ex;
								array[num3] = ((ex2 != null) ? ex2.ToString() : null);
								Log.Error(string.Concat(array));
							}
						}
					}
					ReorderableWidget.StopDragging();
				}
				ReorderableWidget.lastFrameReorderableCount = ReorderableWidget.reorderables.Count;
				ReorderableWidget.multiGroups.Clear();
				ReorderableWidget.groups.Clear();
				ReorderableWidget.reorderables.Clear();
			}
		}

		// Token: 0x060032BA RID: 12986 RVA: 0x00143924 File Offset: 0x00141B24
		public static int NewGroup(Action<int, int> reorderedAction, ReorderableDirection direction, Rect rect, float drawLineExactlyBetween_space = -1f, Action<int, Vector2> extraDraggedItemOnGUI = null, bool playSoundOnStartReorder = true)
		{
			if (Event.current.type != EventType.repaint)
			{
				return -1;
			}
			int count = ReorderableWidget.groups.Count;
			ReorderableWidget.ReorderableGroup item = default(ReorderableWidget.ReorderableGroup);
			item.reorderedAction = reorderedAction;
			item.direction = direction;
			item.absRect = new Rect(UI.GUIToScreenPoint(Vector2.zero), rect.size);
			item.drawLineExactlyBetween_space = drawLineExactlyBetween_space;
			item.extraDraggedItemOnGUI = extraDraggedItemOnGUI;
			item.playSoundOnStartReorder = playSoundOnStartReorder;
			ReorderableWidget.groups.Add(item);
			if (ReorderableWidget.draggingReorderable >= 0 && ReorderableWidget.hoveredGroup == count && ReorderableWidget.lastInsertNear == -1)
			{
				ReorderableWidget.DrawLine(count, new Rect(0f, 0f, rect.width, rect.height));
			}
			return count;
		}

		// Token: 0x060032BB RID: 12987 RVA: 0x001439E4 File Offset: 0x00141BE4
		public static int NewMultiGroup(List<int> includedGroups, Action<int, int, int, int> reorderedAction)
		{
			if (Event.current.type != EventType.repaint)
			{
				return -1;
			}
			ReorderableWidget.ReorderableMultiGroup item = default(ReorderableWidget.ReorderableMultiGroup);
			item.includedGroups = includedGroups;
			item.reorderedAction = reorderedAction;
			ReorderableWidget.multiGroups.Add(item);
			return ReorderableWidget.multiGroups.Count - 1;
		}

		// Token: 0x060032BC RID: 12988 RVA: 0x00143A30 File Offset: 0x00141C30
		public static bool Reorderable(int groupID, Rect rect, bool useRightButton = false, bool highlightDragged = true)
		{
			if (Event.current.type == EventType.repaint)
			{
				ReorderableWidget.ReorderableInstance item = default(ReorderableWidget.ReorderableInstance);
				item.groupID = groupID;
				item.rect = rect;
				item.absRect = new Rect(UI.GUIToScreenPoint(rect.position), rect.size);
				ReorderableWidget.reorderables.Add(item);
				int num = ReorderableWidget.reorderables.Count - 1;
				if (ReorderableWidget.draggingReorderable != -1 && (ReorderableWidget.dragBegun || (Vector2.Distance(ReorderableWidget.clickedAt, Event.current.mousePosition) > 5f && ReorderableWidget.groupClicked == groupID)))
				{
					if (!ReorderableWidget.dragBegun)
					{
						if (groupID >= 0 && groupID < ReorderableWidget.groups.Count && ReorderableWidget.groups[groupID].playSoundOnStartReorder)
						{
							SoundDefOf.DragElement.PlayOneShotOnCamera(null);
						}
						ReorderableWidget.dragBegun = true;
					}
					if (highlightDragged && ReorderableWidget.draggingReorderable == num)
					{
						GUI.color = ReorderableWidget.HighlightColor;
						Widgets.DrawHighlight(rect);
						GUI.color = Color.white;
					}
					if (ReorderableWidget.lastInsertNear == num && groupID >= 0 && groupID < ReorderableWidget.groups.Count)
					{
						Rect rect2 = ReorderableWidget.reorderables[ReorderableWidget.lastInsertNear].rect;
						ReorderableWidget.DrawLine(groupID, rect2);
					}
				}
				return ReorderableWidget.draggingReorderable == num && ReorderableWidget.dragBegun;
			}
			if (Event.current.rawType == EventType.mouseUp)
			{
				ReorderableWidget.released = true;
			}
			if (Event.current.type == EventType.mouseDown && ((useRightButton && Event.current.button == 1) || (!useRightButton && Event.current.button == 0)) && Mouse.IsOver(rect))
			{
				ReorderableWidget.clicked = true;
				ReorderableWidget.clickedAt = Event.current.mousePosition;
				ReorderableWidget.groupClicked = groupID;
				ReorderableWidget.clickedInRect = rect;
			}
			return false;
		}

		// Token: 0x060032BD RID: 12989 RVA: 0x00143BE4 File Offset: 0x00141DE4
		private static int CurrentInsertNear(out bool toTheLeft)
		{
			toTheLeft = false;
			if (ReorderableWidget.draggingReorderable < 0)
			{
				return -1;
			}
			int groupID = ReorderableWidget.reorderables[ReorderableWidget.draggingReorderable].groupID;
			if (groupID < 0 || groupID >= ReorderableWidget.groups.Count)
			{
				Log.ErrorOnce("Reorderable used invalid group.", 1968375560);
				return -1;
			}
			int num = -1;
			for (int i = 0; i < ReorderableWidget.reorderables.Count; i++)
			{
				ReorderableWidget.ReorderableInstance reorderableInstance = ReorderableWidget.reorderables[i];
				if ((reorderableInstance.groupID == groupID || ReorderableWidget.AreInMultiGroup(reorderableInstance.groupID, groupID)) && (num == -1 || Event.current.mousePosition.DistanceToRect(reorderableInstance.absRect) < Event.current.mousePosition.DistanceToRect(ReorderableWidget.reorderables[num].absRect)))
				{
					num = i;
				}
			}
			if (num >= 0)
			{
				ReorderableWidget.ReorderableInstance reorderableInstance2 = ReorderableWidget.reorderables[num];
				if (ReorderableWidget.groups[reorderableInstance2.groupID].direction == ReorderableDirection.Horizontal)
				{
					toTheLeft = (Event.current.mousePosition.x < reorderableInstance2.absRect.center.x);
				}
				else
				{
					toTheLeft = (Event.current.mousePosition.y < reorderableInstance2.absRect.center.y);
				}
			}
			return num;
		}

		// Token: 0x060032BE RID: 12990 RVA: 0x00143D20 File Offset: 0x00141F20
		private static void DrawLine(int groupID, Rect r)
		{
			ReorderableWidget.ReorderableGroup reorderableGroup = ReorderableWidget.groups[groupID];
			if (reorderableGroup.DrawLineExactlyBetween)
			{
				if (reorderableGroup.direction == ReorderableDirection.Horizontal)
				{
					r.xMin -= reorderableGroup.drawLineExactlyBetween_space / 2f;
					r.xMax += reorderableGroup.drawLineExactlyBetween_space / 2f;
				}
				else
				{
					r.yMin -= reorderableGroup.drawLineExactlyBetween_space / 2f;
					r.yMax += reorderableGroup.drawLineExactlyBetween_space / 2f;
				}
			}
			GUI.color = ReorderableWidget.LineColor;
			if (reorderableGroup.direction == ReorderableDirection.Horizontal)
			{
				if (ReorderableWidget.lastInsertNearLeft)
				{
					Widgets.DrawLine(r.position, new Vector2(r.x, r.yMax), ReorderableWidget.LineColor, 2f);
				}
				else
				{
					Widgets.DrawLine(new Vector2(r.xMax, r.y), new Vector2(r.xMax, r.yMax), ReorderableWidget.LineColor, 2f);
				}
			}
			else if (ReorderableWidget.lastInsertNearLeft)
			{
				Widgets.DrawLine(r.position, new Vector2(r.xMax, r.y), ReorderableWidget.LineColor, 2f);
			}
			else
			{
				Widgets.DrawLine(new Vector2(r.x, r.yMax), new Vector2(r.xMax, r.yMax), ReorderableWidget.LineColor, 2f);
			}
			GUI.color = Color.white;
		}

		// Token: 0x060032BF RID: 12991 RVA: 0x00143EA4 File Offset: 0x001420A4
		private static int GetIndexWithinGroup(int index)
		{
			if (index < 0 || index >= ReorderableWidget.reorderables.Count)
			{
				return -1;
			}
			int num = -1;
			for (int i = 0; i <= index; i++)
			{
				if (ReorderableWidget.reorderables[i].groupID == ReorderableWidget.reorderables[index].groupID)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060032C0 RID: 12992 RVA: 0x00143EFC File Offset: 0x001420FC
		private static int FindLastReorderableIndexWithinGroup(int groupID)
		{
			if (groupID < 0 || groupID >= ReorderableWidget.groups.Count)
			{
				return -1;
			}
			int result = -1;
			for (int i = 0; i < ReorderableWidget.reorderables.Count; i++)
			{
				if (ReorderableWidget.reorderables[i].groupID == groupID)
				{
					result = i;
				}
			}
			return result;
		}

		// Token: 0x060032C1 RID: 12993 RVA: 0x00143F4C File Offset: 0x0014214C
		private static ReorderableWidget.ReorderableMultiGroup? GetMultiGroupByGroupID(int groupID)
		{
			foreach (ReorderableWidget.ReorderableMultiGroup reorderableMultiGroup in ReorderableWidget.multiGroups)
			{
				if (reorderableMultiGroup.includedGroups.Contains(groupID))
				{
					return new ReorderableWidget.ReorderableMultiGroup?(reorderableMultiGroup);
				}
			}
			return null;
		}

		// Token: 0x060032C2 RID: 12994 RVA: 0x00143FBC File Offset: 0x001421BC
		private static bool AreInMultiGroup(int groupA, int groupB)
		{
			ReorderableWidget.ReorderableMultiGroup? multiGroupByGroupID = ReorderableWidget.GetMultiGroupByGroupID(groupA);
			return multiGroupByGroupID != null && groupA != groupB && multiGroupByGroupID.Value.includedGroups.Contains(groupB);
		}

		// Token: 0x060032C3 RID: 12995 RVA: 0x00143FF1 File Offset: 0x001421F1
		private static void StopDragging()
		{
			ReorderableWidget.draggingReorderable = -1;
			ReorderableWidget.dragStartPos = default(Vector2);
			ReorderableWidget.lastInsertNear = -1;
			ReorderableWidget.dragBegun = false;
		}

		// Token: 0x04002069 RID: 8297
		private static List<ReorderableWidget.ReorderableGroup> groups = new List<ReorderableWidget.ReorderableGroup>();

		// Token: 0x0400206A RID: 8298
		private static List<ReorderableWidget.ReorderableMultiGroup> multiGroups = new List<ReorderableWidget.ReorderableMultiGroup>();

		// Token: 0x0400206B RID: 8299
		private static List<ReorderableWidget.ReorderableInstance> reorderables = new List<ReorderableWidget.ReorderableInstance>();

		// Token: 0x0400206C RID: 8300
		private static int draggingReorderable = -1;

		// Token: 0x0400206D RID: 8301
		private static Vector2 dragStartPos;

		// Token: 0x0400206E RID: 8302
		private static bool clicked;

		// Token: 0x0400206F RID: 8303
		private static bool released;

		// Token: 0x04002070 RID: 8304
		private static bool dragBegun;

		// Token: 0x04002071 RID: 8305
		private static Vector2 clickedAt;

		// Token: 0x04002072 RID: 8306
		private static int groupClicked;

		// Token: 0x04002073 RID: 8307
		private static Rect clickedInRect;

		// Token: 0x04002074 RID: 8308
		private static int lastInsertNear = -1;

		// Token: 0x04002075 RID: 8309
		private static int hoveredGroup = -1;

		// Token: 0x04002076 RID: 8310
		private static bool lastInsertNearLeft;

		// Token: 0x04002077 RID: 8311
		private static int lastFrameReorderableCount = -1;

		// Token: 0x04002078 RID: 8312
		private const float MinMouseMoveToHighlightReorderable = 5f;

		// Token: 0x04002079 RID: 8313
		private static readonly Color LineColor = new Color(1f, 1f, 1f, 0.6f);

		// Token: 0x0400207A RID: 8314
		private static readonly Color HighlightColor = new Color(1f, 1f, 1f, 0.3f);

		// Token: 0x0400207B RID: 8315
		private const float LineWidth = 2f;

		// Token: 0x020029EF RID: 10735
		private struct ReorderableGroup
		{
			// Token: 0x17002BB6 RID: 11190
			// (get) Token: 0x060104F3 RID: 66803 RVA: 0x005C272F File Offset: 0x005C092F
			public bool DrawLineExactlyBetween
			{
				get
				{
					return this.drawLineExactlyBetween_space > 0f;
				}
			}

			// Token: 0x0400AC05 RID: 44037
			public Action<int, int> reorderedAction;

			// Token: 0x0400AC06 RID: 44038
			public ReorderableDirection direction;

			// Token: 0x0400AC07 RID: 44039
			public float drawLineExactlyBetween_space;

			// Token: 0x0400AC08 RID: 44040
			public Action<int, Vector2> extraDraggedItemOnGUI;

			// Token: 0x0400AC09 RID: 44041
			public Rect absRect;

			// Token: 0x0400AC0A RID: 44042
			public bool playSoundOnStartReorder;
		}

		// Token: 0x020029F0 RID: 10736
		private struct ReorderableMultiGroup
		{
			// Token: 0x0400AC0B RID: 44043
			public Action<int, int, int, int> reorderedAction;

			// Token: 0x0400AC0C RID: 44044
			public List<int> includedGroups;
		}

		// Token: 0x020029F1 RID: 10737
		private struct ReorderableInstance
		{
			// Token: 0x0400AC0D RID: 44045
			public int groupID;

			// Token: 0x0400AC0E RID: 44046
			public Rect rect;

			// Token: 0x0400AC0F RID: 44047
			public Rect absRect;
		}
	}
}
