using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020018C1 RID: 6337
	[StaticConstructorOnStartup]
	public class CompHoldingPlatformTarget : ThingComp
	{
		// Token: 0x17001B3B RID: 6971
		// (get) Token: 0x06009ECD RID: 40653 RVA: 0x0038FB45 File Offset: 0x0038DD45
		public CompProperties_HoldingPlatformTarget Props
		{
			get
			{
				return (CompProperties_HoldingPlatformTarget)this.props;
			}
		}

		// Token: 0x17001B3C RID: 6972
		// (get) Token: 0x06009ECE RID: 40654 RVA: 0x0038FB54 File Offset: 0x0038DD54
		public CompStudiable CompStudiable
		{
			get
			{
				CompStudiable result;
				if ((result = this.compStudiable) == null)
				{
					result = (this.compStudiable = this.parent.GetComp<CompStudiable>());
				}
				return result;
			}
		}

		// Token: 0x17001B3D RID: 6973
		// (get) Token: 0x06009ECF RID: 40655 RVA: 0x0038FB80 File Offset: 0x0038DD80
		public CompActivity CompActivity
		{
			get
			{
				if (this.didCheckForActivityComp)
				{
					return this.compActivity;
				}
				if (this.compActivity == null)
				{
					Pawn pawn = this.parent as Pawn;
					this.compActivity = (((pawn != null) ? pawn.activity : null) ?? this.parent.GetComp<CompActivity>());
				}
				this.didCheckForActivityComp = true;
				return this.compActivity;
			}
		}

		// Token: 0x17001B3E RID: 6974
		// (get) Token: 0x06009ED0 RID: 40656 RVA: 0x0038FBDD File Offset: 0x0038DDDD
		public CompEntityHolder EntityHolder
		{
			get
			{
				return this.targetHolder.TryGetComp<CompEntityHolder>();
			}
		}

		// Token: 0x17001B3F RID: 6975
		// (get) Token: 0x06009ED1 RID: 40657 RVA: 0x0038FBEC File Offset: 0x0038DDEC
		public bool StudiedAtHoldingPlatform
		{
			get
			{
				if (!this.EverStudiable)
				{
					return false;
				}
				Pawn pawn = this.parent as Pawn;
				return pawn == null || ((!pawn.IsAnimal || pawn.RaceProps.IsAnomalyEntity) && ((pawn.IsMutant && pawn.mutant.Def.canBeCapturedToHoldingPlatform) || (!pawn.RaceProps.Humanlike && !pawn.Inhumanized() && !pawn.kindDef.studiableAsPrisoner)));
			}
		}

		// Token: 0x17001B40 RID: 6976
		// (get) Token: 0x06009ED2 RID: 40658 RVA: 0x0038FC6C File Offset: 0x0038DE6C
		public bool CanStudy
		{
			get
			{
				return this.containmentMode == EntityContainmentMode.Study && this.EverStudiable;
			}
		}

		// Token: 0x17001B41 RID: 6977
		// (get) Token: 0x06009ED3 RID: 40659 RVA: 0x0038FC7F File Offset: 0x0038DE7F
		private bool EverStudiable
		{
			get
			{
				return !this.parent.Destroyed && (!(this.parent is Pawn) || (this.CompStudiable != null && this.CompStudiable.AnomalyKnowledge > 0f));
			}
		}

		// Token: 0x17001B42 RID: 6978
		// (get) Token: 0x06009ED4 RID: 40660 RVA: 0x0038FCBB File Offset: 0x0038DEBB
		public Building_HoldingPlatform HeldPlatform
		{
			get
			{
				return this.parent.ParentHolder as Building_HoldingPlatform;
			}
		}

		// Token: 0x17001B43 RID: 6979
		// (get) Token: 0x06009ED5 RID: 40661 RVA: 0x0038FCCD File Offset: 0x0038DECD
		public bool CurrentlyHeldOnPlatform
		{
			get
			{
				return this.HeldPlatform != null && this.parent.SpawnedOrAnyParentSpawned;
			}
		}

		// Token: 0x17001B44 RID: 6980
		// (get) Token: 0x06009ED6 RID: 40662 RVA: 0x0038FCE4 File Offset: 0x0038DEE4
		public bool CanBeCaptured
		{
			get
			{
				Pawn pawn = this.parent as Pawn;
				if (pawn == null)
				{
					return true;
				}
				if (pawn.Faction == Faction.OfPlayer)
				{
					return false;
				}
				if (pawn.IsMutant && !pawn.mutant.Def.canBeCapturedToHoldingPlatform)
				{
					return false;
				}
				if (this.CompStudiable != null && Find.Anomaly.HighestLevelReached < this.CompStudiable.Props.minMonolithLevelForStudy && Find.Anomaly.GenerateMonolith)
				{
					return false;
				}
				if (!pawn.Downed && !(pawn.ParentHolder is Pawn_CarryTracker))
				{
					CompActivity compActivity = this.CompActivity;
					return compActivity != null && compActivity.IsDormant;
				}
				return true;
			}
		}

		// Token: 0x06009ED7 RID: 40663 RVA: 0x0038FD87 File Offset: 0x0038DF87
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (this.targetHolder != null && this.parent.Map != this.targetHolder.Map)
			{
				this.targetHolder = null;
			}
		}

		// Token: 0x06009ED8 RID: 40664 RVA: 0x0038FDB0 File Offset: 0x0038DFB0
		public override void CompTick()
		{
			base.CompTick();
			Pawn pawn = this.parent as Pawn;
			if (pawn != null)
			{
				if (!pawn.SpawnedOrAnyParentSpawned)
				{
					return;
				}
				if (this.CurrentlyHeldOnPlatform)
				{
					this.CaptivityTick(pawn);
				}
				else if (this.targetHolder != null && (this.targetHolder.Destroyed || this.EntityHolder.HeldPawn != null))
				{
					this.targetHolder = null;
				}
				if (this.isEscaping && pawn.mindState.enemyTarget == null)
				{
					this.isEscaping = false;
				}
			}
			Building_HoldingPlatform heldPlatform = this.HeldPlatform;
			if (heldPlatform != null && heldPlatform.HasAttachedBioferriteHarvester)
			{
				this.extractBioferrite = false;
			}
			if (this.CanBeCaptured)
			{
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.CapturingEntities, OpportunityType.Important);
			}
		}

		// Token: 0x06009ED9 RID: 40665 RVA: 0x0038FE60 File Offset: 0x0038E060
		private void CaptivityTick(Pawn pawn)
		{
			pawn.mindState.entityTicksInCaptivity++;
			Building_HoldingPlatform building_HoldingPlatform = this.targetHolder as Building_HoldingPlatform;
			if (building_HoldingPlatform != null && building_HoldingPlatform != this.HeldPlatform && building_HoldingPlatform.Occupied)
			{
				this.targetHolder = null;
			}
			if (this.parent.IsHashIntervalTick(2500))
			{
				float num = ContainmentUtility.InitiateEscapeMtbDays(pawn, null);
				if (num >= 0f && Rand.MTBEventOccurs(num, 60000f, 2500f))
				{
					this.Escape(true);
				}
			}
		}

		// Token: 0x06009EDA RID: 40666 RVA: 0x0038FEE4 File Offset: 0x0038E0E4
		public void Notify_HeldOnPlatform(ThingOwner newOwner)
		{
			this.targetHolder = null;
			Pawn pawn = null;
			Pawn pawn2 = this.parent as Pawn;
			if (pawn2 != null)
			{
				pawn2.mindState.lastAssignedInteractTime = Find.TickManager.TicksGame;
				PawnComponentsUtility.AddAndRemoveDynamicComponents(pawn2, false);
				pawn = pawn2;
			}
			if (newOwner != null)
			{
				if (this.Props.heldPawnKind != null)
				{
					PawnKindDef heldPawnKind = this.Props.heldPawnKind;
					Faction ofEntities = Faction.OfEntities;
					PawnGenerationContext context = PawnGenerationContext.NonPlayer;
					float? fixedBiologicalAge = new float?(0f);
					Pawn pawn3 = PawnGenerator.GeneratePawn(new PawnGenerationRequest(heldPawnKind, ofEntities, context, null, true, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, fixedBiologicalAge, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
					newOwner.TryAdd(pawn3, true);
					CompHoldingPlatformTarget compHoldingPlatformTarget = pawn3.TryGetComp<CompHoldingPlatformTarget>();
					if (compHoldingPlatformTarget != null)
					{
						compHoldingPlatformTarget.Notify_HeldOnPlatform(newOwner);
					}
					pawn = pawn3;
					if (this.Props.heldPawnKind == PawnKindDefOf.Revenant)
					{
						CompBiosignatureOwner compBiosignatureOwner = this.parent.TryGetComp<CompBiosignatureOwner>();
						if (compBiosignatureOwner != null)
						{
							pawn3.TryGetComp<CompRevenant>().biosignature = compBiosignatureOwner.biosignature;
						}
						CompStudiable compStudiable;
						if (pawn3.TryGetComp(out compStudiable))
						{
							compStudiable.lastStudiedTick = Find.TickManager.TicksGame;
						}
					}
					Find.HiddenItemsManager.SetDiscovered(pawn3.def);
					this.parent.Destroy(DestroyMode.Vanish);
				}
				this.containmentMode = EntityContainmentMode.Study;
			}
			if (pawn == null || this.HeldPlatform == null)
			{
				return;
			}
			Lord lord = pawn.GetLord();
			if (lord != null)
			{
				lord.Notify_PawnLost(pawn, PawnLostCondition.MadePrisoner, null);
			}
			CompActivity compActivity = pawn.TryGetComp<CompActivity>();
			if (compActivity != null)
			{
				compActivity.Notify_HeldOnPlatform();
			}
			Find.StudyManager.UpdateStudiableCache(this.HeldPlatform, this.HeldPlatform.Map);
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.CapturingEntities, KnowledgeAmount.Total);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.ContainingEntities, OpportunityType.Important);
		}

		// Token: 0x06009EDB RID: 40667 RVA: 0x003900DE File Offset: 0x0038E2DE
		public void Notify_ReleasedFromPlatform()
		{
			Find.StudyManager.UpdateStudiableCache(this.HeldPlatform, this.HeldPlatform.Map);
		}

		// Token: 0x06009EDC RID: 40668 RVA: 0x003900FC File Offset: 0x0038E2FC
		public void Escape(bool initiator)
		{
			List<Pawn> list = new List<Pawn>();
			List<Building> list2 = new List<Building>
			{
				this.HeldPlatform
			};
			this.HeldPlatform.EjectContents();
			Pawn pawn = this.parent as Pawn;
			if (pawn == null)
			{
				return;
			}
			pawn.health.overrideDeathOnDownedChance = 0f;
			list.Add(pawn);
			this.isEscaping = true;
			if (this.Props.lookForTargetOnEscape && !pawn.Downed)
			{
				Pawn enemyTarget = (Pawn)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable, (Thing x) => x is Pawn && x.def.race.intelligence >= Intelligence.ToolUser, 0f, 9999f, default(IntVec3), float.MaxValue, true, true, true, false);
				pawn.mindState.enemyTarget = enemyTarget;
			}
			CompRevenant compRevenant = pawn.TryGetComp<CompRevenant>();
			if (compRevenant != null)
			{
				compRevenant.revenantState = RevenantState.Escape;
			}
			HediffComp_Invisibility invisibilityComp = pawn.GetInvisibilityComp();
			if (invisibilityComp != null)
			{
				invisibilityComp.BecomeVisible(true);
			}
			if (!initiator)
			{
				return;
			}
			Room room = pawn.GetRoom(RegionType.Set_All);
			if (room == null)
			{
				return;
			}
			foreach (Thing thing in (from x in room.ContainedAndAdjacentThings
			where x is Building_HoldingPlatform
			select x).ToList<Thing>())
			{
				Pawn heldPawn = ((Building_HoldingPlatform)thing).HeldPawn;
				if (heldPawn != null && heldPawn != pawn)
				{
					CompHoldingPlatformTarget compHoldingPlatformTarget = heldPawn.TryGetComp<CompHoldingPlatformTarget>();
					if (compHoldingPlatformTarget != null && compHoldingPlatformTarget.CurrentlyHeldOnPlatform)
					{
						float num = ContainmentUtility.InitiateEscapeMtbDays(heldPawn, null);
						if (num > 0f && Rand.Chance(CompHoldingPlatformTarget.JoinEscapeChanceFromEscapeIntervalCurve.Evaluate(num)))
						{
							list.Add(heldPawn);
							list2.Add(compHoldingPlatformTarget.HeldPlatform);
							compHoldingPlatformTarget.Escape(false);
						}
					}
				}
			}
			Find.LetterStack.ReceiveLetter(LetterMaker.MakeLetter("LetterLabelEscapingFromHoldingPlatform".Translate(), "LetterEscapingFromHoldingPlatform".Translate((from p in list
			select p.LabelCap).ToLineList("  - ", false)), LetterDefOf.ThreatBig, list2, null, null, null), null, 0, true);
		}

		// Token: 0x06009EDD RID: 40669 RVA: 0x00390344 File Offset: 0x0038E544
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompHoldingPlatformTarget.<CompGetGizmosExtra>d__38 <CompGetGizmosExtra>d__ = new CompHoldingPlatformTarget.<CompGetGizmosExtra>d__38(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x06009EDE RID: 40670 RVA: 0x00390354 File Offset: 0x0038E554
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (this.CanBeCaptured && ContainmentUtility.ShowContainmentStats(this.parent))
			{
				float num = this.parent.GetStatValue(StatDefOf.MinimumContainmentStrength, true, -1);
				if (this.Props.heldPawnKind != null)
				{
					num = this.Props.heldPawnKind.race.GetStatValueAbstract(StatDefOf.MinimumContainmentStrength, null);
				}
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += "Capturable".Translate() + ". " + StatDefOf.MinimumContainmentStrength.LabelCap + ": " + num.ToString("F1");
			}
			return text;
		}

		// Token: 0x06009EDF RID: 40671 RVA: 0x00390420 File Offset: 0x0038E620
		public override void PostExposeData()
		{
			Scribe_References.Look<Thing>(ref this.targetHolder, "targetHolder", false);
			Scribe_Values.Look<bool>(ref this.isEscaping, "isEscaping", false, false);
			Scribe_Values.Look<EntityContainmentMode>(ref this.containmentMode, "containmentMode", EntityContainmentMode.MaintainOnly, false);
			Scribe_Values.Look<bool>(ref this.extractBioferrite, "extractBioferrite", false, false);
		}

		// Token: 0x04005DFE RID: 24062
		private const int CheckInitiateEscapeIntervalTicks = 2500;

		// Token: 0x04005DFF RID: 24063
		private static readonly SimpleCurve JoinEscapeChanceFromEscapeIntervalCurve = new SimpleCurve
		{
			{
				new CurvePoint(120f, 0.33f),
				true
			},
			{
				new CurvePoint(60f, 0.5f),
				true
			},
			{
				new CurvePoint(10f, 0.9f),
				true
			}
		};

		// Token: 0x04005E00 RID: 24064
		private static readonly CachedTexture CaptureIcon = new CachedTexture("UI/Commands/CaptureEntity");

		// Token: 0x04005E01 RID: 24065
		private static readonly CachedTexture TransferIcon = new CachedTexture("UI/Commands/TransferEntity");

		// Token: 0x04005E02 RID: 24066
		private static readonly Texture2D CancelTex = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		// Token: 0x04005E03 RID: 24067
		public Thing targetHolder;

		// Token: 0x04005E04 RID: 24068
		public bool isEscaping;

		// Token: 0x04005E05 RID: 24069
		public EntityContainmentMode containmentMode;

		// Token: 0x04005E06 RID: 24070
		public bool extractBioferrite;

		// Token: 0x04005E07 RID: 24071
		[Unsaved(false)]
		private CompStudiable compStudiable;

		// Token: 0x04005E08 RID: 24072
		[Unsaved(false)]
		private CompActivity compActivity;

		// Token: 0x04005E09 RID: 24073
		[Unsaved(false)]
		private bool didCheckForActivityComp;
	}
}
