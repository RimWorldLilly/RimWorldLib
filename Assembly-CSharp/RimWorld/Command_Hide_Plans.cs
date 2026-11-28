using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C70 RID: 7280
	[StaticConstructorOnStartup]
	public class Command_Hide_Plans : Command_Hide
	{
		// Token: 0x0600B9E3 RID: 47587 RVA: 0x004193A4 File Offset: 0x004175A4
		public Command_Hide_Plans(IHideable hideable) : base(hideable)
		{
		}

		// Token: 0x0600B9E4 RID: 47588 RVA: 0x004193AD File Offset: 0x004175AD
		protected override IEnumerable<FloatMenuOption> GetOptions()
		{
			return Command_Hide_Plans.GetHideOptions();
		}

		// Token: 0x0600B9E5 RID: 47589 RVA: 0x004193B4 File Offset: 0x004175B4
		public static IEnumerable<FloatMenuOption> GetHideOptions()
		{
			return new Command_Hide_Plans.<GetHideOptions>d__3(-2);
		}

		// Token: 0x040069DA RID: 27098
		private static readonly Texture2D PlanTex = ContentFinder<Texture2D>.Get("UI/Designators/PlanOn", true);
	}
}
