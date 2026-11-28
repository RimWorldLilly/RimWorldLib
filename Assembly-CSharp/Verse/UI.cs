using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200008A RID: 138
	public static class UI
	{
		// Token: 0x170000DE RID: 222
		// (get) Token: 0x0600063F RID: 1599 RVA: 0x0002520C File Offset: 0x0002340C
		public static Vector2 MousePositionOnUI
		{
			get
			{
				return Input.mousePosition / Prefs.UIScale;
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000640 RID: 1600 RVA: 0x00025224 File Offset: 0x00023424
		public static Vector2 MousePositionOnUIInverted
		{
			get
			{
				Vector2 mousePositionOnUI = UI.MousePositionOnUI;
				mousePositionOnUI.y = (float)UI.screenHeight - mousePositionOnUI.y;
				return mousePositionOnUI;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000641 RID: 1601 RVA: 0x0002524C File Offset: 0x0002344C
		public static Vector2 MousePosUIInvertedUseEventIfCan
		{
			get
			{
				if (Event.current != null)
				{
					return UI.GUIToScreenPoint(Event.current.mousePosition);
				}
				return UI.MousePositionOnUIInverted;
			}
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x0002526C File Offset: 0x0002346C
		public static void ApplyUIScale()
		{
			if (Prefs.UIScale != UI.lastUIScale)
			{
				UI.lastUIScale = Prefs.UIScale;
				GenUI.ClearLabelWidthCache();
			}
			if (Prefs.UIScale == 1f)
			{
				UI.screenWidth = Screen.width;
				UI.screenHeight = Screen.height;
				return;
			}
			UI.screenWidth = Mathf.RoundToInt((float)Screen.width / Prefs.UIScale);
			UI.screenHeight = Mathf.RoundToInt((float)Screen.height / Prefs.UIScale);
			float uiscale = Prefs.UIScale;
			float uiscale2 = Prefs.UIScale;
			GUI.matrix = Matrix4x4.TRS(new Vector3(0f, 0f, 0f), Quaternion.identity, new Vector3(uiscale, uiscale2, 1f));
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x0002531C File Offset: 0x0002351C
		public static void FocusControl(string controlName, Window window)
		{
			GUI.FocusControl(controlName);
			Find.WindowStack.Notify_ManuallySetFocus(window);
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x0002532F File Offset: 0x0002352F
		public static void UnfocusCurrentControl()
		{
			GUI.FocusControl(null);
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x00025338 File Offset: 0x00023538
		public static void UnfocusCurrentTextField()
		{
			GUI.SetNextControlName("FOR_UNFOCUS");
			GUI.TextField(default(Rect), "");
			GUI.FocusControl("FOR_UNFOCUS");
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x0002536D File Offset: 0x0002356D
		public static Vector2 GUIToScreenPoint(Vector2 guiPoint)
		{
			return GUIUtility.GUIToScreenPoint(guiPoint / Prefs.UIScale);
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x00025380 File Offset: 0x00023580
		public static Rect GUIToScreenRect(Rect guiRect)
		{
			return new Rect
			{
				min = UI.GUIToScreenPoint(guiRect.min),
				max = UI.GUIToScreenPoint(guiRect.max)
			};
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x000253BC File Offset: 0x000235BC
		public static void RotateAroundPivot(float angle, Vector2 center)
		{
			GUIUtility.RotateAroundPivot(angle, center * Prefs.UIScale);
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x000253D0 File Offset: 0x000235D0
		public static Vector2 MapToUIPosition(this Vector3 v)
		{
			Vector3 vector = Find.Camera.WorldToScreenPoint(v) / Prefs.UIScale;
			return new Vector2(vector.x, (float)UI.screenHeight - vector.y);
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x0002540B File Offset: 0x0002360B
		public static Vector3 UIToMapPosition(float x, float y)
		{
			return UI.UIToMapPosition(new Vector2(x, y));
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x0002541C File Offset: 0x0002361C
		public static Vector3 UIToMapPosition(Vector2 screenLoc)
		{
			Ray ray = Find.Camera.ScreenPointToRay(screenLoc * Prefs.UIScale);
			return new Vector3(ray.origin.x, 0f, ray.origin.z);
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x00025466 File Offset: 0x00023666
		public static float CurUICellSize()
		{
			return (new Vector3(1f, 0f, 0f).MapToUIPosition() - new Vector3(0f, 0f, 0f).MapToUIPosition()).x;
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x000254A4 File Offset: 0x000236A4
		public static Vector3 MouseMapPosition()
		{
			return UI.UIToMapPosition(UI.MousePositionOnUI);
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x000254B0 File Offset: 0x000236B0
		public static IntVec3 MouseCell()
		{
			return UI.UIToMapPosition(UI.MousePositionOnUI).ToIntVec3();
		}

		// Token: 0x0400020B RID: 523
		public static int screenWidth;

		// Token: 0x0400020C RID: 524
		public static int screenHeight;

		// Token: 0x0400020D RID: 525
		private static float lastUIScale;
	}
}
