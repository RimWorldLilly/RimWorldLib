using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D30 RID: 3376
	public static class ShipJobMaker
	{
		// Token: 0x06005EB3 RID: 24243 RVA: 0x00229F9D File Offset: 0x0022819D
		public static ShipJob MakeShipJob(ShipJobDef def)
		{
			ShipJob shipJob = (ShipJob)Activator.CreateInstance(def.jobClass);
			shipJob.def = def;
			shipJob.loadID = Find.UniqueIDsManager.GetNextShipJobID();
			return shipJob;
		}
	}
}
