using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001ACC RID: 6860
	[StaticConstructorOnStartup]
	public class Building_SubcoreScanner : Building_Enterable, IThingHolderWithDrawnPawn, IThingHolder
	{
		// Token: 0x17001F52 RID: 8018
		// (get) Token: 0x0600AE87 RID: 44679 RVA: 0x003D74B3 File Offset: 0x003D56B3
		public float HeldPawnDrawPos_Y
		{
			get
			{
				return this.DrawPos.y + 0.03658537f;
			}
		}

		// Token: 0x17001F53 RID: 8019
		// (get) Token: 0x0600AE88 RID: 44680 RVA: 0x003D74C8 File Offset: 0x003D56C8
		public float HeldPawnBodyAngle
		{
			get
			{
				return base.Rotation.AsAngle;
			}
		}

		// Token: 0x17001F54 RID: 8020
		// (get) Token: 0x0600AE89 RID: 44681 RVA: 0x003D74E3 File Offset: 0x003D56E3
		public PawnPosture HeldPawnPosture
		{
			get
			{
				return PawnPosture.LayingOnGroundFaceUp;
			}
		}

		// Token: 0x17001F55 RID: 8021
		// (get) Token: 0x0600AE8A RID: 44682 RVA: 0x003D74E6 File Offset: 0x003D56E6
		public bool PowerOn
		{
			get
			{
				return this.TryGetComp<CompPowerTrader>().PowerOn;
			}
		}

		// Token: 0x17001F56 RID: 8022
		// (get) Token: 0x0600AE8B RID: 44683 RVA: 0x003D74F3 File Offset: 0x003D56F3
		public override Vector3 PawnDrawOffset
		{
			get
			{
				return Vector3.zero;
			}
		}

		// Token: 0x17001F57 RID: 8023
		// (get) Token: 0x0600AE8C RID: 44684 RVA: 0x003D74FC File Offset: 0x003D56FC
		public Pawn Occupant
		{
			get
			{
				for (int i = 0; i < this.innerContainer.Count; i++)
				{
					Pawn pawn = this.innerContainer[i] as Pawn;
					if (pawn != null)
					{
						return pawn;
					}
				}
				return null;
			}
		}

		// Token: 0x17001F58 RID: 8024
		// (get) Token: 0x0600AE8D RID: 44685 RVA: 0x003D7538 File Offset: 0x003D5738
		public bool AllRequiredIngredientsLoaded
		{
			get
			{
				if (!this.debugDisableNeedForIngredients)
				{
					for (int i = 0; i < this.def.building.subcoreScannerFixedIngredients.Count; i++)
					{
						if (this.GetRequiredCountOf(this.def.building.subcoreScannerFixedIngredients[i].FixedIngredient) > 0)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x17001F59 RID: 8025
		// (get) Token: 0x0600AE8E RID: 44686 RVA: 0x003D7594 File Offset: 0x003D5794
		public SubcoreScannerState State
		{
			get
			{
				if (!this.initScanner || !this.PowerOn)
				{
					return SubcoreScannerState.Inactive;
				}
				if (!this.AllRequiredIngredientsLoaded)
				{
					return SubcoreScannerState.WaitingForIngredients;
				}
				if (this.Occupant == null)
				{
					return SubcoreScannerState.WaitingForOccupant;
				}
				return SubcoreScannerState.Occupied;
			}
		}

		// Token: 0x17001F5A RID: 8026
		// (get) Token: 0x0600AE8F RID: 44687 RVA: 0x003D75BD File Offset: 0x003D57BD
		public bool DestroyOccupantBrain
		{
			get
			{
				return this.def.building.destroyBrain;
			}
		}

		// Token: 0x0600AE90 RID: 44688 RVA: 0x003D75D0 File Offset: 0x003D57D0
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			for (int i = 0; i < this.def.building.subcoreScannerFixedIngredients.Count; i++)
			{
				this.def.building.subcoreScannerFixedIngredients[i].ResolveReferences();
			}
		}

		// Token: 0x0600AE91 RID: 44689 RVA: 0x003D7620 File Offset: 0x003D5820
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Effecter effecter = this.progressBarEffecter;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.progressBarEffecter = null;
			Effecter effecter2 = this.effectHusk;
			if (effecter2 != null)
			{
				effecter2.Cleanup();
			}
			this.effectHusk = null;
			Effecter effecter3 = this.effectStart;
			if (effecter3 != null)
			{
				effecter3.Cleanup();
			}
			this.effectStart = null;
			if (!base.BeingTransportedOnGravship && this.DestroyOccupantBrain && this.Occupant != null)
			{
				this.KillOccupant();
			}
			base.DeSpawn(mode);
		}

		// Token: 0x0600AE92 RID: 44690 RVA: 0x003D769C File Offset: 0x003D589C
		public int GetRequiredCountOf(ThingDef thingDef)
		{
			for (int i = 0; i < this.def.building.subcoreScannerFixedIngredients.Count; i++)
			{
				if (this.def.building.subcoreScannerFixedIngredients[i].FixedIngredient == thingDef)
				{
					int num = this.innerContainer.TotalStackCountOfDef(this.def.building.subcoreScannerFixedIngredients[i].FixedIngredient);
					return (int)this.def.building.subcoreScannerFixedIngredients[i].GetBaseCount() - num;
				}
			}
			return 0;
		}

		// Token: 0x0600AE93 RID: 44691 RVA: 0x003D7730 File Offset: 0x003D5930
		public override AcceptanceReport CanAcceptPawn(Pawn selPawn)
		{
			if (!selPawn.IsColonist && !selPawn.IsSlaveOfColony && !selPawn.IsPrisonerOfColony)
			{
				return false;
			}
			if (this.selectedPawn != null && this.selectedPawn != selPawn)
			{
				return false;
			}
			if (!this.PowerOn)
			{
				return "CannotUseNoPower".Translate();
			}
			if (this.State != SubcoreScannerState.WaitingForOccupant)
			{
				switch (this.State)
				{
				case SubcoreScannerState.Inactive:
					return "SubcoreScannerNotInit".Translate();
				case SubcoreScannerState.WaitingForIngredients:
				{
					StringBuilder stringBuilder = new StringBuilder("SubcoreScannerRequiresIngredients".Translate() + ": ");
					bool flag = false;
					for (int i = 0; i < this.def.building.subcoreScannerFixedIngredients.Count; i++)
					{
						IngredientCount ingredientCount = this.def.building.subcoreScannerFixedIngredients[i];
						int num = this.innerContainer.TotalStackCountOfDef(ingredientCount.FixedIngredient);
						int num2 = (int)ingredientCount.GetBaseCount();
						if (num < num2)
						{
							if (flag)
							{
								stringBuilder.Append(", ");
							}
							StringBuilder stringBuilder2 = stringBuilder;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
							defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(ingredientCount.FixedIngredient.LabelCap);
							defaultInterpolatedStringHandler.AppendLiteral(" x");
							defaultInterpolatedStringHandler.AppendFormatted<int>(num2 - num);
							stringBuilder2.Append(defaultInterpolatedStringHandler.ToStringAndClear());
							flag = true;
						}
					}
					return stringBuilder.ToString();
				}
				case SubcoreScannerState.Occupied:
					return "SubcoreScannerOccupied".Translate();
				}
			}
			else
			{
				if (selPawn.IsQuestLodger())
				{
					return "CryptosleepCasketGuestsNotAllowed".Translate();
				}
				if (selPawn.health.hediffSet.HasHediff(HediffDefOf.ScanningSickness, false))
				{
					return "SubcoreScannerPawnHasSickness".Translate(HediffDefOf.ScanningSickness.label);
				}
				if (selPawn.DevelopmentalStage.Baby())
				{
					return "SubcoreScannerBabyNotAllowed".Translate();
				}
			}
			return true;
		}

		// Token: 0x0600AE94 RID: 44692 RVA: 0x003D7930 File Offset: 0x003D5B30
		public override void TryAcceptPawn(Pawn pawn)
		{
			if (!this.CanAcceptPawn(pawn))
			{
				return;
			}
			bool flag = pawn.DeSpawnOrDeselect(DestroyMode.Vanish);
			if (pawn.holdingOwner != null)
			{
				pawn.holdingOwner.TryTransferToContainer(pawn, this.innerContainer, true);
			}
			else
			{
				this.innerContainer.TryAdd(pawn, true);
			}
			if (flag)
			{
				Find.Selector.Select(pawn, false, false);
			}
			this.fabricationTicksLeft = this.def.building.subcoreScannerTicks;
		}

		// Token: 0x0600AE95 RID: 44693 RVA: 0x003D79A4 File Offset: 0x003D5BA4
		public bool CanAcceptIngredient(Thing thing)
		{
			return this.GetRequiredCountOf(thing.def) > 0;
		}

		// Token: 0x0600AE96 RID: 44694 RVA: 0x003D79B8 File Offset: 0x003D5BB8
		public void EjectContents()
		{
			Pawn occupant = this.Occupant;
			if (occupant == null)
			{
				this.innerContainer.TryDropAll(this.InteractionCell, base.Map, ThingPlaceMode.Near, null, null, true);
			}
			else
			{
				if (this.def.building.subcoreScannerHediff != null)
				{
					Pawn_HealthTracker health = occupant.health;
					if (health != null)
					{
						health.AddHediff(this.def.building.subcoreScannerHediff, null, null, null);
					}
				}
				if (this.DestroyOccupantBrain)
				{
					this.KillOccupant();
				}
				for (int i = this.innerContainer.Count - 1; i >= 0; i--)
				{
					if (this.innerContainer[i] is Pawn || this.innerContainer[i] is Corpse)
					{
						Thing thing;
						this.innerContainer.TryDrop(this.innerContainer[i], this.InteractionCell, base.Map, ThingPlaceMode.Near, 1, out thing, null, null);
					}
				}
				this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			}
			this.selectedPawn = null;
			this.initScanner = false;
		}

		// Token: 0x0600AE97 RID: 44695 RVA: 0x003D7AC0 File Offset: 0x003D5CC0
		private void KillOccupant()
		{
			Pawn occupant = this.Occupant;
			DamageInfo dinfo = new DamageInfo(DamageDefOf.ExecutionCut, 9999f, 999f, -1f, null, occupant.health.hediffSet.GetBrain(), null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false);
			dinfo.SetIgnoreInstantKillProtection(true);
			dinfo.SetAllowDamagePropagation(false);
			occupant.forceNoDeathNotification = true;
			occupant.TakeDamage(dinfo);
			occupant.forceNoDeathNotification = false;
			ThoughtUtility.GiveThoughtsForPawnExecuted(occupant, null, PawnExecutionKind.Ripscanned);
			Messages.Message("MessagePawnKilledRipscanner".Translate(occupant.Named("PAWN")), occupant, MessageTypeDefOf.NegativeHealthEvent, true);
		}

		// Token: 0x0600AE98 RID: 44696 RVA: 0x003D7B61 File Offset: 0x003D5D61
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			Building_SubcoreScanner.<GetFloatMenuOptions>d__41 <GetFloatMenuOptions>d__ = new Building_SubcoreScanner.<GetFloatMenuOptions>d__41(-2);
			<GetFloatMenuOptions>d__.<>4__this = this;
			<GetFloatMenuOptions>d__.<>3__selPawn = selPawn;
			return <GetFloatMenuOptions>d__;
		}

		// Token: 0x0600AE99 RID: 44697 RVA: 0x003D7B78 File Offset: 0x003D5D78
		public static bool WasLoadingCancelled(Thing thing)
		{
			Building_SubcoreScanner building_SubcoreScanner = thing as Building_SubcoreScanner;
			return building_SubcoreScanner != null && !building_SubcoreScanner.initScanner;
		}

		// Token: 0x0600AE9A RID: 44698 RVA: 0x003D7B9C File Offset: 0x003D5D9C
		public override void DynamicDrawPhaseAt(DrawPhase phase, Vector3 drawLoc, bool flip = false)
		{
			base.DynamicDrawPhaseAt(phase, drawLoc, flip);
			Pawn occupant = this.Occupant;
			if (occupant != null)
			{
				occupant.Drawer.renderer.DynamicDrawPhaseAt(phase, drawLoc, null, true);
			}
		}

		// Token: 0x0600AE9B RID: 44699 RVA: 0x003D7BD8 File Offset: 0x003D5DD8
		protected override void Tick()
		{
			base.Tick();
			if (Building_SubcoreScanner.MotePerRotation == null)
			{
				Building_SubcoreScanner.MotePerRotation = new Dictionary<Rot4, ThingDef>
				{
					{
						Rot4.South,
						ThingDefOf.SoftScannerGlow_South
					},
					{
						Rot4.East,
						ThingDefOf.SoftScannerGlow_East
					},
					{
						Rot4.West,
						ThingDefOf.SoftScannerGlow_West
					},
					{
						Rot4.North,
						ThingDefOf.SoftScannerGlow_North
					}
				};
				Building_SubcoreScanner.MotePerRotationRip = new Dictionary<Rot4, ThingDef>
				{
					{
						Rot4.South,
						ThingDefOf.RipScannerGlow_South
					},
					{
						Rot4.East,
						ThingDefOf.RipScannerGlow_East
					},
					{
						Rot4.West,
						ThingDefOf.RipScannerGlow_West
					},
					{
						Rot4.North,
						ThingDefOf.RipScannerGlow_North
					}
				};
			}
			SubcoreScannerState state = this.State;
			if (state == SubcoreScannerState.Occupied)
			{
				this.fabricationTicksLeft--;
				if (this.fabricationTicksLeft <= 0)
				{
					if (!this.DestroyOccupantBrain)
					{
						Messages.Message("MessageSubcoreSoftscannerCompleted".Translate(this.Occupant.Named("PAWN")), this.Occupant, MessageTypeDefOf.PositiveEvent, true);
					}
					this.EjectContents();
					GenPlace.TryPlaceThing(ThingMaker.MakeThing(this.def.building.subcoreScannerOutputDef, null), this.InteractionCell, base.Map, ThingPlaceMode.Near, null, null, null, 1);
					if (this.def.building.subcoreScannerComplete != null)
					{
						this.def.building.subcoreScannerComplete.PlayOneShot(this);
					}
				}
				if (this.workingMote == null || this.workingMote.Destroyed)
				{
					this.workingMote = MoteMaker.MakeAttachedOverlay(this, this.DestroyOccupantBrain ? Building_SubcoreScanner.MotePerRotationRip[base.Rotation] : Building_SubcoreScanner.MotePerRotation[base.Rotation], Vector3.zero, 1f, -1f);
				}
				Mote mote = this.workingMote;
				if (mote != null)
				{
					mote.Maintain();
				}
				if (this.DestroyOccupantBrain)
				{
					if (this.effectHusk == null)
					{
						this.effectHusk = EffecterDefOf.RipScannerHeadGlow.Spawn(this, base.MapHeld, Building_SubcoreScanner.HuskEffectOffsets[base.Rotation]);
					}
					this.effectHusk.EffectTick(this, this);
				}
				if (this.progressBarEffecter == null)
				{
					this.progressBarEffecter = EffecterDefOf.ProgressBar.Spawn();
				}
				this.progressBarEffecter.EffectTick(this, TargetInfo.Invalid);
				MoteProgressBar mote2 = ((SubEffecter_ProgressBar)this.progressBarEffecter.children[0]).mote;
				mote2.progress = 1f - (float)this.fabricationTicksLeft / (float)this.def.building.subcoreScannerTicks;
				mote2.offsetZ = -0.8f;
				if (this.def.building.subcoreScannerWorking != null)
				{
					if (this.sustainerWorking == null || this.sustainerWorking.Ended)
					{
						this.sustainerWorking = this.def.building.subcoreScannerWorking.TrySpawnSustainer(SoundInfo.InMap(this, MaintenanceType.PerTick));
					}
					else
					{
						this.sustainerWorking.Maintain();
					}
				}
			}
			else
			{
				Effecter effecter = this.effectHusk;
				if (effecter != null)
				{
					effecter.Cleanup();
				}
				this.effectHusk = null;
				Effecter effecter2 = this.progressBarEffecter;
				if (effecter2 != null)
				{
					effecter2.Cleanup();
				}
				this.progressBarEffecter = null;
			}
			if (state == SubcoreScannerState.Occupied)
			{
				if (this.def.building.subcoreScannerStartEffect != null)
				{
					if (this.effectStart == null)
					{
						this.effectStart = this.def.building.subcoreScannerStartEffect.Spawn();
						this.effectStart.Trigger(this, new TargetInfo(this.InteractionCell, base.Map, false), -1);
					}
					this.effectStart.EffectTick(this, new TargetInfo(this.InteractionCell, base.Map, false));
					return;
				}
			}
			else
			{
				Effecter effecter3 = this.effectStart;
				if (effecter3 != null)
				{
					effecter3.Cleanup();
				}
				this.effectStart = null;
			}
		}

		// Token: 0x0600AE9C RID: 44700 RVA: 0x003D7FB2 File Offset: 0x003D61B2
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_SubcoreScanner.<GetGizmos>d__45 <GetGizmos>d__ = new Building_SubcoreScanner.<GetGizmos>d__45(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600AE9D RID: 44701 RVA: 0x003D7FC4 File Offset: 0x003D61C4
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			switch (this.State)
			{
			case SubcoreScannerState.WaitingForIngredients:
				stringBuilder.AppendLineIfNotEmpty();
				stringBuilder.Append("SubcoreScannerWaitingForIngredients".Translate());
				this.AppendIngredientsList(stringBuilder);
				break;
			case SubcoreScannerState.WaitingForOccupant:
				stringBuilder.AppendLineIfNotEmpty();
				stringBuilder.Append("SubcoreScannerWaitingForOccupant".Translate());
				break;
			case SubcoreScannerState.Occupied:
				stringBuilder.AppendLineIfNotEmpty();
				stringBuilder.Append("SubcoreScannerCompletesIn".Translate() + ": " + this.fabricationTicksLeft.ToStringTicksToPeriod(true, false, true, true, false));
				break;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600AE9E RID: 44702 RVA: 0x003D808C File Offset: 0x003D628C
		private void AppendIngredientsList(StringBuilder sb)
		{
			for (int i = 0; i < this.def.building.subcoreScannerFixedIngredients.Count; i++)
			{
				IngredientCount ingredientCount = this.def.building.subcoreScannerFixedIngredients[i];
				int value = this.innerContainer.TotalStackCountOfDef(ingredientCount.FixedIngredient);
				int value2 = (int)ingredientCount.GetBaseCount();
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(7, 3);
				defaultInterpolatedStringHandler.AppendLiteral(" - ");
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(ingredientCount.FixedIngredient.LabelCap);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(value);
				defaultInterpolatedStringHandler.AppendLiteral(" / ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(value2);
				sb.AppendInNewLine(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x0600AE9F RID: 44703 RVA: 0x003D814D File Offset: 0x003D634D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.initScanner, "initScanner", false, false);
			Scribe_Values.Look<int>(ref this.fabricationTicksLeft, "fabricationTicksLeft", 0, false);
		}

		// Token: 0x040064F8 RID: 25848
		private bool initScanner;

		// Token: 0x040064F9 RID: 25849
		private int fabricationTicksLeft;

		// Token: 0x040064FA RID: 25850
		private Effecter effectStart;

		// Token: 0x040064FB RID: 25851
		private Effecter effectHusk;

		// Token: 0x040064FC RID: 25852
		private bool debugDisableNeedForIngredients;

		// Token: 0x040064FD RID: 25853
		private Mote workingMote;

		// Token: 0x040064FE RID: 25854
		private Sustainer sustainerWorking;

		// Token: 0x040064FF RID: 25855
		private Effecter progressBarEffecter;

		// Token: 0x04006500 RID: 25856
		public static readonly Texture2D CancelLoadingIcon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		// Token: 0x04006501 RID: 25857
		public static readonly CachedTexture InsertPersonIcon = new CachedTexture("UI/Icons/InsertPersonSubcoreScanner");

		// Token: 0x04006502 RID: 25858
		private static Dictionary<Rot4, ThingDef> MotePerRotation;

		// Token: 0x04006503 RID: 25859
		private static Dictionary<Rot4, ThingDef> MotePerRotationRip;

		// Token: 0x04006504 RID: 25860
		private static readonly Dictionary<Rot4, Vector3> HuskEffectOffsets = new Dictionary<Rot4, Vector3>
		{
			{
				Rot4.North,
				new Vector3(0f, 0f, 0.47f)
			},
			{
				Rot4.South,
				new Vector3(0f, 0f, -0.3f)
			},
			{
				Rot4.East,
				new Vector3(0.4f, 0f, -0.025f)
			},
			{
				Rot4.West,
				new Vector3(-0.4f, 0f, -0.025f)
			}
		};

		// Token: 0x04006505 RID: 25861
		private const float ProgressBarOffsetZ = -0.8f;

		// Token: 0x04006506 RID: 25862
		public CachedTexture InitScannerIcon = new CachedTexture("UI/Icons/SubcoreScannerStart");
	}
}
