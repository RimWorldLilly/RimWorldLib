using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200235A RID: 9050
	[StaticConstructorOnStartup]
	public static class ExpandableLandmarksUtility
	{
		// Token: 0x1700276A RID: 10090
		// (get) Token: 0x0600E04C RID: 57420 RVA: 0x00536D68 File Offset: 0x00534F68
		private static Texture2D LandmarkTex
		{
			get
			{
				Texture2D result;
				if ((result = ExpandableLandmarksUtility.landmarkTex) == null)
				{
					result = (ExpandableLandmarksUtility.landmarkTex = ContentFinder<Texture2D>.Get("World/WorldObjects/Expanding/LandmarkExpanding", true));
				}
				return result;
			}
		}

		// Token: 0x1700276B RID: 10091
		// (get) Token: 0x0600E04D RID: 57421 RVA: 0x00536D84 File Offset: 0x00534F84
		private static float TransitionPct
		{
			get
			{
				if (!Find.PlaySettings.showExpandingLandmarks)
				{
					return 0f;
				}
				return ExpandableLandmarksUtility.transitionPct;
			}
		}

		// Token: 0x1700276C RID: 10092
		// (get) Token: 0x0600E04E RID: 57422 RVA: 0x00536DA0 File Offset: 0x00534FA0
		private static List<PlanetTile> LandmarksToShow
		{
			get
			{
				if (!ExpandableLandmarksUtility.cachedLandmarksToShow.Empty<PlanetTile>())
				{
					return ExpandableLandmarksUtility.cachedLandmarksToShow;
				}
				foreach (PlanetTile planetTile in Find.World.landmarks.landmarks.Keys)
				{
					if (!Find.WorldObjects.AnyWorldObjectAt(planetTile))
					{
						ExpandableLandmarksUtility.cachedLandmarksToShow.Add(planetTile);
					}
				}
				return ExpandableLandmarksUtility.cachedLandmarksToShow;
			}
		}

		// Token: 0x0600E04F RID: 57423 RVA: 0x00536E2C File Offset: 0x0053502C
		public static void ResetStaticData()
		{
			ExpandableLandmarksUtility.landmarkTex = null;
			ExpandableLandmarksUtility.cachedLandmarksToShow.Clear();
		}

		// Token: 0x0600E050 RID: 57424 RVA: 0x00536E40 File Offset: 0x00535040
		public static void ExpandableLandmarksUpdate()
		{
			float num = Time.deltaTime * 3f;
			if (Find.WorldCameraDriver.CurrentZoom <= WorldCameraZoomRange.VeryClose)
			{
				ExpandableLandmarksUtility.transitionPct -= num;
			}
			else
			{
				ExpandableLandmarksUtility.transitionPct += num;
			}
			ExpandableLandmarksUtility.transitionPct = Mathf.Clamp01(ExpandableLandmarksUtility.transitionPct);
		}

		// Token: 0x0600E051 RID: 57425 RVA: 0x00536E90 File Offset: 0x00535090
		public static void ExpandableLandmarksOnGUI()
		{
			foreach (PlanetTile planetTile in ExpandableLandmarksUtility.LandmarksToShow)
			{
				try
				{
					bool flag = ExpandableLandmarksUtility.IsHighlighted(planetTile);
					if (ExpandableLandmarksUtility.TransitionPct != 0f || flag)
					{
						if (!ExpandableLandmarksUtility.HiddenBehindTerrainNow(planetTile))
						{
							Color white = Color.white;
							white.a = (flag ? 1f : ExpandableLandmarksUtility.transitionPct);
							GUI.color = white;
							Rect rect = ExpandableLandmarksUtility.ExpandedIconScreenRect(planetTile);
							Material material = null;
							if (flag)
							{
								material = ExpandableLandmarksUtility.GetMaterial(white);
							}
							Widgets.DrawTextureRotated(rect, ExpandableLandmarksUtility.LandmarkTex, 0f, material);
						}
					}
				}
				catch (Exception value)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Error while drawing landmark at ");
					defaultInterpolatedStringHandler.AppendFormatted(planetTile.ToStringSafe<PlanetTile>());
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			GUI.color = Color.white;
		}

		// Token: 0x0600E052 RID: 57426 RVA: 0x00536FB0 File Offset: 0x005351B0
		private static bool IsHighlighted(PlanetTile tile)
		{
			Dialog_WorldSearch dialog_WorldSearch;
			return Find.WindowStack.TryGetWindow<Dialog_WorldSearch>(out dialog_WorldSearch) && ((dialog_WorldSearch.Highlighted != null && dialog_WorldSearch.Highlighted.tile == tile) || (dialog_WorldSearch.CommonSearchWidget.filter.Text.Length >= 1 && dialog_WorldSearch.IsListed(tile)));
		}

		// Token: 0x0600E053 RID: 57427 RVA: 0x0053700C File Offset: 0x0053520C
		private static Material GetMaterial(Color color)
		{
			return MaterialPool.MatFrom(new MaterialRequest(null, ShaderDatabase.ExpandingIconUI, color)
			{
				needsMainTex = false,
				colorTwo = ExpandableLandmarksUtility.HighlightedColor
			});
		}

		// Token: 0x0600E054 RID: 57428 RVA: 0x00537044 File Offset: 0x00535244
		private static Rect ExpandedIconScreenRect(PlanetTile tile)
		{
			Vector2 vector = GenWorldUI.WorldToUIPosition(ExpandableLandmarksUtility.DrawPos(tile));
			bool flag = tile.Layer != Find.WorldSelector.SelectedLayer;
			float num = 30f;
			if (flag)
			{
				num /= 2f;
			}
			return new Rect(vector.x - num / 2f, vector.y - num / 2f, num, num);
		}

		// Token: 0x0600E055 RID: 57429 RVA: 0x005370A6 File Offset: 0x005352A6
		private static bool HiddenBehindTerrainNow(PlanetTile tile)
		{
			return WorldRendererUtility.HiddenBehindTerrainNow(ExpandableLandmarksUtility.DrawPos(tile)) || tile.Layer != PlanetLayer.Selected;
		}

		// Token: 0x0600E056 RID: 57430 RVA: 0x005370C8 File Offset: 0x005352C8
		private static Vector3 DrawPos(PlanetTile tile)
		{
			return tile.Layer.Origin + Find.WorldGrid.GetTileCenter(tile);
		}

		// Token: 0x0600E057 RID: 57431 RVA: 0x005370E6 File Offset: 0x005352E6
		public static void Notify_WorldObjectsChanged()
		{
			ExpandableLandmarksUtility.cachedLandmarksToShow.Clear();
		}

		// Token: 0x04008FF6 RID: 36854
		private static float transitionPct;

		// Token: 0x04008FF7 RID: 36855
		private static List<PlanetTile> cachedLandmarksToShow = new List<PlanetTile>();

		// Token: 0x04008FF8 RID: 36856
		private const float LandmarkIconSize = 30f;

		// Token: 0x04008FF9 RID: 36857
		private const float TransitionSpeed = 3f;

		// Token: 0x04008FFA RID: 36858
		private static readonly Color HighlightedColor = ExpandableWorldObjectsUtility.HighlightedColor;

		// Token: 0x04008FFB RID: 36859
		private static Texture2D landmarkTex;
	}
}
