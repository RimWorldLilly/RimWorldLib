using System;

namespace Verse
{
	// Token: 0x0200049B RID: 1179
	public static class HediffMaker
	{
		// Token: 0x06002098 RID: 8344 RVA: 0x000C336C File Offset: 0x000C156C
		public static Hediff MakeHediff(HediffDef def, Pawn pawn, BodyPartRecord partRecord = null)
		{
			if (pawn == null)
			{
				Log.Error("Cannot make hediff " + ((def != null) ? def.ToString() : null) + " for null pawn.");
				return null;
			}
			Hediff hediff = (Hediff)Activator.CreateInstance(def.hediffClass);
			hediff.def = def;
			hediff.pawn = pawn;
			hediff.Part = partRecord;
			hediff.loadID = Find.UniqueIDsManager.GetNextHediffID();
			hediff.PostMake();
			return hediff;
		}

		// Token: 0x06002099 RID: 8345 RVA: 0x000C33DA File Offset: 0x000C15DA
		public static Hediff Debug_MakeConcreteExampleHediff(HediffDef def)
		{
			Hediff hediff = (Hediff)Activator.CreateInstance(def.hediffClass);
			hediff.def = def;
			hediff.loadID = Find.UniqueIDsManager.GetNextHediffID();
			hediff.PostMake();
			return hediff;
		}
	}
}
