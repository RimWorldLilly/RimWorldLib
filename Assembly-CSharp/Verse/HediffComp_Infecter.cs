using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200044C RID: 1100
	public class HediffComp_Infecter : HediffComp
	{
		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x06001F76 RID: 8054 RVA: 0x000BF293 File Offset: 0x000BD493
		public HediffCompProperties_Infecter Props
		{
			get
			{
				return (HediffCompProperties_Infecter)this.props;
			}
		}

		// Token: 0x06001F77 RID: 8055 RVA: 0x000BF2A0 File Offset: 0x000BD4A0
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			if (this.parent.IsPermanent())
			{
				this.ticksUntilInfect = -2;
				return;
			}
			if (this.parent.Part != null)
			{
				if (this.parent.Part.def.IsSolid(this.parent.Part, base.Pawn.health.hediffSet.hediffs))
				{
					this.ticksUntilInfect = -2;
					return;
				}
				if (base.Pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(this.parent.Part))
				{
					this.ticksUntilInfect = -2;
					return;
				}
			}
			float num = this.Props.infectionChance;
			if (base.Pawn.RaceProps.Animal)
			{
				num *= 0.1f;
			}
			if (Rand.Value <= num)
			{
				this.ticksUntilInfect = HealthTuning.InfectionDelayRange.RandomInRange;
				return;
			}
			this.ticksUntilInfect = -2;
		}

		// Token: 0x06001F78 RID: 8056 RVA: 0x000BF385 File Offset: 0x000BD585
		public override void CompExposeData()
		{
			Scribe_Values.Look<float>(ref this.infectionChanceFactorFromTendRoom, "infectionChanceFactor", 0f, false);
			Scribe_Values.Look<int>(ref this.ticksUntilInfect, "ticksUntilInfect", -2, false);
			Scribe_Values.Look<bool>(ref this.fromScaria, "fromScaria", false, false);
		}

		// Token: 0x06001F79 RID: 8057 RVA: 0x000BF3C2 File Offset: 0x000BD5C2
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			if (this.ticksUntilInfect > 0)
			{
				this.ticksUntilInfect -= delta;
				if (this.ticksUntilInfect <= 0)
				{
					this.CheckMakeInfection();
				}
			}
		}

		// Token: 0x06001F7A RID: 8058 RVA: 0x000BF3EC File Offset: 0x000BD5EC
		public override void CompTended(float quality, float maxQuality, int batchPosition = 0)
		{
			base.CompTended(quality, maxQuality, batchPosition);
			if (base.Pawn.Spawned)
			{
				Room room = base.Pawn.GetRoom(RegionType.Set_All);
				if (room != null)
				{
					this.infectionChanceFactorFromTendRoom = room.GetStat(RoomStatDefOf.InfectionChanceFactor);
				}
			}
		}

		// Token: 0x06001F7B RID: 8059 RVA: 0x000BF434 File Offset: 0x000BD634
		private void CheckMakeInfection()
		{
			if (base.Pawn.health.immunity.DiseaseContractChanceFactor(HediffDefOf.WoundInfection, this.parent.Part) <= 0.001f)
			{
				this.ticksUntilInfect = -3;
				return;
			}
			if (base.Pawn.health.hediffSet.HasRegeneration)
			{
				this.ticksUntilInfect = -3;
				return;
			}
			float num = 1f;
			HediffComp_TendDuration hediffComp_TendDuration = this.parent.TryGetComp<HediffComp_TendDuration>();
			if (hediffComp_TendDuration != null && hediffComp_TendDuration.IsTended)
			{
				num *= this.infectionChanceFactorFromTendRoom;
				num *= HediffComp_Infecter.InfectionChanceFactorFromTendQualityCurve.Evaluate(hediffComp_TendDuration.tendQuality);
			}
			num *= HediffComp_Infecter.InfectionChanceFactorFromSeverityCurve.Evaluate(this.parent.Severity);
			if (base.Pawn.Faction == Faction.OfPlayer)
			{
				num *= Find.Storyteller.difficulty.playerPawnInfectionChanceFactor;
			}
			if (Rand.Value < num)
			{
				this.ticksUntilInfect = -4;
				base.Pawn.health.AddHediff(this.fromScaria ? HediffDefOf.ScariaInfection : HediffDefOf.WoundInfection, this.parent.Part, null, null);
				return;
			}
			this.ticksUntilInfect = -3;
		}

		// Token: 0x06001F7C RID: 8060 RVA: 0x000BF560 File Offset: 0x000BD760
		public override string CompDebugString()
		{
			if (this.ticksUntilInfect > 0)
			{
				return "infection may appear in: " + this.ticksUntilInfect.ToString() + " ticks\ninfectChnceFactorFromTendRoom: " + this.infectionChanceFactorFromTendRoom.ToStringPercent();
			}
			if (this.ticksUntilInfect == -4)
			{
				return "already created infection";
			}
			if (this.ticksUntilInfect == -3)
			{
				return "failed to make infection";
			}
			if (this.ticksUntilInfect == -2)
			{
				return "will not make infection";
			}
			if (this.ticksUntilInfect == -1)
			{
				return "uninitialized data!";
			}
			return "unexpected ticksUntilInfect = " + this.ticksUntilInfect.ToString();
		}

		// Token: 0x0400188A RID: 6282
		private int ticksUntilInfect = -1;

		// Token: 0x0400188B RID: 6283
		private float infectionChanceFactorFromTendRoom = 1f;

		// Token: 0x0400188C RID: 6284
		public bool fromScaria;

		// Token: 0x0400188D RID: 6285
		private const int UninitializedValue = -1;

		// Token: 0x0400188E RID: 6286
		private const int WillNotInfectValue = -2;

		// Token: 0x0400188F RID: 6287
		private const int FailedToMakeInfectionValue = -3;

		// Token: 0x04001890 RID: 6288
		private const int AlreadyMadeInfectionValue = -4;

		// Token: 0x04001891 RID: 6289
		private static readonly SimpleCurve InfectionChanceFactorFromTendQualityCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.7f),
				true
			},
			{
				new CurvePoint(1f, 0.4f),
				true
			}
		};

		// Token: 0x04001892 RID: 6290
		private static readonly SimpleCurve InfectionChanceFactorFromSeverityCurve = new SimpleCurve
		{
			{
				new CurvePoint(1f, 0.1f),
				true
			},
			{
				new CurvePoint(12f, 1f),
				true
			}
		};
	}
}
