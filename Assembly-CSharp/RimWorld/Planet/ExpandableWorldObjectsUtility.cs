using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200232A RID: 9002
	[StaticConstructorOnStartup]
	public static class ExpandableWorldObjectsUtility
	{
		// Token: 0x170026FE RID: 9982
		// (get) Token: 0x0600DE50 RID: 56912 RVA: 0x0052A9C9 File Offset: 0x00528BC9
		public static float RawTransitionPct
		{
			get
			{
				if (!Find.PlaySettings.showImportantExpandingIcons)
				{
					return 0f;
				}
				return ExpandableWorldObjectsUtility.transitionPct;
			}
		}

		// Token: 0x0600DE51 RID: 56913 RVA: 0x0052A9E4 File Offset: 0x00528BE4
		public static float TransitionPct(WorldObject wo)
		{
			if (ExpandableWorldObjectsUtility.HiddenByRules(wo))
			{
				return 0f;
			}
			if (wo.def.fullyExpandedInSpace && wo.Tile.LayerDef.isSpace && wo.Tile.Layer != Find.WorldSelector.SelectedLayer)
			{
				return 0.3f;
			}
			if (wo.def.fullyExpandedInSpace && wo.Tile.LayerDef.isSpace)
			{
				return 1f;
			}
			return ExpandableWorldObjectsUtility.transitionPct;
		}

		// Token: 0x0600DE52 RID: 56914 RVA: 0x0052AA70 File Offset: 0x00528C70
		public static bool HiddenByRules(WorldObject wo)
		{
			if (wo.Tile.LayerDef.isSpace)
			{
				return false;
			}
			bool flag = ExpandableWorldObjectsUtility.IsNonPlayerSettlement(wo);
			return (!Find.PlaySettings.showBasesExpandingIcons && flag) || (!Find.PlaySettings.showImportantExpandingIcons && !flag);
		}

		// Token: 0x0600DE53 RID: 56915 RVA: 0x0052AAC0 File Offset: 0x00528CC0
		private static bool IsNonPlayerSettlement(WorldObject wo)
		{
			Settlement settlement = wo as Settlement;
			return settlement != null && !settlement.Faction.IsPlayer && !settlement.HasMap;
		}

		// Token: 0x170026FF RID: 9983
		// (get) Token: 0x0600DE54 RID: 56916 RVA: 0x0052AAEF File Offset: 0x00528CEF
		public static float ExpandMoreTransitionPct
		{
			get
			{
				if (!Find.PlaySettings.showImportantExpandingIcons)
				{
					return 0f;
				}
				return ExpandableWorldObjectsUtility.expandMoreTransitionPct;
			}
		}

		// Token: 0x0600DE55 RID: 56917 RVA: 0x0052AB08 File Offset: 0x00528D08
		public static void ExpandableWorldObjectsUpdate()
		{
			float num = Time.deltaTime * 3f;
			if (Find.WorldCameraDriver.CurrentZoom <= WorldCameraZoomRange.VeryClose)
			{
				ExpandableWorldObjectsUtility.transitionPct -= num;
			}
			else
			{
				ExpandableWorldObjectsUtility.transitionPct += num;
			}
			ExpandableWorldObjectsUtility.transitionPct = Mathf.Clamp01(ExpandableWorldObjectsUtility.transitionPct);
			float num2 = Time.deltaTime * 4f;
			if (Find.WorldCameraDriver.CurrentZoom <= WorldCameraZoomRange.Far)
			{
				ExpandableWorldObjectsUtility.expandMoreTransitionPct -= num2;
			}
			else
			{
				ExpandableWorldObjectsUtility.expandMoreTransitionPct += num2;
			}
			ExpandableWorldObjectsUtility.expandMoreTransitionPct = Mathf.Clamp01(ExpandableWorldObjectsUtility.expandMoreTransitionPct);
		}

		// Token: 0x0600DE56 RID: 56918 RVA: 0x0052AB9C File Offset: 0x00528D9C
		public static void ExpandableWorldObjectsOnGUI()
		{
			if (!DebugViewSettings.drawWorldObjects)
			{
				return;
			}
			if (Event.current.type != EventType.repaint)
			{
				return;
			}
			ExpandableWorldObjectsUtility.tmpWorldObjects.Clear();
			ExpandableWorldObjectsUtility.tmpWorldObjects.AddRange(Find.WorldObjects.AllWorldObjects);
			ExpandableWorldObjectsUtility.SortByExpandingIconPriority(ExpandableWorldObjectsUtility.tmpWorldObjects);
			WorldTargeter worldTargeter = Find.WorldTargeter;
			List<WorldObject> worldObjectsUnderMouse = null;
			if (worldTargeter.IsTargeting)
			{
				worldObjectsUnderMouse = GenWorldUI.WorldObjectsUnderMouse(UI.MousePositionOnUI);
			}
			for (int i = 0; i < ExpandableWorldObjectsUtility.tmpWorldObjects.Count; i++)
			{
				try
				{
					WorldObject worldObject = ExpandableWorldObjectsUtility.tmpWorldObjects[i];
					if (worldObject.def.expandingIcon)
					{
						float num = ExpandableWorldObjectsUtility.TransitionPct(worldObject);
						bool flag = ExpandableWorldObjectsUtility.IsHighlighted(worldObject);
						if (num != 0f || flag)
						{
							if (!worldObject.HiddenBehindTerrainNow())
							{
								Material material = worldObject.ExpandingMaterial;
								Color expandingIconColor = worldObject.ExpandingIconColor;
								expandingIconColor.a = (flag ? 1f : num);
								if (worldTargeter.IsTargetedNow(worldObject, worldObjectsUnderMouse))
								{
									float num2 = GenMath.LerpDouble(-1f, 1f, 0.7f, 1f, Mathf.Sin(Time.time * 8f));
									expandingIconColor.r *= num2;
									expandingIconColor.g *= num2;
									expandingIconColor.b *= num2;
								}
								GUI.color = expandingIconColor;
								Rect rect = ExpandableWorldObjectsUtility.ExpandedIconScreenRect(worldObject, 1f);
								if (worldObject.ExpandingIconFlipHorizontal)
								{
									rect.x = rect.xMax;
									rect.width *= -1f;
								}
								if (material == null && flag)
								{
									material = ExpandableWorldObjectsUtility.GetMaterial(expandingIconColor, ExpandableWorldObjectsUtility.HighlightedColor);
								}
								else if (material == null)
								{
									MapParent mapParent = worldObject as MapParent;
									if (mapParent != null && mapParent.HasMap)
									{
										material = ExpandableWorldObjectsUtility.GetMaterial(expandingIconColor, ExpandableWorldObjectsUtility.HasMapColor);
									}
								}
								Widgets.DrawTextureRotated(rect, worldObject.ExpandingIcon, worldObject.ExpandingIconRotation, material);
							}
						}
					}
				}
				catch (Exception value)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Error while drawing ");
					defaultInterpolatedStringHandler.AppendFormatted(ExpandableWorldObjectsUtility.tmpWorldObjects[i].ToStringSafe<WorldObject>());
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			ExpandableWorldObjectsUtility.tmpWorldObjects.Clear();
			GUI.color = Color.white;
		}

		// Token: 0x0600DE57 RID: 56919 RVA: 0x0052AE0C File Offset: 0x0052900C
		private static bool IsHighlighted(WorldObject wo)
		{
			Dialog_WorldSearch dialog_WorldSearch;
			return Find.WindowStack.TryGetWindow<Dialog_WorldSearch>(out dialog_WorldSearch) && dialog_WorldSearch.CommonSearchWidget.filter.Text.Length >= 1 && dialog_WorldSearch.IsListed(wo);
		}

		// Token: 0x0600DE58 RID: 56920 RVA: 0x0052AE4C File Offset: 0x0052904C
		private static Material GetMaterial(Color color, Color highlight)
		{
			return MaterialPool.MatFrom(new MaterialRequest(null, ShaderDatabase.ExpandingIconUI, color)
			{
				needsMainTex = false,
				colorTwo = highlight
			});
		}

		// Token: 0x0600DE59 RID: 56921 RVA: 0x0052AE80 File Offset: 0x00529080
		public static Rect ExpandedIconScreenRect(WorldObject o, float factor = 1f)
		{
			Vector2 vector = o.ScreenPos();
			float num = 30f * o.def.expandingIconDrawSize;
			float num2;
			if (o.ExpandMore)
			{
				num2 = Mathf.Lerp(num, num * 1.35f, ExpandableWorldObjectsUtility.ExpandMoreTransitionPct);
			}
			else
			{
				num2 = num;
			}
			num2 *= factor;
			return new Rect(vector.x - num2 / 2f, vector.y - num2 / 2f, num2, num2);
		}

		// Token: 0x0600DE5A RID: 56922 RVA: 0x0052AEEC File Offset: 0x005290EC
		public static bool IsExpanded(WorldObject o)
		{
			return ExpandableWorldObjectsUtility.TransitionPct(o) > 0.5f && o.def.expandingIcon;
		}

		// Token: 0x0600DE5B RID: 56923 RVA: 0x0052AF08 File Offset: 0x00529108
		public static void GetExpandedWorldObjectUnderMouse(Vector2 mousePos, List<WorldObject> outList)
		{
			outList.Clear();
			Vector2 vector = mousePos;
			vector.y = (float)UI.screenHeight - vector.y;
			List<WorldObject> allWorldObjects = Find.WorldObjects.AllWorldObjects;
			for (int i = 0; i < allWorldObjects.Count; i++)
			{
				WorldObject worldObject = allWorldObjects[i];
				if (ExpandableWorldObjectsUtility.IsExpanded(worldObject) && ExpandableWorldObjectsUtility.ExpandedIconScreenRect(worldObject, 1f).Contains(vector) && !worldObject.HiddenBehindTerrainNow())
				{
					outList.Add(worldObject);
				}
			}
			ExpandableWorldObjectsUtility.SortByExpandingIconPriority(outList);
			outList.Reverse();
		}

		// Token: 0x0600DE5C RID: 56924 RVA: 0x0052AF90 File Offset: 0x00529190
		private static void SortByExpandingIconPriority(List<WorldObject> worldObjects)
		{
			Vector3 cameraPos = Find.WorldCameraDriver.CameraPosition;
			worldObjects.SortBy(delegate(WorldObject x)
			{
				float num = x.ExpandingIconPriority;
				if (x.Faction != null && x.Faction.IsPlayer)
				{
					num += 0.001f;
				}
				return num;
			}, (WorldObject x) => -(x.DrawPos - cameraPos).sqrMagnitude);
		}

		// Token: 0x04008EAF RID: 36527
		private static float transitionPct;

		// Token: 0x04008EB0 RID: 36528
		private static float expandMoreTransitionPct;

		// Token: 0x04008EB1 RID: 36529
		private static readonly List<WorldObject> tmpWorldObjects = new List<WorldObject>();

		// Token: 0x04008EB2 RID: 36530
		private const float WorldObjectIconSize = 30f;

		// Token: 0x04008EB3 RID: 36531
		private const float ExpandMoreWorldObjectIconSizeFactor = 1.35f;

		// Token: 0x04008EB4 RID: 36532
		private const float TransitionSpeed = 3f;

		// Token: 0x04008EB5 RID: 36533
		private const float ExpandMoreTransitionSpeed = 4f;

		// Token: 0x04008EB6 RID: 36534
		private const float BackgroundSpaceOpacity = 0.3f;

		// Token: 0x04008EB7 RID: 36535
		private static readonly Color HasMapColor = new Color(0.1f, 0.9f, 0.1f, 1f);

		// Token: 0x04008EB8 RID: 36536
		public static readonly Color HighlightedColor = new Color(0.9f, 0.8f, 0.1f, 1f);
	}
}
