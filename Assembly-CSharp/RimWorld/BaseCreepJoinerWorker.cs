using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001484 RID: 5252
	public abstract class BaseCreepJoinerWorker
	{
		// Token: 0x17001617 RID: 5655
		// (get) Token: 0x060080E7 RID: 32999 RVA: 0x002DFE7A File Offset: 0x002DE07A
		// (set) Token: 0x060080E8 RID: 33000 RVA: 0x002DFE82 File Offset: 0x002DE082
		public Pawn_CreepJoinerTracker Tracker { get; set; }

		// Token: 0x17001618 RID: 5656
		// (get) Token: 0x060080E9 RID: 33001 RVA: 0x002DFE8B File Offset: 0x002DE08B
		public virtual bool CanOccurOnDeath
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001619 RID: 5657
		// (get) Token: 0x060080EA RID: 33002 RVA: 0x002DFE8E File Offset: 0x002DE08E
		public Pawn Pawn
		{
			get
			{
				return this.Tracker.Pawn;
			}
		}

		// Token: 0x060080EB RID: 33003 RVA: 0x002DFE9B File Offset: 0x002DE09B
		public virtual void OnCreated()
		{
		}

		// Token: 0x060080EC RID: 33004 RVA: 0x002DFE9D File Offset: 0x002DE09D
		public virtual bool CanDoResponse()
		{
			return true;
		}

		// Token: 0x060080ED RID: 33005
		public abstract void DoResponse(List<TargetInfo> looktargets, List<NamedArgument> namedArgs);
	}
}
