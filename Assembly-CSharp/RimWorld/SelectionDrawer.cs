using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D88 RID: 7560
	[StaticConstructorOnStartup]
	public static class SelectionDrawer
	{
		// Token: 0x1700245E RID: 9310
		// (get) Token: 0x0600C3F8 RID: 50168 RVA: 0x0047B2E3 File Offset: 0x004794E3
		public static Dictionary<object, float> SelectTimes
		{
			get
			{
				return SelectionDrawer.selectTimes;
			}
		}

		// Token: 0x0600C3F9 RID: 50169 RVA: 0x0047B2EA File Offset: 0x004794EA
		public static void Notify_Selected(object t)
		{
			SelectionDrawer.selectTimes[t] = Time.realtimeSinceStartup;
		}

		// Token: 0x0600C3FA RID: 50170 RVA: 0x0047B2FC File Offset: 0x004794FC
		public static void Clear()
		{
			SelectionDrawer.selectTimes.Clear();
		}

		// Token: 0x0600C3FB RID: 50171 RVA: 0x0047B308 File Offset: 0x00479508
		public static void Notify_DrawnStorageGroup(StorageGroup storageGroup)
		{
			SelectionDrawer.drawnStorageGroupBrackets.Add(storageGroup);
		}

		// Token: 0x0600C3FC RID: 50172 RVA: 0x0047B316 File Offset: 0x00479516
		public static bool DrawnStorageGroupThisFrame(StorageGroup storageGroup)
		{
			return SelectionDrawer.drawnStorageGroupBrackets.Contains(storageGroup);
		}

		// Token: 0x0600C3FD RID: 50173 RVA: 0x0047B324 File Offset: 0x00479524
		public static void DrawSelectionOverlays()
		{
			SelectionDrawer.drawnStorageGroupBrackets.Clear();
			if (Find.ScreenshotModeHandler.Active)
			{
				return;
			}
			foreach (object obj in Find.Selector.SelectedObjects)
			{
				SelectionDrawer.DrawSelectionBracketFor(obj, null);
				Thing thing = obj as Thing;
				if (thing != null)
				{
					thing.DrawExtraSelectionOverlays();
				}
			}
		}

		// Token: 0x0600C3FE RID: 50174 RVA: 0x0047B3A0 File Offset: 0x004795A0
		public static void DrawSelectionBracketFor(object obj, Material overrideMat = null)
		{
			Zone zone = obj as Zone;
			if (zone != null)
			{
				GenDraw.DrawFieldEdges(zone.Cells, 2900);
				return;
			}
			Plan plan = obj as Plan;
			if (plan != null)
			{
				GenDraw.DrawFieldEdges(plan.Cells, 3600);
				return;
			}
			Thing thing = obj as Thing;
			if (thing != null)
			{
				CellRect? customRectForSelector = thing.CustomRectForSelector;
				Vector3 vector = thing.DrawPos;
				if (customRectForSelector != null)
				{
					SelectionDrawerUtility.CalculateSelectionBracketPositionsWorld<object>(SelectionDrawer.bracketLocs, thing, customRectForSelector.Value.CenterVector3, new Vector2((float)customRectForSelector.Value.Width, (float)customRectForSelector.Value.Height), SelectionDrawer.selectTimes, Vector2.one, 1f, thing.def.deselectedSelectionBracketFactor);
				}
				else
				{
					Pawn pawn = thing.SpawnedParentOrMe as Pawn;
					if (pawn != null && pawn != thing)
					{
						vector = pawn.DrawPos;
						bool flag;
						PawnRenderUtility.CalculateCarriedDrawPos(pawn, thing, ref vector, out flag);
						SelectionDrawerUtility.CalculateSelectionBracketPositionsWorld<object>(SelectionDrawer.bracketLocs, thing, vector, thing.RotatedSize.ToVector2(), SelectionDrawer.selectTimes, Vector2.one, 1f, thing.def.deselectedSelectionBracketFactor);
					}
					else
					{
						Building_Enterable building_Enterable = thing.SpawnedParentOrMe as Building_Enterable;
						if (building_Enterable != null && building_Enterable != thing)
						{
							SelectionDrawerUtility.CalculateSelectionBracketPositionsWorld<object>(SelectionDrawer.bracketLocs, thing, building_Enterable.DrawPos + building_Enterable.PawnDrawOffset, thing.RotatedSize.ToVector2(), SelectionDrawer.selectTimes, Vector2.one, 1f, thing.def.deselectedSelectionBracketFactor);
						}
						else
						{
							if (thing.DrawPosHeld == null)
							{
								return;
							}
							vector = thing.DrawPosHeld.Value;
							SelectionDrawerUtility.CalculateSelectionBracketPositionsWorld<object>(SelectionDrawer.bracketLocs, thing, vector, thing.RotatedSize.ToVector2(), SelectionDrawer.selectTimes, Vector2.one, 1f, thing.def.deselectedSelectionBracketFactor);
						}
					}
				}
				float num = thing.MultipleItemsPerCellDrawn() ? 0.8f : 1f;
				float d = 1f;
				CameraDriver cameraDriver = Find.CameraDriver;
				float num2 = Mathf.Clamp01(Mathf.InverseLerp(cameraDriver.config.sizeRange.max * 0.84999996f, cameraDriver.config.sizeRange.max, cameraDriver.ZoomRootSize));
				if (thing is Pawn)
				{
					if (thing.def.Size == IntVec2.One)
					{
						num *= Mathf.Min(1f + num2 / 2f, 2f);
					}
					else
					{
						d = Mathf.Min(1f + num2 / 2f, 2f);
					}
				}
				int num3 = 0;
				for (int i = 0; i < 4; i++)
				{
					Quaternion q = Quaternion.AngleAxis((float)num3, Vector3.up);
					Vector3 pos = (SelectionDrawer.bracketLocs[i] - vector) * num + vector;
					Graphics.DrawMesh(MeshPool.plane10, Matrix4x4.TRS(pos, q, new Vector3(num, 1f, num) * d), overrideMat ?? SelectionDrawer.SelectionBracketMat, 0);
					num3 -= 90;
				}
			}
		}

		// Token: 0x04007006 RID: 28678
		private static Dictionary<object, float> selectTimes = new Dictionary<object, float>();

		// Token: 0x04007007 RID: 28679
		private static HashSet<StorageGroup> drawnStorageGroupBrackets = new HashSet<StorageGroup>();

		// Token: 0x04007008 RID: 28680
		private static readonly Material SelectionBracketMat = MaterialPool.MatFrom("UI/Overlays/SelectionBracket", ShaderDatabase.MetaOverlay);

		// Token: 0x04007009 RID: 28681
		private static Vector3[] bracketLocs = new Vector3[4];
	}
}
