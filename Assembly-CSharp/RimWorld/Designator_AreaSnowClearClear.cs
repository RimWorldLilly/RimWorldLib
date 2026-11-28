using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C3F RID: 7231
	public class Designator_AreaSnowClearClear : Designator_AreaSnowClear
	{
		// Token: 0x0600B851 RID: 47185 RVA: 0x004107FC File Offset: 0x0040E9FC
		public Designator_AreaSnowClearClear() : base(DesignateMode.Remove)
		{
			string key = "DesignatorAreaSnowClearClear";
			string key2 = "DesignatorAreaSnowClearClearDesc";
			if (ModsConfig.OdysseyActive)
			{
				key = "DesignatorAreaSnowOrSandClearClear";
				key2 = "DesignatorAreaSnowOrSandClearClearDesc";
			}
			this.defaultLabel = key.Translate();
			this.defaultDesc = key2.Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/SnowClearAreaOff", true);
			this.soundDragSustain = SoundDefOf.Designate_DragAreaDelete;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_ZoneDelete;
		}
	}
}
