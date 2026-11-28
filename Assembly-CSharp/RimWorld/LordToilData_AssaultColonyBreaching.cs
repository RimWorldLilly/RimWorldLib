using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CC3 RID: 3267
	public class LordToilData_AssaultColonyBreaching : LordToilData
	{
		// Token: 0x06005CA9 RID: 23721 RVA: 0x002204A9 File Offset: 0x0021E6A9
		public LordToilData_AssaultColonyBreaching()
		{
		}

		// Token: 0x06005CAA RID: 23722 RVA: 0x002204D2 File Offset: 0x0021E6D2
		public LordToilData_AssaultColonyBreaching(Lord lord)
		{
			this.breachingGrid = new BreachingGrid(lord.Map, lord);
		}

		// Token: 0x06005CAB RID: 23723 RVA: 0x0022050D File Offset: 0x0021E70D
		public void Reset()
		{
			this.breachDest = IntVec3.Invalid;
			this.breachStart = IntVec3.Invalid;
			this.currentTarget = null;
			this.soloAttacker = null;
			this.breachingGrid.Reset();
		}

		// Token: 0x06005CAC RID: 23724 RVA: 0x00220540 File Offset: 0x0021E740
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.breachDest, "breachDest", default(IntVec3), false);
			Scribe_Values.Look<IntVec3>(ref this.breachStart, "breachStart", default(IntVec3), false);
			Scribe_Values.Look<bool>(ref this.preferMelee, "preferMelee", false, false);
			Scribe_Deep.Look<BreachingGrid>(ref this.breachingGrid, "breachingGrid", Array.Empty<object>());
			Scribe_References.Look<Thing>(ref this.currentTarget, "currentTarget", false);
			Scribe_References.Look<Pawn>(ref this.soloAttacker, "soloAttacker", false);
			Scribe_Values.Look<float>(ref this.maxRange, "maxRange", 0f, false);
		}

		// Token: 0x040034CF RID: 13519
		public IntVec3 breachDest = IntVec3.Invalid;

		// Token: 0x040034D0 RID: 13520
		public IntVec3 breachStart = IntVec3.Invalid;

		// Token: 0x040034D1 RID: 13521
		public bool preferMelee;

		// Token: 0x040034D2 RID: 13522
		public BreachingGrid breachingGrid;

		// Token: 0x040034D3 RID: 13523
		public Thing currentTarget;

		// Token: 0x040034D4 RID: 13524
		public Pawn soloAttacker;

		// Token: 0x040034D5 RID: 13525
		public float maxRange = 12f;
	}
}
