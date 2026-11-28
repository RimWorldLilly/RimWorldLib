using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200166A RID: 5738
	public class StageEndTrigger_AnyPawnDead : StageEndTrigger
	{
		// Token: 0x06009017 RID: 36887 RVA: 0x0034EF14 File Offset: 0x0034D114
		protected virtual bool Trigger(LordJob_Ritual ritual)
		{
			foreach (string roleId in this.roleIds)
			{
				using (IEnumerator<Pawn> enumerator2 = ritual.assignments.AssignedPawns(roleId).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.Dead)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06009018 RID: 36888 RVA: 0x0034EFA8 File Offset: 0x0034D1A8
		public override Trigger MakeTrigger(LordJob_Ritual ritual, TargetInfo spot, IEnumerable<TargetInfo> foci, RitualStage stage)
		{
			return new Trigger_Custom((TriggerSignal signal) => this.Trigger(ritual));
		}

		// Token: 0x06009019 RID: 36889 RVA: 0x0034EFCD File Offset: 0x0034D1CD
		public override void ExposeData()
		{
			Scribe_Collections.Look<string>(ref this.roleIds, "roleIds", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x040057F4 RID: 22516
		[NoTranslate]
		public List<string> roleIds;
	}
}
