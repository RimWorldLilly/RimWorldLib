using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CE1 RID: 3297
	public class LordToil_HiveRelatedData : LordToilData
	{
		// Token: 0x06005D39 RID: 23865 RVA: 0x0022320C File Offset: 0x0022140C
		public override void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.assignedHives.RemoveAll((KeyValuePair<Pawn, Hive> x) => x.Key.Destroyed);
			}
			Scribe_Collections.Look<Pawn, Hive>(ref this.assignedHives, "assignedHives", LookMode.Reference, LookMode.Reference);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.assignedHives.RemoveAll((KeyValuePair<Pawn, Hive> x) => x.Value == null);
			}
		}

		// Token: 0x04003506 RID: 13574
		public Dictionary<Pawn, Hive> assignedHives = new Dictionary<Pawn, Hive>();
	}
}
