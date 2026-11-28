using System;

namespace RimWorld
{
	// Token: 0x02001ED1 RID: 7889
	[DefOf]
	public static class InstructionDefOf
	{
		// Token: 0x0600CA8C RID: 51852 RVA: 0x004A620E File Offset: 0x004A440E
		static InstructionDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(InstructionDefOf));
		}

		// Token: 0x04007F78 RID: 32632
		public static InstructionDef RandomizeCharacter;

		// Token: 0x04007F79 RID: 32633
		public static InstructionDef ChooseLandingSite;
	}
}
