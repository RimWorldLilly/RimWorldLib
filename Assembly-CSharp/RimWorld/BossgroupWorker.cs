using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000F47 RID: 3911
	public class BossgroupWorker
	{
		// Token: 0x060064D0 RID: 25808 RVA: 0x0023FCB4 File Offset: 0x0023DEB4
		public virtual AcceptanceReport CanResolve(Pawn caller)
		{
			int lastBossgroupCalled = Find.BossgroupManager.lastBossgroupCalled;
			int num = Find.TickManager.TicksGame - lastBossgroupCalled;
			if (num < 120000)
			{
				return "BossgroupAvailableIn".Translate((120000 - num).ToStringTicksToPeriod(true, false, true, true, false));
			}
			PawnKindDef pendingBossgroup = CallBossgroupUtility.GetPendingBossgroup();
			if (pendingBossgroup != null)
			{
				return "BossgroupIncoming".Translate(pendingBossgroup.label);
			}
			return true;
		}

		// Token: 0x060064D1 RID: 25809 RVA: 0x0023FD34 File Offset: 0x0023DF34
		public virtual AcceptanceReport ShouldSummonNow(Map map)
		{
			List<Lord> lords = map.lordManager.lords;
			for (int i = 0; i < lords.Count; i++)
			{
				LordJob lordJob = lords[i].LordJob;
				if (lordJob is LordJob_TradeWithColony || lordJob is LordJob_BestowingCeremony || lordJob is LordJob_VisitColony)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060064D2 RID: 25810 RVA: 0x0023FD90 File Offset: 0x0023DF90
		public virtual void Resolve(Map map, int wave)
		{
			Find.BossgroupManager.Notify_BossgroupCalled(this.def);
			Slate slate = new Slate();
			slate.Set<BossgroupDef>("bossgroup", this.def, false);
			slate.Set<Map>("map", map, false);
			slate.Set<ThingDef>("reward", this.def.rewardDef, false);
			slate.Set<int>("wave", wave, false);
			QuestUtility.GenerateQuestAndMakeAvailable(this.def.quest, slate);
		}

		// Token: 0x04003A5E RID: 14942
		public const int TimeBetweenAllBossgroups = 120000;

		// Token: 0x04003A5F RID: 14943
		public BossgroupDef def;
	}
}
