using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001AB2 RID: 6834
	[StaticConstructorOnStartup]
	public class Building_GeneExtractor : Building_Enterable, IThingHolderWithDrawnPawn, IThingHolder
	{
		// Token: 0x17001EEF RID: 7919
		// (get) Token: 0x0600AD16 RID: 44310 RVA: 0x003D01F9 File Offset: 0x003CE3F9
		private Pawn ContainedPawn
		{
			get
			{
				return this.innerContainer.FirstOrDefault<Thing>() as Pawn;
			}
		}

		// Token: 0x17001EF0 RID: 7920
		// (get) Token: 0x0600AD17 RID: 44311 RVA: 0x003D020B File Offset: 0x003CE40B
		public bool PowerOn
		{
			get
			{
				return this.PowerTraderComp.PowerOn;
			}
		}

		// Token: 0x17001EF1 RID: 7921
		// (get) Token: 0x0600AD18 RID: 44312 RVA: 0x003D0218 File Offset: 0x003CE418
		public override bool IsContentsSuspended
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001EF2 RID: 7922
		// (get) Token: 0x0600AD19 RID: 44313 RVA: 0x003D021B File Offset: 0x003CE41B
		private CompPowerTrader PowerTraderComp
		{
			get
			{
				if (this.cachedPowerComp == null)
				{
					this.cachedPowerComp = this.TryGetComp<CompPowerTrader>();
				}
				return this.cachedPowerComp;
			}
		}

		// Token: 0x17001EF3 RID: 7923
		// (get) Token: 0x0600AD1A RID: 44314 RVA: 0x003D0237 File Offset: 0x003CE437
		public Texture2D InsertPawnTex
		{
			get
			{
				if (this.cachedInsertPawnTex == null)
				{
					this.cachedInsertPawnTex = ContentFinder<Texture2D>.Get("UI/Gizmos/InsertPawn", true);
				}
				return this.cachedInsertPawnTex;
			}
		}

		// Token: 0x17001EF4 RID: 7924
		// (get) Token: 0x0600AD1B RID: 44315 RVA: 0x003D025E File Offset: 0x003CE45E
		public float HeldPawnDrawPos_Y
		{
			get
			{
				return this.DrawPos.y + 0.03658537f;
			}
		}

		// Token: 0x17001EF5 RID: 7925
		// (get) Token: 0x0600AD1C RID: 44316 RVA: 0x003D0274 File Offset: 0x003CE474
		public float HeldPawnBodyAngle
		{
			get
			{
				return base.Rotation.Opposite.AsAngle;
			}
		}

		// Token: 0x17001EF6 RID: 7926
		// (get) Token: 0x0600AD1D RID: 44317 RVA: 0x003D0297 File Offset: 0x003CE497
		public PawnPosture HeldPawnPosture
		{
			get
			{
				return PawnPosture.LayingOnGroundFaceUp;
			}
		}

		// Token: 0x17001EF7 RID: 7927
		// (get) Token: 0x0600AD1E RID: 44318 RVA: 0x003D029C File Offset: 0x003CE49C
		public override Vector3 PawnDrawOffset
		{
			get
			{
				return IntVec3.West.RotatedBy(base.Rotation).ToVector3() / (float)this.def.size.x;
			}
		}

		// Token: 0x0600AD1F RID: 44319 RVA: 0x003D02D7 File Offset: 0x003CE4D7
		public override void PostPostMake()
		{
			if (!ModLister.CheckBiotech("gene extractor"))
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostPostMake();
		}

		// Token: 0x0600AD20 RID: 44320 RVA: 0x003D02F3 File Offset: 0x003CE4F3
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			this.sustainerWorking = null;
			if (this.progressBar != null)
			{
				this.progressBar.Cleanup();
				this.progressBar = null;
			}
			base.DeSpawn(mode);
		}

		// Token: 0x0600AD21 RID: 44321 RVA: 0x003D0320 File Offset: 0x003CE520
		protected override void Tick()
		{
			base.Tick();
			if (this.IsHashIntervalTick(250))
			{
				this.PowerTraderComp.PowerOutput = (base.Working ? (-base.PowerComp.Props.PowerConsumption) : (-base.PowerComp.Props.idlePowerDraw));
			}
			if (base.Working)
			{
				if (this.ContainedPawn == null)
				{
					this.Cancel();
					return;
				}
				if (this.PowerTraderComp.PowerOn)
				{
					this.TickEffects();
					if (this.PowerOn)
					{
						this.ticksRemaining--;
					}
					if (this.ticksRemaining <= 0)
					{
						this.Finish();
						return;
					}
				}
				else
				{
					this.powerCutTicks++;
					if (this.powerCutTicks >= 60000)
					{
						Pawn containedPawn = this.ContainedPawn;
						if (containedPawn != null)
						{
							Messages.Message("GeneExtractorNoPowerEjectedMessage".Translate(containedPawn.Named("PAWN")), containedPawn, MessageTypeDefOf.NegativeEvent, false);
						}
						this.Cancel();
						return;
					}
				}
			}
			else
			{
				if (this.selectedPawn != null && this.selectedPawn.Dead)
				{
					this.Cancel();
				}
				if (this.progressBar != null)
				{
					this.progressBar.Cleanup();
					this.progressBar = null;
				}
			}
		}

		// Token: 0x0600AD22 RID: 44322 RVA: 0x003D045C File Offset: 0x003CE65C
		private void TickEffects()
		{
			if (this.sustainerWorking == null || this.sustainerWorking.Ended)
			{
				this.sustainerWorking = SoundDefOf.GeneExtractor_Working.TrySpawnSustainer(SoundInfo.InMap(this, MaintenanceType.PerTick));
			}
			else
			{
				this.sustainerWorking.Maintain();
			}
			if (this.progressBar == null)
			{
				this.progressBar = EffecterDefOf.ProgressBarAlwaysVisible.Spawn();
			}
			this.progressBar.EffectTick(new TargetInfo(base.Position + IntVec3.North.RotatedBy(base.Rotation), base.Map, false), TargetInfo.Invalid);
			MoteProgressBar mote = ((SubEffecter_ProgressBar)this.progressBar.children[0]).mote;
			if (mote != null)
			{
				mote.progress = 1f - Mathf.Clamp01((float)this.ticksRemaining / 30000f);
				mote.offsetZ = ((base.Rotation == Rot4.North) ? 0.5f : -0.5f);
			}
		}

		// Token: 0x0600AD23 RID: 44323 RVA: 0x003D0558 File Offset: 0x003CE758
		public override AcceptanceReport CanAcceptPawn(Pawn pawn)
		{
			if (!pawn.IsColonist && !pawn.IsSlaveOfColony && !pawn.IsPrisonerOfColony && (!pawn.IsColonySubhuman || !pawn.IsGhoul))
			{
				return false;
			}
			if (this.selectedPawn != null && this.selectedPawn != pawn)
			{
				return false;
			}
			if (!pawn.RaceProps.Humanlike || pawn.IsQuestLodger())
			{
				return false;
			}
			if (!this.PowerOn)
			{
				return "NoPower".Translate().CapitalizeFirst();
			}
			if (this.innerContainer.Count > 0)
			{
				return "Occupied".Translate();
			}
			if (pawn.genes != null)
			{
				if (pawn.genes.GenesListForReading.Any((Gene x) => x.def.passOnDirectly))
				{
					if (!pawn.genes.GenesListForReading.Any((Gene x) => x.def.biostatArc == 0))
					{
						return "PawnHasNoNonArchiteGenes".Translate(pawn.Named("PAWN"));
					}
					if (pawn.health.hediffSet.HasHediff(HediffDefOf.XenogerminationComa, false))
					{
						return "InXenogerminationComa".Translate();
					}
					return true;
				}
			}
			return "PawnHasNoGenes".Translate(pawn.Named("PAWN"));
		}

		// Token: 0x0600AD24 RID: 44324 RVA: 0x003D06D4 File Offset: 0x003CE8D4
		private void Cancel()
		{
			this.startTick = -1;
			this.selectedPawn = null;
			this.sustainerWorking = null;
			this.powerCutTicks = 0;
			this.innerContainer.TryDropAll(this.def.hasInteractionCell ? this.InteractionCell : base.Position, base.Map, ThingPlaceMode.Near, null, null, true);
		}

		// Token: 0x0600AD25 RID: 44325 RVA: 0x003D0730 File Offset: 0x003CE930
		private void Finish()
		{
			this.selectedPawn = null;
			this.sustainerWorking = null;
			this.powerCutTicks = 0;
			if (this.ContainedPawn != null)
			{
				Pawn containedPawn = this.ContainedPawn;
				Rand.PushState(containedPawn.thingIDNumber ^ this.startTick);
				Building_GeneExtractor.<>c__DisplayClass34_0 CS$<>8__locals1 = new Building_GeneExtractor.<>c__DisplayClass34_0();
				CS$<>8__locals1.genesToAdd = new List<GeneDef>();
				Genepack genepack = (Genepack)ThingMaker.MakeThing(ThingDefOf.Genepack, null);
				int num = Mathf.Min((int)Building_GeneExtractor.GeneCountChanceCurve.RandomElementByWeight((CurvePoint p) => p.y).x, containedPawn.genes.GenesListForReading.Count((Gene x) => x.def.biostatArc == 0));
				int num2 = 0;
				Gene gene;
				while (num2 < num && containedPawn.genes.GenesListForReading.TryRandomElementByWeight(new Func<Gene, float>(CS$<>8__locals1.<Finish>g__SelectionWeight|0), out gene))
				{
					CS$<>8__locals1.genesToAdd.Add(gene.def);
					num2++;
				}
				if (CS$<>8__locals1.genesToAdd.Any<GeneDef>())
				{
					genepack.Initialize(CS$<>8__locals1.genesToAdd);
				}
				GeneUtility.ExtractXenogerm(containedPawn, Mathf.RoundToInt(60000f * GeneTuning.GeneExtractorRegrowingDurationDaysRange.RandomInRange));
				IntVec3 intVec = this.def.hasInteractionCell ? this.InteractionCell : base.Position;
				this.innerContainer.TryDropAll(intVec, base.Map, ThingPlaceMode.Near, null, null, true);
				if (!containedPawn.Dead && (containedPawn.IsPrisonerOfColony || containedPawn.IsSlaveOfColony))
				{
					Pawn_NeedsTracker needs = containedPawn.needs;
					if (needs != null)
					{
						Need_Mood mood = needs.mood;
						if (mood != null)
						{
							ThoughtHandler thoughts = mood.thoughts;
							if (thoughts != null)
							{
								MemoryThoughtHandler memories = thoughts.memories;
								if (memories != null)
								{
									memories.TryGainMemory(ThoughtDefOf.XenogermHarvested_Prisoner, null, null);
								}
							}
						}
					}
				}
				if (CS$<>8__locals1.genesToAdd.Any<GeneDef>())
				{
					GenPlace.TryPlaceThing(genepack, intVec, base.Map, ThingPlaceMode.Near, null, null, null, 1);
				}
				Messages.Message("GeneExtractionComplete".Translate(containedPawn.Named("PAWN")) + ": " + (from x in CS$<>8__locals1.genesToAdd
				select x.label).ToCommaList(false, false).CapitalizeFirst(), new LookTargets(new TargetInfo[]
				{
					containedPawn,
					genepack
				}), MessageTypeDefOf.PositiveEvent, true);
				Rand.PopState();
			}
			this.startTick = -1;
		}

		// Token: 0x0600AD26 RID: 44326 RVA: 0x003D09C8 File Offset: 0x003CEBC8
		public override void TryAcceptPawn(Pawn pawn)
		{
			if (this.CanAcceptPawn(pawn))
			{
				this.selectedPawn = pawn;
				bool flag = pawn.DeSpawnOrDeselect(DestroyMode.Vanish);
				if (this.innerContainer.TryAddOrTransfer(pawn, true))
				{
					this.startTick = Find.TickManager.TicksGame;
					this.ticksRemaining = 30000;
				}
				if (flag)
				{
					Find.Selector.Select(pawn, false, false);
				}
			}
		}

		// Token: 0x0600AD27 RID: 44327 RVA: 0x003D0A2C File Offset: 0x003CEC2C
		protected override void SelectPawn(Pawn pawn)
		{
			if (pawn.health.hediffSet.HasHediff(HediffDefOf.XenogermReplicating, false))
			{
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmExtractXenogermWillKill".Translate(pawn.Named("PAWN")), delegate()
				{
					this.<>n__0(pawn);
				}, false, null, WindowLayer.Dialog));
				return;
			}
			base.SelectPawn(pawn);
		}

		// Token: 0x0600AD28 RID: 44328 RVA: 0x003D0AAF File Offset: 0x003CECAF
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			Building_GeneExtractor.<GetFloatMenuOptions>d__37 <GetFloatMenuOptions>d__ = new Building_GeneExtractor.<GetFloatMenuOptions>d__37(-2);
			<GetFloatMenuOptions>d__.<>4__this = this;
			<GetFloatMenuOptions>d__.<>3__selPawn = selPawn;
			return <GetFloatMenuOptions>d__;
		}

		// Token: 0x0600AD29 RID: 44329 RVA: 0x003D0AC6 File Offset: 0x003CECC6
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_GeneExtractor.<GetGizmos>d__38 <GetGizmos>d__ = new Building_GeneExtractor.<GetGizmos>d__38(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600AD2A RID: 44330 RVA: 0x003D0AD8 File Offset: 0x003CECD8
		public override void DynamicDrawPhaseAt(DrawPhase phase, Vector3 drawLoc, bool flip = false)
		{
			base.DynamicDrawPhaseAt(phase, drawLoc, flip);
			if (base.Working && this.ContainedPawn != null)
			{
				this.ContainedPawn.Drawer.renderer.DynamicDrawPhaseAt(phase, drawLoc + this.PawnDrawOffset, null, true);
			}
		}

		// Token: 0x0600AD2B RID: 44331 RVA: 0x003D0B2C File Offset: 0x003CED2C
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (this.selectedPawn != null && this.innerContainer.Count == 0)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += "WaitingForPawn".Translate(this.selectedPawn.Named("PAWN")).Resolve();
			}
			else if (base.Working && this.ContainedPawn != null)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text = text + "ExtractingXenogermFrom".Translate(this.ContainedPawn.Named("PAWN")).Resolve() + "\n";
				if (this.PowerOn)
				{
					text += "DurationLeft".Translate(this.ticksRemaining.ToStringTicksToPeriod(true, false, true, true, false)).Resolve();
				}
				else
				{
					text += "ExtractionPausedNoPower".Translate((60000 - this.powerCutTicks).ToStringTicksToPeriod(true, false, true, true, false).Named("TIME")).Colorize(ColorLibrary.RedReadable);
				}
			}
			return text;
		}

		// Token: 0x0600AD2C RID: 44332 RVA: 0x003D0C63 File Offset: 0x003CEE63
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksRemaining, "ticksRemaining", 0, false);
			Scribe_Values.Look<int>(ref this.powerCutTicks, "powerCutTicks", 0, false);
		}

		// Token: 0x04006469 RID: 25705
		private int ticksRemaining;

		// Token: 0x0400646A RID: 25706
		private int powerCutTicks;

		// Token: 0x0400646B RID: 25707
		[Unsaved(false)]
		private CompPowerTrader cachedPowerComp;

		// Token: 0x0400646C RID: 25708
		[Unsaved(false)]
		private Texture2D cachedInsertPawnTex;

		// Token: 0x0400646D RID: 25709
		[Unsaved(false)]
		private Sustainer sustainerWorking;

		// Token: 0x0400646E RID: 25710
		[Unsaved(false)]
		private Effecter progressBar;

		// Token: 0x0400646F RID: 25711
		private const int TicksToExtract = 30000;

		// Token: 0x04006470 RID: 25712
		private const int NoPowerEjectCumulativeTicks = 60000;

		// Token: 0x04006471 RID: 25713
		private static readonly Texture2D CancelIcon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		// Token: 0x04006472 RID: 25714
		private static readonly SimpleCurve GeneCountChanceCurve = new SimpleCurve
		{
			{
				new CurvePoint(1f, 0.7f),
				true
			},
			{
				new CurvePoint(2f, 0.2f),
				true
			},
			{
				new CurvePoint(3f, 0.08f),
				true
			},
			{
				new CurvePoint(4f, 0.02f),
				true
			}
		};
	}
}
