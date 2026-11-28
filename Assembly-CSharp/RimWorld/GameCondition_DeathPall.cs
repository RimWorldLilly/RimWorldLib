using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200113D RID: 4413
	public class GameCondition_DeathPall : GameCondition_ForceWeather
	{
		// Token: 0x06006EDC RID: 28380 RVA: 0x0026D9CC File Offset: 0x0026BBCC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.nextResurrectTick, "nextResurrectTick", 0, false);
			Scribe_Values.Look<int>(ref this.lastResurrectMessageTick, "lastResurrectMessageTick", -9999, false);
		}

		// Token: 0x06006EDD RID: 28381 RVA: 0x0026D9FC File Offset: 0x0026BBFC
		public override void Init()
		{
			if (!ModLister.CheckAnomaly("Death pall"))
			{
				this.End();
				return;
			}
			base.Init();
			this.nextResurrectTick = Find.TickManager.TicksGame + GameCondition_DeathPall.ResurrectIntervalRange.RandomInRange;
		}

		// Token: 0x06006EDE RID: 28382 RVA: 0x0026DA40 File Offset: 0x0026BC40
		public override void GameConditionTick()
		{
			if (Find.TickManager.TicksGame < this.nextResurrectTick || Find.TickManager.TicksGame % 60 != 0)
			{
				return;
			}
			foreach (Map map in base.AffectedMaps)
			{
				foreach (Thing thing in map.listerThings.ThingsInGroup(ThingRequestGroup.Corpse))
				{
					Corpse corpse = thing as Corpse;
					if (corpse != null && MutantUtility.CanResurrectAsShambler(corpse, false) && corpse.Age >= 15000)
					{
						Pawn pawn = this.ResurrectPawn(corpse);
						if (!pawn.Position.Fogged(map))
						{
							this.TryDoMessage(pawn);
						}
						this.nextResurrectTick = Find.TickManager.TicksGame + GameCondition_DeathPall.ResurrectIntervalRange.RandomInRange;
						return;
					}
				}
			}
		}

		// Token: 0x06006EDF RID: 28383 RVA: 0x0026DB5C File Offset: 0x0026BD5C
		private Pawn ResurrectPawn(Corpse corpse)
		{
			Pawn innerPawn = corpse.InnerPawn;
			MutantUtility.ResurrectAsShambler(innerPawn, 60000, null);
			return innerPawn;
		}

		// Token: 0x06006EE0 RID: 28384 RVA: 0x0026DB70 File Offset: 0x0026BD70
		public override void End()
		{
			Find.LetterStack.ReceiveLetter("LetterLabelDeathPallEnded".Translate(), "LetterDeathPallEnded".Translate(), LetterDefOf.NeutralEvent, null, 0, true);
			base.End();
			base.SingleMap.weatherDecider.StartNextWeather();
		}

		// Token: 0x06006EE1 RID: 28385 RVA: 0x0026DBB0 File Offset: 0x0026BDB0
		private void TryDoMessage(LookTargets target)
		{
			if (Find.TickManager.TicksGame - this.lastResurrectMessageTick < 60000)
			{
				return;
			}
			Messages.Message("DeathPallResurrectedMessage".Translate(), target, MessageTypeDefOf.NegativeEvent, false);
			this.lastResurrectMessageTick = Find.TickManager.TicksGame;
		}

		// Token: 0x040046E8 RID: 18152
		private static readonly IntRange ResurrectIntervalRange = new IntRange(600, 1800);

		// Token: 0x040046E9 RID: 18153
		private const int ResurrectCheckInterval = 60;

		// Token: 0x040046EA RID: 18154
		private const int ResurrectCooldown = 15000;

		// Token: 0x040046EB RID: 18155
		private const int ResurrectMessageCooldown = 60000;

		// Token: 0x040046EC RID: 18156
		private int nextResurrectTick;

		// Token: 0x040046ED RID: 18157
		private int lastResurrectMessageTick = -9999;
	}
}
