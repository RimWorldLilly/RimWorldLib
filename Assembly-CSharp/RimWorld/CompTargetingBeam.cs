using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019AC RID: 6572
	[StaticConstructorOnStartup]
	public class CompTargetingBeam : ThingComp
	{
		// Token: 0x17001CB5 RID: 7349
		// (get) Token: 0x0600A4FC RID: 42236 RVA: 0x003AE9AE File Offset: 0x003ACBAE
		private CompProperties_TargetingBeam Props
		{
			get
			{
				return (CompProperties_TargetingBeam)this.props;
			}
		}

		// Token: 0x0600A4FD RID: 42237 RVA: 0x003AE9BC File Offset: 0x003ACBBC
		public override void PostDraw()
		{
			base.PostDraw();
			if (this.parent != null)
			{
				Pawn pawn = this.parent as Pawn;
				if (pawn != null)
				{
					if (pawn.stances != null)
					{
						Stance_Warmup stance_Warmup = pawn.stances.curStance as Stance_Warmup;
						if (stance_Warmup != null && stance_Warmup.verb != null && stance_Warmup.verb is Verb_Shoot)
						{
							GenDraw.DrawLineBetween(this.parent.TrueCenter(), stance_Warmup.verb.CurrentTarget.CenterVector3, CompTargetingBeam.LaserLineMat, 0.1f);
						}
					}
					return;
				}
			}
		}

		// Token: 0x0400617C RID: 24956
		private static Material LaserLineMat = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, new Color(1f, 0.5f, 0.5f));

		// Token: 0x0400617D RID: 24957
		private const float LineWidth = 0.1f;
	}
}
