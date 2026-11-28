using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002094 RID: 8340
	public class Pawn_FlightTracker : IExposable
	{
		// Token: 0x170025BD RID: 9661
		// (get) Token: 0x0600D236 RID: 53814 RVA: 0x004D3685 File Offset: 0x004D1885
		public bool Flying
		{
			get
			{
				return this.flightState > Pawn_FlightTracker.FlightState.Grounded;
			}
		}

		// Token: 0x170025BE RID: 9662
		// (get) Token: 0x0600D237 RID: 53815 RVA: 0x004D3690 File Offset: 0x004D1890
		public bool CanEverFly
		{
			get
			{
				if (this.pawn.IsMutant && this.pawn.mutant.Def.disableFlying)
				{
					return false;
				}
				if (!this.pawn.RaceProps.canFlyInVacuum)
				{
					Map mapHeld = this.pawn.MapHeld;
					if (mapHeld != null)
					{
						BiomeDef biome = mapHeld.Biome;
						if (biome != null && biome.inVacuum)
						{
							return false;
						}
					}
				}
				return this.pawn.GetStatValue(StatDefOf.MaxFlightTime, true, 300) > 0f;
			}
		}

		// Token: 0x170025BF RID: 9663
		// (get) Token: 0x0600D238 RID: 53816 RVA: 0x004D3715 File Offset: 0x004D1915
		public bool CanFlyNow
		{
			get
			{
				return !this.Flying && this.CanEverFly && this.flightCooldownTicks <= 0;
			}
		}

		// Token: 0x170025C0 RID: 9664
		// (get) Token: 0x0600D239 RID: 53817 RVA: 0x004D3735 File Offset: 0x004D1935
		public int MaxFlightTicks
		{
			get
			{
				return Mathf.RoundToInt(this.pawn.GetStatValue(StatDefOf.MaxFlightTime, true, 300) * 60f);
			}
		}

		// Token: 0x170025C1 RID: 9665
		// (get) Token: 0x0600D23A RID: 53818 RVA: 0x004D3758 File Offset: 0x004D1958
		public float PositionOffsetFactor
		{
			get
			{
				switch (this.flightState)
				{
				case Pawn_FlightTracker.FlightState.Flying:
					return 1f;
				case Pawn_FlightTracker.FlightState.TakingOff:
					return Pawn_FlightTracker.TakeoffCurve.Evaluate((float)this.lerpTick / 50f);
				case Pawn_FlightTracker.FlightState.Landing:
					return Pawn_FlightTracker.LandingCurve.Evaluate((float)this.lerpTick / 50f);
				}
				return 0f;
			}
		}

		// Token: 0x0600D23B RID: 53819 RVA: 0x004D37BF File Offset: 0x004D19BF
		public Pawn_FlightTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600D23C RID: 53820 RVA: 0x004D37D8 File Offset: 0x004D19D8
		public void FlightTick()
		{
			this.CheckFlyAnimation();
			switch (this.flightState)
			{
			case Pawn_FlightTracker.FlightState.Grounded:
				if (this.flightCooldownTicks > 0)
				{
					this.flightCooldownTicks--;
				}
				if (this.pawn.CurJob != null && this.pawn.CurJob.flying)
				{
					this.pawn.CurJob.flying = false;
				}
				break;
			case Pawn_FlightTracker.FlightState.Flying:
				this.flyingTicks++;
				if (this.flyingTicks >= this.MaxFlightTicks)
				{
					this.flightState = Pawn_FlightTracker.FlightState.Landing;
					return;
				}
				break;
			case Pawn_FlightTracker.FlightState.TakingOff:
				this.lerpTick++;
				if (this.lerpTick >= 50)
				{
					this.flightState = Pawn_FlightTracker.FlightState.Flying;
					this.lerpTick = 0;
					return;
				}
				break;
			case Pawn_FlightTracker.FlightState.Landing:
				this.lerpTick++;
				if (this.lerpTick >= 50)
				{
					this.flightState = Pawn_FlightTracker.FlightState.Grounded;
					this.lerpTick = 0;
					this.flightCooldownTicks = Mathf.RoundToInt(this.pawn.GetStatValue(StatDefOf.FlightCooldown, true, -1) * 60f);
					this.flyingTicks = -1;
					if (this.pawn.Downed && !this.pawn.Position.WalkableBy(this.pawn.Map, this.pawn))
					{
						this.pawn.Kill(null, null);
						Corpse corpse = this.pawn.Corpse;
						if (corpse != null && !corpse.Destroyed)
						{
							corpse.Destroy(DestroyMode.Vanish);
							return;
						}
					}
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x0600D23D RID: 53821 RVA: 0x004D395B File Offset: 0x004D1B5B
		public void Notify_PreSwapMap()
		{
			this.playing = null;
			this.pawn.Drawer.renderer.SetAnimation(null);
		}

		// Token: 0x0600D23E RID: 53822 RVA: 0x004D397C File Offset: 0x004D1B7C
		public void Notify_JobStarted(Job job)
		{
			if (!this.CanEverFly)
			{
				return;
			}
			if (job.def.tryStartFlying && this.CanFlyNow)
			{
				if (Rand.Chance((job.def.overrideFlyChance >= 0f) ? job.def.overrideFlyChance : this.pawn.RaceProps.flightStartChanceOnJobStart))
				{
					this.StartFlyingInternal();
					job.flying = true;
					return;
				}
			}
			else if ((job.def.tryStartFlying || job.def.ifFlyingKeepFlying) && this.Flying)
			{
				job.flying = true;
				return;
			}
			job.flying = false;
			if (this.Flying)
			{
				this.ForceLand();
			}
		}

		// Token: 0x0600D23F RID: 53823 RVA: 0x004D3A2A File Offset: 0x004D1C2A
		public void StartFlying()
		{
			if (!this.CanFlyNow)
			{
				return;
			}
			this.StartFlyingInternal();
		}

		// Token: 0x0600D240 RID: 53824 RVA: 0x004D3A3B File Offset: 0x004D1C3B
		private void StartFlyingInternal()
		{
			this.flightState = Pawn_FlightTracker.FlightState.TakingOff;
			this.lerpTick = 0;
			this.flyingTicks = -1;
			this.CheckFlyAnimation();
		}

		// Token: 0x0600D241 RID: 53825 RVA: 0x004D3A58 File Offset: 0x004D1C58
		private void CheckFlyAnimation()
		{
			bool flying = this.Flying;
			if (!flying && this.playing != null)
			{
				if (this.pawn.Drawer.renderer.CurAnimation == this.playing)
				{
					this.pawn.Drawer.renderer.SetAnimation(null);
				}
				this.playing = null;
				return;
			}
			if (flying)
			{
				AnimationDef bestFlyAnimation = Pawn_FlightTracker.GetBestFlyAnimation(this.pawn, null);
				if (this.pawn.Drawer.renderer.CurAnimation == bestFlyAnimation)
				{
					return;
				}
				this.pawn.Drawer.renderer.SetAnimation(bestFlyAnimation);
				this.playing = bestFlyAnimation;
			}
		}

		// Token: 0x0600D242 RID: 53826 RVA: 0x004D3B00 File Offset: 0x004D1D00
		public void ForceLand()
		{
			if (this.flightState != Pawn_FlightTracker.FlightState.TakingOff && this.flightState != Pawn_FlightTracker.FlightState.Flying)
			{
				return;
			}
			this.flightState = Pawn_FlightTracker.FlightState.Landing;
			this.lerpTick = 25;
			this.flightCooldownTicks = Mathf.RoundToInt(this.pawn.GetStatValue(StatDefOf.FlightCooldown, true, -1) * 60f) + 50;
		}

		// Token: 0x0600D243 RID: 53827 RVA: 0x004D3B58 File Offset: 0x004D1D58
		public string GetStatusString()
		{
			if (this.Flying)
			{
				return "Flying".Translate().CapitalizeFirst();
			}
			return null;
		}

		// Token: 0x0600D244 RID: 53828 RVA: 0x004D3B88 File Offset: 0x004D1D88
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.flyingTicks, "flyingTicks", -1, false);
			Scribe_Values.Look<int>(ref this.flightCooldownTicks, "flightCooldownTicks", 0, false);
			Scribe_Values.Look<int>(ref this.lerpTick, "lerpTick", 0, false);
			Scribe_Values.Look<Pawn_FlightTracker.FlightState>(ref this.flightState, "flightState", Pawn_FlightTracker.FlightState.Grounded, false);
			Scribe_Defs.Look<AnimationDef>(ref this.playing, "playing");
		}

		// Token: 0x0600D245 RID: 53829 RVA: 0x004D3BF0 File Offset: 0x004D1DF0
		public static AnimationDef GetBestFlyAnimation(Pawn pawn, Rot4? facingOverride = null)
		{
			if (((pawn != null) ? pawn.kindDef : null) == null)
			{
				return null;
			}
			if (pawn.RaceProps.Humanlike)
			{
				return null;
			}
			Rot4 a = facingOverride ?? pawn.Rotation;
			bool flag = a == Rot4.South;
			bool flag2 = a == Rot4.North;
			bool flag3 = pawn.gender == Gender.Female;
			Pawn_AgeTracker ageTracker = pawn.ageTracker;
			if (ageTracker != null)
			{
				PawnKindLifeStage pawnKindLifeStage = ageTracker.CurKindLifeStage;
				if (pawnKindLifeStage != null)
				{
					if (flag && pawnKindLifeStage.flyingAnimationSouth != null)
					{
						if (flag3 && pawnKindLifeStage.flyingAnimationSouthFemale != null)
						{
							return pawnKindLifeStage.flyingAnimationSouthFemale;
						}
						return pawnKindLifeStage.flyingAnimationSouth;
					}
					else if ((flag2 || flag) && pawnKindLifeStage.flyingAnimationNorth != null)
					{
						if (flag3 && pawnKindLifeStage.flyingAnimationNorthFemale != null)
						{
							return pawnKindLifeStage.flyingAnimationNorthFemale;
						}
						return pawnKindLifeStage.flyingAnimationNorth;
					}
					else if (pawnKindLifeStage.flyingAnimationEast != null)
					{
						if (flag3 && pawnKindLifeStage.flyingAnimationEastFemale != null)
						{
							return pawnKindLifeStage.flyingAnimationEastFemale;
						}
						return pawnKindLifeStage.flyingAnimationEast;
					}
					else if (pawnKindLifeStage.flyingAnimationNorth != null)
					{
						if (flag3 && pawnKindLifeStage.flyingAnimationNorthFemale != null)
						{
							return pawnKindLifeStage.flyingAnimationNorthFemale;
						}
						return pawnKindLifeStage.flyingAnimationNorth;
					}
					else if (pawnKindLifeStage.flyingAnimationSouth != null)
					{
						if (flag3 && pawnKindLifeStage.flyingAnimationSouthFemale != null)
						{
							return pawnKindLifeStage.flyingAnimationSouthFemale;
						}
						return pawnKindLifeStage.flyingAnimationSouth;
					}
				}
			}
			if (pawn.ageTracker == null)
			{
				return null;
			}
			int i = pawn.ageTracker.CurLifeStageIndex;
			while (i > 0)
			{
				i--;
				PawnKindLifeStage pawnKindLifeStage = pawn.kindDef.lifeStages[i];
				if (flag && pawnKindLifeStage.flyingAnimationSouth != null)
				{
					if (flag3 && pawnKindLifeStage.flyingAnimationSouthFemale != null)
					{
						return pawnKindLifeStage.flyingAnimationSouthFemale;
					}
					return pawnKindLifeStage.flyingAnimationSouth;
				}
				else if ((flag2 || flag) && pawnKindLifeStage.flyingAnimationNorth != null)
				{
					if (flag3 && pawnKindLifeStage.flyingAnimationNorthFemale != null)
					{
						return pawnKindLifeStage.flyingAnimationNorthFemale;
					}
					return pawnKindLifeStage.flyingAnimationNorth;
				}
				else if (pawnKindLifeStage.flyingAnimationEast != null)
				{
					if (flag3 && pawnKindLifeStage.flyingAnimationEastFemale != null)
					{
						return pawnKindLifeStage.flyingAnimationEastFemale;
					}
					return pawnKindLifeStage.flyingAnimationEast;
				}
				else if (pawnKindLifeStage.flyingAnimationNorth != null)
				{
					if (flag3 && pawnKindLifeStage.flyingAnimationNorthFemale != null)
					{
						return pawnKindLifeStage.flyingAnimationNorthFemale;
					}
					return pawnKindLifeStage.flyingAnimationNorth;
				}
				else if (pawnKindLifeStage.flyingAnimationSouth != null)
				{
					if (flag3 && pawnKindLifeStage.flyingAnimationSouthFemale != null)
					{
						return pawnKindLifeStage.flyingAnimationSouthFemale;
					}
					return pawnKindLifeStage.flyingAnimationSouth;
				}
			}
			return null;
		}

		// Token: 0x0400852B RID: 34091
		private Pawn pawn;

		// Token: 0x0400852C RID: 34092
		private int flyingTicks = -1;

		// Token: 0x0400852D RID: 34093
		private int flightCooldownTicks;

		// Token: 0x0400852E RID: 34094
		private int lerpTick;

		// Token: 0x0400852F RID: 34095
		private AnimationDef playing;

		// Token: 0x04008530 RID: 34096
		private Pawn_FlightTracker.FlightState flightState;

		// Token: 0x04008531 RID: 34097
		private const int TakeoffDurationTicks = 50;

		// Token: 0x04008532 RID: 34098
		private const int LandingDurationTicks = 50;

		// Token: 0x04008533 RID: 34099
		private static readonly SimpleCurve TakeoffCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(0.5f, 0.6f),
				true
			},
			{
				new CurvePoint(1f, 1f),
				true
			}
		};

		// Token: 0x04008534 RID: 34100
		private static readonly SimpleCurve LandingCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(0.5f, 0.4f),
				true
			},
			{
				new CurvePoint(1f, 0f),
				true
			}
		};

		// Token: 0x02003C5D RID: 15453
		private enum FlightState
		{
			// Token: 0x0400EF4E RID: 61262
			Grounded,
			// Token: 0x0400EF4F RID: 61263
			Flying,
			// Token: 0x0400EF50 RID: 61264
			TakingOff,
			// Token: 0x0400EF51 RID: 61265
			Landing
		}
	}
}
