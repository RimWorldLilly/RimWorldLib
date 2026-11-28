using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200232F RID: 9007
	public static class WorldRendererUtility
	{
		// Token: 0x17002704 RID: 9988
		// (get) Token: 0x0600DE7B RID: 56955 RVA: 0x0052BD44 File Offset: 0x00529F44
		public static WorldRenderMode CurrentWorldRenderMode
		{
			get
			{
				if (Find.World == null)
				{
					return WorldRenderMode.None;
				}
				if (WorldComponent_GravshipController.CutsceneInProgress)
				{
					if (Find.CurrentMap != null && Find.CurrentMap.generatorDef.renderWorld)
					{
						return WorldRenderMode.Background;
					}
					return WorldRenderMode.None;
				}
				else
				{
					if (Current.ProgramState == ProgramState.Playing && Find.CurrentMap == null)
					{
						return WorldRenderMode.Planet;
					}
					if (Find.World.renderer.wantedMode == WorldRenderMode.Planet)
					{
						return WorldRenderMode.Planet;
					}
					if (Find.CurrentMap != null && Find.CurrentMap.generatorDef.renderWorld)
					{
						return WorldRenderMode.Background;
					}
					return Find.World.renderer.wantedMode;
				}
			}
		}

		// Token: 0x17002705 RID: 9989
		// (get) Token: 0x0600DE7C RID: 56956 RVA: 0x0052BDCB File Offset: 0x00529FCB
		public static bool WorldRendered
		{
			get
			{
				return WorldRendererUtility.CurrentWorldRenderMode > WorldRenderMode.None;
			}
		}

		// Token: 0x17002706 RID: 9990
		// (get) Token: 0x0600DE7D RID: 56957 RVA: 0x0052BDD5 File Offset: 0x00529FD5
		public static bool WorldBackgroundNow
		{
			get
			{
				return WorldRendererUtility.CurrentWorldRenderMode == WorldRenderMode.Background;
			}
		}

		// Token: 0x17002707 RID: 9991
		// (get) Token: 0x0600DE7E RID: 56958 RVA: 0x0052BDDF File Offset: 0x00529FDF
		public static bool WorldSelected
		{
			get
			{
				return WorldRendererUtility.CurrentWorldRenderMode == WorldRenderMode.Planet;
			}
		}

		// Token: 0x17002708 RID: 9992
		// (get) Token: 0x0600DE7F RID: 56959 RVA: 0x0052BDE9 File Offset: 0x00529FE9
		public static bool DrawingMap
		{
			get
			{
				return WorldRendererUtility.CurrentWorldRenderMode != WorldRenderMode.Planet;
			}
		}

		// Token: 0x0600DE80 RID: 56960 RVA: 0x0052BDF8 File Offset: 0x00529FF8
		public static void UpdateGlobalShadersParams()
		{
			Vector3 v = -GenCelestial.CurSunPositionInWorldSpace();
			float value = (WorldRendererUtility.WorldBackgroundNow || !PlanetLayer.Selected.IsRootSurface || Find.PlaySettings.usePlanetDayNightSystem) ? 1f : 0f;
			Shader.SetGlobalVector(ShaderPropertyIDs.PlanetSunLightDirection, v);
			Shader.SetGlobalFloat(ShaderPropertyIDs.PlanetSunLightEnabled, value);
			Shader.SetGlobalFloat(ShaderPropertyIDs.BackgroundModeEnabled, WorldRendererUtility.WorldBackgroundNow ? 1f : 0f);
		}

		// Token: 0x0600DE81 RID: 56961 RVA: 0x0052BE77 File Offset: 0x0052A077
		public static void PrintQuadTangentialToPlanet(Vector3 pos, float size, float altOffset, LayerSubMesh subMesh, bool counterClockwise = false, float rotation = 0f, bool printUVs = true)
		{
			WorldRendererUtility.PrintQuadTangentialToPlanet(pos, pos, size, altOffset, subMesh, counterClockwise, rotation, printUVs);
		}

		// Token: 0x0600DE82 RID: 56962 RVA: 0x0052BE8C File Offset: 0x0052A08C
		public static void PrintQuadTangentialToPlanet(Vector3 pos, Vector3 posForTangents, float size, float altOffset, LayerSubMesh subMesh, bool counterClockwise = false, float rotation = 0f, bool printUVs = true)
		{
			Vector3 a;
			Vector3 a2;
			WorldRendererUtility.GetTangentsToPlanet(posForTangents, out a, out a2, rotation);
			Vector3 normalized = posForTangents.normalized;
			float d = size * 0.5f;
			Vector3 item = pos - a * d - a2 * d + normalized * altOffset;
			Vector3 item2 = pos - a * d + a2 * d + normalized * altOffset;
			Vector3 item3 = pos + a * d + a2 * d + normalized * altOffset;
			Vector3 item4 = pos + a * d - a2 * d + normalized * altOffset;
			int count = subMesh.verts.Count;
			subMesh.verts.Add(item);
			subMesh.verts.Add(item2);
			subMesh.verts.Add(item3);
			subMesh.verts.Add(item4);
			if (printUVs)
			{
				subMesh.uvs.Add(new Vector2(0f, 0f));
				subMesh.uvs.Add(new Vector2(0f, 1f));
				subMesh.uvs.Add(new Vector2(1f, 1f));
				subMesh.uvs.Add(new Vector2(1f, 0f));
			}
			if (counterClockwise)
			{
				subMesh.tris.Add(count + 2);
				subMesh.tris.Add(count + 1);
				subMesh.tris.Add(count);
				subMesh.tris.Add(count + 3);
				subMesh.tris.Add(count + 2);
				subMesh.tris.Add(count);
				return;
			}
			subMesh.tris.Add(count);
			subMesh.tris.Add(count + 1);
			subMesh.tris.Add(count + 2);
			subMesh.tris.Add(count);
			subMesh.tris.Add(count + 2);
			subMesh.tris.Add(count + 3);
		}

		// Token: 0x0600DE83 RID: 56963 RVA: 0x0052C0DC File Offset: 0x0052A2DC
		public static void DrawQuadTangentialToPlanet(Vector3 pos, float size, float altOffset, Material material, float rotationAngle = 0f, bool counterClockwise = false, bool useSkyboxLayer = false, MaterialPropertyBlock propertyBlock = null)
		{
			if (material == null)
			{
				Log.Warning("Tried to draw quad with null material.");
				return;
			}
			Vector3 normalized = pos.normalized;
			Vector3 vector;
			if (counterClockwise)
			{
				vector = -normalized;
			}
			else
			{
				vector = normalized;
			}
			Quaternion rhs = Quaternion.LookRotation(Vector3.Cross(vector, Vector3.up), vector);
			Quaternion q = Quaternion.AngleAxis(rotationAngle, normalized) * rhs;
			Vector3 s = new Vector3(size, 1f, size);
			Matrix4x4 matrix = Matrix4x4.TRS(pos + normalized * altOffset, q, s);
			int layer = useSkyboxLayer ? WorldCameraManager.WorldSkyboxLayer : WorldCameraManager.WorldLayer;
			if (propertyBlock != null)
			{
				Graphics.DrawMesh(MeshPool.plane10, matrix, material, layer, null, 0, propertyBlock);
				return;
			}
			Graphics.DrawMesh(MeshPool.plane10, matrix, material, layer);
		}

		// Token: 0x0600DE84 RID: 56964 RVA: 0x0052C194 File Offset: 0x0052A394
		public static void GetTangentsToPlanet(Vector3 pos, out Vector3 first, out Vector3 second, float rotation = 0f)
		{
			Vector3 normalized = pos.normalized;
			Vector3 upwards = Vector3.up;
			if (Mathf.Abs(Vector3.Dot(normalized, Vector3.up)) > 0.999f)
			{
				upwards = Vector3.right;
			}
			Quaternion rhs = Quaternion.LookRotation(normalized, upwards);
			Quaternion rotation2 = Quaternion.AngleAxis(rotation, normalized) * rhs;
			first = rotation2 * Vector3.up;
			second = rotation2 * Vector3.right;
		}

		// Token: 0x0600DE85 RID: 56965 RVA: 0x0052C204 File Offset: 0x0052A404
		public static Vector3 ProjectOnQuadTangentialToPlanet(Vector3 center, Vector2 point)
		{
			Vector3 a;
			Vector3 a2;
			WorldRendererUtility.GetTangentsToPlanet(center, out a, out a2, 0f);
			return point.x * a + point.y * a2;
		}

		// Token: 0x0600DE86 RID: 56966 RVA: 0x0052C240 File Offset: 0x0052A440
		public static void GetTangentialVectorFacing(Vector3 root, Vector3 pointToFace, out Vector3 forward, out Vector3 right)
		{
			Quaternion rotation = Quaternion.LookRotation(root, pointToFace);
			forward = rotation * Vector3.up;
			right = rotation * Vector3.left;
		}

		// Token: 0x0600DE87 RID: 56967 RVA: 0x0052C278 File Offset: 0x0052A478
		public static void PrintTextureAtlasUVs(int indexX, int indexY, int numX, int numY, LayerSubMesh subMesh)
		{
			float num = 1f / (float)numX;
			float num2 = 1f / (float)numY;
			float num3 = (float)indexX * num;
			float num4 = (float)indexY * num2;
			subMesh.uvs.Add(new Vector2(num3, num4));
			subMesh.uvs.Add(new Vector2(num3, num4 + num2));
			subMesh.uvs.Add(new Vector2(num3 + num, num4 + num2));
			subMesh.uvs.Add(new Vector2(num3 + num, num4));
		}

		// Token: 0x0600DE88 RID: 56968 RVA: 0x0052C30C File Offset: 0x0052A50C
		public static bool HiddenBehindTerrainNow(Vector3 pos)
		{
			Dictionary<int, PlanetLayer> dictionary = (Dictionary<int, PlanetLayer>)Find.WorldGrid.PlanetLayers;
			foreach (int key in dictionary.Keys)
			{
				PlanetLayer planetLayer = dictionary[key];
				if (planetLayer.Visible && planetLayer.Def.obstructsExpandingIcons && planetLayer.LineIntersects(Find.WorldCameraDriver.CameraPosition, pos, 1f))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04008EE6 RID: 36582
		private const float WorldIntersectionFactor = 1f;
	}
}
