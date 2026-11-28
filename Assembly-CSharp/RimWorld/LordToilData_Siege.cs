using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CF5 RID: 3317
	public class LordToilData_Siege : LordToilData
	{
		// Token: 0x06005DA1 RID: 23969 RVA: 0x00224EFC File Offset: 0x002230FC
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.siegeCenter, "siegeCenter", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.baseRadius, "baseRadius", 16f, false);
			Scribe_Values.Look<float>(ref this.blueprintPoints, "blueprintPoints", 0f, false);
			Scribe_Values.Look<float>(ref this.desiredBuilderFraction, "desiredBuilderFraction", 0.5f, false);
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.blueprints.RemoveAll((Blueprint blue) => blue.Destroyed);
			}
			Scribe_Collections.Look<Blueprint>(ref this.blueprints, "blueprints", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x04003525 RID: 13605
		public IntVec3 siegeCenter;

		// Token: 0x04003526 RID: 13606
		public float baseRadius = 16f;

		// Token: 0x04003527 RID: 13607
		public float blueprintPoints;

		// Token: 0x04003528 RID: 13608
		public float desiredBuilderFraction = 0.5f;

		// Token: 0x04003529 RID: 13609
		public List<Blueprint> blueprints = new List<Blueprint>();
	}
}
