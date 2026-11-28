using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001AD1 RID: 6865
	public class Building_VoidMonolith : Building, ITargetingSource, IThingGlower
	{
		// Token: 0x17001F6B RID: 8043
		// (get) Token: 0x0600AECA RID: 44746 RVA: 0x003D8A30 File Offset: 0x003D6C30
		public override int? OverrideGraphicIndex
		{
			get
			{
				return new int?(Find.Anomaly.LevelDef.graphicIndex);
			}
		}

		// Token: 0x17001F6C RID: 8044
		// (get) Token: 0x0600AECB RID: 44747 RVA: 0x003D8A46 File Offset: 0x003D6C46
		public bool IsAutoActivating
		{
			get
			{
				return this.autoActivateTick > 0 && Find.TickManager.TicksGame > this.autoActivateTick - 300000;
			}
		}

		// Token: 0x17001F6D RID: 8045
		// (get) Token: 0x0600AECC RID: 44748 RVA: 0x003D8A6B File Offset: 0x003D6C6B
		public int TicksUntilAutoActivate
		{
			get
			{
				return this.autoActivateTick - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x17001F6E RID: 8046
		// (get) Token: 0x0600AECD RID: 44749 RVA: 0x003D8A7E File Offset: 0x003D6C7E
		private TargetInfo EffecterInfo
		{
			get
			{
				return new TargetInfo(base.Position, base.Map, false);
			}
		}

		// Token: 0x17001F6F RID: 8047
		// (get) Token: 0x0600AECE RID: 44750 RVA: 0x003D8A94 File Offset: 0x003D6C94
		public override CellRect? CustomRectForSelector
		{
			get
			{
				return new CellRect?(GenAdj.OccupiedRect(base.Position, Rot4.North, Find.Anomaly.LevelDef.sizeIncludingAttachments ?? this.def.Size));
			}
		}

		// Token: 0x17001F70 RID: 8048
		// (get) Token: 0x0600AECF RID: 44751 RVA: 0x003D8AE3 File Offset: 0x003D6CE3
		public override Texture UIIconOverride
		{
			get
			{
				return Find.Anomaly.LevelDef.UIIcon;
			}
		}

		// Token: 0x17001F71 RID: 8049
		// (get) Token: 0x0600AED0 RID: 44752 RVA: 0x003D8AF4 File Offset: 0x003D6CF4
		public override string LabelNoCount
		{
			get
			{
				if (Find.Anomaly.LevelDef.monolithLabel != null)
				{
					return Find.Anomaly.LevelDef.monolithLabel;
				}
				return base.LabelNoCount;
			}
		}

		// Token: 0x17001F72 RID: 8050
		// (get) Token: 0x0600AED1 RID: 44753 RVA: 0x003D8B1D File Offset: 0x003D6D1D
		public override string DescriptionFlavor
		{
			get
			{
				if (Find.Anomaly.LevelDef.monolithDescription != null)
				{
					return Find.Anomaly.LevelDef.monolithDescription;
				}
				return base.DescriptionFlavor;
			}
		}

		// Token: 0x0600AED2 RID: 44754 RVA: 0x003D8B46 File Offset: 0x003D6D46
		public bool ShouldBeLitNow()
		{
			return Find.Anomaly.LevelDef.monolithGlows;
		}

		// Token: 0x17001F73 RID: 8051
		// (get) Token: 0x0600AED3 RID: 44755 RVA: 0x003D8B57 File Offset: 0x003D6D57
		public bool CasterIsPawn
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001F74 RID: 8052
		// (get) Token: 0x0600AED4 RID: 44756 RVA: 0x003D8B5A File Offset: 0x003D6D5A
		public bool IsMeleeAttack
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001F75 RID: 8053
		// (get) Token: 0x0600AED5 RID: 44757 RVA: 0x003D8B5D File Offset: 0x003D6D5D
		public bool Targetable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001F76 RID: 8054
		// (get) Token: 0x0600AED6 RID: 44758 RVA: 0x003D8B60 File Offset: 0x003D6D60
		public bool MultiSelect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001F77 RID: 8055
		// (get) Token: 0x0600AED7 RID: 44759 RVA: 0x003D8B63 File Offset: 0x003D6D63
		public bool HidePawnTooltips
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001F78 RID: 8056
		// (get) Token: 0x0600AED8 RID: 44760 RVA: 0x003D8B66 File Offset: 0x003D6D66
		public Thing Caster
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17001F79 RID: 8057
		// (get) Token: 0x0600AED9 RID: 44761 RVA: 0x003D8B69 File Offset: 0x003D6D69
		public Pawn CasterPawn
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001F7A RID: 8058
		// (get) Token: 0x0600AEDA RID: 44762 RVA: 0x003D8B6C File Offset: 0x003D6D6C
		public Verb GetVerb
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001F7B RID: 8059
		// (get) Token: 0x0600AEDB RID: 44763 RVA: 0x003D8B6F File Offset: 0x003D6D6F
		public TargetingParameters targetParams
		{
			get
			{
				return Building_VoidMonolith.targetParmsInt;
			}
		}

		// Token: 0x17001F7C RID: 8060
		// (get) Token: 0x0600AEDC RID: 44764 RVA: 0x003D8B76 File Offset: 0x003D6D76
		public virtual ITargetingSource DestinationSelector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001F7D RID: 8061
		// (get) Token: 0x0600AEDD RID: 44765 RVA: 0x003D8B79 File Offset: 0x003D6D79
		public Texture2D UIIcon
		{
			get
			{
				if (Find.Anomaly.Level != 0)
				{
					return ContentFinder<Texture2D>.Get("UI/Commands/ActivateMonolith", true);
				}
				return ContentFinder<Texture2D>.Get("UI/Commands/ActivateMonolithInitial", true);
			}
		}

		// Token: 0x0600AEDE RID: 44766 RVA: 0x003D8BA0 File Offset: 0x003D6DA0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Quest>(ref this.quest, "quest", false);
			Scribe_Values.Look<int>(ref this.disturbingVisionTick, "disturbingVisionTick", 0, false);
			Scribe_Values.Look<int>(ref this.autoActivateTick, "autoActivateTick", 0, false);
			Scribe_Collections.Look<Thing>(ref this.monolithAttachments, "monolithAttachments", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.activatedDialogTick, "activatedDialogTick", 0, false);
			Scribe_References.Look<Pawn>(ref this.activatorPawn, "activatorPawn", false);
			Scribe_References.Look<Thing>(ref this.pyramidThing, "pyramidThing", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.monolithAttachments == null)
			{
				this.monolithAttachments = new List<Thing>();
			}
		}

		// Token: 0x0600AEDF RID: 44767 RVA: 0x003D8C50 File Offset: 0x003D6E50
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (!ModLister.CheckAnomaly("Void monolith"))
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			if (!respawningAfterLoad)
			{
				if (Find.Anomaly.Level > 0)
				{
					this.TryGetComp<CompProximityLetter>().letterSent = true;
				}
				if (Find.Anomaly.monolith == null)
				{
					Find.Anomaly.monolith = this;
				}
				this.disturbingVisionTick = Find.TickManager.TicksGame + Mathf.RoundToInt(Building_VoidMonolith.DisturbingVisionRangeDaysRange.RandomInRange * 60000f);
			}
			base.SpawnSetup(map, respawningAfterLoad);
			this.UpdateAttachments();
		}

		// Token: 0x0600AEE0 RID: 44768 RVA: 0x003D8CDC File Offset: 0x003D6EDC
		protected override void Tick()
		{
			base.Tick();
			if (this.IsHashIntervalTick(30))
			{
				if (Find.Anomaly.Level == 0 && !base.Map.reservationManager.IsReserved(this))
				{
					if (this.level0HintEffecter == null)
					{
						this.level0HintEffecter = EffecterDefOf.MonolithL0Glow.Spawn(this.EffecterInfo, this.EffecterInfo, 1f);
					}
				}
				else
				{
					Effecter effecter = this.level0HintEffecter;
					if (effecter != null)
					{
						effecter.Cleanup();
					}
					this.level0HintEffecter = null;
				}
			}
			Effecter effecter2 = this.level0HintEffecter;
			if (effecter2 != null)
			{
				effecter2.EffectTick(this.EffecterInfo, this.EffecterInfo);
			}
			if (Find.Anomaly.Level == 0 && this.disturbingVisionTick > 0 && Find.TickManager.TicksGame > this.disturbingVisionTick)
			{
				Pawn arg;
				if (base.Map.mapPawns.FreeColonistsSpawned.TryRandomElement(out arg))
				{
					Find.LetterStack.ReceiveLetter("VoidMonolithVisionLabel".Translate(), "VoidMonolithVisionText".Translate(arg.Named("PAWN")), LetterDefOf.NeutralEvent, this, null, null, null, null, 0, true);
					this.disturbingVisionTick = -99999;
				}
				else
				{
					this.disturbingVisionTick += 15000;
				}
			}
			if (this.autoActivateTick > 0)
			{
				if (Find.TickManager.TicksGame == this.autoActivateTick - 300000)
				{
					Find.LetterStack.ReceiveLetter("MonolithAutoActivatingLabel".Translate(), "MonolithAutoActivatingText".Translate(), LetterDefOf.NegativeEvent, this, null, null, null, null, 0, true);
				}
				if (Find.TickManager.TicksGame == this.autoActivateTick - 60)
				{
					Find.LetterStack.ReceiveLetter("MonolithAutoActivatedLabel".Translate(), "MonolithAutoActivatedText".Translate(), LetterDefOf.ThreatBig, this, null, null, null, null, 0, true);
				}
				Pawn pawn;
				if (Find.TickManager.TicksGame > this.autoActivateTick && base.Map.mapPawns.FreeColonists.TryRandomElement(out pawn))
				{
					this.Activate(pawn);
				}
			}
			if (this.IsAutoActivating)
			{
				if (this.autoActivateEffecter == null)
				{
					this.autoActivateEffecter = EffecterDefOf.MonolithAutoActivating.Spawn();
				}
				this.autoActivateEffecter.EffectTick(this.EffecterInfo, this.EffecterInfo);
			}
			else if (this.autoActivateEffecter != null)
			{
				this.autoActivateEffecter.Cleanup();
				this.autoActivateEffecter = null;
			}
			if (Find.Anomaly.Level == MonolithLevelDefOf.Gleaming.level && Find.CurrentMap == base.MapHeld)
			{
				if (this.gleamingEffecter == null)
				{
					this.gleamingEffecter = EffecterDefOf.MonolithGleaming_Sustained.Spawn();
				}
				this.gleamingEffecter.EffectTick(this.EffecterInfo, this.EffecterInfo);
				if (this.gleamingVoidNodeEffecter == null)
				{
					this.gleamingVoidNodeEffecter = EffecterDefOf.MonolithGleamingVoidNode.Spawn(this, base.Map, 1f);
				}
				this.gleamingVoidNodeEffecter.EffectTick(this.EffecterInfo, this.EffecterInfo);
			}
			else if (this.gleamingEffecter != null || Find.CurrentMap != base.MapHeld)
			{
				Effecter effecter3 = this.gleamingEffecter;
				if (effecter3 != null)
				{
					effecter3.Cleanup();
				}
				this.gleamingEffecter = null;
				Effecter effecter4 = this.gleamingVoidNodeEffecter;
				if (effecter4 != null)
				{
					effecter4.Cleanup();
				}
				this.gleamingVoidNodeEffecter = null;
			}
			if (this.activatedDialogTick > 0 && Find.TickManager.TicksGame > this.activatedDialogTick)
			{
				this.OpenActivatedDialog();
			}
		}

		// Token: 0x0600AEE1 RID: 44769 RVA: 0x003D9030 File Offset: 0x003D7230
		public bool CanActivate(out string reason, out string reasonShort)
		{
			reason = "";
			reasonShort = "";
			if (!Find.Anomaly.LevelDef.advanceThroughActivation)
			{
				return false;
			}
			MonolithLevelDef nextLevelDef = Find.Anomaly.NextLevelDef;
			if (nextLevelDef == null)
			{
				return false;
			}
			if (nextLevelDef.entityCatagoryCompletionRequired != null && Find.EntityCodex.DiscoveredCount(nextLevelDef.entityCatagoryCompletionRequired) < nextLevelDef.entityCountCompletionRequired)
			{
				int value = nextLevelDef.entityCountCompletionRequired - Find.EntityCodex.DiscoveredCount(nextLevelDef.entityCatagoryCompletionRequired);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 2);
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("VoidMonolithRequiresDiscovery".Translate());
				defaultInterpolatedStringHandler.AppendLiteral(":\n  - ");
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("VoidMonolithRequiresCategory".Translate(value, nextLevelDef.entityCatagoryCompletionRequired.label));
				reason = defaultInterpolatedStringHandler.ToStringAndClear();
				reasonShort = "VoidMonolithRequiresDiscoveryShort".Translate();
				return false;
			}
			foreach (GameCondition gameCondition in base.Map.GameConditionManager.ActiveConditions)
			{
				List<GameConditionDef> unreachableDuringConditions = nextLevelDef.unreachableDuringConditions;
				if (unreachableDuringConditions != null && unreachableDuringConditions.Contains(gameCondition.def))
				{
					reason = gameCondition.def.LabelCap;
					reasonShort = gameCondition.def.LabelCap;
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600AEE2 RID: 44770 RVA: 0x003D91AC File Offset: 0x003D73AC
		public void Activate(Pawn pawn)
		{
			this.CheckAndGenerateQuest();
			Find.Anomaly.IncrementLevel();
			EffecterDefOf.MonolithLevelChanged.Spawn().Trigger(this.EffecterInfo, this.EffecterInfo, -1);
			this.activatedDialogTick = Find.TickManager.TicksGame + 360;
			this.activatorPawn = pawn;
			if (Find.Anomaly.Level == 1)
			{
				CompStudiable comp = base.GetComp<CompStudiable>();
				if (comp != null)
				{
					comp.SetStudyEnabled(true);
				}
			}
			this.autoActivateTick = -99999;
		}

		// Token: 0x0600AEE3 RID: 44771 RVA: 0x003D9230 File Offset: 0x003D7430
		public void CheckAndGenerateQuest()
		{
			if (Find.Anomaly.monolith == null)
			{
				Find.Anomaly.monolith = this;
			}
			if (this.quest == null && !Find.Anomaly.LevelDef.postEndgame && Find.Anomaly.GenerateMonolith)
			{
				Slate slate = new Slate();
				slate.Set<Map>("map", base.Map, false);
				slate.Set<Building_VoidMonolith>("monolith", this, false);
				this.quest = QuestUtility.GenerateQuestAndMakeAvailable(QuestScriptDefOf.EndGame_VoidMonolith, slate);
				Find.SignalManager.SendSignal(new Signal("MonolithLevelChanged", true));
			}
		}

		// Token: 0x0600AEE4 RID: 44772 RVA: 0x003D92C4 File Offset: 0x003D74C4
		private void OpenActivatedDialog()
		{
			DiaNode diaNode = new DiaNode(Find.Anomaly.LevelDef.activatedLetterText.Formatted(this.activatorPawn.Named("PAWN")));
			diaNode.options.Add(new DiaOption("VoidMonolithViewQuest".Translate())
			{
				action = delegate()
				{
					Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
					((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(this.quest);
				},
				resolveTree = true
			});
			if (Find.Anomaly.LevelDef != MonolithLevelDefOf.VoidAwakened)
			{
				List<DiaOption> options = diaNode.options;
				DiaOption diaOption = new DiaOption("VoidMonolithViewResearch".Translate());
				diaOption.action = delegate()
				{
					Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Research, true);
					((MainTabWindow_Research)MainButtonDefOf.Research.TabWindow).CurTab = ResearchTabDefOf.Anomaly;
				};
				diaOption.resolveTree = true;
				options.Add(diaOption);
			}
			if (Find.Anomaly.Level == 1)
			{
				List<DiaOption> options2 = diaNode.options;
				DiaOption diaOption2 = new DiaOption("ViewEntityCodex".Translate());
				diaOption2.action = delegate()
				{
					Find.WindowStack.Add(new Dialog_EntityCodex(null));
				};
				diaOption2.resolveTree = true;
				options2.Add(diaOption2);
			}
			diaNode.options.Add(new DiaOption("Close".Translate())
			{
				resolveTree = true
			});
			Dialog_NodeTree dialog_NodeTree = new Dialog_NodeTree(diaNode, false, false, null);
			dialog_NodeTree.forcePause = true;
			Find.WindowStack.Add(dialog_NodeTree);
			this.activatedDialogTick = -99999;
			this.activatorPawn = null;
		}

		// Token: 0x0600AEE5 RID: 44773 RVA: 0x003D943C File Offset: 0x003D763C
		public void Investigate(Pawn pawn)
		{
			Dialog_NodeTree dialog_NodeTree = new Dialog_NodeTree(new DiaNode("VoidMonolithInvestigatedText".Translate(pawn.Named("PAWN")))
			{
				options = 
				{
					new DiaOption("VoidMonolithInvestigate".Translate())
					{
						action = delegate()
						{
							if (pawn != null)
							{
								Job job = JobMaker.MakeJob(JobDefOf.ActivateMonolith, this);
								pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
							}
						},
						resolveTree = true
					},
					new DiaOption("VoidMonolithWalkAway".Translate())
					{
						resolveTree = true
					}
				}
			}, false, false, null);
			dialog_NodeTree.forcePause = true;
			Find.WindowStack.Add(dialog_NodeTree);
		}

		// Token: 0x0600AEE6 RID: 44774 RVA: 0x003D94F8 File Offset: 0x003D76F8
		public void SetLevel(MonolithLevelDef levelDef)
		{
			CompGlower comp = base.GetComp<CompGlower>();
			int monolithGlowRadiusOverride = levelDef.monolithGlowRadiusOverride;
			if (monolithGlowRadiusOverride != -1)
			{
				comp.GlowRadius = (float)monolithGlowRadiusOverride;
			}
			comp.UpdateLit(base.Map);
			if (comp.Glows)
			{
				comp.ForceRegister(base.Map);
			}
			if (base.Spawned)
			{
				this.UpdateAttachments();
				base.DirtyMapMesh(base.Map);
			}
			if (Find.Anomaly.LevelDef.activatedSound != null)
			{
				Find.Anomaly.LevelDef.activatedSound.PlayOneShot(this);
			}
			if (levelDef.level == MonolithLevelDefOf.Gleaming.level)
			{
				EffecterDefOf.MonolithGleaming_Transition.SpawnMaintained(base.PositionHeld, base.Map, 1f);
			}
		}

		// Token: 0x0600AEE7 RID: 44775 RVA: 0x003D95B4 File Offset: 0x003D77B4
		private void UpdateAttachments()
		{
			TerrainDef newTerr = base.Map.Biome.TerrainForAffordance(this.def.terrainAffordanceNeeded);
			Thing.allowDestroyNonDestroyable = true;
			foreach (Thing thing in this.monolithAttachments)
			{
				if (!thing.Destroyed)
				{
					thing.Destroy(DestroyMode.Vanish);
				}
			}
			Thing.allowDestroyNonDestroyable = false;
			this.monolithAttachments.Clear();
			if (Find.Anomaly.LevelDef.attachments == null)
			{
				return;
			}
			foreach (MonolithAttachment monolithAttachment in Find.Anomaly.LevelDef.attachments)
			{
				IntVec3 intVec = base.Position + monolithAttachment.offset.ToIntVec3;
				foreach (IntVec3 c in GenAdj.OccupiedRect(intVec, Rot4.North, monolithAttachment.def.Size))
				{
					if (!c.GetAffordances(base.Map).Contains(this.def.terrainAffordanceNeeded))
					{
						base.Map.terrainGrid.RemoveTopLayer(c, false);
						base.Map.terrainGrid.SetTerrain(c, newTerr);
					}
				}
				Thing thing2 = ThingMaker.MakeThing(monolithAttachment.def, null);
				thing2.TryGetComp<CompSelectProxy>().thingToSelect = this;
				GenSpawn.Spawn(thing2, intVec, base.Map, Rot4.North, WipeMode.FullRefund, false, true);
				thing2.overrideGraphicIndex = new int?(monolithAttachment.graphicIndex);
				thing2.DirtyMapMesh(base.Map);
				this.monolithAttachments.Add(thing2);
			}
		}

		// Token: 0x0600AEE8 RID: 44776 RVA: 0x003D97D8 File Offset: 0x003D79D8
		public void Collapse()
		{
			Map map = Find.Anomaly.monolith.Map;
			Predicate<IntVec3> <>9__0;
			for (int i = 0; i < 3; i++)
			{
				IntVec3 position = Find.Anomaly.monolith.Position;
				Map map3 = map;
				int squareRadius = 5;
				Predicate<IntVec3> validator;
				if ((validator = <>9__0) == null)
				{
					validator = (<>9__0 = ((IntVec3 c) => c.Standable(map)));
				}
				IntVec3 loc;
				if (CellFinder.TryFindRandomCellNear(position, map3, squareRadius, validator, out loc, -1))
				{
					GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.MonolithFragment, null), loc, Find.Anomaly.monolith.Map, WipeMode.Vanish);
				}
			}
			int j = 10;
			Predicate<IntVec3> <>9__1;
			while (j > 0)
			{
				IntVec3 position2 = Find.Anomaly.monolith.Position;
				Map map2 = map;
				int squareRadius2 = 5;
				Predicate<IntVec3> validator2;
				if ((validator2 = <>9__1) == null)
				{
					validator2 = (<>9__1 = ((IntVec3 c) => c.Standable(map)));
				}
				IntVec3 loc2;
				if (CellFinder.TryFindRandomCellNear(position2, map2, squareRadius2, validator2, out loc2, -1))
				{
					Thing thing = ThingMaker.MakeThing(ThingDefOf.Shard, null);
					thing.stackCount = Mathf.Min(Rand.RangeInclusive(1, 2), j);
					j -= thing.stackCount;
					GenSpawn.Spawn(thing, loc2, Find.Anomaly.monolith.Map, WipeMode.Vanish);
				}
			}
		}

		// Token: 0x0600AEE9 RID: 44777 RVA: 0x003D9904 File Offset: 0x003D7B04
		public void Reset()
		{
			this.quest = null;
			base.GetComp<CompVoidStructure>().Reset();
			this.SetLevel(Find.Anomaly.LevelDef);
		}

		// Token: 0x0600AEEA RID: 44778 RVA: 0x003D9928 File Offset: 0x003D7B28
		public bool CanHitTarget(LocalTargetInfo target)
		{
			return this.ValidateTarget(target, false);
		}

		// Token: 0x0600AEEB RID: 44779 RVA: 0x003D9934 File Offset: 0x003D7B34
		public bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			if (!target.IsValid || target.Pawn == null)
			{
				return false;
			}
			if (target.Pawn.Downed)
			{
				if (showMessages)
				{
					Messages.Message("VoidMonolithActivatorDowned".Translate(), target.Pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (target.Pawn.InMentalState)
			{
				if (showMessages)
				{
					Messages.Message("VoidMonolithActivatorMentalState".Translate(), target.Pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (!target.Pawn.CanCasuallyInteractNow(false, false, false, true))
			{
				if (showMessages)
				{
					Messages.Message("VoidMonolithActivatorBusy".Translate(), target.Pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (!target.Pawn.CanReach(this, PathEndMode.InteractionCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				if (showMessages)
				{
					Messages.Message("NoPath".Translate(), target.Pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600AEEC RID: 44780 RVA: 0x003D9A49 File Offset: 0x003D7C49
		public void DrawHighlight(LocalTargetInfo target)
		{
			if (target.IsValid)
			{
				GenDraw.DrawTargetHighlight(target);
			}
		}

		// Token: 0x0600AEED RID: 44781 RVA: 0x003D9A5C File Offset: 0x003D7C5C
		public void OrderForceTarget(LocalTargetInfo target)
		{
			if (this.ValidateTarget(target, false))
			{
				Pawn pawn = target.Pawn;
				if (Find.Anomaly.Level == 0)
				{
					Job job = JobMaker.MakeJob(JobDefOf.InvestigateMonolith, this);
					pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
					Messages.Message(Find.Anomaly.LevelDef.pawnSentToActivateMessage.Formatted(pawn.Named("PAWN")), this, MessageTypeDefOf.NeutralEvent, true);
					this.TryGetComp<CompProximityLetter>().letterSent = true;
					return;
				}
				this.OrderActivation(pawn, true);
			}
		}

		// Token: 0x0600AEEE RID: 44782 RVA: 0x003D9AFC File Offset: 0x003D7CFC
		private void OrderActivation(Pawn pawn, bool sendMessage)
		{
			Job job = JobMaker.MakeJob(JobDefOf.ActivateMonolith, this);
			pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			if (sendMessage)
			{
				Messages.Message(Find.Anomaly.LevelDef.pawnSentToActivateMessage.Formatted(pawn.Named("PAWN")), this, MessageTypeDefOf.NeutralEvent, true);
			}
		}

		// Token: 0x0600AEEF RID: 44783 RVA: 0x003D9B66 File Offset: 0x003D7D66
		public void AutoActivate(int tick)
		{
			this.autoActivateTick = tick;
			this.TryGetComp<CompProximityLetter>().letterSent = true;
			this.disturbingVisionTick = -99999;
		}

		// Token: 0x0600AEF0 RID: 44784 RVA: 0x003D9B88 File Offset: 0x003D7D88
		public void OnGUI(LocalTargetInfo target)
		{
			Widgets.MouseAttachedLabel("VoidMonolithChooseActivator".Translate(), 0f, 0f, null);
			if (this.ValidateTarget(target, false) && this.targetParams.CanTarget(target.Pawn, this))
			{
				GenUI.DrawMouseAttachment(this.UIIcon);
				return;
			}
			GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
		}

		// Token: 0x0600AEF1 RID: 44785 RVA: 0x003D9BF4 File Offset: 0x003D7DF4
		public override string GetInspectString()
		{
			string inspectString = base.GetInspectString();
			StringBuilder stringBuilder = new StringBuilder();
			if (!Find.Anomaly.LevelDef.levelInspectText.NullOrEmpty())
			{
				stringBuilder.Append(Find.Anomaly.LevelDef.levelInspectText);
			}
			if (Find.Anomaly.Level > 0)
			{
				if (Find.Anomaly.LevelDef.advanceThroughActivation)
				{
					string text;
					string text2;
					if (this.CanActivate(out text, out text2))
					{
						stringBuilder.AppendLineIfNotEmpty().Append(Find.Anomaly.LevelDef.monolithCanBeActivatedText);
					}
					else if (!text.NullOrEmpty())
					{
						stringBuilder.AppendLineIfNotEmpty().Append(text);
					}
				}
			}
			else
			{
				stringBuilder.AppendLineIfNotEmpty().Append("VoidMonolithUndiscovered".Translate());
			}
			if (!inspectString.NullOrEmpty())
			{
				stringBuilder.AppendLineIfNotEmpty().Append(inspectString);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600AEF2 RID: 44786 RVA: 0x003D9CCE File Offset: 0x003D7ECE
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_VoidMonolith.<GetGizmos>d__79 <GetGizmos>d__ = new Building_VoidMonolith.<GetGizmos>d__79(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600AEF3 RID: 44787 RVA: 0x003D9CDE File Offset: 0x003D7EDE
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			Building_VoidMonolith.<GetFloatMenuOptions>d__80 <GetFloatMenuOptions>d__ = new Building_VoidMonolith.<GetFloatMenuOptions>d__80(-2);
			<GetFloatMenuOptions>d__.<>4__this = this;
			<GetFloatMenuOptions>d__.<>3__selPawn = selPawn;
			return <GetFloatMenuOptions>d__;
		}

		// Token: 0x0400650C RID: 25868
		private static readonly FloatRange DisturbingVisionRangeDaysRange = new FloatRange(13f, 16f);

		// Token: 0x0400650D RID: 25869
		private const int DisturbingVisionRetryTicks = 15000;

		// Token: 0x0400650E RID: 25870
		public const int ActivateLetterDelayTicks = 360;

		// Token: 0x0400650F RID: 25871
		private const int AutoActivateAlertTicks = 300000;

		// Token: 0x04006510 RID: 25872
		private const int MonolithFragmentCount = 3;

		// Token: 0x04006511 RID: 25873
		private const int MonolithShardCount = 10;

		// Token: 0x04006512 RID: 25874
		private const int CollapseScatterRadius = 5;

		// Token: 0x04006513 RID: 25875
		private const int HintEffecterInterval = 30;

		// Token: 0x04006514 RID: 25876
		public Quest quest;

		// Token: 0x04006515 RID: 25877
		private int disturbingVisionTick = -99999;

		// Token: 0x04006516 RID: 25878
		private int autoActivateTick = -99999;

		// Token: 0x04006517 RID: 25879
		private List<Thing> monolithAttachments = new List<Thing>();

		// Token: 0x04006518 RID: 25880
		private Thing pyramidThing;

		// Token: 0x04006519 RID: 25881
		private int activatedDialogTick = -99999;

		// Token: 0x0400651A RID: 25882
		private Pawn activatorPawn;

		// Token: 0x0400651B RID: 25883
		private Effecter gleamingEffecter;

		// Token: 0x0400651C RID: 25884
		private Effecter gleamingVoidNodeEffecter;

		// Token: 0x0400651D RID: 25885
		private Effecter autoActivateEffecter;

		// Token: 0x0400651E RID: 25886
		private Effecter level0HintEffecter;

		// Token: 0x0400651F RID: 25887
		private static readonly TargetingParameters targetParmsInt = new TargetingParameters
		{
			canTargetBuildings = false,
			canTargetAnimals = false,
			canTargetMechs = false,
			onlyTargetColonists = true
		};
	}
}
