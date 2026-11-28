using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200016A RID: 362
	public class GameConditionDef : Def
	{
		// Token: 0x06000AD6 RID: 2774 RVA: 0x0003BCDD File Offset: 0x00039EDD
		public bool CanCoexistWith(GameConditionDef other)
		{
			return this != other && (!ModsConfig.AnomalyActive || this == GameConditionDefOf.UnnaturalDarkness || other != GameConditionDefOf.UnnaturalDarkness) && (this.exclusiveConditions == null || !this.exclusiveConditions.Contains(other));
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x0003BD17 File Offset: 0x00039F17
		public static GameConditionDef Named(string defName)
		{
			return DefDatabase<GameConditionDef>.GetNamed(defName, true);
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x0003BD20 File Offset: 0x00039F20
		public override IEnumerable<string> ConfigErrors()
		{
			GameConditionDef.<ConfigErrors>d__34 <ConfigErrors>d__ = new GameConditionDef.<ConfigErrors>d__34(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x0400083E RID: 2110
		public Type conditionClass = typeof(GameCondition);

		// Token: 0x0400083F RID: 2111
		private List<GameConditionDef> exclusiveConditions;

		// Token: 0x04000840 RID: 2112
		[MustTranslate]
		public string startMessage;

		// Token: 0x04000841 RID: 2113
		[MustTranslate]
		public string endMessage;

		// Token: 0x04000842 RID: 2114
		[MustTranslate]
		public string letterText;

		// Token: 0x04000843 RID: 2115
		public List<ThingDef> letterHyperlinks;

		// Token: 0x04000844 RID: 2116
		public LetterDef letterDef;

		// Token: 0x04000845 RID: 2117
		public bool canBePermanent;

		// Token: 0x04000846 RID: 2118
		public bool showPermanentInTooltip = true;

		// Token: 0x04000847 RID: 2119
		public bool allowUnderground = true;

		// Token: 0x04000848 RID: 2120
		public bool requireFish;

		// Token: 0x04000849 RID: 2121
		[MustTranslate]
		public string descriptionFuture;

		// Token: 0x0400084A RID: 2122
		[NoTranslate]
		public string jumpToSourceKey = "ClickToJumpToSource";

		// Token: 0x0400084B RID: 2123
		public List<GameConditionDef> silencedByConditions;

		// Token: 0x0400084C RID: 2124
		public bool natural = true;

		// Token: 0x0400084D RID: 2125
		public bool preventNeutralVisitors;

		// Token: 0x0400084E RID: 2126
		public bool preventShuttleLaunch;

		// Token: 0x0400084F RID: 2127
		public bool causesTraderCaravanExit;

		// Token: 0x04000850 RID: 2128
		public bool preventIncidents;

		// Token: 0x04000851 RID: 2129
		public bool displayOnUI = true;

		// Token: 0x04000852 RID: 2130
		public bool pennedAnimalsSeekShelter;

		// Token: 0x04000853 RID: 2131
		public ThingDef spreadsFilth;

		// Token: 0x04000854 RID: 2132
		public bool canAffectAllPlanetLayers;

		// Token: 0x04000855 RID: 2133
		public List<PlanetLayerDef> layerWhitelist;

		// Token: 0x04000856 RID: 2134
		public List<PlanetLayerDef> layerBlacklist;

		// Token: 0x04000857 RID: 2135
		public PsychicDroneLevel defaultDroneLevel = PsychicDroneLevel.BadMedium;

		// Token: 0x04000858 RID: 2136
		public bool preventRain;

		// Token: 0x04000859 RID: 2137
		public WeatherDef weatherDef;

		// Token: 0x0400085A RID: 2138
		public float temperatureOffset = -10f;

		// Token: 0x0400085B RID: 2139
		public float minNearbyPollution;

		// Token: 0x0400085C RID: 2140
		public SimpleCurve mtbOverNearbyPollutionCurve;

		// Token: 0x0400085D RID: 2141
		public float fishPopulationOffsetPerDay;
	}
}
