using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F80 RID: 3968
	public class JoyGiverDef : Def
	{
		// Token: 0x17001160 RID: 4448
		// (get) Token: 0x06006593 RID: 26003 RVA: 0x00242D7D File Offset: 0x00240F7D
		public JoyGiver Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (JoyGiver)Activator.CreateInstance(this.giverClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x06006594 RID: 26004 RVA: 0x00242DAF File Offset: 0x00240FAF
		public override IEnumerable<string> ConfigErrors()
		{
			JoyGiverDef.<ConfigErrors>d__16 <ConfigErrors>d__ = new JoyGiverDef.<ConfigErrors>d__16(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04003C40 RID: 15424
		public Type giverClass;

		// Token: 0x04003C41 RID: 15425
		public float baseChance;

		// Token: 0x04003C42 RID: 15426
		public bool requireChair = true;

		// Token: 0x04003C43 RID: 15427
		public List<ThingDef> thingDefs;

		// Token: 0x04003C44 RID: 15428
		public JobDef jobDef;

		// Token: 0x04003C45 RID: 15429
		public bool desireSit = true;

		// Token: 0x04003C46 RID: 15430
		public float pctPawnsEverDo = 1f;

		// Token: 0x04003C47 RID: 15431
		public bool unroofedOnly;

		// Token: 0x04003C48 RID: 15432
		public JoyKindDef joyKind;

		// Token: 0x04003C49 RID: 15433
		public List<PawnCapacityDef> requiredCapacities = new List<PawnCapacityDef>();

		// Token: 0x04003C4A RID: 15434
		public bool canDoWhileInBed;

		// Token: 0x04003C4B RID: 15435
		public bool requiresEnjoyOutdoors;

		// Token: 0x04003C4C RID: 15436
		public bool countsForRecRoom = true;

		// Token: 0x04003C4D RID: 15437
		private JoyGiver workerInt;
	}
}
