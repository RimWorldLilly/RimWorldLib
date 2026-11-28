using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200112F RID: 4399
	public class GameCondition : IExposable, ILoadReferenceable
	{
		// Token: 0x170012DF RID: 4831
		// (get) Token: 0x06006E45 RID: 28229 RVA: 0x0026B5F0 File Offset: 0x002697F0
		protected Map SingleMap
		{
			get
			{
				return this.gameConditionManager.ownerMap;
			}
		}

		// Token: 0x170012E0 RID: 4832
		// (get) Token: 0x06006E46 RID: 28230 RVA: 0x0026B5FD File Offset: 0x002697FD
		public virtual string Label
		{
			get
			{
				return this.def.label;
			}
		}

		// Token: 0x170012E1 RID: 4833
		// (get) Token: 0x06006E47 RID: 28231 RVA: 0x0026B60A File Offset: 0x0026980A
		public virtual string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.def);
			}
		}

		// Token: 0x170012E2 RID: 4834
		// (get) Token: 0x06006E48 RID: 28232 RVA: 0x0026B61D File Offset: 0x0026981D
		public virtual string LetterText
		{
			get
			{
				return this.def.letterText;
			}
		}

		// Token: 0x170012E3 RID: 4835
		// (get) Token: 0x06006E49 RID: 28233 RVA: 0x0026B62A File Offset: 0x0026982A
		public virtual bool Expired
		{
			get
			{
				return !this.Permanent && Find.TickManager.TicksGame > this.startTick + this.Duration;
			}
		}

		// Token: 0x170012E4 RID: 4836
		// (get) Token: 0x06006E4A RID: 28234 RVA: 0x0026B64F File Offset: 0x0026984F
		public virtual bool ElectricityDisabled
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170012E5 RID: 4837
		// (get) Token: 0x06006E4B RID: 28235 RVA: 0x0026B652 File Offset: 0x00269852
		public int TicksPassed
		{
			get
			{
				return Find.TickManager.TicksGame - this.startTick;
			}
		}

		// Token: 0x170012E6 RID: 4838
		// (get) Token: 0x06006E4C RID: 28236 RVA: 0x0026B665 File Offset: 0x00269865
		public virtual string Description
		{
			get
			{
				return this.def.description;
			}
		}

		// Token: 0x170012E7 RID: 4839
		// (get) Token: 0x06006E4D RID: 28237 RVA: 0x0026B672 File Offset: 0x00269872
		public virtual int TransitionTicks
		{
			get
			{
				return 300;
			}
		}

		// Token: 0x170012E8 RID: 4840
		// (get) Token: 0x06006E4E RID: 28238 RVA: 0x0026B679 File Offset: 0x00269879
		// (set) Token: 0x06006E4F RID: 28239 RVA: 0x0026B6A5 File Offset: 0x002698A5
		public int TicksLeft
		{
			get
			{
				if (this.Permanent)
				{
					Log.ErrorOnce("Trying to get ticks left of a permanent condition.", 384767654);
					return 360000000;
				}
				return this.Duration - this.TicksPassed;
			}
			set
			{
				this.Duration = this.TicksPassed + value;
			}
		}

		// Token: 0x170012E9 RID: 4841
		// (get) Token: 0x06006E50 RID: 28240 RVA: 0x0026B6B5 File Offset: 0x002698B5
		// (set) Token: 0x06006E51 RID: 28241 RVA: 0x0026B6BD File Offset: 0x002698BD
		public bool Permanent
		{
			get
			{
				return this.permanent;
			}
			set
			{
				if (value)
				{
					this.duration = -1;
				}
				this.permanent = value;
			}
		}

		// Token: 0x170012EA RID: 4842
		// (get) Token: 0x06006E52 RID: 28242 RVA: 0x0026B6D0 File Offset: 0x002698D0
		// (set) Token: 0x06006E53 RID: 28243 RVA: 0x0026B6F5 File Offset: 0x002698F5
		public int Duration
		{
			get
			{
				if (this.Permanent)
				{
					Log.ErrorOnce("Trying to get duration of a permanent condition.", 100394867);
					return 360000000;
				}
				return this.duration;
			}
			set
			{
				this.permanent = false;
				this.duration = value;
			}
		}

		// Token: 0x170012EB RID: 4843
		// (get) Token: 0x06006E54 RID: 28244 RVA: 0x0026B708 File Offset: 0x00269908
		public virtual string TooltipString
		{
			get
			{
				string text = this.def.LabelCap.ToString();
				Map map;
				if (this.SingleMap != null)
				{
					map = this.SingleMap;
				}
				else if (Find.CurrentMap != null)
				{
					map = Find.CurrentMap;
				}
				else
				{
					map = Find.AnyPlayerHomeMap;
				}
				if (this.Permanent && !this.forceDisplayAsDuration && this.def.showPermanentInTooltip)
				{
					text += "\n" + "Permanent".Translate().CapitalizeFirst();
				}
				else
				{
					Vector2 location = (map != null) ? Find.WorldGrid.LongLatOf(map.Tile) : Vector2.zero;
					text = string.Concat(new string[]
					{
						text,
						"\n",
						"Started".Translate(),
						": ",
						GenDate.DateFullStringAt((long)GenDate.TickGameToAbs(this.startTick), location).Colorize(ColoredText.DateTimeColor)
					});
					text = string.Concat(new string[]
					{
						text,
						"\n",
						"Lasted".Translate(),
						": ",
						this.TicksPassed.ToStringTicksToPeriod(true, false, true, true, false).Colorize(ColoredText.DateTimeColor)
					});
				}
				text += "\n";
				text = text + "\n" + this.Description.ResolveTags();
				if (this.conditionCauser != null && !this.hideSource && CameraJumper.CanJump(this.conditionCauser))
				{
					text = text + "\n\n" + this.def.jumpToSourceKey.Translate().Resolve();
				}
				else
				{
					Quest quest = this.quest;
					if (quest != null && !quest.hidden)
					{
						text = text + "\n\n" + "CausedByQuest".Translate(this.quest.name).Resolve();
					}
					else if (this.psychicRitualDef != null)
					{
						string str = text;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 2);
						defaultInterpolatedStringHandler.AppendLiteral("\n\n");
						defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("CausedByPsychicRitual".Translate());
						defaultInterpolatedStringHandler.AppendLiteral(": ");
						defaultInterpolatedStringHandler.AppendFormatted(this.psychicRitualDef.label.CapitalizeFirst());
						text = str + defaultInterpolatedStringHandler.ToStringAndClear();
					}
					else if (!this.def.natural)
					{
						string str2 = text;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(2, 1);
						defaultInterpolatedStringHandler2.AppendLiteral("\n\n");
						defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("SourceUnknown".Translate());
						text = str2 + defaultInterpolatedStringHandler2.ToStringAndClear();
					}
				}
				if (map != null && GameCondition.MapExcludedByFilter(this.def, map))
				{
					string str3 = text;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(2, 1);
					defaultInterpolatedStringHandler3.AppendLiteral("\n\n");
					defaultInterpolatedStringHandler3.AppendFormatted<TaggedString>("ThisWillNotAffectLayer".Translate(map.Tile.LayerDef.gerundLabel.Named("GERUND"), map.Tile.LayerDef.label.Named("LAYER")));
					text = str3 + defaultInterpolatedStringHandler3.ToStringAndClear();
				}
				return text;
			}
		}

		// Token: 0x170012EC RID: 4844
		// (get) Token: 0x06006E55 RID: 28245 RVA: 0x0026BA38 File Offset: 0x00269C38
		public List<Map> AffectedMaps
		{
			get
			{
				if (!GenCollection.ListsEqual<Map>(this.cachedAffectedMapsForMaps, Find.Maps))
				{
					this.cachedAffectedMapsForMaps.Clear();
					this.cachedAffectedMapsForMaps.AddRange(Find.Maps);
					this.cachedAffectedMaps.Clear();
					if (this.CanApplyOnMap(this.gameConditionManager.ownerMap))
					{
						this.cachedAffectedMaps.Add(this.gameConditionManager.ownerMap);
					}
					GameCondition.tmpGameConditionManagers.Clear();
					this.gameConditionManager.GetChildren(GameCondition.tmpGameConditionManagers);
					for (int i = 0; i < GameCondition.tmpGameConditionManagers.Count; i++)
					{
						if (this.CanApplyOnMap(GameCondition.tmpGameConditionManagers[i].ownerMap))
						{
							this.cachedAffectedMaps.Add(GameCondition.tmpGameConditionManagers[i].ownerMap);
						}
					}
					GameCondition.tmpGameConditionManagers.Clear();
				}
				return this.cachedAffectedMaps;
			}
		}

		// Token: 0x06006E56 RID: 28246 RVA: 0x0026BB1C File Offset: 0x00269D1C
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.uniqueID, "uniqueID", -1, false);
			Scribe_Values.Look<bool>(ref this.suppressEndMessage, "suppressEndMessage", false, false);
			Scribe_Defs.Look<GameConditionDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
			Scribe_Values.Look<bool>(ref this.permanent, "permanent", false, false);
			Scribe_References.Look<Quest>(ref this.quest, "quest", false);
			Scribe_Values.Look<bool>(ref this.forceDisplayAsDuration, "forceDisplayAsDuration", false, false);
			Scribe_Values.Look<bool>(ref this.hideSource, "hideSource", false, false);
			Scribe_Defs.Look<PsychicRitualDef>(ref this.psychicRitualDef, "psychicRitualDef");
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06006E57 RID: 28247 RVA: 0x0026BBDE File Offset: 0x00269DDE
		public virtual void GameConditionTick()
		{
		}

		// Token: 0x06006E58 RID: 28248 RVA: 0x0026BBE0 File Offset: 0x00269DE0
		public virtual void GameConditionDraw(Map map)
		{
		}

		// Token: 0x06006E59 RID: 28249 RVA: 0x0026BBE2 File Offset: 0x00269DE2
		public virtual void Init()
		{
			if (!this.def.startMessage.NullOrEmpty())
			{
				Messages.Message(this.def.startMessage, MessageTypeDefOf.NeutralEvent, true);
			}
		}

		// Token: 0x06006E5A RID: 28250 RVA: 0x0026BC0C File Offset: 0x00269E0C
		public virtual void End()
		{
			if (!this.suppressEndMessage && this.def.endMessage != null && !this.cachedAffectedMaps.Any(new Predicate<Map>(this.HiddenByOtherCondition)))
			{
				Messages.Message(this.def.endMessage, MessageTypeDefOf.NeutralEvent, true);
			}
			this.gameConditionManager.OnConditionEnd(this);
		}

		// Token: 0x06006E5B RID: 28251 RVA: 0x0026BC6C File Offset: 0x00269E6C
		public bool CanApplyOnMap(Map map)
		{
			if (map == null)
			{
				return false;
			}
			if (map.generatorDef.isUnderground && !this.def.allowUnderground)
			{
				return false;
			}
			if (GameCondition.MapExcludedByFilter(this.def, map))
			{
				return false;
			}
			if (ModsConfig.OdysseyActive && this.def.requireFish)
			{
				WaterBodyTracker waterBodyTracker = map.waterBodyTracker;
				if (waterBodyTracker == null || !waterBodyTracker.AnyBodyContainsFish)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06006E5C RID: 28252 RVA: 0x0026BCD4 File Offset: 0x00269ED4
		public static bool MapExcludedByFilter(GameConditionDef def, Map map)
		{
			return map.Tile.Valid && ((!def.layerWhitelist.NullOrEmpty<PlanetLayerDef>() && !def.layerWhitelist.Contains(map.Tile.LayerDef)) || (!def.layerBlacklist.NullOrEmpty<PlanetLayerDef>() && def.layerBlacklist.Contains(map.Tile.LayerDef)) || (!def.canAffectAllPlanetLayers && map.Tile.LayerDef.onlyAllowWhitelistedGameConditions && (def.layerWhitelist.NullOrEmpty<PlanetLayerDef>() || !def.layerWhitelist.Contains(map.Tile.LayerDef))));
		}

		// Token: 0x06006E5D RID: 28253 RVA: 0x0026BD90 File Offset: 0x00269F90
		public bool HiddenByOtherCondition(Map map)
		{
			if (this.def.silencedByConditions.NullOrEmpty<GameConditionDef>())
			{
				return false;
			}
			for (int i = 0; i < this.def.silencedByConditions.Count; i++)
			{
				if (map.gameConditionManager.ConditionIsActive(this.def.silencedByConditions[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006E5E RID: 28254 RVA: 0x0026BDED File Offset: 0x00269FED
		public virtual float SkyGazeChanceFactor(Map map)
		{
			return 1f;
		}

		// Token: 0x06006E5F RID: 28255 RVA: 0x0026BDF4 File Offset: 0x00269FF4
		public virtual float SkyGazeJoyGainFactor(Map map)
		{
			return 1f;
		}

		// Token: 0x06006E60 RID: 28256 RVA: 0x0026BDFB File Offset: 0x00269FFB
		public virtual float TemperatureOffset()
		{
			return 0f;
		}

		// Token: 0x06006E61 RID: 28257 RVA: 0x0026BE02 File Offset: 0x0026A002
		public virtual float SkyTargetLerpFactor(Map map)
		{
			return 0f;
		}

		// Token: 0x06006E62 RID: 28258 RVA: 0x0026BE0C File Offset: 0x0026A00C
		public virtual SkyTarget? SkyTarget(Map map)
		{
			return null;
		}

		// Token: 0x06006E63 RID: 28259 RVA: 0x0026BE22 File Offset: 0x0026A022
		public virtual float AnimalDensityFactor(Map map)
		{
			return 1f;
		}

		// Token: 0x06006E64 RID: 28260 RVA: 0x0026BE29 File Offset: 0x0026A029
		public virtual float PlantDensityFactor(Map map)
		{
			return 1f;
		}

		// Token: 0x06006E65 RID: 28261 RVA: 0x0026BE30 File Offset: 0x0026A030
		public virtual bool AllowEnjoyableOutsideNow(Map map)
		{
			return true;
		}

		// Token: 0x06006E66 RID: 28262 RVA: 0x0026BE33 File Offset: 0x0026A033
		public virtual List<SkyOverlay> SkyOverlays(Map map)
		{
			return null;
		}

		// Token: 0x06006E67 RID: 28263 RVA: 0x0026BE36 File Offset: 0x0026A036
		public virtual void DoCellSteadyEffects(IntVec3 c, Map map)
		{
		}

		// Token: 0x06006E68 RID: 28264 RVA: 0x0026BE38 File Offset: 0x0026A038
		public virtual WeatherDef ForcedWeather()
		{
			return null;
		}

		// Token: 0x06006E69 RID: 28265 RVA: 0x0026BE3B File Offset: 0x0026A03B
		public virtual float MinWindSpeed()
		{
			return 0f;
		}

		// Token: 0x06006E6A RID: 28266 RVA: 0x0026BE42 File Offset: 0x0026A042
		public virtual float WeatherCommonalityFactor(WeatherDef weather, Map map)
		{
			return 1f;
		}

		// Token: 0x06006E6B RID: 28267 RVA: 0x0026BE49 File Offset: 0x0026A049
		public virtual void PostMake()
		{
			this.uniqueID = Find.UniqueIDsManager.GetNextGameConditionID();
		}

		// Token: 0x06006E6C RID: 28268 RVA: 0x0026BE5B File Offset: 0x0026A05B
		public virtual void RandomizeSettings(float points, Map map, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
		}

		// Token: 0x06006E6D RID: 28269 RVA: 0x0026BE5D File Offset: 0x0026A05D
		public string GetUniqueLoadID()
		{
			return base.GetType().Name + "_" + this.uniqueID.ToString();
		}

		// Token: 0x040046B6 RID: 18102
		public GameConditionManager gameConditionManager;

		// Token: 0x040046B7 RID: 18103
		public Thing conditionCauser;

		// Token: 0x040046B8 RID: 18104
		public bool hideSource;

		// Token: 0x040046B9 RID: 18105
		public GameConditionDef def;

		// Token: 0x040046BA RID: 18106
		public int uniqueID = -1;

		// Token: 0x040046BB RID: 18107
		public int startTick;

		// Token: 0x040046BC RID: 18108
		public bool suppressEndMessage;

		// Token: 0x040046BD RID: 18109
		private int duration = -1;

		// Token: 0x040046BE RID: 18110
		private bool permanent;

		// Token: 0x040046BF RID: 18111
		public bool forceDisplayAsDuration;

		// Token: 0x040046C0 RID: 18112
		private List<Map> cachedAffectedMaps = new List<Map>();

		// Token: 0x040046C1 RID: 18113
		private List<Map> cachedAffectedMapsForMaps = new List<Map>();

		// Token: 0x040046C2 RID: 18114
		public Quest quest;

		// Token: 0x040046C3 RID: 18115
		public PsychicRitualDef psychicRitualDef;

		// Token: 0x040046C4 RID: 18116
		private static List<GameConditionManager> tmpGameConditionManagers = new List<GameConditionManager>();
	}
}
