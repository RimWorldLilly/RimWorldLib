using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C8B RID: 3211
	public class LordJob_HiveQueen : LordJob_StructureThreatCluster
	{
		// Token: 0x06005AFA RID: 23290 RVA: 0x00217483 File Offset: 0x00215683
		public LordJob_HiveQueen()
		{
		}

		// Token: 0x06005AFB RID: 23291 RVA: 0x0021748B File Offset: 0x0021568B
		public LordJob_HiveQueen(Faction faction, IntVec3 position, float wanderRadius, bool sendWokenUpMessage = true, bool awakeOnClamor = false) : base(faction, position, wanderRadius, sendWokenUpMessage, awakeOnClamor)
		{
		}

		// Token: 0x06005AFC RID: 23292 RVA: 0x0021749A File Offset: 0x0021569A
		protected override LordToil GetIdleToil()
		{
			return new LordToil_WanderNest();
		}
	}
}
