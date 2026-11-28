using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CDC RID: 3292
	public class LordToil_HateChant : LordToil
	{
		// Token: 0x1700103F RID: 4159
		// (get) Token: 0x06005D1D RID: 23837 RVA: 0x00222820 File Offset: 0x00220A20
		protected LordToilData_HateChant Data
		{
			get
			{
				return (LordToilData_HateChant)this.data;
			}
		}

		// Token: 0x06005D1E RID: 23838 RVA: 0x0022282D File Offset: 0x00220A2D
		public LordToil_HateChant()
		{
			this.data = new LordToilData_HateChant();
		}

		// Token: 0x06005D1F RID: 23839 RVA: 0x0022284B File Offset: 0x00220A4B
		public LordToil_HateChant(IEnumerable<PsychicRitualParticipant> participants)
		{
			this.data = new LordToilData_HateChant();
			this.SetParticipants(participants);
		}

		// Token: 0x06005D20 RID: 23840 RVA: 0x00222870 File Offset: 0x00220A70
		public override void Init()
		{
			base.Init();
			this.TryCreatingDrone();
			this.Data.lastDroneUpdate = Find.TickManager.TicksGame;
		}

		// Token: 0x06005D21 RID: 23841 RVA: 0x00222894 File Offset: 0x00220A94
		public override void UpdateAllDuties()
		{
			foreach (PsychicRitualParticipant psychicRitualParticipant in this.Data.chanters)
			{
				Pawn pawn;
				IntVec3 intVec;
				psychicRitualParticipant.Deconstruct(out pawn, out intVec);
				Pawn pawn2 = pawn;
				IntVec3 c = intVec;
				if (((pawn2 != null) ? pawn2.mindState : null) != null)
				{
					pawn2.mindState.duty = new PawnDuty(DutyDefOf.PerformHateChant, c, -1f);
				}
				if (pawn2 != null)
				{
					pawn2.health.AddHediff(HediffDefOf.PsychicTrance, null, null, null);
				}
			}
		}

		// Token: 0x06005D22 RID: 23842 RVA: 0x00222940 File Offset: 0x00220B40
		public override void LordToilTick()
		{
			base.LordToilTick();
			if (this.Data.condition != null && this.Data.condition.level < HateChantDroneLevel.Extreme && Find.TickManager.TicksGame > this.Data.lastDroneUpdate + this.DroneBoostInterval)
			{
				this.Data.lastDroneUpdate = Find.TickManager.TicksGame;
				GameCondition_HateChantDrone condition = this.Data.condition;
				condition.level += 1;
				Messages.Message("MessageHateChantIncreased".Translate(), MessageTypeDefOf.ThreatSmall, true);
			}
		}

		// Token: 0x06005D23 RID: 23843 RVA: 0x002229D9 File Offset: 0x00220BD9
		public override void Cleanup()
		{
			base.Cleanup();
			if (this.Data.condition == null)
			{
				return;
			}
			this.Data.condition.End();
			this.Data.condition = null;
		}

		// Token: 0x06005D24 RID: 23844 RVA: 0x00222A0C File Offset: 0x00220C0C
		public void SetParticipants(IEnumerable<PsychicRitualParticipant> participants)
		{
			PsychicRitualParticipant[] collection = (participants as PsychicRitualParticipant[]) ?? participants.ToArray<PsychicRitualParticipant>();
			this.Data.chanters = new List<PsychicRitualParticipant>(collection);
		}

		// Token: 0x06005D25 RID: 23845 RVA: 0x00222A3C File Offset: 0x00220C3C
		private void TryCreatingDrone()
		{
			GameConditionManager gameConditionManager = this.lord.Map.gameConditionManager;
			if (gameConditionManager == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Couldn't find condition manager for incident target ");
				defaultInterpolatedStringHandler.AppendFormatted<Map>(this.lord.Map);
				Log.ErrorOnce(defaultInterpolatedStringHandler.ToStringAndClear(), 70849667);
				return;
			}
			if (gameConditionManager.ConditionIsActive(GameConditionDefOf.HateChantDrone))
			{
				return;
			}
			List<GameCondition> activeConditions = gameConditionManager.ActiveConditions;
			for (int i = 0; i < activeConditions.Count; i++)
			{
				if (!GameConditionDefOf.HateChantDrone.CanCoexistWith(activeConditions[i].def))
				{
					return;
				}
			}
			GameCondition_HateChantDrone gameCondition_HateChantDrone = (GameCondition_HateChantDrone)GameConditionMaker.MakeCondition(GameConditionDefOf.HateChantDrone, -1);
			gameConditionManager.RegisterCondition(gameCondition_HateChantDrone);
			this.Data.condition = gameCondition_HateChantDrone;
		}

		// Token: 0x040034FB RID: 13563
		private readonly int DroneBoostInterval = 15000;
	}
}
