using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000FCD RID: 4045
	public class QuestScriptDef : Def
	{
		// Token: 0x17001198 RID: 4504
		// (get) Token: 0x0600669D RID: 26269 RVA: 0x00246BE8 File Offset: 0x00244DE8
		public bool IsRootRandomSelected
		{
			get
			{
				return this.rootSelectionWeight != 0f && this.randomlySelectable;
			}
		}

		// Token: 0x17001199 RID: 4505
		// (get) Token: 0x0600669E RID: 26270 RVA: 0x00246BFF File Offset: 0x00244DFF
		public bool IsRootDecree
		{
			get
			{
				return this.decreeSelectionWeight != 0f;
			}
		}

		// Token: 0x1700119A RID: 4506
		// (get) Token: 0x0600669F RID: 26271 RVA: 0x00246C11 File Offset: 0x00244E11
		public bool IsRootAny
		{
			get
			{
				return this.IsRootRandomSelected || this.IsRootDecree || this.isRootSpecial;
			}
		}

		// Token: 0x1700119B RID: 4507
		// (get) Token: 0x060066A0 RID: 26272 RVA: 0x00246C2B File Offset: 0x00244E2B
		public bool IsEpic
		{
			get
			{
				return this.epic;
			}
		}

		// Token: 0x060066A1 RID: 26273 RVA: 0x00246C33 File Offset: 0x00244E33
		public void Run()
		{
			this.InitializeRules();
			this.root.Run();
		}

		// Token: 0x060066A2 RID: 26274 RVA: 0x00246C48 File Offset: 0x00244E48
		public void InitializeRules()
		{
			if (this.questDescriptionRules != null)
			{
				QuestGen.AddQuestDescriptionRules(this.questDescriptionRules);
			}
			if (this.questNameRules != null)
			{
				QuestGen.AddQuestNameRules(this.questNameRules);
			}
			if (this.questDescriptionAndNameRules != null)
			{
				QuestGen.AddQuestDescriptionRules(this.questDescriptionAndNameRules);
				QuestGen.AddQuestNameRules(this.questDescriptionAndNameRules);
			}
			if (this.questContentRules != null)
			{
				QuestGen.AddQuestContentRules(this.questContentRules);
			}
		}

		// Token: 0x060066A3 RID: 26275 RVA: 0x00246CAC File Offset: 0x00244EAC
		public bool CanRun(Slate slate, IIncidentTarget target)
		{
			bool result;
			using (new ProfilerBlock("CanRun()"))
			{
				int num;
				if (!slate.TryGet<int>("points", out num, false))
				{
					num = 0;
				}
				if (this.lastCheckCanRunTick == Find.TickManager.TicksGame && this.lastCheckCanRunPoints == num)
				{
					result = this.lastCanRunResult;
				}
				else
				{
					this.lastCheckCanRunTick = Find.TickManager.TicksGame;
					this.lastCheckCanRunPoints = num;
					try
					{
						this.lastCanRunResult = (target != null && this.CanQuestOccurOnTile(target.Tile) && this.root.TestRun(slate.DeepCopy()));
						Scenario scenario = Find.Scenario;
						int num2 = 0;
						while (num2 < scenario.parts.Count && this.lastCanRunResult)
						{
							ScenPart_DisableQuest scenPart_DisableQuest = scenario.parts[num2] as ScenPart_DisableQuest;
							if (scenPart_DisableQuest != null && scenPart_DisableQuest.questDef == this)
							{
								this.lastCanRunResult = false;
							}
							num2++;
						}
					}
					catch (Exception value)
					{
						this.lastCanRunResult = false;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(44, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Error while checking if can generate quest: ");
						defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
						Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
						throw;
					}
					result = this.lastCanRunResult;
				}
			}
			return result;
		}

		// Token: 0x060066A4 RID: 26276 RVA: 0x00246E18 File Offset: 0x00245018
		private bool CanQuestOccurOnTile(PlanetTile tile)
		{
			if (!tile.Valid)
			{
				return true;
			}
			PlanetLayerDef layerDef = tile.LayerDef;
			return (this.layerWhitelist.NullOrEmpty<PlanetLayerDef>() || this.layerWhitelist.Contains(layerDef)) && (this.layerBlacklist.NullOrEmpty<PlanetLayerDef>() || !this.layerBlacklist.Contains(layerDef)) && (this.autoAccept || this.everAcceptableInSpace || !layerDef.isSpace) && (!this.neverPossibleInSpace || !layerDef.isSpace);
		}

		// Token: 0x060066A5 RID: 26277 RVA: 0x00246EA0 File Offset: 0x002450A0
		public MapParent TryFindNewSuitableMapParentForRetarget()
		{
			foreach (Map map in Current.Game.PlayerHomeMaps)
			{
				if (this.CanQuestOccurOnTile(map.Tile))
				{
					return map.Parent;
				}
			}
			foreach (Map map2 in Current.Game.Maps)
			{
				if (this.CanQuestOccurOnTile(map2.Tile) && GravshipUtility.GetPlayerGravEngine_NewTemp(map2) != null)
				{
					return map2.Parent;
				}
			}
			return null;
		}

		// Token: 0x060066A6 RID: 26278 RVA: 0x00246F68 File Offset: 0x00245168
		public bool IsParentSuitableForQuest(MapParent mapParent)
		{
			return this.CanQuestOccurOnTile(mapParent.Tile);
		}

		// Token: 0x060066A7 RID: 26279 RVA: 0x00246F78 File Offset: 0x00245178
		public bool CanRun(float points, IIncidentTarget target)
		{
			Slate slate = new Slate();
			slate.Set<float>("points", points, false);
			return this.CanRun(slate, target);
		}

		// Token: 0x060066A8 RID: 26280 RVA: 0x00246FA0 File Offset: 0x002451A0
		public override IEnumerable<string> ConfigErrors()
		{
			QuestScriptDef.<ConfigErrors>d__61 <ConfigErrors>d__ = new QuestScriptDef.<ConfigErrors>d__61(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04003F93 RID: 16275
		public QuestNode root;

		// Token: 0x04003F94 RID: 16276
		public float rootSelectionWeight;

		// Token: 0x04003F95 RID: 16277
		public SimpleCurve rootSelectionWeightFactorFromPointsCurve;

		// Token: 0x04003F96 RID: 16278
		public bool randomlySelectable = true;

		// Token: 0x04003F97 RID: 16279
		public float rootMinPoints;

		// Token: 0x04003F98 RID: 16280
		public float rootMinProgressScore;

		// Token: 0x04003F99 RID: 16281
		public int rootEarliestDay;

		// Token: 0x04003F9A RID: 16282
		public bool rootIncreasesPopulation;

		// Token: 0x04003F9B RID: 16283
		public float minRefireDays;

		// Token: 0x04003F9C RID: 16284
		public float decreeSelectionWeight;

		// Token: 0x04003F9D RID: 16285
		public List<string> decreeTags;

		// Token: 0x04003F9E RID: 16286
		public RulePack questDescriptionRules;

		// Token: 0x04003F9F RID: 16287
		public RulePack questNameRules;

		// Token: 0x04003FA0 RID: 16288
		public RulePack questDescriptionAndNameRules;

		// Token: 0x04003FA1 RID: 16289
		public RulePack questContentRules;

		// Token: 0x04003FA2 RID: 16290
		public RulePack questSubjectRules;

		// Token: 0x04003FA3 RID: 16291
		public bool autoAccept;

		// Token: 0x04003FA4 RID: 16292
		public bool hideOnCleanup;

		// Token: 0x04003FA5 RID: 16293
		public FloatRange expireDaysRange = new FloatRange(-1f, -1f);

		// Token: 0x04003FA6 RID: 16294
		public bool nameMustBeUnique;

		// Token: 0x04003FA7 RID: 16295
		public int defaultChallengeRating = -1;

		// Token: 0x04003FA8 RID: 16296
		public bool defaultHidden;

		// Token: 0x04003FA9 RID: 16297
		public bool isRootSpecial;

		// Token: 0x04003FAA RID: 16298
		public bool canGiveRoyalFavor;

		// Token: 0x04003FAB RID: 16299
		public string questAvailableLetterLabel;

		// Token: 0x04003FAC RID: 16300
		public LetterDef questAvailableLetterDef;

		// Token: 0x04003FAD RID: 16301
		public bool questAvailableLetterTextIsDescription;

		// Token: 0x04003FAE RID: 16302
		public bool hideInvolvedFactionsInfo;

		// Token: 0x04003FAF RID: 16303
		public bool affectedByPopulation;

		// Token: 0x04003FB0 RID: 16304
		public bool affectedByPoints = true;

		// Token: 0x04003FB1 RID: 16305
		public bool defaultCharity;

		// Token: 0x04003FB2 RID: 16306
		public HistoryEventDef successHistoryEvent;

		// Token: 0x04003FB3 RID: 16307
		public HistoryEventDef failedOrExpiredHistoryEvent;

		// Token: 0x04003FB4 RID: 16308
		public bool sendAvailableLetter = true;

		// Token: 0x04003FB5 RID: 16309
		public bool epic;

		// Token: 0x04003FB6 RID: 16310
		public QuestScriptDef epicParent;

		// Token: 0x04003FB7 RID: 16311
		public bool endOnColonyMove = true;

		// Token: 0x04003FB8 RID: 16312
		public bool everAcceptableInSpace;

		// Token: 0x04003FB9 RID: 16313
		public bool neverPossibleInSpace;

		// Token: 0x04003FBA RID: 16314
		public List<QuestGiverTag> givenBy = new List<QuestGiverTag>();

		// Token: 0x04003FBB RID: 16315
		public List<PlanetLayerDef> layerWhitelist;

		// Token: 0x04003FBC RID: 16316
		public List<PlanetLayerDef> layerBlacklist;

		// Token: 0x04003FBD RID: 16317
		public bool canOccurOnAllPlanetLayers;

		// Token: 0x04003FBE RID: 16318
		[Unsaved(false)]
		private int lastCheckCanRunTick;

		// Token: 0x04003FBF RID: 16319
		[Unsaved(false)]
		private int lastCheckCanRunPoints;

		// Token: 0x04003FC0 RID: 16320
		[Unsaved(false)]
		private bool lastCanRunResult;
	}
}
