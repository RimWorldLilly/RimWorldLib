using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E87 RID: 3719
	public class ThoughtWorker_PyromaniacNearFlames : ThoughtWorker
	{
		// Token: 0x06006251 RID: 25169 RVA: 0x00235788 File Offset: 0x00233988
		public override string PostProcessLabel(Pawn p, string label)
		{
			int num = this.NumFiresNear(p);
			if (num == 1)
			{
				return base.PostProcessLabel(p, label);
			}
			return base.PostProcessLabel(p, label) + " x" + num.ToString();
		}

		// Token: 0x06006252 RID: 25170 RVA: 0x002357C3 File Offset: 0x002339C3
		public override float MoodMultiplier(Pawn p)
		{
			return (float)this.NumFiresNear(p);
		}

		// Token: 0x06006253 RID: 25171 RVA: 0x002357CD File Offset: 0x002339CD
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return this.NumFiresNear(p) > 0;
		}

		// Token: 0x06006254 RID: 25172 RVA: 0x002357E0 File Offset: 0x002339E0
		private int NumFiresNear(Pawn p)
		{
			int num = 0;
			Room room = p.Position.GetRoom(p.Map);
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(p.Position, 8f, true))
			{
				if (intVec.InBounds(p.Map) && !intVec.Fogged(p.Map))
				{
					Room room2 = intVec.GetRoom(p.Map);
					if (room2 != null && room == room2)
					{
						num += FireUtility.NumFiresAt(intVec, p.Map);
						if (num >= this.def.stackLimit)
						{
							break;
						}
					}
				}
			}
			return Mathf.Min(num, this.def.stackLimit);
		}

		// Token: 0x04003624 RID: 13860
		private const float MaxDist = 8f;
	}
}
