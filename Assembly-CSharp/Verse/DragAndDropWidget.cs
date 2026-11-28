using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000645 RID: 1605
	public static class DragAndDropWidget
	{
		// Token: 0x1700099E RID: 2462
		// (get) Token: 0x06003243 RID: 12867 RVA: 0x0014074D File Offset: 0x0013E94D
		public static bool Dragging
		{
			get
			{
				return DragAndDropWidget.dragBegun;
			}
		}

		// Token: 0x06003244 RID: 12868 RVA: 0x00140754 File Offset: 0x0013E954
		public static void DragAndDropWidgetOnGUI_BeforeWindowStack()
		{
			if (DragAndDropWidget.dragBegun && DragAndDropWidget.draggingDraggable >= 0 && DragAndDropWidget.draggingDraggable < DragAndDropWidget.draggables.Count)
			{
				int groupID = DragAndDropWidget.draggables[DragAndDropWidget.draggingDraggable].groupID;
				if (groupID >= 0 && groupID < DragAndDropWidget.groups.Count && DragAndDropWidget.groups[groupID].extraDraggedItemOnGUI != null)
				{
					DragAndDropWidget.groups[groupID].extraDraggedItemOnGUI(DragAndDropWidget.draggables[DragAndDropWidget.draggingDraggable].context, DragAndDropWidget.dragStartPos);
				}
			}
		}

		// Token: 0x06003245 RID: 12869 RVA: 0x001407E8 File Offset: 0x0013E9E8
		public static void DragAndDropWidgetOnGUI_AfterWindowStack()
		{
			if (Event.current.rawType == EventType.mouseUp)
			{
				DragAndDropWidget.released = true;
			}
			if (Event.current.type == EventType.repaint)
			{
				if (DragAndDropWidget.clicked)
				{
					DragAndDropWidget.StopDragging();
					for (int i = 0; i < DragAndDropWidget.draggables.Count; i++)
					{
						if (DragAndDropWidget.draggables[i].rect == DragAndDropWidget.clickedInRect)
						{
							DragAndDropWidget.draggingDraggable = i;
							Action onStartDragging = DragAndDropWidget.draggables[i].onStartDragging;
							if (onStartDragging != null)
							{
								onStartDragging();
							}
							DragAndDropWidget.dragStartPos = Event.current.mousePosition;
							break;
						}
					}
					DragAndDropWidget.mouseIsDown = true;
					DragAndDropWidget.clicked = false;
				}
				if (DragAndDropWidget.draggingDraggable >= DragAndDropWidget.draggables.Count)
				{
					DragAndDropWidget.StopDragging();
				}
				if (DragAndDropWidget.draggables.Count != DragAndDropWidget.lastFrameDraggableCount)
				{
					DragAndDropWidget.StopDragging();
				}
				if (DragAndDropWidget.released)
				{
					DragAndDropWidget.released = false;
					if (!DragAndDropWidget.dragBegun && DragAndDropWidget.mouseIsDown)
					{
						foreach (DragAndDropWidget.DraggableInstance draggableInstance in DragAndDropWidget.draggables)
						{
							Rect absRect = draggableInstance.absRect;
							if (absRect.Contains(Event.current.mousePosition) && draggableInstance.clickHandler != null)
							{
								draggableInstance.clickHandler();
							}
						}
					}
					DragAndDropWidget.mouseIsDown = false;
					if (DragAndDropWidget.dragBegun && DragAndDropWidget.draggingDraggable >= 0)
					{
						DragAndDropWidget.DraggableInstance draggableInstance2 = DragAndDropWidget.draggables[DragAndDropWidget.draggingDraggable];
						DragAndDropWidget.DropAreaInstance? dropAreaInstance = null;
						for (int j = DragAndDropWidget.dropAreas.Count - 1; j >= 0; j--)
						{
							DragAndDropWidget.DropAreaInstance dropAreaInstance2 = DragAndDropWidget.dropAreas[j];
							if (draggableInstance2.groupID == dropAreaInstance2.groupID && dropAreaInstance2.absRect.Contains(Event.current.mousePosition))
							{
								dropAreaInstance = new DragAndDropWidget.DropAreaInstance?(dropAreaInstance2);
							}
						}
						if (dropAreaInstance != null)
						{
							Action<object> onDrop = dropAreaInstance.Value.onDrop;
							if (onDrop != null)
							{
								onDrop(draggableInstance2.context);
							}
						}
						else
						{
							SoundDefOf.ClickReject.PlayOneShotOnCamera(null);
						}
					}
					DragAndDropWidget.StopDragging();
				}
				DragAndDropWidget.lastFrameDraggableCount = DragAndDropWidget.draggables.Count;
				DragAndDropWidget.groups.Clear();
				DragAndDropWidget.draggables.Clear();
				DragAndDropWidget.dropAreas.Clear();
			}
		}

		// Token: 0x06003246 RID: 12870 RVA: 0x00140A3C File Offset: 0x0013EC3C
		public static int NewGroup(Action<object, Vector2> extraDraggedItemOnGUI = null)
		{
			if (Event.current.type != EventType.repaint)
			{
				return -1;
			}
			DragAndDropWidget.DraggableGroup item = default(DragAndDropWidget.DraggableGroup);
			item.extraDraggedItemOnGUI = extraDraggedItemOnGUI;
			DragAndDropWidget.groups.Add(item);
			return DragAndDropWidget.groups.Count - 1;
		}

		// Token: 0x06003247 RID: 12871 RVA: 0x00140A80 File Offset: 0x0013EC80
		public static bool Draggable(int groupID, Rect rect, object context, Action clickHandler = null, Action onStartDragging = null)
		{
			if (Event.current.type == EventType.repaint)
			{
				DragAndDropWidget.DraggableInstance item = default(DragAndDropWidget.DraggableInstance);
				item.groupID = groupID;
				item.rect = rect;
				item.context = context;
				item.clickHandler = clickHandler;
				item.onStartDragging = onStartDragging;
				item.absRect = new Rect(UI.GUIToScreenPoint(rect.position), rect.size);
				DragAndDropWidget.draggables.Add(item);
				int num = DragAndDropWidget.draggables.Count - 1;
				if (DragAndDropWidget.draggingDraggable != -1 && (DragAndDropWidget.dragBegun || Vector2.Distance(DragAndDropWidget.clickedAt, Event.current.mousePosition) > 5f))
				{
					if (!DragAndDropWidget.dragBegun)
					{
						SoundDefOf.DragElement.PlayOneShotOnCamera(null);
						DragAndDropWidget.dragBegun = true;
					}
					if (DragAndDropWidget.draggingDraggable == num)
					{
						GUI.color = DragAndDropWidget.HighlightColor;
						Widgets.DrawHighlight(rect);
						GUI.color = Color.white;
					}
				}
				return DragAndDropWidget.draggingDraggable == num && DragAndDropWidget.dragBegun;
			}
			if (Event.current.rawType == EventType.mouseUp)
			{
				DragAndDropWidget.released = true;
			}
			if (Event.current.type == EventType.mouseDown && Event.current.button == 0 && Mouse.IsOver(rect))
			{
				DragAndDropWidget.clicked = true;
				DragAndDropWidget.clickedAt = Event.current.mousePosition;
				DragAndDropWidget.clickedInRect = rect;
			}
			return false;
		}

		// Token: 0x06003248 RID: 12872 RVA: 0x00140BC8 File Offset: 0x0013EDC8
		public static void DropArea(int groupID, Rect rect, Action<object> onDrop, object context)
		{
			if (Event.current.type == EventType.repaint)
			{
				DragAndDropWidget.DropAreaInstance item = default(DragAndDropWidget.DropAreaInstance);
				item.groupID = groupID;
				item.rect = rect;
				item.onDrop = onDrop;
				item.absRect = new Rect(UI.GUIToScreenPoint(rect.position), rect.size);
				item.context = context;
				DragAndDropWidget.dropAreas.Add(item);
			}
		}

		// Token: 0x06003249 RID: 12873 RVA: 0x00140C34 File Offset: 0x0013EE34
		public static object CurrentlyDraggedDraggable()
		{
			if (!DragAndDropWidget.dragBegun || DragAndDropWidget.draggingDraggable < 0)
			{
				return null;
			}
			return DragAndDropWidget.draggables[DragAndDropWidget.draggingDraggable].context;
		}

		// Token: 0x0600324A RID: 12874 RVA: 0x00140C5C File Offset: 0x0013EE5C
		public static object HoveringDropArea(int groupID)
		{
			DragAndDropWidget.DropAreaInstance? dropAreaInstance = null;
			for (int i = DragAndDropWidget.dropAreas.Count - 1; i >= 0; i--)
			{
				DragAndDropWidget.DropAreaInstance dropAreaInstance2 = DragAndDropWidget.dropAreas[i];
				if (groupID == dropAreaInstance2.groupID && dropAreaInstance2.rect.Contains(Event.current.mousePosition))
				{
					dropAreaInstance = new DragAndDropWidget.DropAreaInstance?(dropAreaInstance2);
				}
			}
			if (dropAreaInstance == null)
			{
				return null;
			}
			return dropAreaInstance.Value.context;
		}

		// Token: 0x0600324B RID: 12875 RVA: 0x00140CD4 File Offset: 0x0013EED4
		public static Rect? HoveringDropAreaRect(int groupID, Vector3? mousePos = null)
		{
			Vector3 point = mousePos ?? Event.current.mousePosition;
			DragAndDropWidget.DropAreaInstance? dropAreaInstance = null;
			for (int i = DragAndDropWidget.dropAreas.Count - 1; i >= 0; i--)
			{
				DragAndDropWidget.DropAreaInstance dropAreaInstance2 = DragAndDropWidget.dropAreas[i];
				if (groupID == dropAreaInstance2.groupID && dropAreaInstance2.rect.Contains(point))
				{
					dropAreaInstance = new DragAndDropWidget.DropAreaInstance?(dropAreaInstance2);
				}
			}
			if (dropAreaInstance == null)
			{
				return null;
			}
			return new Rect?(dropAreaInstance.GetValueOrDefault().rect);
		}

		// Token: 0x0600324C RID: 12876 RVA: 0x00140D78 File Offset: 0x0013EF78
		public static object DraggableAt(int groupID, Vector3 mousePos)
		{
			DragAndDropWidget.DraggableInstance? draggableInstance = null;
			for (int i = DragAndDropWidget.draggables.Count - 1; i >= 0; i--)
			{
				DragAndDropWidget.DraggableInstance draggableInstance2 = DragAndDropWidget.draggables[i];
				if (groupID == draggableInstance2.groupID && draggableInstance2.rect.Contains(mousePos))
				{
					draggableInstance = new DragAndDropWidget.DraggableInstance?(draggableInstance2);
				}
			}
			if (draggableInstance == null)
			{
				return null;
			}
			return draggableInstance.Value.context;
		}

		// Token: 0x0600324D RID: 12877 RVA: 0x00140DE8 File Offset: 0x0013EFE8
		private static object GetDraggable(int groupID, Vector3 mousePosAbs, int direction)
		{
			float num = float.PositiveInfinity;
			DragAndDropWidget.DraggableInstance? draggableInstance = null;
			for (int i = DragAndDropWidget.draggables.Count - 1; i >= 0; i--)
			{
				DragAndDropWidget.DraggableInstance draggableInstance2 = DragAndDropWidget.draggables[i];
				if (groupID == draggableInstance2.groupID)
				{
					Rect absRect = draggableInstance2.absRect;
					if (mousePosAbs.y >= absRect.yMin && mousePosAbs.y <= absRect.yMax)
					{
						float num2 = (mousePosAbs.x - absRect.xMax) * (float)direction;
						if (num2 >= 0f && num2 < num)
						{
							num = num2;
							draggableInstance = new DragAndDropWidget.DraggableInstance?(draggableInstance2);
						}
					}
				}
			}
			if (draggableInstance == null)
			{
				return null;
			}
			return draggableInstance.Value.context;
		}

		// Token: 0x0600324E RID: 12878 RVA: 0x00140E98 File Offset: 0x0013F098
		public static object GetDraggableBefore(int groupID, Vector3 mousePosAbs)
		{
			return DragAndDropWidget.GetDraggable(groupID, mousePosAbs, 1);
		}

		// Token: 0x0600324F RID: 12879 RVA: 0x00140EA2 File Offset: 0x0013F0A2
		public static object GetDraggableAfter(int groupID, Vector3 mousePosAbs)
		{
			return DragAndDropWidget.GetDraggable(groupID, mousePosAbs, -1);
		}

		// Token: 0x06003250 RID: 12880 RVA: 0x00140EAC File Offset: 0x0013F0AC
		private static void StopDragging()
		{
			DragAndDropWidget.draggingDraggable = -1;
			DragAndDropWidget.dragStartPos = default(Vector2);
			DragAndDropWidget.dragBegun = false;
		}

		// Token: 0x04002024 RID: 8228
		private static List<DragAndDropWidget.DraggableGroup> groups = new List<DragAndDropWidget.DraggableGroup>();

		// Token: 0x04002025 RID: 8229
		private static List<DragAndDropWidget.DropAreaInstance> dropAreas = new List<DragAndDropWidget.DropAreaInstance>();

		// Token: 0x04002026 RID: 8230
		private static List<DragAndDropWidget.DraggableInstance> draggables = new List<DragAndDropWidget.DraggableInstance>();

		// Token: 0x04002027 RID: 8231
		private static int draggingDraggable = -1;

		// Token: 0x04002028 RID: 8232
		private static Vector2 dragStartPos;

		// Token: 0x04002029 RID: 8233
		private static bool mouseIsDown;

		// Token: 0x0400202A RID: 8234
		private static bool clicked;

		// Token: 0x0400202B RID: 8235
		private static bool released;

		// Token: 0x0400202C RID: 8236
		private static bool dragBegun;

		// Token: 0x0400202D RID: 8237
		private static Vector2 clickedAt;

		// Token: 0x0400202E RID: 8238
		private static Rect clickedInRect;

		// Token: 0x0400202F RID: 8239
		private static int lastFrameDraggableCount = -1;

		// Token: 0x04002030 RID: 8240
		private const float MinMouseMoveToHighlightDraggable = 5f;

		// Token: 0x04002031 RID: 8241
		private static readonly Color LineColor = new Color(1f, 1f, 1f, 0.6f);

		// Token: 0x04002032 RID: 8242
		private static readonly Color HighlightColor = new Color(1f, 1f, 1f, 0.3f);

		// Token: 0x04002033 RID: 8243
		private const float LineWidth = 2f;

		// Token: 0x020029DF RID: 10719
		private struct DraggableGroup
		{
			// Token: 0x0400ABCF RID: 43983
			public Action<object, Vector2> extraDraggedItemOnGUI;
		}

		// Token: 0x020029E0 RID: 10720
		private struct DropAreaInstance
		{
			// Token: 0x0400ABD0 RID: 43984
			public int groupID;

			// Token: 0x0400ABD1 RID: 43985
			public Rect rect;

			// Token: 0x0400ABD2 RID: 43986
			public Rect absRect;

			// Token: 0x0400ABD3 RID: 43987
			public Action<object> onDrop;

			// Token: 0x0400ABD4 RID: 43988
			public object context;
		}

		// Token: 0x020029E1 RID: 10721
		private struct DraggableInstance
		{
			// Token: 0x0400ABD5 RID: 43989
			public int groupID;

			// Token: 0x0400ABD6 RID: 43990
			public Rect rect;

			// Token: 0x0400ABD7 RID: 43991
			public Rect absRect;

			// Token: 0x0400ABD8 RID: 43992
			public object context;

			// Token: 0x0400ABD9 RID: 43993
			public Action clickHandler;

			// Token: 0x0400ABDA RID: 43994
			public Action onStartDragging;
		}
	}
}
