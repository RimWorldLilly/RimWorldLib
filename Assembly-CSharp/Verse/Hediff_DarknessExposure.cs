using System;
using System.Collections.Generic;
using RimWorld;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020004B7 RID: 1207
	public class Hediff_DarknessExposure : Hediff
	{
		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x06002180 RID: 8576 RVA: 0x000C70D2 File Offset: 0x000C52D2
		public override bool ShouldRemove
		{
			get
			{
				return !this.pawn.Spawned || this.pawn.Downed || !GameCondition_UnnaturalDarkness.InUnnaturalDarkness(this.pawn);
			}
		}

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x06002181 RID: 8577 RVA: 0x000C70FE File Offset: 0x000C52FE
		private DamageDef RandomDamageDef
		{
			get
			{
				if (Hediff_DarknessExposure.possibleDamageDefs == null)
				{
					Hediff_DarknessExposure.possibleDamageDefs = new List<DamageDef>
					{
						DamageDefOf.Scratch,
						DamageDefOf.Bite,
						DamageDefOf.Cut
					};
				}
				return Hediff_DarknessExposure.possibleDamageDefs.RandomElement<DamageDef>();
			}
		}

		// Token: 0x06002182 RID: 8578 RVA: 0x000C713C File Offset: 0x000C533C
		public override void PostAdd(DamageInfo? dinfo)
		{
			if (!ModLister.CheckAnomaly("Darkness exposure"))
			{
				this.pawn.health.RemoveHediff(this);
				return;
			}
			base.PostAdd(dinfo);
		}

		// Token: 0x06002183 RID: 8579 RVA: 0x000C7164 File Offset: 0x000C5364
		public override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (this.nextDamageTick < 0)
			{
				this.nextDamageTick = Find.TickManager.TicksGame + Hediff_DarknessExposure.DamageIntervalTicks_Fresh.RandomInRange;
			}
			if (Find.TickManager.TicksGame >= this.nextDamageTick)
			{
				SoundDefOf.DarknessDamage.PlayOneShot(this.pawn);
				BattleLogEntry_DamageTaken battleLogEntry_DamageTaken = new BattleLogEntry_DamageTaken(this.pawn, RulePackDefOf.DamageEvent_UnnaturalDarkness, null);
				Find.BattleLog.Add(battleLogEntry_DamageTaken);
				this.pawn.TakeDamage(new DamageInfo(this.RandomDamageDef, Hediff_DarknessExposure.DamageRange.RandomInRange, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false)).AssociateWithLog(battleLogEntry_DamageTaken);
				this.nextDamageTick = Find.TickManager.TicksGame + Hediff_DarknessExposure.DamageIntervalTicks_NonFresh.RandomInRange;
				this.CheckNotifyPlayer();
			}
		}

		// Token: 0x06002184 RID: 8580 RVA: 0x000C7248 File Offset: 0x000C5448
		private void CheckNotifyPlayer()
		{
			if (!PawnUtility.ShouldSendNotificationAbout(this.pawn) && !this.pawn.IsColonySubhumanPlayerControlled)
			{
				return;
			}
			if (this.lastNotifyTick + 5000 < Find.TickManager.TicksGame)
			{
				GameCondition_UnnaturalDarkness activeCondition = this.pawn.MapHeld.gameConditionManager.GetActiveCondition<GameCondition_UnnaturalDarkness>();
				if (activeCondition != null && !activeCondition.anyColonistAttacked)
				{
					Find.LetterStack.ReceiveLetter("PawnAttackedInDarknessLabel".Translate(this.pawn), "PawnAttackedInDarknessText".Translate(this.pawn), LetterDefOf.ThreatSmall, this.pawn, null, null, null, null, 0, true);
					activeCondition.anyColonistAttacked = true;
				}
				else
				{
					Messages.Message("MessagePawnAttackedInDarkness".Translate(this.pawn), this.pawn, MessageTypeDefOf.ThreatSmall, false);
				}
				this.lastNotifyTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x06002185 RID: 8581 RVA: 0x000C733F File Offset: 0x000C553F
		public override bool TryMergeWith(Hediff other)
		{
			return false;
		}

		// Token: 0x06002186 RID: 8582 RVA: 0x000C7342 File Offset: 0x000C5542
		public override void Notify_Downed()
		{
			this.nextDamageTick = -1;
		}

		// Token: 0x06002187 RID: 8583 RVA: 0x000C734B File Offset: 0x000C554B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.nextDamageTick, "nextDamageTick", -1, false);
			Scribe_Values.Look<int>(ref this.lastNotifyTick, "lastNotifyTick", -1, false);
		}

		// Token: 0x04001970 RID: 6512
		private int nextDamageTick = -1;

		// Token: 0x04001971 RID: 6513
		private int lastNotifyTick = -1;

		// Token: 0x04001972 RID: 6514
		private static readonly IntRange DamageIntervalTicks_Fresh = new IntRange(480, 720);

		// Token: 0x04001973 RID: 6515
		private static readonly IntRange DamageIntervalTicks_NonFresh = new IntRange(180, 300);

		// Token: 0x04001974 RID: 6516
		private static readonly FloatRange DamageRange = new FloatRange(2.5f, 5.5f);

		// Token: 0x04001975 RID: 6517
		private const int TicksBetweenWarnings = 5000;

		// Token: 0x04001976 RID: 6518
		private static List<DamageDef> possibleDamageDefs;
	}
}
