using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200058D RID: 1421
	public class MoteDualAttached_Cards : MoteDualAttached
	{
		// Token: 0x060029F5 RID: 10741 RVA: 0x00100738 File Offset: 0x000FE938
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			if (this.link1.Linked && this.link1.Target.HasThing)
			{
				Pawn pawn = this.link2.Target.Thing as Pawn;
				if (pawn != null && pawn.Rotation == Rot4.North)
				{
					return;
				}
			}
			if (this.link2.Linked && this.link2.Target.HasThing)
			{
				Pawn pawn2 = this.link2.Target.Thing as Pawn;
				if (pawn2 != null && pawn2.Rotation == Rot4.North)
				{
					return;
				}
			}
			base.DrawAt(drawLoc, flip);
		}
	}
}
