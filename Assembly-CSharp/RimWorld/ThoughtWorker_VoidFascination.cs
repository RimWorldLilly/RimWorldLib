using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E95 RID: 3733
	public class ThoughtWorker_VoidFascination : ThoughtWorker
	{
		// Token: 0x06006276 RID: 25206 RVA: 0x00236058 File Offset: 0x00234258
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return ThoughtState.Inactive;
			}
			foreach (Thing thing in p.Map.listerThings.ThingsInGroup(ThingRequestGroup.EntityHolder))
			{
				bool flag;
				if (thing == null)
				{
					flag = (null != null);
				}
				else
				{
					CompEntityHolder compEntityHolder = thing.TryGetComp<CompEntityHolder>();
					flag = (((compEntityHolder != null) ? compEntityHolder.HeldPawn : null) != null);
				}
				if (flag)
				{
					return ThoughtState.ActiveAtStage(0);
				}
			}
			return ThoughtState.Inactive;
		}
	}
}
