using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020018BE RID: 6334
	public class CompHitchingSpot : ThingComp
	{
		// Token: 0x06009EC3 RID: 40643 RVA: 0x0038F95B File Offset: 0x0038DB5B
		public void AddPawn(Pawn ropedPawn)
		{
			this.ropedPawns.Add(ropedPawn);
		}

		// Token: 0x06009EC4 RID: 40644 RVA: 0x0038F969 File Offset: 0x0038DB69
		public void RemovePawn(Pawn unropedPawn)
		{
			this.ropedPawns.Remove(unropedPawn);
		}

		// Token: 0x17001B3A RID: 6970
		// (get) Token: 0x06009EC5 RID: 40645 RVA: 0x0038F978 File Offset: 0x0038DB78
		private Graphic HitchGraphic
		{
			get
			{
				if (this.hitchGraphic == null)
				{
					this.hitchGraphic = GraphicDatabase.Get<Graphic_Single>("Things/Building/Misc/CaravanHitchingPin", ShaderDatabase.Cutout, Vector2.one, Color.white);
				}
				return this.hitchGraphic;
			}
		}

		// Token: 0x06009EC6 RID: 40646 RVA: 0x0038F9A8 File Offset: 0x0038DBA8
		public override void PostDraw()
		{
			base.PostDraw();
			if (this.ropedPawns.Count > 0)
			{
				this.HitchGraphic.DrawFromDef(this.parent.DrawPos + CompHitchingSpot.HitchAdustment + Altitudes.AltIncVect * 0.25f, Rot4.North, null, 0f);
			}
		}

		// Token: 0x06009EC7 RID: 40647 RVA: 0x0038FA08 File Offset: 0x0038DC08
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompHitchingSpot.<CompGetGizmosExtra>d__8 <CompGetGizmosExtra>d__ = new CompHitchingSpot.<CompGetGizmosExtra>d__8(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x06009EC8 RID: 40648 RVA: 0x0038FA18 File Offset: 0x0038DC18
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Collections.Look<Pawn>(ref this.ropedPawns, "ropedPawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				foreach (Pawn pawn in this.ropedPawns)
				{
					if (pawn == null || pawn.roping == null || pawn.roping.RopedToHitchingSpot != this.parent)
					{
						this.RemovePawn(pawn);
					}
				}
			}
		}

		// Token: 0x04005DEE RID: 24046
		private static readonly Vector3 HitchAdustment = new Vector3(0f, 0f, 0.15f);

		// Token: 0x04005DEF RID: 24047
		private List<Pawn> ropedPawns = new List<Pawn>();

		// Token: 0x04005DF0 RID: 24048
		private Graphic hitchGraphic;
	}
}
