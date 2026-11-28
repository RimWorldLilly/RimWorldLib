using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010E2 RID: 4322
	public abstract class QuestPart_RequirementsToAccept : QuestPart
	{
		// Token: 0x17001275 RID: 4725
		// (get) Token: 0x06006C07 RID: 27655 RVA: 0x0025F074 File Offset: 0x0025D274
		public virtual IEnumerable<GlobalTargetInfo> Culprits
		{
			get
			{
				return Enumerable.Empty<GlobalTargetInfo>();
			}
		}

		// Token: 0x06006C08 RID: 27656
		public abstract AcceptanceReport CanAccept();

		// Token: 0x17001276 RID: 4726
		// (get) Token: 0x06006C09 RID: 27657 RVA: 0x0025F07B File Offset: 0x0025D27B
		public virtual bool ShowInRequirementBox
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006C0A RID: 27658 RVA: 0x0025F07E File Offset: 0x0025D27E
		public virtual bool CanPawnAccept(Pawn p)
		{
			return true;
		}
	}
}
