using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CC2 RID: 7362
	public class Dialog_NodeTreeWithFactionInfo : Dialog_NodeTree
	{
		// Token: 0x0600BD07 RID: 48391 RVA: 0x0043A5A1 File Offset: 0x004387A1
		public Dialog_NodeTreeWithFactionInfo(DiaNode nodeRoot, Faction faction, bool delayInteractivity = false, bool radioMode = false, string title = null) : base(nodeRoot, delayInteractivity, radioMode, title)
		{
			this.faction = faction;
			if (faction != null)
			{
				this.minOptionsAreaHeight = 60f;
			}
		}

		// Token: 0x0600BD08 RID: 48392 RVA: 0x0043A5C4 File Offset: 0x004387C4
		public override void DoWindowContents(Rect inRect)
		{
			base.DoWindowContents(inRect);
			if (this.faction != null && !this.faction.Hidden)
			{
				float num = inRect.height - 79f;
				FactionUIUtility.DrawRelatedFactionInfo(inRect, this.faction, ref num);
			}
		}

		// Token: 0x04006C09 RID: 27657
		private Faction faction;

		// Token: 0x04006C0A RID: 27658
		private const float RelatedFactionInfoSize = 79f;
	}
}
