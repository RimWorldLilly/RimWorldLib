using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001669 RID: 5737
	public class StageEndTrigger_RolesUnarmed : StageEndTrigger
	{
		// Token: 0x06009014 RID: 36884 RVA: 0x0034EECF File Offset: 0x0034D0CF
		public override Trigger MakeTrigger(LordJob_Ritual ritual, TargetInfo spot, IEnumerable<TargetInfo> foci, RitualStage stage)
		{
			return new Trigger_Custom(delegate(TriggerSignal signal)
			{
				foreach (string roleId in this.roleIds)
				{
					using (List<ThingWithComps>.Enumerator enumerator2 = ritual.PawnWithRole(roleId).equipment.AllEquipmentListForReading.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.def.IsWeapon)
							{
								return false;
							}
						}
					}
				}
				return true;
			});
		}

		// Token: 0x06009015 RID: 36885 RVA: 0x0034EEF4 File Offset: 0x0034D0F4
		public override void ExposeData()
		{
			Scribe_Collections.Look<string>(ref this.roleIds, "roleIds", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x040057F3 RID: 22515
		[NoTranslate]
		public List<string> roleIds;
	}
}
