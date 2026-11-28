using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001124 RID: 4388
	public interface IThingStudied
	{
		// Token: 0x06006DEE RID: 28142
		void OnStudied(Pawn studier, float amount, KnowledgeCategoryDef category = null);
	}
}
