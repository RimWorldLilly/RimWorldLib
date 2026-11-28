using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E5D RID: 3677
	public class ThoughtWorker_WearingColor_Favorite : ThoughtWorker_WearingColor
	{
		// Token: 0x060061CE RID: 25038 RVA: 0x00234064 File Offset: 0x00232264
		protected override Color? Color(Pawn p)
		{
			if (p.DevelopmentalStage.Baby())
			{
				return null;
			}
			ColorDef favoriteColor = p.story.favoriteColor;
			if (favoriteColor == null)
			{
				return null;
			}
			return new Color?(favoriteColor.color);
		}
	}
}
