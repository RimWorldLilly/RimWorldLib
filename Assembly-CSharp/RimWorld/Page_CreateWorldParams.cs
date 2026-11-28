using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Profile;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001D02 RID: 7426
	public class Page_CreateWorldParams : Page
	{
		// Token: 0x170022DE RID: 8926
		// (get) Token: 0x0600BFC4 RID: 49092 RVA: 0x0045B438 File Offset: 0x00459638
		public override string PageTitle
		{
			get
			{
				return "CreateWorld".Translate();
			}
		}

		// Token: 0x0600BFC5 RID: 49093 RVA: 0x0045B449 File Offset: 0x00459649
		public override void PreOpen()
		{
			base.PreOpen();
			if (!this.initialized)
			{
				this.Reset();
				this.initialized = true;
			}
		}

		// Token: 0x0600BFC6 RID: 49094 RVA: 0x0045B466 File Offset: 0x00459666
		public override void PostOpen()
		{
			base.PostOpen();
			TutorSystem.Notify_Event("PageStart-CreateWorldParams");
		}

		// Token: 0x0600BFC7 RID: 49095 RVA: 0x0045B480 File Offset: 0x00459680
		public void Reset()
		{
			this.seedString = GenText.RandomSeedString();
			this.planetCoverage = ((!Prefs.DevMode || !UnityData.isEditor) ? (ModsConfig.OdysseyActive ? 0.5f : 0.3f) : 0.05f);
			this.rainfall = OverallRainfall.Normal;
			this.temperature = OverallTemperature.Normal;
			this.population = OverallPopulation.Normal;
			this.landmarkDensity = LandmarkDensity.Normal;
			this.pollution = (ModsConfig.BiotechActive ? 0.05f : 0f);
			this.ResetFactionCounts();
		}

		// Token: 0x0600BFC8 RID: 49096 RVA: 0x0045B504 File Offset: 0x00459704
		private void ResetFactionCounts()
		{
			this.factions = new List<FactionDef>();
			foreach (FactionDef factionDef in FactionGenerator.ConfigurableFactions)
			{
				if (factionDef.startingCountAtWorldCreation > 0)
				{
					for (int i = 0; i < factionDef.startingCountAtWorldCreation; i++)
					{
						this.factions.Add(factionDef);
					}
				}
			}
			using (IEnumerator<FactionDef> enumerator = FactionGenerator.ConfigurableFactions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FactionDef faction = enumerator.Current;
					if (faction.replacesFaction != null)
					{
						this.factions.RemoveAll((FactionDef x) => x == faction.replacesFaction);
					}
				}
			}
			this.initialFactions = new List<FactionDef>();
			this.initialFactions.AddRange(this.factions);
		}

		// Token: 0x0600BFC9 RID: 49097 RVA: 0x0045B5FC File Offset: 0x004597FC
		public override void DoWindowContents(Rect rect)
		{
			base.DrawPageTitle(rect);
			Rect mainRect = base.GetMainRect(rect, 0f, false);
			float num = (mainRect.width - this.Margin) * 0.5f;
			Rect rect2 = new Rect(mainRect.x, mainRect.y, num, mainRect.height);
			Widgets.BeginGroup(rect2);
			Text.Font = GameFont.Small;
			float num2 = 0f;
			float width = rect2.width - 200f;
			Widgets.Label(new Rect(0f, num2, 200f, 30f), "WorldSeed".Translate());
			Rect rect3 = new Rect(200f, num2, width, 30f);
			this.seedString = Widgets.TextField(rect3, this.seedString);
			num2 += 40f;
			if (Widgets.ButtonText(new Rect(200f, num2, width, 30f), "RandomizeSeed".Translate(), true, true, true, null))
			{
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
				this.seedString = GenText.RandomSeedString();
			}
			num2 += 40f;
			Widgets.Label(new Rect(0f, num2, 200f, 30f), "PlanetCoverage".Translate());
			Rect rect4 = new Rect(200f, num2, width, 30f);
			if (Widgets.ButtonText(rect4, this.planetCoverage.ToStringPercent(), true, true, true, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				float[] array = Prefs.DevMode ? Page_CreateWorldParams.PlanetCoveragesDev : Page_CreateWorldParams.PlanetCoverages;
				for (int i = 0; i < array.Length; i++)
				{
					float coverage = array[i];
					string text = coverage.ToStringPercent();
					if (coverage <= 0.1f)
					{
						text += " (dev)";
					}
					FloatMenuOption item = new FloatMenuOption(text, delegate()
					{
						if (this.planetCoverage != coverage)
						{
							this.planetCoverage = coverage;
							if (this.planetCoverage == 1f)
							{
								Messages.Message("MessageMaxPlanetCoveragePerformanceWarning".Translate(), MessageTypeDefOf.CautionInput, false);
							}
						}
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					list.Add(item);
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			TooltipHandler.TipRegionByKey(new Rect(0f, num2, rect4.xMax, rect4.height), "PlanetCoverageTip");
			num2 += 40f;
			Widgets.Label(new Rect(0f, num2, 200f, 30f), "PlanetRainfall".Translate());
			Rect rect5 = new Rect(200f, num2, width, 30f);
			this.rainfall = (OverallRainfall)Mathf.RoundToInt(Widgets.HorizontalSlider(rect5, (float)this.rainfall, 0f, (float)(OverallRainfallUtility.EnumValuesCount - 1), true, "PlanetRainfall_Normal".Translate(), "PlanetRainfall_Low".Translate(), "PlanetRainfall_High".Translate(), 1f));
			num2 += 40f;
			Widgets.Label(new Rect(0f, num2, 200f, 30f), "PlanetTemperature".Translate());
			Rect rect6 = new Rect(200f, num2, width, 30f);
			this.temperature = (OverallTemperature)Mathf.RoundToInt(Widgets.HorizontalSlider(rect6, (float)this.temperature, 0f, (float)(OverallTemperatureUtility.EnumValuesCount - 1), true, "PlanetTemperature_Normal".Translate(), "PlanetTemperature_Low".Translate(), "PlanetTemperature_High".Translate(), 1f));
			num2 += 40f;
			Widgets.Label(new Rect(0f, num2, 200f, 30f), "PlanetPopulation".Translate());
			Rect rect7 = new Rect(200f, num2, width, 30f);
			this.population = (OverallPopulation)Mathf.RoundToInt(Widgets.HorizontalSlider(rect7, (float)this.population, 0f, (float)(OverallPopulationUtility.EnumValuesCount - 1), true, "PlanetPopulation_Normal".Translate(), "PlanetPopulation_Low".Translate(), "PlanetPopulation_High".Translate(), 1f));
			if (ModsConfig.OdysseyActive)
			{
				num2 += 40f;
				Widgets.Label(new Rect(0f, num2, 200f, 30f), "PlanetLandmarkDensity".Translate());
				Rect rect8 = new Rect(200f, num2, width, 30f);
				this.landmarkDensity = (LandmarkDensity)Mathf.RoundToInt(Widgets.HorizontalSlider(rect8, (float)this.landmarkDensity, 0f, (float)(LandmarkDensityUtility.EnumValuesCount - 1), true, "PlanetLandmarkDensity_Normal".Translate(), "PlanetLandmarkDensity_Low".Translate(), "PlanetLandmarkDensity_High".Translate(), 1f));
			}
			if (ModsConfig.BiotechActive)
			{
				num2 += 40f;
				Widgets.Label(new Rect(0f, num2, 200f, 30f), "PlanetPollution".Translate());
				Rect rect9 = new Rect(200f, num2, width, 30f);
				this.pollution = Widgets.HorizontalSlider(rect9, this.pollution, 0f, 1f, true, this.pollution.ToStringPercent(), null, null, 0.05f);
			}
			if (!TutorSystem.TutorialMode)
			{
				num2 += 40f;
				Widgets.Label(new Rect(0f, num2, 200f, 30f), "AdvancedSettings".Translate());
				if (Widgets.ButtonText(new Rect(200f, num2, width, 30f), "Edit".Translate() + "...", true, true, true, null))
				{
					Find.WindowStack.Add(new Dialog_AdvancedGameConfig());
				}
			}
			Widgets.EndGroup();
			Rect rect10 = new Rect(mainRect.x + mainRect.xMax - num, mainRect.y, num, mainRect.height);
			bool isDefaultFactionCounts = this.factions.SetsEqual(this.initialFactions);
			WorldFactionsUIUtility.DoWindowContents(rect10, this.factions, isDefaultFactionCounts);
			float y = rect.yMax - 38f;
			float x = mainRect.center.x;
			Rect rect11 = new Rect(x - Page.BottomButSize.x - 8.5f, y, Page.BottomButSize.x, Page.BottomButSize.y);
			if (Widgets.ButtonText(rect11, "ResetAll".Translate(), true, true, true, null))
			{
				this.Reset();
			}
			rect11.x = x + 8.5f;
			if (Widgets.ButtonText(rect11, "ResetFactions".Translate(), true, true, true, null))
			{
				this.ResetFactionCounts();
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			}
			base.DoBottomButtons(rect, "WorldGenerate".Translate(), null, null, true, true);
		}

		// Token: 0x0600BFCA RID: 49098 RVA: 0x0045BCEE File Offset: 0x00459EEE
		protected override bool CanDoNext()
		{
			if (!base.CanDoNext())
			{
				return false;
			}
			LongEventHandler.QueueLongEvent(delegate()
			{
				Find.GameInitData.ResetWorldRelatedMapInitData();
				Current.Game.World = WorldGenerator.GenerateWorld(this.planetCoverage, this.seedString, this.rainfall, this.temperature, this.population, this.landmarkDensity, this.factions, this.pollution);
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					if (this.next != null)
					{
						Find.WindowStack.Add(this.next);
					}
					MemoryUtility.UnloadUnusedUnityAssets();
					Find.World.renderer.RegenerateAllLayersNow();
					this.Close(true);
				});
			}, "GeneratingWorld", true, null, true, false, null);
			return false;
		}

		// Token: 0x04006E44 RID: 28228
		private bool initialized;

		// Token: 0x04006E45 RID: 28229
		private string seedString;

		// Token: 0x04006E46 RID: 28230
		private float planetCoverage;

		// Token: 0x04006E47 RID: 28231
		private OverallRainfall rainfall;

		// Token: 0x04006E48 RID: 28232
		private OverallTemperature temperature;

		// Token: 0x04006E49 RID: 28233
		private OverallPopulation population;

		// Token: 0x04006E4A RID: 28234
		private LandmarkDensity landmarkDensity;

		// Token: 0x04006E4B RID: 28235
		public float pollution;

		// Token: 0x04006E4C RID: 28236
		private List<FactionDef> factions;

		// Token: 0x04006E4D RID: 28237
		private List<FactionDef> initialFactions;

		// Token: 0x04006E4E RID: 28238
		private static readonly float[] PlanetCoverages = new float[]
		{
			0.3f,
			0.5f,
			1f
		};

		// Token: 0x04006E4F RID: 28239
		private static readonly float[] PlanetCoveragesDev = new float[]
		{
			0.3f,
			0.5f,
			1f,
			0.05f
		};

		// Token: 0x04006E50 RID: 28240
		private const float LabelWidth = 200f;
	}
}
