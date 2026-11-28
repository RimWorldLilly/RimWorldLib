using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000752 RID: 1874
	public static class NoiseDebugUI
	{
		// Token: 0x17000B95 RID: 2965
		// (set) Token: 0x06003B65 RID: 15205 RVA: 0x0017A45B File Offset: 0x0017865B
		public static IntVec2 RenderSize
		{
			set
			{
				NoiseRenderer.renderSize = value;
			}
		}

		// Token: 0x06003B66 RID: 15206 RVA: 0x0017A464 File Offset: 0x00178664
		public static void StoreTexture(Texture2D texture, string name)
		{
			NoiseDebugUI.Noise2D item = new NoiseDebugUI.Noise2D(texture, name);
			NoiseDebugUI.noises2D.Add(item);
		}

		// Token: 0x06003B67 RID: 15207 RVA: 0x0017A484 File Offset: 0x00178684
		public static void StoreNoiseRender(ModuleBase noise, string name, IntVec2 renderSize)
		{
			NoiseDebugUI.RenderSize = renderSize;
			NoiseDebugUI.StoreNoiseRender(noise, name);
		}

		// Token: 0x06003B68 RID: 15208 RVA: 0x0017A494 File Offset: 0x00178694
		public static void StoreNoiseRender(ModuleBase noise, string name)
		{
			if (!Prefs.DevMode || !DebugViewSettings.drawRecordedNoise)
			{
				return;
			}
			NoiseDebugUI.Noise2D item = new NoiseDebugUI.Noise2D(noise, name);
			NoiseDebugUI.noises2D.Add(item);
		}

		// Token: 0x06003B69 RID: 15209 RVA: 0x0017A4C4 File Offset: 0x001786C4
		public static void StorePlanetNoise(ModuleBase noise, string name)
		{
			if (!Prefs.DevMode || !DebugViewSettings.drawRecordedNoise)
			{
				return;
			}
			NoiseDebugUI.NoisePlanet item = new NoiseDebugUI.NoisePlanet(noise, name);
			NoiseDebugUI.planetNoises.Add(item);
		}

		// Token: 0x06003B6A RID: 15210 RVA: 0x0017A4F4 File Offset: 0x001786F4
		public static void NoiseDebugOnGUI()
		{
			if (!Prefs.DevMode || !DebugViewSettings.drawRecordedNoise)
			{
				return;
			}
			if (Widgets.ButtonText(new Rect(0f, 40f, 200f, 30f), "Clear noise renders", true, true, true, null))
			{
				NoiseDebugUI.Clear();
			}
			if (Widgets.ButtonText(new Rect(200f, 40f, 200f, 30f), "Hide noise renders", true, true, true, null))
			{
				DebugViewSettings.drawRecordedNoise = false;
			}
			if (WorldRendererUtility.WorldSelected)
			{
				if (NoiseDebugUI.planetNoises.Any<NoiseDebugUI.NoisePlanet>() && Widgets.ButtonText(new Rect(400f, 40f, 200f, 30f), "Next planet noise", true, true, true, null))
				{
					if (Event.current.button == 1)
					{
						if (NoiseDebugUI.currentPlanetNoise == null || NoiseDebugUI.planetNoises.IndexOf(NoiseDebugUI.currentPlanetNoise) == -1)
						{
							NoiseDebugUI.currentPlanetNoise = NoiseDebugUI.planetNoises[NoiseDebugUI.planetNoises.Count - 1];
						}
						else if (NoiseDebugUI.planetNoises.IndexOf(NoiseDebugUI.currentPlanetNoise) == 0)
						{
							NoiseDebugUI.currentPlanetNoise = null;
						}
						else
						{
							NoiseDebugUI.currentPlanetNoise = NoiseDebugUI.planetNoises[NoiseDebugUI.planetNoises.IndexOf(NoiseDebugUI.currentPlanetNoise) - 1];
						}
					}
					else if (NoiseDebugUI.currentPlanetNoise == null || NoiseDebugUI.planetNoises.IndexOf(NoiseDebugUI.currentPlanetNoise) == -1)
					{
						NoiseDebugUI.currentPlanetNoise = NoiseDebugUI.planetNoises[0];
					}
					else if (NoiseDebugUI.planetNoises.IndexOf(NoiseDebugUI.currentPlanetNoise) == NoiseDebugUI.planetNoises.Count - 1)
					{
						NoiseDebugUI.currentPlanetNoise = null;
					}
					else
					{
						NoiseDebugUI.currentPlanetNoise = NoiseDebugUI.planetNoises[NoiseDebugUI.planetNoises.IndexOf(NoiseDebugUI.currentPlanetNoise) + 1];
					}
				}
				if (NoiseDebugUI.currentPlanetNoise != null)
				{
					Rect rect = new Rect(605f, 40f, 300f, 30f);
					Text.Font = GameFont.Medium;
					Widgets.Label(rect, NoiseDebugUI.currentPlanetNoise.name);
					Text.Font = GameFont.Small;
				}
			}
			float num = 0f;
			float num2 = 90f;
			Text.Font = GameFont.Tiny;
			foreach (NoiseDebugUI.Noise2D noise2D in NoiseDebugUI.noises2D)
			{
				Texture2D texture = noise2D.Texture;
				if (num + (float)texture.width + 5f > (float)UI.screenWidth)
				{
					num = 0f;
					num2 += (float)(texture.height + 5 + 25);
				}
				GUI.DrawTexture(new Rect(num, num2, (float)texture.width, (float)texture.height), texture);
				Rect rect2 = new Rect(num, num2 - 15f, (float)texture.width, (float)texture.height);
				GUI.color = Color.black;
				Widgets.Label(rect2, noise2D.name);
				GUI.color = Color.white;
				Widgets.Label(new Rect(rect2.x + 1f, rect2.y + 1f, rect2.width, rect2.height), noise2D.name);
				num += (float)(texture.width + 5);
			}
		}

		// Token: 0x06003B6B RID: 15211 RVA: 0x0017A82C File Offset: 0x00178A2C
		public static void RenderPlanetNoise()
		{
			if (!Prefs.DevMode || !DebugViewSettings.drawRecordedNoise)
			{
				return;
			}
			if (NoiseDebugUI.currentPlanetNoise == null)
			{
				return;
			}
			if (NoiseDebugUI.planetNoiseMesh == null)
			{
				List<int> triangles;
				SphereGenerator.Generate(6, 100.3f, Vector3.forward, 360f, out NoiseDebugUI.planetNoiseMeshVerts, out triangles);
				NoiseDebugUI.planetNoiseMesh = new Mesh();
				NoiseDebugUI.planetNoiseMesh.name = "NoiseDebugUI";
				NoiseDebugUI.planetNoiseMesh.SetVertices(NoiseDebugUI.planetNoiseMeshVerts);
				NoiseDebugUI.planetNoiseMesh.SetTriangles(triangles, 0);
				NoiseDebugUI.lastDrawnPlanetNoise = null;
			}
			if (NoiseDebugUI.lastDrawnPlanetNoise != NoiseDebugUI.currentPlanetNoise)
			{
				NoiseDebugUI.UpdatePlanetNoiseVertexColors();
				NoiseDebugUI.lastDrawnPlanetNoise = NoiseDebugUI.currentPlanetNoise;
			}
			Graphics.DrawMesh(NoiseDebugUI.planetNoiseMesh, Vector3.zero, Quaternion.identity, WorldMaterials.VertexColor, WorldCameraManager.WorldLayer);
		}

		// Token: 0x06003B6C RID: 15212 RVA: 0x0017A8EC File Offset: 0x00178AEC
		public static void Clear()
		{
			for (int i = 0; i < NoiseDebugUI.noises2D.Count; i++)
			{
				Object.Destroy(NoiseDebugUI.noises2D[i].Texture);
			}
			NoiseDebugUI.noises2D.Clear();
			NoiseDebugUI.ClearPlanetNoises();
		}

		// Token: 0x06003B6D RID: 15213 RVA: 0x0017A934 File Offset: 0x00178B34
		public static void ClearPlanetNoises()
		{
			NoiseDebugUI.planetNoises.Clear();
			NoiseDebugUI.currentPlanetNoise = null;
			NoiseDebugUI.lastDrawnPlanetNoise = null;
			if (NoiseDebugUI.planetNoiseMesh != null)
			{
				Mesh localPlanetNoiseMesh = NoiseDebugUI.planetNoiseMesh;
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					Object.Destroy(localPlanetNoiseMesh);
				});
				NoiseDebugUI.planetNoiseMesh = null;
			}
		}

		// Token: 0x06003B6E RID: 15214 RVA: 0x0017A98C File Offset: 0x00178B8C
		private static void UpdatePlanetNoiseVertexColors()
		{
			NoiseDebugUI.planetNoiseMeshColors.Clear();
			for (int i = 0; i < NoiseDebugUI.planetNoiseMeshVerts.Count; i++)
			{
				byte b = (byte)Mathf.Clamp((NoiseDebugUI.currentPlanetNoise.noise.GetValue(NoiseDebugUI.planetNoiseMeshVerts[i]) * 0.5f + 0.5f) * 255f, 0f, 255f);
				NoiseDebugUI.planetNoiseMeshColors.Add(new Color32(b, b, b, byte.MaxValue));
			}
			NoiseDebugUI.planetNoiseMesh.SetColors(NoiseDebugUI.planetNoiseMeshColors);
		}

		// Token: 0x040026DC RID: 9948
		private static List<NoiseDebugUI.Noise2D> noises2D = new List<NoiseDebugUI.Noise2D>();

		// Token: 0x040026DD RID: 9949
		private static List<NoiseDebugUI.NoisePlanet> planetNoises = new List<NoiseDebugUI.NoisePlanet>();

		// Token: 0x040026DE RID: 9950
		private static Mesh planetNoiseMesh;

		// Token: 0x040026DF RID: 9951
		private static NoiseDebugUI.NoisePlanet currentPlanetNoise;

		// Token: 0x040026E0 RID: 9952
		private static NoiseDebugUI.NoisePlanet lastDrawnPlanetNoise;

		// Token: 0x040026E1 RID: 9953
		private static List<Color32> planetNoiseMeshColors = new List<Color32>();

		// Token: 0x040026E2 RID: 9954
		private static List<Vector3> planetNoiseMeshVerts;

		// Token: 0x02002A8B RID: 10891
		private class Noise2D
		{
			// Token: 0x17002BE4 RID: 11236
			// (get) Token: 0x060106DF RID: 67295 RVA: 0x005C7CA5 File Offset: 0x005C5EA5
			public Texture2D Texture
			{
				get
				{
					if (this.tex == null)
					{
						this.tex = NoiseRenderer.NoiseRendered(this.noise);
					}
					return this.tex;
				}
			}

			// Token: 0x060106E0 RID: 67296 RVA: 0x005C7CCC File Offset: 0x005C5ECC
			public Noise2D(Texture2D tex, string name)
			{
				this.tex = tex;
				this.name = name;
			}

			// Token: 0x060106E1 RID: 67297 RVA: 0x005C7CE2 File Offset: 0x005C5EE2
			public Noise2D(ModuleBase noise, string name)
			{
				this.noise = noise;
				this.name = name;
			}

			// Token: 0x0400AE50 RID: 44624
			public string name;

			// Token: 0x0400AE51 RID: 44625
			private Texture2D tex;

			// Token: 0x0400AE52 RID: 44626
			private ModuleBase noise;
		}

		// Token: 0x02002A8C RID: 10892
		private class NoisePlanet
		{
			// Token: 0x060106E2 RID: 67298 RVA: 0x005C7CF8 File Offset: 0x005C5EF8
			public NoisePlanet(ModuleBase noise, string name)
			{
				this.name = name;
				this.noise = noise;
			}

			// Token: 0x0400AE53 RID: 44627
			public string name;

			// Token: 0x0400AE54 RID: 44628
			public ModuleBase noise;
		}
	}
}
