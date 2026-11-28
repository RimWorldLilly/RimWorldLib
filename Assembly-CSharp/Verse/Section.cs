using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002AA RID: 682
	public class Section
	{
		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x060014F1 RID: 5361 RVA: 0x000790BD File Offset: 0x000772BD
		public CellRect Bounds
		{
			get
			{
				return this.bounds;
			}
		}

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x060014F2 RID: 5362 RVA: 0x000790C8 File Offset: 0x000772C8
		public CellRect CellRect
		{
			get
			{
				if (this.calculatedRect == default(CellRect))
				{
					this.calculatedRect = new CellRect(this.botLeft.x, this.botLeft.z, 17, 17);
					this.calculatedRect.ClipInsideMap(this.map);
				}
				return this.calculatedRect;
			}
		}

		// Token: 0x060014F3 RID: 5363 RVA: 0x00079128 File Offset: 0x00077328
		public Section(IntVec3 sectCoords, Map map)
		{
			this.botLeft = sectCoords * 17;
			this.map = map;
			bool flag = map.info.disableSunShadows || map.Biome.disableShadows;
			bool disableShadows = map.Biome.disableShadows;
			foreach (Type type in typeof(SectionLayer).AllSubclassesNonAbstract())
			{
				if ((!flag || !typeof(SectionLayer_SunShadows).IsAssignableFrom(type)) && (!disableShadows || !typeof(SectionLayer_EdgeShadows).IsAssignableFrom(type)) && (!(type == typeof(SectionLayer_PollutionCloud)) || ModsConfig.BiotechActive))
				{
					SectionLayer sectionLayer = (SectionLayer)Activator.CreateInstance(type, new object[]
					{
						this
					});
					this.layers.Add(sectionLayer);
					SectionLayer_Dynamic sectionLayer_Dynamic = sectionLayer as SectionLayer_Dynamic;
					if (sectionLayer_Dynamic != null)
					{
						this.dynamic.Add(sectionLayer_Dynamic);
					}
				}
			}
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x00079260 File Offset: 0x00077460
		public void DrawSection()
		{
			if (this.anyLayerDirty)
			{
				this.RegenerateDirtyLayers();
			}
			for (int i = 0; i < this.layers.Count; i++)
			{
				this.layers[i].DrawLayer();
			}
			if (DebugViewSettings.drawSectionEdges)
			{
				Vector3 a = this.botLeft.ToVector3();
				GenDraw.DrawLineBetween(a, a + new Vector3(0f, 0f, 17f));
				GenDraw.DrawLineBetween(a, a + new Vector3(17f, 0f, 0f));
				if (this.CellRect.Contains(UI.MouseCell()))
				{
					Vector3 a2 = this.bounds.Min.ToVector3();
					Vector3 a3 = this.bounds.Max.ToVector3() + new Vector3(1f, 0f, 1f);
					GenDraw.DrawLineBetween(a2, a2 + new Vector3((float)this.bounds.Width, 0f, 0f), SimpleColor.Magenta, 0.2f);
					GenDraw.DrawLineBetween(a2, a2 + new Vector3(0f, 0f, (float)this.bounds.Height), SimpleColor.Magenta, 0.2f);
					GenDraw.DrawLineBetween(a3, a3 - new Vector3((float)this.bounds.Width, 0f, 0f), SimpleColor.Magenta, 0.2f);
					GenDraw.DrawLineBetween(a3, a3 - new Vector3(0f, 0f, (float)this.bounds.Height), SimpleColor.Magenta, 0.2f);
				}
			}
		}

		// Token: 0x060014F5 RID: 5365 RVA: 0x00079404 File Offset: 0x00077604
		public void DrawDynamicSections(CellRect view)
		{
			for (int i = 0; i < this.dynamic.Count; i++)
			{
				if (this.dynamic[i].ShouldDrawDynamic(view))
				{
					this.dynamic[i].DrawLayer();
				}
			}
		}

		// Token: 0x060014F6 RID: 5366 RVA: 0x0007944C File Offset: 0x0007764C
		public void RegenerateSingleLayer(SectionLayer layer)
		{
			if (!layer.Visible)
			{
				return;
			}
			try
			{
				layer.Regenerate();
				if (layer.Isnt<SectionLayer_Dynamic>())
				{
					this.bounds = this.bounds.Encapsulate(layer.GetBoundaryRect());
				}
			}
			catch (Exception value)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Could not regenerate layer ");
				defaultInterpolatedStringHandler.AppendFormatted(layer.ToStringSafe<SectionLayer>());
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			layer.RefreshSubMeshBounds();
		}

		// Token: 0x060014F7 RID: 5367 RVA: 0x000794E8 File Offset: 0x000776E8
		public void RegenerateAllLayers()
		{
			this.bounds = this.CellRect;
			for (int i = 0; i < this.layers.Count; i++)
			{
				if (this.layers[i].Visible)
				{
					try
					{
						this.layers[i].Regenerate();
						if (this.layers[i].Isnt<SectionLayer_Dynamic>())
						{
							this.bounds = this.bounds.Encapsulate(this.layers[i].GetBoundaryRect());
						}
					}
					catch (Exception value)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Could not regenerate layer ");
						defaultInterpolatedStringHandler.AppendFormatted(this.layers[i].ToStringSafe<SectionLayer>());
						defaultInterpolatedStringHandler.AppendLiteral(": ");
						defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
						Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
					}
				}
			}
			for (int j = 0; j < this.layers.Count; j++)
			{
				this.layers[j].RefreshSubMeshBounds();
			}
		}

		// Token: 0x060014F8 RID: 5368 RVA: 0x00079604 File Offset: 0x00077804
		private void RegenerateDirtyLayers()
		{
			this.bounds = this.CellRect;
			this.anyLayerDirty = false;
			for (int i = 0; i < this.layers.Count; i++)
			{
				if (this.layers[i].Visible)
				{
					if (this.layers[i].Dirty)
					{
						try
						{
							this.layers[i].Regenerate();
							if (this.layers[i].Isnt<SectionLayer_Dynamic>())
							{
								this.bounds = this.bounds.Encapsulate(this.layers[i].GetBoundaryRect());
							}
							goto IL_117;
						}
						catch (Exception value)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 2);
							defaultInterpolatedStringHandler.AppendLiteral("Could not regenerate layer ");
							defaultInterpolatedStringHandler.AppendFormatted(this.layers[i].ToStringSafe<SectionLayer>());
							defaultInterpolatedStringHandler.AppendLiteral(": ");
							defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
							Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
							goto IL_117;
						}
					}
					if (this.layers[i].Isnt<SectionLayer_Dynamic>())
					{
						this.bounds = this.bounds.Encapsulate(this.layers[i].GetBoundaryRect());
					}
				}
				IL_117:;
			}
			for (int j = 0; j < this.layers.Count; j++)
			{
				this.layers[j].RefreshSubMeshBounds();
			}
		}

		// Token: 0x060014F9 RID: 5369 RVA: 0x00079774 File Offset: 0x00077974
		public bool TryUpdate(CellRect view)
		{
			if (this.dirtyFlags == 0UL)
			{
				return false;
			}
			this.bounds = this.CellRect;
			bool flag = false;
			bool flag2 = this.bounds.Overlaps(view);
			bool result = false;
			for (int i = 0; i < this.layers.Count; i++)
			{
				SectionLayer sectionLayer = this.layers[i];
				sectionLayer.Dirty = (sectionLayer.Dirty || (this.dirtyFlags & sectionLayer.relevantChangeTypes) > 0UL);
				if (sectionLayer.Dirty)
				{
					if (!flag2)
					{
						flag = (flag || sectionLayer.Dirty);
					}
					else
					{
						try
						{
							sectionLayer.Regenerate();
						}
						catch (Exception value)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 2);
							defaultInterpolatedStringHandler.AppendLiteral("Could not regenerate layer ");
							defaultInterpolatedStringHandler.AppendFormatted(sectionLayer.ToStringSafe<SectionLayer>());
							defaultInterpolatedStringHandler.AppendLiteral(": ");
							defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
							Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						finally
						{
						}
						result = true;
						sectionLayer.Dirty = false;
					}
				}
			}
			for (int j = 0; j < this.layers.Count; j++)
			{
				if (this.layers[j].Isnt<SectionLayer_Dynamic>())
				{
					this.bounds = this.bounds.Encapsulate(this.layers[j].GetBoundaryRect());
				}
			}
			for (int k = 0; k < this.layers.Count; k++)
			{
				this.layers[k].RefreshSubMeshBounds();
			}
			this.anyLayerDirty = flag;
			this.dirtyFlags = 0UL;
			return result;
		}

		// Token: 0x060014FA RID: 5370 RVA: 0x0007991C File Offset: 0x00077B1C
		public SectionLayer GetLayer(Type type)
		{
			return this.layers.FirstOrDefault((SectionLayer sect) => sect.GetType() == type);
		}

		// Token: 0x060014FB RID: 5371 RVA: 0x00079950 File Offset: 0x00077B50
		public void Dispose()
		{
			for (int i = 0; i < this.layers.Count; i++)
			{
				this.layers[i].Dispose();
			}
			for (int j = 0; j < this.dynamic.Count; j++)
			{
				this.dynamic[j].Dispose();
			}
			this.layers.Clear();
			this.dynamic.Clear();
		}

		// Token: 0x0400126B RID: 4715
		public IntVec3 botLeft;

		// Token: 0x0400126C RID: 4716
		public readonly Map map;

		// Token: 0x0400126D RID: 4717
		private CellRect calculatedRect;

		// Token: 0x0400126E RID: 4718
		private CellRect bounds;

		// Token: 0x0400126F RID: 4719
		public ulong dirtyFlags;

		// Token: 0x04001270 RID: 4720
		private bool anyLayerDirty;

		// Token: 0x04001271 RID: 4721
		private readonly List<SectionLayer> layers = new List<SectionLayer>();

		// Token: 0x04001272 RID: 4722
		private readonly List<SectionLayer_Dynamic> dynamic = new List<SectionLayer_Dynamic>();

		// Token: 0x04001273 RID: 4723
		public const int Size = 17;
	}
}
