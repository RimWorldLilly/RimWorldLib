using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002A2 RID: 674
	public class WaterInfo : MapComponent
	{
		// Token: 0x060014B7 RID: 5303 RVA: 0x00076D6E File Offset: 0x00074F6E
		public WaterInfo(Map map) : base(map)
		{
		}

		// Token: 0x060014B8 RID: 5304 RVA: 0x00076D8D File Offset: 0x00074F8D
		public override void MapRemoved()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				Object.Destroy(this.riverFlowTexture);
			});
		}

		// Token: 0x060014B9 RID: 5305 RVA: 0x00076DA0 File Offset: 0x00074FA0
		public void SetTextures()
		{
			Camera subcamera = Current.SubcameraDriver.GetSubcamera(SubcameraDefOf.WaterDepth);
			if (subcamera != null)
			{
				Shader.SetGlobalTexture(ShaderPropertyIDs.WaterOutputTex, subcamera.targetTexture);
			}
			if (this.skyTex == null)
			{
				this.skyTex = ContentFinder<Texture2D>.Get("Other/WaterReflection", true);
			}
			Shader.SetGlobalTexture(ShaderPropertyIDs.WaterReflectionTex, this.skyTex);
			Camera camera = Find.Camera;
			Vector4 value = camera.targetTexture ? new Vector4((float)camera.targetTexture.width, (float)camera.targetTexture.height, 1f / (float)camera.targetTexture.width, 1f / (float)camera.targetTexture.height) : new Vector4((float)Screen.width, (float)Screen.height, 1f / (float)Screen.width, 1f / (float)Screen.height);
			Shader.SetGlobalVector(ShaderPropertyIDs.MainCameraScreenParams, value);
			Matrix4x4 worldToCameraMatrix = camera.worldToCameraMatrix;
			Matrix4x4 value2 = GL.GetGPUProjectionMatrix(camera.projectionMatrix, false) * worldToCameraMatrix;
			Shader.SetGlobalMatrix(ShaderPropertyIDs.MainCameraVP, value2);
			if (this.riverFlowTexture == null && this.riverFlowMap != null && this.riverFlowMap.Count > 0)
			{
				this.flowMapPixels = new Color[this.riverFlowMap.Count / 2];
				for (int i = 0; i < this.map.Size.x; i++)
				{
					for (int j = 0; j < this.map.Size.z; j++)
					{
						IntVec3 intVec = new IntVec3(i, 0, j);
						if (this.map.terrainGrid.BaseTerrainAt(intVec).IsRiver)
						{
							int num = intVec.x * this.map.Size.z + intVec.z;
							Vector2 vector = new Vector2(this.riverFlowMap[num * 2], this.riverFlowMap[num * 2 + 1]);
							int num2 = 1;
							foreach (IntVec3 intVec2 in GenAdjFast.AdjacentCells8Way(intVec))
							{
								if (intVec2.InBounds(this.map) && this.map.terrainGrid.BaseTerrainAt(intVec).IsRiver)
								{
									int num3 = intVec2.x * this.map.Size.z + intVec2.z;
									vector += new Vector2(this.riverFlowMap[num3 * 2], this.riverFlowMap[num3 * 2 + 1]);
									num2++;
								}
							}
							this.flowMapPixels[j * this.map.Size.x + i] = new Color(vector.x / (float)num2, vector.y / (float)num2, 0f);
						}
					}
				}
				this.riverFlowTexture = new Texture2D(this.map.Size.x, this.map.Size.z, TextureFormat.RGFloat, false);
				this.riverFlowTexture.SetPixels(this.flowMapPixels);
				this.riverFlowTexture.wrapMode = TextureWrapMode.Clamp;
				this.riverFlowTexture.Apply();
			}
			Shader.SetGlobalTexture(ShaderPropertyIDs.WaterOffsetTex, this.riverFlowTexture);
			Shader.SetGlobalVector(ShaderPropertyIDs.MapSize, new Vector4((float)this.map.Size.x, (float)this.map.Size.z));
		}

		// Token: 0x060014BA RID: 5306 RVA: 0x00077158 File Offset: 0x00075358
		public Vector3 GetWaterMovement(Vector3 position)
		{
			if (this.riverFlowMap == null)
			{
				return Vector3.zero;
			}
			IntVec3 intVec = position.ToIntVec3();
			int num = intVec.x * this.map.Size.z + intVec.z;
			return new Vector3(this.riverFlowMap[num * 2], 0f, this.riverFlowMap[num * 2 + 1]);
		}

		// Token: 0x060014BB RID: 5307 RVA: 0x000771C4 File Offset: 0x000753C4
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				string text = null;
				if (this.riverFlowMap != null)
				{
					text = Convert.ToBase64String(CompressUtility.Compress(MemoryMarshal.AsBytes<float>(this.riverFlowMap.AsSpan<float>()))).AddLineBreaksToLongString();
				}
				Scribe_Values.Look<string>(ref text, "riverFlowMapDeflate", null, false);
				return;
			}
			if (Scribe.EnterNode("riverFlowMapDeflate"))
			{
				Scribe.ExitNode();
				string text2 = null;
				Scribe_Values.Look<string>(ref text2, "riverFlowMapDeflate", null, false);
				if (text2 != null)
				{
					Span<float> span = MemoryMarshal.Cast<byte, float>(CompressUtility.Decompress(Convert.FromBase64String(text2)));
					this.riverFlowMap = new List<float>(span.Length);
					Span<float> destination = this.riverFlowMap.AsSpan(span.Length);
					span.CopyTo(destination);
					this.riverFlowMap.UnsafeSetCount(span.Length);
				}
			}
		}

		// Token: 0x060014BC RID: 5308 RVA: 0x00077298 File Offset: 0x00075498
		public void DebugDrawRiver()
		{
			if (this.riverGraph.NullOrEmpty<RiverNode>())
			{
				return;
			}
			if (DebugViewSettings.drawRiverDebug)
			{
				foreach (RiverNode riverNode in this.riverGraph)
				{
					GenDraw.DrawLineBetween(riverNode.start, riverNode.end, SimpleColor.Green, 5f);
					GenDraw.DrawArrowRotated(Vector3.Lerp(riverNode.start, riverNode.end, 0.5f), riverNode.start.AngleToFlat(riverNode.end) - 270f, false);
				}
			}
			if (DebugViewSettings.drawRiverFlowDebug)
			{
				foreach (IntVec3 intVec in this.map.AllCells)
				{
					int num = intVec.x * this.map.Size.z + intVec.z;
					Vector3 vector = new Vector3(this.riverFlowMap[num * 2], 0f, this.riverFlowMap[num * 2 + 1]);
					if (!(vector == Vector3.zero))
					{
						GenDraw.DrawArrowRotated(intVec.ToVector3Shifted(), vector.AngleFlat(), false);
					}
				}
			}
		}

		// Token: 0x0400123D RID: 4669
		public Texture2D riverFlowTexture;

		// Token: 0x0400123E RID: 4670
		public IntVec3 lakeCenter = IntVec3.Invalid;

		// Token: 0x0400123F RID: 4671
		public List<RiverNode> riverGraph = new List<RiverNode>();

		// Token: 0x04001240 RID: 4672
		public List<float> riverFlowMap;

		// Token: 0x04001241 RID: 4673
		[Obsolete]
		public CellRect riverFlowMapBounds;

		// Token: 0x04001242 RID: 4674
		private Color[] flowMapPixels;

		// Token: 0x04001243 RID: 4675
		private Texture2D skyTex;

		// Token: 0x04001244 RID: 4676
		private const string ReflectionTexture = "Other/WaterReflection";
	}
}
