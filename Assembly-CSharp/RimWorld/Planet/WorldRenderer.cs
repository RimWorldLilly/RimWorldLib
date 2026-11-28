using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200232D RID: 9005
	public class WorldRenderer
	{
		// Token: 0x17002701 RID: 9985
		// (get) Token: 0x0600DE67 RID: 56935 RVA: 0x0052B6D0 File Offset: 0x005298D0
		private bool ShouldRegenerateDirtyLayersInLongEvent
		{
			get
			{
				foreach (KeyValuePair<int, PlanetLayer> keyValuePair in Find.WorldGrid.PlanetLayers)
				{
					int num;
					PlanetLayer planetLayer;
					keyValuePair.Deconstruct(out num, out planetLayer);
					PlanetLayer planetLayer2 = planetLayer;
					if (planetLayer2.Visible)
					{
						foreach (WorldDrawLayer worldDrawLayer in planetLayer2.WorldDrawLayers)
						{
							if (worldDrawLayer.Dirty && worldDrawLayer is WorldDrawLayer_Terrain)
							{
								return true;
							}
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17002702 RID: 9986
		// (get) Token: 0x0600DE68 RID: 56936 RVA: 0x0052B788 File Offset: 0x00529988
		public IEnumerable<WorldDrawLayerBase> AllDrawLayers
		{
			get
			{
				return new WorldRenderer.<get_AllDrawLayers>d__5(-2);
			}
		}

		// Token: 0x17002703 RID: 9987
		// (get) Token: 0x0600DE69 RID: 56937 RVA: 0x0052B791 File Offset: 0x00529991
		public IEnumerable<WorldDrawLayerBase> AllVisibleDrawLayers
		{
			get
			{
				return new WorldRenderer.<get_AllVisibleDrawLayers>d__7(-2);
			}
		}

		// Token: 0x0600DE6A RID: 56938 RVA: 0x0052B79C File Offset: 0x0052999C
		public void SetAllLayersDirty()
		{
			foreach (WorldDrawLayerBase worldDrawLayerBase in this.AllDrawLayers)
			{
				worldDrawLayerBase.SetDirty();
			}
		}

		// Token: 0x0600DE6B RID: 56939 RVA: 0x0052B7E8 File Offset: 0x005299E8
		public void SetDirty<T>(PlanetLayer planetLayer) where T : WorldDrawLayer
		{
			foreach (WorldDrawLayerBase worldDrawLayerBase in this.AllDrawLayers)
			{
				WorldDrawLayer worldDrawLayer = worldDrawLayerBase as WorldDrawLayer;
				if (worldDrawLayer != null && planetLayer == worldDrawLayer.planetLayer && worldDrawLayerBase is T)
				{
					worldDrawLayerBase.SetDirty();
				}
			}
		}

		// Token: 0x0600DE6C RID: 56940 RVA: 0x0052B850 File Offset: 0x00529A50
		public bool HasGlobalLayer<T>() where T : WorldDrawLayer
		{
			T t;
			return this.TryGetGlobalLayer<T>(out t);
		}

		// Token: 0x0600DE6D RID: 56941 RVA: 0x0052B868 File Offset: 0x00529A68
		public bool HasLayer<T>(PlanetLayer planetLayer) where T : WorldDrawLayer
		{
			T t;
			return this.TryGetLayer<T>(planetLayer, out t);
		}

		// Token: 0x0600DE6E RID: 56942 RVA: 0x0052B87E File Offset: 0x00529A7E
		public bool TryGetGlobalLayer<T>(out T layer) where T : WorldDrawLayer
		{
			layer = this.GetGlobalLayer<T>();
			return layer != null;
		}

		// Token: 0x0600DE6F RID: 56943 RVA: 0x0052B89A File Offset: 0x00529A9A
		public bool TryGetLayer<T>(PlanetLayer planetLayer, out T layer) where T : WorldDrawLayer
		{
			layer = this.GetLayer<T>(planetLayer);
			return layer != null;
		}

		// Token: 0x0600DE70 RID: 56944 RVA: 0x0052B8B8 File Offset: 0x00529AB8
		public T GetGlobalLayer<T>() where T : WorldDrawLayer
		{
			foreach (WorldDrawLayerBase worldDrawLayerBase in Find.WorldGrid.GlobalLayers)
			{
				T t = worldDrawLayerBase as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x0600DE71 RID: 56945 RVA: 0x0052B924 File Offset: 0x00529B24
		public T GetLayer<T>(PlanetLayer planetLayer) where T : WorldDrawLayer
		{
			foreach (WorldDrawLayerBase worldDrawLayerBase in this.AllDrawLayers)
			{
				WorldDrawLayer worldDrawLayer = worldDrawLayerBase as WorldDrawLayer;
				if (worldDrawLayer != null && planetLayer == worldDrawLayer.planetLayer)
				{
					T t = worldDrawLayerBase as T;
					if (t != null)
					{
						return t;
					}
				}
			}
			return default(T);
		}

		// Token: 0x0600DE72 RID: 56946 RVA: 0x0052B9A4 File Offset: 0x00529BA4
		public void RegenerateAllLayersNow()
		{
			foreach (WorldDrawLayerBase worldDrawLayerBase in this.AllDrawLayers)
			{
				if (worldDrawLayerBase.Visible)
				{
					worldDrawLayerBase.RegenerateNow();
				}
			}
		}

		// Token: 0x0600DE73 RID: 56947 RVA: 0x0052B9F8 File Offset: 0x00529BF8
		private IEnumerable RegenerateDirtyLayersNow_Async()
		{
			WorldRenderer.<RegenerateDirtyLayersNow_Async>d__17 <RegenerateDirtyLayersNow_Async>d__ = new WorldRenderer.<RegenerateDirtyLayersNow_Async>d__17(-2);
			<RegenerateDirtyLayersNow_Async>d__.<>4__this = this;
			return <RegenerateDirtyLayersNow_Async>d__;
		}

		// Token: 0x0600DE74 RID: 56948 RVA: 0x0052BA08 File Offset: 0x00529C08
		public void Notify_StaticWorldObjectPosChanged()
		{
			foreach (WorldDrawLayerBase worldDrawLayerBase in this.AllDrawLayers)
			{
				WorldDrawLayer_WorldObjects worldDrawLayer_WorldObjects = worldDrawLayerBase as WorldDrawLayer_WorldObjects;
				if (worldDrawLayer_WorldObjects != null)
				{
					worldDrawLayer_WorldObjects.SetDirty();
				}
			}
		}

		// Token: 0x0600DE75 RID: 56949 RVA: 0x0052BA5C File Offset: 0x00529C5C
		public void Notify_TilePollutionChanged(PlanetTile tileId)
		{
			foreach (WorldDrawLayerBase worldDrawLayerBase in this.AllDrawLayers)
			{
				WorldDrawLayer_Pollution worldDrawLayer_Pollution = worldDrawLayerBase as WorldDrawLayer_Pollution;
				if (worldDrawLayer_Pollution != null)
				{
					worldDrawLayer_Pollution.Notify_TilePollutionChanged(tileId);
				}
			}
		}

		// Token: 0x0600DE76 RID: 56950 RVA: 0x0052BAB4 File Offset: 0x00529CB4
		public void CheckActivateWorldCamera()
		{
			Find.WorldCamera.gameObject.SetActive(WorldRendererUtility.WorldRendered);
		}

		// Token: 0x0600DE77 RID: 56951 RVA: 0x0052BACA File Offset: 0x00529CCA
		public bool RegenerateLayersIfDirtyInLongEvent()
		{
			if (this.ShouldRegenerateDirtyLayersInLongEvent)
			{
				this.asynchronousRegenerationActive = true;
				LongEventHandler.QueueLongEvent(this.RegenerateDirtyLayersNow_Async(), "GeneratingPlanet", null, false, false);
				return true;
			}
			return false;
		}

		// Token: 0x0600DE78 RID: 56952 RVA: 0x0052BAF4 File Offset: 0x00529CF4
		public void DrawWorldLayers()
		{
			if (this.asynchronousRegenerationActive)
			{
				Log.Error("Called DrawWorldLayers() but already regenerating. This shouldn't ever happen because LongEventHandler should have stopped us.");
				return;
			}
			if (this.RegenerateLayersIfDirtyInLongEvent())
			{
				return;
			}
			GlobalRendererUtility.UpdateGlobalShadersParams();
			WorldRendererUtility.UpdateGlobalShadersParams();
			foreach (WorldDrawLayerBase worldDrawLayerBase in Find.WorldGrid.GlobalLayers)
			{
				if (worldDrawLayerBase.Visible)
				{
					try
					{
						worldDrawLayerBase.Render();
					}
					catch (Exception value)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Error drawing global WorldLayer: ");
						defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
						Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
					}
				}
			}
			foreach (KeyValuePair<int, PlanetLayer> keyValuePair in Find.WorldGrid.PlanetLayers)
			{
				int num;
				PlanetLayer planetLayer;
				keyValuePair.Deconstruct(out num, out planetLayer);
				PlanetLayer planetLayer2 = planetLayer;
				if (planetLayer2.Visible)
				{
					foreach (WorldDrawLayer worldDrawLayer in planetLayer2.WorldDrawLayers)
					{
						try
						{
							worldDrawLayer.Render();
						}
						catch (Exception value2)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(50, 3);
							defaultInterpolatedStringHandler2.AppendLiteral("Error drawing planet layer WorldLayer ");
							defaultInterpolatedStringHandler2.AppendFormatted(worldDrawLayer.GetType().Name);
							defaultInterpolatedStringHandler2.AppendLiteral(": ");
							defaultInterpolatedStringHandler2.AppendFormatted<Exception>(value2);
							defaultInterpolatedStringHandler2.AppendLiteral(" on layer ");
							defaultInterpolatedStringHandler2.AppendFormatted(planetLayer2.Def.label);
							Log.Error(defaultInterpolatedStringHandler2.ToStringAndClear());
						}
					}
				}
			}
		}

		// Token: 0x0600DE79 RID: 56953 RVA: 0x0052BCD0 File Offset: 0x00529ED0
		public PlanetTile GetTileFromRayHit(RaycastHit hit)
		{
			foreach (WorldDrawLayerBase worldDrawLayerBase in this.AllVisibleDrawLayers)
			{
				WorldDrawLayer worldDrawLayer = worldDrawLayerBase as WorldDrawLayer;
				PlanetTile result;
				if (worldDrawLayer != null && worldDrawLayer.Raycastable && worldDrawLayer.TryGetTileFromRayHit(hit, out result))
				{
					return result;
				}
			}
			return PlanetTile.Invalid;
		}

		// Token: 0x04008EE0 RID: 36576
		public WorldRenderMode wantedMode;

		// Token: 0x04008EE1 RID: 36577
		private bool asynchronousRegenerationActive;
	}
}
