using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001663 RID: 5731
	public class StageEndTrigger_RoleTagSet : StageEndTrigger
	{
		// Token: 0x06009006 RID: 36870 RVA: 0x0034EB65 File Offset: 0x0034CD65
		public override Trigger MakeTrigger(LordJob_Ritual ritual, TargetInfo spot, IEnumerable<TargetInfo> foci, RitualStage stage)
		{
			return new Trigger_TickCondition(delegate()
			{
				foreach (string r in this.roleIds)
				{
					if (!this.ArrivedCheck(r, ritual))
					{
						return false;
					}
				}
				if (this.clearTag)
				{
					foreach (string b in this.roleIds)
					{
						foreach (KeyValuePair<Pawn, PawnTags> keyValuePair in ritual.perPawnTags)
						{
							RitualRole ritualRole = ritual.RoleFor(keyValuePair.Key, true);
							if (ritualRole != null && ritualRole.id == b && keyValuePair.Value.Contains(this.tag))
							{
								keyValuePair.Value.tags.Remove(this.tag);
							}
						}
					}
				}
				return true;
			}, 1);
		}

		// Token: 0x06009007 RID: 36871 RVA: 0x0034EB8C File Offset: 0x0034CD8C
		protected virtual bool ArrivedCheck(string r, LordJob_Ritual ritual)
		{
			if (!ritual.RoleFilled(r))
			{
				return true;
			}
			foreach (KeyValuePair<Pawn, PawnTags> keyValuePair in ritual.perPawnTags)
			{
				RitualRole ritualRole = ritual.RoleFor(keyValuePair.Key, true);
				if (ritualRole != null && ritualRole.id == r && keyValuePair.Value.Contains(this.tag))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009008 RID: 36872 RVA: 0x0034EC20 File Offset: 0x0034CE20
		public override void ExposeData()
		{
			Scribe_Collections.Look<string>(ref this.roleIds, "roleIds", LookMode.Undefined, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.clearTag, "clearTag", false, false);
			Scribe_Values.Look<string>(ref this.tag, "tag", null, false);
		}

		// Token: 0x040057EE RID: 22510
		public List<string> roleIds;

		// Token: 0x040057EF RID: 22511
		public bool clearTag;

		// Token: 0x040057F0 RID: 22512
		[NoTranslate]
		public string tag;
	}
}
