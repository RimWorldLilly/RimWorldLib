using System;
using System.Collections.Generic;
using LudeonTK;
using TMPro;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020022EB RID: 8939
	public class WorldFeatures : IExposable
	{
		// Token: 0x0600DC91 RID: 56465 RVA: 0x005228FF File Offset: 0x00520AFF
		private static void TextWrapThreshold_Changed()
		{
			Find.WorldFeatures.textsCreated = false;
		}

		// Token: 0x0600DC92 RID: 56466 RVA: 0x0052290C File Offset: 0x00520B0C
		protected static void ForceLegacyText_Changed()
		{
			Find.WorldFeatures.textsCreated = false;
		}

		// Token: 0x0600DC93 RID: 56467 RVA: 0x0052291C File Offset: 0x00520B1C
		public void ExposeData()
		{
			Scribe_Collections.Look<WorldFeature>(ref this.features, "features", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				WorldGrid grid = Find.WorldGrid;
				if (grid.Surface.tileFeature != null && grid.Surface.tileFeature.Length != 0)
				{
					DataSerializeUtility.LoadUshort(grid.Surface.tileFeature, grid.TilesCount, delegate(int i, ushort data)
					{
						grid[i].feature = ((data == ushort.MaxValue) ? null : this.GetFeatureWithID((int)data));
					});
				}
				this.textsCreated = false;
			}
		}

		// Token: 0x0600DC94 RID: 56468 RVA: 0x005229B8 File Offset: 0x00520BB8
		public void UpdateFeatures()
		{
			if (!this.textsCreated)
			{
				this.textsCreated = true;
				this.CreateTextsAndSetPosition();
			}
			bool flag = Find.PlaySettings.showWorldFeatures && !WorldRendererUtility.WorldBackgroundNow;
			for (int i = 0; i < this.features.Count; i++)
			{
				Vector3 position = WorldFeatures.texts[i].Position;
				bool flag2 = flag && !WorldRendererUtility.HiddenBehindTerrainNow(position);
				if (flag2 != WorldFeatures.texts[i].Active)
				{
					WorldFeatures.texts[i].SetActive(flag2);
					WorldFeatures.texts[i].WrapAroundPlanetSurface(this.features[i].layer);
				}
				if (flag2)
				{
					this.UpdateAlpha(WorldFeatures.texts[i], this.features[i]);
				}
			}
		}

		// Token: 0x0600DC95 RID: 56469 RVA: 0x00522A94 File Offset: 0x00520C94
		public WorldFeature GetFeatureWithID(int uniqueID)
		{
			for (int i = 0; i < this.features.Count; i++)
			{
				if (this.features[i].uniqueID == uniqueID)
				{
					return this.features[i];
				}
			}
			return null;
		}

		// Token: 0x0600DC96 RID: 56470 RVA: 0x00522ADC File Offset: 0x00520CDC
		private void UpdateAlpha(WorldFeatureTextMesh text, WorldFeature feature)
		{
			float num = 0.3f * feature.alpha;
			if (!Mathf.Approximately(text.Color.a, num))
			{
				text.Color = new Color(1f, 1f, 1f, num);
				text.WrapAroundPlanetSurface(feature.layer);
			}
			float num2 = Time.deltaTime * 5f;
			if (this.GoodCameraAltitudeFor(feature))
			{
				feature.alpha += num2;
			}
			else
			{
				feature.alpha -= num2;
			}
			feature.alpha = Mathf.Clamp01(feature.alpha);
		}

		// Token: 0x0600DC97 RID: 56471 RVA: 0x00522B74 File Offset: 0x00520D74
		private bool GoodCameraAltitudeFor(WorldFeature feature)
		{
			float num = feature.EffectiveDrawSize;
			float altitude = Find.WorldCameraDriver.altitude;
			float num2 = 1f / (altitude / WorldFeatures.AlphaScale * (altitude / WorldFeatures.AlphaScale));
			num *= num2;
			if (!feature.layer.IsSelected)
			{
				return false;
			}
			if (Find.WorldCameraDriver.CurrentZoom <= WorldCameraZoomRange.VeryClose && num >= 0.56f)
			{
				return false;
			}
			if (num < WorldFeatures.VisibleMinimumSize)
			{
				return Find.WorldCameraDriver.AltitudePercent <= 0.07f;
			}
			return num <= WorldFeatures.VisibleMaximumSize || Find.WorldCameraDriver.AltitudePercent >= 0.35f;
		}

		// Token: 0x0600DC98 RID: 56472 RVA: 0x00522C10 File Offset: 0x00520E10
		private void CreateTextsAndSetPosition()
		{
			this.CreateOrDestroyTexts();
			float averageTileSize = Find.WorldGrid.AverageTileSize;
			for (int i = 0; i < this.features.Count; i++)
			{
				WorldFeatures.texts[i].Text = this.features[i].name.WordWrapAt(WorldFeatures.TextWrapThreshold);
				WorldFeatures.texts[i].Size = this.features[i].EffectiveDrawSize * averageTileSize;
				Vector3 normalized = this.features[i].drawCenter.normalized;
				Quaternion quaternion = Quaternion.LookRotation(Vector3.Cross(normalized, Vector3.up), normalized);
				quaternion *= Quaternion.Euler(Vector3.right * 90f);
				quaternion *= Quaternion.Euler(Vector3.forward * (90f - this.features[i].drawAngle));
				WorldFeatures.texts[i].Rotation = quaternion;
				WorldFeatures.texts[i].LocalPosition = this.features[i].drawCenter;
				WorldFeatures.texts[i].WrapAroundPlanetSurface(this.features[i].layer);
				WorldFeatures.texts[i].SetActive(false);
			}
		}

		// Token: 0x0600DC99 RID: 56473 RVA: 0x00522D70 File Offset: 0x00520F70
		private void CreateOrDestroyTexts()
		{
			for (int i = 0; i < WorldFeatures.texts.Count; i++)
			{
				WorldFeatures.texts[i].Destroy();
			}
			WorldFeatures.texts.Clear();
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			LoadedLanguage defaultLanguage = LanguageDatabase.defaultLanguage;
			for (int j = 0; j < this.features.Count; j++)
			{
				WorldFeatureTextMesh worldFeatureTextMesh = new WorldFeatureTextMesh_TextMeshPro();
				worldFeatureTextMesh.Init();
				WorldFeatures.texts.Add(worldFeatureTextMesh);
			}
		}

		// Token: 0x0600DC9A RID: 56474 RVA: 0x00522DE8 File Offset: 0x00520FE8
		private bool HasCharactersUnsupportedByTextMeshPro(string str)
		{
			TMP_FontAsset font = WorldFeatureTextMesh_TextMeshPro.WorldTextPrefab.GetComponent<TextMeshPro>().font;
			for (int i = 0; i < str.Length; i++)
			{
				if (!this.HasCharacter(font, str[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600DC9B RID: 56475 RVA: 0x00522E2C File Offset: 0x0052102C
		private bool HasCharacter(TMP_FontAsset font, char character)
		{
			if (TMP_FontAsset.GetCharacters(font).IndexOf(character) >= 0)
			{
				return true;
			}
			List<TMP_FontAsset> fallbackFontAssetTable = font.fallbackFontAssetTable;
			for (int i = 0; i < fallbackFontAssetTable.Count; i++)
			{
				if (TMP_FontAsset.GetCharacters(fallbackFontAssetTable[i]).IndexOf(character) >= 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04008D6A RID: 36202
		public List<WorldFeature> features = new List<WorldFeature>();

		// Token: 0x04008D6B RID: 36203
		public bool textsCreated;

		// Token: 0x04008D6C RID: 36204
		private static List<WorldFeatureTextMesh> texts = new List<WorldFeatureTextMesh>();

		// Token: 0x04008D6D RID: 36205
		private const float BaseAlpha = 0.3f;

		// Token: 0x04008D6E RID: 36206
		private const float AlphaChangeSpeed = 5f;

		// Token: 0x04008D6F RID: 36207
		[TweakValue("Interface", 0f, 300f)]
		private static float TextWrapThreshold = 150f;

		// Token: 0x04008D70 RID: 36208
		[TweakValue("Interface.World", 0f, 100f)]
		protected static bool ForceLegacyText = false;

		// Token: 0x04008D71 RID: 36209
		[TweakValue("Interface.World", 1f, 150f)]
		protected static float AlphaScale = 30f;

		// Token: 0x04008D72 RID: 36210
		[TweakValue("Interface.World", 0f, 1f)]
		protected static float VisibleMinimumSize = 0.04f;

		// Token: 0x04008D73 RID: 36211
		[TweakValue("Interface.World", 0f, 5f)]
		protected static float VisibleMaximumSize = 1f;
	}
}
