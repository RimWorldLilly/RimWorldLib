using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D89 RID: 7561
	[StaticConstructorOnStartup]
	public static class SelectionDrawerUtility
	{
		// Token: 0x0600C400 RID: 50176 RVA: 0x0047B6F4 File Offset: 0x004798F4
		public static void CalculateSelectionBracketPositionsUI<T>(Vector2[] bracketLocs, T obj, Rect rect, Dictionary<T, float> selectTimes, Vector2 textureSize, float jumpDistanceFactor = 1f)
		{
			float num;
			float num2;
			if (!selectTimes.TryGetValue(obj, out num))
			{
				num2 = 1f;
			}
			else
			{
				num2 = Mathf.Max(0f, 1f - (Time.realtimeSinceStartup - num) / 0.07f);
			}
			float num3 = num2 * 0.2f * jumpDistanceFactor;
			float num4 = 0.5f * (rect.width - textureSize.x) + num3;
			float num5 = 0.5f * (rect.height - textureSize.y) + num3;
			bracketLocs[0] = new Vector2(rect.center.x - num4, rect.center.y - num5);
			bracketLocs[1] = new Vector2(rect.center.x + num4, rect.center.y - num5);
			bracketLocs[2] = new Vector2(rect.center.x + num4, rect.center.y + num5);
			bracketLocs[3] = new Vector2(rect.center.x - num4, rect.center.y + num5);
		}

		// Token: 0x0600C401 RID: 50177 RVA: 0x0047B814 File Offset: 0x00479A14
		public static void CalculateSelectionBracketPositionsWorld<T>(Vector3[] bracketLocs, T obj, Vector3 worldPos, Vector2 worldSize, Dictionary<T, float> selectTimes, Vector2 textureSize, float jumpDistanceFactor = 1f, float deselectedJumpFactor = 1f)
		{
			float num;
			float num2;
			if (!selectTimes.TryGetValue(obj, out num))
			{
				num2 = deselectedJumpFactor;
			}
			else
			{
				num2 = Mathf.Max(0f, 1f - (Time.realtimeSinceStartup - num) / 0.07f);
			}
			float num3 = num2 * 0.2f * jumpDistanceFactor;
			Vector3 vector = worldPos;
			float num4 = 0.5f * (worldSize.x - textureSize.x) + num3;
			float num5 = 0.5f * (worldSize.y - textureSize.y) + num3;
			float y = AltitudeLayer.MetaOverlays.AltitudeFor();
			Thing thing = obj as Thing;
			if (thing != null)
			{
				ThingDef thingDef = GenConstruct.BuiltDefOf(thing.def) as ThingDef;
				if (((thingDef != null) ? thingDef.building : null) != null && thingDef.building.isAttachment)
				{
					vector += (thing.Rotation.AsVector2 * 0.5f).ToVector3();
				}
			}
			bracketLocs[0] = new Vector3(vector.x - num4, y, vector.z - num5);
			bracketLocs[1] = new Vector3(vector.x + num4, y, vector.z - num5);
			bracketLocs[2] = new Vector3(vector.x + num4, y, vector.z + num5);
			bracketLocs[3] = new Vector3(vector.x - num4, y, vector.z + num5);
		}

		// Token: 0x0600C402 RID: 50178 RVA: 0x0047B980 File Offset: 0x00479B80
		public static void DrawSelectionOverlayOnGUI(object target, Rect rect, float scale, float selectedTextJump)
		{
			Vector2 textureSize = new Vector2((float)SelectionDrawerUtility.SelectedTexGUI.width * scale, (float)SelectionDrawerUtility.SelectedTexGUI.height * scale);
			SelectionDrawerUtility.CalculateSelectionBracketPositionsUI<object>(SelectionDrawerUtility.bracketLocs, target, rect, SelectionDrawer.SelectTimes, textureSize, selectedTextJump);
			SelectionDrawerUtility.DrawSelectionOverlayOnGUI(SelectionDrawerUtility.bracketLocs, scale);
		}

		// Token: 0x0600C403 RID: 50179 RVA: 0x0047B9CC File Offset: 0x00479BCC
		public static void DrawSelectionOverlayOnGUI(Vector2[] bracketLocs, float selectedTexScale)
		{
			int num = 90;
			for (int i = 0; i < 4; i++)
			{
				Widgets.DrawTextureRotated(bracketLocs[i], SelectionDrawerUtility.SelectedTexGUI, (float)num, selectedTexScale, null);
				num += 90;
			}
		}

		// Token: 0x0600C404 RID: 50180 RVA: 0x0047BA02 File Offset: 0x00479C02
		public static void DrawSelectionOverlayWholeGUI(Rect rect)
		{
			GUI.DrawTexture(rect, TexUI.SelectionBracketWhole);
		}

		// Token: 0x0400700A RID: 28682
		private const float SelJumpDuration = 0.07f;

		// Token: 0x0400700B RID: 28683
		private const float SelJumpDistance = 0.2f;

		// Token: 0x0400700C RID: 28684
		public static readonly Texture2D SelectedTexGUI = ContentFinder<Texture2D>.Get("UI/Overlays/SelectionBracketGUI", true);

		// Token: 0x0400700D RID: 28685
		private static Vector2[] bracketLocs = new Vector2[4];
	}
}
