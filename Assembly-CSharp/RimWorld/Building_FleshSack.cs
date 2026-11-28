using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AAF RID: 6831
	public class Building_FleshSack : Building_Casket
	{
		// Token: 0x17001EE3 RID: 7907
		// (get) Token: 0x0600ACF1 RID: 44273 RVA: 0x003CF3F8 File Offset: 0x003CD5F8
		public override bool CanOpen
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600ACF2 RID: 44274 RVA: 0x003CF3FC File Offset: 0x003CD5FC
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (base.Spawned && this.innerContainer.Count > 0 && mode != DestroyMode.Vanish)
			{
				Messages.Message("FleshmassContainedMessage".Translate() + ": " + this.innerContainer.ContentsString.CapitalizeFirst(), new LookTargets(base.Position, base.Map), MessageTypeDefOf.NegativeEvent, true);
			}
			base.Destroy(mode);
		}
	}
}
