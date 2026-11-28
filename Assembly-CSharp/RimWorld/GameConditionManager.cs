using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001132 RID: 4402
	public sealed class GameConditionManager : IExposable
	{
		// Token: 0x170012ED RID: 4845
		// (get) Token: 0x06006E74 RID: 28276 RVA: 0x0026BF99 File Offset: 0x0026A199
		public List<GameCondition> ActiveConditions
		{
			get
			{
				return this.activeConditions;
			}
		}

		// Token: 0x170012EE RID: 4846
		// (get) Token: 0x06006E75 RID: 28277 RVA: 0x0026BFA1 File Offset: 0x0026A1A1
		public GameConditionManager Parent
		{
			get
			{
				if (this.ownerMap != null)
				{
					return Find.World.gameConditionManager;
				}
				return null;
			}
		}

		// Token: 0x170012EF RID: 4847
		// (get) Token: 0x06006E76 RID: 28278 RVA: 0x0026BFB7 File Offset: 0x0026A1B7
		public float MapBrightness
		{
			get
			{
				if (!ModsConfig.AnomalyActive)
				{
					return 1f;
				}
				GameConditionManager.MapBrightnessTracker mapBrightnessTracker = this.mapBrightnessTracker;
				if (mapBrightnessTracker == null)
				{
					return 1f;
				}
				return mapBrightnessTracker.CurBrightness;
			}
		}

		// Token: 0x170012F0 RID: 4848
		// (get) Token: 0x06006E77 RID: 28279 RVA: 0x0026BFDB File Offset: 0x0026A1DB
		public bool BrightnessChanging
		{
			get
			{
				if (ModsConfig.AnomalyActive)
				{
					GameConditionManager.MapBrightnessTracker mapBrightnessTracker = this.mapBrightnessTracker;
					return mapBrightnessTracker != null && mapBrightnessTracker.Changing;
				}
				return false;
			}
		}

		// Token: 0x170012F1 RID: 4849
		// (get) Token: 0x06006E78 RID: 28280 RVA: 0x0026BFF7 File Offset: 0x0026A1F7
		public bool DarknessVisible
		{
			get
			{
				if (ModsConfig.AnomalyActive && DebugViewSettings.drawDarknessOverlay)
				{
					GameConditionManager.MapBrightnessTracker mapBrightnessTracker = this.mapBrightnessTracker;
					return mapBrightnessTracker != null && mapBrightnessTracker.DarknessVisible;
				}
				return false;
			}
		}

		// Token: 0x170012F2 RID: 4850
		// (get) Token: 0x06006E79 RID: 28281 RVA: 0x0026C01A File Offset: 0x0026A21A
		public bool IsAlwaysDarkOutside
		{
			get
			{
				return this.cachedAlwaysDark;
			}
		}

		// Token: 0x06006E7A RID: 28282 RVA: 0x0026C022 File Offset: 0x0026A222
		public GameConditionManager(Map map)
		{
			this.ownerMap = map;
			this.mapBrightnessTracker = new GameConditionManager.MapBrightnessTracker();
		}

		// Token: 0x06006E7B RID: 28283 RVA: 0x0026C047 File Offset: 0x0026A247
		public GameConditionManager(World world)
		{
		}

		// Token: 0x06006E7C RID: 28284 RVA: 0x0026C05C File Offset: 0x0026A25C
		public bool ElectricityDisabled(Map map)
		{
			foreach (GameCondition gameCondition in this.activeConditions)
			{
				if (gameCondition.ElectricityDisabled && gameCondition.CanApplyOnMap(map) && !gameCondition.HiddenByOtherCondition(map))
				{
					return true;
				}
			}
			GameConditionManager parent = this.Parent;
			return parent != null && parent.ElectricityDisabled(map);
		}

		// Token: 0x06006E7D RID: 28285 RVA: 0x0026C0DC File Offset: 0x0026A2DC
		public float FishPopulationOffsetFactorPerDay(Map map, out GameCondition culprit)
		{
			culprit = null;
			if (!ModsConfig.OdysseyActive)
			{
				return 0f;
			}
			foreach (GameCondition gameCondition in this.activeConditions)
			{
				GameCondition_GillRot gameCondition_GillRot = gameCondition as GameCondition_GillRot;
				if (gameCondition_GillRot != null && gameCondition.CanApplyOnMap(map) && !gameCondition.HiddenByOtherCondition(map))
				{
					culprit = gameCondition;
					return gameCondition_GillRot.fishPopulationOffsetFactorPerDay;
				}
			}
			return 0.025f;
		}

		// Token: 0x06006E7E RID: 28286 RVA: 0x0026C168 File Offset: 0x0026A368
		public void RegisterCondition(GameCondition cond)
		{
			this.activeConditions.Add(cond);
			cond.startTick = Mathf.Max(cond.startTick, Find.TickManager.TicksGame);
			cond.gameConditionManager = this;
			cond.Init();
			if (!this.cachedAlwaysDark)
			{
				this.cachedAlwaysDark = (cond.Permanent && cond is GameCondition_NoSunlight);
			}
			foreach (Map map in Find.Maps)
			{
				map.events.Notify_GameConditionAdded(cond);
			}
		}

		// Token: 0x06006E7F RID: 28287 RVA: 0x0026C214 File Offset: 0x0026A414
		public void OnConditionEnd(GameCondition cond)
		{
			this.activeConditions.Remove(cond);
			if (this.cachedAlwaysDark && cond.Permanent && cond is GameCondition_NoSunlight)
			{
				this.RecalcAlwaysDark();
			}
			foreach (Map map in Find.Maps)
			{
				map.events.Notify_GameConditionRemoved(cond);
			}
		}

		// Token: 0x06006E80 RID: 28288 RVA: 0x0026C294 File Offset: 0x0026A494
		public void ExposeData()
		{
			Scribe_Collections.Look<GameCondition>(ref this.activeConditions, "activeConditions", LookMode.Deep, Array.Empty<object>());
			Scribe_Deep.Look<GameConditionManager.MapBrightnessTracker>(ref this.mapBrightnessTracker, "mapBrightnessTracker", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				for (int i = 0; i < this.activeConditions.Count; i++)
				{
					this.activeConditions[i].gameConditionManager = this;
				}
				return;
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.mapBrightnessTracker == null && this.ownerMap != null)
				{
					this.mapBrightnessTracker = new GameConditionManager.MapBrightnessTracker();
				}
				this.RecalcAlwaysDark();
			}
		}

		// Token: 0x06006E81 RID: 28289 RVA: 0x0026C328 File Offset: 0x0026A528
		public void GameConditionManagerTick()
		{
			for (int i = this.activeConditions.Count - 1; i >= 0; i--)
			{
				GameCondition gameCondition = this.activeConditions[i];
				if (gameCondition.Expired)
				{
					gameCondition.End();
				}
				else
				{
					gameCondition.GameConditionTick();
				}
			}
			GameConditionManager.MapBrightnessTracker mapBrightnessTracker = this.mapBrightnessTracker;
			if (mapBrightnessTracker == null)
			{
				return;
			}
			mapBrightnessTracker.Tick();
		}

		// Token: 0x06006E82 RID: 28290 RVA: 0x0026C380 File Offset: 0x0026A580
		public void SetTargetBrightness(float target, float lerpSeconds = 5f)
		{
			GameConditionManager.MapBrightnessTracker mapBrightnessTracker = this.mapBrightnessTracker;
			if (mapBrightnessTracker != null)
			{
				mapBrightnessTracker.Change(target, lerpSeconds);
			}
			Map map = this.ownerMap;
			if (map == null)
			{
				return;
			}
			map.mapDrawer.WholeMapChanged(MapMeshFlagDefOf.GroundGlow);
		}

		// Token: 0x06006E83 RID: 28291 RVA: 0x0026C3B4 File Offset: 0x0026A5B4
		public void GameConditionManagerDraw(Map map)
		{
			for (int i = this.activeConditions.Count - 1; i >= 0; i--)
			{
				this.activeConditions[i].GameConditionDraw(map);
			}
			GameConditionManager parent = this.Parent;
			if (parent == null)
			{
				return;
			}
			parent.GameConditionManagerDraw(map);
		}

		// Token: 0x06006E84 RID: 28292 RVA: 0x0026C3FC File Offset: 0x0026A5FC
		public void DoSteadyEffects(IntVec3 c, Map map)
		{
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				this.activeConditions[i].DoCellSteadyEffects(c, map);
			}
			GameConditionManager parent = this.Parent;
			if (parent == null)
			{
				return;
			}
			parent.DoSteadyEffects(c, map);
		}

		// Token: 0x06006E85 RID: 28293 RVA: 0x0026C444 File Offset: 0x0026A644
		private void RecalcAlwaysDark()
		{
			this.cachedAlwaysDark = false;
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				if (this.activeConditions[i].Permanent && this.activeConditions[i] is GameCondition_NoSunlight)
				{
					this.cachedAlwaysDark = true;
					return;
				}
			}
		}

		// Token: 0x06006E86 RID: 28294 RVA: 0x0026C49C File Offset: 0x0026A69C
		public bool ConditionIsActive(GameConditionDef def)
		{
			return this.GetActiveCondition(def) != null;
		}

		// Token: 0x06006E87 RID: 28295 RVA: 0x0026C4A8 File Offset: 0x0026A6A8
		public GameCondition GetActiveCondition(GameConditionDef def)
		{
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				if (def == this.activeConditions[i].def)
				{
					return this.activeConditions[i];
				}
			}
			GameConditionManager parent = this.Parent;
			if (parent == null)
			{
				return null;
			}
			return parent.GetActiveCondition(def);
		}

		// Token: 0x06006E88 RID: 28296 RVA: 0x0026C500 File Offset: 0x0026A700
		public T GetActiveCondition<T>() where T : GameCondition
		{
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				T t = this.activeConditions[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			GameConditionManager parent = this.Parent;
			if (parent == null)
			{
				return default(T);
			}
			return parent.GetActiveCondition<T>();
		}

		// Token: 0x06006E89 RID: 28297 RVA: 0x0026C560 File Offset: 0x0026A760
		public PsychicDroneLevel GetHighestPsychicDroneLevelFor(Gender gender, Map map)
		{
			PsychicDroneLevel psychicDroneLevel = PsychicDroneLevel.None;
			for (int i = 0; i < this.ActiveConditions.Count; i++)
			{
				if (this.activeConditions[i].CanApplyOnMap(map))
				{
					GameCondition_PsychicEmanation gameCondition_PsychicEmanation = this.activeConditions[i] as GameCondition_PsychicEmanation;
					if (gameCondition_PsychicEmanation != null && gameCondition_PsychicEmanation.gender == gender && gameCondition_PsychicEmanation.level > psychicDroneLevel)
					{
						psychicDroneLevel = gameCondition_PsychicEmanation.level;
					}
				}
			}
			return psychicDroneLevel;
		}

		// Token: 0x06006E8A RID: 28298 RVA: 0x0026C5C8 File Offset: 0x0026A7C8
		public void GetChildren(List<GameConditionManager> outChildren)
		{
			if (this == Find.World.gameConditionManager)
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					outChildren.Add(maps[i].gameConditionManager);
				}
			}
		}

		// Token: 0x06006E8B RID: 28299 RVA: 0x0026C60C File Offset: 0x0026A80C
		public float TotalHeightAt(float width)
		{
			float num = 0f;
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				if ((this.ownerMap == null || !this.activeConditions[i].HiddenByOtherCondition(this.ownerMap)) && this.activeConditions[i].def.displayOnUI)
				{
					num += Text.CalcHeight(this.activeConditions[i].LabelCap, width - 6f);
				}
			}
			if (this.Parent != null)
			{
				num += this.Parent.TotalHeightAt(width);
			}
			return num;
		}

		// Token: 0x06006E8C RID: 28300 RVA: 0x0026C6A8 File Offset: 0x0026A8A8
		public void DoConditionsUI(Rect rect)
		{
			Widgets.BeginGroup(rect);
			float num = 0f;
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				if (this.activeConditions[i].def.displayOnUI && (this.ownerMap == null || (this.activeConditions[i].CanApplyOnMap(this.ownerMap) && !this.activeConditions[i].HiddenByOtherCondition(this.ownerMap))))
				{
					string labelCap = this.activeConditions[i].LabelCap;
					Rect rect2 = new Rect(0f, num, rect.width, Text.CalcHeight(labelCap, rect.width - 6f));
					Text.Font = GameFont.Small;
					Text.Anchor = TextAnchor.MiddleRight;
					Widgets.DrawHighlightIfMouseover(rect2);
					Rect rect3 = rect2;
					rect3.width -= 6f;
					Widgets.Label(rect3, labelCap);
					if (Mouse.IsOver(rect2))
					{
						TooltipHandler.TipRegion(rect2, new TipSignal(this.activeConditions[i].TooltipString, 976090154 ^ i));
					}
					if (Widgets.ButtonInvisible(rect2, true))
					{
						if (this.activeConditions[i].conditionCauser != null && !this.activeConditions[i].hideSource && CameraJumper.CanJump(this.activeConditions[i].conditionCauser))
						{
							CameraJumper.TryJumpAndSelect(this.activeConditions[i].conditionCauser, CameraJumper.MovementMode.Pan);
						}
						else if (this.activeConditions[i].quest != null)
						{
							Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
							((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(this.activeConditions[i].quest);
						}
					}
					num += rect2.height;
				}
			}
			rect.yMin += num;
			GUI.EndGroup();
			Text.Anchor = TextAnchor.UpperLeft;
			GameConditionManager parent = this.Parent;
			if (parent == null)
			{
				return;
			}
			parent.DoConditionsUI(rect);
		}

		// Token: 0x06006E8D RID: 28301 RVA: 0x0026C8B8 File Offset: 0x0026AAB8
		public void GetAllGameConditionsAffectingMap(Map map, List<GameCondition> listToFill)
		{
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				if (this.activeConditions[i].CanApplyOnMap(map))
				{
					listToFill.Add(this.activeConditions[i]);
				}
			}
			if (this.Parent != null)
			{
				this.Parent.GetAllGameConditionsAffectingMap(map, listToFill);
			}
		}

		// Token: 0x06006E8E RID: 28302 RVA: 0x0026C918 File Offset: 0x0026AB18
		internal float AggregateTemperatureOffset()
		{
			float num = 0f;
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				if (this.activeConditions[i].CanApplyOnMap(this.ownerMap))
				{
					num += this.activeConditions[i].TemperatureOffset();
				}
			}
			if (this.Parent != null)
			{
				num += this.Parent.AggregateTemperatureOffset();
			}
			return num;
		}

		// Token: 0x06006E8F RID: 28303 RVA: 0x0026C988 File Offset: 0x0026AB88
		internal float AggregateAnimalDensityFactor(Map map)
		{
			float num = 1f;
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				if (this.activeConditions[i].CanApplyOnMap(this.ownerMap))
				{
					num *= this.activeConditions[i].AnimalDensityFactor(map);
				}
			}
			if (this.Parent != null)
			{
				num *= this.Parent.AggregateAnimalDensityFactor(map);
			}
			return num;
		}

		// Token: 0x06006E90 RID: 28304 RVA: 0x0026C9F8 File Offset: 0x0026ABF8
		internal float AggregatePlantDensityFactor(Map map)
		{
			float num = 1f;
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				if (this.activeConditions[i].CanApplyOnMap(this.ownerMap))
				{
					num *= this.activeConditions[i].PlantDensityFactor(map);
				}
			}
			if (this.Parent != null)
			{
				num *= this.Parent.AggregatePlantDensityFactor(map);
			}
			return num;
		}

		// Token: 0x06006E91 RID: 28305 RVA: 0x0026CA68 File Offset: 0x0026AC68
		internal float AggregateSkyGazeJoyGainFactor(Map map)
		{
			float num = 1f;
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				if (this.activeConditions[i].CanApplyOnMap(this.ownerMap))
				{
					num *= this.activeConditions[i].SkyGazeJoyGainFactor(map);
				}
			}
			if (this.Parent != null)
			{
				num *= this.Parent.AggregateSkyGazeJoyGainFactor(map);
			}
			return num;
		}

		// Token: 0x06006E92 RID: 28306 RVA: 0x0026CAD8 File Offset: 0x0026ACD8
		internal float AggregateSkyGazeChanceFactor(Map map)
		{
			float num = 1f;
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				if (this.activeConditions[i].CanApplyOnMap(this.ownerMap))
				{
					num *= this.activeConditions[i].SkyGazeChanceFactor(map);
				}
			}
			if (this.Parent != null)
			{
				num *= this.Parent.AggregateSkyGazeChanceFactor(map);
			}
			return num;
		}

		// Token: 0x06006E93 RID: 28307 RVA: 0x0026CB48 File Offset: 0x0026AD48
		internal bool AllowEnjoyableOutsideNow(Map map)
		{
			GameConditionDef gameConditionDef;
			return this.AllowEnjoyableOutsideNow(map, out gameConditionDef);
		}

		// Token: 0x06006E94 RID: 28308 RVA: 0x0026CB60 File Offset: 0x0026AD60
		internal bool AllowEnjoyableOutsideNow(Map map, out GameConditionDef reason)
		{
			for (int i = 0; i < this.activeConditions.Count; i++)
			{
				GameCondition gameCondition = this.activeConditions[i];
				if (this.activeConditions[i].CanApplyOnMap(this.ownerMap) && !gameCondition.AllowEnjoyableOutsideNow(map))
				{
					reason = gameCondition.def;
					return false;
				}
			}
			reason = null;
			return this.Parent == null || this.Parent.AllowEnjoyableOutsideNow(map, out reason);
		}

		// Token: 0x06006E95 RID: 28309 RVA: 0x0026CBD8 File Offset: 0x0026ADD8
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (GameCondition saveable in this.activeConditions)
			{
				stringBuilder.AppendLine(Scribe.saver.DebugOutputFor(saveable));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040046C5 RID: 18117
		public Map ownerMap;

		// Token: 0x040046C6 RID: 18118
		private List<GameCondition> activeConditions = new List<GameCondition>();

		// Token: 0x040046C7 RID: 18119
		private GameConditionManager.MapBrightnessTracker mapBrightnessTracker;

		// Token: 0x040046C8 RID: 18120
		private bool cachedAlwaysDark;

		// Token: 0x040046C9 RID: 18121
		private const float TextPadding = 6f;

		// Token: 0x02003081 RID: 12417
		private class MapBrightnessTracker : IExposable
		{
			// Token: 0x170030A5 RID: 12453
			// (get) Token: 0x0601253B RID: 75067 RVA: 0x006308CB File Offset: 0x0062EACB
			public float CurBrightness
			{
				get
				{
					return Mathf.Lerp(this.brightness, this.targetBrightness, this.lerp);
				}
			}

			// Token: 0x170030A6 RID: 12454
			// (get) Token: 0x0601253C RID: 75068 RVA: 0x006308E4 File Offset: 0x0062EAE4
			public bool Changing
			{
				get
				{
					return this.lerp < 1f;
				}
			}

			// Token: 0x170030A7 RID: 12455
			// (get) Token: 0x0601253D RID: 75069 RVA: 0x006308F3 File Offset: 0x0062EAF3
			public bool DarknessVisible
			{
				get
				{
					return this.Changing || this.CurBrightness < 1f;
				}
			}

			// Token: 0x0601253E RID: 75070 RVA: 0x0063090C File Offset: 0x0062EB0C
			public void Change(float newBrightness, float lerpSeconds = 5f)
			{
				if (!ModLister.CheckAnomaly("Map brightness"))
				{
					return;
				}
				this.brightness = this.CurBrightness;
				this.targetBrightness = newBrightness;
				this.lerp = 0f;
				this.lerpSeconds = lerpSeconds;
			}

			// Token: 0x0601253F RID: 75071 RVA: 0x00630940 File Offset: 0x0062EB40
			public void Tick()
			{
				if (ModsConfig.AnomalyActive && this.lerp < 1f)
				{
					this.lerp += this.lerpSeconds / 60f * Time.deltaTime;
				}
			}

			// Token: 0x06012540 RID: 75072 RVA: 0x00630978 File Offset: 0x0062EB78
			public void ExposeData()
			{
				Scribe_Values.Look<float>(ref this.brightness, "brightness", 1f, false);
				Scribe_Values.Look<float>(ref this.targetBrightness, "targetBrightness", 1f, false);
				Scribe_Values.Look<float>(ref this.lerp, "lerp", 1f, false);
				Scribe_Values.Look<float>(ref this.lerpSeconds, "lerpSeconds", 5f, false);
			}

			// Token: 0x0400C50B RID: 50443
			private float brightness = 1f;

			// Token: 0x0400C50C RID: 50444
			private float targetBrightness = 1f;

			// Token: 0x0400C50D RID: 50445
			private float lerp = 1f;

			// Token: 0x0400C50E RID: 50446
			private float lerpSeconds;
		}
	}
}
