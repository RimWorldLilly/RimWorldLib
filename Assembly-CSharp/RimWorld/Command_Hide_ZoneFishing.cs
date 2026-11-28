using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C71 RID: 7281
	[StaticConstructorOnStartup]
	public class Command_Hide_ZoneFishing : Command_Hide
	{
		// Token: 0x0600B9E7 RID: 47591 RVA: 0x004193CF File Offset: 0x004175CF
		public Command_Hide_ZoneFishing(IHideable hideable) : base(hideable)
		{
		}

		// Token: 0x0600B9E8 RID: 47592 RVA: 0x004193D8 File Offset: 0x004175D8
		protected override IEnumerable<FloatMenuOption> GetOptions()
		{
			return Command_Hide_ZoneFishing.GetHideOptions();
		}

		// Token: 0x0600B9E9 RID: 47593 RVA: 0x004193DF File Offset: 0x004175DF
		public static IEnumerable<FloatMenuOption> GetHideOptions()
		{
			return new Command_Hide_ZoneFishing.<GetHideOptions>d__4(-2);
		}

		// Token: 0x040069DB RID: 27099
		private const string IconTexPath = "UI/Designators/ZoneCreate_Fishing";

		// Token: 0x040069DC RID: 27100
		private static Texture2D cachedIcon;
	}
}
