using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020004A6 RID: 1190
	public class Hediff_CubeInterest : Hediff
	{
		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x060020EF RID: 8431 RVA: 0x000C4ABC File Offset: 0x000C2CBC
		public Hediff_CubeWithdrawal WithdrawalHediff
		{
			get
			{
				Hediff_CubeWithdrawal result;
				if ((result = this.cachedHediff) == null)
				{
					result = (this.cachedHediff = this.pawn.health.hediffSet.GetFirstHediff<Hediff_CubeWithdrawal>());
				}
				return result;
			}
		}

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x060020F0 RID: 8432 RVA: 0x000C4AF1 File Offset: 0x000C2CF1
		public bool InWithdrawal
		{
			get
			{
				return this.pawn.health.hediffSet.HasHediff(HediffDefOf.CubeWithdrawal, false);
			}
		}

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x060020F1 RID: 8433 RVA: 0x000C4B0E File Offset: 0x000C2D0E
		public int Sculptures
		{
			get
			{
				return this.sculptures.Count;
			}
		}

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x060020F2 RID: 8434 RVA: 0x000C4B1C File Offset: 0x000C2D1C
		public bool InCaravanWithCube
		{
			get
			{
				Caravan caravan = this.pawn.GetCaravan();
				return caravan != null && CaravanInventoryUtility.HasThings(caravan, ThingDefOf.GoldenCube, 1, null);
			}
		}

		// Token: 0x060020F3 RID: 8435 RVA: 0x000C4B47 File Offset: 0x000C2D47
		public override void PostAdd(DamageInfo? dinfo)
		{
			if (!ModLister.CheckAnomaly("Cube interest"))
			{
				this.pawn.health.RemoveHediff(this);
				return;
			}
			base.PostAdd(dinfo);
		}

		// Token: 0x060020F4 RID: 8436 RVA: 0x000C4B70 File Offset: 0x000C2D70
		public override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (this.ageTicks <= 1000)
			{
				return;
			}
			if (Rand.MTBEventOccurs(1.5f, 60000f, (float)delta) && !this.InWithdrawal && !this.InCaravanWithCube)
			{
				this.StartWithdrawal();
			}
			if (Rand.MTBEventOccurs(Hediff_CubeInterest.MentalBreakSeverityDaysMTB.Evaluate(this.Severity), 60000f, (float)delta) && !this.pawn.InMentalState && this.pawn.CurJobDef != JobDefOf.GoldenCubePlay && this.pawn.SpawnedOrAnyParentSpawned)
			{
				this.DoMentalBreak();
			}
			if (GenTicks.IsTickIntervalDelta(60, delta))
			{
				this.UpdateSculptures();
			}
		}

		// Token: 0x060020F5 RID: 8437 RVA: 0x000C4C1C File Offset: 0x000C2E1C
		private void UpdateSculptures()
		{
			bool flag = false;
			for (int i = this.sculptures.Count - 1; i >= 0; i--)
			{
				if (this.sculptures[i].DestroyedOrNull() || (!this.sculptures[i].SpawnedOrAnyParentSpawned && !this.sculptures[i].IsInCaravan()))
				{
					flag = true;
					this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.DestroyedCubeSculpture, null, null);
					this.sculptures.RemoveAt(i);
					float num = Hediff_CubeInterest.CubeRageSeverityIncrease.Evaluate(this.Severity);
					if (num > 0f)
					{
						HealthUtility.AdjustSeverity(this.pawn, HediffDefOf.CubeRage, num);
					}
				}
			}
			if (flag)
			{
				Messages.Message("MessageGoldenCubeSculptureDestroyed".Translate(this.pawn.Named("PAWN")), this.pawn, MessageTypeDefOf.NeutralEvent, true);
			}
		}

		// Token: 0x060020F6 RID: 8438 RVA: 0x000C4D18 File Offset: 0x000C2F18
		public override void PostRemoved()
		{
			base.PostRemoved();
			Hediff hediff;
			if (this.pawn.health.hediffSet.TryGetHediff(HediffDefOf.CubeRage, out hediff))
			{
				this.pawn.health.RemoveHediff(hediff);
			}
		}

		// Token: 0x060020F7 RID: 8439 RVA: 0x000C4D5C File Offset: 0x000C2F5C
		protected override void OnStageIndexChanged(int stageIndex)
		{
			if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				Messages.Message("MessageGoldenCubeSeverityIncreased".Translate(this.pawn.Named("PAWN")), this.pawn, MessageTypeDefOf.NeutralEvent, true);
			}
		}

		// Token: 0x060020F8 RID: 8440 RVA: 0x000C4DAB File Offset: 0x000C2FAB
		public void Notify_BuiltSculpture(Thing sculpture)
		{
			this.sculptures.Add(sculpture);
		}

		// Token: 0x060020F9 RID: 8441 RVA: 0x000C4DBC File Offset: 0x000C2FBC
		public void Notify_PlayedWith()
		{
			if (this.InWithdrawal)
			{
				this.pawn.health.RemoveHediff(this.WithdrawalHediff);
				this.cachedHediff = null;
			}
			this.Severity += Hediff_CubeInterest.SeverityPerPlay.RandomInRange;
			Pawn_NeedsTracker needs = this.pawn.needs;
			if (needs == null)
			{
				return;
			}
			Need_Mood mood = needs.mood;
			if (mood == null)
			{
				return;
			}
			ThoughtHandler thoughts = mood.thoughts;
			if (thoughts == null)
			{
				return;
			}
			MemoryThoughtHandler memories = thoughts.memories;
			if (memories == null)
			{
				return;
			}
			memories.TryGainMemory(ThoughtDefOf.PlayedWithCube, null, null);
		}

		// Token: 0x060020FA RID: 8442 RVA: 0x000C4E42 File Offset: 0x000C3042
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Hediff_CubeInterest.<GetGizmos>d__23 <GetGizmos>d__ = new Hediff_CubeInterest.<GetGizmos>d__23(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x060020FB RID: 8443 RVA: 0x000C4E54 File Offset: 0x000C3054
		private void DoMentalBreak()
		{
			if (this.pawn.InMentalState)
			{
				return;
			}
			this.pawn.mindState.mentalBreaker.TryDoMentalBreak("MentalBreakReason_CubeSculpting".Translate(this.pawn.Named("PAWN")), MentalBreakDefOf.CubeSculpting);
		}

		// Token: 0x060020FC RID: 8444 RVA: 0x000C4EAC File Offset: 0x000C30AC
		private void StartWithdrawal()
		{
			if (this.InWithdrawal)
			{
				return;
			}
			this.cachedHediff = (Hediff_CubeWithdrawal)this.pawn.health.AddHediff(HediffDefOf.CubeWithdrawal, null, null, null);
		}

		// Token: 0x060020FD RID: 8445 RVA: 0x000C4EED File Offset: 0x000C30ED
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Thing>(ref this.sculptures, "sculptures", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x0400193D RID: 6461
		private List<Thing> sculptures = new List<Thing>();

		// Token: 0x0400193E RID: 6462
		private Hediff_CubeWithdrawal cachedHediff;

		// Token: 0x0400193F RID: 6463
		private const float MTBPlayWantedDays = 1.5f;

		// Token: 0x04001940 RID: 6464
		private const int InitialDelayTicks = 1000;

		// Token: 0x04001941 RID: 6465
		private const int SculptureUpdateRateTicks = 60;

		// Token: 0x04001942 RID: 6466
		private static readonly FloatRange SeverityPerPlay = new FloatRange(0.045f, 0.105f);

		// Token: 0x04001943 RID: 6467
		private static readonly SimpleCurve MentalBreakSeverityDaysMTB = new SimpleCurve
		{
			{
				new CurvePoint(0.01f, 5f),
				true
			},
			{
				new CurvePoint(0.5f, 3f),
				true
			},
			{
				new CurvePoint(0.8f, 2f),
				true
			}
		};

		// Token: 0x04001944 RID: 6468
		private static readonly SimpleCurve CubeRageSeverityIncrease = new SimpleCurve
		{
			{
				new CurvePoint(0.1f, 0.1f),
				true
			},
			{
				new CurvePoint(0.5f, 0.2f),
				true
			},
			{
				new CurvePoint(1f, 0.4f),
				true
			}
		};
	}
}
